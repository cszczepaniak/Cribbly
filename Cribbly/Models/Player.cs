using Microsoft.AspNetCore.Identity;

namespace Cribbly.Models
{
    public class Player : IdentityUser
    {
        public Team Team { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}