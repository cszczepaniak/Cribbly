using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cribbly.Models;

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
    }
}
