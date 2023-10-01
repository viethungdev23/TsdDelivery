using TsdDelivery.Application.Models;
using TsdDelivery.Application.Models.Coordinates;
using TsdDelivery.Application.Models.Reservation.Request;
using TsdDelivery.Application.Models.Reservation.Response;

namespace TsdDelivery.Application.Interface;

public interface IReservationService
{
    public Task<OperationResult<CalculatePriceResponse>> CalculateTotalPrice(CalculatePriceRequest request);
    public Task<OperationResult<CreateReservationResponse>> CreateReservation(CreateReservationRequest request);
    public Task<OperationResult<List<ReservationResponse>>> GetAllReservation();
    public Task<OperationResult<List<ReservationAwaitingDriverResponse>>> GetAwaitingDriverReservation(CurrentCoordinates currentCoordinates,DestinationCoordinates destinationCoordinates);
    public Task<OperationResult<ReservationAwaitingDriverDetailResponse>> GetAwaitingDriverReservationDetail(Guid reservationId,CurrentCoordinates currentCoordinates,DestinationCoordinates destinationCoordinates);
    public Task<OperationResult<ReservationResponse>> AcceptReservation(Guid driverId, Guid reservationId);
}