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
    //Require user to be logged in to access any endpoint below
    [Authorize]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GamesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var games = _context.PlayInGames.ToList();
            var _3waygames = _context.PlayInGames.OfType<_3WayGame>().ToList();

            if (games.Count == 0)
            {
                return RedirectToAction(nameof(CreateGamesSetup));
            }

            GamesView model = new GamesView(games, _3waygames);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateGamesSetup()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateGames()
        {
            //Get standings and divisions
            var standings = _context.Standings.ToList();
            var divisions = _context.Divisions.ToList();

            //Combinations so everyone in a 4 team division plays each other once
            int[][] div4gameCodes = new int[][] 
            {
              new int[] { 0, 1, 1 }, new int[] { 2, 3, 1 },
              new int[] { 0, 2, 2 }, new int[] { 1, 3, 2 },
              new int[] { 0, 3, 3 }, new int[] { 1, 2, 3 }
            };

            //division of 3
            int[][] div3gameCodes = new int[][] 
            {
              new int[] { 0, 1, 1 }, new int[] { 0, 2, 2 }, new int[] { 1, 2, 2 }
            };

            //division of 5
            int[][] div5gameCodes = new int[][]
            {
              new int[] { 0, 1, 1 }, new int[] { 2, 3, 1 },
              new int[] { 0, 2, 2 }, new int[] { 1, 4, 2 },
              new int[] { 0, 3, 3 }, new int[] { 1, 2, 3 }, 
            };

            foreach (var div in divisions)
            {
                //Get all teams in the division
                var teams = standings.Where(m => m.Division == div.DivName).ToList();

                switch (teams.Count)
                {
                    case 5:
                        foreach (int[] code in div5gameCodes)
                        {
                            //Create new game
                            var newGame = new PlayInGame()
                            {
                                Team1Id = teams[code[0]].id,
                                Team1Name = teams[code[0]].TeamName,
                                Team2Id = teams[code[1]].id,
                                Team2Name = teams[code[1]].TeamName,
                                Division = div.DivName,
                                GameNumber = code[2],
                                WinningTeamId = 0
                            };
                            //Add to DB
                            _context.PlayInGames.Add(newGame);
                        }

                        _3WayGame a = new _3WayGame()
                        {
                            Team1Id = teams[2].id,
                            Team1Name = teams[2].TeamName,
                            Team2Id = teams[3].id,
                            Team2Name = teams[3].TeamName,
                            Team3Id = teams[4].id,
                            Team3Name = teams[4].TeamName,
                            Division = div.DivName,
                            WinningTeamId = 0
                        };
                        _context.PlayInGames.Add(a);
                        break;

                    case 4:
                        foreach (int[] code in div4gameCodes)
                        {
                            //Create new game
                            var newGame = new PlayInGame()
                            {
                                Team1Id = teams[code[0]].id,
                                Team1Name = teams[code[0]].TeamName,
                                Team2Id = teams[code[1]].id,
                                Team2Name = teams[code[1]].TeamName,
                                Division = div.DivName,
                                GameNumber = code[2],
                                WinningTeamId = 0
                            };
                            //Add to DB
                            _context.PlayInGames.Add(newGame);
                        }
                        break;

                    case 3:
                        //Configure games for a 3 team division
                        foreach (int[] code in div3gameCodes)
                        {
                            //Create new game
                            var newGame = new PlayInGame()
                            {
                                Team1Id = teams[code[0]].id,
                                Team1Name = teams[code[0]].TeamName,
                                Team2Id = teams[code[1]].id,
                                Team2Name = teams[code[1]].TeamName,
                                Division = div.DivName,
                                GameNumber = code[2],
                                WinningTeamId = 0
                            };
                            //Add to DB
                            _context.PlayInGames.Add(newGame);
                        }

                        _3WayGame a4 = new _3WayGame()
                        {
                            Team1Id = teams[0].id,
                            Team1Name = teams[0].TeamName,
                            Team2Id = teams[1].id,
                            Team2Name = teams[1].TeamName,
                            Team3Id = teams[2].id,
                            Team3Name = teams[2].TeamName,
                            Division = div.DivName,
                            WinningTeamId = 0
                        };
                        _context.PlayInGames.Add(a4);

                        break;
                    default:
                        //Something went wrong making the divisions
                        break;
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

            var games = _context.PlayInGames.Where(m => m.Team1Id == userId || m.Team2Id == userId).OrderBy(m => m.GameNumber).ToList();
            model.Games = games;
            model.standing = standing;

            return View(model);
        }

        [HttpPost]
        public IActionResult PostScore(PostScoreView model)
        {
            int i = 0;
            int userId = _context.ApplicationUsers.FirstOrDefault(m => m.Email == _userManager.GetUserName(User)).TeamId;
            string username = _context.ApplicationUsers.FirstOrDefault(m => m.Email == _userManager.GetUserName(User)).UserName;

            //Find your team's standing
            var standing = _context.Standings.FirstOrDefault(m => m.id == model.TeamId);
            var games = _context.PlayInGames.Where(m => m.Team1Id == userId || m.Team2Id == userId).OrderBy(m => m.GameNumber).ToList();

            foreach (PlayInGame game in games)
            {
                if (game.Team1Name == standing.TeamName)
                {
                    //User is team1 in the gameData object
                    try
                    {
                        FindWinner(game, model, standing, 1, i);
                    }
                    catch
                    {
                        ModelState.AddModelError("Game", "Error posting score. Please confirm with the other team that scores were submitted correctly.");
                        return RedirectToAction("PostScore");
                    }

                }
                else
                {
                    //User is team2 in the gameData object
                    try
                    {
                        FindWinner(game, model, standing, 2, i);
                    }
                    catch
                    {
                        ModelState.AddModelError("Game", "Error posting score. Please confirm with the other team that scores were submitted correctly.");
                        return RedirectToAction("PostScore");
                    }

                }

                game.LastUpdated = DateTime.Now;
                game.UpdatedBy = username;
                i++;
            }




            _context.SaveChanges();

            return RedirectToAction("MyTeam", "Teams", new { id = model.TeamId });
        }

        public void FindWinner(PlayInGame gameData, PostScoreView model, Standing standing, int key, int i)
        {
            char resultCode = 'X';
            char oppResultCode = 'X';
            int oppTeamScore = -1;
            int teamScore = -1;
            string oppTeamName = "";
            List<int> teamScores = new List<int>{ model.Game1Score, model.Game2Score, model.Game3Score };

            //Set variables for the opposite team name and score 
            if (key == 1)
            {
                oppTeamScore = gameData.Team2TotalScore;
                teamScore = teamScores[i];
                gameData.Team1TotalScore = teamScore;
                oppTeamName = gameData.Team2Name;

            } else if (key == 2)
            {
                oppTeamScore = gameData.Team1TotalScore;
                teamScore = teamScores[i];
                gameData.Team2TotalScore = teamScore;
                oppTeamName = gameData.Team1Name;
            }

            if (oppTeamScore == 121 && teamScore == 121)
            {
                //Both teams said they won. Throw error
                throw new System.ArgumentException("Bad value was provided");
            }

            switch (oppTeamScore)
            {
                case 0:
                    //Other team has not posted their score yet. Return notification
                    break;
                case 121:
                    //Other team won, find score difference
                    gameData.ScoreDifference = oppTeamScore - teamScore;
                    gameData.WinningTeamName = oppTeamName;
                    gameData.WinningTeamId = _context.Teams.FirstOrDefault(m => m.Name == oppTeamName).Id;
                    resultCode = 'L';
                    oppResultCode = 'W';
                    break;
                default:
                    //Your team won. Double check to make sure you submitted a score of 121
                    if (teamScore == 121)
                    {
                        gameData.ScoreDifference = teamScore - oppTeamScore;
                        gameData.WinningTeamName = model.TeamName;
                        gameData.WinningTeamId = model.TeamId;
                        resultCode = 'W';
                        oppResultCode = 'L';
                    }
                    //Neither team has a score of 121. Something is wrong
                    else
                    {
                        throw new System.ArgumentException("Bad value was provided");
                    }
                    break;
            }
            //Fill in the appropriate properties depending on which game number is being posted
            switch (gameData.GameNumber)
            {
                case 1:
                    standing.G1Score = teamScore;
                    standing.G1WinLoss = resultCode;
                    _context.Standings.FirstOrDefault(m => m.TeamName == oppTeamName).G1WinLoss = oppResultCode;
                    break;

                case 2:
                    standing.G2Score = teamScore;
                    standing.G2WinLoss = resultCode;
                    _context.Standings.FirstOrDefault(m => m.TeamName == oppTeamName).G2WinLoss = oppResultCode;
                    break;

                case 3:
                    standing.G3Score = teamScore;
                    standing.G2WinLoss = resultCode;
                    _context.Standings.FirstOrDefault(m => m.TeamName == oppTeamName).G3WinLoss = oppResultCode;
                    break;
            }

            standing.TotalScore = standing.G1Score + standing.G2Score + standing.G3Score;
            _context.SaveChanges();
        }
        /*
         * ==============================
         * EDIT GAME SCORE (Admin)
         * ==============================
         */
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditGame(int? id)
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