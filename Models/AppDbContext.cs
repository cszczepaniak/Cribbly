using Cribbly.Data;
using Cribbly.Models.Relationships;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cribbly.Models
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

            modelBuilder.Entity<Team>()
                .HasOne<Division>(t => t.Division)
                .WithMany(d => d.Teams);

            modelBuilder.ConfigureTeamDbSettings();
            modelBuilder.ConfigurePlayInGameRelationships();
            modelBuilder.ConfigureTeamAndPlayInGameMapping();
            modelBuilder.ConfigurePlayerDbSettings();
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<PlayInGame> PlayInGames { get; set; }
        public DbSet<PlayInGameDataModel> PlayInGameDataModels { get; set; }

    }
}