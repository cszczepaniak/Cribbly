using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Data;

namespace Cribbly.Models
{
    public class UserDataView
    {
        public ApplicationDbContext _context { get; set; }
        public Team _team { get; set; }
#nullable enable
        public Standing? _standing { get; set; }


        public UserDataView(ApplicationDbContext context, Team team, Standing? standing)
        {
            _context = context;
            _team = team;
            _standing = standing;
        }
    }
#nullable disable
}
