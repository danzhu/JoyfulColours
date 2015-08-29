using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Library
{
    public struct Instruction
    {
        public string Type { get; }
        public string Arg { get; } // TODO: Remove arg for unified access
        public string[] Args { get; }

        int position;

        public bool HasNext
        {
            get { return position < Args.Length; }
        }

        public Instruction(string line)
        {
            int space = line.IndexOf(' ');
            Type = line.Substring(0, space);
            Arg = line.Substring(space + 1);
            
            Args = Arg.Split(' ');
            position = 0;
        }

        public string String()
        {
            return Args[position++];
        }

        public bool Bool()
        {
            return bool.Parse(Args[position++]);
        }
        
        public int Int()
        {
            return int.Parse(Args[position++]);
        }

        public float Float()
        {
            return float.Parse(Args[position++]);
        }

        public double Double()
        {
            return double.Parse(Args[position++]);
        }

        public Position3D Position3D()
        {
            return new Position3D(Int(), Int(), Int());
        }

        public Dimension3D Dimension3D()
        {
            return new Dimension3D(Int(), Int(), Int());
        }

        public Point3D Point3D()
        {
            return new Point3D(Double(), Double(), Double());
        }

        public Vector3D Vector3D()
        {
            return new Vector3D(Double(), Double(), Double());
        }

        public Color Color()
        {
            Color c = new Color();
            c.ScA = 1.0f;
            c.ScR = Float();
            c.ScG = Float();
            c.ScB = Float();
            return c;
        }

        public override string ToString() => $"{Type} {Arg}";
    }
}
