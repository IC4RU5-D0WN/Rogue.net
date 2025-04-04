using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

//MAPPIIN LIITTYVÄT ASIAT KUTEN KOKO JA TEXTUURIT YMS

namespace net_rogue
{
    internal class Map
    {
        public Texture imageTexture;
        public List<Item> items;
        public List<Enemy> enemies;
        public MapLayer[] layers;
        public int mapWidth;
        public int mapHeight;
        public int[] data;

        const int imagesPerRow = 12;
        
        Color bg_color = Raylib.YELLOW;

        public enum MapTile : int
        {
            Floor = 48,
            Wall = 40
        }

        public Map()
        {
            mapWidth = 1;
            mapHeight = 1;
            layers = new MapLayer[3];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new MapLayer(mapWidth * mapHeight);
            }
            enemies = new List<Enemy>() { };
            items = new List<Item>() { };
        }

        public MapLayer GetLayer(string layerName)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
            return null; // Wanted layer was not found!
        }

        public bool GetEnemyAt(int x, int y)
        {
            bool check = false;
            foreach(Enemy enemy in enemies)
            {
                if (enemy.position.X == x && enemy.position.Y == y) 
                {
                    check = true; break; 
                }
            }
            return check;
        }
        public bool GetItemAt(int x, int y)
        {
            bool check = false;
            foreach (Item item in items)
            {
                if (item.position.X == x && item.position.Y == y)
                {
                    check = true; break;
                }
            }
            return check;
        }
        public void LoadItems()
        {
            // Hae viholliset sisältävä taso kentästä
            MapLayer ItemLayer = GetLayer("items");
            int[] ItemTiles = ItemLayer.data;
            int layerHeight = ItemTiles.Length / mapWidth;

            // Käy taso läpi ja luo viholliset
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;

                    int tileId = ItemTiles[index];

                    if (tileId == 0)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        continue;
                    }
                    else
                    {
                        // Tässä kohdassa kenttää on jokin vihollinen

                        // Tässä pitää vähentää 1,
                        // koska Tiled editori tallentaa
                        // palojen numerot alkaen 1:sestä.
                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetItemName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        items.Add(new Item(name, position, spriteId));
                    }
                }
            }
        }
        public void LoadEnemies()
        {
            // Hae viholliset sisältävä taso kentästä
            MapLayer enemyLayer = GetLayer("enemies");
            int[] enemyTiles = enemyLayer.data;
            int layerHeight = enemyTiles.Length / mapWidth;

            // Käy taso läpi ja luo viholliset
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;

                    int tileId = enemyTiles[index];

                    if (tileId == 0)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        continue;
                    }
                    else
                    {
                        // Tässä kohdassa kenttää on jokin vihollinen

                        // Tässä pitää vähentää 1,
                        // koska Tiled editori tallentaa
                        // palojen numerot alkaen 1:sestä.
                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetEnemyName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        enemies.Add(new Enemy(name, position, spriteId));
                    }
                }
            }
        }
        public string GetEnemyName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Ghost"; break;
                case 109: return "Cyclops"; break;
                default: return "Unknown"; break;
            }
        }
        public string GetItemName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Juttu"; break;
                case 109: return "Tikku"; break;
                default: return "Unknown"; break;
            }
        }

        public void Draw()
        {
            MapLayer groundLayer = GetLayer("ground");
            int[] data = groundLayer.data;
            int layerHeight = data.Length / mapWidth;

            for (int y = 0; y < layerHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {
                    int drawPixelX = (int)(x * Game.tileSize);
                    int drawPixelY = (int)(y * Game.tileSize);
                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileId = data[index]; // Read the tile value at index
                    int imageX = tileId % imagesPerRow;
                    int imageY = (int)(tileId / imagesPerRow);
                    int imagePixelX = (imageX-1) * Game.tileSize;
                    int imagePixelY = (imageY) * Game.tileSize;
                    if (tileId == 12 || tileId == 24 || tileId == 36 || tileId == 48 || tileId == 60)
                    {
                        imagePixelY = (imageY-1) * Game.tileSize;
                    }

                    Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);


                    Vector2 pixelPosition = new Vector2(drawPixelX, drawPixelY);

                    Raylib.DrawTextureRec(imageTexture, imageRect, pixelPosition, Raylib.WHITE);

                    //// Draw the tile graphics
                    ////Console.SetCursorPosition(x, y);
                    //switch (tileId)
                    //{
                    //    case 1:
                    //        //Console.Write("."); // Floor
                    //        Raylib.DrawRectangle(drawPixelX, drawPixelY, Game.tileSize, Game.tileSize, Raylib.BLACK);
                    //        Raylib.DrawText(" ", drawPixelX, drawPixelY, Game.tileSize, Raylib.WHITE);
                    //        break;
                    //    case 2:
                    //        //Console.Write("#"); // Wall
                    //        Raylib.DrawRectangle(drawPixelX, drawPixelY, Game.tileSize, Game.tileSize, Raylib.GRAY);
                    //        Raylib.DrawText("#", drawPixelX, drawPixelY, Game.tileSize, Raylib.DARKGRAY);
                    //        break;
                    //    default:
                    //        //Console.Write(" ");
                    //        Raylib.DrawRectangle(drawPixelX, drawPixelY, Game.tileSize, Game.tileSize, Raylib.BLACK);
                    //        Raylib.DrawText(" ", drawPixelX, drawPixelY, Game.tileSize, Raylib.WHITE);
                    //        break;
                    //}
                }
            }
            foreach (Item Toutput in items)
            {
                Raylib.DrawRectangle(Convert.ToInt32(Toutput.position.X * Game.tileSize), Convert.ToInt32(Toutput.position.Y * Game.tileSize), Game.tileSize, Game.tileSize, Raylib.YELLOW);
            }
            foreach (Enemy Toutput in enemies)
            {
                Raylib.DrawRectangle(Convert.ToInt32(Toutput.position.X * Game.tileSize), Convert.ToInt32(Toutput.position.Y * Game.tileSize), Game.tileSize, Game.tileSize, Raylib.RED);
            }
            
        }

        public void DrawRaylib()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(bg_color);

            Raylib.EndDrawing();
        }
    }
}
