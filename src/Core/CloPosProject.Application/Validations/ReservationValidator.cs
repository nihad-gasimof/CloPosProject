using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class ReservationValidator : AbstractValidator<Reservation>
    {
        public ReservationValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("CustomerName is required").MaximumLength(200).WithMessage("CustomerName must not exceed 200 characters");
            RuleFor(x => x.CustomerPhone).MaximumLength(50).WithMessage("CustomerPhone must not exceed 50 characters");
            RuleFor(x => x.CustomerEmail).MaximumLength(200).WithMessage("CustomerEmail must not exceed 200 characters");
            RuleFor(x => x.GuestCount).GreaterThan(0).WithMessage("GuestCount must be greater than zero");
        }
    }
}
