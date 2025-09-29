using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Predictvie.Modals
{
    public class MatchData
    {
        [LoadColumn(0)]
        public float Corners { get; set; }

        [LoadColumn(1)]
        public float Bookings { get; set; }

        // Additional features that may influence the prediction
        [LoadColumn(2)]
        public float HomeTeamForm { get; set; } // Average corners in last N matches for home team

        [LoadColumn(3)]
        public float AwayTeamForm { get; set; } // Average corners in last N matches for away team

        [LoadColumn(4)]
        public float HomeTeamAverageCorners { get; set; } // Historical average corners for home team

        [LoadColumn(5)]
        public float AwayTeamAverageCorners { get; set; } // Historical average corners for away team

        [LoadColumn(6)]
        public float HomeTeamAverageBookings { get; set; } // Historical average bookings for home team

        [LoadColumn(7)]
        public float AwayTeamAverageBookings { get; set; } // Historical average bookings for away team

        [LoadColumn(8)]
        public bool IsHomeGame { get; set; } // 1 if home game, 0 if away game

    }
}
