using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Data;
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
            var team = new TeamDataModel()
            {
                Player1ID = p1.Id,
                Player2ID = p2.Id,
            };
            await db.TeamDataModels.AddAsync(team);
            return GetTeamWithPlayer(p1);
        }
        // TODO maybe this should be in PlayInGameService? :nods:
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

        public Team GetTeamWithPlayer(Player p)
        {
            var team = db.TeamDataModels.Where(tdm => tdm.Player1ID.Equals(p.Id) || tdm.Player2ID.Equals(p.Id)).FirstOrDefault();
            return teamFromDataModel(team);
        }

        public Team GetTeamByID(int id)
        {
            var team = db.TeamDataModels.Where(tdm => tdm.ID == id).FirstOrDefault();
            if (team == null)
            {
                throw new Exception("Team not found.");
            }
            return teamFromDataModel(team);
        }

        private Team teamFromDataModel(TeamDataModel team)
        {
            // TODO make a player service to fetch these instead of doing it below
            var p1 = db.Users.Where(p => p.Id == team.Player1ID).FirstOrDefault();
            var p2 = db.Users.Where(p => p.Id == team.Player2ID).FirstOrDefault();
            // TODO make a game service to fetch these instead of doing it below
            var g1 = db.PlayInGames.Where(g => g.ID == team.PlayInGame1ID).FirstOrDefault();
            var g2 = db.PlayInGames.Where(g => g.ID == team.PlayInGame2ID).FirstOrDefault();
            var g3 = db.PlayInGames.Where(g => g.ID == team.PlayInGame3ID).FirstOrDefault();
            return new Team()
            {
                ID = team.ID,
                Name = team.Name,
                Players = new List<Player> { p1, p2 },
                PlayInGames = new List<PlayInGame>() { g1, g2, g3 },
                Division = team.Division,
                TournamentSeed = team.TournamentSeed,
                TournamentRound = team.TournamentRound,
            };
        }
    }
}