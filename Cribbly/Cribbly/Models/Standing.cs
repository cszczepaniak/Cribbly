using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models
{
    public class Standing
    {
        private int id { get; set; }
        public Team Team { get; set; }
        public Result GameOneResult { get; set; }
        public Result GameTwoResult { get; set; }
        public Result GameThreeResult { get; set; }
        public int TotalScore { get; set; }
    }
}
