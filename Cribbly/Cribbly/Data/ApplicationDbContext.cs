using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cribbly.Models;
using Cribbly.Models.Gameplay;

namespace Cribbly.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cribbly.Models.Team> Teams { get; set; }
        public DbSet<Cribbly.Models.ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Cribbly.Models.Standing> Standings { get; set; }
        public DbSet<Cribbly.Models.Division> Divisions { get; set; }
        public DbSet<Cribbly.Models.Gameplay.PlayInGame> PlayInGames { get; set; }
        public DbSet<Cribbly.Models.Gameplay._3WayGame> ThreeWayGames { get; set; }
    }
}
