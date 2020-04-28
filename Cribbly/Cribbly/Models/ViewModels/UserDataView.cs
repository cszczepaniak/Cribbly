using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Data;
using Cribbly.Models.Gameplay;

namespace Cribbly.Models
{
    public class UserDataView
    {
        public ApplicationDbContext _context { get; set; }
        public Team _team { get; set; }
#nullable enable
        public Standing? _standing { get; set; }

        public List<PlayInGame>? _games { get; set; }
        public List<_3WayGame>? _3waygames { get; set; }

        public UserDataView(ApplicationDbContext context, Team team, Standing? standing, List<PlayInGame>? games, List<_3WayGame>? threewaygames)
        {
            _context = context;
            _team = team;
            _standing = standing;
            _games = games;
            _3waygames = threewaygames;
        }
    }
#nullable disable
}
