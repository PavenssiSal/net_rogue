using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public enum Class
        {
            Fighter,
            Swordfighter,
            Archer,
            Wizard,
            ManWithBigWoodenStick,
            Homeless
        }
    internal class PlayerCharacter
    {
        public string PlayerName { get; set; }
        public Race rotu;
    }
}
