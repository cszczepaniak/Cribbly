using Cribbly.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cribbly.Services
{
    public class AppDbContext : IdentityDbContext<Player>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasOne<Team>(p => p.Team)
                .WithMany(t => t.Players);
        }

        public DbSet<Team> Teams { get; set; }
    }
}