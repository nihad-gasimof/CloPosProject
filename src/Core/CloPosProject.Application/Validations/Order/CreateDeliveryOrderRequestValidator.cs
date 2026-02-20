using CloPosProject.Application.DTOs.Order;
using FluentValidation;

namespace CloPosProject.Application.Validations.Order
{
    public class CreateDeliveryOrderRequestValidator : AbstractValidator<CreateDeliveryOrderRequest>
    {
        public CreateDeliveryOrderRequestValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.CustomerPhone).NotEmpty();
            RuleFor(x => x.DeliveryAddress).NotEmpty();
            RuleFor(x => x.DeliveryFee).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());
        }
    }
}
