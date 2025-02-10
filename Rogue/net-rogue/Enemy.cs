using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace net_rogue
{
    internal class Enemy
    {
        public string name;       
        public Vector2 position;  
        public int spriteIndex;  
        public Enemy(string name, Vector2 position, int spriteIndex)
        {

            this.name = name;

        }
    }
}
