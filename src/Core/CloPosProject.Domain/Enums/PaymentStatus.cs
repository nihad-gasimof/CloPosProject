using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Enums
{
    public enum PaymentStatus
    {
       FullyPaid,
       Cancelled,
       Rejected,
       Refused,
       Expired,
       Authorized,
       PartiallyPaid,
       Funded,
       Declined,
       Voided,
       Refunded,
       Closed
    }
}
