using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class IngredientValidator : AbstractValidator<Ingredient>
    {
        public IngredientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.NameAz)
                .MaximumLength(200).WithMessage("NameAz must not exceed 200 characters");

            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("CurrentStock must be non-negative");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("MinimumStock must be non-negative");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative");
        }
    }
}
