using Consulteer.API.Seeds;
using Consulteer.Application.DTO;
using Consulteer.Application.Features.Users.Commands.LoginUser;
using Consulteer.Application.Features.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consulteer.API.Controllers
{

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMediator _mediatr;


        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mediatr = mediator;
        }


        [Authorize("Permission.CanViewAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allUsersExceptCurrentUser = await _userManager.Users.ToListAsync();
            return Ok(allUsersExceptCurrentUser);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Me()
        {
            var currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            return Ok(new UserDTO { Email = currentUser.Email, Id = currentUser.Id, Name = currentUser.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> Seed()
        {
            await DefaultRoles.SeedAsync(_userManager, _roleManager);
            await DefaultRoles.SeedBasicUserAsync(_userManager, _roleManager);
            await DefaultRoles.SeedSuperAdminAsync(_userManager, _roleManager);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUserToken(string email, string password)
        {
            var request = new LoginUserCommand(email, password);
            var result = await _mediatr.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(string email, string name, string password)
        {
            var request = new RegisterUserCommand(email, name, password);
            var result = await _mediatr.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
