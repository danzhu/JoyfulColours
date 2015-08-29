using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class Skeleton
    {
        public string ID { get; set; }

        public Dictionary<string, NodeTemplate> Nodes { get; } = new Dictionary<string, NodeTemplate>();
        public NodeTemplate Root { get; set; }

        NodeTemplate node;

        public Skeleton(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        public virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "node":
                    string id = i.String();
                    node = new NodeTemplate(id);
                    Nodes[id] = node;
                    if (Root == null)
                        Root = node;
                    break;
                case "par": // Parent
                    Nodes[i.String()].Children.Add(node);
                    break;
                case "cent":
                    node.Center = i.Point3D();
                    break;
                case "axis":
                    node.Axis = i.Vector3D();
                    break;
            }
        }
    }
}
