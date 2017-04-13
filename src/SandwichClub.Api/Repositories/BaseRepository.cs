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
using System.Threading;

namespace SandwichClub.Api.Repositories
{
    public class BaseRepository
    {
        protected static ConcurrentDictionary<Type, object> DefaultValues = new ConcurrentDictionary<Type, object>();
    }

    public abstract class BaseRepository<TId, T> : BaseRepository, IBaseRepository<TId, T> where T : class
    {
        private readonly SemaphoreSlim _contextSemaphore = new SemaphoreSlim(1);
        private readonly ScContext _context;
        private readonly DbSet<T> _dbSet;
        protected readonly IMapper Mapper;

        protected BaseRepository(ScContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<T>();
            Mapper = mapper;
        }

        protected TResult Execute<TResult>(Func<ScContext, DbSet<T>, TResult> action)
        {
            bool success = false;
            try
            {
                success = _contextSemaphore.Wait(1000);
                return action(_context, _dbSet);
            }
            finally
            {
                if (success)
                    _contextSemaphore.Release();
            }
        }

        protected async Task<TResult> ExecuteAsync<TResult>(Func<DbSet<T>, Task<TResult>> action)
            => await ExecuteAsync(async (context, dbSet) => await action(dbSet));

        protected async Task<TResult> ExecuteAsync<TResult>(Func<ScContext, DbSet<T>, Task<TResult>> action)
        {
            bool success = false;
            try
            {
                success = await _contextSemaphore.WaitAsync(1000);
                return await action(_context, _dbSet);
            }
            finally
            {
                if (success)
                    _contextSemaphore.Release();
            }
        }

        protected async Task ExecuteAsync(Func<ScContext, DbSet<T>, Task> action)
        {
            await ExecuteAsync(async (context, dbSet) =>
            {
                await action(context, dbSet);
                return true;
            });
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
            return await ExecuteAsync(async dbSet => await dbSet.FindAsync(keys));
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
            return Execute((context, dbSet) =>
            {
                var entry = context.ChangeTracker.Entries<T>().Where(e => t.Equals(e.Entity)).FirstOrDefault();
                if (entry != null)
                {
                    Mapper.Map<T, T>(t, entry.Entity);
                    return entry;
                }
                return context.Attach<T>(t);
            });
        }

        public async Task<IEnumerable<T>> GetAsync()
            => await ExecuteAsync(async dbSet => await dbSet.ToListAsync());

        public async Task<int> CountAsync()
        {
            return await ExecuteAsync(async dbSet => await dbSet.CountAsync());
        }

        public async Task<T> InsertAsync(T t)
        {
            return await ExecuteAsync(async (context, dbSet) =>
            {
                var entry = Entry(t);
                if (entry.State != EntityState.Unchanged)
                    throw new DatabaseException("Can't insert entity which already exists");
                entry.State = EntityState.Added;
                await context.SaveChangesAsync();
                return entry.Entity;
            });
        }

        public async Task<T> UpdateAsync(T t)
        {
            return await ExecuteAsync(async (context, dbSet) =>
            {
                var entry = Entry(t);
                if (entry.State == EntityState.Deleted)
                    throw new DatabaseException("Can't update entity which is deleted");
                if (entry.State != EntityState.Added)
                    entry.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return entry.Entity;
            });
        }

        public async Task<T> DeleteAsync(TId id)
        {
            return await DeleteAsync(await GetByIdAsync(id));
        }

        public async Task<T> DeleteAsync(T t)
        {
            return await ExecuteAsync(async (context, dbSet) =>
            {
                var entry = Entry(t);
                if (entry.State == EntityState.Deleted)
                    return entry.Entity;
                entry.State = EntityState.Deleted;
                await context.SaveChangesAsync();
                return entry.Entity;
            });
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await ExecuteAsync(async (context, dbSet) =>
            {
                return await context.Database.BeginTransactionAsync();
            });
        }
    }
}
