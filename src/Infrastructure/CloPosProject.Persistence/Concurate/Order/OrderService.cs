using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.DTOs.Payment;
using CloPosProject.Domain.Entities;
using CloPosProject.Domain.Enums;
using CloPosProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Order
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly CloPosProject.Application.Abstract.Payment.IPaymentService _paymentService;
        private const decimal TAX_RATE = 0.18m;


        public OrderService(ApplicationDbContext context, CloPosProject.Application.Abstract.Payment.IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<SimpleResponse<string>> ApplyDiscountAsync(Guid orderId, decimal discount)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return new SimpleResponse<string>("Sifariş tapılmadı");

            if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
                return new SimpleResponse<string>("Bu sifarişə endirim tətbiq edilə bilməz");

            if (discount > order.TotalAmount)
                return new SimpleResponse<string>("Endirim ümumi məbləğdən çox ola bilməz");

            order.ApplyDiscount(discount);
            await _context.SaveChangesAsync();


            return new SimpleResponse<string>("Endirim uğurla tətbiq edildi");
        }


        public async Task<SimpleResponse<string>> CancelOrderAsync(Guid orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                    .ThenInclude(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                    .ThenInclude(i => i.Inventory)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return new SimpleResponse<string>("Sifariş tapılmadı");

            if (order.Status == OrderStatus.Completed)
                return new SimpleResponse<string>("Tamamlanmış sifariş ləğv edilə bilməz");

            if (order.Status == OrderStatus.Cancelled)
                return new SimpleResponse<string>("Sifariş artıq ləğv edilib");

            if (order.Status == OrderStatus.Confirmed || order.Status == OrderStatus.Preparing)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    foreach (var menuItemIngredient in orderItem.MenuItem.MenuItemIngredients)
                    {
                        var returnQuantity = menuItemIngredient.Quantity * orderItem.Quantity;

                        if (menuItemIngredient.Ingredient.Inventory != null)
                        {
                            menuItemIngredient.Ingredient.Inventory.AddStock(returnQuantity, 0);


                        }
                    }
                }
            }

            order.MarkAsCancelled();
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return new SimpleResponse<string>("Sifaris Legv Edildi");
        }

        public async Task<SimpleResponse<Guid>> CreateDineInOrderAsync(Guid tableId, Guid waiterId, string tableNumber, string notes, List<OrderItemRequest> items)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            if (items == null || !items.Any())
                return new SimpleResponse<Guid>("Sifariş item-ları boş ola bilməz.");

            if (items.Any(i => i.Quantity <= 0))
                return new SimpleResponse<Guid>("Məhsul miqdarı müsbət olmalıdır");

            var menuItemIds = items.Select(i => i.MenuItemId).Distinct().ToList();
            var menuItems = await _context.MenuItems
                .Include(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                    .ThenInclude(i => i.Inventory)
                .Where(m => menuItemIds.Contains(m.Id))
                .ToListAsync();

            if (menuItems.Count != menuItemIds.Count)
                return new SimpleResponse<Guid>("Bəzi məhsullar tapılmadı");

            var inactiveItems = menuItems.Where(m => !m.IsAvailable).ToList();
            if (inactiveItems.Any())
                return new SimpleResponse<Guid>($"Bu məhsullar mövcud deyil: {string.Join(", ", inactiveItems.Select(m => m.Name))}");
            var insufficientStockItems = new List<string>();
            foreach (var item in items)
            {
                var menuItem = menuItems.First(m => m.Id == item.MenuItemId);

                foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
                {
                    var requiredQuantity = menuItemIngredient.Quantity * item.Quantity;
                    var availableQuantity = menuItemIngredient.Ingredient.CurrentStock;

                    if (availableQuantity < requiredQuantity)
                    {
                        insufficientStockItems.Add(
                            $"{menuItem.Name} (kifayət qədər {menuItemIngredient.Ingredient.Name} yoxdur: " +
                            $"lazım {requiredQuantity}{menuItemIngredient.Ingredient.Unit}, " +
                            $"mövcud {availableQuantity}{menuItemIngredient.Ingredient.Unit})"
                        );
                    }


                }
            }
            if (insufficientStockItems.Any())
                return new SimpleResponse<Guid>("Kifayət qədər ingredient yoxdur:\n" + string.Join("\n", insufficientStockItems));
            var order = CloPosProject.Domain.Entities.Order.CreateDineInOrder(waiterId, tableId, tableNumber, notes);
            await _context.Orders.AddAsync(order);
            foreach (var item in items)
            {
                var menuItem = menuItems.FirstOrDefault(m => m.Id == item.MenuItemId);

                var orderItem = new OrderItem(
                    order.Id,
                    menuItem.Id,
                    menuItem.Name,
                    menuItem.Price,
                    item.Quantity,
                    item.SpecialInstructions

                );
                await _context.Set<OrderItem>().AddAsync(orderItem);

            }
            foreach (var item in items)
            {
                var menuItem = menuItems.First(m => m.Id == item.MenuItemId);

                foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
                {
                    var requiredQuantity = menuItemIngredient.Quantity * item.Quantity;

                    if (menuItemIngredient.Ingredient.Inventory != null)
                    {
                        menuItemIngredient.Ingredient.Inventory.RemoveStock(requiredQuantity);

                    }
                }
            }
            await _context.SaveChangesAsync();
            var savedOrder = await _context.Orders
              .Include(o => o.OrderItems)
              .FirstAsync(o => o.Id == order.Id);
            savedOrder.CalculateTotals(TAX_RATE);
            await _context.SaveChangesAsync();

            // Create payment request and persist payment (do not mark order completed)
            var purchaseDto = await _paymentService.CreatePaymentRequest(new CloPosProject.Application.DTOs.Payment.OrderCreateDto
            {
                Amount = savedOrder.FinalAmount,
                Currency = "AZN",
                Description = $"Order {savedOrder.OrderNumber}",
                RedirectUrl = savedOrder.OrderType == OrderType.DineIn ? savedOrder.TableNumber ?? string.Empty : string.Empty
            });

            var payment = new CloPosProject.Domain.Entities.Payment
            {
                PurchaseId = int.TryParse(purchaseDto.Order.Id.ToString(), out var pid) ? pid : 0,
                Password = purchaseDto.Order.Password,
                Secret = purchaseDto.Order.Secret,
                OrderId = savedOrder.Id,
                CreatedDate = DateTime.UtcNow,
                PaymentStatus = CloPosProject.Domain.Enums.PaymentStatus.Authorized
            };
            await _context.Set<CloPosProject.Domain.Entities.Payment>().AddAsync(payment);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return new SimpleResponse<Guid>("Sifariş uğurla  yaradıldı", order.Id);
        }

        public async Task<SimpleResponse<Guid>> CreateDeliveryOrderAsync(string customerName, string customerPhone, string deliveryAddress, DeliveryProvider deliveryProvider, decimal deliveryFee, string deliveryInstructions, string notes, List<OrderItemRequest> items)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            if (string.IsNullOrWhiteSpace(customerName))
                return new SimpleResponse<Guid>("Müştəri adı boş ola bilməz");

            if (string.IsNullOrWhiteSpace(customerPhone))
                return new SimpleResponse<Guid>("Müştəri telefonu boş ola bilməz");

            if (string.IsNullOrWhiteSpace(deliveryAddress))
                return new SimpleResponse<Guid>("Çatdırılma ünvanı boş ola bilməz");

            if (deliveryProvider == DeliveryProvider.None)
                return new SimpleResponse<Guid>("Çatdırılma provayderı seçilməlidir");

            if (deliveryFee < 0)
                return new SimpleResponse<Guid>("Çatdırılma haqqı mənfi ola bilməz");


            if (items == null || !items.Any())
                return new SimpleResponse<Guid>("Sifariş item-ları boş ola bilməz.");
            if (items.Any(i => i.Quantity <= 0))
                return new SimpleResponse<Guid>("Məhsul miqdarı müsbət olmalıdır");

            var menuItemIds = items.Select(i => i.MenuItemId).Distinct().ToList();
            var menuItems = await _context.MenuItems
                .Include(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                    .ThenInclude(i => i.Inventory)
                .Where(m => menuItemIds.Contains(m.Id))
                .ToListAsync();

            if (menuItems.Count != menuItemIds.Count)
                return new SimpleResponse<Guid>("Bəzi məhsullar tapılmadı");

            var inactiveItems = menuItems.Where(m => !m.IsAvailable).ToList();
            if (inactiveItems.Any())
                return new SimpleResponse<Guid>($"Bu məhsullar mövcud deyil: {string.Join(", ", inactiveItems.Select(m => m.Name))}");
            var insufficientStockItems = new List<string>();
            foreach (var item in items)
            {
                var menuItem = menuItems.First(m => m.Id == item.MenuItemId);

                foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
                {
                    var requiredQuantity = menuItemIngredient.Quantity * item.Quantity;
                    var availableQuantity = menuItemIngredient.Ingredient.CurrentStock;

                    if (availableQuantity < requiredQuantity)
                    {
                        insufficientStockItems.Add(
                            $"{menuItem.Name} (kifayət qədər {menuItemIngredient.Ingredient.Name} yoxdur: " +
                            $"lazım {requiredQuantity}{menuItemIngredient.Ingredient.Unit}, " +
                            $"mövcud {availableQuantity}{menuItemIngredient.Ingredient.Unit})"
                        );
                    }


                }
            }
            if (insufficientStockItems.Any())
                return new SimpleResponse<Guid>("Kifayət qədər ingredient yoxdur:\n" + string.Join("\n", insufficientStockItems));
            var order = CloPosProject.Domain.Entities.Order.CreateDeliveryOrder(
                   customerName,
                   customerPhone,
                   deliveryAddress,
                   deliveryProvider,
                   deliveryFee,
                   deliveryInstructions,
                   notes
               );
            await _context.Orders.AddAsync(order);
            foreach (var item in items)
            {
                var menuItem = menuItems.FirstOrDefault(m => m.Id == item.MenuItemId);

                var orderItem = new OrderItem(
                    order.Id,
                    menuItem.Id,
                    menuItem.Name,
                    menuItem.Price,
                    item.Quantity,
                    item.SpecialInstructions

                );
                await _context.Set<OrderItem>().AddAsync(orderItem);

            }
            foreach (var item in items)
            {
                var menuItem = menuItems.First(m => m.Id == item.MenuItemId);

                foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
                {
                    var requiredQuantity = menuItemIngredient.Quantity * item.Quantity;

                    if (menuItemIngredient.Ingredient.Inventory != null)
                    {
                        menuItemIngredient.Ingredient.Inventory.RemoveStock(requiredQuantity);

                    }
                }
            }
            await _context.SaveChangesAsync();
            var savedOrder = await _context.Orders
              .Include(o => o.OrderItems)
              .FirstAsync(o => o.Id == order.Id);
            savedOrder.CalculateTotals(TAX_RATE);
            await _context.SaveChangesAsync();

            // Create payment request and attach payment info (do not mark order completed yet)
            var purchaseDto = await _paymentService.CreatePaymentRequest(new CloPosProject.Application.DTOs.Payment.OrderCreateDto
            {
                Amount = savedOrder.FinalAmount,
                Currency = "AZN",
                Description = $"Order {savedOrder.OrderNumber}",
                RedirectUrl = savedOrder.OrderType == OrderType.Delivery ? savedOrder.EstimatedDeliveryTime?.ToString("o") ?? string.Empty : string.Empty
            });

            // Persist payment record
            var payment = new CloPosProject.Domain.Entities.Payment
            {
                PurchaseId = int.TryParse(purchaseDto.Order.Id.ToString(), out var pid) ? pid : 0,
                Password = purchaseDto.Order.Password,
                Secret = purchaseDto.Order.Secret,
                OrderId = savedOrder.Id,
                CreatedDate = DateTime.UtcNow,
                PaymentStatus = CloPosProject.Domain.Enums.PaymentStatus.Authorized
            };
            await _context.Set<CloPosProject.Domain.Entities.Payment>().AddAsync(payment);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return new SimpleResponse<Guid>("Çatdırılma sifarişi uğurla yaradıldı", order.Id);
        }

        public async Task<SimpleResponse<Guid>> CreateTakeAwayOrderAsync(string customerName, string customerPhone, DateTime? pickupTime, string notes, List<OrderItemRequest> items)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            if (items == null || !items.Any())
                return new SimpleResponse<Guid>("Sifariş item-ları boş ola bilməz.");
            if (items.Any(i => i.Quantity <= 0))
                return new SimpleResponse<Guid>("Məhsul miqdarı müsbət olmalıdır");

            if (string.IsNullOrWhiteSpace(customerName))
                return new SimpleResponse<Guid>("Müştəri adı boş ola bilməz");

            if (string.IsNullOrWhiteSpace(customerPhone))
                return new SimpleResponse<Guid>("Müştəri telefonu boş ola bilməz");

            if (pickupTime.HasValue && pickupTime.Value < DateTime.UtcNow)
                return new SimpleResponse<Guid>("Götürmə vaxtı keçmişdə ola bilməz");
            var menuItemIds = items.Select(i => i.MenuItemId).Distinct().ToList();
            var menuItems = await _context.MenuItems
                .Include(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                    .ThenInclude(i => i.Inventory)
                .Where(m => menuItemIds.Contains(m.Id))
                .ToListAsync();

            if (menuItems.Count != menuItemIds.Count)
                return new SimpleResponse<Guid>("Bəzi məhsullar tapılmadı");

            var inactiveItems = menuItems.Where(m => !m.IsAvailable).ToList();
            if (inactiveItems.Any())
                return new SimpleResponse<Guid>($"Bu məhsullar mövcud deyil: {string.Join(", ", inactiveItems.Select(m => m.Name))}");
            var insufficientStockItems = new List<string>();
            foreach (var item in items)
            {
                var menuItem = menuItems.First(m => m.Id == item.MenuItemId);

                foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
                {
                    var requiredQuantity = menuItemIngredient.Quantity * item.Quantity;
                    var availableQuantity = menuItemIngredient.Ingredient.CurrentStock;

                    if (availableQuantity < requiredQuantity)
                    {
                        insufficientStockItems.Add(
                            $"{menuItem.Name} (kifayət qədər {menuItemIngredient.Ingredient.Name} yoxdur: " +
                            $"lazım {requiredQuantity}{menuItemIngredient.Ingredient.Unit}, " +
                            $"mövcud {availableQuantity}{menuItemIngredient.Ingredient.Unit})"
                        );
                    }


                }
            }
            if (insufficientStockItems.Any())
                return new SimpleResponse<Guid>("Kifayət qədər ingredient yoxdur:\n" + string.Join("\n", insufficientStockItems));
            var order = CloPosProject.Domain.Entities.Order.CreateTakeAwayOrder(customerName, customerPhone, pickupTime, notes);
            await _context.Orders.AddAsync(order);
            foreach (var item in items)
            {
                var menuItem = menuItems.FirstOrDefault(m => m.Id == item.MenuItemId);

                var orderItem = new OrderItem(
                    order.Id,
                    menuItem.Id,
                    menuItem.Name,
                    menuItem.Price,
                    item.Quantity,
                    item.SpecialInstructions

                );
                await _context.Set<OrderItem>().AddAsync(orderItem);

            }
            foreach (var item in items)
            {
                var menuItem = menuItems.First(m => m.Id == item.MenuItemId);

                foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
                {
                    var requiredQuantity = menuItemIngredient.Quantity * item.Quantity;

                    if (menuItemIngredient.Ingredient.Inventory != null)
                    {
                        menuItemIngredient.Ingredient.Inventory.RemoveStock(requiredQuantity);

                    }
                }
            }
            await _context.SaveChangesAsync();
            var savedOrder = await _context.Orders
              .Include(o => o.OrderItems)
              .FirstAsync(o => o.Id == order.Id);
            savedOrder.CalculateTotals(TAX_RATE);
            await _context.SaveChangesAsync();

            // Create payment request and persist payment (do not mark order completed)
            var purchaseDto = await _paymentService.CreatePaymentRequest(new CloPosProject.Application.DTOs.Payment.OrderCreateDto
            {
                Amount = savedOrder.FinalAmount,
                Currency = "AZN",
                Description = $"Order {savedOrder.OrderNumber}",
                RedirectUrl = savedOrder.OrderType == OrderType.TakeAway ? savedOrder.PickupTime?.ToString("o") ?? string.Empty : string.Empty
            });

            var payment = new CloPosProject.Domain.Entities.Payment
            {
                PurchaseId = int.TryParse(purchaseDto.Order.Id.ToString(), out var pid) ? pid : 0,
                Password = purchaseDto.Order.Password,
                Secret = purchaseDto.Order.Secret,
                OrderId = savedOrder.Id,
                CreatedDate = DateTime.UtcNow,
                PaymentStatus = CloPosProject.Domain.Enums.PaymentStatus.Authorized
            };
            await _context.Set<CloPosProject.Domain.Entities.Payment>().AddAsync(payment);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return new SimpleResponse<Guid>("Götürüb aparma sifarişi uğurla yaradıldı", order.Id);
        }

        public async Task<SimpleResponse<List<OrderSummaryResponse>>> GetActiveDeliveryOrdersAsync(int pageNumber = 1,
     int pageSize = 20)
        {
            var activeStatuses = new[]
               {
                    OrderStatus.Pending,
                    OrderStatus.Confirmed,
                    OrderStatus.Preparing,
                    OrderStatus.Ready
                };

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderType == OrderType.Delivery &&
                           activeStatuses.Contains(o.Status))
                .OrderBy(o => o.EstimatedDeliveryTime).Skip((pageNumber-1)*pageSize).Take(pageSize)
                .ToListAsync();

            var responses = orders.Select(MapToSummaryResponse).ToList();
            return new SimpleResponse<List<OrderSummaryResponse>>(responses);
        }

        public async  Task<SimpleResponse<List<OrderSummaryResponse>>> GetActiveOrdersAsync(int pageNumber = 1,
    int pageSize = 20)
        {
            var activeStatuses = new[]
                {
                    OrderStatus.Pending,
                    OrderStatus.Confirmed,
                    OrderStatus.Preparing,
                    OrderStatus.Ready
                };

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => activeStatuses.Contains(o.Status))
                .OrderBy(o => o.OrderDate).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
                .ToListAsync();

            var responses = orders.Select(MapToSummaryResponse).ToList();
            return new SimpleResponse<List<OrderSummaryResponse>>(responses);
        }
        

        public async Task<SimpleResponse<List<OrderSummaryResponse>>> GetAllAsync(int pageNumber = 1,
     int pageSize = 20, OrderStatus? status = null, OrderType? orderType = null, DateTime? date = null)
        {
                var query = _context.Orders
                    .Include(o => o.OrderItems)
                    .AsQueryable();

                if (status.HasValue)
                    query = query.Where(o => o.Status == status.Value);

                if (orderType.HasValue)
                    query = query.Where(o => o.OrderType == orderType.Value);

                if (date.HasValue)
                {
                    var startOfDay = date.Value.Date;
                    var endOfDay = startOfDay.AddDays(1);
                    query = query.Where(o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay);
                }

                var orders = await query
                    .OrderByDescending(o => o.OrderDate).Skip((pageNumber-1)*pageSize).Take(pageSize)
                    .ToListAsync();

                var responses = orders.Select(MapToSummaryResponse).ToList();
                return new SimpleResponse<List<OrderSummaryResponse>>(responses);
            }

        public async Task<SimpleResponse<OrderResponse>> GetByIdAsync(Guid id)
        {
            var order = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return new SimpleResponse<OrderResponse>("Sifariş tapılmadı");

            var response = MapToResponse(order);
            return new SimpleResponse<OrderResponse>(response);
        }

        public async Task<SimpleResponse<PurchaseDto>> CreatePaymentForOrderAsync(
            Guid orderId,
            string redirectUrl)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return new SimpleResponse<PurchaseDto>("Sifariş tapılmadı");

          
            if (order.IsPaid)
                return new SimpleResponse<PurchaseDto>("Sifariş artıq ödənilib");

            order.CalculateTotals(TAX_RATE);

         
            var purchaseDto = await _paymentService.CreatePaymentRequest(new OrderCreateDto
            {
                Amount = order.FinalAmount,
                Currency = "AZN",
                Description = $"Order {order.OrderNumber}",
                RedirectUrl = redirectUrl
            });

            
            var payment = new CloPosProject.Domain.Entities.Payment
            {
                PurchaseId = int.TryParse(purchaseDto.Order.Id.ToString(), out var pid) ? pid : 0,
                Password = purchaseDto.Order.Password,
                Secret = purchaseDto.Order.Secret,
                OrderId = order.Id,
                CreatedDate = DateTime.UtcNow,
                PaymentStatus = CloPosProject.Domain.Enums.PaymentStatus.Authorized
            };

            await _context.Set<CloPosProject.Domain.Entities.Payment>().AddAsync(payment);
            await _context.SaveChangesAsync();
            return new SimpleResponse<PurchaseDto>(purchaseDto);
        }
        public async Task<SimpleResponse<string>> VerifyAndCompletePaymentAsync(
    int purchaseId)
        {
            var payment = await _context.Set<CloPosProject.Domain.Entities.Payment>()
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);

            if (payment == null)
                return new SimpleResponse<string>("Ödəniş tapılmadı");


            var paymentStatus = await _paymentService.GetPaymentStatus(purchaseId);

            if (paymentStatus.IsSuccess)
            {
                payment.MarkAsPaid(paymentStatus.TransactionId);    
                payment.Order.MarkAsCompleted();

                await _context.SaveChangesAsync();

                return new SimpleResponse<string>("Ödəniş uğurla tamamlandı");
            }
            else
            {
                payment.MarkAsFailed();
                await _context.SaveChangesAsync();

                return new SimpleResponse<string>("Ödəniş uğursuz oldu");
            }
        }
        public async Task<SimpleResponse<OrderResponse>> GetByOrderNumberAsync(string orderNumber)
        {
            var order = await _context.Orders
                  .Include(o => o.OrderItems)
                      .ThenInclude(oi => oi.MenuItem)
                  .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

            if (order == null)
                return new SimpleResponse<OrderResponse>("Sifariş tapılmadı");

            var response = MapToResponse(order);
            return new SimpleResponse<OrderResponse>(response);
        }

        public async Task<SimpleResponse<List<OrderSummaryResponse>>> GetPendingTakeAwayOrdersAsync(int pageNumber = 1,
     int pageSize = 20)
        {
            var pendingStatuses = new[]
            {
                    OrderStatus.Pending,
                    OrderStatus.Confirmed,
                    OrderStatus.Preparing,
                    OrderStatus.Ready
                };

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderType == OrderType.TakeAway &&
                           pendingStatuses.Contains(o.Status) &&
                           !o.IsPickedUp)
                .OrderBy(o => o.PickupTime).Skip((pageNumber - 1) * pageSize).Take(pageSize)

                .ToListAsync();

            var responses = orders.Select(MapToSummaryResponse).ToList();
            return new SimpleResponse<List<OrderSummaryResponse>>(responses);
        }
        

        public async  Task<SimpleResponse<List<OrderSummaryResponse>>> GetTableOrdersAsync( Guid tableId, int pageNumber = 1,
    int pageSize = 20)
        {
            var activeStatuses = new[]
               {
                    OrderStatus.Pending,
                    OrderStatus.Confirmed,
                    OrderStatus.Preparing,
                    OrderStatus.Ready
                };

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.TableId == tableId && activeStatuses.Contains(o.Status))
                .OrderByDescending(o => o.OrderDate).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
                .ToListAsync();

            var responses = orders.Select(MapToSummaryResponse).ToList();
            return new SimpleResponse<List<OrderSummaryResponse>>(responses);
        }

        public async  Task<SimpleResponse<List<OrderSummaryResponse>>> GetTodayOrdersAsync(int pageNumber = 1,
     int pageSize = 20)
        {
            return await GetAllAsync(pageNumber ,pageSize ,null, null, DateTime.Today);
        }

        public async Task<SimpleResponse<string>> MarkAsPickedUpAsync(Guid orderId)
        {

            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return new SimpleResponse<string>("Sifariş tapılmadı");
            order.MarkAsPickedUp();
            await _context.SaveChangesAsync();
            return new SimpleResponse<string>("Sifariş götürüldü kimi qeyd edildi");
        }

        public async Task<SimpleResponse<string>> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {

            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return new SimpleResponse<string>("Sifariş tapılmadı");


            order.UpdateStatus(newStatus);
            await _context.SaveChangesAsync();



            return new SimpleResponse<string>("Status uğurla dəyişdirildi");
        }


        private OrderResponse MapToResponse(CloPosProject.Domain.Entities.Order order)
        {
            var items = order.OrderItems
     .Select(oi => new OrderItemResponse
     {
         MenuItemId = oi.MenuItemId,
         MenuItemName = oi.MenuItemName,
         Quantity = oi.Quantity,
         UnitPrice = oi.UnitPrice,
         TotalPrice = oi.Subtotal,
         Notes = oi.SpecialInstructions
     })
     .ToList();

            var response = new OrderResponse
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.OrderDate,
                Status = order.Status,
                OrderType = order.OrderType,

                TableId = order.TableId,
                WaiterId = order.WaiterId,
                TableName = order.TableNumber?.ToString(),

                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                DeliveryAddress = order.DeliveryAddress,
                DeliveryProvider = order.DeliveryProvider?.ToString(),
                DeliveryInstructions = order.DeliveryInstructions,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,

                PickupTime = order.PickupTime,
                IsPickedUp = order.IsPickedUp,

                SubTotal = order.TotalAmount,
                Tax = order.Tax,
                DiscountAmount = order.Discount,
                DeliveryFee = order.DeliveryFee,
                FinalAmount = order.FinalAmount,

                Notes = order.Notes,
                Items = items 
            };
            return response;
        }

        private OrderSummaryResponse MapToSummaryResponse(CloPosProject.Domain.Entities.Order order)
        {
            return new OrderSummaryResponse
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.OrderDate,
                Status = order.Status,
                OrderType = order.OrderType,
                FinalAmount = order.FinalAmount,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                TotalItemCount = order.OrderItems.Count,
                TableName = order.TableNumber?.ToString()
            };
        }
    }
}
