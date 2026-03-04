using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using CloPosProject.Application.Features.Queries.Reservation;
using CloPosProject.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Reservation
{
    public class GetAllReservationsHandler : IRequestHandler<GetAllReservationsQuery, SimpleResponse<List<ReservationResponse>>>
    {
        private readonly IReservationService _service;
        public GetAllReservationsHandler(IReservationService service) => _service = service;
        public async Task<SimpleResponse<List<ReservationResponse>>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
            => await _service.GetAllAsync(request.Date, request.Status);
    }
}
