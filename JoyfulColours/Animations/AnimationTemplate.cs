using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Animations
{
    /// <summary>
    /// A template storing data for a <see cref="ModelAnimation"/>.
    /// </summary>
    public class AnimationTemplate
    {
        public string ID { get; }

        public bool IsAbsolute { get; set; } = true;
        public double Duration { get; set; } = 1.0;
        public Easing Easing { get; set; } = Easings.Linear;

        // Master translation and rotation
        public Vector3D? Translation { get; set; }
        public double? Rotation { get; set; }

        public Dictionary<string, Vector3D> Translations { get; }
            = new Dictionary<string, Vector3D>();
        public Dictionary<string, double> PrimaryRotations { get; }
            = new Dictionary<string, double>();
        public Dictionary<string, double> SecondaryRotations { get; }
            = new Dictionary<string, double>();

        public AnimationTemplate() { }

        public AnimationTemplate(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        protected virtual void Load(Loader l, Instruction i)
        {
            // TODO: Support more types of animations
            // TODO: Master translation / rotation
            switch (i.Type)
            {
                case "abs":
                    IsAbsolute = i.Bool();
                    break;
                case "dur":
                    Duration = i.Double();
                    break;
                case "ease":
                    Easing = Easings.Map[i.String()];
                    break;
                case "t":
                    Translations.Add(i.String(), i.Vector3D());
                    break;
                case "r":
                    PrimaryRotations.Add(i.String(), i.Double());
                    break;
                case "r2":
                    SecondaryRotations.Add(i.String(), i.Double());
                    break;
            }
        }
    }
}
