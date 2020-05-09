using System.Collections.Generic;

namespace Cribbly.Models
{
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
    }
}