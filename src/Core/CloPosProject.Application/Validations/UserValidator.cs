using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).MaximumLength(256).WithMessage("UserName must not exceed 256 characters");
            RuleFor(x => x.Email).MaximumLength(256).WithMessage("Email must not exceed 256 characters");
        }
    }
}
