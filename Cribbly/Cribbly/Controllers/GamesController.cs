using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cribbly.Models;
using Cribbly.Models.Gameplay;
using Cribbly.Models.ViewModels;
using Cribbly.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Cribbly.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GamesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
         [HttpGet]
        public IActionResult PostScore()
        {
            int userId = _context.ApplicationUsers.FirstOrDefault(m => m.Email == _userManager.GetUserName(User)).TeamId;
            var standing = _context.Standings.FirstOrDefault(m => m.id == userId);

            PostScoreView model = new PostScoreView();

            //Check for game 1 score data
            if (standing.G1Score == 0)
            {
                model.GameNumber = 1;
            }
            //If it exists, check for game 2 score data
            else if (standing.G2Score == 0)
            {
                model.GameNumber = 2;
            }
            //If that exists, check for game 3 score data
            else if (standing.G3Score == 0)
            {
                model.GameNumber = 3;
            }
            //If that exists, return error page
            else
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult PostScore(PostScoreView model)
        {
            //Client side validation logic ensures data is clean
            //Find your team's standing
            var standing = _context.Standings.FirstOrDefault(m => m.id == model.TeamId);
            //Find the correct PlayInGame object
            PlayInGame gameData = _context.PlayInGames
                .FirstOrDefault(m => m.Team1Name == standing.TeamName || m.Team2Name == standing.TeamName);
            
            if (gameData.Team1Name == standing.TeamName)
            {
                //User is team1 in the gameData object
                gameData.Team1TotalScore = model.TotalScore;
                FindWinner(gameData, model, standing, 1);
            }
            else
            {
                //User is team2 in the gameData object
                gameData.Team2TotalScore = model.TotalScore;
                FindWinner(gameData, model, standing, 2);
            }


            return RedirectToRoute("/Teams/MyTeam");
        }

        public void FindWinner(PlayInGame gameData, PostScoreView model, Standing standing, int key)
        {
            char resultCode = 'X';
            int oppTeamScore = -1;
            string oppTeamName = "";

            //Set variables for the opposite team name and score 
            if (key == 1)
            {
                oppTeamScore = gameData.Team2TotalScore;
                oppTeamName = gameData.Team2Name;

            } else if (key == 2)
            {
                oppTeamScore = gameData.Team1TotalScore;
                oppTeamName = gameData.Team1Name;
            }

            switch (oppTeamScore)
            {
                case 0:
                    //Other team has not posted their score yet. Return notification
                    break;
                case 121:
                    //Other team won, find score difference
                    gameData.ScoreDifference = oppTeamScore - model.TotalScore;
                    gameData.WinningTeamName = oppTeamName;
                    gameData.WinningTeamId = _context.Teams.FirstOrDefault(m => m.Name == oppTeamName).Id;
                    resultCode = 'L';
                    break;
                default:
                    //Your team won. Double check to make sure you submitted a score of 121
                    if (model.TotalScore == 121)
                    {
                        gameData.ScoreDifference = model.TotalScore - oppTeamScore;
                        gameData.WinningTeamName = model.TeamName;
                        gameData.WinningTeamId = model.TeamId;
                        resultCode = 'W';
                    }
                    //Neither team has a score of 121. Something is wrong
                    else
                    {
                        //Error
                    }
                    break;
            }
            //Fill in the appropriate properties depending on which game number is being posted
            switch (model.GameNumber)
            {
                case 1:
                    standing.G1Score = model.TotalScore;
                    standing.G1WinLoss = resultCode;
                    break;

                case 2:
                    standing.G2Score = model.TotalScore;
                    standing.G2WinLoss = resultCode;
                    break;

                case 3:
                    standing.G3Score = model.TotalScore;
                    standing.G2WinLoss = resultCode;
                    break;
            }

            standing.TotalScore += model.TotalScore;
            _context.SaveChanges();
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