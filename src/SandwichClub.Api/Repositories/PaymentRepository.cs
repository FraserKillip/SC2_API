using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class PaymentRepository : BaseRepository<int, Payment>, IPaymentRepository
    {
        public PaymentRepository(ScContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Payment>> GetByUserId(int userId)
        {
            return await ExecuteAsync(async payments => await payments.Where(p => p.UserId == userId).ToListAsync());
        }
    }
}