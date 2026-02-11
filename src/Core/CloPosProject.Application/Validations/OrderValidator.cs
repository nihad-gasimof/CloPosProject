using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("OrderNumber is required")
                .MaximumLength(100).WithMessage("OrderNumber must not exceed 100 characters");

            RuleFor(x => x.SubTotal).GreaterThanOrEqualTo(0).WithMessage("SubTotal must be non-negative");
            RuleFor(x => x.Tax).GreaterThanOrEqualTo(0).WithMessage("Tax must be non-negative");
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount must be non-negative");
            RuleFor(x => x.Total).GreaterThanOrEqualTo(0).WithMessage("Total must be non-negative");
        }
    }
}
