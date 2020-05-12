using System.Collections.Generic;
using System.Linq;
using Cribbly.Models;

namespace Cribbly.Services
{
    public class PlayerService
    {
        private AppDbContext db;
        public PlayerService(AppDbContext db)
        {
            this.db = db;
        }

        public Player GetPlayerByID(string id) =>
            db.Users.Where(u => u.Id == id).FirstOrDefault();

        public Player GetByFirstAndLastName(string first, string last) =>
            db.Users.Where(u => u.FirstName.Equals(first) && u.LastName.Equals(last)).FirstOrDefault();

    }
}