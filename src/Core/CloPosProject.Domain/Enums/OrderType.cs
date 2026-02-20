using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Enums
{
    public enum OrderType
    {
        DineIn,       // Restoranda (masa)
        TakeAway,     // Götürüb aparma
        Delivery      // Çatdırılma (Wolt, Bolt və s.)
    }
}
