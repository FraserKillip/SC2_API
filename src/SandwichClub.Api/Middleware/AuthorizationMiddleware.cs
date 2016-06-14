using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SC2_API.AuthorisationMiddleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Headers.Keys.Contains("Sandwich-Auth-Token"))
            {
                context.Response.Headers.Add("Has token", "true");
            }

            await _next.Invoke(context);
        }
    }
}