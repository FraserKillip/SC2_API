using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandwichClub.Api.Repositories
{
    public interface IBaseRepository<TId, T>
    {
        Task<IEnumerable<T>> GetAsync();

        Task<int> CountAsync();

        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<TId> ids);

        Task<T> GetByIdAsync(TId id);

        Task<T> InsertAsync(T t);

        Task<T> UpdateAsync(T t);

        Task<T> DeleteAsync(TId id);

        Task<T> DeleteAsync(T t);
    }
}