using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Services.Mapper;
using SandwichClub.Api.Repositories;

namespace SandwichClub.Api.Services
{
    public class BaseService<TId, T, TDto, TRepo> : IBaseService<TId, TDto> where T : class where TDto : class where TRepo : IBaseRepository<TId, T>
    {
        protected readonly TRepo Repository;
        protected readonly IMapper<T, TDto> Mapper;

        protected BaseService(TRepo repo, IMapper<T, TDto> mapper)
        {
            Repository = repo;
            Mapper = mapper;
        }

        public virtual async Task<IEnumerable<TDto>> GetAsync()
        {
            var items = await Repository.GetAsync();
            return await ToDtosAsync(items);
        }

        public virtual Task<int> CountAsync()
        {
            return Repository.CountAsync();
        }

        public virtual async Task<TDto> GetByIdAsync(TId id)
        {
            var item = await Repository.GetByIdAsync(id);

            // Check for nulls
            if (item == null)
                return null;

            var dto = ToDto(item);
            await HydrateDtoAsync(item, dto);
            return dto;
        }

        public virtual async Task<TDto> InsertAsync(TDto t)
        {
            var model = ToModel(t);
            var updated = await Repository.InsertAsync(model);
            var dto = ToDto(updated);
            await HydrateDtoAsync(updated, dto);
            return dto;
        }

        public virtual Task UpdateAsync(TDto t)
        {
            var model = ToModel(t);
            return Repository.UpdateAsync(model);
        }

        public virtual Task DeleteAsync(TId id)
        {
            return Repository.DeleteAsync(id);
        }

        public TDto ToDto(T t)
        {
            return Mapper.ToDto(t);
        }

        public T ToModel(TDto dto)
        {
            return Mapper.ToModel(dto);
        }

        protected virtual Task HydrateDtoAsync(T t, TDto dto)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task<IList<TDto>> ToDtosAsync(IList<T> items)
        {
            var dtos = new List<TDto>(items.Count);

            // Convert to dtos
            foreach (var item in items)
            {
                var dto = ToDto(item);
                await HydrateDtoAsync(item, dto);
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
