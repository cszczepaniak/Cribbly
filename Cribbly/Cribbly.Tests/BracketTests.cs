using System;
using System.Collections.Generic;
using System.Text;

namespace Cribbly.Tests
{
    public class BracketTests
    {
        [Fact]
        public void TestAdvance()
        {

        }
        private Bracket makeBracket(int nTeams)
        {
            var standings = new List<Standing>()
            {
                new Standing { Division="div0",TeamName="team0", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div0",TeamName="team1", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div0",TeamName="team2", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div0",TeamName="team3", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div0",TeamName="team4", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div1",TeamName="team5", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div1",TeamName="team6", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div1",TeamName="team7", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div1",TeamName="team8", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
                new Standing { Division="div1",TeamName="team9", G1Score=121, G2Score=121, G3Score=121, G1WinLoss=20, G1WinLoss=20, G1WinLoss=20 },
            };
        }
    }

}
