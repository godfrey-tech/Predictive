using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using System.Globalization;

namespace ConsoleApp1.Modals
{
    public class CustomDateConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return DateTime.MinValue; // or return DateTime.MinValue if you want to default to a specific date
            }

            // Try to parse the date in different formats
            if (DateTime.TryParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date; // Full date format
            }
            else if (DateTime.TryParseExact(text, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date; // Two-digit year format
            }
            else if (DateTime.TryParseExact(text, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date; // Year only format
            }
            else if (DateTime.TryParseExact(text, "yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date; // Two-digit year only format
            }

            throw new FormatException($"Unable to convert '{text}' to DateTime.");
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value != null ? ((DateTime)value).ToString("dd/MM/yyyy") : string.Empty; // Format the output as needed
        }
    }
}
