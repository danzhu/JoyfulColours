using JoyfulColours.Elements;
using JoyfulColours.Library;
using JoyfulColours.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Animations
{
    public class Movement : Procedure
    {
        public MovementTemplate Template { get; }
        public Actor Actor { get; }

        public Sequence Animation { get; }
        public ModelAnimation MovementAnimation { get; }

        Animation timeout;

        public Movement(MovementTemplate template, Actor actor)
        {
            Template = template;
            Actor = actor;

            Animation = template.Animation.Create(actor);
            Animation.Completed += TestMovement;

            MovementAnimation = new ModelAnimation();
            MovementAnimation.Duration = template.Animation.Duration / template.Speed;
            MovementAnimation.IsAbsolute = true;
            MovementAnimation.AddTranslation(actor.Translation, new Vector3D());
            MovementAnimation.AddRotation(actor.Rotation, 0);

            timeout = new Animation(0.1);
            timeout.Completed += TestMovement;

            Started += TestMovement;
        }
        
        private void TestMovement(object sender, EventArgs e)
        {
            // TODO: Fix bottleneck
            if (IsStopping)
                CompleteAnimation();
            else if (Actor.Move(this))
                StartAnimation();
            else
                timeout.Start();
        }

        private void StartAnimation()
        {
            // TODO: Make type for MovementAnimation?
            MovementAnimation.TranslationTargets[0] = Actor.Position;
            MovementAnimation.RotationTargets[0] = Actor.Direction;
            Animation.Start();
            MovementAnimation.Start();
        }

        private void CompleteAnimation()
        {
            Complete();
        }

        protected override void OnStopping(EventArgs e)
        {
            base.OnStopping(e);
            Animation.Stop();
        }
    }
}
