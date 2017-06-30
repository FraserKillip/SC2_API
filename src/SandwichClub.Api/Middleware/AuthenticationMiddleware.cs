using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using Microsoft.Extensions.Primitives;

namespace SandwichClub.Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, UserAuthItem> _tokenCache = new ConcurrentDictionary<string, UserAuthItem>();

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var config = context.RequestServices.GetService<IOptions<AuthenticationMiddlewareConfig>>();
            var session = context.RequestServices.GetService<IScSession>();
            var authorisationService = context.RequestServices.GetService<IAuthorisationService>();
            session.WithContext(context);

            await Authenticate(context, config, session, authorisationService);
            await _next(context);
        }

        internal async Task Authenticate(HttpContext context, IOptions<AuthenticationMiddlewareConfig> config, IScSession session, IAuthorisationService authorisationService)
        {
            if (config.Value.IgnoreAuth)
            {
                var userService = context.RequestServices.GetService<IUserService>();
                session.CurrentUser = (await userService.GetAsync()).FirstOrDefault();
                return;
            }

            var ntoken = GetToken(context);

            if (!ntoken.HasValue || ntoken.Value == default(string))
            {
                // Missing token
                return;
            }

            var token = ntoken.Value;

            if (_tokenCache.TryGetValue(token, out UserAuthItem authItem))
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
                        authItem = new UserAuthItem { CacheTime = DateTime.Now, User = user };
                        _tokenCache.TryAdd(token, authItem);
                        session.CurrentUser = user;
                    }
                }
            }

            if (session.CurrentUser == null)
                session.InvalidToken = true;
        }

        internal StringValues? GetToken(HttpContext context)
        {
            var headers = context.Request.Headers;

            if (!headers.Keys.Contains("Sandwich-Auth-Token") && !headers.Keys.Contains("sandwich-auth-token"))
                return null;

            return context.Request.Headers.Keys.Contains("Sandwich-Auth-Token")
                ? context.Request.Headers["Sandwich-Auth-Token"]
                : context.Request.Headers["sandwich-auth-token"];
        }

        private class UserAuthItem
        {
            public User User { get; set; }
            public DateTime CacheTime { get; set; }
        }
    }

    public class AuthenticationMiddlewareConfig
    {
        public bool IgnoreAuth { get; set; }
    }
}
