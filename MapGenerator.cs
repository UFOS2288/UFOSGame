using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFOSGame
{
    public class MapDecal : Object
    {
        //0 - 1
        public double chance = 0.2;
        public ObjectData decalData = new ObjectData()
        {
                name = "Rock",
                desc = "The rock.",
                color = ConsoleColor.DarkGray,
                components = new List<IComponent>() { },
                isInWorld = true,
                position = Vector.zero,
                sprite = 'c',
                orderInRender = -1,
        };

        public MapDecal(double chancee)
        {
            chance = chancee;
        }

        public override void OnInit()
        {
            data = decalData;
        }
    }

    public static class MapGenerator
    {
        public static void GenerateDecals(MapDecal[] decalPallette, int seed, int mapBorders)
        {
            Console.WriteLine("START!");
            Random random = new Random(seed);
            for (int x = -mapBorders; x < mapBorders + 1; x++)
            {
                for (int y = -mapBorders; y < mapBorders + 1; y++)
                {
                    //Object.SpawnObject(new MapDecal(1), new Vector(x,y));
                    MapDecal selectedDec = SelectDecal(random.NextDouble(), decalPallette);
                    if (selectedDec != null)
                    {
                        Object.SpawnObject(selectedDec, new Vector(x, y));
                    }
                }
            }
        }
        private static MapDecal SelectDecal(double random, MapDecal[] decals)
        {
            List<MapDecal> potentialDecols = new List<MapDecal>();
            foreach (MapDecal decal in decals)
            {
                if (decal.chance <= random)
                {
                    potentialDecols.Add(decal);
                }
            }
            MapDecal maxDecal = null;
            double maxDecalChance = 0;
            foreach (MapDecal decal in potentialDecols)
            {
                if (decal.chance > maxDecalChance)
                {
                    maxDecal = decal;
                    maxDecalChance = decal.chance;
                }
            }
            return maxDecal;
        }
    }
}
