using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Middleware
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
            var session = context.RequestServices.GetService<IScSession>();

            if (session.CurrentUser == null)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync($"{(session.InvalidToken ? "Invalid" : "Missing")} Sandwich-Auth-Token header");
                return;
            }

            await _next(context);
        }
    }
}