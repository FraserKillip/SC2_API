using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;

namespace SandwichClub.Api.Services
{
    public abstract class SaveOnlyBaseService<TId, T, TRepo> : BaseService<TId, T, TRepo> where TId : struct where T : class where TRepo : IBaseRepository<TId, T>
    {

        protected SaveOnlyBaseService(TRepo repo, ILogger<SaveOnlyBaseService<TId, T, TRepo>> logger) : base(repo, logger)
        {
        }

        public override Task<T> InsertAsync(T t)
        {
            return SaveAsync(t);
        }

        public override Task<T> UpdateAsync(T t)
        {
            return SaveAsync(t);
        }
    }
}
