using JoyfulColours.Elements;
using JoyfulColours.Library;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace JoyfulColours.Interface
{
    public class UITemplate
    {
        public string ID { get; set; }
        public string Xaml { get; set; }
        
        public UITemplate(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        public virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "xaml":
                    Xaml = l.Find(i.String()).Read();
                    break;
            }
        }
    }
}
