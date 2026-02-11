using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class InventoryTransactionValidator : AbstractValidator<InventoryTransaction>
    {
        public InventoryTransactionValidator()
        {
            RuleFor(x => x.Type).IsInEnum().WithMessage("Type is required");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative");
            RuleFor(x => x.SupplierName).MaximumLength(500).WithMessage("SupplierName must not exceed 500 characters");
            RuleFor(x => x.InvoiceNumber).MaximumLength(200).WithMessage("InvoiceNumber must not exceed 200 characters");
        }
    }
}
