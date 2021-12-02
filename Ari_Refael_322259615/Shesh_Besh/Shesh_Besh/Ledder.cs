using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shesh_Besh
{
    class Ledder : Tile
    {
        public int To;

        public Ledder(int NT, int Line, int To) : base(NT, Line)
        {
            this.To = To;
        }
    }
}
