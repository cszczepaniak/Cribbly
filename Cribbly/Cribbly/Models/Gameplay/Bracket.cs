using System;
using System.Collections.Generic;
using System.Linq;

namespace Cribbly.Models.Gameplay
{
    public class Bracket
    {
        private int numTeams;
        private int numRounds;
        public int NumTeams
        {
            get
            {
                return numTeams;
            }
        }
        public List<BracketTeam> Teams { get; set; }
        public Dictionary<int, BracketTeam[]> Rounds { get; private set; }
        public Bracket(List<Standing> standings, List<PlayInGame> playInGames, int numTeams)
        {
            if ((int)Math.Ceiling(Math.Log((double)numTeams, 2.0)) ==
                (int)Math.Floor(Math.Log((double)numTeams, 2.0)))
            {
                this.numTeams = numTeams;
                numRounds = (int)Math.Log((double)numTeams, 2.0);
            }
            else
            {
                throw new ArgumentException();
            }
            Teams = seed(standings, playInGames);
            Rounds = buildBracket(Teams);
        }

        public BracketTeam Advance(BracketTeam team)
        {
            if (team.Round < numRounds)
            {
                var round = Rounds[team.Round];
                team.Round++;
                // find out which index this team is in the round
                int idx = -1;
                for (int i = 0; i < round.Length; i++)
                {
                    if (team.Seed == round[i].Seed)
                    {
                        idx = i;
                        break;
                    }
                    if (idx == -1)
                    {
                        // didn't find the team - weird!
                        throw new IndexOutOfRangeException();
                    }
                }
                var opp = idx % 2 == 0 ? round[idx + 1] : round[idx - 1];
                opp.Eliminate();
                Rounds[team.Round][idx / 2] = team;
            }
            return team;
        }

        private List<BracketTeam> seed(List<Standing> standings, List<PlayInGame> playInGames)
        {
            var currSeed = 1;
            var bracketTeams = new List<BracketTeam>();
            var divisions = standings
                .Select(s => s.Division)
                .Distinct()
                .Where(d => d != null)
                .ToList();
            // First add the first team in each division to the bracket pool
            if (divisions.Count > 0)
            {
                foreach (var d in divisions)
                {
                    var divisionStandings = standings
                        .Where(s => s.Division.Equals(d))
                        .OrderByDescending(s => s.TotalWins);
                    var t = divisionStandings.First();
                    bracketTeams.Add(new BracketTeam(currSeed, t.TeamName));
                    currSeed++;
                    standings.Remove(divisionStandings.First());
                }
            }
            // Then fill the rest of the pool with top overall remaining teams
            var wildcards = standings
                .OrderByDescending(s => s.G1Score + s.G2Score + s.G3Score)
                .Take(numTeams - bracketTeams.Count);
            foreach (var wc in wildcards)
            {
                bracketTeams.Add(new BracketTeam(currSeed, wc.TeamName));
                currSeed++;
            }
            return bracketTeams;
        }

        private Dictionary<int, BracketTeam[]> buildBracket(List<BracketTeam> teams)
        {
            var b = new Dictionary<int, BracketTeam[]>();
            if (teams.Count == 0)
            {
                return b;
            }
            var bTeams = getFirstRound(teams);
            // Add the first round to the dictionary
            b[1] = bTeams.ToArray();
            var nTeamsThisRound = bTeams.Count >> 1;
            var round = 2;
            // Loop through to build the rest of the bracket
            while (nTeamsThisRound > 1)
            {
                var thisRnd = new BracketTeam[nTeamsThisRound];
                var prevRnd = b[round - 1];
                for (int i = 0; i < prevRnd.Length - 1; i += 2)
                {
                    var t1 = prevRnd[i];
                    var t2 = prevRnd[i + 1];
                    if (t1.Round >= round)
                    {
                        thisRnd[i / 2] = t1;
                        t2.Eliminate();
                    }
                    else if (t2.Round >= round)
                    {
                        thisRnd[i / 2] = t2;
                        t1.Eliminate();
                    }
                    else
                    {
                        thisRnd[i / 2] = new BracketPlaceholder();
                    }
                }
                b[round] = thisRnd;
                nTeamsThisRound >>= 1;
                round++;
            }
            return b;
        }

        private List<BracketTeam> getFirstRound(List<BracketTeam> teams)
        {
            var bTeams = new List<BracketTeam>();
            for (int i = 0; i < teams.Count / 2; i++)
            {
                bTeams.Add(teams[i]);
                bTeams.Add(teams[teams.Count - i - 1]);
            }
            return bTeams;
        }
    }
}