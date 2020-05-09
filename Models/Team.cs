using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models
{
    public class Team
    {
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [MinLength(2)]
        [MaxLength(2)]
        public List<Player> Players { get; set; }

        [MinLength(0)]
        [MaxLength(3)]
        public List<TeamPlayInGame> TeamPlayInGames { get; set; }
    }
}