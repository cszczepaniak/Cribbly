using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Cribbly.Data;

namespace Cribbly.Models
{
    public class UniqueTeamName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var teamNames = context.Teams.FirstOrDefault(n => n.Name == (string)value);
            if (teamNames != null)
            {
                return new ValidationResult("Sorry, that team name is taken");
            }
            return base.GetValidationResult(value, new ValidationContext(this));
        }
    }
}
