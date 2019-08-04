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
            //Find all standings 
            //Return all results to the view
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateStandingsSetup()
        {
            var teamlessUsers = _context.ApplicationUsers.Where(m => m.HasTeam == false).ToList();
            return View(teamlessUsers);
        }

        //Create standings and schedule 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateStandings()
        {
            //Find all users where HasTeam = 0
            var teamlessUsers = _context.ApplicationUsers.Where(m => m.HasTeam == false).ToList();
            List<Team> newTeams = new List<Team>();
            Random rnd = new Random();

            //For all users with no team, pair them up with another user who also does not have a team
            for (var i = 0; i < teamlessUsers.Count; i++)
            {
                try
                {
                    //Get first and last names of users
                    string p1 = teamlessUsers[i].FirstName + " " + teamlessUsers[i].LastName;
                    string p2 = teamlessUsers[i + 1].FirstName + " " + teamlessUsers[i + 1].LastName;

                    //Seed random 6 digit numbers for Team Id
                    int id = rnd.Next(100000, 999999);

                    //Create new team 
                    Team team = new Team()
                    {
                        Id = id,
                        Name = p1 + " / " + p2,
                        PlayerOne = p1,
                        PlayerTwo = p2
                    };

                    //Add new team to List
                    newTeams.Add(team);
                    //Remove two users from teamlessPlayers List
                    teamlessUsers.RemoveRange(i, 2);
                    //Set user team attributes to reflect them now being teamed up
                    teamlessUsers[i].HasTeam = teamlessUsers[i + 1].HasTeam = true;
                    teamlessUsers[i].TeamId = teamlessUsers[i + 1].TeamId = id;
                    //Reset i to 0 so we grab elements from the beginning of the list
                    i = 0;
                }
                //List out of bounds - this means there is a user left over
                catch
                {
                    //Alert admin that a user is left over and exit loop
                    break;
                }


            }
            //Add al new teams and save the DB
            _context.Teams.AddRange(newTeams);
            _context.SaveChanges();

            //Find all teams
            var allTeams = _context.Teams.ToList();

            //Populate all completed teams in the Standings table
            foreach (var team in allTeams)
            {
                //Create a new Standing obj
                Standing standing = new Standing
                {
                    TeamName = team.Name
                };
                //Add the Standing to the DB
                _context.Standings.Add(standing);
            }

            return RedirectToAction(nameof(GetAllStandings));
        }

        //Submit a score to be added to the standings
        [HttpPost]
        public IActionResult PostScore()
        {
            //Find your team's standing
            //Fill in the appropriate properties depending on which game number is being posted
            return RedirectToAction(nameof(GetStanding));
        }

        //Edit a score. Limited to Admins
        [Authorize(Roles = "Admin")]
        [HttpGet]
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
            return RedirectToAction(nameof(GetStanding));
        }

        //Get information on your team
        [HttpGet]
        public IActionResult GetStanding(UserDataView data)
        {
            //Get ApplicationDbContext
            //Get the standing object where the team name equals the given name
            return View();
        }
    }
}