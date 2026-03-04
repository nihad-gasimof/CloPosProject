using CloPosProject.Domain.Enums;

namespace CloPosProject.Application.DTOs.Reservation
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public Guid TableId { get; set; }
        public string TableNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public int GuestCount { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime EstimatedEndTime { get; set; }
        public ReservationStatus Status { get; set; }
        public string StatusDisplay { get; set; }
        public string SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }
}
