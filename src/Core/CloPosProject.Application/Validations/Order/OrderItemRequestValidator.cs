using CloPosProject.Application.DTOs.Order;
using FluentValidation;

namespace CloPosProject.Application.Validations.Order
{
    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemRequestValidator()
        {
            RuleFor(x => x.MenuItemId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
