using System.Collections.Generic;

namespace SandwichClub.Api.Services.Mapper
{
    public interface IMapper<T, TDto> where T : class where TDto : class
    {
        T ToModel(TDto dto);
        TDto ToDto(T t);

        IEnumerable<T> ToModel(IEnumerable<TDto> dto);
        IEnumerable<TDto> ToDto(IEnumerable<T> t);
    }
}
