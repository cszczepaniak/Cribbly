using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models
{
    public class Team
    {
        private int id { get; set; }
        public string Name { get; set; }
        public string[] Members { get; set; }
        public string Division { get; set; }
    }
}
