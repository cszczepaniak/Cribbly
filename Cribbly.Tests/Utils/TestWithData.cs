using Cribbly.Models;
using Microsoft.EntityFrameworkCore;

namespace Cribbly.Tests.Utils
{
    public abstract class TestWithData
    {
        protected TestWithData(DbContextOptions<AppDbContext> contextOptions)
        {
            ContextOptions = contextOptions;
            initDB();
        }
        protected DbContextOptions<AppDbContext> ContextOptions { get; }
        private void initDB()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        protected abstract void Seed();
    }
}