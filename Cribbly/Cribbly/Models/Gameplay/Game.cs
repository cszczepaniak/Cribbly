using System;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models.Gameplay
{
    public class Game
    {
        public int id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        [Display(Name = "Winning Team ID")]
        public int WinningTeamId { get; set; }
        [Display(Name = "Team 1 Name")]
        public string Team1Name { get; set; }
        [Display(Name = "Team 2 Name")]
        public string Team2Name { get; set; }
        [Display(Name = "Winning Team Name")]
        public string WinningTeamName { get; set; }
        //Metadata proerties
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}
