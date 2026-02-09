using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Enums
{
    public enum TableStatus
    {
        Available,    // masa boşdur
        Occupied,     // masa doludur
        Reserved,     // masa rezerv olunub
        Cleaning      // masa təmizlənir
    }
}
