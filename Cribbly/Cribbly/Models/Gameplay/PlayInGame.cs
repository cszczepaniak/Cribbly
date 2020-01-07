using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models.Gameplay
{
    public class PlayInGame
    {
        public int id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        [Display(Name ="Team 1 Total Score")]
        public int Team1TotalScore { get; set; }
        [Display(Name = "Team 2 Total Score")]
        public int Team2TotalScore { get; set; }
        [Display(Name = "Winning Team ID")]
        public int WinningTeamId { get; set; }
        [Display(Name = "Score Difference")]
        public int ScoreDifference { get; set; }
        [Display(Name = "Game Number")]
        public int GameNumber { get; set; }
        [Display(Name = "Team 1 Name")]
        public string Team1Name { get; set; }
        [Display(Name = "Team 2 Name")]
        public string Team2Name { get; set; }
        [Display(Name = "Winning Team")]
        public string WinningTeamName { get; set; }
        [Display(Name = "Division")]
        public string Division { get; set; }
        //Metadata proerties
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}
