using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class UserRepository : BaseRepository<int, User>, IUserRepository
    {
        public UserRepository(SC2Context context) : base(context)
        {
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            if (id == 0)
                return null;
            return await _dbSet.FirstOrDefaultAsync(u => u.UserId == id);
        }
    }
}
