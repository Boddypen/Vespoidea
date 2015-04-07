using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anura
{
    public class Projectile
    {
        public int ID;
        public float X, Y, XV, YV;

        private Random random;

        public Projectile(int id, float x, float y, float xv, float yv)
        {
            random = new Random();

            this.ID = id;

            this.X = x;
            this.Y = y;
            this.XV = xv;
            this.YV = yv;
        }

        public void update()
        {
            // Move the projectile
            X += XV;
            Y += YV;

            XV += (0.8F / random.Next(1, 10)) - (0.8F / random.Next(1, 10));
            YV += (0.8F / random.Next(1, 10)) - (0.8F / random.Next(1, 10));
            
            // Slow it down
            if (XV < -0.5F || XV > 0.5F) XV *= 0.995F; else XV = 0.0F;
            if (YV < -0.5F || YV > 0.5F) YV *= 0.995F; else YV = 0.0F;
        }
    }
}
