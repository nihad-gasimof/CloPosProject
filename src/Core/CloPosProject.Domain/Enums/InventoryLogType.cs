using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Enums
{
    public enum InventoryLogType
    {
        StockIn,        // Stok əlavəsi
        StockOut,       // Sifariş üçün çıxarış
        Adjustment,     // Manual düzəliş
        Waste,          // İtki/zay
        Return          // Geri qaytarma
    }
}
