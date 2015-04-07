using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anura
{
    public class Particle
    {
        public int ID;
        public float X, Y, XV = 0, YV = 0;

        public Particle(int id, float x, float y, float xv, float yv)
        {
            this.ID = id;

            this.X = x;
            this.Y = y;
            this.XV = xv;
            this.YV = yv;
        }

        public void update()
        {
            X += XV;
            Y += YV;

            if (XV < -0.5F || XV > 0.5F) XV *= 0.85F; else XV = 0.0F;
            if (YV < -0.5F || YV > 0.5F) YV *= 0.85F; else YV = 0.0F;
        }
    }
}
