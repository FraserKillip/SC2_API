using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SandwichClub.Api.Repositories
{
    public abstract class BaseRepository<TId, T> : IBaseRepository<TId, T> where T : class
    {
        protected readonly ScContext Context;
        protected readonly DbSet<T> DbSet;
        protected readonly IMapper Mapper;

        protected BaseRepository(ScContext context, IMapper mapper)
        {
            Context = context;
            DbSet = context.Set<T>();
            Mapper = mapper;
        }

        public abstract Task<T> GetByIdAsync(TId id);

        public abstract Task<IEnumerable<T>>  GetByIdsAsync(IEnumerable<TId> ids);

        protected EntityEntry<T> Entry(T t)
        {
            var entry = Context.ChangeTracker.Entries<T>().Where(e => t.Equals(e.Entity)).FirstOrDefault();
            if (entry != null)
            {
                Mapper.Map<T, T>(t, entry.Entity);
                return entry;
            }
            return Context.Attach<T>(t);
        }

        public async Task<IEnumerable<T>> GetAsync()
            => await DbSet.ToListAsync();

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<T> InsertAsync(T t)
        {
            var entry = Entry(t);
            if (entry.State != EntityState.Unchanged)
                throw new DatabaseException("Can't insert entity which already exists");
            entry.State = EntityState.Added;
            await Context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<T> UpdateAsync(T t)
        {
            var entry = Entry(t);
            if (entry.State == EntityState.Deleted)
                throw new DatabaseException("Can't update entity which is deleted");
            entry.State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<T> DeleteAsync(TId id)
        {
            return await DeleteAsync(await GetByIdAsync(id));
        }

        public async Task<T> DeleteAsync(T t)
        {
            var entry = Entry(t);
            if (entry.State == EntityState.Deleted)
                return entry.Entity;
            entry.State = EntityState.Deleted;
            await Context.SaveChangesAsync();
            return entry.Entity;
        }
    }
}
