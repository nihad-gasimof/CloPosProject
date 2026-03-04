using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using MediatR;

namespace CloPosProject.Application.Features.Commands.Reservation
{
    public record CreateReservationCommand(CreateReservationRequest Request) : IRequest<SimpleResponse<System.Guid>>;
}
