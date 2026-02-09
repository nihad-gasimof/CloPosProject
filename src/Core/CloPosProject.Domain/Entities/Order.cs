using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; private set; }
        public Guid TableId { get; private set; }
        public Table Table { get; private set; }

        public Guid WaiterId { get; private set; }
        public User Waiter { get; private set; }

        public OrderStatus Status { get; private set; }
        public decimal SubTotal { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total { get; private set; }
        public string Notes { get; private set; }
      

        public List<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

      
    }

}
