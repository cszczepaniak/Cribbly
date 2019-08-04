using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models
{
    public class Standing
    {
        public int id { get; set; }
        public string TeamName { get; set; }
        public string Division { get; set; }
        public int G1Score { get; set; }
        public char G1WinLoss { get; set; }
        public int G2Score { get; set; }
        public char G2WinLoss { get; set; }
        public int G3Score { get; set; }
        public char G3WinLoss { get; set; }
        public int Seed { get; set; }
        public int TotalScore { get; set; }
        public int TotalWinLoss
        {
            get { return (new int[] { G1WinLoss, G2WinLoss, G3WinLoss }).Sum(); }
        }
        public int TotalWins
        {
            get { return (new int[] { G1Score, G2Score, G3Score }).Where(s => s == 121).Count(); }
        }

        public Standing()
        {
            G1Score = G2Score = G3Score = TotalScore = 0;
            Seed = -1;
        }
    }
}
