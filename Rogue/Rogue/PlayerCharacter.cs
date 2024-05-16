using System.Numerics;
using TurboMapReader;
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
        public char Picture;
        public ConsoleColor Color;
        public Color color;
        public Color drawColor;

        Texture image;
        int imagePixelX;
        int imagePixelY;

        int imagesPerRow = 12;
        int tileSize = 16;
        // indeksit ovat:
        // 0, 1
        // 2, 3
        int atlasIndex;

        


        public void Move(int moveX, int moveY)
        {
            // Move the player
            position.X += moveX;
            position.Y += moveY;
        }

        public void SetImageAndIndex(Texture atlasImage, int imagesPerRow, int index)
        {
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * Game.tileSize;
            imagePixelY = (int)(index / imagesPerRow) * Game.tileSize;
        }

        public enum MapTile : int
        {
            Floor = 5,
            Wall = 8
        }

        public void Draw()
        {

            atlasIndex = 1 + 10 * imagesPerRow;


            // Laske kuvan kohta
            int imageX = atlasIndex % imagesPerRow;
            int imageY = (int)(atlasIndex / imagesPerRow); 
            int imagePixelX = imageX * tileSize; 
            int imagePixelY = imageY * tileSize; 

            // Laske pelaajan kordinaatit pikseleissä
            int pixelX = (int)(position.X * Game.tileSize);
            int pixelY = (int)(position.Y * Game.tileSize);

            Vector2 pixelPosition = new Vector2(pixelX, pixelY);

            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

            Raylib.DrawTextureRec(image, imageRect, pixelPosition, Raylib.WHITE);
        }
    }
}