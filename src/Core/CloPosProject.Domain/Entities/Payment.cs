using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        public decimal Amount { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string TransactionId { get; private set; }
        public DateTime CreatedAt { get; private set; }

    }
}