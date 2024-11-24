using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabt.Core.Entities;

namespace Talabat.API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);
            return user;
        }
    } 
}
