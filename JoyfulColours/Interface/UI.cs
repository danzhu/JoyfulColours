using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace JoyfulColours.Interface
{
    public class UI
    {
        public UITemplate Template { get; }

        public FrameworkElement Visual { get; }

        public UI(UITemplate template)
        {
            Template = template;

            Visual = XamlReader.Parse(template.Xaml) as FrameworkElement;
        }
        
        private void InitializeScript(ScriptScope scope)
        {
            scope.SetVariable("ui", Visual);
            InjectElements(scope, Visual);
        }

        private void InjectElements(ScriptScope scope, FrameworkElement e)
        {
            string name = e.GetValue(FrameworkElement.NameProperty) as string;
            if (name != null)
                scope.SetVariable(name, e);

            foreach (object obj in LogicalTreeHelper.GetChildren(e))
            {
                FrameworkElement element = obj as FrameworkElement;
                if (element != null)
                    InjectElements(scope, element);
            }
        }
    }
}
