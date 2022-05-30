using WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
