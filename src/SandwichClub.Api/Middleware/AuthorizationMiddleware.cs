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
        private readonly ConcurrentDictionary<string, UserAuthItem> _tokenCache = new ConcurrentDictionary<string, UserAuthItem>();

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var config = context.RequestServices.GetService<IOptions<AuthorizationMiddlewareConfig>>();
            var session = context.RequestServices.GetService<IScSession>();
            var authorisationService = context.RequestServices.GetService<IAuthorisationService>();
            session.WithContext(context);

            if (config.Value.IgnoreAuth)
            {
                var userService = context.RequestServices.GetService<IUserService>();
                session.CurrentUser = (await userService.GetAsync()).FirstOrDefault();
                await _next(context);
                return;
            }

            if (!context.Request.Headers.Keys.Contains("Sandwich-Auth-Token"))
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Missing Sandwich-Auth-Token header");
                return;
            }

            var token = context.Request.Headers["Sandwich-Auth-Token"];

            UserAuthItem authItem;
            if (_tokenCache.TryGetValue(token, out authItem))
            {
                if (DateTime.Now.Subtract(authItem.CacheTime).TotalMinutes > 30)
                    _tokenCache.TryRemove(token, out authItem);
                else
                    session.CurrentUser = authItem.User;
            }
            else
            {
                if (await authorisationService.CanAuthorise(token))
                {
                    var user = await authorisationService.Authorise(token);
                    if (user != null)
                    {
                        authItem = new UserAuthItem {CacheTime = DateTime.Now, User = user};
                        _tokenCache.TryAdd(token, authItem);
                        session.CurrentUser = user;
                    }
                }
            }

            if (session.CurrentUser != null)
                await _next.Invoke(context);
            else
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid Sandwich-Auth-Token header");
            }
        }

        private class UserAuthItem
        {
            public User User { get; set; }
            public DateTime CacheTime { get; set; }
        }
    }

    public class AuthorizationMiddlewareConfig
    {
        public bool IgnoreAuth { get; set; }
    }
}