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
            var expTeams = new List<BracketTeam>()
            {
                new BracketTeam { Round=1, Seed=1, TeamName="team0" },
                new BracketTeam { Round=1, Seed=2, TeamName="team5" },
                new BracketTeam { Round=1, Seed=3, TeamName="team1" },
                new BracketTeam { Round=1, Seed=4, TeamName="team6" },
                new BracketTeam { Round=1, Seed=5, TeamName="team2" },
                new BracketTeam { Round=1, Seed=6, TeamName="team9" },
                new BracketTeam { Round=1, Seed=7, TeamName="team8" },
                new BracketTeam { Round=1, Seed=8, TeamName="team4" },
            };
            for (int i = 0; i < expTeams.Count; i++)
            {
                Assert.Equal(expTeams[i].Round, bracket.Teams[i].Round);
                Assert.Equal(expTeams[i].Seed, bracket.Teams[i].Seed);
                Assert.Equal(expTeams[i].TeamName, bracket.Teams[i].TeamName);
            }
        }

        [Fact]
        public void TestAdvance()
        {

        }
        private List<Standing> getMockStandings()
        {
            return new List<Standing>()
            {
                new Standing { Division="div0",TeamName="team0", G1Score=121, G2Score=121, G3Score=121 }, // 363, 3-0, division winner
                new Standing { Division="div0",TeamName="team1", G1Score=121, G2Score=120, G3Score=121 }, // 362, 2-1
                new Standing { Division="div0",TeamName="team2", G1Score=120, G2Score=120, G3Score=121 }, // 361, 1-2, but close games
                new Standing { Division="div0",TeamName="team3", G1Score=100, G2Score=96,  G3Score=87  }, // 283, 0-3
                new Standing { Division="div0",TeamName="team4", G1Score=100, G2Score=105, G3Score=112 }, // 317, 0-3
                new Standing { Division="div1",TeamName="team5", G1Score=121, G2Score=121, G3Score=120 }, // 362, 2-1, division winner
                new Standing { Division="div1",TeamName="team6", G1Score=121, G2Score=121, G3Score=119 }, // 361, 2-1 but slightly worse
                new Standing { Division="div1",TeamName="team7", G1Score=121, G2Score=90,  G3Score=80  }, // 291, 2-1 but bad losses
                new Standing { Division="div1",TeamName="team8", G1Score=121, G2Score=96,  G3Score=112 }, // 329, 1-2
                new Standing { Division="div1",TeamName="team9", G1Score=120, G2Score=120, G3Score=120 }, // 360, 0-3 but close games
            };
        }
    }

}
