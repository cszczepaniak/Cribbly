﻿using System;
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
            int[][] gameCodes = new int[][] 
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
                ArgumentOutOfRangeException IsOutOfRange = null;
                //Get all teams in the division
                var teams = standings.Where(m => m.Division == div.DivName).ToList();

                foreach (int[] code in gameCodes)
                {
                    try
                    {
                        if (teams.Count == 5)
                        {
                            IsOutOfRange = new ArgumentOutOfRangeException();
                            break;
                        }

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
                    catch(ArgumentOutOfRangeException e)
                    {
                        IsOutOfRange = e;
                        break;
                    }
                }
                    
                if (IsOutOfRange != null)
                {
                    //Handle exception based on how many teams are left over
                    var gamesToRemove = _context.PlayInGames.Where(m => m.Division == div.DivName);

                    switch (teams.Count)
                    {
                        case 5:
                            //Remove created games, and reconfigure one division for 5 teams
                            _context.PlayInGames.RemoveRange(gamesToRemove);

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

                        case 3:
                            //Configure games for a 3 team division
                            _context.PlayInGames.RemoveRange(gamesToRemove);

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

            var game = _context.PlayInGames.FirstOrDefault(m => m.GameNumber == model.GameNumber && m.Team1Id == userId || m.Team2Id == userId);
            model.Game = game;

            return View(model);
        }

        [HttpPost]
        public IActionResult PostScore(PostScoreView model)
        {
            string username = _context.ApplicationUsers.FirstOrDefault(m => m.Email == _userManager.GetUserName(User)).UserName;

            //Client side validation logic ensures data is clean
            //Find your team's standing
            var standing = _context.Standings.FirstOrDefault(m => m.id == model.TeamId);
            //Find the correct PlayInGame object
            PlayInGame gameData = _context.PlayInGames
                .FirstOrDefault(m => m.Team1Id == model.TeamId || m.Team2Id == model.TeamId && m.GameNumber == model.GameNumber); 
            
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

            gameData.LastUpdated = DateTime.Now;
            gameData.UpdatedBy = username;
            _context.SaveChanges();

            return RedirectToAction("MyTeam", "Teams", new { id = model.TeamId });
        }

        public void FindWinner(PlayInGame gameData, PostScoreView model, Standing standing, int key)
        {
            char resultCode = 'X';
            char oppResultCode = 'X';
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
                    oppResultCode = 'W';
                    break;
                default:
                    //Your team won. Double check to make sure you submitted a score of 121
                    if (model.TotalScore == 121)
                    {
                        gameData.ScoreDifference = model.TotalScore - oppTeamScore;
                        gameData.WinningTeamName = model.TeamName;
                        gameData.WinningTeamId = model.TeamId;
                        resultCode = 'W';
                        oppResultCode = 'L';
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
                    _context.Standings.FirstOrDefault(m => m.TeamName == oppTeamName).G1WinLoss = oppResultCode;
                    break;

                case 2:
                    standing.G2Score = model.TotalScore;
                    standing.G2WinLoss = resultCode;
                    _context.Standings.FirstOrDefault(m => m.TeamName == oppTeamName).G2WinLoss = oppResultCode;
                    break;

                case 3:
                    standing.G3Score = model.TotalScore;
                    standing.G2WinLoss = resultCode;
                    _context.Standings.FirstOrDefault(m => m.TeamName == oppTeamName).G3WinLoss = oppResultCode;
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