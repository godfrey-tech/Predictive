using System;
using System.Collections.Generic;

namespace Predictive.Bookings.Integrations.SportMonks
{
    // SportMonks returns full official team names ("Manchester United"); this
    // project's CSVs (football-data.co.uk convention) use short names ("Man United").
    // Normalizes the former to the latter so a SportMonks-discovered fixture can be
    // looked up against historical CSV data. All 5 SportMonks-plan leagues verified
    // against real upcoming fixtures and this project's actual CSV team lists as of
    // 2026-09-13 — a team absent from both sides (newly promoted/relegated, not yet
    // in the CSV's tracked seasons) is expected to fall back to defaults, not a bug.
    // Extend as new mismatches surface each season.
    public static class SportMonksTeamNameNormalizer
    {
        private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
        {
            // Premier League
            { "AFC Bournemouth", "Bournemouth" },
            { "Brighton & Hove Albion", "Brighton" },
            { "Leeds United", "Leeds" },
            { "Manchester City", "Man City" },
            { "Manchester United", "Man United" },
            { "Newcastle United", "Newcastle" },
            { "Nottingham Forest", "Nott'm Forest" },
            { "Tottenham Hotspur", "Tottenham" },
            { "West Ham United", "West Ham" },
            { "Wolverhampton Wanderers", "Wolves" },

            // Bundesliga — verified against real upcoming SportMonks fixtures vs this
            // project's Bundesliga CSV team list.
            { "FC Bayern München", "Bayern Munich" },
            { "Borussia Mönchengladbach", "M'gladbach" },
            { "TSG Hoffenheim", "Hoffenheim" },
            { "Bayer 04 Leverkusen", "Leverkusen" },
            { "Eintracht Frankfurt", "Ein Frankfurt" },
            { "FSV Mainz 05", "Mainz" },
            { "SC Freiburg", "Freiburg" },
            { "Hamburger SV", "Hamburg" },
            { "FC Köln", "FC Koln" },
            { "VfB Stuttgart", "Stuttgart" },
            { "Borussia Dortmund", "Dortmund" },
            { "FC Union Berlin", "Union Berlin" },
            { "FC Augsburg", "Augsburg" },

            // Ligue 1
            { "Angers SCO", "Angers" },
            { "Paris Saint Germain", "Paris SG" },
            { "LOSC Lille", "Lille" },
            { "Olympique Lyonnais", "Lyon" },
            { "Olympique Marseille", "Marseille" },

            // Serie A
            { "AC Milan", "Milan" },

            // La Liga
            { "Athletic Club", "Ath Bilbao" },
            { "Atlético de Madrid", "Ath Madrid" },
            { "Deportivo Alavés", "Alaves" },
            { "Celta de Vigo", "Celta" },
            { "Real Betis", "Betis" },
            { "Espanyol", "Espanol" },
            { "FC Barcelona", "Barcelona" },
            { "Rayo Vallecano", "Vallecano" },
            { "Real Sociedad", "Sociedad" },
        };

        public static string Normalize(string sportMonksName)
            => Map.TryGetValue(sportMonksName, out var csvName) ? csvName : sportMonksName;
    }
}
