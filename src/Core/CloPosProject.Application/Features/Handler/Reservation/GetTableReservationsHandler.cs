using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using CloPosProject.Application.Features.Queries.Reservation;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class GetTableReservationsHandler : IRequestHandler<GetTableReservationsQuery, SimpleResponse<List<ReservationResponse>>>
    {
        private readonly IReservationService _service;
        public GetTableReservationsHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<List<ReservationResponse>>> Handle(GetTableReservationsQuery request, CancellationToken cancellationToken)
            => await _service.GetTableReservationsAsync(request.TableId, request.Date);
    }
}
