namespace CloPosProject.Application.DTOs.Reservation
{
    public record UpdateReservationRequest(
       string CustomerName,
       string CustomerPhone,
       string CustomerEmail,
       int GuestCount,
       string SpecialRequests
   );
}
