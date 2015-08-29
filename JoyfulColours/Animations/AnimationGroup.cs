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
    public class AnimationGroup
    {
        public List<AnimationTemplate> Templates { get; } = new List<AnimationTemplate>();

        public double Duration => Templates.Sum(t => t.Duration);
        public int StopIndex { get; set; } = -1;

        public AnimationGroup(Loader l)
        {
            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        protected virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "anm":
                    Templates.Add(l.Find(i.String()).Load<AnimationTemplate>());
                    while (i.HasNext)
                        switch (i.String())
                        {
                            case "-stop":
                                StopIndex = Templates.Count - 1;
                                break;
                            default:
                                break;
                        }
                    break;
            }
        }

        public Sequence Create(Model model)
        {
            List<Procedure> list = new List<Procedure>();
            foreach (AnimationTemplate template in Templates)
                list.Add(new ModelAnimation(model, template));

            if (StopIndex != -1)
                return new Sequence(list, StopIndex);
            else
                return new Sequence(list);
        }
    }
}
