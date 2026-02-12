using CloPosProject.Application.DTOs.Authentication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Validations.User
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("Email must not exceed 100 characters");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            RuleFor(x => x.Password).MaximumLength(100).WithMessage("Password must not exceed 100 characters");
            RuleFor(x => x).Must(x => x.Password != null && !x.Password.Contains(" ")).WithMessage("Password must not contain spaces");

        }
    }
}
