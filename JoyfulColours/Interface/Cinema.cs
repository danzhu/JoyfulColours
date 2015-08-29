using JoyfulColours.Animations;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JoyfulColours.Interface
{
    public static class Cinema
    {
        static Grid grid;
        static Viewport3D viewport;
        static Grid fade;
        static TextBlock subtitle;

        static BrushConverter brush = new BrushConverter();

        // TODO: Separate logic for Window
        public static void Initialize(MainWindow window)
        {
            grid = window.grid;
            viewport = window.viewport;
            fade = window.fade;
            subtitle = window.subtitle;
        }

        public static FadeAnimation Fade(double duration, string color = "Black")
        {
            FadeAnimation anim = new FadeAnimation(fade);
            anim.Duration = duration;
            anim.Opacity = 1;
            anim.Started += (sender, e) =>
            {
                fade.Background = brush.ConvertFromString(color) as SolidColorBrush;
            };
            return anim;
        }

        public static FadeAnimation Restore(double duration)
        {
            FadeAnimation anim = new FadeAnimation(fade);
            anim.Duration = duration;
            anim.Opacity = 0;
            return anim;
        }

        public static Animation Subtitle(string text, double duration)
        {
            Animation anim = new Animation(duration);
            anim.Started += (sender, e) =>
            {
                subtitle.Visibility = Visibility.Visible;
                subtitle.Text = text;
            };
            anim.Completed += (sender, e) => subtitle.Visibility = Visibility.Collapsed;
            return anim;
        }

        public static void Notify(string message)
        {
            // TODO: Notify player
        }
        
        public static void AddUI(UI ui)
        {
            grid.Children.Add(ui.Visual);
        }
    }
}
