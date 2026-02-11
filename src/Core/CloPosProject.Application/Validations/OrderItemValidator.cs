using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative");
            RuleFor(x => x.SpecialInstructions).MaximumLength(1000).WithMessage("SpecialInstructions must not exceed 1000 characters");
        }
    }
}
