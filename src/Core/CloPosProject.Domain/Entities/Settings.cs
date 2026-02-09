using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class Settings : BaseEntity
    {
        public string RestaurantName { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public decimal TaxRate { get; private set; }
        public string Currency { get; private set; }
        public TimeSpan OpeningTime { get; private set; }
        public TimeSpan ClosingTime { get; private set; }
        public int DefaultReservationDuration { get; private set; }
        public bool EnableReservations { get; private set; }

    }

}