using System;
using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using ZeroElectric.Vinculum;

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
            Console.Clear();
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

            
            player.SetImageAndIndex(Character, 1, 0);
            level01.SetImageAndIndex(Wall, 1, 0);
            level01.SetImageAndIndex(Floor, 1, 0);

            // Create render texture and set filtering
            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);

            Raylib.SetTargetFPS(30);
            Console.Clear();
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
            Raylib.EndDrawing();
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
                //Check collisions with walls
                int newX = (int)player.position.X + moveX;
                int newY = (int)player.position.Y + moveY;
                int index = newX + newY * level01.mapWidth;

                if (level01.mapTiles[index] != 5)
                {
                    // The new position is not a floor tile (not walkable), so do not move the player
                    moveX = 0; moveY = 0;
                    
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

                Console.Clear();
                DrawGame();
            }
        }
        private void GameLoop()
        {
            while (Raylib.WindowShouldClose() == false)
            {
                UpdateGame();
                DrawGame();
            } // while(true) ends
        } // GameLoop ends
    }
}
