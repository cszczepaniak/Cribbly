using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cribbly.Models;
using Cribbly.Data;

namespace Cribbly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ResetGames()
        {
            var playInGames = _context.PlayInGames.ToList();
            _context.PlayInGames.RemoveRange(playInGames);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ResetDivisions()
        {
            var divisions = _context.Divisions.ToList();
            _context.Divisions.RemoveRange(divisions);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ResetStandings()
        {
            var standings = _context.Standings.ToList();
            _context.Standings.RemoveRange(standings);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        //Clears all games, divisions, and teams. Users are NOT deleted
        public IActionResult ResetTournament()
        {
            var playInGames = _context.PlayInGames.ToList();
            _context.PlayInGames.RemoveRange(playInGames);

            var divisions = _context.Divisions.ToList();
            _context.Divisions.RemoveRange(divisions);

            var standings = _context.Standings.ToList();
            _context.Standings.RemoveRange(standings);

            var teams = _context.Teams.ToList();
            _context.Teams.RemoveRange(teams);

            var players = _context.ApplicationUsers.ToList();
            foreach (var player in players)
            {
                player.HasTeam = false;
                player.TeamId = 0;
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
