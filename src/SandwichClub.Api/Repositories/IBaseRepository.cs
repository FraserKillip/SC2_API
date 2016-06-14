using System.Collections.Generic;

namespace SandwichClub.Api.Repositories
{
    public interface IBaseRepository<TId, T>
    {
        T GetById(TId id);

        IEnumerable<T> Get();

        int Count();

        T Insert(T t);

        void Update(T t);

        void Delete(TId id);

        void Delete(T t);
    }
}