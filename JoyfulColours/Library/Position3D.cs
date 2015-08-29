using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Library
{
    public struct Position3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Position3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void ForEachInRange(Dimension3D dim, Action<Position3D> action)
        {
            for (int x = 0; x < dim.Width; x++)
                for (int y = 0; y < dim.Height; y++)
                    for (int z = 0; z < dim.Length; z++)
                        action(new Position3D(X + x, Y + y, Z + z));
        }
        
        public Position3D Rotate(Direction dir)
        {
            Position3D p = new Position3D();
            p.X = dir.Shift(1) * X + dir.Shift(0) * Z;
            p.Y = 0;
            p.Z = dir.Shift(1) * Z + dir.Shift(3) * X;
            return p;
        }

        public Position3D Offset(Position3D p)
        {
            return new Position3D(X + p.X, Y + p.Y, Z + p.Z);
        }

        public static implicit operator Point3D(Position3D l)
        {
            return new Point3D(l.X, l.Y, l.Z);
        }

        public static implicit operator Vector3D(Position3D l)
        {
            return new Vector3D(l.X, l.Y, l.Z);
        }

        public static Vector3D operator -(Position3D p1, Position3D p2)
        {
            return new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
        }
        
        public override string ToString() => $"{X}, {Y}, {Z}";
    }
}
