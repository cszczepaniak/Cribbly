using System.Collections.Generic;
using System.Linq;
using Cribbly;
using Cribbly.Models;
using Cribbly.Services;
using Cribbly.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit;

namespace Cribbly.Tests.Services
{
    public class PlayerServiceTests : TestWithData
    {
        private PlayerService playerService;
        public PlayerServiceTests() :
            base(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("playerServiceTestDb").Options)
        {
            Seed();
            playerService = new PlayerService(new AppDbContext(ContextOptions));
        }
        protected override void Seed()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                context.Users.AddRange(new List<Player>()
                {
                    new Player(){Id = "p1", FirstName = "Alice", LastName = "Smith"},
                    new Player(){Id = "p2", FirstName = "Bob", LastName = "Smith"},
                    new Player(){Id = "p3", FirstName = "Chuck", LastName = "Smith"},
                });
                context.SaveChanges();
            }
        }

        [Theory]
        [InlineData("p1", true, "Alice", "Smith")]
        [InlineData("p2", true, "Bob", "Smith")]
        [InlineData("p3", true, "Chuck", "Smith")]
        [InlineData("p4", false, "", "")]
        public void CanGetPlayerByID(string id, bool exists, string expFirst, string expLast)
        {
            var p = playerService.GetPlayerByID(id);
            if (exists)
            {
                Assert.Equal(id, p.Id);
                Assert.Equal(expFirst, p.FirstName);
                Assert.Equal(expLast, p.LastName);
            }
            else
            {
                Assert.Null(p);
            }
        }

        [Theory]
        [MemberData(nameof(CanGetPlayersByIDsData))]
        public void CanGetPlayersByIDs(List<string> ids, List<string> expIDs, List<string> expFirsts, List<string> expLasts)
        {
            var ps = playerService.GetPlayersByIDs(ids);
            Assert.Equal(expIDs, ps.Select(p => p.Id));
            Assert.Equal(expFirsts, ps.Select(p => p.FirstName));
            Assert.Equal(expLasts, ps.Select(p => p.LastName));
        }
        public static IEnumerable<object[]> CanGetPlayersByIDsData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<string>(){"p1"},
                    new List<string>(){"p1"},
                    new List<string>(){"Alice"},
                    new List<string>(){"Smith"},
                },
                new object[]
                {
                    new List<string>(){"p1", "p3"},
                    new List<string>(){"p1", "p3"},
                    new List<string>(){"Alice", "Chuck"},
                    new List<string>(){"Smith", "Smith"},
                },
                new object[]
                {
                    new List<string>(){"p1", "p2", "p3"},
                    new List<string>(){"p1", "p2", "p3"},
                    new List<string>(){"Alice", "Bob", "Chuck"},
                    new List<string>(){"Smith", "Smith", "Smith"},
                },
                new object[]
                {
                    new List<string>(){"p1", "p4"},
                    new List<string>(){"p1"},
                    new List<string>(){"Alice"},
                    new List<string>(){"Smith"},
                },
            };
    }
}