using SandwichClub.Api.Middleware;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SandwichClub.Api.Tests.Middleware
{
    public class AuthorizationMiddlewareTests : MiddlewareTestBase<AuthorizationMiddleware>
    {
        private IScSession _session;

        public AuthorizationMiddlewareTests()
        {
            Mock<IScSession>().SetupAllProperties();
            _session = Mock<IScSession>().Object;
        }

        [Fact]
        public async Task TestInvoke_WhenUserAuthenticated_ShouldNotModifyResponse()
        {
            // Given
            _session.CurrentUser = new Moq.Mock<User>().Object;

            // When
            await Service.Invoke(Context, _session);

            // Verify
            Assert.False(Context.Response.HasStarted);

        }

        [Fact]
        public async Task TestInvoke_WhenUserNotAuthenticated_ShouldNotContine()
        {
            // Given
            _session.CurrentUser = null;

            // When
            await Service.Invoke(Context, _session);

            // Verify
            Assert.False(NextDelegateCalled);
        }


        [Fact]
        public async Task TestInvoke_WhenUserNotAuthenticated_ShouldHaveUnauthorizedStatusCode()
        {
            // Given
            _session.CurrentUser = null;

            // When
            await Service.Invoke(Context, _session);

            // Verify
            Assert.Equal((int)HttpStatusCode.Unauthorized, Context.Response.StatusCode);
        }
    }
}
