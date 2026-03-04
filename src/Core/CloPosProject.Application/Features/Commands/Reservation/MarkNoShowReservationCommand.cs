using CloPosProject.Application.BaseResponseModel;
using MediatR;

namespace CloPosProject.Application.Features.Commands.Reservation
{
    public record MarkNoShowReservationCommand(System.Guid Id) : IRequest<SimpleResponse<string>>;
}
