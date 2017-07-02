using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SandwichClub.Api.Tests.Middleware
{
    public abstract class MiddlewareTestBase<T> : UnitTestBase<T> where T : class
    {
        /// <summary>
        /// True if the next RequestDelegate was called by the middleware
        /// </summary>
        protected bool NextDelegateCalled { get; private set; }

        protected HttpContext Context { get; } = new DefaultHttpContext();

        protected MiddlewareTestBase()
        {
            RequestDelegate next = (HttpContext context) => { NextDelegateCalled = true; return Task.CompletedTask; };

            ServiceProvider.RegisterInstance(next);
        }
    }
}
