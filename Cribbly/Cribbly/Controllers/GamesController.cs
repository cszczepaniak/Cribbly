using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cribbly.Models;
using Cribbly.Models.Gameplay;
using Cribbly.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Cribbly.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var games = _context.PlayInGames.ToList();

            if (games.Count == 0)
            {
                return RedirectToAction(nameof(CreateGamesSetup));
            }

            return View(games);
        }

        public IActionResult CreateGamesSetup()
        {
            return View();
        }

        public IActionResult CreateGames()
        {
            //Get standings and divisions
            var standings = _context.Standings.ToList();
            var divisions = _context.Divisions.ToList();

            //Combinations so everyone in a 4 team division plays each other once
            int[][] gameCodes = new int[][] 
            { new int[] { 1, 2 }, new int[] { 1, 3 },
              new int[] { 1, 4 }, new int[] {2, 3},
              new int[] { 2, 4 }, new int[] { 3, 4 }
            };

            foreach (var div in divisions)
            {
                //Get all teams in the division
                var teams = standings.Where(m => m.Division == div.DivName).ToList();

                try
                {
                    foreach (int[] code in gameCodes)
                    {
                        //Create new game
                        var newGame = new PlayInGame()
                        {
                            Team1Id = teams[code[0]].id,
                            Team1Name = teams[code[0]].TeamName,
                            Team2Id = teams[code[1]].id,
                            Team2Name = teams[code[1]].TeamName,
                            WinningTeamName = null,
                            WinningTeamId = 0
                        };
                        //Add to DB
                        _context.PlayInGames.Add(newGame);
                    }
                }
                catch (Exception e)
                {
                    //Check to ensure that the exception was thrown due to a division with an odd number of teams
                    if (e is IndexOutOfRangeException)
                    {
                        //Handle exception based on how many teams are left over
                        switch(teams.Count)
                        {
                            case 1:
                                //Reconfigure one division to have 5 teams
                                break;
                            case 2:
                                //Reconfigure two divisions to have 5 teams
                                break;
                            case 3:
                                //Configure a 3 team division
                                break;
                            default:
                                //Something went wrong making the divisions
                                break;
                        }
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        /*
         * ==============================
         * SUBMIT GAME SCORE (User)
         * ==============================
         */
        public IActionResult PostScore()
        {
            //Check for game 1 score data
            //If it exists, check for game 2 score data
            //If that exists, check for game 3 score data
            //If that exists, return error page

            return View();
        }

        [HttpPost]
        public IActionResult PostScore(PlayInGame game)
        {
            //Find your team's standing
            //Fill in the appropriate properties depending on which game number is being posted
            return RedirectToRoute("/Teams/MyTeam");
        }
        /*
         * ==============================
         * EDIT GAME SCORE (Admin)
         * ==============================
         */
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditGame()
        {
            //Return the team that matches the appropriate Id
            return View();
        }
        /*
         * ==============================
         * SAVE EDITED SCORE (Admin)
         * ==============================
         */
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult SaveGame()
        {
            //Return the team that matches the appropriate Id
            return View();
        }
    }
}