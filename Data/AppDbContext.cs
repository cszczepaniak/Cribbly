using Cribbly.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cribbly.Data
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

            modelBuilder.Entity<TeamPlayInGame>()
                .HasKey(t => new { t.TeamID, t.PlayInGameID });

            modelBuilder.Entity<TeamPlayInGame>()
                .HasOne(tg => tg.Team)
                .WithMany(t => )
        }

        public DbSet<Team> Teams { get; set; }
    }
}