using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Cribbly.Models
{
    public class Team
    {
        [Key]
        private int id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Team name cannot be more than 20 characters")]
        public string Name { get; set; }
        [Required]
        public string[] Members { get; set; }
        [Required]
        public string Division { get; set; }
    }
}
