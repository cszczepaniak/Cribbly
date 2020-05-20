using System.Collections.Generic;
using System.Linq;
using Cribbly.Models;

namespace Cribbly.Services
{
    public class PlayInGameService : IPlayInGameService
    {
        private AppDbContext db;
        public PlayInGameService(AppDbContext db)
        {
            this.db = db;
        }

        public PlayInGame GetByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<PlayInGame> GetByIDs(List<int> ids)
        {
            throw new System.NotImplementedException();
        }
    }
}