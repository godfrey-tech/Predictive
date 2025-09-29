using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictvie.Modals
{
    public class Event
    {
        public EventType Type { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public string PlayerName { get; set; }
    }
}
