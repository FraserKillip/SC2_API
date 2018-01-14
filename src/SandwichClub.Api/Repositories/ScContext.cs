using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore.Design;

namespace SandwichClub.Api.Repositories
{
    public class ScContext : DbContext
    {
        public ScContext(DbContextOptions<ScContext> options)
            : base(options)
        { }

        //public ScContext()
        //    : base(new DbContextOptionsBuilder<ScContext>().UseSqlite("./database.sqlite").Options)
        //{ }

        public SemaphoreSlim ContextSemaphore { get; } = new SemaphoreSlim(1);

        public DbSet<Week> Weeks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WeekUserLink> WeekUserLinks { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Week>().HasKey(w => w.WeekId);
            builder.Entity<User>().HasKey(u => u.UserId);
            builder.Entity<WeekUserLink>().HasKey(wu => new { wu.WeekId, wu.UserId });
            builder.Entity<Payment>().HasKey(p => p.Id);
            base.OnModelCreating(builder);
        }
    }

    public class DesignTimeScContextFactory : IDesignTimeDbContextFactory<ScContext>
    {
        public ScContext CreateDbContext(string[] args)
        {
            return new ScContext(new DbContextOptionsBuilder<ScContext>().UseSqlite("./database.sqlite").Options);
        }
    }
}