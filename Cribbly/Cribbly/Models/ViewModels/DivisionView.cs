using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Data;

namespace Cribbly.Models.ViewModels
{
    public class DivisionView
    {
        public ApplicationDbContext _context { get; set; }
        public List<Standing> _standings { get; set; }
        public List<Division> _divisions { get; set; }

        public DivisionView(ApplicationDbContext context)
        {
            _context = context;
            _standings = context.Standings.ToList();
            _divisions = context.Divisions.ToList();
        }
    }
}
