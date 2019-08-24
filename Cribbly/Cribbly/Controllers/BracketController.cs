using Cribbly.Data;
using Cribbly.Models;
using Cribbly.Models.Gameplay;
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
        private const int _numTeams = 32;
        private bool _seeded = false;

        public BracketController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bracket = _context.Bracket.FirstOrDefault();
            return View(bracket);
        }

        // GET: /Bracket/SeedBracket
        //[Authorize(Roles = "Admin")]
        public IActionResult SeedBracket()
        {
            var standings = _context.Standings.ToList();
            // Get the pool of teams who made the cut
            var bracketPool = GetBracketPool(standings);
            // Add a seed to the sorted bracket pool members
            // TODO update these members in the database
            bracketPool.Zip(Enumerable.Range(1, _numTeams), (s, i) => s.Seed = i);

            var bracket = new Bracket(_context, bracketPool.Select(s => s.id).ToList());
            _context.Bracket.Add(bracket);
            _context.SaveChanges();
            return Redirect("/Bracket");
        }

        private List<Standing> GetBracketPool(List<Standing> standings)
        {
            var bracketTeams = new List<Standing>();
            // TODO standings aren't set - do something in that case
            var divisions = standings.Select(s => s.Division).Distinct().Where(d => d != null).ToList();
            // First add the first team in each division to the bracket pool
            if (divisions.Count > 0)
            {
                foreach (var d in divisions)
                {
                    var divisionStandings = standings.Where(s => s.Division.Equals(d)).OrderBy(s => s.TotalWins);
                    bracketTeams.Add(divisionStandings.First());
                    standings.Remove(divisionStandings.First());
                }
            }
            // Then fill the rest of the pool with top overall remaining teams
            var remaining = standings.OrderBy(s => s.TotalWinLoss);
            bracketTeams.AddRange(remaining.Take(_numTeams - bracketTeams.Count()));
            return bracketTeams.OrderBy(s => s.TotalWinLoss).ToList();
        }

    }
}
