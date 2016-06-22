using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandwichClub.Api.Controllers.Mapper
{
    public interface IMapper<T, TDto> where T : class where TDto : class
    {
        Task<T> ToModelAsync(TDto dto);
        Task<TDto> ToDtoAsync(T model);

        Task<IEnumerable<T>> ToModelAsync(IEnumerable<TDto> dtos);
        Task<IEnumerable<TDto>> ToDtoAsync(IEnumerable<T> models);
    }
}
