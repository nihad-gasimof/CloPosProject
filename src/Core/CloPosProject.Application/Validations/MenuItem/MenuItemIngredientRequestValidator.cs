using CloPosProject.Application.DTOs.MenuItem;
using FluentValidation;

namespace CloPosProject.Application.Validations.MenuItem
{
    public class MenuItemIngredientRequestValidator : AbstractValidator<MenuItemIngredientRequest>
    {
        public MenuItemIngredientRequestValidator()
        {
            RuleFor(x => x.IngredientId)
                .NotEmpty().WithMessage("IngredientId is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero");
        }
    }
}
