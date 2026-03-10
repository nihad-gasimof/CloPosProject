using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using CloPosProject.Domain.Enums;
using CloPosProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly int CLEANING_TIME_MINUTES=30;
        private readonly int MIN_TIME_SLOT_MINUTES=30;
        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SimpleResponse<string>> CancelReservationAsync(Guid id, string reason)
        {
            var reservation =await _context.Reservations.Include(x => x.Table).FirstOrDefaultAsync(x => x.Id == id);
            if (reservation == null) { 
                return new SimpleResponse<string>("Rezervasiya tapılmadı"); }
            reservation.Cancel(reason);
            if (reservation.Table.Status == TableStatus.Reserved)
            {
                reservation.Table.MarkAsAvailable();
            }
            await _context.SaveChangesAsync();
            return new SimpleResponse<string>("Rezervasiya ləğv edildi");
        }

        public async Task<SimpleResponse<string>> CheckInReservationAsync(Guid id)
        {
            var reservation = await _context.Reservations
                      .Include(r => r.Table)
                      .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return new SimpleResponse<string>("Rezervasiya tapılmadı");

            reservation.CheckIn();

            
            reservation.Table.MarkAsOccupied();

            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Müştəri gəldi, masa məşğul edildi");
        }

        public async Task<SimpleResponse<string>> CompleteReservationAsync(Guid id)
        {
            var reservation = await _context.Reservations
                  .Include(r => r.Table)
                  .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return new SimpleResponse<string>("Rezervasiya tapılmadı");

            reservation.Complete();

            reservation.Table.MarkAsCleaning();

            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Rezervasiya tamamlandı. Masa təmizlənir.");
        }

        public async Task<SimpleResponse<string>> ConfirmReservationAsync(Guid id)
        {
            var reservation = await _context.Reservations
                    .Include(r => r.Table)
                    .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return new SimpleResponse<string>("Rezervasiya tapılmadı");

            reservation.Confirm();

            reservation.Table.MarkAsReserved();

            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Rezervasiya təsdiqləndi və masa rezerv edildi");
        }

        public async Task<SimpleResponse<Guid>> CreateReservationAsync(Guid tableId, string customerName, string customerPhone, string customerEmail, int guestCount, DateTime reservationDate, TimeSpan reservationTime, int durationMinutes, string specialRequests)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null)
                return new SimpleResponse<Guid>("Masa tapılmadı");

            if (!table.IsActive)
                return new SimpleResponse<Guid>("Bu masa aktiv deyil");

            if (guestCount > table.Capacity)
                return new SimpleResponse<Guid>($"Bu masa maksimum {table.Capacity} nəfərlik tutuma malikdir");

            // Keçmiş tarix yoxlaması
            var reservationDateTime = reservationDate.Date + reservationTime;
            if (reservationDateTime < DateTime.UtcNow)
                return new SimpleResponse<Guid>("Keçmiş tarixə rezervasiya edilə bilməz");

            // Masa müsaitliyini yoxla
            var isAvailable = await IsTableAvailableInternalAsync(
                tableId,
                reservationDate,
                reservationTime,
                durationMinutes
            );

            if (!isAvailable)
                return new SimpleResponse<Guid>("Bu vaxt üçün masa müsait deyil. Başqa vaxt seçin.");

            var reservation = new CloPosProject.Domain.Entities.Reservation(
                tableId,
                customerName,
                customerPhone,
                customerEmail,
                guestCount,
                reservationDate,
                reservationTime,
                durationMinutes,
                specialRequests
            );

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return new SimpleResponse<Guid>("Rezervasiya uğurla yaradıldı", reservation.Id);
        }

        public async Task<SimpleResponse<List<ReservationResponse>>> GetAllAsync(DateTime? date = null, ReservationStatus? status = null, Expression<Func<Domain.Entities.Reservation, bool>>? predicate = null)
        {
            var query = _context.Reservations
                            .Include(r => r.Table)
                            .AsQueryable();

            if (date.HasValue)
            {
                var dateOnly = date.Value.Date;
                query = query.Where(r => r.ReservationDate == dateOnly);
            }
            if (predicate!=null)
            {
                query = query.Where(predicate);
            }
            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            var reservations = await query
                .OrderBy(r => r.ReservationDate)
                .ThenBy(r => r.ReservationTime)
                .ToListAsync();

            var responses = reservations.Select(MapToResponse).ToList();
            return new SimpleResponse<List<ReservationResponse>>(responses);
        }

        public async Task<SimpleResponse<List<AvailableTimeSlot>>> GetAvailableTimeSlotsAsync(Guid tableId, DateTime date, int durationMinutes=30)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null)
                return new SimpleResponse<List<AvailableTimeSlot>>("Masa tapılmadı");

            var openingTime = new TimeSpan(9, 0, 0);
            var closingTime = new TimeSpan(23, 0, 0);

            var availableSlots = new List<AvailableTimeSlot>();
            var currentSlot = openingTime;

            while (currentSlot + TimeSpan.FromMinutes(durationMinutes) <= closingTime)
            {
                var isAvailable = await IsTableAvailableInternalAsync(
                    tableId,
                    date,
                    currentSlot,
                    durationMinutes
                );

                if (isAvailable)
                {
                    availableSlots.Add(new AvailableTimeSlot
                    {
                        StartTime = currentSlot,
                        EndTime = currentSlot + TimeSpan.FromMinutes(durationMinutes),
                        DisplayTime = $"{currentSlot:hh\\:mm} - {currentSlot + TimeSpan.FromMinutes(durationMinutes):hh\\:mm}"
                    });
                }

                currentSlot = currentSlot.Add(TimeSpan.FromMinutes(MIN_TIME_SLOT_MINUTES));
            }

            return new SimpleResponse<List<AvailableTimeSlot>>(availableSlots);
        }

        public async Task<SimpleResponse<ReservationResponse>> GetByIdAsync(Guid id)
        {
            var reservation = await _context.Reservations
                    .Include(r => r.Table)
                    .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return new SimpleResponse<ReservationResponse>("Rezervasiya tapılmadı");

            var response = MapToResponse(reservation);
            return new SimpleResponse<ReservationResponse>(response);
        }

        public async Task<SimpleResponse<List<ReservationResponse>>> GetTableReservationsAsync(Guid tableId, DateTime date)
        {
            var dateOnly = date.Date;
            var reservations = await _context.Reservations
                .Include(r => r.Table)
                .Where(r => r.TableId == tableId && r.ReservationDate == dateOnly)
                .OrderBy(r => r.ReservationTime)
                .ToListAsync();

            var responses = reservations.Select(MapToResponse).ToList();
            return new SimpleResponse<List<ReservationResponse>>(responses);
        }

        public async Task<SimpleResponse<List<ReservationResponse>>> GetTodayReservationsAsync()
        {
            return await GetAllAsync(date: DateTime.Today);
        }

        public async Task<SimpleResponse<List<ReservationResponse>>> GetUpcomingReservationsAsync()
        {
            var now = DateTime.UtcNow;
            var reservations = await _context.Reservations
                .Include(r => r.Table)
                .Where(r => (r.ReservationDate > now.Date ||
                            (r.ReservationDate == now.Date && r.ReservationTime > now.TimeOfDay)) &&
                           (r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.Pending))
                .OrderBy(r => r.ReservationDate)
                .ThenBy(r => r.ReservationTime)
                .Take(10)
                .ToListAsync();

            var responses = reservations.Select(MapToResponse).ToList();
            return new SimpleResponse<List<ReservationResponse>>(responses);
        }

        public async Task<SimpleResponse<bool>> IsTableAvailableAsync(Guid tableId, DateTime reservationDate, TimeSpan reservationTime, int durationMinutes)
        {
            var isAvailable = await IsTableAvailableInternalAsync(
                  tableId,
                  reservationDate,
                  reservationTime,
                  durationMinutes
              );

            return new SimpleResponse<bool>(isAvailable);
        }

        public async Task<SimpleResponse<string>> MarkAsNoShowAsync(Guid id)
        {
            var reservation = await _context.Reservations
                  .Include(r => r.Table)
                  .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return new SimpleResponse<string>("Rezervasiya tapılmadı");

            reservation.MarkAsNoShow();

            if (reservation.Table.Status == TableStatus.Reserved)
            {
                reservation.Table.MarkAsAvailable();
            }

            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Rezervasiya 'Gəlmədi' kimi qeyd edildi");
        }

        public async Task ProcessExpiredReservationsAsync()
        {
            var now = DateTime.UtcNow;

            var expiredReservations = _context.Reservations
       .Where(r => r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed)
       .AsEnumerable() 
       .Where(r => r.ReservationDate + r.ReservationTime < DateTime.UtcNow)
       .ToList();

            foreach (var reservation in expiredReservations)
            {
                reservation.MarkAsNoShow();
                if (reservation.Table.Status == TableStatus.Reserved)
                {
                    reservation.Table.MarkAsAvailable();
                }
            }

            var completedReservations = await _context.Reservations
                .Include(r => r.Table)
                .Where(r => r.Status == ReservationStatus.Completed)
                .ToListAsync();

            foreach (var reservation in completedReservations)
            {
                if (reservation.EstimatedEndTime.AddMinutes(CLEANING_TIME_MINUTES) < now)
                {
                    if (reservation.Table.Status != TableStatus.Available &&
                        reservation.Table.Status != TableStatus.Occupied)
                    {
                        reservation.Table.MarkAsAvailable();
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
        private async Task<bool> IsTableAvailableInternalAsync(
           Guid tableId,
           DateTime reservationDate,
           TimeSpan reservationTime,
           int durationMinutes)
        {
            var dateOnly = reservationDate.Date;
            var requestedStart = reservationTime;
            var requestedEnd = reservationTime.Add(TimeSpan.FromMinutes(durationMinutes + CLEANING_TIME_MINUTES));

         
            var existingReservations = await _context.Reservations
                .Where(r => r.TableId == tableId &&
                           r.ReservationDate == dateOnly &&
                           (r.Status == ReservationStatus.Confirmed ||
                            r.Status == ReservationStatus.Seated ||
                            r.Status == ReservationStatus.Pending))
                .ToListAsync();

            foreach (var existing in existingReservations)
            {
                var existingStart = existing.ReservationTime;
                var existingEnd = existing.ReservationTime.Add(
                    TimeSpan.FromMinutes(existing.DurationMinutes + CLEANING_TIME_MINUTES)
                );

                // Overlap yoxlaması
                if (requestedStart < existingEnd && requestedEnd > existingStart)
                {
                    return false;
                }
            }

            return true;
        }
        private ReservationResponse MapToResponse(CloPosProject.Domain.Entities.Reservation reservation)
        {
            return new ReservationResponse
            {
                Id = reservation.Id,
                TableId = reservation.TableId,
                TableNumber = reservation.Table.TableNumber,
                CustomerName = reservation.CustomerName,
                CustomerPhone = reservation.CustomerPhone,
                CustomerEmail = reservation.CustomerEmail,
                GuestCount = reservation.GuestCount,
                ReservationDate = reservation.ReservationDate,
                ReservationTime = reservation.ReservationTime,
                ReservationDateTime = reservation.ReservationDateTime,
                DurationMinutes = reservation.DurationMinutes,
                EstimatedEndTime = reservation.EstimatedEndTime,
                Status = reservation.Status,
                StatusDisplay = GetStatusDisplay(reservation.Status),
                SpecialRequests = reservation.SpecialRequests,
                CreatedAt = reservation.CreatedAt,
                ConfirmedAt = reservation.ConfirmedAt
            };
        }
        private string GetStatusDisplay(ReservationStatus status)
        {
            return status switch
            {
                ReservationStatus.Pending => "Gözləmədə",
                ReservationStatus.Confirmed => "Təsdiqləndi",
                ReservationStatus.Seated => "Oturdu",
                ReservationStatus.Completed => "Tamamlandı",
                ReservationStatus.Cancelled => "Ləğv edildi",
                ReservationStatus.NoShow => "Gəlmədi",
                _ => status.ToString()
            };
        }
    }
}
