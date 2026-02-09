using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending,      // ödəniş gözləyir
        Completed,    // ödəniş tamamlanıb
        Failed,       // ödəniş uğursuz olub
        Refunded      // ödəniş geri qaytarılıb
    }
}
