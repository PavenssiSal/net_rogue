using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    public class MapLoader
    {
        public Map LoadTestMap()
        {
            Map test = new Map();
            test.mapWidth = 8;
            test.mapTiles = new int[] {
                2, 2, 2, 2, 2, 2, 2, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 2, 2,
                2, 2, 2, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 2, 2, 2, 2, 2, 2, 2 };

            return test;
        }
        public Map ReadMapFromFile(string filename)
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
            return LoadTestMap(); // Return the test map.
        }

        public Map LoadMapFromFile()
        {
            string mapfile = "Maps/mapfile.json";
            if (!File.Exists(mapfile))
            {
                Console.WriteLine($"File {mapfile} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

                string fileContents;
            using (StreamReader reader = File.OpenText(mapfile))
            {
                fileContents = reader.ReadToEnd(); // Read all lines into fileContents
            }

            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            return loadedMap;
        }
    }
}

