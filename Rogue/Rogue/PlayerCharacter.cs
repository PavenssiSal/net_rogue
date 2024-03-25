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
        public Color drawColor;

        public void Move(int moveX, int moveY)
        {
            // Move the player
            
            position.X += moveX;
            position.Y += moveY;
        }
        public void Draw()
        {
            // Pelaajan kuva ja väri
            image = '¤';
            drawColor = Raylib.MAGENTA; // Set drawColor to Raylib's Color.MAGENTA

            // Draw the player
            int pixelX = (int)(position.X * Game.tileSize);
            int pixelY = (int)(position.Y * Game.tileSize);

            // Draw rectangle
            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, drawColor);

            // Draw character '@'
            Raylib.DrawText("@", pixelX + (Game.tileSize / 4), pixelY + (Game.tileSize / 4), Game.tileSize, Raylib.WHITE);

        }
    }
}
