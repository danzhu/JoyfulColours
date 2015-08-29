using JoyfulColours.Animations;
using JoyfulColours.Elements;
using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Interface
{
    public class Cam
    {
        public PerspectiveCamera Camera { get; } = new PerspectiveCamera();

        public Dictionary<string, CamAnimation> Animations { get; }
            = new Dictionary<string, CamAnimation>();

        public bool TrackTarget { get; set; }

        public Point3D Position
        {
            get { return Camera.Position; }
            set
            {
                if (TrackTarget)
                    Camera.LookDirection = Target - value;
                Camera.Position = value;
            }
        }

        public Point3D Target
        {
            get { return Camera.Position + Camera.LookDirection; }
            set
            {
                Camera.LookDirection = value - Camera.Position;
            }
        }

        public Vector3D LookDirection
        {
            get { return Camera.LookDirection; }
            set { Camera.LookDirection = value; }
        }

        public Cam(Loader l)
        {
            // TODO: Set transformation to follow Scene
            CamAnimation anim = null;

            foreach (Instruction i in l.Parse())
            {
                switch (i.Type)
                {
                    case "track":
                        TrackTarget = i.Bool();
                        break;
                    case "pos":
                        if (anim == null)
                            Position = i.Point3D();
                        else
                            anim.Position = i.Point3D();
                        break;
                    case "tar":
                        if (anim == null)
                            Target = i.Point3D();
                        else
                            anim.Target = i.Point3D();
                        break;
                    case "dir":
                        if (anim == null)
                            LookDirection = i.Vector3D();
                        else
                            anim.LookDirection = i.Vector3D();
                        break;
                    case "anim":
                        Animations[i.String()] = anim = new CamAnimation(this);
                        break;
                    case "ease":
                        anim.Easing = Easings.Map[i.String()];
                        break;
                    case "abs":
                        anim.IsAbsolute = i.Bool();
                        break;
                    case "dur":
                        anim.Duration = i.Double();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Follow(ModelVisual3D visual)
        {
            Camera.Transform = visual.Transform;
        }
    }
}
