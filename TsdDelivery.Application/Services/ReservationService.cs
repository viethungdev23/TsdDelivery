using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TsdDelivery.Application.Commons;
using TsdDelivery.Application.Interface;
using TsdDelivery.Application.Models;
using TsdDelivery.Application.Models.Reservation.DTO;
using TsdDelivery.Application.Models.Reservation.Request;
using TsdDelivery.Application.Models.Reservation.Response;
using TsdDelivery.Application.Services.Momo.Request;
using TsdDelivery.Domain.Entities;
using TsdDelivery.Domain.Entities.Enums;

namespace TsdDelivery.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly AppConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapService _mapService;
    private readonly IClaimsService _claimsService;

    public ReservationService(IUnitOfWork unitOfWork, IMapper mapper, AppConfiguration appConfiguration,
        ICurrentTime currentTime, IClaimsService claimsService, IMapService mapService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = appConfiguration;
        _currentTime = currentTime;
        _claimsService = claimsService;
        _mapService = mapService;
    }

    public async Task<OperationResult<CalculatePriceResponse>> CalculateTotalPrice(CalculatePriceRequest request)
    {
        // here implement logic 
        decimal totalAmount = 0;
        var result = new OperationResult<CalculatePriceResponse>();
        try
        {
            foreach (var id in request.ServiceIds)
            {
                var service = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
                var shippingRates = await _unitOfWork.ShippingRateRepository.GetMulti(s => s.ServiceId == id);
                totalAmount += service.Price + CalculateShippingRateByKm(request.Distance, shippingRates);
            }

            result.Payload = new CalculatePriceResponse() { Distance = request.Distance, TotalAmount = totalAmount };
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        return result;
    }

    public async Task<OperationResult<CreateReservationResponse>> CreateReservation(CreateReservationRequest request)
    {
        var result = new OperationResult<CreateReservationResponse>();
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                if (request.IsNow == false && request.PickUpDateTime < DateTime.UtcNow.AddHours(7))
                {
                    throw new Exception($"pickUpDateTime can not be less than now");
                }
                var goods = new Goods()
                {
                    Height = request.GoodsDto.Height,
                    Length = request.GoodsDto.Length,
                    Name = request.GoodsDto.Name,
                    Weight = request.GoodsDto.Weight,
                    Width = request.GoodsDto.Width
                };
                var reservation = new Reservation()
                {
                    Distance = request.Distance,
                    RecipientName = request.RecipientName,
                    RecipientPhone = request.RecipientPhone,
                    ReciveLocation = request.ReceiveLocation,
                    SendLocation = request.SendLocation,
                    IsNow = request.IsNow,
                    PickUpDateTime = request.IsNow == true ? DateTime.UtcNow.AddHours(7) : request.PickUpDateTime!.Value,
                    Goods = goods,
                    TotallPrice = request.TotalPrice,
                    UserId = _claimsService.GetCurrentUserId,
                    ReservationStatus = ReservationStatus.AwaitingPayment,
                    latitudeSendLocation = request.latitudeSendLocation,
                    longitudeSendLocation = request.longitudeSendLocation
                };
                var entity = await _unitOfWork.ReservationRepository.AddAsync(reservation);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (!isSuccess) throw new NotImplementedException();

                foreach (var serviceId in request.ServiceIds)
                {
                    var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId);
                    await _unitOfWork.ReservationDetailRepository.AddAsync(new ReservationDetail
                        { Reservation = entity, Service = service });
                    await _unitOfWork.SaveChangeAsync();
                }

                // thưc hiện chức năng thanh toán ở đây sau khi tạo xong đơn 
                // đang để defaut phương thức thanh toán là MOMO
                var paymentMethod = "Momo";
                var paymentUrl = string.Empty;
                var momoOneTimePayRequest = new MomoOneTimePaymentRequest(_configuration.MomoConfig.PartnerCode,
                    DateTime.Now.Ticks.ToString() + entity.Id ?? string.Empty, (long)request.TotalPrice!,
                    entity.Id!.ToString() ?? string.Empty,
                    "Thanh toán đặt xe TSD" ?? string.Empty, _configuration.MomoConfig.ReturnUrl,
                    _configuration.MomoConfig.IpnUrl, "captureWallet",
                    string.Empty);
                momoOneTimePayRequest.MakeSignature(_configuration.MomoConfig.AccessKey,
                    _configuration.MomoConfig.SecretKey);
                (bool createMomoLinkResult, string? createMessage, string? deepLink) =
                    momoOneTimePayRequest.GetLink(_configuration.MomoConfig.PaymentUrl);
                if (createMomoLinkResult)
                {
                    result.Payload = new CreateReservationResponse()
                    {
                        Id = entity.Id,
                        PaymentUrl = createMessage,
                        deeplink = deepLink
                    };
                }
                else
                {
                    throw new Exception(createMessage);
                }

                await transaction.CommitAsync();
                
                // goi Background Service Check status sau 2p
                var timeToCancel = DateTime.UtcNow.AddMinutes(5);
                string id = BackgroundJob.Schedule<IBackgroundService>(
                    x => x.AutoCancelReservationWhenOverAllowPaymentTime(entity.Id), timeToCancel);
            }
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
                await transaction.RollbackAsync();
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return result;
        }
    }

    public async Task<OperationResult<List<ReservationResponse>>> GetAllReservation()
    {
        var result = new OperationResult<List<ReservationResponse>>();
        try
        {
            var reservations = await _unitOfWork.ReservationRepository.GetAllAsync();
            var list = _mapper.Map<List<ReservationResponse>>(reservations);
            result.Payload = list;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        return result;
    }

    public async Task<OperationResult<List<ReservationAwaitingDriverResponse>>> GetAwaitingDriverReservation(Coordinates coordinates)
    {
        var result = new OperationResult<List<ReservationAwaitingDriverResponse>>();
        try
        {
            var reservations =
                await _unitOfWork.ReservationRepository.GetMulti(x =>
                    x.ReservationStatus == ReservationStatus.AwaitingDriver);
            var list = new List<ReservationAwaitingDriverResponse>();
            foreach (var x in reservations)
            {
                double? dis = null;
                bool highPriorityLevel = false;
                if (CheckHasValue(coordinates))
                {
                    dis = await GetDistanseKm(coordinates!.Latitude.Value, coordinates!.Longitude.Value, x.latitudeSendLocation,
                        x.longitudeSendLocation);
                    if (dis! <= 10)        // if distance between driver and reservation < 10km
                    {
                        highPriorityLevel = true;
                    }
                }
                var response = new ReservationAwaitingDriverResponse()
                {
                    Id = x.Id,
                    RecipientName = x.RecipientName,
                    RecipientPhone = x.RecipientPhone,
                    ReciveLocation = x.ReciveLocation,
                    SendLocation = x.SendLocation,
                    IsNow = x.IsNow,
                    PickUpDateTime = x.PickUpDateTime,
                    ReservationStatus = x.ReservationStatus.ToString(),
                    TotallPrice = x.TotallPrice,
                    Distance = x.Distance,
                    distanceFromCurrentReservationToYou = dis ?? null,
                    HighPriorityLevel = highPriorityLevel,
                    GoodsDto = new GoodsDto
                    {
                        Width = x.Goods.Width,
                        Length = x.Goods.Length,
                        Name = x.Goods.Name,
                        Weight = x.Goods.Weight,
                        Height = x.Goods.Height
                    }
                };
                list.Add(response);
            }

            result.Payload = list;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        return result;
    }

    public async Task<OperationResult<ReservationAwaitingDriverResponse>> GetAwaitingDriverReservationDetail(Guid id, Coordinates coordinates)
    {
        var result = new OperationResult<ReservationAwaitingDriverResponse>();
        try
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(id);
            var test = await _unitOfWork.ReservationRepository.GetReservationDetail(id);
            result.Payload = _mapper.Map<ReservationAwaitingDriverResponse>(reservation);
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        return result;
    }

    public async Task<OperationResult<ReservationResponse>> AcceptReservation(Guid driverId, Guid reservationId)
    {
        var result = new OperationResult<ReservationResponse>();
        try
        {
            var driver = await _unitOfWork.UserRepository.GetByIdAsync(driverId);
            var claimId = _claimsService.GetCurrentUserId;
            if (!claimId.Equals(driverId))
            {
                result.AddError(ErrorCode.ServerError,"The driverID does not match the account used to login");
                return result;
            }
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(reservationId);
            if (!reservation!.ReservationStatus.Equals(ReservationStatus.AwaitingDriver))
            {
                switch (reservation.ReservationStatus)
                {
                    case ReservationStatus.AwaitingPayment:
                        result.AddError(ErrorCode.ServerError,"đơn hàng này đang trong trạng thái chờ thanh toán");
                        break;
                    case ReservationStatus.Cancelled:
                        result.AddError(ErrorCode.ServerError,"đơn hàng này đã bị cancel");
                        break;
                    case ReservationStatus.Completed:
                        result.AddError(ErrorCode.ServerError,"đơn hàng này đã hoàn thành");
                        break;
                    default:
                        result.AddError(ErrorCode.ServerError,"đơn hàng này đã được nhận bởi tài xế khác");
                        break;
                }
                return result;
            }
            reservation.ReservationStatus = ReservationStatus.OnTheWayToPickupPoint;
            reservation.Driver = driver;
            await _unitOfWork.SaveChangeAsync();
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
        return result;
    }

    private decimal CalculateShippingRateByKm(decimal km, List<ShippingRate> list)
    {
        decimal total = 0;
        if (!list.Any()) return 0;
        foreach (var sr in list)
        {
            decimal multiplier;
            if (km > sr.KmTo)
            {
                multiplier = sr.KmTo - sr.KmFrom;
            }
            else if (km > sr.KmFrom && km < sr.KmTo)
            {
                multiplier = km - sr.KmFrom + 1;
            }
            else
            {
                multiplier = 0;
            }

            total += sr.Price * multiplier;
        }

        return total;
    }

    private async Task<double?> GetDistanseKm(double originLat, double originLon, double destLat, double destLon)
    {
        try
        {
            var distance =
                await _mapService.CaculateDistanceBetweenTwoCoordinates(originLat, originLon, destLat, destLon);
            var km = distance / 1000;
            return km;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private bool CheckHasValue(Coordinates coordinates)
    {
        if (coordinates!.Longitude.HasValue && coordinates!.Latitude.HasValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}