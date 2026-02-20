using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Order
{
    public record CreateTakeAwayOrderRequest(
     string CustomerName,      
     string CustomerPhone,     
     DateTime? PickupTime,     
     string Notes,             
     List<OrderItemRequest> Items
 );
}
