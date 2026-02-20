using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Order
{
    public record CreateDeliveryOrderRequest(
       string CustomerName,          
       string CustomerPhone,         
       string DeliveryAddress,        
       DeliveryProvider DeliveryProvider, 
       decimal DeliveryFee,
       string DeliveryInstructions,  
       string Notes,                  
       List<OrderItemRequest> Items
   );
}
