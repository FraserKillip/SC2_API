using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class UserRepository : BaseRepository<int, User>, IUserRepository
    {
        public UserRepository(ScContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            if (id == 0)
                return null;
            return await DbSet.FirstOrDefaultAsync(u => u.UserId == id);
        }
        
        public override async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await DbSet.Where(u => ids.Contains(u.UserId)).ToListAsync();
        }

        public async Task<User> GetBySocialId(string id)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.FacebookId == id);
        }
    }
}
