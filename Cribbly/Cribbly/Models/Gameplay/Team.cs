using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Cribbly.Models
{
    public class Team
    {
        public int Id { get; set; }
        [UniqueTeamName]
        [Required]
        [StringLength(50, ErrorMessage = "Team name cannot be more than 50 characters")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Player One")]
        public string PlayerOne { get; set; }
        [Required]
        [Display(Name = "Player Two")]
        public string PlayerTwo { get; set; }
    }
}
