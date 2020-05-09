using System.Collections.Generic;
using System.Linq;
using Cribbly.Data;

namespace Cribbly.Services
{
    public class PlayerService
    {
        private AppDbContext db;
        public PlayerService(AppDbContext db)
        {
            this.db = db;
        }
    }
}