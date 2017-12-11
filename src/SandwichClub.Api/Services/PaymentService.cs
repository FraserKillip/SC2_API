using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class PaymentService : BaseService<int, Payment, IPaymentRepository>, IPaymentService
    {
        private readonly IWeekService _weekService;

        public PaymentService(IPaymentRepository repo, ILogger<BaseService<int, Payment, IPaymentRepository>> logger,
            IWeekService weekService)
            : base(repo, logger)
        {
            _weekService = weekService;
        }

        public override int GetId(Payment t)
            => t.Id;

        public async Task<decimal> GetTotalPaidForUser(int userId)
        {
            var payments = await Repository.GetByUserId(userId);

            return payments.Sum(p => p.Amount);
        }

        public async Task<decimal> GetTotalOwedForUser(int userId)
        {
            var totalCosts = await _weekService.GetTotalCostsForUserAsync(userId);
            var totalPaid = await GetTotalPaidForUser(userId);

            return totalCosts - totalPaid;
        }

        public async Task<Payment> PayOwedForUser(int userId)
        {
            var amountOwed = await GetTotalOwedForUser(userId);

            if (amountOwed <= 0)
                return new Payment { UserId = userId };

            return await InsertAsync(new Payment
            {
                UserId = userId,
                CreatedTime = DateTime.UtcNow,
                Amount = amountOwed,
            });
        }

        public Task<IEnumerable<Payment>> GetByUserIdAsync(int userId)
            => Repository.GetByUserId(userId);
    }
}