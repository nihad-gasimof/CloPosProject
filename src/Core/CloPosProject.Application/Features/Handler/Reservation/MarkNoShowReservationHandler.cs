using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Reservation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class MarkNoShowReservationHandler : IRequestHandler<MarkNoShowReservationCommand, SimpleResponse<string>>
    {
        private readonly IReservationService _service;
        public MarkNoShowReservationHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(MarkNoShowReservationCommand request, CancellationToken cancellationToken)
            => await _service.MarkAsNoShowAsync(request.Id);
    }
}
