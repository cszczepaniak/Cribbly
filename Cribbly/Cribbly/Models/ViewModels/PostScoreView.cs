﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models.ViewModels
{
    public class PostScoreView
    {
        public int GameNumber { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int TotalScore { get; set; }
    }
}
