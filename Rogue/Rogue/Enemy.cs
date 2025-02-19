using System;
using System.Numerics;
using Newtonsoft.Json;
using ZeroElectric.Vinculum;

namespace Rogue.Images
{
    internal class Enemy
    {
        public string name { get; set; }      // Vihollisen nimi
        public Vector2 position { get; set; } // Missä vihollinen on kentässä
        public int spriteId { get; set; }     // Tunniste viholliselle (mätsää karttaan)

        [JsonIgnore]
        private Texture graphics; // Viittaus kuvaan, ei tallenneta JSON:iin

        private int DrawIndex;
        private int imagesPerRow = 12;
        private int tileSize = 16;
        private int atlasIndex;

        // Oletuskonstruktori (tarvitaan JSON-deserialisointiin)
        public Enemy() { }

        // Pääasiallinen konstruktori
        public Enemy(string name, Vector2 position, int spriteId, Texture spriteAtlas)
        {
            this.name = name;
            this.position = position;
            this.spriteId = spriteId;
            this.graphics = spriteAtlas; // Asetetaan tekstuuri
        }

        // Kopiokonstruktori
        public Enemy(Enemy copyFrom)
        {
            this.name = copyFrom.name;
            this.position = copyFrom.position;
            this.spriteId = copyFrom.spriteId;
 
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
