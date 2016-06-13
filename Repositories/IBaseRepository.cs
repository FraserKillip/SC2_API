using System.Collections.Generic;

namespace SC2_API.Repositories
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