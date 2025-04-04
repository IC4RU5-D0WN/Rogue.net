using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// MAPIN KOKO

namespace net_rogue
{
    internal class MapLayer
    {
        public string name;
        public int[] data;
        public MapLayer(int mapSize)
        {
            name = "";
            data = new int[mapSize];
        }
    }
}
