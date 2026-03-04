using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Reservation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class CancelReservationHandler : IRequestHandler<CancelReservationCommand, SimpleResponse<string>>
    {
        private readonly IReservationService _service;
        public CancelReservationHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
            => await _service.CancelReservationAsync(request.Id, request.Reason);
    }
}
