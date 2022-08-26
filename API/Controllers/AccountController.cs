using API.DTOs;
using API.Services;
using Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        

        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if(user == null) {
                return Unauthorized("Username is not recognized"); 
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return CreateUser(user,userRoles);
            }

            return Unauthorized("Incorrect Password");
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await _userManager.Users.AnyAsync(u => u.UserName == registerDto.Username)) {
                return BadRequest("Username already taken");
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                JobTitle = registerDto.JobTitle,
                Department = registerDto.Department
            };

            var result = await _userManager.CreateAsync(user,registerDto.Password);

            if(result.Succeeded) {
                
                await _userManager.AddToRoleAsync(user, registerDto.UserRole);
                IList<string> userRoles = new List<string> { registerDto.UserRole };

                return CreateUser(user,userRoles);
            }

            return BadRequest("Problem in registering user");
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Name));

            if (user == null) 
                return BadRequest("Problem in getting user");

            var userRoles  = await _userManager.GetRolesAsync(user);
            
            return CreateUser(user,userRoles);
        } 

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserDto> userDto = new List<UserDto>();
            foreach(var user in users)
            {
                userDto.Add(CreateUser(user,await _userManager.GetRolesAsync(user)));
            }
            return  userDto;
        }

        private UserDto CreateUser(AppUser user,IList<string> roles)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                JobTitle = user.JobTitle,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                Department = user.Department,
                Roles = new List<string>(roles)
            };
        }

    }
}
