using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Cribbly.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Team name cannot be more than 20 characters")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Player One")]
        public string PlayerOne { get; set; }
        [Required]
        [Display(Name = "Player Two")]
        public string PlayerTwo { get; set; }
    }
}
