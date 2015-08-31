using JoyfulColours.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Animations
{
    /// <summary>
    /// A single animation between two states of a model.
    /// </summary>
    public class AnimationStep : Animation
    {
        public Model Model { get; }

        public bool IsAbsolute { get; set; } = true;

        public List<TranslateTransform3D> Translations { get; }
            = new List<TranslateTransform3D>();
        public List<Vector3D> TranslationTargets { get; }
            = new List<Vector3D>();
        List<Vector3D> startTranslations = new List<Vector3D>();
        List<Vector3D> deltaTranslations = new List<Vector3D>();

        public List<AxisAngleRotation3D> Rotations { get; }
            = new List<AxisAngleRotation3D>();
        public List<double> RotationTargets { get; }
            = new List<double>();
        List<double> startRotations = new List<double>();
        List<double> deltaRotations = new List<double>();

        public AnimationStep()
        {

        }

        public AnimationStep(StepTemplate template, Model model) : this()
        {
            Model = model;

            IsAbsolute = template.Pose.IsAbsolute;
            Duration = template.Duration;
            Easing = template.Easing;
            
            // Node transform
            foreach (var item in template.Pose.Translations)
                AddTranslation(Model.Nodes[item.Key].Translation, item.Value);
            foreach (var item in template.Pose.PrimaryRotations)
                AddRotation(Model.Nodes[item.Key].PrimaryRotation, item.Value);
            foreach (var item in template.Pose.SecondaryRotations)
                AddRotation(Model.Nodes[item.Key].PrimaryRotation, item.Value);
        }

        public void AddTranslation(TranslateTransform3D t, Vector3D end)
        {
            Translations.Add(t);
            TranslationTargets.Add(end);
        }

        public void AddRotation(AxisAngleRotation3D r, double end)
        {
            Rotations.Add(r);
            RotationTargets.Add(end);
        }

        protected override void OnStarted()
        {
            startTranslations.Clear();
            deltaTranslations.Clear();
            startRotations.Clear();
            deltaRotations.Clear();
            for (int i = 0; i < Translations.Count; i++)
            {
                TranslateTransform3D t = Translations[i];
                Vector3D end = TranslationTargets[i];

                Vector3D start = new Vector3D(t.OffsetX, t.OffsetY, t.OffsetZ);
                startTranslations.Add(start);
                deltaTranslations.Add(IsAbsolute ? end - start : end);
            }
            for (int i = 0; i < Rotations.Count; i++)
            {
                AxisAngleRotation3D r = Rotations[i];
                double end = RotationTargets[i];

                startRotations.Add(r.Angle);
                deltaRotations.Add(IsAbsolute ? end - r.Angle : end);
            }
            base.OnStarted();
        }

        protected override void OnUpdated()
        {
            for (int i = 0; i < Translations.Count; i++)
            {
                TranslateTransform3D t = Translations[i];
                Vector3D r = startTranslations[i] + deltaTranslations[i] * Progress;
                t.OffsetX = r.X;
                t.OffsetY = r.Y;
                t.OffsetZ = r.Z;
            }
            for (int i = 0; i < Rotations.Count; i++)
            {
                Rotations[i].Angle = startRotations[i] + deltaRotations[i] * Progress;
            }
            base.OnUpdated();
        }
    }

}
