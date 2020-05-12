using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Models;

namespace Cribbly.Services
{
    public class TeamService : ITeamService
    {
        private AppDbContext db;

        public TeamService(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<Team> CreateTeamAsync(Player p1, Player p2)
        {
            var team = new Team()
            {
                Players = new List<Player>() { p1, p2 }
            };
            await db.Teams.AddAsync(team);
            return team;
        }

        public async void ReportWinAsync(Team winner, Team loser, int loserScore)
        {
            if (loserScore < 0 || loserScore > Constants.WinningScore - 1)
            {
                throw new ArgumentException($"Opponent's score must be between 0 and {Constants.WinningScore - 1} since they lost.");
            }
            if (winner.PlayInGames.Count > Constants.NumPlayInGames - 1)
            {
                throw new Exception($"Team '{winner.Name}' already has {Constants.NumPlayInGames} play in games.");
            }
            if (loser.PlayInGames.Count > Constants.NumPlayInGames - 1)
            {
                throw new Exception($"Team '{loser.Name}' already has {Constants.NumPlayInGames} play in games.");
            }
            var game = new PlayInGame()
            {
                Teams = new List<Team>() { winner, loser },
                Scores = new List<int>() { Constants.WinningScore, loserScore }
            };
            await db.PlayInGames.AddAsync(game);
        }

        public Team GetTeamByID(int id) =>
            db.Teams.Where(t => t.ID == id).FirstOrDefault();
    }
}