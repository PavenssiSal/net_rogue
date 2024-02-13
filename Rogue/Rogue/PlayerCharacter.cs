﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public Role Luokka;

        public Vector2 position;

        // Lisätään pelaajalle kuvan ja piirtovärin muuttujat
        public char image;
        public ConsoleColor drawColor;

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
            drawColor = ConsoleColor.Magenta;

            Console.ForegroundColor = drawColor;

            // Draw the player
            Console.SetCursorPosition((int)position.X, (int)position.Y);
            Console.Write(image);
        }
    }
}
