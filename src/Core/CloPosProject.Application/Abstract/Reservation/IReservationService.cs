using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Reservation
{
    public interface IReservationService
    {
        Task<SimpleResponse<Guid>> CreateReservationAsync(
           Guid tableId,
           string customerName,
           string customerPhone,
           string customerEmail,
           int guestCount,
           DateTime reservationDate,
           TimeSpan reservationTime,
           int durationMinutes,
           string specialRequests);

        Task<SimpleResponse<string>> ConfirmReservationAsync(Guid id);
        Task<SimpleResponse<string>> CheckInReservationAsync(Guid id);
        Task<SimpleResponse<string>> CompleteReservationAsync(Guid id);
        Task<SimpleResponse<string>> CancelReservationAsync(Guid id, string reason);
        Task<SimpleResponse<string>> MarkAsNoShowAsync(Guid id);

        Task<SimpleResponse<ReservationResponse>> GetByIdAsync(Guid id);
        Task<SimpleResponse<List<ReservationResponse>>> GetAllAsync(
            DateTime? date = null,
            ReservationStatus? status = null
            , Expression<Func<CloPosProject.Domain.Entities.Reservation, bool>> predicate = null);
        Task<SimpleResponse<List<ReservationResponse>>> GetTodayReservationsAsync();
        Task<SimpleResponse<List<ReservationResponse>>> GetUpcomingReservationsAsync();
        Task<SimpleResponse<List<ReservationResponse>>> GetTableReservationsAsync(Guid tableId, DateTime date);

        Task<SimpleResponse<List<AvailableTimeSlot>>> GetAvailableTimeSlotsAsync(
            Guid tableId,
            DateTime date,
            int durationMinutes);

        Task<SimpleResponse<bool>> IsTableAvailableAsync(
            Guid tableId,
            DateTime reservationDate,
            TimeSpan reservationTime,
            int durationMinutes);

        Task ProcessExpiredReservationsAsync();
    }
}
