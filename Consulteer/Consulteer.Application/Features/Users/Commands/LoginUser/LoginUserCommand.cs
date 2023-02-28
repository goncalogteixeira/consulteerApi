using AspNetCoreHero.Results;
using Consulteer.API.Constants;
using Consulteer.Application.DTO;
using Consulteer.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Consulteer.Application.Features.Users.Commands.LoginUser
{
    public partial class LoginUserCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }


        public string Password { get; set; }

        public LoginUserCommand(
            string email
            , string password
            )
        {
            Email = email;
            Password = password;
        }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenServices _tokenService;

        public LoginUserCommandHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenServices tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return await Result<string>.FailAsync("User does not exist");
            }
            var signIn = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!signIn.Succeeded)
            {
                return await Result<string>.FailAsync("Login for this user failed");
            }
            var role = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            var token = _tokenService.GenerateToken(user.Email, role.Any() ? role.FirstOrDefault() : "", claims);

            return await Result<string>.SuccessAsync(token);

        }
    }
}
