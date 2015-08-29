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

            Started += Start;
            Updated += Update;
        }

        private void Start(object sender, EventArgs e)
        {
            startOpacity = Element.Opacity;
            deltaOpacity = Opacity - startOpacity;
        }

        private void Update(object sender, EventArgs e)
        {
            Element.Opacity = startOpacity + deltaOpacity * Progress;
        }
    }
}
