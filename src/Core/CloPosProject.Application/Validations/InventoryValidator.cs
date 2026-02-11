using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class InventoryValidator : AbstractValidator<Inventory>
    {
        public InventoryValidator()
        {
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be non-negative");
            RuleFor(x => x.AverageUnitPrice).GreaterThanOrEqualTo(0).WithMessage("AverageUnitPrice must be non-negative");
        }
    }
}
