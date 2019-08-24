using Cribbly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models.Gameplay
{
    public class Bracket
    {
        public int Id { get; set; }
        public bool IsSeeded { get; set; }
        // TODO migrate the database to add standing ids
        public List<int> StandingIds { get; set; }
        public List<Standing> Standings { get; set; }
        public Bracket()
        {
        }

        public Bracket(ApplicationDbContext context, List<int> standingIds)
        {
            StandingIds = standingIds;
            Standings = new List<Standing>();
            foreach(int id in standingIds)
            {
                Standings.Add(context.Standings.Find(id));
            }
            IsSeeded = true;
        }
    }
}
