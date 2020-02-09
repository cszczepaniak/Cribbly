using System;
using System.Collections.Generic;
using System.Linq;

namespace Cribbly.Models.Gameplay
{
    public class Bracket
    {
        private readonly int NumRounds;
        public int NumTeams { get; private set; }
        public List<BracketTeam> Teams { get; set; }
        public Dictionary<int, BracketTeam[]> Rounds { get; private set; }

        // Use this constructor if the bracket has already been seeded
        public Bracket(List<BracketTeam> teams)
        {
            if (!IsPowerOfTwo(teams.Count()))
            {
                throw new ArgumentException();
            }
            NumTeams = teams.Count();
            NumRounds = (int)Math.Log(NumTeams, 2.0);
            Teams = teams;
            Rounds = BuildBracket(Teams);
        }

        // Use this constructor if the bracket has not been seeded
        public Bracket(List<Standing> standings)
        {
            NumRounds = (int)Math.Min(Math.Floor(Math.Log(standings.Count, 2)), 5);
            NumTeams = 1 << NumRounds;
            Teams = Seed(standings);
            Rounds = BuildBracket(Teams);
        }

        public bool Advance(BracketTeam team)
        {
            if (team.Round < NumRounds && !team.IsEliminated())
            {
                var round = Rounds[team.Round];
                team.Round++;
                int idx = IndexInRound(team, round);
                var opp = idx % 2 == 0 ? round[idx + 1] : round[idx - 1];
                opp.Eliminate();
                Rounds[team.Round][idx / 2] = team;
                return true;
            }
            return false;
        }

        public bool Unadvance(BracketTeam team)
        {
            if (team.Round > 1)
            {
                int thisRoundIdx = IndexInRound(team, Rounds[team.Round]);
                Rounds[team.Round][thisRoundIdx] = new BracketPlaceholder();
                team.Round--;

                var round = Rounds[team.Round];
                int prevRoundIdx = IndexInRound(team, round);
                var opp = prevRoundIdx % 2 == 0 ? round[prevRoundIdx + 1] : round[prevRoundIdx - 1];
                opp.Uneliminate();
                return true;
            }
            return false;
        }

        private bool IsPowerOfTwo(int n)
        {
            return (int)Math.Ceiling(Math.Log(n, 2.0)) == (int)Math.Floor(Math.Log(n, 2.0));
        }

        private int IndexInRound(BracketTeam team, BracketTeam[] round)
        {
            for (int i = 0; i < round.Length; i++)
            {
                if (team.Seed == round[i].Seed)
                {
                    return i;
                }
            }
            throw new IndexOutOfRangeException();
        }

        private List<BracketTeam> Seed(List<Standing> standings)
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
                .OrderByDescending(s => s.TotalScoreCalc)
                .ThenByDescending(s => s.TotalWins)
                .Take(NumTeams - bracketTeams.Count);
            foreach (var wc in wildcards)
            {
                bracketTeams.Add(new BracketTeam(currSeed, wc.TeamName));
                currSeed++;
            }
            return bracketTeams;
        }

        private Dictionary<int, BracketTeam[]> BuildBracket(List<BracketTeam> teams)
        {
            var b = new Dictionary<int, BracketTeam[]>();
            if (teams.Count == 0)
            {
                return b;
            }
            var bTeams = GetFirstRound(teams);
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

        private List<BracketTeam> GetFirstRound(List<BracketTeam> teams)
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