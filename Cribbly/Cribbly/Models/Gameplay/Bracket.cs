using Cribbly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models.Gameplay
{
    public class Bracket
    {
        public int Id { get; set; }
        public bool IsSeeded { get; set; }
        public List<Standing> Standings { get; set; }
        public List<BracketRound> Rounds { get; set; }
        public Bracket()
        {
        }

        public Bracket(List<Standing> standings)
        {
            Standings = standings;
            IsSeeded = true;
        }
    }
}
