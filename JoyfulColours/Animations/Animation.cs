using JoyfulColours.Elements;
using JoyfulColours.Library;
using JoyfulColours.Procedures;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Linq;
using System.Windows.Threading;
using JoyfulColours.Logic;

namespace JoyfulColours.Animations
{
    public class Animation : Procedure
    {
        #region Static Members

        static int fps;
        public static int FPS
        {
            get { return fps; }
            set
            {
                timer.Interval = new TimeSpan(10000000 / value);
                fps = value;
            }
        }

        static double time = 0.0;
        public static double Time { get { return time; } }

        public static double TimeScale { get; set; } = 1.0;

        static List<Animation> animations = new List<Animation>();

        static DispatcherTimer timer = new DispatcherTimer();

        public static void Initialize()
        {
            FPS = 60;
            timer.Tick += UpdateTime;
            timer.Start();
        }
        
        public static void Pause()
        {
            timer.Stop();
        }

        public static void Resume()
        {
            timer.Start();
        }
        
        private static void UpdateTime(object sender, EventArgs e)
        {
            time += TimeScale / fps;
            
            List<Animation> copy = new List<Animation>(animations);

            copy.ForEach(anim => anim.Update());
        }

        #endregion

        double startTime;
        public double StartTime => startTime;

        public double Duration { get; set; } = -1.0;

        double progress;
        public double Progress => progress;
        
        public Easing Easing { get; set; } = Easings.Linear;

        /// <summary>
        /// Initialize an empty <see cref="Animation"/> that has no duration or action.
        /// </summary>
        public Animation() { }

        /// <summary>
        /// Initialize an empty <see cref="Animation"/> with specified duration.
        /// </summary>
        /// <param name="duration">Duration of the animation.</param>
        public Animation(double duration)
        {
            Duration = duration;
        }

        /// <summary>
        /// Prepare the current <see cref="Animation"/> to start.
        /// <para>
        /// Note that any previous animation on same properties of the same object will NOT be
        /// cleared. Those properties will only be overridden every frame in the duration of
        /// the newer animation.
        /// </para>
        /// </summary>
        protected override void OnStarted()
        {
            startTime = time;
            progress = 0;
            animations.Add(this);
            base.OnStarted();
        }
        
        /// <summary>
        /// Derived classes need to override this method to update their own animations.
        /// </summary>
        protected virtual void OnUpdated()
        {
            // Do nothing: individual animations update their own animation here
        }

        private void Update()
        {
            // TODO: Make progress optional
            double p = (time - startTime) / Duration;
            progress = Easing(Math.Min(p, 1.0));
            OnUpdated();
            if (p >= 1.0)
                Complete();
        }

        protected override void OnCompleted()
        {
            animations.Remove(this);
            base.OnCompleted();
        }

        protected override void OnSkipped()
        {
            // Set progress to done
            progress = 1.0;
            // Raise update event immediate to finish animation
            OnUpdated();

            base.OnSkipped();
        }
    }
}