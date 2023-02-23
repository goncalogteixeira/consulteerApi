using Consulteer.API.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consulteer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
        {
            private readonly UserManager<IdentityUser> _userManager;
            public UsersController(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }

       
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
                return Ok(allUsersExceptCurrentUser);
            }

        [HttpGet]
        public async Task<IActionResult> Me()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User); 
            return Ok(currentUser);
        }


        [HttpPost]
        public async Task<IActionResult> Register(string email, string name, string password)
        {
            var defaultUser = new IdentityUser
            {
                UserName = name,
                Email = email,
                EmailConfirmed = true
            };
            if (_userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, password);
                    await _userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    return Ok(defaultUser);
                }
            }
            return Problem();
        }
    }
    
}
