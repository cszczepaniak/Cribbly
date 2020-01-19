using Cribbly.Data;
using Cribbly.Models;
using Cribbly.Models.Gameplay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
TODO make this work

Given a list of Standing objects resulting from the prelim games, we need
to:
  1. Pick 32 Standings based on their prelim performance
  2. Assign a seed to these standings
  3. Store the tournament teams in the db somehow
  4. Be able to keep track of their tournament performance (are they moving on?)

Model concept:
- Seed (primary key in bracket table)
- Team (has team name and team member info)
- Round (which round is this team in currently?)

*/

namespace Cribbly.Controllers
{
    public class BracketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int _numTeams = 16;

        public BracketController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var teams = _context.BracketTeams.ToList();
            var b = getBracket(teams);
            return View(b);
        }

        // This would ideally be a post method, but unsure how to actually
        // get the client to send a post at the moment.

        //to do the above, you need to a) have your iActionResult take a parameter and b) declare what your data model will 
        //be in the view using @model. see my PostScore view for an example
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Seed()
        {
            var standings = _context.Standings.ToList();
            _context.BracketTeams.AddRange(getBracketPool(standings));
            _context.SaveChanges();
            return Redirect("/Bracket");
        }

        // This would ideally be a post method, but unsure how to actually
        // get the client to send a post at the moment.

        //to do the above, you need to a) have your iActionResult take a parameter and b) declare what your data model will 
        //be in the view using @model. see my PostScore view for an example
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Unseed()
        {
            var bracketTeams = _context.BracketTeams.ToList();
            if (bracketTeams.Count > 0)
            {
                _context.BracketTeams.RemoveRange(bracketTeams);
            }
            _context.SaveChanges();
            return Redirect("/Bracket");
        }

        // This would ideally be a post method, but unsure how to actually
        // get the client to send a post at the moment.

        //to do the above, you need to a) have your iActionResult take a parameter and b) declare what your data model will 
        //be in the view using @model. see my PostScore view for an example
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/Bracket/Advance/")]
        public IActionResult Advance(int round, int seed)
        {
            //var team = _context.BracketTeams.Where(t => t.Seed == seed).First();
            var bracket = getBracket(_context.BracketTeams.ToList());
            var teamsInRound = bracket[round];
            var team = teamsInRound.Where(t => t.Seed == seed).First();
            if (team == null)
            {
                throw new Exception("Team not found!");
            }
            if (team.Round < getNumRounds(_numTeams) && !team.IsEliminated())
            {
                team.Round++;
                _context.Update(team);
                _context.SaveChanges();
            }
            return Redirect("/Bracket");
        }

        // This would ideally be a post method, but unsure how to actually
        // get the client to send a post at the moment.

        //to do the above, you need to a) have your iActionResult take a parameter and b) declare what your data model will 
        //be in the view using @model. see my PostScore view for an example
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/Bracket/Unadvance/{seed}")]
        public IActionResult Unadvance(int seed)
        {
            var team = _context.BracketTeams.Where(t => t.Seed == seed).First();
            if (team.Round > 1)
            {
                team.Round--;
                _context.Update(team);
                _context.SaveChanges();
            }
            return Redirect("/Bracket");
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

        private List<BracketTeam> getBracketPool(List<Standing> standings)
        {
            var currSeed = 1;
            var bracketTeams = new List<BracketTeam>();
            // TODO standings aren't set - do something in that case
            var divisions = standings.Select(s => s.Division).Distinct().Where(d => d != null).ToList();
            // First add the first team in each division to the bracket pool
            if (divisions.Count > 0)
            {
                foreach (var d in divisions)
                {
                    var divisionStandings = standings
                        .Where(s => s.Division.Equals(d))
                        .OrderByDescending(s => s.TotalWins)
                        .ThenByDescending(s => s.TotalWinLoss);
                    var t = divisionStandings.First();
                    bracketTeams.Add(new BracketTeam(currSeed, t.TeamName));
                    currSeed++;
                    standings.Remove(divisionStandings.First());
                }
            }
            // Then fill the rest of the pool with top overall remaining teams
            var remaining = standings.OrderByDescending(s => s.TotalWinLoss);
            var wildcards = remaining.Take(_numTeams - bracketTeams.Count()).ToArray();
            foreach (var wc in wildcards)
            {
                bracketTeams.Add(new BracketTeam(currSeed, wc.TeamName));
                currSeed++;
            }
            return bracketTeams;
        }

        private Dictionary<int, BracketTeam[]> getBracket(List<BracketTeam> teams)
        {
            var bTeams = getFirstRound(teams);

            // Add the first round to the dictionary
            var b = new Dictionary<int, BracketTeam[]>();
            b[1] = bTeams.ToArray();
            var nTeamsThisRound = bTeams.Count >> 1;
            var round = 2;
            // Loop through to build bracket
            while (nTeamsThisRound > 0)
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
        private int getNumRounds(int numTeams)
        {
            var n = 0;
            while (numTeams > 1)
            {
                numTeams >>= 1;
                n++;
            }
            System.Console.WriteLine("###### NUM ROUNDS: " + n);
            return n;
        }
    }
}
