using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class ApiResponse
    {
        public Filters Filters { get; set; }
        public ResultSet ResultSet { get; set; }
        public List<Match> Matches { get; set; }
    }
}
