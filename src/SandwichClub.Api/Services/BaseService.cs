using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;

namespace SandwichClub.Api.Services
{
    public abstract class BaseService<TId, T, TRepo> : IBaseService<TId, T> where TId : struct where T : class where TRepo : IBaseRepository<TId, T>
    {
        protected readonly TRepo Repository;
        protected readonly ILogger<BaseService<TId, T, TRepo>> Logger;

        protected BaseService(TRepo repo, ILogger<BaseService<TId, T, TRepo>> logger)
        {
            Repository = repo;
            Logger = logger;
        }

        public virtual Task<IEnumerable<T>> GetAsync()
        {
            return Repository.GetAsync();
        }

        public virtual Task<int> CountAsync()
        {
            return Repository.CountAsync();
        }

        public virtual Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<TId> ids)
        {
            return Repository.GetByIdsAsync(ids);
        }

        public virtual Task<T> GetByIdAsync(TId id)
        {
            return Repository.GetByIdAsync(id);
        }

        public virtual Task<T> InsertAsync(T t)
        {
            return Repository.InsertAsync(t);
        }

        public virtual Task<T> UpdateAsync(T t)
        {
            return Repository.UpdateAsync(t);
        }

        public virtual async Task<T> SaveAsync(T t)
        {
            var id = GetId(t);

            Logger.LogTrace("Saving {0} with id {1}", t.GetType().Name, id);

            var entity = await Repository.GetByIdAsync(id);
            var exists = entity != null;
            var delete = SaveShouldDelete(t);

            Logger.LogTrace("Entity with id {0} does{1} exist and should{2} be deleted", id, exists ? "" : " not", delete ? "" : " not");

            if (delete && exists)
                await Repository.DeleteAsync(entity);
            if (delete) return t;

            if (exists)
                return await Repository.UpdateAsync(t);
            else
                return await Repository.InsertAsync(t);
        }

        public virtual Task DeleteAsync(TId id)
        {
            return Repository.DeleteAsync(id);
        }

        public abstract TId GetId(T t);

        protected virtual bool SaveShouldDelete(T t)
        {
            return false;
        }
    }
}
