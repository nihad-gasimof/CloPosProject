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
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public OrderType OrderType { get; private set; }

        // Qiymət məlumatları
        public decimal TotalAmount { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Discount { get; private set; }
        public decimal DeliveryFee { get; private set; }
        public decimal FinalAmount { get; private set; }

        // Müştəri məlumatları
        public string CustomerName { get; private set; }
        public string CustomerPhone { get; private set; }
        public string Notes { get; private set; }

        // Restoran məlumatları (DineIn)
        public Guid WaiterId { get; private set; }
        public User Waiter{ get; private set; }
        public Guid? TableId { get; private set; }
        public Table? Table{ get; private set; }
        public string TableNumber { get; private set; }

        // Çatdırılma məlumatları (Delivery)
        public DeliveryProvider? DeliveryProvider { get; private set; }
        public string DeliveryAddress { get; private set; }
        public string DeliveryInstructions { get; private set; }
        public DateTime? EstimatedDeliveryTime { get; private set; }

        // TakeAway məlumatları
        public DateTime? PickupTime { get; private set; }
        public bool IsPickedUp { get; private set; }
        public ICollection<Payment> Payments { get;  set; } = [];

        public List<OrderItem> OrderItems { get; private set; } = new();

        private Order() { }
        public bool IsPaid => Payments.Any(p => p.PaymentStatus == PaymentStatus.FullyPaid);
        // DineIn constructor
        public static Order CreateDineInOrder(
            Guid WaiterId,
            Guid tableId,
            string tableNumber,
            string notes = null)
        {
            return new Order
            {
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderType = OrderType.DineIn,
                TableId = tableId,
                TableNumber = tableNumber,
                Notes = notes,
                WaiterId=WaiterId
                ,
                DeliveryProvider = Enums.DeliveryProvider.None,
                DeliveryFee = 0
            };
        }

        // TakeAway constructor
        public static Order CreateTakeAwayOrder(
            string customerName,
            string customerPhone,
            DateTime? pickupTime = null,
            string notes = null)
        {
            return new Order
            {
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderType = OrderType.TakeAway,
                CustomerName = customerName,
                CustomerPhone = customerPhone,
                PickupTime = pickupTime ?? DateTime.UtcNow.AddMinutes(30), // Default 30 dəqiqə
                Notes = notes,
                DeliveryProvider = Enums.DeliveryProvider.None,
                DeliveryFee = 0,
                IsPickedUp = false
            };
        }

        // Delivery constructor
        public static Order CreateDeliveryOrder(
            string customerName,
            string customerPhone,
            string deliveryAddress,
            DeliveryProvider deliveryProvider,
            decimal deliveryFee,
            string deliveryInstructions = null,
            string notes = null)
        {
            var estimatedTime = CalculateEstimatedDeliveryTime(deliveryProvider);

            return new Order
            {
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderType = OrderType.Delivery,
                CustomerName = customerName,
                CustomerPhone = customerPhone,
                DeliveryAddress = deliveryAddress,
                DeliveryProvider = deliveryProvider,
                DeliveryFee = deliveryFee,
                DeliveryInstructions = deliveryInstructions,
                EstimatedDeliveryTime = estimatedTime,
                Notes = notes
            };
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        }

        private static DateTime CalculateEstimatedDeliveryTime(DeliveryProvider provider)
        {
            // Provider-ə görə təxmini çatdırılma vaxtı
            var minutes = provider switch
            {
                Enums.DeliveryProvider.Wolt => 45,
                Enums.DeliveryProvider.Bolt => 40,
                Enums.DeliveryProvider.Yemeksepeti => 50,
                Enums.DeliveryProvider.OwnDelivery => 35,
                _ => 45
            };

            return DateTime.UtcNow.AddMinutes(minutes);
        }

        public void CalculateTotals(decimal taxRate = 0.18m)
        {
            TotalAmount = OrderItems.Sum(oi => oi.Subtotal);
            Tax = TotalAmount * taxRate;
            FinalAmount = TotalAmount + Tax - Discount + DeliveryFee;
        }

        public void ApplyDiscount(decimal discount)
        {
            if (discount < 0)
                throw new ArgumentException("Endirim mənfi ola bilməz");

            if (discount > TotalAmount)
                throw new ArgumentException("Endirim ümumi məbləğdən çox ola bilməz");

            Discount = discount;
            CalculateTotals();
        }

        public void UpdateDeliveryFee(decimal newFee)
        {
            if (OrderType != OrderType.Delivery)
                throw new InvalidOperationException("Yalnız çatdırılma sifarişləri üçün çatdırılma haqqı dəyişdirilə bilər");

            if (newFee < 0)
                throw new ArgumentException("Çatdırılma haqqı mənfi ola bilməz");

            DeliveryFee = newFee;
            CalculateTotals();
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            // Status dəyişmə qaydalarını yoxla
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Ləğv edilmiş sifarişin statusu dəyişdirilə bilməz");

            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("Tamamlanmış sifarişin statusu dəyişdirilə bilməz");

            Status = newStatus;
        }

        public void MarkAsConfirmed() => UpdateStatus(OrderStatus.Confirmed);
        public void MarkAsPreparing() => UpdateStatus(OrderStatus.Preparing);

        public void MarkAsReady()
        {
            UpdateStatus(OrderStatus.Ready);

            // TakeAway üçün pickup vaxtını set et
            if (OrderType == OrderType.TakeAway && !PickupTime.HasValue)
            {
                PickupTime = DateTime.UtcNow.AddMinutes(10);
            }
        }

        public void MarkAsCompleted()
        {
            UpdateStatus(OrderStatus.Completed);

            // TakeAway üçün götürüldü işarəsi
            if (OrderType == OrderType.TakeAway)
            {
                IsPickedUp = true;
            }
        }

        public void MarkAsCancelled()
        {
            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("Tamamlanmış sifariş ləğv edilə bilməz");

            Status = OrderStatus.Cancelled;
        }

        public void MarkAsPickedUp()
        {
            if (OrderType != OrderType.TakeAway)
                throw new InvalidOperationException("Yalnız götürüb aparma sifarişləri götürülə bilər");

            if (Status != OrderStatus.Ready)
                throw new InvalidOperationException("Sifariş hazır olmadan götürülə bilməz");

            IsPickedUp = true;
            MarkAsCompleted();
        }
    }
}

