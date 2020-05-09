using System.Collections.Generic;

namespace Cribbly.Models
{
    public class PlayInGame
    {
        public int ID { get; set; }
        public List<TeamPlayInGame> TeamPlayInGames { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }
}