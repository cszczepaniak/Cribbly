using System.Collections.Generic;

namespace Cribbly.Models
{
    public class Team
    {
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public List<PlayInGame> PlayInGames { get; set; }
    }
}