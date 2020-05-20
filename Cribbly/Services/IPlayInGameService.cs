using System.Collections.Generic;
using Cribbly.Models;

namespace Cribbly.Services
{
    public interface IPlayInGameService
    {
        PlayInGame GetByID(int id);
        List<PlayInGame> GetByIDs(List<int> ids);
    }
}