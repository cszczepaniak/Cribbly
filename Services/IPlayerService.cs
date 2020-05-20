using System.Collections.Generic;
using Cribbly.Models;

namespace Cribbly.Services
{
    public interface IPlayerService
    {
        Player GetPlayerByID(string id);
        List<Player> GetPlayersByIDs(List<string> ids);

        Player GetByFirstAndLastName(string first, string last);
    }
}