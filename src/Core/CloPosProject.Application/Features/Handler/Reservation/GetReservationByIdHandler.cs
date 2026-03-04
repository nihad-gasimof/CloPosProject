using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using CloPosProject.Application.Features.Queries.Reservation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, SimpleResponse<ReservationResponse>>
    {
        private readonly IReservationService _service;
        public GetReservationByIdHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<ReservationResponse>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
            => await _service.GetByIdAsync(request.Id);
    }
}
