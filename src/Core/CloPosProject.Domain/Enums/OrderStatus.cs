using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,      // sifariş yaradılıb, təsdiq gözləyir
        Confirmed,    // sifariş təsdiqlənib
        Preparing,    // mətbəxdə hazırlanır
        Ready,        // hazırdır, servisə çıxarılmağa hazır
        Served,       // masaya verilib
        Completed,    // ödəniş tamamlanıb, sifariş tam bitib
        Cancelled     // ləğv edilib
    }

}
