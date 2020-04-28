using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models.Gameplay
{
    public class _3WayGame : PlayInGame
    {
        [Display(Name = "Team 3 ID")]
        public int Team3Id { get; set; }
        [Display(Name = "Team 3 Name")]
        public string Team3Name { get; set; }
        [Display(Name = "Team 3 Total Score")]
        public int Team3TotalScore { get; set; }
    }
}
