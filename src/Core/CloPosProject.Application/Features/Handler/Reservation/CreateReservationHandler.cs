using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Reservation;
using CloPosProject.Application.DTOs.Reservation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, SimpleResponse<System.Guid>>
    {
        private readonly IReservationService _service;
        public CreateReservationHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<System.Guid>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
            => await _service.CreateReservationAsync(
                request.Request.TableId,
                request.Request.CustomerName,
                request.Request.CustomerPhone,
                request.Request.CustomerEmail,
                request.Request.GuestCount,
                request.Request.ReservationDate,
                request.Request.ReservationTime,
                request.Request.DurationMinutes,
                request.Request.SpecialRequests);
    }
}
