using Cribbly.Models;
using Microsoft.EntityFrameworkCore;

namespace Cribbly.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
    }
}