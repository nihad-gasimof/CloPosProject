using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Order
{
    public record OrderItemRequest(
        Guid MenuItemId,
        int Quantity,
        string SpecialInstructions  
    );
}
