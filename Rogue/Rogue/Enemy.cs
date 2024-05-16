using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue.Images
{

    internal class Enemy
    {
        public string name;       // Vihollisen nimi
        public Vector2 position;  // Missä vihollinen on kentässä
        private Texture graphics; // Viittaus kuvaan jossa vihollisen kuva on
        private int DrawIndex;    // Missä kohdassa kuvaa vihollinen on

        int imagesPerRow = 12;
        int tileSize = 16;

        int atlasIndex;
        public Enemy(string name, Vector2 position, Texture graphics, int drawIndex)
        {
            this.name = name;
            this.position = position;
            this.graphics = graphics;
            this.DrawIndex = drawIndex;
        }

        public void Draw()
        {

            atlasIndex = 0 + 7 * imagesPerRow;

            // Laske kuvan kohta
            int imageX = atlasIndex % imagesPerRow;
            int imageY = (int)(atlasIndex / imagesPerRow);
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
