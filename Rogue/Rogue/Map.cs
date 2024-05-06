using Rogue.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{

    public class MapLayer
    {
        public string name;
        public int[] mapTiles;
    }

    public class Map
    {
        public int mapWidth;
        public int mapHeight;
        public MapLayer[] layers;

        List<Enemy> enemies;
        List<Items> items;

        public MapLayer GetLayer(string layerName)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
           return null; // Wanted layer was not found!
        }

        public Vector2 position;

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
        int atlasIndex2;
        

        public void SetImageAndIndex(Texture atlasImage, int imagesPerRow, int index)
        {
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * Game.tileSize;
            imagePixelY = (int)(index / imagesPerRow) * Game.tileSize;
        }

        public void LoadEnemiesAndItems(Texture spriteAtlas)
        {
            enemies = new List<Enemy>();
            items = new List<Items>();

            MapLayer enemyLayer = GetLayer("enemies");
            MapLayer itemLayer = GetLayer("items");

            int[] enemyTiles = enemyLayer.mapTiles;
            int[] itemTiles = itemLayer.mapTiles;

            int mapHeight = enemyTiles.Length / mapWidth;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;
                    int enemyTileId = enemyTiles[index];
                    int itemTileId = itemTiles[index];

                    if (enemyTileId != 0)
                    {
                        // Tässä kohdassa kenttää on vihollinen
                        // enemyTileId voi olla sama kuin drawIndex
                        enemies.Add(new Enemy("Gandalf the purple", position, spriteAtlas, enemyTileId));
                    }

                    if (itemTileId != 0)
                    {
                        // Tässä kohdassa kenttää on esine
                        // itemTileId voi olla sama kuin drawIndex
                        items.Add(new Items("Potion", position, spriteAtlas, itemTileId));
                    }
                }
            }
        }

        internal Enemy GetEnemyAt(int x, int y)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.position.X == x && enemy.position.Y == y)
                {
                    return enemy;
                }
            }
            return null;
        }

        internal Items GetItemAt(int x, int y)
        {
            foreach (Items item in items)
            {
                if (item.position.X == x && item.position.Y == y)
                {
                    return item;
                }
            }
            return null;
        }



        public void Draw()
        {
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;


            

            int mapHeight = mapTiles.Length / mapWidth; // Calculate the height: the amount of rows

            atlasIndex = 4 + 3 * imagesPerRow;
            atlasIndex2 = 1 + 4 * imagesPerRow;
            


            // Laske kuvan kohta

            //Seinän
            int WallX = atlasIndex % imagesPerRow;
            int WallY = (int)(atlasIndex / imagesPerRow);
            int imagePixelX = WallX * tileSize;
            int imagePixelY = WallY * tileSize;
            
            //Lattian
            int FloorX = atlasIndex2 % imagesPerRow;
            int FloorY = (int)(atlasIndex2 / imagesPerRow);
            int imagePixelXB = FloorX * tileSize;
            int imagePixelYB = FloorY * tileSize;

        

            Rectangle WallTexture = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

            Rectangle FloorTexture = new Rectangle(imagePixelXB, imagePixelYB, Game.tileSize, Game.tileSize);

         



            for (int y = 0; y < mapHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {

                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)

                    int tileId = mapTiles[index]; // Read the tile value at index
                    if (tileId == 0)
                    {
                        continue;
                    }
                    int tileIndex = tileId - 1;
                    // Laske palan pikselikordinaatit kuvassa tileIndex;in avulla

                    int pixelX = (int)(x * tileSize);
                            int pixelY = (int)(y * tileSize);
                    void Move(int moveX, int moveY)
                    {
                        // Move the player
                        position.X += moveX;
                        position.Y += moveY;
                    }

                    Vector2 pixelPosition = new Vector2(pixelX, pixelY);

                    switch (tileId)
                    {
                        case 5:

                            // Floor
                            //Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, Raylib.BLANK);
                            //Raylib.DrawText(".", pixelX + 5, pixelY, tileSize, Raylib.WHITE);
                            Raylib.DrawTextureRec(image, FloorTexture, pixelPosition, Raylib.WHITE);
                            break;
                        case 8:
                            //Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, Raylib.DARKGRAY); // Wall
                            //Raylib.DrawText("#", pixelX, pixelY, tileSize, Raylib.WHITE);
                            Raylib.DrawTextureRec(image, WallTexture, pixelPosition, Raylib.WHITE);
                            break;
                        default:
                            break;
                    }
                }
            }

            // Piirretään sitten esineet
            foreach (Items item in items)
            {
                item.Draw();
            }

            // Lopuksi piirretään viholliset
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw();
            }
        }
    }
}
