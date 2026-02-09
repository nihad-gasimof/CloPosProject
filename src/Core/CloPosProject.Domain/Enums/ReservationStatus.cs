using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Enums
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Arrived,
        Completed,
        Cancelled,
        NoShow
    }
}
