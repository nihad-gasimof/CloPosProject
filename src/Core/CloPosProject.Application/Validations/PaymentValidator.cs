using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero");
            RuleFor(x => x.TransactionId).MaximumLength(200).WithMessage("TransactionId must not exceed 200 characters");
        }
    }
}
