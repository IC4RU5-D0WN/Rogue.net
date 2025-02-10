using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TurboMapReader;

namespace net_rogue
{
    internal class MapLoader
    {
        public Map ConvertTiledMapToMap(TiledMap turboMap)
        {
            Map rogueMap = new Map();

            int mapWidht = turboMap.width;
            int mapheight = turboMap.height;

            TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");
            TurboMapReader.MapLayer enemiesLayer = turboMap.GetLayerByName("enemies");
            TurboMapReader.MapLayer itemsLayer = turboMap.GetLayerByName("items");

            int howManyTiles = groundLayer.data.Length;
            int[] groundTiles = groundLayer.data;

            MapLayer myGroundLayer = new MapLayer(howManyTiles);
            myGroundLayer.name = "ground";
            myGroundLayer.data = groundTiles;
            rogueMap.mapWidth = groundLayer.width;
            rogueMap.mapHeight = groundLayer.height;
            // Tallenna taso kenttään
            rogueMap.layers[0] = myGroundLayer;

            //for(int i=0; i < myGroundLayer.data.Length; i++ )
            //{
            //    data[i];
            //}

            //foreach(int id in data)
            //{
            //    i
            //}
            // tason "enemies" tiedot...
            howManyTiles = enemiesLayer.data.Length;
            int[] enemiesTiles = enemiesLayer.data;

            MapLayer myenemiesLayer = new MapLayer(howManyTiles);
            myenemiesLayer.name = "enemies";

            rogueMap.layers[1] = myenemiesLayer;

            // tason "items" tiedot...
            howManyTiles = itemsLayer.data.Length;
            int[] itemsTiles = itemsLayer.data;

            MapLayer myitemsLayer = new MapLayer(howManyTiles);
            myitemsLayer.name = "items";

            rogueMap.layers[2] = myitemsLayer;

            // Lopulta palauta kenttä
            return rogueMap;
        }

        public Map? ReadTiledMapFromFile(string filename)
        {
            // Lataa tiedosto käyttäen TurboMapReaderia   
            TurboMapReader.TiledMap mapMadeInTiled = TurboMapReader.MapReader.LoadMapFromFile(filename);

            // Tarkista onnistuiko lataaminen
            if (mapMadeInTiled != null)
            {
                // Muuta Map olioksi ja palauta
                return ConvertTiledMapToMap(mapMadeInTiled);
            }
            else
            {
                // OH NO!
                return null;
            }
        }
        public Map LoadMapFromFile(string filename)
        {

            bool fileFound = File.Exists(filename);
            if (fileFound == false)
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents;

            using (StreamReader reader = File.OpenText(filename))
            {
     
                fileContents = File.ReadAllText(filename);
            }

            // HIGLY FUTILE: How to convert the file contents to a Map object?
            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents); ;
            
            return loadedMap;
        }
        public void TestFileReading(string filename)
        {
            // NOTE: This is just a test, it does not load a map from the file yet

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

        public Map LoadTestMap()
        {
            Map Test = new Map();
            Test.mapWidth = 8;
            Test.data = new int[] {
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 2, 2,
            2, 2, 2, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 1, 2,
            2, 2, 2, 2, 2, 2, 2, 2 };
            return Test;
        }
    }
}
