using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandwichClub.Api.Controllers.Mapper
{
    public abstract class BaseMapper<T, TDto> : IMapper<T, TDto> where T : class where TDto : class
    {
        public abstract Task<T> ToModelAsync(TDto dto);

        public abstract Task<TDto> ToDtoAsync(T t);

        public virtual async Task<IEnumerable<T>> ToModelAsync(IEnumerable<TDto> dtos)
        {
            var models = dtos.Select(ToModelAsync);
            return await Task.WhenAll(models);
        }

        public virtual async Task<IEnumerable<TDto>> ToDtoAsync(IEnumerable<T> models)
        {
            var dtos = models.Select(ToDtoAsync);
            return await Task.WhenAll(dtos);
        }
    }
}
