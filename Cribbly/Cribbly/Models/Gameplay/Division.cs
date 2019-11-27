using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cribbly.Models
{
    public class Division
    {
        public int Id { get; set; }
        [Display(Name = "Division Name")]
        public string DivName { get; set; }
    }
}
