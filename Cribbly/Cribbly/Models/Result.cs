using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int GameNum { get; set; }
        public int Score { get; set; }
        public char WinLoss { get; set; }
    }
}
