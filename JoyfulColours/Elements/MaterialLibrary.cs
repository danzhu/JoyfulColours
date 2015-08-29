using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class MaterialLibrary
    {
        public Dictionary<string, Material> Materials { get; }
            = new Dictionary<string, Material>();

        public MaterialLibrary(Loader l)
        {
            MaterialGroup group = null;
            DiffuseMaterial diffuse = null;
            SpecularMaterial specular = null;

            foreach (Instruction i in l.Parse())
            {
                switch (i.Type)
                {
                    case "newmtl":
                        group = new MaterialGroup();

                        diffuse = new DiffuseMaterial();
                        group.Children.Add(diffuse);
                        specular = new SpecularMaterial();
                        group.Children.Add(specular);

                        Materials.Add(i.Arg, group);
                        break;
                    case "Ns":
                        specular.SpecularPower = i.Double();
                        break;
                    case "d":
                    case "Tr":
                        diffuse.Brush.Opacity = i.Float();
                        break;
                    case "Ka":
                        diffuse.AmbientColor = i.Color();
                        break;
                    case "Kd":
                        diffuse.Brush = new SolidColorBrush(i.Color());
                        break;
                    case "Ks":
                        specular.Brush = new SolidColorBrush(i.Color());
                        break;
                    // TODO: UV Maps
                    case "illum":
                    case "Ni": // Index of Refraction
                        // Unsupported, ignore
                        break;
                    default:
                        // Unrecognized symbol
                        break;
                }
            }
        }
    }
}
