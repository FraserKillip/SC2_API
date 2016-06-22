using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SandwichClub.Api.Repositories
{
    public abstract class BaseRepository<TId, T> : IBaseRepository<TId, T> where T : class
    {
        protected readonly ScContext Context;
        protected readonly DbSet<T> DbSet;

        protected BaseRepository(ScContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public abstract Task<T> GetByIdAsync(TId id);

        public abstract Task<IEnumerable<T>>  GetByIdsAsync(IEnumerable<TId> ids);

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<T> InsertAsync(T t)
        {
            var i = DbSet.Add(t);
            await Context.SaveChangesAsync();
            return i.Entity;
        }

        public async Task UpdateAsync(T t)
        {
            DbSet.Update(t);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            
            await DeleteAsync(await GetByIdAsync(id));
        }

        public async Task DeleteAsync(T t)
        {
            DbSet.Remove(t);
            await Context.SaveChangesAsync();
        }
    }
}
