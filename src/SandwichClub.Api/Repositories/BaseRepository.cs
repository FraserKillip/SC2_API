using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Collections.Concurrent;
using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace SandwichClub.Api.Repositories
{
    public class BaseRepository
    {
        protected static ConcurrentDictionary<Type, object> DefaultValues = new ConcurrentDictionary<Type, object>();
    }

    public abstract class BaseRepository<TId, T> : BaseRepository, IBaseRepository<TId, T> where T : class
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

        public virtual object[] GetKeys(TId id)
            => new object[] { id };

        public async virtual Task<T> GetByIdAsync(TId id)
        {
            // Check if the id is a default value
            if (Equals(id, default(TId)))
                return null;
            
            var keys = GetKeys(id);
            // Check all keys are not defaults
            foreach (var key in keys)
            {
                if (key == null)
                    return null;
                if (DefaultValues.GetOrAdd(key.GetType(), t => t.GetTypeInfo().IsValueType ? Activator.CreateInstance(t) : null) == key)
                    return null;
            }

            // Find the item
            throw new NotImplementedException("Waiting for EntityFrameworkCore 1.1.0 :'(");
            //return await DbSet.FindAsync(keys);
        }

        public async virtual Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<TId> ids)
        {
            var items = new List<Task<T>>();

            foreach (var id in ids)
                items.Add(GetByIdAsync(id));

            return (await Task.WhenAll(items)).Where(e => e != null);
        }

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
            if (entry.State != EntityState.Added)
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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Context.Database.BeginTransactionAsync();
        }
    }
}
