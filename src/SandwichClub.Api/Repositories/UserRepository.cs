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
        
        public override async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await ExecuteAsync(async (dbSet) => await dbSet.Where(u => ids.Contains(u.UserId)).ToListAsync());
        }

        public async Task<User> GetBySocialId(string id)
        {
            return await ExecuteAsync(async (dbSet) => await dbSet.FirstOrDefaultAsync(u => u.FacebookId == id));
        }
    }
}
