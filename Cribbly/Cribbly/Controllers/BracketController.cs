using Cribbly.Data;
using Cribbly.Models;
using Cribbly.Models.Gameplay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cribbly.Controllers
{
    public class BracketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int _numTeams = 16;
        private Bracket bracket;

        public BracketController(ApplicationDbContext context)
        {
            _context = context;
            if (_context.BracketTeams.Count() > 0)
            {
                // the bracket has been seeded
                bracket = new Bracket(_context.BracketTeams.ToList());
            }
        }

        public IActionResult Index()
        {
            if (bracket != null)
            {
                return View(bracket.Rounds);
            }
            return View(new Dictionary<int, BracketTeam[]>());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Seed()
        {
            var standings = _context.Standings.ToList();
            if (standings.Count < _numTeams)
            {
                // ope, not enough standings, return early
                return Redirect("/Bracket");
            }
            bracket = new Bracket(standings, _numTeams);
            _context.BracketTeams.AddRange(bracket.Teams);
            _context.SaveChanges();
            return Redirect("/Bracket");
        }

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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/Bracket/Advance/")]
        public IActionResult Advance(int round, int seed)
        {
            if (bracket == null)
            {
                return Redirect("/Bracket");
            }
            var bt = bracket.Rounds[round].Where(t => t.Seed == seed).First();
            var ok = bracket.Advance(bt);
            if (ok)
            {
                _context.Update(bt);
                _context.SaveChanges();
            }
            return Redirect("/Bracket");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/Bracket/Unadvance/")]
        public IActionResult Unadvance(int round, int seed)
        {
            if (bracket == null)
            {
                return Redirect("/Bracket");
            }
            var bt = bracket.Rounds[round].Where(t => t.Seed == seed).First();
            var ok = bracket.Unadvance(bt);
            if (ok)
            {
                _context.Update(bt);
                _context.SaveChanges();
            }
            return Redirect("/Bracket");
        }
    }
}
