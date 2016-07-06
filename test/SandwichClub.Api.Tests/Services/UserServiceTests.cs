using System.Threading.Tasks;
using Moq;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using Xunit;

namespace SandwichClub.Api.Tests.Services
{
    public class UserServiceTests : UnitTestBase<UserService>
    {
        [Fact]
        public void TestGetBySocialId()
        {
            // Given
            var repo = Mock<IUserRepository>();

            var socialId = "test";
            repo.Setup(i => i.GetBySocialId(It.IsAny<string>())).Returns(Task.FromResult<User>(null));

            // When
            Service.GetBySocialId(socialId);

            // Verify
            repo.Verify(i => i.GetBySocialId(socialId), Times.Once);
        }
    }
}
