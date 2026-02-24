using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Payment
{
    public class PaymentStatusDto
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
