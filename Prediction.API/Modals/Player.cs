using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictvie.Modals
{
    public class Player
    {
        public string PlayerId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Number { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
    }
}
