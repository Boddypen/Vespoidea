using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anura
{
    public class Level
    {
        private Random random;

        private List<Item> items;
        private List<Projectile> projectiles;
        private List<Object> objects;
        private List<Particle> particles;

        public int[,] tiles;
        public int
            initialPlayerX,
            initialPlayerY,
            width,
            height;

        public Level(int width, int height)
        {
            random = new Random();

            this.width = width;
            this.height = height;

            items = new List<Item>();
            projectiles = new List<Projectile>();
            particles = new List<Particle>();

            tiles = new int[width, height];
        }

        public List<Item> getItems()
        {
            return items;
        }

        public List<Projectile> getProjectiles()
        {
            return projectiles;
        }

        public List<Object> getObjects()
        {
            return objects;
        }

        public List<Particle> getParticles()
        {
            return particles;
        }

        public Boolean collides(int x, int y)
        {
            switch (tiles[x, y])
            {
                case 0: return false; // Dirt
                case 1: return true; // Stone
                default: return false;
            }
        }
    }
}
