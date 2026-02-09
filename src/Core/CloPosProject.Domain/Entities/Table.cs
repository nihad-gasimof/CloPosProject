using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloPosProject.Domain.Enums;
namespace CloPosProject.Domain.Entities
{
    public class Table: BaseEntity
    {
        public string TableNumber { get; private set; }
        public int Capacity { get; private set; }
        public TableStatus Status { get; private set; }
        public bool IsActive { get; private set; }

        public List<Order>? Orders { get; private set; } = new List<Order>();
        public List<Reservation>? Reservations { get; private set; } = new List<Reservation>();
    }
}
