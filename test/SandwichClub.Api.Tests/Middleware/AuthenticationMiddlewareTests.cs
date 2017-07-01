using Microsoft.Extensions.Options;
using Moq;
using SandwichClub.Api.Middleware;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using System.Threading.Tasks;
using Xunit;

namespace SandwichClub.Api.Tests.Middleware
{
    public class AuthenticationMiddlewareTests : MiddlewareTestBase<AuthenticationMiddleware>
    {
        private IOptions<AuthenticationMiddlewareConfig> MiddlewareOptions { get; } = Options.Create(new AuthenticationMiddlewareConfig());
        private IScSession Session => Mock<IScSession>().Object;
        private IAuthorisationService AuthService => Mock<IAuthorisationService>().Object;

        public AuthenticationMiddlewareTests()
        {
            Mock<IAuthorisationService>().Setup(i => i.CanAuthorise(It.IsAny<string>())).Returns((string token) => Task.FromResult(token.StartsWith("facebook ")));

            Mock<IScSession>().SetupAllProperties();
        }

        [Fact]
        public async Task TestAuthenticate_WhenNoTokenIsProvided_ShouldNotSetUser()
        {
            // When
            await Service.Authenticate(Context, MiddlewareOptions, Session, AuthService);

            // Verify
            Assert.Null(Session.CurrentUser);
            Assert.False(Session.InvalidToken);
        }

        [Fact]
        public async Task TestAuthenticate_WhenInvalidAuthProviderIsProvided_ShouldNotSetUser()
        {
            // Given
            Context.Request.Headers["Sandwich-Auth-Token"] = "custom aaaabbbbccccdddd";

            // When
            await Service.Authenticate(Context, MiddlewareOptions, Session, AuthService);

            // Verify
            Assert.Null(Session.CurrentUser);
            Assert.True(Session.InvalidToken);
        }

        [Fact]
        public async Task TestAuthenticate_WhenInvalidTokenIsProvided_ShouldNotSetUser()
        {
            // Given
            Context.Request.Headers["Sandwich-Auth-Token"] = "facebook aaaabbbbccccdddd";

            // When
            await Service.Authenticate(Context, MiddlewareOptions, Session, AuthService);

            // Verify
            Assert.Null(Session.CurrentUser);
            Assert.True(Session.InvalidToken);
        }

        [Fact]
        public async Task TestAuthenticate_WhenValidTokenIsProvided_ShouldSetUser()
        {
            // Given
            const string token = "facebook aaaabbbbccccdddd";
            Context.Request.Headers["Sandwich-Auth-Token"] = token;
            Mock<IAuthorisationService>().Setup(i => i.Authorise(token)).Returns(Task.FromResult(new User()));

            // When
            await Service.Authenticate(Context, MiddlewareOptions, Session, AuthService);

            // Verify
            Assert.NotNull(Session.CurrentUser);
        }
    }
}
