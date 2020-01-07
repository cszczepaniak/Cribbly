using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models.Gameplay
{
    public class BracketTeam
    {
        // Primary key in db
        [Key]
        public int Seed { get; set; }
        public int Round { get; set; }
        public string TeamName { get; set; }
        public BracketTeam() { }

        public BracketTeam(int seed, string teamName)
        {
            Seed = seed;
            TeamName = teamName;
            Round = 1;
        }
    }

    public class BracketPlaceholder : BracketTeam
    {
        public BracketPlaceholder()
        {
            TeamName = "";
        }
    }
}