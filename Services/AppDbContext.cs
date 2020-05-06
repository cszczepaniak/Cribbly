using Cribbly.Models;
using Microsoft.EntityFrameworkCore;

namespace Cribbly.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
    }
}