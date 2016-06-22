using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories;

namespace SandwichClub.Api.Services
{
    public class BaseService<TId, T, TRepo> : IBaseService<TId, T> where T : class where TRepo : IBaseRepository<TId, T>
    {
        protected readonly TRepo Repository;

        protected BaseService(TRepo repo)
        {
            Repository = repo;
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

        public virtual Task UpdateAsync(T t)
        {
            return Repository.UpdateAsync(t);
        }

        public virtual Task DeleteAsync(TId id)
        {
            return Repository.DeleteAsync(id);
        }
    }
}
