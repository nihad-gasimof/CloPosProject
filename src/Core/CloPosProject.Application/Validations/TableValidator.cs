using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class TableValidator : AbstractValidator<Table>
    {
        public TableValidator()
        {
            RuleFor(x => x.TableNumber)
                .NotEmpty().WithMessage("TableNumber is required")
                .MaximumLength(50).WithMessage("TableNumber must not exceed 50 characters");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than zero");
        }
    }
}
