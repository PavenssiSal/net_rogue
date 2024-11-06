using Rogue.Images;
using System;
using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using ZeroElectric.Vinculum;

using TurboMapReader;
using RayGuiCreator;
using System.Globalization;
using System.Security.Claims;
using System.Reflection.PortableExecutable;

namespace Rogue
{
    public class Game
    {
        string Rotu;
        string Luokka;
        string PlayerName;
        public PlayerCharacter player = new PlayerCharacter();
        Map level01;
        TiledMap level;

        public static readonly int tileSize = 16;

        // Pelin koko ja renderöintitextuuri
        int game_width;
        int game_height;
        RenderTexture game_screen;

        SettingsMenu settingsMenu;
        PauseMenu pauseMenu;
        
        enum GameState
        {
            MainMenu,
            CharacterCreator,
            Settings,
            PauseMenu,
            GameLoop
        }

        GameState currentGameState;
        TextBoxEntry playerNameEntry;
        public MultipleChoiceEntry classChoices = new MultipleChoiceEntry(new string[] { Role.Wizard.ToString(), Role.Swordfighter.ToString(), Role.Archer.ToString(), Role.Homeless.ToString(), Role.ManWithBigWoodenStick.ToString() });
        public MultipleChoiceEntry raceChoices = new MultipleChoiceEntry(new string[] { Race.Human.ToString(), Race.Elf.ToString(), Race.Rat.ToString(), Race.Jesus.ToString() });


        void DrawMainMenu()
        {
            int menuWidth = Raylib.GetScreenWidth() / 4;
            int menuX = Raylib.GetScreenWidth() / 2 - menuWidth / 2;
            int menuY = 10;
            int rowHeight = Raylib.GetScreenHeight() / 10;
            RayGuiCreator.MenuCreator creator = new RayGuiCreator.MenuCreator(menuX, menuY, rowHeight, menuWidth);

            //Valikko alkaa tästä
            creator.Label("Main menu");

            if (creator.Button("Start Game"))
            {
                currentGameState = GameState.CharacterCreator;
            }
            creator.Label("");
            if (creator.Button("Settings"))
            {
                currentGameState = GameState.Settings;
            }
        }
        void DrawCharacterCreatorMenu()
        {
            int menuWidth = Raylib.GetScreenWidth() / 4;
            int menuX = Raylib.GetScreenWidth() / 2 - menuWidth / 2;
            int menuY = 10;
            int rowHeight = Raylib.GetScreenHeight() / 25;
            RayGuiCreator.MenuCreator creator = new RayGuiCreator.MenuCreator(menuX, menuY, rowHeight, menuWidth);

            creator.Label("Character name");
            creator.TextBox(playerNameEntry); //Modified player's name
            creator.Label("");
            creator.Label("Character Race:");
            creator.ToggleGroup(raceChoices);
            creator.Label("");
            creator.Label("Character Class");
            creator.ToggleGroup(classChoices);
            creator.Label("");
            if (creator.Button("Start Game"))
            {
                if (TestName( playerNameEntry.ToString() ))
                {
                    player.PlayerName = playerNameEntry.ToString();

                    switch (raceChoices.GetSelected())
                    {
                        case "Human":
                            player.rotu = Race.Human;
                            break;
                        case "Elf":
                            player.rotu = Race.Elf;
                            break;
                        case "Rat":
                            player.rotu = Race.Rat;
                            break;
                        case "Jesus":
                            player.rotu = Race.Jesus;
                            break;

                    }
                    switch (classChoices.GetSelected())
                    {
                        case "Wizard":
                            player.luokka = Role.Wizard;
                            break;
                        case "Warrior":
                            player.luokka = Role.Swordfighter;
                            break;
                        case "Archer":
                            player.luokka = Role.Archer;
                            break;
                        case "Homeless":
                            player.luokka = Role.Homeless;
                            break;
                        case "ManWithBigWoodenStick":
                            player.luokka = Role.ManWithBigWoodenStick;
                            break;

                    }
                    currentGameState = GameState.GameLoop;
                }
            }
            creator.EndMenu();
        }
        public bool TestName(string nimi)
        {
            if (string.IsNullOrEmpty(nimi))
            {
                return false;
            }
            bool nameOk = true;
            for (int i = 0; i < nimi.Length; i++)
            {
                char kirjain = nimi[i];
                if (char.IsLetter(kirjain) == false)
                {
                    nameOk = false;
                    Console.WriteLine("Name cannot have numbers");
                    break;
                }
            }
            return nameOk;
        }
        
        private PlayerCharacter CreateCharacter()
        {
            PlayerCharacter player = new PlayerCharacter();
            return player;
        }

        public void Run()
        {
            InIt();
            GameLoop();
        }
        void OnSettingsBackButtonPressed(object sender, EventArgs args)
        {
            currentGameState = GameState.MainMenu;
        }
        private void InIt()
        {
            currentGameState = GameState.MainMenu;

            settingsMenu = new SettingsMenu();
            // Kytke asetusvalikon tapahtumaan funktio
            settingsMenu.BackButtonPressedEvent += this.OnSettingsBackButtonPressed;
            pauseMenu = new PauseMenu();
            // Kytke asetusvalikon tapahtumaan funktio
            pauseMenu.BackButtonPressedEvent += this.OnSettingsBackButtonPressed;
            // Tätä funktiota kutsutaan kun asetusvalikon Back nappia on painettu

            TurboMapReader.TiledMap tileMap = TurboMapReader.MapReader.LoadMapFromFile("Maps/Rogue_map_simple.json");
            playerNameEntry = new TextBoxEntry(14);
            player = CreateCharacter();
            //MapLoader loader = new MapLoader();
            //level01 = loader.LoadMapFromFile();
            MapLoader Reader = new MapLoader();
            level01 = Reader.LoadLayeredMap("Maps/Rogue_map_simple.json");
            level = MapReader.LoadMapFromFile("Maps/Rogue_map_simple.json");
            Console.WriteLine(level);
            Reader.ToMap(level01, level);

            // Set the window size
            game_width = 480;
            game_height = 360;
            Raylib.InitWindow(game_width * 2, game_height * 2, "Rogue Game");

            // Load the sprite atlas image
            Texture Character = Raylib.LoadTexture("Images/tilemap_packed.png");

            Texture Wall = Raylib.LoadTexture("Images/tilemap_packed.png");
            Texture Floor = Raylib.LoadTexture("Images/tilemap_packed.png");
            Texture Items = Raylib.LoadTexture("Images/tilemap_packed.png");

            Texture spriteAtlas = Raylib.LoadTexture("Images/tilemap_packed.png");


            player.SetImageAndIndex(Character, 1, 0);
            level01.SetImageAndIndex(Wall, 1, 0);
            level01.SetImageAndIndex(Floor, 1, 0);
            level01.SetImageAndIndex(Items, 1, 0);
            level01.LoadEnemiesAndItems(spriteAtlas);

            // Create render texture and set filtering
            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);

            Raylib.SetTargetFPS(30);
            //Console.Clear();
        }

        public void MainMenu()
        {
            // Tyhjennä ruutu ja aloita piirtäminen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            // Laske ylimmän napin paikka ruudulla.
            int button_width = 200;
            int button_height = 40;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height / 2;

            // Piirrä pelin nimi nappien yläpuolelle
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Rogue");

            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Start Game") == 1)
            {
                // Start the game
                Console.WriteLine("Fuck you");
                
                currentGameState = GameState.CharacterCreator;
            }

            // Piirrä seuraava nappula edellisen alapuolelle
            button_y += button_height * 2;

            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Options") == 1)
            {
                // Go to options somehow
            }

            button_y += button_height * 2;

            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Quit") == 1)
            {
                // Quit the game
                Environment.Exit(0);
            }
            Raylib.EndDrawing();
        }

        private void DrawGame()
        {
            // Piirrä peli renderöintitextuuriin
            Raylib.BeginTextureMode(game_screen);
            Raylib.ClearBackground(Raylib.BLANK);
            level01.Draw();
            player.Draw();

            Raylib.EndTextureMode();

            Raylib.BeginDrawing();
            // Piirrä peli skaalattuna ruudulle
            DrawGameToTexture();
            //DrawMainMenu();
            Raylib.EndDrawing();
        }
        public void DrawGameToTexture() 
        {
            //DrawGame();
            DrawGameScaled();
        }
        private void DrawGameScaled()
        {
            // Piirrä peli skaalattuna ruudulle
            Raylib.ClearBackground(Raylib.BLANK);

            // Lasketaan skaala
            int draw_width = Raylib.GetScreenWidth();
            int draw_height = Raylib.GetScreenHeight();
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);

            // Lasketaan piirrettävän alueen sijainti ja koko
            Rectangle source = new Rectangle(0, 0, game_width, -game_height);
            Rectangle destination = new Rectangle((draw_width - game_width * scale) * 0.5f,
                                                   (draw_height - game_height * scale) * 0.5f,
                                                   game_width * scale,
                                                   game_height * scale);

            // Piirrä renderöintitextuuri skaalattuna ruudulle
            Raylib.DrawTexturePro(game_screen.texture, source, destination, Vector2.Zero, 0f, Raylib.WHITE);
           // Raylib.EndDrawing();
        }
        private void UpdateGame()
        {
            // Set player starting position
            player.position = new Vector2(5, 5);
            while (true)
            {
                int moveX = 0;
                int moveY = 0;

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP) || Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                {
                    moveY = -1;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) || Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    moveY = 1;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT) || Raylib.IsKeyPressed(KeyboardKey.KEY_A))
                {
                    moveX = -1;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_D))
                {
                    moveX = 1;
                }
                //Check collisions with walls
                int newX = (int)player.position.X + moveX;
                int newY = (int)player.position.Y + moveY;
                int index = newX + newY * level01.mapWidth;

                //Most definitely not the intended way, but it works
                MapLayer layer = level01.GetLayer("Ground");
                MapLayer Itemlayer = level01.GetLayer("Items");

                if (layer.mapTiles[index] != 50)
                {
                    // The new position is not a floor tile (not walkable), so do not move the player
                    moveX = 0; moveY = 0;
                }

                // Tarkista, onko uudessa ruudussa vihollinen
                Enemy enemy = level01.GetEnemyAt(newX, newY);
                if (enemy != null)
                {
                    Console.WriteLine($"You hit an enemy: {enemy.name}");
                }
                bool lukko = true;
                // Tarkista, onko uudessa ruudussa esine
                Items item = level01.GetItemAt(newX, newY);
                if (item != null)
                {
                    while (lukko = true)
                    {
                        Console.WriteLine($"You find an item: {item.name}");
                        break;
                    }
                }

                //Liikutetaan pelaajaa
                player.Move(moveX, moveY);

                //Estetään pelaaja menemästä kartan ulkopuolelle
                if (player.position.X < 0)
                {
                    player.position.X = 0;
                }
                else if (player.position.X > Console.WindowWidth - 1)
                {
                    player.position.X = Console.WindowWidth - 1;
                }
                if (player.position.Y < 0)
                {
                    player.position.Y = 0;
                }
                else if (player.position.Y > Console.WindowHeight - 1)
                {
                    player.position.Y = Console.WindowHeight - 1;
                }
                DrawGame();
            }
        }
        private void GameLoop()
        {
            while (Raylib.WindowShouldClose() == false)
            {
                switch (currentGameState)
                {
                    case GameState.MainMenu:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        // Tämä koodi on uutta
                        DrawMainMenu();
                        //MainMenu();
                        Raylib.EndDrawing();
                        break;
                    case GameState.CharacterCreator:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.DARKGRAY);
                        DrawCharacterCreatorMenu();
                        Raylib.EndDrawing();
                        break;
                    case GameState.GameLoop:
                        //Testing stuff to see if loads and player info work properly
                        Console.WriteLine("Fuck me bruh");
                        Console.WriteLine($"Player info: Name: {player.PlayerName}, Class: {player.luokka}, Race: {player.rotu}");
                        // Tämä koodi on se mitä GameLoop() funktiossa oli ennen muutoksia
                        UpdateGame();
                        DrawGameToTexture();
                        break;
                    case GameState.Settings:
                        settingsMenu.DrawMenu();
                        break;
                }
            } // while(true) ends
        } // GameLoop ends
    }
}
