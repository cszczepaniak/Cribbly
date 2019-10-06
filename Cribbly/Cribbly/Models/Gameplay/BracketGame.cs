using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models.Gameplay
{
    public class BracketGame
    {
        public enum GameWinner
        {
            Team1 = 0,
            Team2 = 1
        }
        public Standing[] Teams { get; set; }
        public Standing Winner { get; private set; }
        public BracketGame(Standing team1, Standing team2)
        {
            Teams = new Standing[2] { team1, team2 };
        }
        public void SetWinner(GameWinner winner)
        {
            Winner = Teams[(int)winner];
        }
    }
}
