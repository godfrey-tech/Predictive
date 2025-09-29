using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class MatchDataV2
    {
        public DateTime DateOne { get; set; }
        public string HostHomeTeam { get; set; }
        public string AwayTeam { get; set; }

        // Home team stats
        public float HostHomeGoalsAvg { get; set; }
        public float HostHomeGoalsConcededAvg { get; set; }
        public float HostHomeWinsLast5 { get; set; }
        public float HostHomeLossesLast5 { get; set; }
        public float HostHomeDrawsLast5 { get; set; }
        public float HostHomeAverageGoalsScoredLast5 { get; set; }

        // Away team stats
        public float HostAwayGoalsAvg { get; set; }
        public float HostAwayGoalsConcededAvg { get; set; }
        public float HostAwayWinsLast5 { get; set; }
        public float HostAwayLossesLast5 { get; set; }
        public float HostAwayDrawsLast5 { get; set; }
        public float HostAwayAverageGoalsScoredLast5 { get; set; }

        // Recent form metrics for the last 5 matches (home or away)
        public float HostWinsLast5 { get; set; }
        public float HostLossesLast5 { get; set; }
        public float HostDrawsLast5 { get; set; }
        public float HostAverageGoalsScoredLast5 { get; set; }

        // Recent form metrics for the last 5 matches (home or away)
        public float OpponentWinsLast5 { get; set; } 
        public float OpponentLossesLast5 { get; set; } 
        public float OpponentDrawsLast5 { get; set; } 
        public float OpponentAverageGoalsScoredLast5 { get; set; }


        //// Opponent home stats
        public float OpponentHomeGoalsAvg { get; set; } // Average goals scored by opponent at home
        public float OpponentHomeGoalsConcededAvg { get; set; } // Average goals conceded by opponent at home
        public float OpponentHomeWinsLast5 { get; set; } // Wins for opponent in last 5 home matches
        public float OpponentHomeLossesLast5 { get; set; } // Losses for opponent in last 5 home matches
        public float OpponentHomeDrawsLast5 { get; set; } // Draws for opponent in last 5 home matches
        public float OpponentHomeAverageGoalsScoredLast5 { get; set; } // Average goals scored by opponent in last 5 home matches

        // Opponent away stats
        public float OpponentAwayGoalsAvg { get; set; } // Average goals scored by opponent when playing away
        public float OpponentAwayGoalsConcededAvg { get; set; } // Average goals conceded by opponent when playing away
        public float OpponentAwayWinsLast5 { get; set; } // Wins for opponent in last 5 away matches
        public float OpponentAwayLossesLast5 { get; set; } // Losses for opponent in last 5 away matches
        public float OpponentAwayDrawsLast5 { get; set; } // Draws for opponent in last 5 away matches
        public float OpponentAwayAverageGoalsScoredLast5 { get; set; } // Average goals scored by opponent in last 5 away matches

        // Head-to-head stats
        public float HeadToHeadWinsHome { get; set; }
        public float HeadToHeadWinsAway { get; set; }
        public float HeadToHeadDraws { get; set; }

        // Match outcome
        public string Outcome { get; set; }
    }

}
