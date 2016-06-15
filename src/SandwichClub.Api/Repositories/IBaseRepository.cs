using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandwichClub.Api.Repositories
{
    public interface IBaseRepository<TId, T>
    {
        Task<T> GetByIdAsync(TId id);

        Task<IList<T>> GetAsync();

        Task<int> CountAsync();

        Task<T> InsertAsync(T t);

        Task UpdateAsync(T t);

        Task DeleteAsync(TId id);

        Task DeleteAsync(T t);
    }
}