namespace Cribbly.Models
{
    // This class is an intermediate object to map the many-to-many relationship between teams and play in games
    public class TeamPlayInGame
    {
        public int TeamID { get; set; }
        public Team Team { get; set; }
        public int PlayInGameID { get; set; }
        public PlayInGame PlayInGame { get; set; }
    }
}