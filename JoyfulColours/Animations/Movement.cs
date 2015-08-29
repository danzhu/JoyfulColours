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
        public MovementAnimation MovementAnimation { get; }
        public ModelAnimation Completion { get; }

        Animation timeout;
        Position3D pos;
        Direction dir;

        public Movement(MovementTemplate template, Actor actor)
        {
            Template = template;
            Actor = actor;

            Animation = template.Animation.Create(actor);
            Animation.Completed += TestContinuation;

            MovementAnimation = new MovementAnimation(actor);
            MovementAnimation.Duration = template.Animation.Duration / template.Speed;

            Completion = new ModelAnimation(actor, template.Completion);
            Completion.Completed += TestMovement;

            timeout = new Animation(0.1);
            timeout.Completed += TestMovement;

            Started += TestMovement;
        }

        private bool CanMove()
        {
            if (Actor.Movement == null)
                Actor.Movement = this;
            else if (Actor.Movement != this)
                return false;

            Position3D offset = Template.PositionOffset;
            if (Template.FollowDirection)
                offset = offset.Rotate(Actor.Direction);
            pos = Actor.Position.Offset(offset);

            if (!Game.Scene.CanPassThrough(pos))
                return false;

            dir = Actor.Direction.Offset(Template.DirectionOffset);
            return true;
        }

        private void TestContinuation(object sender, EventArgs e)
        {
            // TODO: Fix bottleneck
            if (!IsStopping && CanMove())
                StartAnimation();
            else
                Completion.Start();
        }

        private void TestMovement(object sender, EventArgs e)
        {
            if (IsStopping)
                Complete();
            else if (CanMove())
                StartAnimation();
            else
                timeout.Start();
        }

        private void StartAnimation()
        {
            MovementAnimation.Position = pos;
            MovementAnimation.Direction = dir;
            Animation.Start();
            MovementAnimation.Start();
        }

        protected override void OnStarted(EventArgs e)
        {
            base.OnStarted(e);
        }

        protected override void OnCompleted(EventArgs e)
        {
            if (Actor.Movement == this)
                Actor.Movement = null;
            base.OnCompleted(e);
        }

        protected override void OnStopping(EventArgs e)
        {
            base.OnStopping(e);
            Animation.Stop();
        }
    }
}
