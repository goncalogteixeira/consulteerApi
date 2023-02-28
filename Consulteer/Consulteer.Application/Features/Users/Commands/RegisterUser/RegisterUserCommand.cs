using AspNetCoreHero.Results;
using Consulteer.API.Constants;
using Consulteer.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Consulteer.Application.Features.Users.Commands.RegisterUser
{
    public partial class RegisterUserCommand : IRequest<Result<UserDTO>>
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public RegisterUserCommand(
            string email
            , string name
            , string password
            )
        {
            Email = email;
            Name = name;
            Password = password;
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserDTO>>
    {

        private readonly UserManager<IdentityUser> _userManager;


        public RegisterUserCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<UserDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var defaultUser = new IdentityUser
            {
                UserName = request.Name,
                Email = request.Email,
                EmailConfirmed = true
            };
            var user = await _userManager.FindByEmailAsync(defaultUser.Email);
            if (user != null)
            {
                return await Result<UserDTO>.FailAsync("Email already exists");
            }
            await _userManager.CreateAsync(defaultUser, request.Password);
            await _userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());

            return await Result<UserDTO>.SuccessAsync(new UserDTO { Email = defaultUser.Email, Name = defaultUser.UserName, Id = defaultUser.Id });

        }
    }
}
