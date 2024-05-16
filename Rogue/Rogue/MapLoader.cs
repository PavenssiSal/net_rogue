using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboMapReader;

namespace Rogue
{
    public class MapLoader
    {
        

        //public Map LoadTestMap()
        //{

        //    Map test = new Map();
        //    test.mapWidth = 8;
        //    test.mapTiles = new int[] {
        //        2, 2, 2, 2, 2, 2, 2, 2,
        //        2, 1, 1, 2, 1, 1, 1, 2,
        //        2, 1, 1, 2, 1, 1, 1, 2,
        //        2, 1, 1, 1, 1, 1, 2, 2,
        //        2, 2, 2, 2, 1, 1, 1, 2,
        //        2, 1, 1, 1, 1, 1, 1, 2,
        //        2, 2, 2, 2, 2, 2, 2, 2 };

        //    return test;
        //}
        Map ReadMapFromFile(string filename)
        {
            using (StreamReader reader = File.OpenText(filename))
            {

                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break; // End of file
                    }
                    Console.WriteLine(line);
                }

            }
            return null; // Return the test map.
        }
        
        public Map LoadMapFromFile()
        {

            string mapfile = "Maps/mapfile_layers.json";
            if (File.Exists(mapfile) == false)
            {
                Console.WriteLine($"File {mapfile} not found");
                 // Return the test map as fallback
            }
            if (File.Exists(mapfile) == true)
            {
                Console.WriteLine("");
            }

                string fileContents;
            using (StreamReader reader = File.OpenText(mapfile))
            {
                fileContents = reader.ReadToEnd(); // Read all lines into fileContents
            }

            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            return loadedMap;
        }

        public Map LoadTiledMapFromFile(string filename) 
        {
            TurboMapReader.TiledMap tiledMap = TurboMapReader.MapReader.LoadMapFromFile("Maps/Rogue_map.json");

            if (tiledMap == null)
            {
                //Error
                Console.WriteLine("You stoopid");
            }

            int mapWidth = tiledMap.width;
            int mapHeight = tiledMap.height;

            TurboMapReader.MapLayer groundLayer = tiledMap.GetLayerByName("ground");
            int howManyTiles = groundLayer.data.Length;
            int[] groundTiles = groundLayer.data;

            MapLayer myGroundLayer = new MapLayer();
            myGroundLayer.mapTiles = new int[howManyTiles];
            //Siirrä palat jollain tavalla...

        }
    }
}

