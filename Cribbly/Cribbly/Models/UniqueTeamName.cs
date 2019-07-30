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
            //Get database context
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            //Get teams where the name is equal to the input property
            var teamNames = context.Teams.FirstOrDefault(n => n.Name == (string)value);
            //If teamNames is not null, then the team name is taken
            if (teamNames != null)
            {
                //Return Validation error
                return new ValidationResult("Sorry, that team name is taken");
            }
            return base.GetValidationResult(value, new ValidationContext(this));
        }
    }
}
