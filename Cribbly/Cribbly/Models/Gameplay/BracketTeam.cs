using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models
{
    class BracketTeam
    {
        // Primary key in db
        [Required]
        public int Seed { get; set; }
        // a list of rounds this team has been in
        public List<int> Rounds { get; set; }
        public string TeamName { get; set; }

        public BracketTeam(int seed, string teamName)
        {
            Seed = seed;
            TeamName = teamName;
            Rounds = new List<int>();
            Rounds.Add(1);
        }
    }
}