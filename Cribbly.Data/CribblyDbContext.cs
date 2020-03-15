using Cribbly.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace Cribbly.Data
{

    public class CribblyDbContext : DbContext
    {
        public CribblyDbContext(DbContextOptions<CribblyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder
        //        .Entity<Article>()
        //        .HasOne(a => a.Author)
        //        .WithMany(a => a.Articles)
        //        .HasForeignKey(a => a.AuthorId);

        //    base.OnModelCreating(builder);
        //}
    }
}