using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Cribbly.Models;
using Cribbly.Models.Gameplay;

namespace Cribbly.Tests
{
    public class BracketTests
    {
        [Fact]
        public void TestSeedBracket()
        {
            var standings = getMockStandings();
            var bracket = new Bracket(standings, 8);
        }

        [Fact]
        public void TestAdvance()
        {

        }
        private List<Standing> getMockStandings()
        {
            return new List<Standing>()
            {
                new Standing { Division="div0",TeamName="team0", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div0",TeamName="team1", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div0",TeamName="team2", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div0",TeamName="team3", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div0",TeamName="team4", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div1",TeamName="team5", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div1",TeamName="team6", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div1",TeamName="team7", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div1",TeamName="team8", G1Score=121, G2Score=121, G3Score=121 },
                new Standing { Division="div1",TeamName="team9", G1Score=121, G2Score=121, G3Score=121 },
            };
        }
    }

}
