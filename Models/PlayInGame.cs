using System.Collections.Generic;

namespace Cribbly.Models
{
    public class PlayInGame
    {
        public int ID { get; set; }
        public List<Team> Teams { get; set; }
        public List<int> Scores { get; set; }
    }
}