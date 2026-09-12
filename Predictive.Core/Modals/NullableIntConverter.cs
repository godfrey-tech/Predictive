using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ConsoleApp1.Modals
{
    public class NullableIntConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return string.IsNullOrEmpty(text) ? (double?)null : double.Parse(text, CultureInfo.InvariantCulture);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}
