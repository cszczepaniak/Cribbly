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
            _context.BracketTeams.AddRange(getBracketPool(standings));
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
            var bracket = getBracket(_context.BracketTeams.ToList());
            var team = getTeamInRound(round, seed, bracket);
            if (team == null)
            {
                return Redirect("Teams/TeamNotFound");
            }
            if (team.Round < getNumRounds(_numTeams) && !team.IsEliminated())
            {
                team.Round++;
                _context.Update(team);
                _context.SaveChanges();
            }
            return Redirect("/Bracket");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/Bracket/Unadvance/")]
        public IActionResult Unadvance(int round, int seed)
        {
            var bracket = getBracket(_context.BracketTeams.ToList());
            var team = getTeamInRound(round, seed, bracket);
            if (team == null)
            {
                return Redirect("Teams/TeamNotFound");
            }
            if (team.Round > 1 && !team.IsEliminated())
            {
                team.Round--;
                _context.Update(team);
                _context.SaveChanges();
            }
            return Redirect("/Bracket");
        }



        private int getNumRounds(int numTeams)
        {
            var n = 0;
            while (numTeams > 1)
            {
                numTeams >>= 1;
                n++;
            }
            return n;
        }

        private BracketTeam getTeamInRound(int round, int seed, Dictionary<int, BracketTeam[]> bracket)
        {
            var teamsInRound = bracket[round];
            var team = teamsInRound.Where(t => t.Seed == seed).First();
            return team;
        }
    }
}
