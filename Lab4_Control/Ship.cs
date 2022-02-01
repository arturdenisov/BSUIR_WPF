using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_Control
{
    class Ship : Unit
    {
        public Ship()
        {
            CoordinateX = 50;
            CoordinateY = 160;
        }

        public override void Move()
        {
            CoordinateX = CoordinateX + 1;
        }

    }
}
