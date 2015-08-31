using JoyfulColours.Elements;
using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Animations
{
    public class MovementAnimation : Animation
    {
        public Actor Actor { get; }

        Point3D startPosition;
        Vector3D deltaPosition;
        public Position3D Position { get; set; }

        double startDirection;
        double deltaDirection;
        public Direction Direction { get; set; }

        public MovementAnimation(Actor actor)
        {
            Actor = actor;
        }

        protected override void OnStarted()
        {
            startPosition = Actor.Position;
            deltaPosition = Position - Actor.Position;
            Actor.SetPosition(Position);

            startDirection = Actor.Direction;
            deltaDirection = Direction - Actor.Direction;
            Actor.SetDirection(Direction);
            base.OnStarted();
        }

        protected override void OnUpdated()
        {
            Point3D p = startPosition + deltaPosition * Progress;
            Actor.Translation.OffsetX = p.X;
            Actor.Translation.OffsetY = p.Y;
            Actor.Translation.OffsetZ = p.Z;

            Actor.Rotation.Angle = startDirection + deltaDirection * Progress;
            base.OnUpdated();
        }
    }
}
