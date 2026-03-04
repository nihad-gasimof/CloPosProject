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
    public class GetAvailableTimeSlotsHandler : IRequestHandler<GetAvailableTimeSlotsQuery, SimpleResponse<List<AvailableTimeSlot>>>
    {
        private readonly IReservationService _service;
        public GetAvailableTimeSlotsHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<List<AvailableTimeSlot>>> Handle(GetAvailableTimeSlotsQuery request, CancellationToken cancellationToken)
            => await _service.GetAvailableTimeSlotsAsync(request.TableId, request.Date, request.DurationMinutes);
    }
}
