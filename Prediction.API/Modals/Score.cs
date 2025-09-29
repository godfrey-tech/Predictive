using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictvie.Modals
{
    public class Score
    {
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public int HomeTeamCorners { get; set; }
        public int AwayTeamCorners { get; set; }
        public int HomeTeamBookings { get; set; }
        public int AwayTeamBookings { get; set; }
    }
}
