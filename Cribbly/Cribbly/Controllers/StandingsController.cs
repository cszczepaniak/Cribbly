using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
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
            var teamlessUsers = _context.ApplicationUsers.Where(m => m.HasTeam == false).ToList();
            List<Team> newTeams = new List<Team>();
            Random rnd = new Random();

            //For all users with no team, pair them up with another user who also does not have a team
            for (var i = 0; i < teamlessUsers.Count; i++)
            {
                string p1 = teamlessUsers[i].FirstName + " " + teamlessUsers[i].LastName;
                string p2 = teamlessUsers[i + 1].FirstName + " " + teamlessUsers[i + 1].LastName;
                //Seed random 6 digit numbers for Team Id
                int id = rnd.Next(100000, 999999);
                Team team = new Team()
                {
                    Id = id,
                    Name = p1 + " / " + p2,
                    PlayerOne = p1,
                    PlayerTwo = p2
                };
                newTeams.Add(team);
                teamlessUsers.RemoveRange(i, 2);
                teamlessUsers[i].HasTeam = teamlessUsers[i + 1].HasTeam = true;
                teamlessUsers[i].TeamId = teamlessUsers[i + 1].TeamId = id;
                i = 0;

            }
            _context.Teams.AddRange(newTeams);
            _context.SaveChanges();
            //Throw up an alert and display left over user if we have an odd number of teamless players

            //Find all teams
            var allTeams = _context.Teams.ToList();
            //Populate all completed teams in the Standings table
            foreach (var team in allTeams)
            {
                Standing standing = new Standing();
                standing.TeamName = team.Name;
                _context.Standings.Add(standing);
            }
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