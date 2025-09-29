using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class Score
    {
        public object Winner { get; set; } // Assuming winner can be null
        public string Duration { get; set; }
        public FullTime FullTime { get; set; }
        public HalfTime HalfTime { get; set; }
    }
}
