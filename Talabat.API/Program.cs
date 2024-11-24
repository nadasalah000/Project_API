using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.API.Helper;
using Talabat.API.MiddleWares;
using Talabt.Core.Entities;
using Talabt.Core.Repositories;
using Talabt.Repository;
using Talabt.Repository.Data;
using Talabt.Repository.Identity;

namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region ConfigureServices
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<MyDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
           
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            builder.Services.AddApplicationServices();
            
            builder.Services.AddIdentityServices();


            #endregion

            var app = builder.Build();
            #region Update Database
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = Services.GetService<MyDbContext>();
                await DbContext.Database.MigrateAsync();
                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();

                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

                await MyDbContextSeed.SeedAsync(DbContext);

            }
            catch (Exception ex)
            { 
            var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Appling The Migration");
            }
            #endregion

            #region Kestral Pipliens
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            #endregion
            app.Run();
        }
    }
}
