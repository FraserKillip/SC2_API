using System.Collections.Generic;
using System.Linq;

namespace SandwichClub.Api.Services.Mapper
{
    public abstract class BaseMapper<T, TDto> : IMapper<T, TDto> where T : class where TDto : class
    {
        public abstract T ToModel(TDto dto);
        public abstract TDto ToDto(T t);

        public IEnumerable<T> ToModel(IEnumerable<TDto> dtos)
        {
            return dtos.Select(ToModel);
        }

        public IEnumerable<TDto> ToDto(IEnumerable<T> models)
        {
            return models.Select(ToDto);
        }
    }
}
