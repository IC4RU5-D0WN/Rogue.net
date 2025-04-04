using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;


// PELAAJAN HAHMON KOODI

namespace net_rogue
{
    internal class PlayerCharacter
    {
        public string Name;
        public Species species;
        public Role role;

        public char image;
        public ZeroElectric.Vinculum.Color Color;

        public Vector2 position;
        public Texture imageTexture;
        public Map currentMap;


        public void move(int moveX, int moveY, Map CurrentMap  )
        {
           
            position.X += moveX;
            position.Y += moveY;
            // Prevent  from going outside screen
            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > Console.WindowWidth - 1)
            {
                position.X = Console.WindowWidth - 1;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else if (position.Y > Console.WindowHeight - 1)
            {
                position.Y = Console.WindowHeight - 1;
            }

            bool enemycheck = currentMap.GetEnemyAt((int)position.X, (int)position.Y);
            bool itemcheck = currentMap.GetItemAt((int)position.X, (int)position.Y);

            if (CurrentMap.layers[0].data[(int)(position.X + (position.Y * CurrentMap.mapWidth))] == 2)  //stops player from moving into tiles with impassable id types
            {
                position.X -= moveX;
                position.Y -= moveY;
            }
            if (currentMap.GetLayer("ground").data[(int)(position.X+ (position.Y * 30))] != 49)
            {
                position.X -= moveX;
                position.Y -= moveY;
            }
           
            if (enemycheck & itemcheck)
            {
                position.X -= moveX;
                position.Y -= moveY;
            }
        }

        public void Draw()
        {
            // Draw the player
            Console.SetCursorPosition((int)position.X, (int)position.Y);
            int drawPixelX = (int)(position.X * Game.tileSize);
            int drawPixelY = (int)(position.Y * Game.tileSize);
            Raylib.DrawTexture(imageTexture, drawPixelX, drawPixelY, Raylib.WHITE);
            


        }
    }

    public enum Species
    {
        Duck,
        Mongoose,
        Elf
    }

    public enum Role
    {
        Cook,
        Smith,
        Rogue
    }
}
