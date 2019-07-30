using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cribbly.Data;
using Cribbly.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cribbly.Controllers
{
    [Authorize]
    public class StandingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StandingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //View master list for all tournament standings
        public IActionResult GetAllStandings()
        {
            //Get ApplicationDbContext
            //Find all standings 
            //Return all results to the view
            return View();
        }

        //Create standings and schedule 
        [Authorize(Roles = "Admin")]
        public IActionResult CreateStandings()
        {
            //Get ApplicationDbContext
            //Find all users where HasTeam = 0
            var teamlessUsers = _context.ApplicationUsers.Where(m => m.HasTeam == false);
            //Find all teams
            var allTeams = _context.Teams.ToList();
            //Populate all completed teams in the Standings table
            foreach (var team in allTeams)
            {
                Standing standing = new Standing();
                standing.TeamName = team.Name;
                _context.Standings.Add(standing);
            }
            //For all users with no team, pair them up with another user who also does not have a team
            foreach (var player in teamlessUsers)
            {
                //DateTime seed = new DateTime().Ticks;
                //Seed random 6 digit numbers for Team Id
                //Random rnd = new Random(seed);
                //int id = rnd.Next(100000, 999999);

            }
            //Throw up an alert and display left over user if we have an odd number of teamless players

            return View();
        }

        //Submit a score to be added to the standings
        public IActionResult PostScore()
        {
            //Get ApplicationDbContext
            //Find your team's standing
            //Fill in the appropriate properties depending on which game number is being posted
            return View();
        }

        //Edit a score. Limited to Admins
        [Authorize(Roles = "Admin")]
        public IActionResult EditScore(UserDataView data)
        {
            //Return the team that matches the appropriate Id
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult SaveScore(UserDataView data)
        {
            //Return the team that matches the appropriate Id
            return View();
        }

        //Get information on your team
        public IActionResult GetStanding(UserDataView data)
        {
            //Get ApplicationDbContext
            //Get the standing object where the team name equals the given name
            return View();
        }
    }
}