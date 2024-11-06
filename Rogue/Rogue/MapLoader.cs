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

        public Map LoadTestMap()
        {
            Map test = new Map();
            test.mapWidth = 8;
            test.layers[0].mapTiles = new int[] {
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 2, 2,
            2, 2, 2, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 1, 2,
            2, 2, 2, 2, 2, 2, 2, 2 };
            return test;

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
        public void TestFileReading(string filename)
        {
            using (StreamReader reader = File.OpenText(filename))
            {
                Console.WriteLine("File contents:");
                Console.WriteLine();

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
        }

        public Map LoadLayeredMap(string filename)
        {
            Map map = new Map();
            bool fileFound = File.Exists(filename);
            if (fileFound == false)
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }
            using (StreamReader reader = File.OpenText(filename))
            {
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Map>(fileContents);
            }
        }

        public Map ToMap(Map map, TiledMap Tmap)
        {
            map.layers[0].name = Tmap.GetLayerByName("Ground").name;
            map.layers[0].mapTiles = Tmap.GetLayerByName("Ground").data;

            map.layers[1].name = Tmap.GetLayerByName("Items").name;
            map.layers[1].mapTiles = Tmap.GetLayerByName("Items").data;

            map.layers[2].name = Tmap.GetLayerByName("Enemies").name;
            map.layers[2].mapTiles = Tmap.GetLayerByName("Enemies").data;

            map.mapWidth = Tmap.width;

            return map;

        }

    }
}

