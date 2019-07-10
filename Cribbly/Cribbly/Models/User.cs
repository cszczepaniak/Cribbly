using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models
{
    public class User
    {
        private int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
