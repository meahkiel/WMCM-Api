using API.DTOs;
using API.Services;
using Core.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            TokenService tokenService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _userManager.Users
               .FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user == null) return Unauthorized("Invalid Email");
            
            
            var result = await _signInManager
                                .CheckPasswordSignInAsync(user, login.Password,false);
            if(result.Succeeded)
            {
                await SetRefreshToken(user);
                return CreateUserObject(user);
            }

            return Unauthorized("Invalid Password");
        }


        #region private method
        private async Task SetRefreshToken(AppUser appUser)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();
            appUser.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(appUser);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
        #endregion



    }
}
