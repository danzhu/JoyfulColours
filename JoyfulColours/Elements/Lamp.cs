using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class Lamp : Element
    {
        public Lamp(Light light)
        {
            Content = light;
        }
    }
}
