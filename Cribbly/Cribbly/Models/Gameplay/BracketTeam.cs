using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models.Gameplay
{
    public class BracketTeam
    {
        // Primary key in db
        [Required]
        public int Seed { get; set; }
        // a list of rounds this team has been in
        public int Round { get; set; }
        public string TeamName { get; set; }

        public BracketTeam(int seed, string teamName)
        {
            Seed = seed;
            TeamName = teamName;
            Round = 1;
        }
    }
}