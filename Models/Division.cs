using System.Collections.Generic;

namespace Cribbly.Models
{
    public class Division
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
    }
}