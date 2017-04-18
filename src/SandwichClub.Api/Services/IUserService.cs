using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IUserService : IBaseService<int, User>
    {
        Task<User> GetBySocialId(string id);
        Task<User> GetPrimaryShopperAsync();
    }
}
