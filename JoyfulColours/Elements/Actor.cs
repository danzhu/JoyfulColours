using JoyfulColours.Animations;
using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    // TODO: Add AI (though it should be a long time after)
    public class Actor : Model
    {
        Movement movement;
        public Movement CurrentMovement => movement;

        public Actor(ActorTemplate template) : base(template) { }

        public bool Move(Movement m)
        {
            if (movement != null && movement != m)
                return false;

            Position3D offset = m.Template.PositionOffset;
            if (m.Template.FollowDirection)
                offset = offset.Rotate(Direction);
            Position3D pos = Position.Offset(offset);

            if (!Game.Scene.CanPassThrough(pos))
                return false;

            Direction dir = Direction.Offset(m.Template.DirectionOffset);
            SetPosition(pos);
            SetDirection(dir);
            movement = m;
            m.Completed += (sender, e) => movement = null;
            return true;
        }

        public override string ToString()
            => $"{nameof(Actor)} \"{Template.ID}\" ({Template.Name})";
    }
}
