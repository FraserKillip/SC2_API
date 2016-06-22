﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandwichClub.Api.Services
{
    public interface IBaseService<TId, T>
    {
        Task<IEnumerable<T>> GetAsync();

        Task<int> CountAsync();

        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<TId> ids);

        Task<T> GetByIdAsync(TId id);

        Task<T> InsertAsync(T t);

        Task UpdateAsync(T t);

        Task DeleteAsync(TId id);
    }
}
