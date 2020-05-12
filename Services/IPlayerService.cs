using Cribbly.Models;

namespace Cribbly.Services
{
    public interface IPlayerService
    {
        public Player GetPlayerByID(string id);

        public Player GetByFirstAndLastName(string first, string last);
    }
}