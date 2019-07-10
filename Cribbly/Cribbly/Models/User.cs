using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Cribbly.Models
{
    public class User : IdentityUser
    {
        private int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
