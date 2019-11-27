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
        private const int _numTeams = 32;
        private const int _numRounds = 6;

        public BracketController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.BracketTeams.ToList());
        }

        // This would ideally be a post method, but unsure how to actually
        // get the client to send a post at the moment.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Seed()
        {
            var standings = _context.Standings.ToList();
            _context.BracketTeams.AddRange(getBracketPool(standings));
            _context.SaveChanges();
            System.Console.WriteLine("!!!!!!!SEEDING BRACKET!!!!!");
            return Redirect("/Bracket");
        }

        // This would ideally be a post method, but unsure how to actually
        // get the client to send a post at the moment.
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/Bracket/Advance/{seed}")]
        public IActionResult Advance(int seed)
        {
            var team = _context.BracketTeams.Where(t => t.Seed == seed).First();
            if (team.Round < _numRounds)
            {
                team.Round++;
                _context.Update(team);
                _context.SaveChanges();
            }
            return Redirect("/Bracket");
        }

        // This would ideally be a post method, but unsure how to actually
        // get the client to send a post at the moment.
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

    }
}
