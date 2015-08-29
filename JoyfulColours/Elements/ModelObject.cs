using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class ModelObject
    {
        public Dictionary<string, GeometryModel3D> Geometries { get; }
            = new Dictionary<string, GeometryModel3D>();

        public MaterialLibrary MaterialLibrary { get; }

        public ModelObject(Loader l)
        {
            GeometryModel3D model = null;
            MeshGeometry3D geometry = null;

            List<Point3D> vertices = new List<Point3D>();
            List<Vector3D> normals = new List<Vector3D>();
            List<Point> textures = new List<Point>();

            foreach (Instruction i in l.Parse())
            {
                switch (i.Type)
                {
                    case "mtllib":
                        MaterialLibrary = l.Find(i.Arg).Load<MaterialLibrary>();
                        break;
                    case "usemtl":
                        model.Material = MaterialLibrary.Materials[i.Arg];
                        break;
                    case "o":
                        model = new GeometryModel3D();
                        Geometries.Add(i.Arg, model);
                        model.Geometry = geometry = new MeshGeometry3D();
                        break;
                    case "f":
                        foreach (string index in i.Args)
                        {
                            string[] values = index.Split('/');
                            geometry.Positions.Add(vertices[int.Parse(values[0]) - 1]);
                            if (values.Length > 1 && values[1] != "")
                                geometry.TextureCoordinates.Add(
                                    textures[int.Parse(values[1]) - 1]);
                            if (values.Length > 2)
                                geometry.Normals.Add(normals[int.Parse(values[2]) - 1]);
                        }
                        break;
                    case "v": // Vertex
                        vertices.Add(new Point3D(i.Double(), i.Double(), i.Double()));
                        break;
                    case "vn": // Vertex normal
                        normals.Add(new Vector3D(i.Double(), i.Double(), i.Double()));
                        break;
                    case "vt": // Vertex texture coordinate
                        textures.Add(new Point(i.Double(), i.Double()));
                        break;
                    case "g": // Group
                        // TODO: Add support for object groups
                        break;
                    case "s": // Smooth
                        // Not supported, just ignore
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
