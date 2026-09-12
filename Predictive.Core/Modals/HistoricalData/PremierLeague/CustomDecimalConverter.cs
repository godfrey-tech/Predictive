using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class CustomDecimalConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            // Return null for empty strings or specific invalid values
            if (string.IsNullOrEmpty(text) || text.Trim() == "#REF!" || text.Trim() == "#N/A")
            {
                return null; // Return null for invalid numbers
            }

            // Try to parse the decimal with invariant culture
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result; // Successfully parsed
            }

            // If parsing fails, throw an exception
            throw new FormatException($"Unable to convert '{text}' to decimal.");
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value?.ToString() ?? string.Empty; // Convert null to empty string
        }
    }
}
