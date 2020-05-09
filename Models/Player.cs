using Microsoft.AspNetCore.Identity;

namespace Cribbly.Data
{
    public class Player : IdentityUser
    {
        public Team Team { get; set; }
    }
}