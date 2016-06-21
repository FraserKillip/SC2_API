using System.Threading.Tasks;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IUserService : IBaseService<int, UserDto>
    {
        Task<User> GetBySocialId(string id);
    }
}
