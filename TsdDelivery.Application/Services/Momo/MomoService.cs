using Microsoft.VisualBasic.CompilerServices;
using TsdDelivery.Application.Commons;
using TsdDelivery.Application.Interface;
using TsdDelivery.Application.Models;
using TsdDelivery.Application.Services.Momo.Request;
using TsdDelivery.Domain.Entities.Enums;

namespace TsdDelivery.Application.Services.Momo;

public class MomoService : IMomoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppConfiguration _configuration;
    public MomoService(IUnitOfWork unitOfWork,AppConfiguration appConfiguration)
    {
        _unitOfWork = unitOfWork;
        _configuration = appConfiguration;
    }
    
    public async Task<OperationResult<string>> ProcessMomoPaymentReturn(MomoOneTimePaymentResultRequest request)
    {
        var result = new OperationResult<string>();
        try
        {
            var isValidSignature = request.IsValidSignature(_configuration.MomoConfig.AccessKey, _configuration.MomoConfig.SecretKey);
            if (isValidSignature)
            {
                var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(Guid.Parse(request.orderId));
                // to do
                if (request.resultCode == 0)
                {
                    reservation.ReservationStatus = ReservationStatus.AwaitingDriver;
                    await _unitOfWork.ReservationRepository.Update(reservation);
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (!isSuccess)
                    {
                        result.AddError(ErrorCode.ServerError,"Fail to update reservation status");
                    }
                    result.Payload = "Thanh toan thanh cong";
                }
                else
                {
                    result.Payload = "Payment process failed";
                }
            }
            else
            {
                result.Payload = "Invalid signature in response";
            }
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
}