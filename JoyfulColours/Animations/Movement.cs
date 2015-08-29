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
        public ModelAnimation Completion { get; }

        Animation timeout;

        public Movement(MovementTemplate template, Actor actor)
        {
            Template = template;
            Actor = actor;

            Animation = template.Animation.Create(actor);
            Animation.Completed += TestContinuation;

            MovementAnimation = new ModelAnimation();
            MovementAnimation.Duration = template.Animation.Duration / template.Speed;
            MovementAnimation.IsAbsolute = true;
            MovementAnimation.AddTranslation(actor.Translation, new Vector3D());
            MovementAnimation.AddRotation(actor.Rotation, 0);

            Completion = new ModelAnimation(actor, template.Completion);
            Completion.Completed += (sender, e) => Complete();

            timeout = new Animation(0.1);
            timeout.Completed += TestMovement;

            Started += TestMovement;
        }

        private void TestContinuation(object sender, EventArgs e)
        {
            // TODO: Fix bottleneck
            if (!IsStopping && Actor.Move(this))
                StartAnimation();
            else
                Completion.Start();
        }

        private void TestMovement(object sender, EventArgs e)
        {
            if (IsStopping)
                Complete();
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
        
        protected override void OnStopping(EventArgs e)
        {
            base.OnStopping(e);
            Animation.Stop();
        }
    }
}
