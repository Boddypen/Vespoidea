using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anura
{
    public class Object
    {
        public int ID, width, height;
        public float X, Y, XV = 0, YV = 0;

        public Object(int id, float x, float y, int width, int height, float xv, float yv)
        {
            this.ID = id;

            this.X = x;
            this.Y = y;
            this.width = width;
            this.height = height;
            this.XV = xv;
            this.YV = yv;
        }

        public void update()
        {
            X += XV;
            Y += YV;

            if (XV < -0.5F || XV > 0.5F) XV *= 0.7F; else XV = 0.0F;
            if (YV < -0.5F || YV > 0.5F) YV *= 0.7F; else YV = 0.0F;
        }
    }
}
