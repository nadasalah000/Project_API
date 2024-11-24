using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.API.Helper;
using Talabt.Core.Repositories;
using Talabt.Core.Services;
using Talabt.Repository;
using Talabt.Services;

namespace Talabat.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection  AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
           // Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                    .SelectMany(P => P.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();
                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });
            return Services;
        }
    }
}
