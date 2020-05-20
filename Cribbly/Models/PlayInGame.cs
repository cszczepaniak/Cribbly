using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cribbly.Models
{
    public class PlayInGame
    {
        public int ID { get; set; }

        // TeamPlayInGames represents the many-to-many relationship between teams and play in games
        [MinLength(0)]
        [MaxLength(3)]
        public List<TeamPlayInGame> TeamPlayInGames { get; set; }
        // PlayInGames is a helper prop to grab the teams from the join table
        public List<Team> Teams
        {
            get => TeamPlayInGames.Select(tg => tg.Team).ToList();
            set => TeamPlayInGames = value.Select(v => new TeamPlayInGame()
            {
                TeamID = v.ID
            }).ToList();
        }

        // TODO will the indices of scores and teams align properly? i.e. will team 0's score be at index 0?
        [MinLength(2)]
        [MaxLength(2)]
        [Range(0, 121)]
        public List<int> Scores { get; set; }
    }
}