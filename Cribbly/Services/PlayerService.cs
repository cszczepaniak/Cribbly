using System.Collections.Generic;
using System.Linq;
using Cribbly.Models;

namespace Cribbly.Services
{
    public class PlayerService : IPlayerService
    {
        private AppDbContext db;
        public PlayerService(AppDbContext db)
        {
            this.db = db;
        }

        public Player GetPlayerByID(string id) =>
            db.Users.Where(u => u.Id == id).FirstOrDefault();

        public List<Player> GetPlayersByIDs(List<string> ids) =>
             db.Users.Where(u => ids.Contains(u.Id)).ToList();

        public Player GetByFirstAndLastName(string first, string last) =>
            db.Users.Where(u => u.FirstName.Equals(first) && u.LastName.Equals(last)).FirstOrDefault();

    }
}