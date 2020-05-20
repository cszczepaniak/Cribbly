using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cribbly.Models.Relationships
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigurePlayInGameRelationships(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayInGame>()
                .Property(g => g.Scores)
                .HasConversion(
                    s => string.Join(',', s.Select(s => s.ToString())),
                    s => s.Split(',', StringSplitOptions.None).Select(s => int.Parse(s)).ToList()
                )
                .Metadata
                .SetValueComparer(new ValueComparer<List<int>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))
                ));
            // max length string could be "1xx,1xx" => 7 characters
            modelBuilder.Entity<PlayInGame>()
                .Property(g => g.Scores)
                .HasColumnType("VARCHAR(7)");

            modelBuilder.Entity<PlayInGame>()
                .Ignore(g => g.Teams);
        }

        public static void ConfigurePlayerDbSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .Ignore(p => p.PhoneNumber)
                .Ignore(p => p.PhoneNumberConfirmed);
        }

        public static void ConfigureTeamDbSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .Property(t => t.TournamentRound)
                .HasDefaultValue(-1);
            modelBuilder.Entity<Team>()
                .Property(t => t.TournamentSeed)
                .HasDefaultValue(-1);
            modelBuilder.Entity<Team>()
                .Ignore(t => t.PlayInGames);
        }

        public static void ConfigureTeamAndPlayInGameMapping(this ModelBuilder modelBuilder)
        {
            // we have to use an entity to represent the join table to do a many-to-many mapping
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
    }
}