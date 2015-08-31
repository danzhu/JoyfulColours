using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JoyfulColours.Animations
{
    public class FadeAnimation : Animation
    {
        public UIElement Element { get; }

        double startOpacity;
        double deltaOpacity;
        public double Opacity { get; set; }

        public FadeAnimation(UIElement element)
        {
            Element = element;
        }

        protected override void OnStarted()
        {
            startOpacity = Element.Opacity;
            deltaOpacity = Opacity - startOpacity;
            base.OnStarted();
        }

        protected override void OnUpdated()
        {
            Element.Opacity = startOpacity + deltaOpacity * Progress;
            base.OnUpdated();
        }
    }
}
