using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IPaymentService : IBaseService<int, Payment>
    {
        Task<decimal> GetTotalPaidForUser(int userId);

        Task<decimal> GetTotalOwedForUser(int userId);

        Task<Payment> PayOwedForUser(int userId);

        Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);
    }
}
