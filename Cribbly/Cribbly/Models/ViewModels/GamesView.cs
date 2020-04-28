using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Models.Gameplay;

namespace Cribbly.Models.ViewModels
{
    public class GamesView
    {
        public List<PlayInGame> _playingames { get; set; }
        public List<_3WayGame> _3waygames { get; set; }

        public GamesView(List<PlayInGame> playingames, List<_3WayGame> threewaygames)
        {
            _playingames = playingames;
            _3waygames = threewaygames;
        }
    }
}
