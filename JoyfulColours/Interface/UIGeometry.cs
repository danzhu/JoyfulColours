using JoyfulColours.Elements;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Interface
{
    public class UIGeometry
    {
        public UITemplate UITemplate { get; }
        public Geometry3D Geometry { get; }

        public UIGeometry(UITemplate ui, Geometry3D geometry)
        {
            UITemplate = ui;
            Geometry = geometry;
        }

        public void CreateVisual3D(Model model, ref ScriptScope script)
        {
            UI ui = new UI(UITemplate, ref script);

            Viewport2DVisual3D visual = new Viewport2DVisual3D();
            visual.Geometry = Geometry;
            visual.Visual = ui.Visual;
            EmissiveMaterial material = new EmissiveMaterial(Brushes.White);
            Viewport2DVisual3D.SetIsVisualHostMaterial(material, true);
            visual.Material = material;

            model.Children.Add(visual);
        }
    }
}
