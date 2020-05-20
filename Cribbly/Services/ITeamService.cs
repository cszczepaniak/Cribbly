using Cribbly.Models;
using System.Threading.Tasks;

namespace Cribbly.Services
{
    public interface ITeamService
    {
        Team GetTeamByID(int id);
        Task<Team> CreateTeamAsync(Player p1, Player p2);

        void ReportWinAsync(Team winner, Team loser, int loserScore);
    }
}