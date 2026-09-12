using ConsoleApp1.Modals.HistoricalData.PremierLeague;
using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Mapper
{
    public static class MatchMapper
    {
        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV1(HistoricalMatchFromCSVFileFirst historicalMatch)
        {
            if (historicalMatch == null)
            {
                throw new ArgumentNullException(nameof(historicalMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = historicalMatch.Div,
                Date = historicalMatch.Date ?? DateTime.MinValue, // Default value if Date is null
                Time = null, // Time is not present in the source
                HomeTeam = historicalMatch.HomeTeam,
                AwayTeam = historicalMatch.AwayTeam,
                FTHG = historicalMatch.FTHG,
                FTAG = historicalMatch.FTAG,
                FTR = historicalMatch.FTR,
                HTHG = historicalMatch.HTHG,
                HTAG = historicalMatch.HTAG,
                HTR = historicalMatch.HTR,
                Referee = historicalMatch.Referee,
                HS = historicalMatch.HS,
                AS = historicalMatch.AS,
                HST = historicalMatch.HST,
                AST = historicalMatch.AST,
                HF = historicalMatch.HF,
                AF = historicalMatch.AF,
                HC = historicalMatch.HC,
                AC = historicalMatch.AC,
                HY = historicalMatch.HY,
                AY = historicalMatch.AY,
                HR = historicalMatch.HR,
                AR = historicalMatch.AR,
                B365H = historicalMatch.B365H,
                B365D = historicalMatch.B365D,
                B365A = historicalMatch.B365A,
                BWH = historicalMatch.BWH,
                BWD = historicalMatch.BWD,
                BWA = historicalMatch.BWA,
                IWH = historicalMatch.IWH,
                IWD = historicalMatch.IWD,
                IWA = historicalMatch.IWA,
                //LBH = historicalMatch.LBH,
                //LBD = historicalMatch.LBD,
                //LBA = historicalMatch.LBA,
                PSH = historicalMatch.PSH,
                PSD = historicalMatch.PSD,
                PSA = historicalMatch.PSA,
                WHH = historicalMatch.WHH,
                WHD = historicalMatch.WHD,
                WHA = historicalMatch.WHA,
                //SJH = historicalMatch.SJH,
                //SJD = historicalMatch.SJD,
                //SJA = historicalMatch.SJA,
                VCH = historicalMatch.VCH,
                VCD = historicalMatch.VCD,
                VCA = historicalMatch.VCA,
                //Bb1X2 = historicalMatch.Bb1X2,
                //BbMxH = historicalMatch.BbMxH,
                //BbAvH = historicalMatch.BbAvH,
                //BbMxD = historicalMatch.BbMxD,
                //BbAvD = historicalMatch.BbAvD,
                //BbMxA = historicalMatch.BbMxA,
                //BbAvA = historicalMatch.BbAvA,
                //BbOU = historicalMatch.BbOU,
                //BbMxOver25 = historicalMatch.BbMxOver25,
                //BbAvOver25 = historicalMatch.BbAvOver25,
                //BbMxUnder25 = historicalMatch.BbMxUnder25,
                //BbAvUnder25 = historicalMatch.BbAvUnder25,
                //BbAH = historicalMatch.BbAH,
                //BbAHh = historicalMatch.BbAHh,
                //BbMxAHH = historicalMatch.BbMxAHH,
                //BbAvAHH = historicalMatch.BbAvAHH,
                //BbMxAHA = historicalMatch.BbMxAHA,
                //BbAvAHA = historicalMatch.BbAvAHA,
                PSCH = historicalMatch.PSCH,
                PSCD = historicalMatch.PSCD,
                PSCA = historicalMatch.PSCA,
                // Additional properties not present in HistoricalMatchFromCSVFileFirst
                MaxH = null,
                MaxD = null,
                MaxA = null,
                AvgH = null,
                AvgD = null,
                AvgA = null,
                B365Over25 = null,
                B365Under25 = null,
                POver25 = null,
                PUnder25 = null,
                MaxOver25 = null,
                MaxUnder25 = null,
                AvgOver25 = null,
                AvgUnder25 = null,
                AHh = null,
                B365AHH = null,
                B365AHA = null,
                PAHH = null,
                PAHA = null,
                MaxAHH = null,
                MaxAHA = null,
                AvgAHH = null,
                AvgAHA = null,
                B365CH = null,
                B365CD = null,
                B365CA = null,
                BWCH = null,
                BWCD = null,
                BWCA = null,
                IWCH = null,
                IWCD = null,
                IWCA = null,
                WHCH = null,
                WHCD = null,
                WHCA = null,
                VCCH = null,
                VCCD = null,
                VCCA = null,
                MaxCH = null,
                MaxCD = null,
                MaxCA = null,
                AvgCH = null,
                AvgCD = null,
                AvgCA = null,
                B365COver25 = null,
                B365CUnder25 = null,
                PCOver25 = null,
                PCUnder25 = null,
                MaxCOver25 = null,
                MaxCUnder25 = null,
                AvgCOver25 = null,
                AvgCUnder25 = null,
                AHCh = null,
                B365CAHH = null,
                B365CAHA = null,
                PCAHH = null,
                PCAHA = null,
                MaxCAHH = null,
                MaxCAHA = null,
                AvgCAHH = null,
                AvgCAHA = null,
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV1(List<HistoricalMatchFromCSVFileFirst> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV1).ToList();
        }

        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileExtraLeague(HistoricalMatchFromCSVFileExtraLeague extraLeagueMatch)
        {
            if (extraLeagueMatch == null)
            {
                throw new ArgumentNullException(nameof(extraLeagueMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = extraLeagueMatch.League, // Assuming Div corresponds to League
                Date = extraLeagueMatch.Date,
                Time = extraLeagueMatch.Time,
                HomeTeam = extraLeagueMatch.Home,
                AwayTeam = extraLeagueMatch.Away,
                FTHG = extraLeagueMatch.HG,
                FTAG = extraLeagueMatch.AG,
                FTR = extraLeagueMatch.Res,
                PSCH = extraLeagueMatch.PSCH,
                PSCD = extraLeagueMatch.PSCD,
                PSCA = extraLeagueMatch.PSCA,
                MaxCH = extraLeagueMatch.MaxCH,
                MaxCD = extraLeagueMatch.MaxCD,
                MaxCA = extraLeagueMatch.MaxCA,
                AvgCH = extraLeagueMatch.AvgCH,
                AvgCD = extraLeagueMatch.AvgCD,
                AvgCA = extraLeagueMatch.AvgCA,
                //BFECH = extraLeagueMatch.BFECH,
                //BFECD = extraLeagueMatch.BFECD,
                //BFECA = extraLeagueMatch.BFECA
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileExtraLeague(List<HistoricalMatchFromCSVFileExtraLeague> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileExtraLeague).ToList();
        }

        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV9(HistoricalMatchFromCSVFileNinth ninthMatch)
        {
            if (ninthMatch == null)
            {
                throw new ArgumentNullException(nameof(ninthMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = ninthMatch.Div,
                Date = ninthMatch.Date,
                Time = ninthMatch.Time,
                HomeTeam = ninthMatch.HomeTeam,
                AwayTeam = ninthMatch.AwayTeam,
                FTHG = ninthMatch.FTHG,
                FTAG = ninthMatch.FTAG,
                FTR = ninthMatch.FTR,
                HTHG = ninthMatch.HTHG,
                HTAG = ninthMatch.HTAG,
                HTR = ninthMatch.HTR,
                HS = ninthMatch.HS,
                AS = ninthMatch.AS,
                HST = ninthMatch.HST,
                AST = ninthMatch.AST,
                HF = ninthMatch.HF,
                AF = ninthMatch.AF,
                HC = ninthMatch.HC,
                AC = ninthMatch.AC,
                HY = ninthMatch.HY,
                AY = ninthMatch.AY,
                HR = ninthMatch.HR,
                AR = ninthMatch.AR,
                B365H = ninthMatch.B365H,
                B365D = ninthMatch.B365D,
                B365A = ninthMatch.B365A,
                BWH = ninthMatch.BWH,
                BWD = ninthMatch.BWD,
                BWA = ninthMatch.BWA,
                //BFH = ninthMatch.BFH,
                //BFD = ninthMatch.BFD,
                //BFA = ninthMatch.BFA,
                PSH = ninthMatch.PSH,
                PSD = ninthMatch.PSD,
                PSA = ninthMatch.PSA,
                WHH = ninthMatch.WHH,
                WHD = ninthMatch.WHD,
                WHA = ninthMatch.WHA,
                //OneXBH = ninthMatch.OneXBH,
                //OneXBD = ninthMatch.OneXBD,
                //OneXBA = ninthMatch.OneXBA,
                MaxH = ninthMatch.MaxH,
                MaxD = ninthMatch.MaxD,
                MaxA = ninthMatch.MaxA,
                AvgH = ninthMatch.AvgH,
                AvgD = ninthMatch.AvgD,
                AvgA = ninthMatch.AvgA,
                B365Over25 = ninthMatch.B365Over25,
                B365Under25 = ninthMatch.B365Under25,
                POver25 = ninthMatch.POver25,
                PUnder25 = ninthMatch.PUnder25,
                MaxOver25 = ninthMatch.MaxOver25,
                MaxUnder25 = ninthMatch.MaxUnder25,
                AvgOver25 = ninthMatch.AvgOver25,
                AvgUnder25 = ninthMatch.AvgUnder25,
                AHh = ninthMatch.AHh,
                B365AHH = ninthMatch.B365AHH,
                B365AHA = ninthMatch.B365AHA,
                PAHH = ninthMatch.PAHH,
                PAHA = ninthMatch.PAHA,
                MaxAHH = ninthMatch.MaxAHH,
                MaxAHA = ninthMatch.MaxAHA,
                AvgAHH = ninthMatch.AvgAHH,
                AvgAHA = ninthMatch.AvgAHA,
                B365CH = ninthMatch.B365CH,
                B365CD = ninthMatch.B365CD,
                B365CA = ninthMatch.B365CA,
                BWCH = ninthMatch.BWCH,
                BWCD = ninthMatch.BWCD,
                BWCA = ninthMatch.BWCA,
                //BFCH = ninthMatch.BFCH,
                //BFCD = ninthMatch.BFCD,
                //BFCA = ninthMatch.BFCA,
                PSCH = ninthMatch.PSCH,
                PSCD = ninthMatch.PSCD,
                PSCA = ninthMatch.PSCA,
                WHCH = ninthMatch.WHCH,
                WHCD = ninthMatch.WHCD,
                WHCA = ninthMatch.WHCA,
                //OneXBCH = ninthMatch.OneXBCH,
                //OneXBCD = ninthMatch.OneXBCD,
                //OneXBCA = ninthMatch.OneXBCA,
                MaxCH = ninthMatch.MaxCH,
                MaxCD = ninthMatch.MaxCD,
                MaxCA = ninthMatch.MaxCA,
                AvgCH = ninthMatch.AvgCH,
                AvgCD = ninthMatch.AvgCD,
                AvgCA = ninthMatch.AvgCA,
                //BFECH = ninthMatch.BFECH,
                //BFECD = ninthMatch.BFECD,
                //BFECA = ninthMatch.BFECA,
                B365COver25 = ninthMatch.B365COver25,
                B365CUnder25 = ninthMatch.B365CUnder25
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV9(List<HistoricalMatchFromCSVFileNinth> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV9).ToList();
        }

        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV8(HistoricalMatchFromCSVFileEighth eighthMatch)
        {
            if (eighthMatch == null)
            {
                throw new ArgumentNullException(nameof(eighthMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = eighthMatch.Div,
                Date = eighthMatch.Date,
                HomeTeam = eighthMatch.HomeTeam,
                AwayTeam = eighthMatch.AwayTeam,
                FTHG = eighthMatch.FTHG,
                FTAG = eighthMatch.FTAG,
                FTR = eighthMatch.FTR,
                HTHG = eighthMatch.HTHG,
                HTAG = eighthMatch.HTAG,
                HTR = eighthMatch.HTR,
                HS = eighthMatch.HS,
                AS = eighthMatch.AS,
                HST = eighthMatch.HST,
                AST = eighthMatch.AST,
                HF = eighthMatch.HF,
                AF = eighthMatch.AF,
                HC = eighthMatch.HC,
                AC = eighthMatch.AC,
                HY = eighthMatch.HY,
                AY = eighthMatch.AY,
                HR = eighthMatch.HR,
                AR = eighthMatch.AR,
                B365H = eighthMatch.B365H,
                B365D = eighthMatch.B365D,
                B365A = eighthMatch.B365A,
                BWH = eighthMatch.BWH,
                BWD = eighthMatch.BWD,
                BWA = eighthMatch.BWA,
                IWH = eighthMatch.IWH,
                IWD = eighthMatch.IWD,
                IWA = eighthMatch.IWA,
                //LBH = eighthMatch.LBH,
                //LBD = eighthMatch.LBD,
                //LBA = eighthMatch.LBA,
                PSH = eighthMatch.PSH,
                PSD = eighthMatch.PSD,
                PSA = eighthMatch.PSA,
                WHH = eighthMatch.WHH,
                WHD = eighthMatch.WHD,
                WHA = eighthMatch.WHA,
                //SJH = eighthMatch.SJH,
                //SJD = eighthMatch.SJD,
                //SJA = eighthMatch.SJA,
                VCH = eighthMatch.VCH,
                VCD = eighthMatch.VCD,
                VCA = eighthMatch.VCA,
                //Bb1X2 = eighthMatch.Bb1X2,
                //BbMxH = eighthMatch.BbMxH,
                //BbAvH = eighthMatch.BbAvH,
                //BbMxD = eighthMatch.BbMxD,
                //BbAvD = eighthMatch.BbAvD,
                //BbMxA = eighthMatch.BbMxA,
                //BbAvA = eighthMatch.BbAvA,
                //BbOU = eighthMatch.BbOU,
                //BbMxOver25 = eighthMatch.BbMxOver25,
                //BbAvOver25 = eighthMatch.BbAvOver25,
                //BbMxUnder25 = eighthMatch.BbMxUnder25,
                //BbAvUnder25 = eighthMatch.BbAvUnder25,
                //BbAH = eighthMatch.BbAH,
                //BbAHh = eighthMatch.BbAHh,
                //BbMxAHH = eighthMatch.BbMxAHH,
                //BbAvAHH = eighthMatch.BbAvAHH,
                //BbMxAHA = eighthMatch.BbMxAHA,
                //BbAvAHA = eighthMatch.BbAvAHA,
                PSCH = eighthMatch.PSCH,
                PSCD = eighthMatch.PSCD,
                PSCA = eighthMatch.PSCA
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV8(List<HistoricalMatchFromCSVFileEighth> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV8).ToList();
        }
        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV7(HistoricalMatchFromCSVFileSeventh sixthMatch)
        {
            if (sixthMatch == null)
            {
                throw new ArgumentNullException(nameof(sixthMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = sixthMatch.Div,
                Date = sixthMatch.Date, // ?? DateTime.MinValue,
                HomeTeam = sixthMatch.HomeTeam,
                AwayTeam = sixthMatch.AwayTeam,
                FTHG = sixthMatch.FTHG,
                FTAG = sixthMatch.FTAG,
                FTR = sixthMatch.FTR,
                HTHG = sixthMatch.HTHG,
                HTAG = sixthMatch.HTAG,
                HTR = sixthMatch.HTR,
                B365H = sixthMatch.B365H,
                B365D = sixthMatch.B365D,
                B365A = sixthMatch.B365A,
                BWH = sixthMatch.BWH,
                BWD = sixthMatch.BWD,
                BWA = sixthMatch.BWA,
                IWH = sixthMatch.IWH,
                IWD = sixthMatch.IWD,
                IWA = sixthMatch.IWA,
                //LBH = sixthMatch.LBH,
                //LBD = sixthMatch.LBD,
                //LBA = sixthMatch.LBA,
                PSH = sixthMatch.PSH,
                PSD = sixthMatch.PSD,
                PSA = sixthMatch.PSA,
                WHH = sixthMatch.WHH,
                WHD = sixthMatch.WHD,
                WHA = sixthMatch.WHA,
                //SJH = sixthMatch.SJH,
                //SJD = sixthMatch.SJD,
                //SJA = sixthMatch.SJA,
                VCH = sixthMatch.VCH,
                VCD = sixthMatch.VCD,
                VCA = sixthMatch.VCA,
                //Bb1X2 = sixthMatch.Bb1X2,
                //BbMxH = sixthMatch.BbMxH,
                //BbAvH = sixthMatch.BbAvH,
                //BbMxD = sixthMatch.BbMxD,
                //BbAvD = sixthMatch.BbAvD,
                //BbMxA = sixthMatch.BbMxA,
                //BbAvA = sixthMatch.BbAvA,
                //BbOU = sixthMatch.BbOU,
                //BbMxOver25 = sixthMatch.BbMxOver25,
                //BbAvOver25 = sixthMatch.BbAvOver25,
                //BbMxUnder25 = sixthMatch.BbMxUnder25,
                //BbAvUnder25 = sixthMatch.BbAvUnder25,
                //BbAH = sixthMatch.BbAH,
                //BbAHh = sixthMatch.BbAHh,
                //BbMxAHH = sixthMatch.BbMxAHH,
                //BbAvAHH = sixthMatch.BbAvAHH,
                //BbMxAHA = sixthMatch.BbMxAHA,
                //BbAvAHA = sixthMatch.BbAvAHA,
                PSCH = sixthMatch.PSCH,
                PSCD = sixthMatch.PSCD,
                PSCA = sixthMatch.PSCA
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV7(List<HistoricalMatchFromCSVFileSeventh> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV7).ToList();
        }
        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV6(HistoricalMatchFromCSVFileSixth sixthMatch)
        {
            if (sixthMatch == null)
            {
                throw new ArgumentNullException(nameof(sixthMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = sixthMatch.Div,
                Date = sixthMatch.Date ?? DateTime.MinValue,
                HomeTeam = sixthMatch.HomeTeam ?? null,
                AwayTeam = sixthMatch.AwayTeam ?? null,
                FTHG = sixthMatch.FTHG,
                FTAG = sixthMatch.FTAG,
                FTR = sixthMatch.FTR ?? null,
                HTHG = sixthMatch.HTHG,
                HTAG = sixthMatch.HTAG,
                HTR = sixthMatch.HTR ?? null,
                //B365H = (double)sixthMatch.B365H,
                //B365D = (double)sixthMatch.B365D,
                //B365A = (double)sixthMatch.B365A,
                B365H = sixthMatch.B365H.HasValue ? (double)sixthMatch.B365H.Value : (double?)null,
                B365D = sixthMatch.B365D.HasValue ? (double)sixthMatch.B365D.Value : (double?)null,
                B365A = sixthMatch.B365A.HasValue ? (double)sixthMatch.B365A.Value : (double?)null,
                BWH = sixthMatch.BWH.HasValue ? (double)sixthMatch.BWH.Value : (double?)null,
                BWD = sixthMatch.BWD.HasValue ? (double)sixthMatch.BWD.Value : (double?)null,
                BWA = sixthMatch.BWA.HasValue ? (double)sixthMatch.BWA.Value : (double?)null,
                IWH = sixthMatch.IWH.HasValue ? (double)sixthMatch.IWH.Value : (double?)null,
                IWD = sixthMatch.IWD.HasValue ? (double)sixthMatch.IWD.Value : (double?)null,
                IWA = sixthMatch.IWA.HasValue ? (double)sixthMatch.IWA.Value : (double?)null,
                PSH = sixthMatch.PSH.HasValue ? (double)sixthMatch.PSH.Value : (double?)null,
                PSD = sixthMatch.PSD.HasValue ? (double)sixthMatch.PSD.Value : (double?)null,
                PSA = sixthMatch.PSA.HasValue ? (double)sixthMatch.PSA.Value : (double?)null,
                WHH = sixthMatch.WHH.HasValue ? (double)sixthMatch.WHH.Value : (double?)null,
                WHD = sixthMatch.WHD.HasValue ? (double)sixthMatch.WHD.Value : (double?)null,
                WHA = sixthMatch.WHA.HasValue ? (double)sixthMatch.WHA.Value : (double?)null,
                VCH = sixthMatch.VCH.HasValue ? (double)sixthMatch.VCH.Value : (double?)null,
                VCD = sixthMatch.VCD.HasValue ? (double)sixthMatch.VCD.Value : (double?)null,
                VCA = sixthMatch.VCA.HasValue ? (double)sixthMatch.VCA.Value : (double?)null,
                PSCH = sixthMatch.PSCH.HasValue ? (double)sixthMatch.PSCH.Value : (double?)null,
                PSCD = sixthMatch.PSCD.HasValue ? (double)sixthMatch.PSCD.Value : (double?)null,
                PSCA = sixthMatch.PSCA.HasValue ? (double)sixthMatch.PSCA.Value : (double?)null


                //Bb1X2 = (decimal)sixthMatch.Bb1X2,
                //BbMxH = (decimal)sixthMatch.BbMxH,
                //BbAvH = (decimal)sixthMatch.BbAvH,
                //BbMxD = (decimal)sixthMatch.BbMxD,
                //BbAvD = (decimal)sixthMatch.BbAvD,
                //BbMxA = (decimal)sixthMatch.BbMxA,
                //BbAvA = (decimal)sixthMatch.BbAvA,
                //BbOU = (decimal)sixthMatch.BbOU,
                //BbMxOver2_5 = (decimal)sixthMatch.BbMxOver2_5,
                //BbAvOver2_5 = (decimal)sixthMatch.BbAvOver2_5,
                //BbMxUnder2_5 = (decimal)sixthMatch.BbMxUnder2_5,
                //BbAvUnder2_5 = (decimal)sixthMatch.BbAvUnder2_5,
                //BbAH = (decimal)sixthMatch.BbAH,
                //BbAHh = (decimal)sixthMatch.BbAHh,
                //BbMxAHH = (decimal)sixthMatch.BbMxAHH,
                //BbAvAHH = (decimal)sixthMatch.BbAvAHH,
                //BbMxAHA = (decimal)sixthMatch.BbMxAHA,
                //BbAvAHA = (decimal)sixthMatch.BbAvAHA,
                //PSCH = (decimal)sixthMatch.PSCH,
                //PSCD = (decimal)sixthMatch.PSCD,
                //PSCA = (decimal)sixthMatch.PSCA
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV6(List<HistoricalMatchFromCSVFileSixth> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV6).ToList();
        }
        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV5(HistoricalMatchFromCSVFileFiveth historicalMatch)
        {
            if (historicalMatch == null)
            {
                throw new ArgumentNullException(nameof(historicalMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = historicalMatch.Div,
                Date = historicalMatch.Date, // Assuming Date is always present
                Time = null, // Time is not present in the source
                HomeTeam = historicalMatch.HomeTeam,
                AwayTeam = historicalMatch.AwayTeam,
                FTHG = historicalMatch.FTHG,
                FTAG = historicalMatch.FTAG,
                FTR = historicalMatch.FTR,
                HTHG = historicalMatch.HTHG,
                HTAG = historicalMatch.HTAG,
                HTR = historicalMatch.HTR,
                Referee = historicalMatch.Referee,
                HS = historicalMatch.HS,
                AS = historicalMatch.AS,
                HST = historicalMatch.HST,
                AST = historicalMatch.AST,
                HF = historicalMatch.HF,
                AF = historicalMatch.AF,
                HC = historicalMatch.HC,
                AC = historicalMatch.AC,
                HY = historicalMatch.HY,
                AY = historicalMatch.AY,
                HR = historicalMatch.HR,
                AR = historicalMatch.AR,
                B365H = (double?)historicalMatch.B365H,
                B365D = (double?)historicalMatch.B365D,
                B365A = (double?)historicalMatch.B365A,
                BWH = (double?)historicalMatch.BWH,
                BWD = (double?)historicalMatch.BWD,
                BWA = (double?)historicalMatch.BWA,
                //IWH = (double?)historicalMatch.IWH,
                //IWD = (double?)historicalMatch.IWD,
                //IWA = (double?)historicalMatch.IWA,
                PSH = (double?)historicalMatch.PSH,
                PSD = (double?)historicalMatch.PSD,
                PSA = (double?)historicalMatch.PSA,
                WHH = (double?)historicalMatch.WHH,
                WHD = (double?)historicalMatch.WHD,
                WHA = (double?)historicalMatch.WHA,
                //VCH = (double?)historicalMatch.VCH,
                //VCD = (double?)historicalMatch.VCD,
                //VCA = (double?)historicalMatch.VCA,
                //Bb1X2 = historicalMatch.Bb1X2,
                //BbMxH = historicalMatch.BbMxH,
                //BbAvH = historicalMatch.BbAvH,
                //BbMxD = historicalMatch.BbMxD,
                //BbAvD = historicalMatch.BbAvD,
                //BbMxA = historicalMatch.BbMxA,
                //BbAvA = historicalMatch.BbAvA,
                //BbOU = historicalMatch.BbOU,
                //BbMxGreaterThan2_5 = historicalMatch.BbMxGreaterThan2_5,
                //BbAvGreaterThan2_5 = historicalMatch.BbAvGreaterThan2_5,
                //BbMxLessThan2_5 = historicalMatch.BbMxLessThan2_5,
                //BbAvLessThan2_5 = historicalMatch.BbAvLessThan2_5,
                //BbAH = historicalMatch.BbAH,
                //BbAHh = historicalMatch.BbAHh,
                //BbMxAHH = historicalMatch.BbMxAHH,
                //BbAvAHH = historicalMatch.BbAvAHH,
                //BbMxAHA = historicalMatch.BbMxAHA,
                //BbAvAHA = historicalMatch.BbAvAHA,
                //PSCH = historicalMatch.PSCH,
                //PSCD = historicalMatch.PSCD,
                //PSCA = historicalMatch.PSCA,
                // Additional properties not present in HistoricalMatchFromCSVFileFourth
                MaxH = null,
                MaxD = null,
                MaxA = null,
                AvgH = null,
                AvgD = null,
                AvgA = null,
                B365Over25 = null,
                B365Under25 = null,
                POver25 = null,
                PUnder25 = null,
                MaxOver25 = null,
                MaxUnder25 = null,
                AvgOver25 = null,
                AvgUnder25 = null,
                AHh = null,
                B365AHH = null,
                B365AHA = null,
                PAHH = null,
                PAHA = null,
                MaxAHH = null,
                MaxAHA = null,
                AvgAHH = null,
                AvgAHA = null,
                B365CH = null,
                B365CD = null,
                B365CA = null,
                BWCH = null,
                BWCD = null,
                BWCA = null,
                //IWCH = null,
                //IWCD = null,
                //IWCA = null,
                WHCH = null,
                WHCD = null,
                WHCA = null,
                //VCCH = null,
                //VCCD = null,
                //VCCA = null,
                MaxCH = null,
                MaxCD = null,
                MaxCA = null,
                AvgCH = null,
                AvgCD = null,
                AvgCA = null,
                B365COver25 = null,
                B365CUnder25 = null,
                PCOver25 = null,
                PCUnder25 = null,
                MaxCOver25 = null,
                MaxCUnder25 = null,
                AvgCOver25 = null,
                AvgCUnder25 = null,
                AHCh = null,
                B365CAHH = null,
                B365CAHA = null,
                PCAHH = null,
                PCAHA = null,
                MaxCAHH = null,
                MaxCAHA = null,
                AvgCAHH = null,
                AvgCAHA = null,
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV5(List<HistoricalMatchFromCSVFileFiveth> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV5).ToList();
        }
        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV4(HistoricalMatchFromCSVFileFourth historicalMatch)
        {
            if (historicalMatch == null)
            {
                throw new ArgumentNullException(nameof(historicalMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = historicalMatch.Div,
                Date = historicalMatch.Date, // Assuming Date is always present
                Time = null, // Time is not present in the source
                HomeTeam = historicalMatch.HomeTeam,
                AwayTeam = historicalMatch.AwayTeam,
                FTHG = historicalMatch.FTHG,
                FTAG = historicalMatch.FTAG,
                FTR = historicalMatch.FTR,
                HTHG = historicalMatch.HTHG ?? 0,
                HTAG = historicalMatch.HTAG ?? 0,
                HTR = historicalMatch.HTR,
                Referee = historicalMatch.Referee,
                HS = (int)(historicalMatch.HS ?? 0),
                AS = (int)(historicalMatch.AS ?? 0),
                HST = (int)(historicalMatch.HST ?? 0),
                AST = (int)(historicalMatch.AST ?? 0),
                HF = (int)(historicalMatch.HF ?? 0),
                AF = (int)(historicalMatch.AF ?? 0),
                HC = (int)(historicalMatch.HC ?? 0),
                AC = (int)(historicalMatch.AC ?? 0),
                HY = (int)(historicalMatch.HY ?? 0),
                AY = (int)(historicalMatch.AY ?? 0),
                HR = (int)(historicalMatch.HR ?? 0),
                AR = (int)(historicalMatch.AR ?? 0),
                B365H = (double?)historicalMatch.B365H ?? 0,
                B365D = (double?)historicalMatch.B365D ?? 0,
                B365A = (double?)historicalMatch.B365A ?? 0,
                BWH = (double?)historicalMatch.BWH ?? 0,
                BWD = (double?)historicalMatch.BWD ?? 0,
                BWA = (double?)historicalMatch.BWA ?? 0,
                IWH = (double?)historicalMatch.IWH ?? 0,
                IWD = (double?)historicalMatch.IWD ?? 0,
                IWA = (double?)historicalMatch.IWA ?? 0,
                PSH = (double?)historicalMatch.PSH ?? 0,
                PSD = (double?)historicalMatch.PSD ?? 0,
                PSA = (double?)historicalMatch.PSA ?? 0,
                WHH = (double?)historicalMatch.WHH ?? 0,
                WHD = (double?)historicalMatch.WHD ?? 0,
                WHA = (double?)historicalMatch.WHA ?? 0,
                VCH = (double?)historicalMatch.VCH ?? 0,
                VCD = (double?)historicalMatch.VCD ?? 0,
                VCA = (double?)historicalMatch.VCA ?? 0,
                //Bb1X2 = historicalMatch.Bb1X2,
                //BbMxH = historicalMatch.BbMxH,
                //BbAvH = historicalMatch.BbAvH,
                //BbMxD = historicalMatch.BbMxD,
                //BbAvD = historicalMatch.BbAvD,
                //BbMxA = historicalMatch.BbMxA,
                //BbAvA = historicalMatch.BbAvA,
                //BbOU = historicalMatch.BbOU,
                //BbMxGreaterThan2_5 = historicalMatch.BbMxGreaterThan2_5,
                //BbAvGreaterThan2_5 = historicalMatch.BbAvGreaterThan2_5,
                //BbMxLessThan2_5 = historicalMatch.BbMxLessThan2_5,
                //BbAvLessThan2_5 = historicalMatch.BbAvLessThan2_5,
                //BbAH = historicalMatch.BbAH,
                //BbAHh = historicalMatch.BbAHh,
                //BbMxAHH = historicalMatch.BbMxAHH,
                //BbAvAHH = historicalMatch.BbAvAHH,
                //BbMxAHA = historicalMatch.BbMxAHA,
                //BbAvAHA = historicalMatch.BbAvAHA,
                //PSCH = historicalMatch.PSCH,
                //PSCD = historicalMatch.PSCD,
                //PSCA = historicalMatch.PSCA,
                // Additional properties not present in HistoricalMatchFromCSVFileFourth
                MaxH = null,
                MaxD = null,
                MaxA = null,
                AvgH = null,
                AvgD = null,
                AvgA = null,
                B365Over25 = null,
                B365Under25 = null,
                POver25 = null,
                PUnder25 = null,
                MaxOver25 = null,
                MaxUnder25 = null,
                AvgOver25 = null,
                AvgUnder25 = null,
                AHh = null,
                B365AHH = null,
                B365AHA = null,
                PAHH = null,
                PAHA = null,
                MaxAHH = null,
                MaxAHA = null,
                AvgAHH = null,
                AvgAHA = null,
                B365CH = null,
                B365CD = null,
                B365CA = null,
                BWCH = null,
                BWCD = null,
                BWCA = null,
                IWCH = null,
                IWCD = null,
                IWCA = null,
                WHCH = null,
                WHCD = null,
                WHCA = null,
                VCCH = null,
                VCCD = null,
                VCCA = null,
                MaxCH = null,
                MaxCD = null,
                MaxCA = null,
                AvgCH = null,
                AvgCD = null,
                AvgCA = null,
                B365COver25 = null,
                B365CUnder25 = null,
                PCOver25 = null,
                PCUnder25 = null,
                MaxCOver25 = null,
                MaxCUnder25 = null,
                AvgCOver25 = null,
                AvgCUnder25 = null,
                AHCh = null,
                B365CAHH = null,
                B365CAHA = null,
                PCAHH = null,
                PCAHA = null,
                MaxCAHH = null,
                MaxCAHA = null,
                AvgCAHH = null,
                AvgCAHA = null,
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV4(List<HistoricalMatchFromCSVFileFourth> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV4).ToList();
        }
        public static HistoricalMatchFromCSVFile MapToHistoricalMatchFromCSVFileV3(HistoricalMatchFromCSVFileThird historicalMatch)
        {
            if (historicalMatch == null)
            {
                throw new ArgumentNullException(nameof(historicalMatch));
            }

            return new HistoricalMatchFromCSVFile
            {
                Div = historicalMatch.Div,
                Date = historicalMatch.Date ?? DateTime.MinValue, // Default value if Date is null
                Time = null, // Time is not present in the source
                HomeTeam = historicalMatch.HomeTeam,
                AwayTeam = historicalMatch.AwayTeam,
                FTHG = historicalMatch.FTHG,
                FTAG = historicalMatch.FTAG,
                FTR = historicalMatch.FTR,
                HTHG = historicalMatch.HTHG,
                HTAG = historicalMatch.HTAG,
                HTR = historicalMatch.HTR,
                Referee = historicalMatch.Referee,
                HS = historicalMatch.HS,
                AS = historicalMatch.AS,
                HST = historicalMatch.HST,
                AST = historicalMatch.AST,
                HF = historicalMatch.HF,
                AF = historicalMatch.AF,
                HC = historicalMatch.HC,
                AC = historicalMatch.AC,
                HY = historicalMatch.HY,
                AY = historicalMatch.AY,
                HR = historicalMatch.HR,
                AR = historicalMatch.AR,
                B365H = historicalMatch.B365H,
                B365D = historicalMatch.B365D,
                B365A = historicalMatch.B365A,
                BWH = historicalMatch.BWH,
                BWD = historicalMatch.BWD,
                BWA = historicalMatch.BWA,
                IWH = historicalMatch.IWH,
                IWD = historicalMatch.IWD,
                IWA = historicalMatch.IWA,
                PSH = historicalMatch.PSH,
                PSD = historicalMatch.PSD,
                PSA = historicalMatch.PSA,
                WHH = historicalMatch.WHH,
                WHD = historicalMatch.WHD,
                WHA = historicalMatch.WHA,
                VCH = historicalMatch.VCH,
                VCD = historicalMatch.VCD,
                VCA = historicalMatch.VCA,
                // Additional properties not present in HistoricalMatchFromCSVFileThird
                MaxH = null,
                MaxD = null,
                MaxA = null,
                AvgH = null,
                AvgD = null,
                AvgA = null,
                B365Over25 = null,
                B365Under25 = null,
                POver25 = null,
                PUnder25 = null,
                MaxOver25 = null,
                MaxUnder25 = null,
                AvgOver25 = null,
                AvgUnder25 = null,
                AHh = null,
                B365AHH = null,
                B365AHA = null,
                PAHH = null,
                PAHA = null,
                MaxAHH = null,
                MaxAHA = null,
                AvgAHH = null,
                AvgAHA = null,
                B365CH = null,
                B365CD = null,
                B365CA = null,
                BWCH = null,
                BWCD = null,
                BWCA = null,
                IWCH = null,
                IWCD = null,
                IWCA = null,
                PSCH = null,
                PSCD = null,
                PSCA = null,
                WHCH = null,
                WHCD = null,
                WHCA = null,
                VCCH = null,
                VCCD = null,
                VCCA = null,
                MaxCH = null,
                MaxCD = null,
                MaxCA = null,
                AvgCH = null,
                AvgCD = null,
                AvgCA = null,
                B365COver25 = null,
                B365CUnder25 = null,
                PCOver25 = null,
                PCUnder25 = null,
                MaxCOver25 = null,
                MaxCUnder25 = null,
                AvgCOver25 = null,
                AvgCUnder25 = null,
                AHCh = null,
                B365CAHH = null,
                B365CAHA = null,
                PCAHH = null,
                PCAHA = null,
                MaxCAHH = null,
                MaxCAHA = null,
                AvgCAHH = null,
                AvgCAHA = null,
            };
        }

        public static List<HistoricalMatchFromCSVFile> MapToHistoricalMatchFromCSVFileV3(List<HistoricalMatchFromCSVFileThird> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToHistoricalMatchFromCSVFileV3).ToList();
        }
        public static SeasonMatchRecord MapToSeasonMatchRecord(HistoricalMatchFromCSVFile historicalMatch)
        {
            if (historicalMatch == null)
            {
                throw new ArgumentNullException(nameof(historicalMatch));
            }
            
            return new SeasonMatchRecord
            {
                LeagueIdentifier = historicalMatch.Div ?? "", //historicalMatch.Div != null ? int.Parse(historicalMatch.Div) : 0, // Assuming Div represents the season year
                Date = historicalMatch.Date,
                HomeTeam = historicalMatch.HomeTeam ?? string.Empty,
                AwayTeam = historicalMatch.AwayTeam ?? string.Empty,
                HomeGoals = historicalMatch.FTHG ?? 0,
                AwayGoals = historicalMatch.FTAG ?? 0,
                HomeHalfTimeGoals = historicalMatch.HTHG ?? 0,
                AwayHalfTimeGoals = historicalMatch.HTAG ?? 0,
                HomeCorners = historicalMatch.HC ?? 0,
                AwayCorners = historicalMatch.AC ?? 0,
                HomeHalfTimeCorners = historicalMatch.HC ?? 0, // Assuming same as HomeCorners for half-time
                AwayHalfTimeCorners = historicalMatch.AC ?? 0, // Assuming same as AwayCorners for half-time
                //HomeBookings = historicalMatch.HY ?? 0,
                //AwayBookings = historicalMatch.AY ?? 0,
                HomeBookings = (historicalMatch.HY ?? 0) + (historicalMatch.HR ?? 0),
                AwayBookings = (historicalMatch.AY ?? 0) + (historicalMatch.AR ?? 0),
                HomeHalfTimeBookings = historicalMatch.HY ?? 0, // Assuming same as HomeBookings for half-time
                AwayHalfTimeBookings = historicalMatch.AY ?? 0,  // Assuming same as AwayBookings for half-time
                Referee = historicalMatch.Referee,
                AvgHomeOdds = historicalMatch.AvgH,
                AvgDrawOdds = historicalMatch.AvgD,
                AvgAwayOdds = historicalMatch.AvgA,
                AvgOver25Odds = historicalMatch.AvgOver25,
                AvgUnder25Odds = historicalMatch.AvgUnder25
            };
        }

        public static List<SeasonMatchRecord> MapToSeasonMatchRecords(List<HistoricalMatchFromCSVFile> historicalMatches)
        {
            if (historicalMatches == null)
            {
                throw new ArgumentNullException(nameof(historicalMatches));
            }

            return historicalMatches.Select(MapToSeasonMatchRecord).ToList();
        }

        public static Dictionary<string, string> TeamsNames()
        {
           return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Arsenal FC", "Arsenal" },
                { "Aston Villa FC", "Aston Villa" },
                { "Liverpool FC", "Liverpool" },
                { "Stoke City FC", "Stoke" },
                { "Norwich City FC", "Norwich" },
                { "Everton FC", "Everton" },
                { "Sunderland AFC", "Sunderland" },
                { "Fulham FC", "Fulham" },
                { "Swansea City AFC", "Swansea" },
                { "Manchester United", "Man United" },
                { "West Bromwich Albion FC", "West Brom" },
                { "Southampton FC", "Southampton" },
                { "West Ham United", "West Ham" },
                { "Cardiff City FC", "Cardiff" },
                { "Chelsea FC", "Chelsea" },
                { "Hull City AFC", "Hull" },
                { "Crystal Palace FC", "Crystal Palace" },
                { "Tottenham Hotspur", "Tottenham" },
                { "Manchester City", "Man City" },
                { "Newcastle United", "Newcastle" },
                { "Leicester City FC", "Leicester" },
                { "Queens Park Rangers FC", "QPR" },
                { "Burnley FC", "Burnley" },
                { "AFC Bournemouth", "Bournemouth" },
                { "Watford FC", "Watford" },
                { "Middlesbrough FC", "Middlesbrough" },
                { "Brighton & Hove Albion", "Brighton" },
                { "Huddersfield Town AFC", "Huddersfield" },
                { "Wolverhampton Wanderers FC", "Wolves" },
                { "Sheffield United FC", "Sheffield United" },
                { "Leeds United", "Leeds" },
                { "Brentford FC", "Brentford" },
                { "Nottingham Forest FC", "Nott'm Forest" },
                { "Luton Town FC", "Luton" }
            };
        }
        public static string? GetOfficialTeamName(string commonName)
        {
            var teamMapping = TeamsNames();
            return teamMapping.TryGetValue(commonName, out var officialName)
                ? officialName
                : null;
        }
    }

}
