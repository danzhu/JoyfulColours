using JoyfulColours.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Animations
{
    public class CamAnimation : Animation
    {
        public Cam Camera { get; }
        public bool IsAbsolute { get; set; }

        Point3D startPosition;
        Vector3D? deltaPosition;
        public Point3D? Position { get; set; }

        Point3D startTarget;
        Vector3D? deltaTarget;
        public Point3D? Target { get; set; }

        Vector3D startDirection;
        Vector3D? deltaDirection;
        public Vector3D? LookDirection { get; set; }

        public CamAnimation(Cam cam)
        {
            Camera = cam;
        }

        protected override void OnStarted()
        {
            startPosition = Camera.Position;
            startTarget = Camera.Target;
            startDirection = Camera.LookDirection;

            if (Position != null)
                deltaPosition = IsAbsolute ? Position - startPosition : (Vector3D)Position;
            if (Target != null)
                deltaTarget = IsAbsolute ? Target - startTarget : (Vector3D)Target;
            if (LookDirection != null)
                deltaDirection = IsAbsolute ? LookDirection - startDirection : LookDirection;
            base.OnStarted();
        }

        protected override void OnUpdated()
        {
            if (deltaPosition != null)
                Camera.Position = startPosition + (Vector3D)deltaPosition * Progress;
            if (deltaTarget != null)
                Camera.Target = startTarget + (Vector3D)deltaTarget * Progress;
            if (deltaDirection != null)
                Camera.LookDirection = startDirection + (Vector3D)deltaDirection * Progress;
        }
    }
}
