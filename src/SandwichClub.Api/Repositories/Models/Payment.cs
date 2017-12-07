using System;
using System.ComponentModel;

namespace SandwichClub.Api.Repositories.Models
{
    [Description("A payment made by a user")]
    public class Payment : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public decimal Amount { get; set; }
    }
}
