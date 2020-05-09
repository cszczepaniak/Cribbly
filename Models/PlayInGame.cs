using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models
{
    public class PlayInGame
    {
        public int ID { get; set; }
        [MinLength(0)]
        [MaxLength(3)]
        public List<TeamPlayInGame> TeamPlayInGames { get; set; }
        [MinLength(2)]
        [MaxLength(2)]
        public List<int> Scores { get; set; }
    }
}