using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class SettingsValidator : AbstractValidator<Settings>
    {
        public SettingsValidator()
        {
            RuleFor(x => x.RestaurantName).MaximumLength(500).WithMessage("RestaurantName must not exceed 500 characters");
            RuleFor(x => x.Address).MaximumLength(1000).WithMessage("Address must not exceed 1000 characters");
            RuleFor(x => x.Phone).MaximumLength(100).WithMessage("Phone must not exceed 100 characters");
            RuleFor(x => x.TaxRate).GreaterThanOrEqualTo(0).WithMessage("TaxRate must be non-negative");
            RuleFor(x => x.Currency).MaximumLength(50).WithMessage("Currency must not exceed 50 characters");
        }
    }
}
