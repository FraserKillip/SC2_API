using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SandwichClub.Api.Controllers.Mapper;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers
{
    [Route("[controller]")]
    public abstract class ControllerBase<TId, T, TDto, TService> : Controller where T : class where TDto : class where TService : IBaseService<TId, T>
    {
        protected readonly TService Service;
        protected readonly IMapper<T, TDto> Mapper;

        protected ControllerBase(TService service, IMapper<T, TDto> mapper)
        {
            Service = service;
            Mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<TDto>> Get()
        {
            var items = await Service.GetAsync();
            return await Mapper.ToDtoAsync(items);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<TDto> Get(TId id)
        {
            var item = await Service.GetByIdAsync(id);

            // Check for nulls
            if (item == null)
                return null;

            return await Mapper.ToDtoAsync(item);
        }

        // POST api/values
        [HttpPost]
        public virtual async Task<TDto> Post([FromBody]TDto value)
        {
            var model = await Mapper.ToModelAsync(value);
            var updated = await Service.InsertAsync(model);
            return await Mapper.ToDtoAsync(updated);
        }

        // PUT api/values
        [HttpPut]
        public virtual async Task<TDto> Put([FromBody]TDto value)
        {
            var model = await Mapper.ToModelAsync(value);
            await Service.UpdateAsync(model);
            return value;
        }

        // DELETE api/values
        [HttpDelete("{id}")]
        public virtual async Task<TDto> Delete(TId id)
        {
            var dto = Get(id);
            await Service.DeleteAsync(id);
            return await dto;
        }
    }
}
