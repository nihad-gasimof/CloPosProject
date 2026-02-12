using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Domain.Entities;
using FluentValidation;

namespace CloPosProject.Application.Validations.User
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Username).MaximumLength(40).WithMessage("UserName must not exceed 40 characters");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("Email must not exceed 100 characters");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");

                RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long");
                RuleFor(x => x.Password).MaximumLength(100).WithMessage("Password must not exceed 100 characters");
                RuleFor(x => x.Name).MaximumLength(40).WithMessage("Name must not exceed 40 characters");
                RuleFor(x => x.Surname).MaximumLength(40).WithMessage("Surname must not exceed 40 characters");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
            RuleFor(x => x).Must(x => x.Password != null && !x.Password.Contains(" ")).WithMessage("Password must not contain spaces");
        }
    }
}
