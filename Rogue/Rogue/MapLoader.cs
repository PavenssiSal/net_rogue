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
    }
}

