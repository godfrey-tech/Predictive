using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictvie.Modals
{
    public class Team
    {
        public string TeamId  { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Coach { get; set; }
        public string Stadium { get; set; }
        public List<Player> Players { get; set; }

    }
}
