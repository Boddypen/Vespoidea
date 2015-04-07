using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anura
{
    public class Dungeon
    {
        private Random random;

        public static int
            DIRT = 0,
            STONE = 1;

        public int levelAmount = 3;

        private Level[] levels;

        public Dungeon()
        {
            random = new Random();

            levels = new Level[levelAmount];
            for (int i = 0; i < levels.Length; i++)
                levels[i] = new Level(8, 8);

            for (int x = 0; x < levels[0].width; x++)
                for (int y = 0; y < levels[0].height; y++)
                    if (random.Next(3) == 0)
                        levels[0].tiles[x, y] = STONE;
                    else
                        levels[0].tiles[x, y] = DIRT;
        }

        public Level getLevel(int index)
        {
            return levels[index];
        }
    }
}
