using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TurboMapReader;
using ZeroElectric.Vinculum;
using RayGuiCreator;
using static net_rogue.Game;

//PELIN KOODIA

namespace net_rogue
{
    internal class Game
    {
        Stack<GameState> stateStack = new Stack<GameState>();
        // List of possible difficulty choices. The indexing starts at 0
        MultipleChoiceEntry difficultyDropDown = new MultipleChoiceEntry(
            new string[] { "Easy", "Medium", "Hard" });

        // List of possible class choices.
        MultipleChoiceEntry classChoices = new MultipleChoiceEntry(
            new string[] { "Warrior", "Thief", "Magic User" });

        // This records whether a checkbox is active or not.
        // The value is changed by the MenuCreator
        bool checkBox = false;

        // Volume value is modified by the volume slider
        float volume = 1.0f;

        // Textbox data for player's name
        TextBoxEntry playerNameEntry = new TextBoxEntry(15);

        // Level number modified by the spinner
        int levelSpinner = 1;
        // Is the spinner active or not. This is changed by the MenuCreator
        bool spinnerEditActive = false;

        public bool valid;
        long number1;
        Map level;
        int CurrentMap;
        public static readonly int tileSize = 16;
        Color color;
       // GameState currentGameState;
        PlayerCharacter player;
        OptionsMenu myOptionsMenu;
        PauseMenu myPauseMenu;
       

        int menuStartX = 10;
        int menuStartY = 0;
        int rowHeight = Raylib.GetScreenHeight() / 20;
        int menuWidth = Raylib.GetScreenWidth() / 4;

        public enum GameState
        {
            MainMenu,
            GameLoop,
            CharacterCreation,
            PauseMenu,
            OptionsMenu
        }

        public void Print()
        {
            Console.WriteLine(" Menu values: ");
            Console.WriteLine($"Ranked:{checkBox}\n" +
            $"Volume: {volume}\n" +
            $"Player name: \"{playerNameEntry}\"\n" + // calls ToString() automatically
            $"Player class {classChoices.GetIndex()}: {classChoices.GetSelected()}\n" +
            $"Difficulty: {difficultyDropDown.GetIndex()}: {difficultyDropDown.GetSelected()}\n" +
            $"Starting level: {levelSpinner}");
        }

        public void ShowMenu()
        {
            Raylib.InitWindow(800, 600, "Character Creator");
            Raylib.SetTargetFPS(30);

            while (Raylib.WindowShouldClose() == false)
            {
                Raylib.BeginDrawing();

                // Clear to the background color that is defined in
                // the active GUI style.
                Raylib.ClearBackground(MenuCreator.GetBackgroundColor());

                DrawCharacterCreationMenu();

                Raylib.EndDrawing();
            }
        }
        void DrawCharacterCreationMenu()
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
            c.Label("Character Creator");

            c.Label("Player name");
            c.TextBox(playerNameEntry);

            if (c.Button("Honk!"))
            {
                Console.Write("Honk!");
            }

            c.Checkbox("Ranked match", ref checkBox);

            c.Label("Character class");
            c.DropDown(classChoices);

            c.Label("Volume");
            c.Slider("Quiet", "Max", ref volume, 0.0f, 1.0f);

            c.Spinner("Starting level", ref levelSpinner, 1, 12, ref spinnerEditActive);

            c.Label("Difficulty toggle");
            c.ToggleGroup(difficultyDropDown);

            if (c.LabelButton("START GAME"))
            {
                stateStack.Push(GameState.GameLoop);
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
        public void DrawMainMenu()
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
            MenuCreator creator = new MenuCreator(menuStartX, menuStartY, rowHeight, menuWidth);
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            creator.Label("Rogue");
            creator.Label("Arrow keys ");

            if (creator.Button("Start Game"))
            {
                stateStack.Push(GameState.CharacterCreation);
            }
            if (creator.Button("Nössö"))
            {
                Raylib.CloseWindow();
            }
            if (creator.Button("Options"))
            {
                stateStack.Push(GameState.OptionsMenu);
            }

            Raylib.EndDrawing();
        }
        public void DrawGameLoop() 
        {
            int moveX = 0;
            int moveY = 0;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                // Move player up
                player.move(0, -1, level);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                // Move player down
                player.move(0, 1, level);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                // Move player left
                player.move(-1, 0, level);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                // Move player right
                player.move(1, 0, level);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
            {
                stateStack.Push(GameState.PauseMenu);
            }


            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            level.Draw();
            player.Draw();
            Raylib.EndDrawing();


        }

        void OnOptionsBackButtonPressed(object sender, EventArgs args)
        {
            stateStack.Pop();
        }

        void OnPauseOptionsButtonPressed(object sender, EventArgs args)
        {
            stateStack.Push(GameState.OptionsMenu);
        }

        void OnPauseBackButtonPressed(object sender, EventArgs args)
        {
            stateStack.Pop();
        }
        public void Run()
        {
            Console.WindowWidth = 50;
            Console.WindowHeight = 20;
            // INIT MUISTA TÄÄ
            player = new PlayerCharacter();

            stateStack.Push(GameState.MainMenu);

            // Set player starting position
            player.position = new Vector2(2, 2);

            Raylib.InitWindow(1280, 720, "W1");
            Raylib.SetTargetFPS(30);
            Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);
            myOptionsMenu = new OptionsMenu();
            // Kytke asetusvalikon tapahtumaan funktio
            myOptionsMenu.BackButtonPressedEvent += this.OnOptionsBackButtonPressed;
            myPauseMenu = new PauseMenu();
            myPauseMenu.BackButtonPressedEvent += this.OnPauseBackButtonPressed;
            myPauseMenu.OptionsPressedEvent += this.OnPauseOptionsButtonPressed;

            MapLoader loader = new MapLoader();

            loader.TestFileReading("Maps/tiledmap.tmj");

            level = loader.ReadTiledMapFromFile("Maps/tiledmap.tmj");
            level.LoadEnemies();
            level.LoadItems();
            CurrentMap = 1;
            player.currentMap = level;
            Console.ForegroundColor = ConsoleColor.White;
            level.Draw();
            player.imageTexture = Raylib.LoadTexture("Textures/MINISHREK.png");
            level.imageTexture = Raylib.LoadTexture("Textures\\tilemap_packed.png");
            player.Color = Raylib.GREEN;

            //Console.Write("@");

            // Start the game loop:
            while (Raylib.WindowShouldClose() == false)
            {
                switch (stateStack.Peek())
                {
                    case GameState.CharacterCreation:
                        DrawCharacterCreationMenu();
                        break;

                    case GameState.MainMenu:
                        // Tämä koodi on uutta
                        
                        DrawMainMenu();
                        break;

                    case GameState.GameLoop:
                        // Tämä koodi on se mitä GameLoop() funktiossa oli ennen muutoksia
                        DrawGameLoop();
                        break;

                    case GameState.OptionsMenu:

                        myOptionsMenu.DrawMenu();

                        break;

                    case GameState.PauseMenu:

                        myPauseMenu.DrawMenu();

                        break;
                }
            }

            Raylib.UnloadTexture(player.imageTexture);
            Raylib.CloseWindow();

        }

    }
}
