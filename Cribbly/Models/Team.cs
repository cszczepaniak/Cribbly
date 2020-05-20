using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cribbly.Models
{
    public class Team
    {
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [MinLength(2)]
        [MaxLength(2)]
        public List<Player> Players { get; set; }

        // TeamPlayInGames represents the many-to-many relationship between teams and play in games
        [MinLength(0)]
        [MaxLength(3)]
        public List<TeamPlayInGame> TeamPlayInGames { get; set; }
        // PlayInGames is a helper prop to grab the play in games from the join table
        public List<PlayInGame> PlayInGames
        {
            get => TeamPlayInGames.Select(tg => tg.PlayInGame).ToList();
            set => TeamPlayInGames = value.Select(v => new TeamPlayInGame()
            {
                PlayInGameID = v.ID
            }).ToList();
        }
        public Division Division { get; set; }
        public int TournamentSeed { get; set; }
        public int TournamentRound { get; set; }
    }
}