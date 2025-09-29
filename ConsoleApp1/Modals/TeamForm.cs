using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class TeamForm
    {
        public string TeamName { get; set; }
        public int Win { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalScored { get; set; }
        public int GoalConceded { get; set; }
    }
}
