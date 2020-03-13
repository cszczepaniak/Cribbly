using System.ComponentModel.DataAnnotations;

namespace Cribbly.Data.Models
{
    public class Team
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Player1ID { get; set; }
        [Required]
        public int Player2ID { get; set; }
        [Range(0, 121)]
        public int Game1Score { get; set; }
        [Range(0, 121)]
        public int Game2Score { get; set; }
        [Range(0, 121)]
        public int Game3Score { get; set; }
    }
}
