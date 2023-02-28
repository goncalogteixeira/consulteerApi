using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulteer.Application.Features.Users.Commands.RegisterUser
{

    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress();

            RuleFor(p => p.Name).NotEmpty().MaximumLength(200);

            RuleFor(p => p.Password).NotEmpty().MaximumLength(20);
        }
    }
}
