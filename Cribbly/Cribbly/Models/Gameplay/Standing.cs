using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models
{
    public class Standing
    {
        public int id { get; set; }
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
        [Display(Name = "Division")]
        public string Division { get; set; }
        [Display(Name = "Game 1 Score")]
        public int G1Score { get; set; }
        [Display(Name = "Game 1 W/L")]
        public char G1WinLoss { get; set; }
        [Display(Name = "Game 2 Score")]
        public int G2Score { get; set; }
        [Display(Name = "Game 2 W/L")]
        public char G2WinLoss { get; set; }
        [Display(Name = "Game 3 Score")]
        public int G3Score { get; set; }
        [Display(Name = "Game 3 W/L")]
        public char G3WinLoss { get; set; }
        [Display(Name = "Total Score")]
        public int TotalScore { get; set; }
        public int TotalWinLoss
        {
            get { return (new int[] { G1WinLoss, G2WinLoss, G3WinLoss }).Sum(); }
        }
        public int TotalWins
        {
            get { return (new int[] { G1Score, G2Score, G3Score }).Where(s => s == 121).Count(); }
        }
        [Display(Name = "Seed")]
        public int Seed { get; set; }

        public Standing(Team team)
        {
            G1Score = G2Score = G3Score = TotalScore = 0;
            Seed = -1;
            G1WinLoss = 'X';
            G2WinLoss = 'X';
            G3WinLoss = 'X';
            id = team.Id;
            TeamName = team.Name;
        }

        public Standing()
        {

        }
    }
}
