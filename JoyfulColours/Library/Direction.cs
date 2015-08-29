using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Library
{
    public struct Direction
    {
        readonly int angle;
        readonly int direction;

        public Direction(int ang)
        {
            direction = angle = ang;
            while (direction < 0)
                direction += 4;
            direction %= 4;
        }

        public int Shift(int offset)
        {
            // Sine function of 90 * angle
            return (2 - ((direction + offset) % 4)) % 2;
        }

        public Direction Offset(int dir)
        {
            return new Direction(angle + dir);
        }
        
        public static implicit operator double (Direction d)
        {
            return d.angle * 90;
        }

        public override string ToString() => $"{direction} ({(double)this} deg.)";
    }
}
