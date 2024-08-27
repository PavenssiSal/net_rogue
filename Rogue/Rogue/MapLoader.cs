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
        public Map? ReadMapFromFile()
        {
            // Lataa tiedosto käyttäen TurboMapReaderia   
            TurboMapReader.TiledMap turboMap = MapReader.LoadMapFromFile("Maps/Rogue_map.json");

            // Tarkista onnistuiko lataaminen
            if (turboMap != null)
            {
                // Muuta Map olioksi ja palauta
                Console.WriteLine("Succesess");
                return ConvertTiledMapToMap(turboMap);
                
            }
            else
            {
                // OH NO!
                Console.WriteLine("Úff");
                return null;
            }
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

        public Map ConvertTiledMapToMap(TiledMap turbomap)
        {
            // Luo tyhjä kenttä
            Map roguemap = new Map();
            // Varaa tilaa kolmelle tasolle
            roguemap.layers = new MapLayer[3];

            // Muunna tason "ground" tiedot
            TurboMapReader.MapLayer groundLayer = turbomap.GetLayerByName("ground");

            // TODO: Lue kentän leveys. Kaikilla TurboMapReader.MapLayer olioilla on sama leveys

            // Kuinka monta kenttäpalaa tässä tasossa on?
            int howManyTiles = groundLayer.data.Length;
            // Taulukko jossa palat ovat
            int[] groundTiles = groundLayer.data;

            // Luo uusi taso tietojen perusteella
            MapLayer myGroundLayer = new MapLayer();
            myGroundLayer.name = "ground";
            myGroundLayer.mapTiles = new int[howManyTiles];


            // TODO: lue tason palat



            // Tallenna taso kenttään
            roguemap.layers[0] = myGroundLayer;

            // TODO: Muunna tason "enemies" tiedot...
            // TODO: Muunna tason "items" tiedot...

            // Lopulta palauta kenttä
            return roguemap;
        }

    }
}

