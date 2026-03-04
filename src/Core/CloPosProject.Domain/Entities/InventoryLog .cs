using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class InventoryLog : BaseEntity
    {
        public Guid IngredientId { get; private set; }
        public Ingredient Ingredient { get; private set; }

        public InventoryLogType LogType { get; private set; }
        public decimal QuantityBefore { get; private set; }
        public decimal QuantityChange { get; private set; }
        public decimal QuantityAfter { get; private set; }
        public decimal? UnitPrice { get; private set; }

        public string Reason { get; private set; }
        public Guid? OrderId { get; private set; }
        public Guid? UserId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private InventoryLog() { }

        public InventoryLog(
            Guid ingredientId,
            InventoryLogType logType,
            decimal quantityBefore,
            decimal quantityChange,
            decimal quantityAfter,
            decimal? unitPrice,
            string reason,
            Guid? orderId = null,
            Guid? userId = null)
        {
            IngredientId = ingredientId;
            LogType = logType;
            QuantityBefore = quantityBefore;
            QuantityChange = quantityChange;
            QuantityAfter = quantityAfter;
            UnitPrice = unitPrice;
            Reason = reason;
            OrderId = orderId;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
