using System.Collections.Generic;

namespace Cribbly.Data
{
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public List<TeamPlayInGame> TeamPlayInGames { get; set; }
    }
}