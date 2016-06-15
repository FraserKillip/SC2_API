using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SandwichClub.Api.Repositories
{
    public abstract class BaseRepository<TId, T> : IBaseRepository<TId, T> where T : class
    {
        protected readonly SC2Context _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(SC2Context context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public abstract Task<T> GetByIdAsync(TId id);

        public async Task<IList<T>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<T> InsertAsync(T t)
        {
            var i = _dbSet.Add(t);
            await _context.SaveChangesAsync();
            return i.Entity;
        }

        public async Task UpdateAsync(T t)
        {
            _dbSet.Update(t);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            
            await DeleteAsync(await GetByIdAsync(id));
        }

        public async Task DeleteAsync(T t)
        {
            _dbSet.Remove(t);
            await _context.SaveChangesAsync();
        }
    }
}
