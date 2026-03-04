using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Reservation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class CompleteReservationHandler : IRequestHandler<CompleteReservationCommand, SimpleResponse<string>>
    {
        private readonly IReservationService _service;
        public CompleteReservationHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(CompleteReservationCommand request, CancellationToken cancellationToken)
            => await _service.CompleteReservationAsync(request.Id);
    }
}
