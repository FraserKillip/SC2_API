using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class ControllerBase<TId, TDto, TService> : Controller where TDto : class where TService : IBaseService<TId, TDto>
    {
        protected readonly TService Service;

        protected ControllerBase(TService service)
        {
            Service = service;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<TDto>> Get()
        {
            return await Service.GetAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<TDto> Get(TId id)
        {
            return await Service.GetByIdAsync(id);
        }

        // POST api/values
        [HttpPost]
        public virtual async Task Post([FromBody]TDto value)
        {
            await Service.InsertAsync(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public virtual async Task Put(TId id, [FromBody]TDto value)
        {
            await Service.UpdateAsync(value);
        }

        // DELETE api/values
        [HttpDelete("{id}")]
        public virtual async Task Delete(TId id)
        {
            await Service.DeleteAsync(id);
        }
    }
}
