using CloPosProject.Application.DTOs.Order;
using FluentValidation;
using System;

namespace CloPosProject.Application.Validations.Order
{
    public class CreateTakeAwayOrderRequestValidator : AbstractValidator<CreateTakeAwayOrderRequest>
    {
        public CreateTakeAwayOrderRequestValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.CustomerPhone).NotEmpty();
            RuleFor(x => x.PickupTime).GreaterThanOrEqualTo(DateTime.UtcNow).When(x => x.PickupTime.HasValue).WithMessage("PickupTime must be in the future");
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());
        }
    }
}
