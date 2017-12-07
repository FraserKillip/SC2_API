using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;
using System.Threading;

namespace SandwichClub.Api.Repositories
{
    public class ScContext : DbContext
    {
        public ScContext(DbContextOptions<ScContext> options)
            : base(options)
        { }

        public SemaphoreSlim ContextSemaphore { get; } = new SemaphoreSlim(1);

         public DbSet<Week> Weeks { get; set; }
         public DbSet<User> Users { get; set; }
         public DbSet<WeekUserLink> WeekUserLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Week>().HasKey(w => w.WeekId);
            builder.Entity<User>().HasKey(u => u.UserId);
            builder.Entity<WeekUserLink>().HasKey(wu => new { wu.WeekId, wu.UserId });
            builder.Entity<Payment>().HasKey(p => p.Id);
            base.OnModelCreating(builder);
        }
    }
}