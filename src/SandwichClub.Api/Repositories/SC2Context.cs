using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class SC2Context : DbContext
    {
        public SC2Context(DbContextOptions<SC2Context> options)
            : base(options)
        { }

         public DbSet<Week> Weeks { get; set; }
         public DbSet<User> Users { get; set; }
         public DbSet<WeekUserLink> WeekUserLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Week>().HasKey(w => w.WeekId);
            builder.Entity<User>().HasKey(u => u.UserId);
            builder.Entity<WeekUserLink>().HasKey(wu => new { wu.WeekId, wu.UserId });
            base.OnModelCreating(builder);
        }
    }
}