using CloPosProject.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Payment
{
    public interface IPaymentService
    {
        Task<PurchaseDto> CreatePaymentRequest(OrderCreateDto dto);
        Task<PaymentStatusDto> GetPaymentStatus(int purchaseId);
     }
}
