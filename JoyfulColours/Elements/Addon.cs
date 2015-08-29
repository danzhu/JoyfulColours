using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class Addon : Element
    {
        public AddonTemplate Template { get; }

        public string Node => Template.Node;
        
        public Addon(AddonTemplate template)
        {
            Template = template;

            // Add geometries
            foreach (GeometryModel3D model in template.Model.Geometries.Values)
            {
                ModelVisual3D m = new ModelVisual3D();
                m.Content = model;
                Children.Add(m);
            }
        }

        public override string ToString() => $"{nameof(Addon)} \"{Template.ID}\"";
    }
}
