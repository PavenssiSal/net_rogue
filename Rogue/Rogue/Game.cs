using Rogue.Images;
using System;
using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using ZeroElectric.Vinculum;


using TurboMapReader;

namespace Rogue
{
    public class Game
    {
        string Rotu;
        string Luokka;
        string PlayerName;
        public PlayerCharacter player = new PlayerCharacter();
        Map level01;

        public static readonly int tileSize = 16;

        // Pelin koko ja renderöintitextuuri
        int game_width;
        int game_height;
        RenderTexture game_screen;

        enum GameState
        {
            MainMenu,
            GameLoop
        }

        GameState currentGameState;

        private string AskName()
        {
            // Nimi valinta (ei hyväksy tyhjää eikä numeroita)
            while (true)
            {
                Console.WriteLine("What is your name?");
                string nimi = Console.ReadLine();
                PlayerName = nimi;

                if (string.IsNullOrEmpty(nimi))
                {
                    Console.WriteLine("Name cannot be blank");
                    continue;
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
                if (nameOk == true)
                {
                    break;
                }
            }
            return PlayerName;
        }
        // Rotu valinta
        private Race AskRace(Race rotu)
        {
            while (true)
            {
                Console.WriteLine("Valitse rotu");
                Console.WriteLine("1: Human");
                Console.WriteLine("2: Elf");
                Console.WriteLine("3: Rat");
                Console.WriteLine("4: Jesus");
                string raceAnswer = Console.ReadLine();
                if (raceAnswer == "1" || raceAnswer == "Human")
                {
                    Rotu = Race.Human.ToString();
                    break;
                }
                if (raceAnswer == "2" || raceAnswer == "Elf")
                {
                    Rotu = Race.Elf.ToString();
                    break;
                }
                if (raceAnswer == "3" || raceAnswer == "Rat")
                {
                    Rotu = Race.Rat.ToString();
                    break;
                }
                if (raceAnswer == "4" || raceAnswer == "Jesus")
                {
                    Rotu = Race.Jesus.ToString();
                    break;
                }
                else
                {
                    Console.WriteLine("Ei ole olemassa, valitse uudestaan");
                }
            }
            return rotu;
        }
        // Luokka valinta
        private Role AskClass(Role luokka)
        {
            while (true)
            {
                Console.WriteLine("Valitse luokka");
                Console.WriteLine("1: Wizard");
                Console.WriteLine("2: Fighter");
                Console.WriteLine("3: Swordfighter");
                Console.WriteLine("4: Archer");
                Console.WriteLine("5: ManWithBigWoodenStick");
                string classAnswer = Console.ReadLine();

                if (classAnswer == "1" || classAnswer == "Wizard")
                {
                    Luokka = Role.Wizard.ToString();
                    break;
                }
                if (classAnswer == "2" || classAnswer == "Fighter")
                {
                    Luokka = Role.Fighter.ToString();
                    break;
                }
                if (classAnswer == "3" || classAnswer == "Swordfighter")
                {
                    Luokka = Role.Swordfighter.ToString();
                    break;
                }
                if (classAnswer == "4" || classAnswer == "Archer")
                {
                    Luokka = Role.Archer.ToString();
                    break;
                }
                if (classAnswer == "5" || classAnswer == "ManWithBigWoodenStick")
                {
                    Luokka = Role.ManWithBigWoodenStick.ToString();
                    break;
                }
                else
                {
                    Console.WriteLine("Ei ole olemassa, valitse uudestaan");
                }
            }
            return luokka;
        }
        private PlayerCharacter CreateCharacter()
        {
            PlayerCharacter player = new PlayerCharacter();
            player.PlayerName = AskName();
            player.rotu = AskRace(player.rotu);
            player.luokka = AskClass(player.luokka);
            return player;
        }

        public void Run()
        {
            //Console.Clear();
            InIt();
            GameLoop();


        }
        private void InIt()
        {


            player = CreateCharacter();
            MapLoader loader = new MapLoader();
            level01 = loader.LoadMapFromFile();

            // Set the window size
            game_width = 480;
            game_height = 270;
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
                currentGameState = GameState.GameLoop;

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

            // Piirrä peli skaalattuna ruudulle
            DrawGameToTexture();
            MainMenu();
        }
        public void DrawGameToTexture() 
        {
            //DrawGame();
            DrawGameScaled();
        }
        private void DrawGameScaled()
        {
            // Piirrä peli skaalattuna ruudulle
            Raylib.BeginDrawing();
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
            player.position = new Vector2(1, 1);
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
                MapLayer layer = level01.GetLayer("ground");
                MapLayer Itemlayer = level01.GetLayer("items");

                if (layer.mapTiles[index] != 5)
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
                        // Tämä koodi on uutta
                        MainMenu();
                        break;

                    case GameState.GameLoop:
                        // Tämä koodi on se mitä GameLoop() funktiossa oli ennen muutoksia
                        Console.WriteLine("Fuck me bruh");
                        UpdateGame();
                        DrawGameToTexture();
                        break;
                }
            } // while(true) ends
        } // GameLoop ends
    }
}
