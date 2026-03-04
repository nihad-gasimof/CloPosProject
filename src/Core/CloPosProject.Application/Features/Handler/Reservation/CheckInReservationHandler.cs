using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Reservation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class CheckInReservationHandler : IRequestHandler<CheckInReservationCommand, SimpleResponse<string>>
    {
        private readonly IReservationService _service;
        public CheckInReservationHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(CheckInReservationCommand request, CancellationToken cancellationToken)
            => await _service.CheckInReservationAsync(request.Id);
    }
}
