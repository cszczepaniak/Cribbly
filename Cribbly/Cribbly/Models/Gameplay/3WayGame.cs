using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models.Gameplay
{
    public class _3WayGame : PlayInGame
    {
        public int Team3Id { get; set; }
        public string Team3Name { get; set; }
        public int Team3TotalScore { get; set; }
    }
}
