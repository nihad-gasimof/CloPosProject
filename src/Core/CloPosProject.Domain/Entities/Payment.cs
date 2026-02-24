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
        public int PurchaseId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public Order Order { get; set; } = null!;
        public Guid OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public void MarkAsPaid(string transactionId)
        {
            PaymentStatus = PaymentStatus.FullyPaid;
        }

        public void MarkAsFailed()
        {
            PaymentStatus = PaymentStatus.Rejected;
        }

    }
}