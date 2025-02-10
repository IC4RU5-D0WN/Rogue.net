using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
using RayGuiCreator;

namespace net_rogue
{
    internal class OptionsMenu
    {
        public event EventHandler BackButtonPressedEvent;

        public void DrawMenu()
        {
            int width = Raylib.GetScreenWidth() / 2;
            // Fit 22 rows on the screen
            int rows = 22;
            int rowHeight = Raylib.GetScreenHeight() / rows;
            // Center the menu horizontally
            int x = (Raylib.GetScreenWidth() / 2) - (width / 2);
            // Center the menu vertically
            int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;
            // 3 pixels between rows, text 3 pixels smaller than row height
            MenuCreator c = new MenuCreator(x, y, rowHeight, width, 3, -3);
            Raylib.ClearBackground(Raylib.BLACK);
            Raylib.BeginDrawing();
            c.Label("Options");

            if (c.Button("Honk!"))
            {
                Console.Write("Honk!");
            }


            if (c.LabelButton("BACK"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }


            // Draws open dropdowns over other menu items
            int menuHeight = c.EndMenu();

            // Draws a rectangle around the menu
            int padding = 2;
            Raylib.DrawRectangleLines(
                x - padding,
                y - padding,
                width + padding * 2,
                menuHeight + padding * 2,
                MenuCreator.GetLineColor());
            Raylib.EndDrawing();
        }
    }
}
