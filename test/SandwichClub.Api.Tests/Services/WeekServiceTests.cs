using System;
using System.Threading.Tasks;
using Moq;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using Xunit;

namespace SandwichClub.Api.Tests.Services
{
    public class WeekServiceTests : UnitTestBase<WeekService>
    {
        private static readonly DateTime startOfComputerTime = new DateTime(1970, 1, 1);

        [Fact]
        public void GetWeekId_GivenStartOfComputerTime_IdShouldBeZero()
        {
            // When
            var weekId = Service.GetWeekId(startOfComputerTime);

            // Verify
            Assert.Equal(0, weekId);
        }

        [Fact]
        public void GetWeekId_GivenStartOfComputerTimeAndOneWeek_IdShouldBeOne()
        {
            // When
            var weekId = Service.GetWeekId(startOfComputerTime.AddDays(7));

            // Verify
            Assert.Equal(1, weekId);
        }

        [Fact]
        public void GetWeekId_GivenDaysMonToSun_IdShouldNotChange()
        {
            // Given
            // Get a week which should match id 1
            var date = startOfComputerTime.AddDays(7);
            // Translate to the monday of that week
            date = date.AddDays((int)DayOfWeek.Monday-(int)date.DayOfWeek);
            var originalId = Service.GetWeekId(date);

            for (var i = 1; i < 7; ++i)
            {
                // When
                var weekId = Service.GetWeekId(date.AddDays(i));

                // Verify
                Assert.Equal(originalId, weekId);
            }
        }

        [Fact]
        public async void GetCurrentWeek_ShouldUseWeekIdForToday()
        {
            // Given
            var weekId = Service.GetWeekId(DateTime.Today);
            var repo = Mock<IWeekRepository>();
            repo.Setup(i => i.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<Week>(null));

            // When
            await Service.GetCurrentWeekAsync();

            // Verify
            repo.Verify(i => i.GetByIdAsync(weekId), Times.Once);
        }
    }
}
