using System;
using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;
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

            Raylib.InitWindow(480, 270, "Rogue Game");

            Raylib.SetTargetFPS(30);
        }
        private void DrawGame()
        {
            Console.Clear();
            level01.Draw();
            player.Draw();
        }
        private void UpdateGame()
        {
            // Set player starting position
            player.position = new Vector2(1, 1);
            while (true)
            {
                int moveX = 0;
                int moveY = 0;

                // Wait for keypress and compare value to ConsoleKey enum
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                {
                    moveY = -1;
                }
                else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                {
                    moveY = 1;
                }
                else if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                {
                    moveX = -1;
                }
                else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                {
                    moveX = 1;
                }

                //Check collisions with walls
                //Check collisions with walls
                int newX = (int)player.position.X + moveX;
                int newY = (int)player.position.Y + moveY;
                int index = newX + newY * level01.mapWidth;

                if (level01.mapTiles[index] != 1)
                {
                    // The new position is not a floor tile (not walkable), so do not move the player
                    continue;
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
                // Redraw the player
                level01.Draw();
                player.Draw();
            }
        }
        private void GameLoop()
        {
            while (Raylib.WindowShouldClose() == false)
            {
                DrawGame();
                UpdateGame();
            } // while(true) ends
        } // GameLoop ends
    }
}
