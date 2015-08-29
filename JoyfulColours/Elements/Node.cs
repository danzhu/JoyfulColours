using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class Node : ModelVisual3D
    {
        public TranslateTransform3D Translation { get; }
        public AxisAngleRotation3D PrimaryRotation { get; }
        public AxisAngleRotation3D SecondaryRotation { get; }

        public List<Addon> Addons { get; } = new List<Addon>();

        public Node(NodeTemplate template)
        {
            Translation = new TranslateTransform3D();
            PrimaryRotation = new AxisAngleRotation3D(template.Axis, 0);

            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new RotateTransform3D(PrimaryRotation, template.Center));
            group.Children.Add(Translation);
            Transform = group;
        }

        public void Equip(Addon addon)
        {
            Children.Add(addon);
            Addons.Add(addon);
        }

        public void Unequip(Addon addon)
        {
            Children.Remove(addon);
            Addons.Remove(addon);
        }
    }

}
