using System.Threading.Tasks;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class UserService : BaseService<int, User, IUserRepository>, IUserService
    {
        public UserService(IUserRepository userRespository) : base(userRespository)
        {
        }

        public Task<User> GetBySocialId(string id)
        {
            return Repository.GetBySocialId(id);
        }
    }
}
