using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Talabt.Core.Entities;
using Talabt.Core.Services;
using Talabt.Repository.Identity;
using Talabt.Services;

namespace Talabat.API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddScoped<ITokenServices, TokenServices>();
            Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            return Services;
        }
    }
}
