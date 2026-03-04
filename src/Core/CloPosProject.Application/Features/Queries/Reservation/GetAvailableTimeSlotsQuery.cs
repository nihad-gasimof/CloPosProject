using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using MediatR;
using System;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.Reservation
{
    public record GetAvailableTimeSlotsQuery(Guid TableId, DateTime Date, int DurationMinutes) : IRequest<SimpleResponse<List<AvailableTimeSlot>>>;
}
