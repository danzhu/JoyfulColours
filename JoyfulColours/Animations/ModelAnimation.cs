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
    public class ModelAnimation
    {
        public List<StepTemplate> Templates { get; } = new List<StepTemplate>();

        public double Duration => Templates.Sum(t => t.Duration);
        public int StopIndex { get; set; } = -1;

        public ModelAnimation(Loader l)
        {
            foreach (Instruction i in l.Parse())
            {
                switch (i.Type)
                {
                    case "anim":
                        AnimationPose pose = l.Find(i.String()).Load<AnimationPose>();
                        Templates.Add(new StepTemplate(pose));
                        if (i.HasNext && i.String() == "-stop")
                            StopIndex = Templates.Count - 1;
                        break;
                    case "dur":
                        Templates.Last().Duration = i.Double();
                        break;
                    case "ease":
                        Templates.Last().Easing = Easings.Map[i.String()];
                        break;
                }
            }
        }
        
        public Sequence Create(Model model)
        {
            List<Procedure> list = new List<Procedure>();
            foreach (StepTemplate template in Templates)
                list.Add(new AnimationStep(template, model));

            if (StopIndex != -1)
                return new Sequence(list, StopIndex);
            else
                return new Sequence(list);
        }
    }
}
