using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ScheduleAuth.API.Filters
{
    public class InternalApiKeyAttribute : Attribute, IAsyncActionFilter
    { 
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var chaveEsperada = config["Internal:ApiKey"];

            var temHeader = context.HttpContext.Request.Headers.TryGetValue("X-Internal-Api-Key", out var chaveRecebida);

            if (!temHeader || chaveRecebida != chaveEsperada)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
