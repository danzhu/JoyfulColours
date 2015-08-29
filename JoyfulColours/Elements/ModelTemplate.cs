using JoyfulColours.Interface;
using JoyfulColours.Library;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    /// <summary>
    /// Provides model-related functionality for model templates.
    /// </summary>
    public class ModelTemplate
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public ModelObject Model { get; set; }
        
        public Dictionary<string, UIGeometry> UITemplates { get; }
            = new Dictionary<string, UIGeometry>();

        public bool IsCollidable { get; set; } = true;

        public Dimension3D Dimension { get; set; } = new Dimension3D(1, 1, 1);

        public Skeleton Skeleton { get; set; }

        public CompiledCode Code { get; set; }

        public ModelTemplate(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        public virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "name":
                    Name = i.Arg;
                    break;
                case "model":
                    Model = l.Find(i.Arg).Load<ModelObject>();
                    break;
                case "dim":
                    Dimension = new Dimension3D(i.Int(), i.Int(), i.Int());
                    break;
                case "colli":
                    IsCollidable = i.Bool();
                    break;
                case "ui":
                    UITemplate ui = Game.Get<UITemplate>(i.String());
                    // Transfer geometry to ui
                    string geo = i.String();
                    UIGeometry model = new UIGeometry(ui, Model.Geometries[geo].Geometry);
                    Model.Geometries.Remove(geo);
                    UITemplates.Add(geo, model);
                    break;
                case "skl":
                    Skeleton = Game.Get<Skeleton>(i.String());
                    break;
                case "script":
                    Code = l.Find(i.String()).Load<CompiledCode>();
                    break;
            }
        }
    }
}
