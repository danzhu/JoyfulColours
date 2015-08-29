using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Library
{
    public struct Dimension3D
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }

        public Dimension3D(int width, int height, int length)
        {
            Width = width;
            Height = height;
            Length = length;
        }
    }
}
