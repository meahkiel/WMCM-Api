using Application.Interface;
using Core.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserAccessorService : IUserAccessorService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public UserAccessorService(IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public async Task<IList<string>> GetUserRole()
        {
            string userName =  _httpContextAccessor
                                    .HttpContext.User
                                    .FindFirstValue(ClaimTypes.Name);

            return await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(userName));
        }
    }
}
