using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class ResultSet
    {
        public int Count { get; set; }
        public string Competitions { get; set; }
        public DateTime First { get; set; }
        public DateTime Last { get; set; }
        public int Played { get; set; }
    }
}
