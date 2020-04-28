using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Cribbly.Data;
using Cribbly.Models;
using Cribbly.Models.Gameplay;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace Cribbly.Controllers
{
    [Authorize]
    public class StandingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public List<Standing> standings;

        public StandingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * ==============================
         * GET ALL STANDINGS AND SCHEDULE (All)
         * ==============================
         */
        public IActionResult GetAllStandings()
        {
            //Get all standings
            var standings = _context.Standings.OrderByDescending(m => m.TotalScore).ToList();
            return View(standings);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult StandingsDisplay()
        {

            updateSeeds();
            //Get all standings
            var standings = _context.Standings.OrderByDescending(m => m.TotalScore).ToList();
            Response.Headers.Add("Refresh", "25");
            //Return all results to the view
            return View(standings);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateStandingsSetup()
        {
            var teamlessUsers = _context.ApplicationUsers.Where(m => m.HasTeam == false).ToList();
            return View(teamlessUsers);
        }

        /*
         * ==============================
         * CREATE STANDINGS AND SCHEDULE (Admin)
         * ==============================
         */
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ConfirmCreateStandings()
        {
            //Find all users where HasTeam = 0
            var teamlessUsers =  _context.ApplicationUsers.Where(m => m.HasTeam == false && m.LastName != "_admin").ToList();
            //Get model from internal method
            CreateStandingView model = PairPlayers(teamlessUsers, false);
            //return data 
            return View(model);
        }
        /*
         * ==============================
         * PAIR PLAYERS (Admin)
         * ==============================
         */
        [Authorize(Roles = "Admin")]
        public CreateStandingView PairPlayers(List<ApplicationUser> teamlessUsers, bool isConfirmed)
        {
            List<Team> newTeams = new List<Team>();
            Random rnd = new Random();
            bool PlayerLeftOver = false;

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
                    //User has confirmed, update the database
                    if (isConfirmed)
                    {
                        teamlessUsers[i].HasTeam = teamlessUsers[i + 1].HasTeam = true;
                        teamlessUsers[i].TeamId = teamlessUsers[i + 1].TeamId = id;
                    }

                    //Add new team to List
                    newTeams.Add(team);
                    //Remove two users from teamlessPlayers List
                    teamlessUsers.RemoveRange(i, 2);
                    //Reset i to 0 so we grab elements from the beginning of the list
                    i = -1;
                }

                //List out of bounds - this means there is a user left over
                catch
                {
                    //Alert admin that a user is left over
                    PlayerLeftOver = true;
                }

            }
            //User has confirmed, update the database
            if (isConfirmed)
            {
                //Add all new teams and save the DB 
                foreach (var team in newTeams)
                {
                    _context.Teams.Add(team);
                }
                _context.SaveChanges();
            }
            return new CreateStandingView(newTeams, PlayerLeftOver);

        }
        /*
         * ==============================
         * UNDO PAIR PLAYERS (Admin)
         * ==============================
         */
        [Authorize(Roles = "Admin")]
        public IActionResult CancelCreateStandings()
        {
            return RedirectToAction(nameof(CreateStandingsSetup));
        }

        /*
         * ==============================
         * COMMIT PLAYER CHANGES TO DB (Admin)
         * ==============================
         */
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateStandings()
        {
            var teamlessUsers = _context.ApplicationUsers.Where(m => m.HasTeam == false && m.LastName != "_admin").ToList();
            CreateStandingView model = PairPlayers(teamlessUsers, true);

            //Find all teams
            var allTeams = _context.Teams.ToList();

            //Populate all completed teams in the Standings table
            foreach (var team in allTeams)
            {
                //Create a new Standing obj
                Standing standing = new Standing(team);

                //Add the Standing to the DB
                _context.Standings.Add(standing);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(GetAllStandings));
        }

        private void updateSeeds()
        {
            var oldSeeds = _context.Standings.Where(m => m.Seed == 1);
            foreach (var seed in oldSeeds)
            {
                seed.Seed = 0;
            }
            var standings = _context.Standings.OrderByDescending(m => m.TotalScore).ToList();
            Bracket bracket = new Bracket(standings);
            List<BracketTeam> teamsInTourney = bracket.Teams;

            foreach (var team in teamsInTourney)
            {
                var standingObj = _context.Standings.FirstOrDefault(m => m.TeamName == team.TeamName);
                standingObj.Seed = 1;
            }

            _context.SaveChanges();
        }
    }
}