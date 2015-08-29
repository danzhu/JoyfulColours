using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class NodeTemplate
    {
        public string Name { get; }

        public Point3D Center { get; set; }
        public Vector3D Axis { get; set; }
        public List<NodeTemplate> Children { get; } = new List<NodeTemplate>();

        public NodeTemplate(string name)
        {
            Name = name;
        }
    }

}
