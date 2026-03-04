using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Reservation
{
    public record CreateReservationRequest(
      Guid TableId,
      string CustomerName,
      string CustomerPhone,
      string CustomerEmail,
      int GuestCount,
      DateTime ReservationDate,
      TimeSpan ReservationTime,
      int DurationMinutes,
      string SpecialRequests
  );
}
