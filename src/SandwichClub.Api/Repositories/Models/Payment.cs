using System;
using System.ComponentModel;

namespace SandwichClub.Api.Repositories.Models
{
    [Description("A payment made by a user")]
    public class Payment : IEntity
    {
        [Description("Id of payment")]
        public int Id { get; set; }

        [Description("User that made the payment")]
        public int UserId { get; set; }

        [Description("Utc time of the payment")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        [Description("Amount that was paid")]
        public decimal Amount { get; set; }
    }
}
