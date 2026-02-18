using CloPosProject.Application.DTOs.Ingredient;
using FluentValidation;

namespace CloPosProject.Application.Validations.Ingredient
{
    public class CreateIngredientDtoValidator : AbstractValidator<CreateIngredientDto>
    {
        public CreateIngredientDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("MinimumStock must be non-negative");

            RuleFor(x => x.InitialQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("InitialQuantity must be non-negative");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative");
        }
    }
}
