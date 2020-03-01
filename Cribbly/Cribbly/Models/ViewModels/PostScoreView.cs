using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Models.Gameplay;

namespace Cribbly.Models.ViewModels
{
    public class PostScoreView
    {
        public int Game1Score { get; set; }
        public int Game2Score { get; set; }
        public int Game3Score { get; set; }
        public int GameNumber { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public Standing standing { get; set; }
        public List<PlayInGame> Games { get; set; }
    }
}
