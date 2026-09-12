using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class CustomDoubleConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text) || text.Trim() == "#" || text.Trim() == "1xBet" || text.Trim() == "x") // Handle empty or invalid values
            {
                return null; // Return null for invalid numbers
            }

            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result; // Successfully parsed
            }

            throw new FormatException($"Unable to convert '{text}' to double.");
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value?.ToString() ?? string.Empty; // Convert null to empty string
        }
    }
}
