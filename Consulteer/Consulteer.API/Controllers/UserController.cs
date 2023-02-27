using Consulteer.API.Constants;
using Consulteer.API.Seeds;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace Consulteer.API.Controllers
{

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly SignInManager<IdentityUser> _signInManager;
            private readonly IConfiguration _configuration;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _signInManager = signInManager;
                _configuration = configuration;
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
            var currentUser = await _userManager.GetUserAsync(HttpContext.User); 
            return Ok(currentUser);
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
            var user = await _userManager.FindByEmailAsync(email);
            var signIn = await _signInManager.PasswordSignInAsync(user, password, false, false);
            var role = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            return Ok(GenerateToken(user, role.FirstOrDefault(), claims));
        }

        [AllowAnonymous]
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
        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private string GenerateToken(IdentityUser user, string role, IList<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, role));
               
               
            
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }

}
