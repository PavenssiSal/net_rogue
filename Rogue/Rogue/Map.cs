using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    public class Map
    {
        public int mapWidth;
        public int mapHeight;
        public int[] mapTiles;

        public Vector2 position;

        public Color drawColor;
        public void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Gray; // Change to map color
            int mapHeight = mapTiles.Length / mapWidth; // Calculate the height: the amount of rows

            int tileSize = 16;
            for (int y = 0; y < mapHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {

                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileId = mapTiles[index]; // Read the tile value at index


                            int pixelX = (int)(x * tileSize);
                            int pixelY = (int)(y * tileSize);
                    void Move(int moveX, int moveY)
                    {
                        // Move the player
                        position.X += moveX;
                        position.Y += moveY;
                    }


                    switch (tileId)
                    {
                        case 1:

                            // Floor
                            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, Raylib.BLANK);
                            Raylib.DrawText(".", pixelX + 5, pixelY, tileSize, Raylib.WHITE);
                            break;
                        case 2:
                            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, Raylib.DARKGRAY); // Wall
                            Raylib.DrawText("#", pixelX, pixelY, tileSize, Raylib.WHITE);
                            break;
                        default:
                            Raylib.DrawRectangle(pixelX, pixelY, tileSize, tileSize, Raylib.BLANK); ;
                            break;
                    }
                }
            }
            
        }
    }
}
