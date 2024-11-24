using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabt.Core.Services;

namespace Talabat.API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSeconds;

        public CachedAttribute(int ExpireTimeInSeconds)
        {
            _expireTimeInSeconds = ExpireTimeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CasheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
          var CachedResponse = await CacheService.GetCachedResponse(CasheKey);
            if(!string.IsNullOrEmpty(CachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var ExecutedEndPointContext = await next.Invoke();
            if(ExecutedEndPointContext.Result is OkObjectResult result)
            {
                await CacheService.CacheResponseAsync(CasheKey, result.Value, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach(var (key,value) in request.Query.OrderBy(X=>X.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
