using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using MediatR;

namespace CloPosProject.Application.Features.Queries.Reservation
{
    public record GetReservationByIdQuery(System.Guid Id) : IRequest<SimpleResponse<ReservationResponse>>;
}
