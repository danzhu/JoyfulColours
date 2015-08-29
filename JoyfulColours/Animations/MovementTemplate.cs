using JoyfulColours.Elements;
using JoyfulColours.Library;
using JoyfulColours.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Animations
{
    public class MovementTemplate
    {
        public AnimationGroup Animation { get; set; }

        public Position3D PositionOffset { get; set; }
        public int DirectionOffset { get; set; }
        public double Speed { get; set; } = 1.0;
        public bool FollowDirection { get; set; } = true;

        public MovementTemplate(Loader l)
        {
            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        protected virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "anim":
                    Animation = l.Resource<AnimationGroup>(i.String());
                    break;
                case "speed":
                    Speed = i.Double();
                    break;
                case "pos":
                    PositionOffset = i.Position3D();
                    break;
                case "ang":
                    DirectionOffset = i.Int();
                    break;
                case "dir":
                    FollowDirection = i.Bool();
                    break;
            }
        }
    }
}
