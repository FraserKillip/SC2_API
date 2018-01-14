using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public interface IPaymentRepository : IBaseRepository<int, Payment>
    {
        Task<IEnumerable<Payment>> GetByUserId(int userId);
    }
}
