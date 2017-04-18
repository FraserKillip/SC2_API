using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class UserService : BaseService<int, User, IUserRepository>, IUserService
    {
        public UserService(IUserRepository userRespository, ILogger<UserService> logger) : base(userRespository, logger)
        {
        }

        public Task<User> GetBySocialId(string id)
        {
            return Repository.GetBySocialId(id);
        }

        public async Task<User> GetPrimaryShopperAsync()
        {
            var shoppers = await Repository.GetShoppersAsync();
            var primaryShopper = shoppers.FirstOrDefault();

            return primaryShopper;
        }

        public override int GetId(User user)
        {
            return user.UserId;
        }
    }
}
