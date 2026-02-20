using CloPosProject.Application.DTOs.MenuItem;
using FluentValidation;

namespace CloPosProject.Application.Validations.MenuItem
{
    public class CreateMenuItemValidator : AbstractValidator<CreateMenuItem>
    {
        public CreateMenuItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(x => x.PreparationTime)
                .GreaterThan(0).WithMessage("PreparationTime must be greater than zero");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");


            RuleForEach(x => x.Ingredients)
                .SetValidator(new MenuItemIngredientRequestValidator());
        }
    }
}
