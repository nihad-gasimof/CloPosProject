using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Order
{
    public interface IOrderService
    {
        Task<SimpleResponse<Guid>> CreateDineInOrderAsync(
            Guid tableId,
            Guid waiterId,
            string tableNumber,
            string notes,
            List<OrderItemRequest> items);
        Task<SimpleResponse<Guid>> CreateTakeAwayOrderAsync(
            string customerName,
            string customerPhone,
            DateTime? pickupTime,
            string notes,
            List<OrderItemRequest> items);

        Task<SimpleResponse<Guid>> CreateDeliveryOrderAsync(
            string customerName,
            string customerPhone,
            string deliveryAddress,
            DeliveryProvider deliveryProvider,
            decimal deliveryFee,
            string deliveryInstructions,
            string notes,
            List<OrderItemRequest> items);
        Task<SimpleResponse<string>> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
        Task<SimpleResponse<string>> CancelOrderAsync(Guid orderId);
        Task<SimpleResponse<string>> ApplyDiscountAsync(Guid orderId, decimal discount);
        Task<SimpleResponse<string>> MarkAsPickedUpAsync(Guid orderId);

        Task<SimpleResponse<OrderResponse>> GetByIdAsync(Guid id);
        Task<SimpleResponse<OrderResponse>> GetByOrderNumberAsync(string orderNumber);
        Task<SimpleResponse<List<OrderSummaryResponse>>> GetAllAsync(int pageNumber = 1,
     int pageSize = 20,
            OrderStatus? status = null,
            OrderType? orderType = null,
            DateTime? date = null
            );
        Task<SimpleResponse<List<OrderSummaryResponse>>> GetTodayOrdersAsync(int pageNumber = 1,
     int pageSize = 20);
        Task<SimpleResponse<List<OrderSummaryResponse>>> GetActiveOrdersAsync(int pageNumber = 1,
     int pageSize = 20);
        Task<SimpleResponse<List<OrderSummaryResponse>>> GetTableOrdersAsync(Guid tableId, int pageNumber = 1,
     int pageSize = 20);
        Task<SimpleResponse<List<OrderSummaryResponse>>> GetPendingTakeAwayOrdersAsync(int pageNumber = 1,
     int pageSize = 20);
        Task<SimpleResponse<List<OrderSummaryResponse>>> GetActiveDeliveryOrdersAsync(int pageNumber = 1,
     int pageSize = 20);
    }
}