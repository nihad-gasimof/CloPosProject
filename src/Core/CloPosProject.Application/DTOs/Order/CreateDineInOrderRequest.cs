using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Order
{
    public record CreateDineInOrderRequest(
        Guid TableId,
        string TableNumber,
        string WaiterId,
        string Notes,  
        List<OrderItemRequest> Items
    );
}
