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
        public ReservationStatus Status { get; private set; }
        public string SpecialRequests { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}