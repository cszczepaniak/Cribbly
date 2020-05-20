using System.ComponentModel.DataAnnotations;
using Cribbly.Models;

namespace Cribbly.Data
{
    public class TeamDataModel
    {
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public string Player1ID { get; set; }
        public string Player2ID { get; set; }
        public int PlayInGame1ID { get; set; }
        public int PlayInGame2ID { get; set; }
        public int PlayInGame3ID { get; set; }
        public Division Division { get; set; }
        public int TournamentSeed { get; set; }
        public int TournamentRound { get; set; }
    }
}