using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

            modelBuilder.Entity<PlayInGame>()
                .Property(g => g.Scores)
                .HasConversion(
                    s => string.Join(',', s.Select(s => s.ToString())),
                    s => s.Split(',', StringSplitOptions.None).Select(s => int.Parse(s)).ToList()
                );

            // many-to-many relationship for teams and play in games
            modelBuilder.Entity<TeamPlayInGame>()
                .HasKey(t => new { t.TeamID, t.PlayInGameID });

            modelBuilder.Entity<TeamPlayInGame>()
                .HasOne(tg => tg.Team)
                .WithMany(t => t.TeamPlayInGames)
                .HasForeignKey(tg => tg.TeamID);

            modelBuilder.Entity<TeamPlayInGame>()
                .HasOne(tg => tg.PlayInGame)
                .WithMany(t => t.TeamPlayInGames)
                .HasForeignKey(tg => tg.PlayInGameID);
        }

        public DbSet<Team> Teams { get; set; }
    }
}