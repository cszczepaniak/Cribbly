using Cribbly.Data;
using Cribbly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Controllers
{
    public class BracketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BracketController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult SeedBracket(List<Standing> standings)
        {
            // Get the pool of teams who made the cut
            var bracketPool = GetBracketPool(standings);
            // Add a seed to the sorted bracket pool members
            bracketPool.Zip(Enumerable.Range(1, 32), (s, i) => s.Seed = i);
            return View();
        }

        private List<Standing> GetBracketPool(List<Standing> standings)
        {
            // Should this be hard coded?
            var nTeams = 32;
            var bracketTeams = new List<Standing>();
            var divisions = standings.Select(s => s.Division).Distinct();
            // First add the first team in each division to the bracket pool
            foreach (var d in divisions)
            {
                var divisionStandings = standings.Where(s => s.Division.Equals(d)).OrderBy(s => s.TotalWins);
                bracketTeams.Add(divisionStandings.First());
                standings.Remove(divisionStandings.First());
            }
            // Then fill the rest of the pool with top overall remaining teams
            var remaining = standings.OrderBy(s => s.TotalWinLoss);
            bracketTeams.AddRange(remaining.Take(nTeams - bracketTeams.Count()));
            return bracketTeams.OrderBy(s => s.TotalWinLoss).ToList();
        }

    }
}
