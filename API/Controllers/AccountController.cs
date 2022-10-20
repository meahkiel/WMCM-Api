using API.DTOs;
using API.Services;
using Core.Users;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Authorize(Roles = "admin,manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly DataContext _dataContext;

        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            TokenService tokenService,DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _dataContext = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDto loginDto) {
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
            var user = new AppUser();
            IdentityResult result;
            if (!String.IsNullOrEmpty(registerDto.Id))
            {
                user = await _userManager.FindByIdAsync(registerDto.Id);
                if(user == null)
                    return BadRequest("User is not recognized");
                
                user.Department = registerDto.Department;
                user.Email = registerDto.Email;
                user.JobTitle = registerDto.JobTitle;
                if(!string.IsNullOrEmpty(registerDto.Password)) {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,registerDto.Password);
                }
                result = await _userManager.UpdateAsync(user);
            }
            else
            {
                if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.Username)) {
                    return BadRequest("Username already taken");
                }
                user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                    JobTitle = registerDto.JobTitle,
                    Department = registerDto.Department
                };
                result = await _userManager.CreateAsync(user, registerDto.Password);
            }
            

            if(result.Succeeded) {
                
                await _userManager.AddToRoleAsync(user, registerDto.Role);
                IList<string> userRoles = new List<string> { registerDto.Role };

                return CreateUser(user,userRoles);
            }

            return BadRequest("Problem in registering user");
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(RegisterDto register)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(register.Username);
                if (user == null)
                    throw new Exception("User cannot find");
                
                await _userManager.UpdateAsync(user);

                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
                
        }
        
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
                Id = user.Id,
                DisplayName = user.DisplayName,
                JobTitle = user.JobTitle,
                Email = user.Email, 
                Token = _tokenService.CreateToken(user,roles[0]),
                Username = user.UserName,
                Department = user.Department,
                Roles = new List<string>(roles)
            };
        }

    }
}
