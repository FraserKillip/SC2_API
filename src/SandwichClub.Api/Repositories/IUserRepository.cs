
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public interface IUserRepository : IBaseRepository<int, User>
    {
        Task<User> GetBySocialId(string id);
    }
}
