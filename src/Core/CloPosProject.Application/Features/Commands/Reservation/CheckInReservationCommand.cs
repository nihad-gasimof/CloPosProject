using CloPosProject.Application.BaseResponseModel;
using MediatR;

namespace CloPosProject.Application.Features.Commands.Reservation
{
    public record CheckInReservationCommand(System.Guid Id) : IRequest<SimpleResponse<string>>;
}
