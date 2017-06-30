using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using Xunit;

namespace SandwichClub.Api.Tests.Services
{
    public class WeekServiceTests
    {
        public class PaymentWeekServiceTests : UnitTestBase<WeekService>
        {
            private const int WeekId = 42;
            private Week Week => _weeks.FirstOrDefault(_ => _.WeekId == WeekId);
            private readonly List<Week> _weeks;
            private readonly List<WeekUserLink> _weekLinks;

            public PaymentWeekServiceTests()
            {
                _weeks = new List<Week> {new Week {WeekId = WeekId}};
                _weekLinks = new List<WeekUserLink>();

                Mock<IWeekRepository>().Setup(i => i.GetByIdAsync(It.IsAny<int>()))
                    .Returns((int id) => Task.FromResult(_weeks.FirstOrDefault(_ => _.WeekId == id)));

                Mock<IWeekUserLinkService>().Setup(i => i.GetByWeekIdAsync(WeekId))
                    .Returns((int id) => Task.FromResult(_weekLinks.Where(_ => _.WeekId == id)));
                Mock<IWeekUserLinkService>().Setup(i => i.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns((int id, bool ignore) => Task.FromResult(_weekLinks.Where(_ => _.UserId == id)));
                Mock<IWeekUserLinkService>().Setup(i => i.CountForWeekAsync(It.IsAny<int>()))
                    .Returns((int id) => Task.FromResult(_weekLinks.Count(_ => _.WeekId == id)));
            }

            public void AddWeekLinks(int number)
            {
                for (var i = 0; i < number; ++i)
                    _weekLinks.Add(new WeekUserLink { WeekId = WeekId, UserId = i });
            }

            public void AddWeekLinks(int number, int weekId)
            {
                for (var i = 0; i < number; ++i)
                    _weekLinks.Add(new WeekUserLink { WeekId = weekId, UserId = i });
            }

            [Theory]
            [InlineData(2, 10, 5)]
            [InlineData(10, 10, 1)]
            public async Task TestGetAmountToPayPerPersonAsync_CheckAmount(int users, double cost, decimal expectedPayment)
            {
                // Given
                Week.Cost = cost;
                AddWeekLinks(users);

                // When
                var calculatedPayment = await Service.GetAmountToPayPerPersonAsync(WeekId);

                // Verify
                Assert.Equal(expectedPayment, calculatedPayment);
            }

            [Theory]
            [InlineData(2, 10, 5)]
            [InlineData(10, 10, 1)]
            public async Task TestMarkAllLinksAsPaidForUserAsync(int users, double cost, double expectedPayment)
            {
                // Given
                var userId = 82;
                Week.Cost = cost;
                AddWeekLinks(users);
                _weekLinks.First().UserId = userId;

                // When
                var result = await Service.MarkAllLinksAsPaidForUserAsync(1);

                // Verify
                var paidLinks = result.ToList();
                Assert.Equal(1, paidLinks.Count);
                var link = paidLinks.First();

                Assert.Equal(expectedPayment, link.Paid, 3);
                Mock<IWeekUserLinkService>().Verify(i => i.SaveAsync(It.IsAny<WeekUserLink>()), Times.Once);
            }

            [Fact]
            public async Task TestGetTotalCostsForUserAsync()
            {
                // Given
                const int userId = 4;

                // User 4 signed up to week 1 & 3
                AddWeekLinks(5, 1);
                AddWeekLinks(2, 2);
                AddWeekLinks(9, 3);

                // Weeks
                _weeks.Add(new Week {WeekId = 1, Cost = 10}); // 10/5 = $2
                _weeks.Add(new Week {WeekId = 2, Cost = 5}); // 5/2 = $2.5
                _weeks.Add(new Week {WeekId = 3, Cost = 65}); // 65/9 = $7.22



                // When
                var cost = await Service.GetTotalCostsForUserAsync(userId);

                // Verify
                Assert.Equal(9.22m, cost, 2); // Should be accurate to 2dp
            }
        }

        public class StaticWeekServiceTests : UnitTestBase<WeekService>
        {
            private static readonly DateTime StartOfComputerTime = new DateTime(1970, 1, 1);

            [Fact]
            public void GetWeekId_GivenStartOfComputerTime_IdShouldBeZero()
            {
                // When
                var weekId = Service.GetWeekId(new DateTime(1970, 1, 5));

                // Verify
                Assert.Equal(0, weekId);
            }

            [Fact]
            public void GetWeekId_GivenStartOfComputerTimeAndOneWeek_IdShouldBeOne()
            {
                // When
                var weekId = Service.GetWeekId(StartOfComputerTime.AddDays(7));

                // Verify
                Assert.Equal(1, weekId);
            }

            [Fact]
            public void GetWeekId_GivenDaysMonToSun_IdShouldNotChange()
            {
                // Given
                // Get a week which should match id 1
                var date = StartOfComputerTime.AddDays(7);
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

        public class RepositoryWeekServiceTests : UnitTestBase<WeekService>
        {
            private const int WeekId = 42;

            private readonly IWeekRepository _weekRepo;

            private Week InsertedWeek => new Week {WeekId = WeekId, ShopperUserId = 1, Cost = 10.0};
            private Week UpdatedWeek => new Week {WeekId = WeekId, ShopperUserId = 2, Cost = 20.0};
            private Week DefaultWeek => new Week {WeekId = WeekId, ShopperUserId = null, Cost = 0.0};

            public RepositoryWeekServiceTests()
            {
                _weekRepo = GetRepository<WeekRepository>(); 
            }

            [Fact]
            public async void GetByIdAsync_ShouldProvideDefaultWeek()
            {
                // When
                var week = await Service.GetByIdAsync(WeekId);

                // Verify
                Assert.Equal(WeekId, week.WeekId);
                Assert.Equal(0.0, week.Cost);
                Assert.False(week.ShopperUserId.HasValue);
            }

            [Fact]
            public async void InsertAsync_ShouldInsert()
            {
                // When
                await Service.InsertAsync(InsertedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(InsertedWeek, week);
            }

            [Fact]
            public async void InsertAsync_WhenInsertingAgain_ShouldUpdate()
            {
                // Given
                await _weekRepo.InsertAsync(InsertedWeek);

                // When
                await Service.InsertAsync(UpdatedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(UpdatedWeek, week);
            }

            [Fact]
            public async void InsertAsync_WhenInsertingDefault_ShouldDelete()
            {
                // Given
                await Service.InsertAsync(InsertedWeek);

                // When
                await Service.InsertAsync(DefaultWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                Assert.Null(week);
            }

            [Fact]
            public async void UpdateAsync_ShouldInsert()
            {
                // When
                await Service.UpdateAsync(InsertedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(InsertedWeek, week);
            }

            [Fact]
            public async Task UpdateAsync_WhenUpdatingAgain_ShouldUpdate()
            {
                // Given
                await _weekRepo.InsertAsync(InsertedWeek);

                // When
                await Service.UpdateAsync(UpdatedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(UpdatedWeek, week);
            }

            [Fact]
            public async void UpdateAsync_WhenInsertingDefault_ShouldDelete()
            {
                // Given
                await Service.InsertAsync(InsertedWeek);

                // When
                await Service.UpdateAsync(DefaultWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                Assert.Null(week);
            }

            private void WeeksEqual(Week expected, Week actual)
            {
                Assert.Equal(expected.WeekId, actual.WeekId);
                Assert.Equal(expected.ShopperUserId, actual.ShopperUserId);
                Assert.Equal(expected.Cost, actual.Cost);
            }
        }
    }
}
