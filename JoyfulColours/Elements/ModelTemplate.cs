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

        public List<EquipmentTemplate> Equipments { get; } = new List<EquipmentTemplate>();
        
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
                    Name = i.String();
                    break;
                case "model":
                    Model = l.Resource<ModelObject>(i.String());
                    break;
                case "dim":
                    Dimension = i.Dimension3D();
                    break;
                case "colli":
                    IsCollidable = i.Bool();
                    break;
                case "ui":
                    UITemplate ui = l.Resource<UITemplate>(i.String());
                    // Transfer geometry to ui
                    string geo = i.String();
                    UIGeometry model = new UIGeometry(ui, Model.Geometries[geo].Geometry);
                    Model.Geometries.Remove(geo);
                    UITemplates.Add(geo, model);
                    break;
                case "skl":
                    Skeleton = l.Resource<Skeleton>(i.String());
                    break;
                case "equip":
                    Equipments.Add(l.Resource<EquipmentTemplate>(i.String()));
                    break;
            }
        }
    }
}
