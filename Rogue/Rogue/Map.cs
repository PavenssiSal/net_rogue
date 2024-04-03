using System;
using System.Collections.Generic;
using System.Linq;
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

        public Color drawColor;
        public void Draw()
        {
            Raylib.BeginDrawing();
            Console.ForegroundColor = ConsoleColor.Gray; // Change to map color
            int mapHeight = mapTiles.Length / mapWidth; // Calculate the height: the amount of rows

            int tileSize = 16;
            for (int y = 0; y < mapHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {

                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileId = mapTiles[index]; // Read the tile value at index

                    Rectangle tileRect = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);

                    // Draw the tile graphics
                    Console.SetCursorPosition(x, y);
                    switch (tileId)
                    {
                        case 1:
                            Raylib.DrawRectangleRec(tileRect, Raylib.BLANK); // Floor
                            Raylib.DrawText(".", (int)(tileRect.x + tileSize * 0.5f), (int)(tileRect.y + tileSize * 0), tileSize, Raylib.WHITE);
                            break;
                        case 2:
                            Raylib.DrawRectangleRec(tileRect, Raylib.DARKGRAY); // Wall
                            Raylib.DrawText("#", (int)(tileRect.x + tileSize * 0), (int)(tileRect.y + tileSize * 0), tileSize, Raylib.WHITE);
                            break;
                        default:
                            Raylib.DrawRectangleRec(tileRect, Raylib.BLANK); ;
                            break;
                    }
                }
            }
            Raylib.EndDrawing();
        }
    }
}
