using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    /// <summary>
    /// Provides basic fucntionality for any models / lights in a 3D scene.
    /// </summary>
    public class Element : ModelVisual3D
    {
        public TranslateTransform3D Translation { get; } = new TranslateTransform3D();
        public AxisAngleRotation3D Rotation { get; } = new AxisAngleRotation3D();
        
        // TODO: Implement IsEnabled

        Position3D position;
        public Position3D Position
        {
            get { return position; }
            set
            {
                position = value;
                Translation.OffsetX = value.X;
                Translation.OffsetY = value.Y;
                Translation.OffsetZ = value.Z;
            }
        }

        Direction direction;
        public Direction Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                Rotation.Angle = value;
            }
        }

        public Element()
        {
            // TODO: Add custom rotation and (optional) scaling
            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new RotateTransform3D(Rotation, 0.5, 0, 0.5));
            group.Children.Add(Translation);
            Transform = group;
        }

        public void SetPosition(Position3D pos)
        {
            position = pos;
        }

        public void SetDirection(Direction dir)
        {
            direction = dir;
        }
    }
}
