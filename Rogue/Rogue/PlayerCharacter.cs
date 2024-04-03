using System.Numerics;
using ZeroElectric.Vinculum;

namespace Rogue
{
    public enum Race
    {
        Human,
        Elf,
        Rat,
        Jesus
    }

    public enum Role
    {
        Fighter,
        Swordfighter,
        Archer,
        Wizard,
        ManWithBigWoodenStick,
        Homeless
    }

    public class PlayerCharacter
    {
        public string PlayerName { get; set; }
        public Race rotu;
        public Role luokka;

        public Vector2 position;

        // Lisätään pelaajalle kuvan ja piirtovärin muuttujat
        public char image;
        public ConsoleColor Color;
        public Color color;
        public Color drawColor;

        public void Move(int moveX, int moveY)
        {
            // Move the player
            position.X += moveX;
            position.Y += moveY;
        }

        public void Draw()
        {
            // Laske pelaajan kordinaatit pikseleissä
            int pixelX = (int)(position.X * Game.tileSize);
            int pixelY = (int)(position.Y * Game.tileSize);

            // Pelaajan väri
            Color drawColor = Raylib.MAGENTA;

            // Piirrä pelaajan neliö
            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, drawColor);

            // Piirrä merkki "@"
            Raylib.DrawText("@", pixelX, pixelY, Game.tileSize, Raylib.BLUE);

            // Og for testing reasons
            image = '¤';
            Color = ConsoleColor.Magenta;

            Console.ForegroundColor = Color;

            // Draw the player
            Console.SetCursorPosition((int)position.X, (int)position.Y);
            Console.Write(image);
        }
    }
}