using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulteer.Application.Features.Users.Commands.LoginUser
{

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress();

            RuleFor(p => p.Password).NotEmpty().MaximumLength(20);
        }
    }
}
