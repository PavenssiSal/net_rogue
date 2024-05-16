using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    internal class Items
    {
        public string name;       // Item nimi
        public Vector2 position;  // Missä Item on kentässä
        private Texture graphics; // Viittaus kuvaan jossa Item kuva on
        private int DrawIndex;    // Missä kohdassa kuvaa Item on

        int imagesPerRow = 12;
        int tileSize = 16;

        int atlasIndex;
        public Items(string name, Vector2 position, Texture graphics, int drawIndex)
        {
            this.name = name;
            this.position = position;
            this.graphics = graphics;
            DrawIndex = drawIndex;
        }

        public void Draw()
        {
            atlasIndex = 7 + 9 * imagesPerRow;

            // Laske kuvan kohta
            int imageX = atlasIndex % imagesPerRow;
            int imageY = atlasIndex / imagesPerRow;
            int imagePixelX = imageX * tileSize;
            int imagePixelY = imageY * tileSize;

            // Laske vihollisen kordinaatit pikseleissä
            int pixelX = (int)(position.X * Game.tileSize);
            int pixelY = (int)(position.Y * Game.tileSize);

            Vector2 pixelPosition = new Vector2(pixelX, pixelY);

            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

            Raylib.DrawTextureRec(graphics, imageRect, pixelPosition, Raylib.WHITE);
        }
    }
}
