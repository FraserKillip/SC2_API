using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandwichClub.Api.Services
{
    public interface IBaseService<TId, TDto>
    {
        Task<TDto> GetByIdAsync(TId id);

        Task<IEnumerable<TDto>> GetAsync();

        Task<int> CountAsync();

        Task<TDto> InsertAsync(TDto t);

        Task UpdateAsync(TDto t);

        Task DeleteAsync(TId id);
    }
}
