namespace CloPosProject.Application.DTOs.Reservation
{
    public class AvailableTimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DisplayTime { get; set; }
    }
}
