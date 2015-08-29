using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Library
{
    public delegate double Easing(double input);

    public static class Easings
    {
        public static Dictionary<string, Easing> Map { get; }
            = new Dictionary<string, Easing>();

        public static void Initialize()
        {
            Map.Add("none", Linear);
            Map.Add("in", EaseIn);
            Map.Add("in2", EaseIn2);
            Map.Add("in3", EaseIn3);
            Map.Add("out", EaseOut);
            Map.Add("out2", EaseOut2);
            Map.Add("out3", EaseOut3);
            Map.Add("in_out", EaseInOut);
            Map.Add("in_out2", EaseInOut2);
            Map.Add("in_out3", EaseInOut3);
            Map.Add("in_circular", EaseInCircular);
            Map.Add("out_circular", EaseOutCircular);
            Map.Add("in_out_circular", EaseInOutCircular);
        }

        public static double Linear(double x)
        {
            return x;
        }

        public static double EaseIn(double x)
        {
            return x * x;
        }

        public static double EaseIn2(double x)
        {
            return Math.Pow(x, 3);
        }

        public static double EaseIn3(double x)
        {
            return Math.Pow(x, 8);
        }

        public static double EaseOut(double x)
        {
            double t = 1 - x;
            return 1 - t * t;
        }

        public static double EaseOut2(double x)
        {
            return 1 - Math.Pow(1 - x, 3);
        }

        public static double EaseOut3(double x)
        {
            return 1 - Math.Pow(1 - x, 8);
        }

        public static double EaseInOut(double x)
        {
            return x < 0.5 ? 2 * x * x : -2 * x * x + 4 * x - 1;
        }

        public static double EaseInOut2(double x)
        {
            return (x < 0.5) ? 4 * Math.Pow(x, 3) :
                4 * Math.Pow(x, 3) - 12 * x * x + 12 * x - 3;
        }

        public static double EaseInOut3(double x)
        {
            return (x < 0.5) ? 128 * Math.Pow(x, 8) : 0.5 + (1 - Math.Pow(2 * (1 - x), 8)) / 2;
        }

        public static double EaseInCircular(double x)
        {
            return 1 - Math.Sqrt(1 - x * x);
        }

        public static double EaseOutCircular(double x)
        {
            return Math.Sqrt(-(x - 2) * x);
        }

        public static double EaseInOutCircular(double x)
        {
            return (x < 0.5) ? 0.5 * (1 - Math.Sqrt(1 - 4 * x * x)) :
                0.5 * (Math.Sqrt(-4 * (x - 2) * x - 3) + 1);
        }
    }
}
