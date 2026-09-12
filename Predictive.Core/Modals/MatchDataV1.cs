using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class MatchDataV1
    {
        public long Date { get; set; }
        public DateTime DateOne { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public float HomeGoalsAvg { get; set; } // Keep as double
        public float AwayGoalsAvg { get; set; } // Keep as double
        public float HomeGoalsConcededAvg { get; set; } // Keep as double
        public float AwayGoalsConcededAvg { get; set; } // Keep as double
        public string Outcome { get; set; } // Win, Draw, Loss

        // Recent form metrics
        public float HomeWinsLast5 { get; set; }
        public float HomeLossesLast5 { get; set; }
        public float HomeDrawsLast5 { get; set; }
        public float HomeAverageGoalsScoredLast5 { get; set; }

        public float AwayWinsLast5 { get; set; }
        public float AwayLossesLast5 { get; set; }
        public float AwayDrawsLast5 { get; set; }
        public float AwayAverageGoalsScoredLast5 { get; set; }

        public float HST { get; set; } // Home Shots on Target
        public float AST { get; set; } // Away Shots on Target

        // Head-to-head performance
        public float HeadToHeadWinsHome { get; set; }
        public float HeadToHeadWinsAway { get; set; }
        public float HeadToHeadDraws { get; set; }
    }
}
