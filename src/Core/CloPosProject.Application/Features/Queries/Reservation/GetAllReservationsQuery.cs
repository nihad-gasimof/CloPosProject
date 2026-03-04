using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using CloPosProject.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.Reservation
{
    public record GetAllReservationsQuery(DateTime? Date, ReservationStatus? Status) : IRequest<SimpleResponse<List<ReservationResponse>>>;
}
