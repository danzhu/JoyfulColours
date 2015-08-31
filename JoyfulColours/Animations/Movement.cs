using JoyfulColours.Elements;
using JoyfulColours.Library;
using JoyfulColours.Logic;
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
        public Sequence Completion { get; }

        Animation timeout;
        Position3D pos;
        Direction dir;

        public Movement(MovementTemplate template, Actor actor)
        {
            Template = template;
            Actor = actor;

            Animation = template.Animation.Create(actor);
            Event.Register(Animation, Completed, TestContinuation);

            MovementAnimation = new MovementAnimation(actor);
            MovementAnimation.Duration = template.Animation.Duration / template.Speed;

            Completion = template.Completion.Create(actor);
            Event.Register(Completion, Completed, TestMovement);

            timeout = new Animation(0.1);
            Event.Register(timeout, Completed, TestMovement);

            Event.Register(this, Started, TestMovement);
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

        private void TestContinuation(object sender, LogicEventArgs e)
        {
            // TODO: Fix bottleneck
            if (!IsStopping && CanMove())
                StartAnimation();
            else
                Completion.Start();
        }

        private void TestMovement(object sender, LogicEventArgs e)
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
        
        protected override void OnCompleted()
        {
            if (Actor.Movement == this)
                Actor.Movement = null;
            base.OnCompleted();
        }

        protected override void OnStopping()
        {
            base.OnStopping();
            Animation.Stop();
        }
    }
}
