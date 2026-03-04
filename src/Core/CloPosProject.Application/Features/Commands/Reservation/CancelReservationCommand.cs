using CloPosProject.Application.BaseResponseModel;
using MediatR;

namespace CloPosProject.Application.Features.Commands.Reservation
{
    public record CancelReservationCommand(System.Guid Id, string Reason) : IRequest<SimpleResponse<string>>;
}
