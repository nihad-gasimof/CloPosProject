using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{

        public class Reservation : BaseEntity
        {
            public Guid TableId { get; private set; }
            public Table Table { get; private set; }

            public string CustomerName { get; private set; }
            public string CustomerPhone { get; private set; }
            public string CustomerEmail { get; private set; }
            public int GuestCount { get; private set; }

            public DateTime ReservationDate { get; private set; }
            public TimeSpan ReservationTime { get; private set; }
            public DateTime ReservationDateTime => ReservationDate.Date + ReservationTime;

            public int DurationMinutes { get; private set; } // Rezervasiya müddəti (dəqiqə)
            public DateTime EstimatedEndTime => ReservationDateTime.AddMinutes(DurationMinutes);

            public ReservationStatus Status { get; private set; }
            public string SpecialRequests { get; private set; }
            public DateTime CreatedAt { get; private set; }
            public DateTime? ConfirmedAt { get; private set; }
            public DateTime? CancelledAt { get; private set; }
            public string? CancellationReason { get; private set; }


            public Reservation(
                Guid tableId,
                string customerName,
                string customerPhone,
                string customerEmail,
                int guestCount,
                DateTime reservationDate,
                TimeSpan reservationTime,
                int durationMinutes,
                string specialRequests = null)
            {
                if (string.IsNullOrWhiteSpace(customerName))
                    throw new ArgumentException("Müştəri adı boş ola bilməz");

                if (string.IsNullOrWhiteSpace(customerPhone))
                    throw new ArgumentException("Telefon nömrəsi boş ola bilməz");

                if (guestCount <= 0)
                    throw new ArgumentException("Qonaq sayı müsbət olmalıdır");

                if (durationMinutes < 30)
                    throw new ArgumentException("Rezervasiya müddəti minimum 30 dəqiqə olmalıdır");

                TableId = tableId;
                CustomerName = customerName;
                CustomerPhone = customerPhone;
                CustomerEmail = customerEmail;
                GuestCount = guestCount;
                ReservationDate = reservationDate.Date;
                ReservationTime = reservationTime;
                DurationMinutes = durationMinutes;
                SpecialRequests = specialRequests;
                Status = ReservationStatus.Pending;
                CreatedAt = DateTime.UtcNow;
            }

            public void Confirm()
            {
                if (Status != ReservationStatus.Pending)
                    throw new InvalidOperationException("Yalnız gözləmədəki rezervasiyalar təsdiqlənə bilər");

                Status = ReservationStatus.Confirmed;
                ConfirmedAt = DateTime.UtcNow;
            }

            public void CheckIn()
            {
                if (Status != ReservationStatus.Confirmed)
                    throw new InvalidOperationException("Yalnız təsdiqlənmiş rezervasiyalar check-in edilə bilər");

                Status = ReservationStatus.Seated;
            }

            public void Complete()
            {
                if (Status != ReservationStatus.Seated)
                    throw new InvalidOperationException("Yalnız oturmuş rezervasiyalar tamamlana bilər");

                Status = ReservationStatus.Completed;
            }

            public void Cancel(string reason)
            {
                if (Status == ReservationStatus.Completed)
                    throw new InvalidOperationException("Tamamlanmış rezervasiya ləğv edilə bilməz");

                if (Status == ReservationStatus.Cancelled)
                    throw new InvalidOperationException("Rezervasiya artıq ləğv edilib");

                Status = ReservationStatus.Cancelled;
                CancelledAt = DateTime.UtcNow;
                CancellationReason = reason;
            }

            public void MarkAsNoShow()
            {
                if (Status != ReservationStatus.Confirmed)
                    throw new InvalidOperationException("Yalnız təsdiqlənmiş rezervasiyalar no-show edilə bilər");

                Status = ReservationStatus.NoShow;
            }

            public bool IsActive()
            {
                return Status == ReservationStatus.Confirmed || Status == ReservationStatus.Seated;
            }

            public bool HasPassed()
            {
                return EstimatedEndTime < DateTime.UtcNow;
            }
        }
    }
