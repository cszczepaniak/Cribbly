using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models.Gameplay
{
    public class PlayInGame
    {
        public int id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int Team1TotalScore { get; set; }
        public int Team2TotalScore { get; set; }
        public int WinningTeamId { get; set; }
        public int ScoreDifference { get; set; }
        public int GameNumber { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public string WinningTeamName { get; set; }
        //Metadata proerties
        public DateTime DateMade { get; set; }
        public int SubmittedBy { get; set; }
        public DateTime LastEdited { get; set; }
        public string EditedBy { get; set; }
    }
}
