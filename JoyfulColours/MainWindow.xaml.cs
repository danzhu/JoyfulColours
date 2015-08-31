using JoyfulColours.Elements;
using JoyfulColours.Logic;
using JoyfulColours.Procedures;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JoyfulColours
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ScriptScope scope;

        bool consoleMode;

        public MainWindow()
        {
            InitializeComponent();

            Game.Initialize(this);
            scope = Game.Engine.CreateScope();
            Game.Engine.Execute("from JoyfulColours import *", scope);
            Game.Engine.Execute("from JoyfulColours.Animations import *", scope);
            Game.Engine.Execute("from JoyfulColours.Procedures import *", scope);
            Game.Engine.Execute("from JoyfulColours.Elements import *", scope);
            Game.Engine.Execute("from JoyfulColours.Interface import *", scope);
            Game.Engine.Execute("from JoyfulColours.Library import *", scope);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                // Switch console open state
                if (!consoleMode)
                {
                    console.Visibility = Visibility.Visible;
                    input.Focus(); // TODO: Focus doesn't work the first time
                }
                else
                {
                    console.Visibility = Visibility.Collapsed;
                }
                consoleMode = !consoleMode;
            }
            else
            {
                if (!consoleMode)
                    Control.SendKeyDown(e);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (!consoleMode)
                Control.SendKeyUp(e);
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string script = input.Text;
                input.Clear();
                Print(script);
                try
                {
                    object result = Game.Engine.Execute(script, scope);
                    if (result != null)
                        Print(result.ToString());
                }
                catch (Exception ex)
                {
                    Print(ex.Message);
                }
                console.ScrollToBottom();
            }
        }

        private void Print(string text)
        {
            TextBlock line = new TextBlock();
            line.Text = text;
            output.Children.Add(line);
        }

        private void viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Select or deselect object in console mode
            Point pos = e.GetPosition(viewport);
            PointHitTestParameters param = new PointHitTestParameters(pos);
            VisualTreeHelper.HitTest(viewport, null, SelectModel, param);

            Control.SendMouseDown(e);
        }

        private HitTestResultBehavior SelectModel(HitTestResult result)
        {
            RayHitTestResult res = result as RayHitTestResult;
            if (res != null)
            {
                DependencyObject visual = res.VisualHit;
                object model = null, scene = null;
                while (visual != null)
                {
                    if (consoleMode)
                    {
                        if (visual is Model)
                            scope.SetVariable("model", model = visual);
                        else if (visual is Scene)
                            scope.SetVariable("scene", scene = visual);
                    }
                    else
                        Control.SendClick(visual, result);
                    visual = VisualTreeHelper.GetParent(visual);
                }
                selection.Text = $"Selection: {model} in {scene}";
            }

            return HitTestResultBehavior.Stop;
        }
    }
}
