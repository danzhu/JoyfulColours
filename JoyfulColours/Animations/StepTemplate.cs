using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Animations
{
    public class StepTemplate
    {
        public AnimationPose Pose { get; set; }
        public double Duration { get; set; } = 0;
        public Easing Easing { get; set; } = Easings.Linear;

        public StepTemplate(AnimationPose pose)
        {
            Pose = pose;
        }
    }
}
