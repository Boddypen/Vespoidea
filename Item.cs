using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anura
{
    public class Item
    {
        public int
            ID,
            X,
            Y,
            value;

        public Item(int id, int x, int y, int value)
        {
            this.ID = id;
            
            this.X = x;
            this.Y = y;

            this.value = value;
        }
    }
}
