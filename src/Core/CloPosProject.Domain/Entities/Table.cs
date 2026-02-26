using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloPosProject.Domain.Enums;
namespace CloPosProject.Domain.Entities
{
    public class Table : BaseEntity
    {
        public string TableNumber { get; private set; } = string.Empty;
        public int Capacity { get; private set; }
        public TableStatus Status { get; private set; }
        public bool IsActive { get; private set; }
        public string Location { get; private set; } = string.Empty;

        public List<Order> Orders { get; private set; } = new();
        public List<Reservation> Reservations { get; private set; } = new();

        private Table() { }

        public Table(string tableNumber, int capacity, string location=null)
        {
            TableNumber = tableNumber;
            Capacity = capacity;
            Location = location;
            Status = TableStatus.Available;
            IsActive = true;
        }

        public void UpdateDetails(string tableNumber, int capacity, string location)
        {
            if (capacity <= 0)
                throw new ArgumentException("Tutum müsbət olmalıdır");

            TableNumber = tableNumber;
            Capacity = capacity;
            Location = location;
        }

        public void MarkAsOccupied()
        {
            if (!IsActive)
                throw new InvalidOperationException("Deaktiv masa istifadə edilə bilməz");

            Status = TableStatus.Occupied;
        }
        public void MarkAsCleaning()
        {
            if (!IsActive)
                throw new InvalidOperationException("Deaktiv masa istifadə edilə bilməz");

            Status = TableStatus.Cleaning;
        }

        public void MarkAsReserved()
        {
            if (!IsActive)
                throw new InvalidOperationException("Deaktiv masa rezerv edilə bilməz");

            if (Status == TableStatus.Occupied)
                throw new InvalidOperationException("Məşğul masa rezerv edilə bilməz");

            Status = TableStatus.Reserved;
        }

        public void MarkAsAvailable()
        {
            Status = TableStatus.Available;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}
