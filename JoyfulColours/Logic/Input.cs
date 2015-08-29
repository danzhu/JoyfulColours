using JoyfulColours.Interface;
using JoyfulColours.Library;
using JoyfulColours.Procedures;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JoyfulColours.Logic
{
    public static class Input
    {
        static Dictionary<Key, Procedure> bindings = new Dictionary<Key, Procedure>();

        public static void SendKeyDown(KeyEventArgs e)
        {
            if (bindings.ContainsKey(e.Key))
                bindings[e.Key].Start();
        }

        public static void SendKeyUp(KeyEventArgs e)
        {
            if (bindings.ContainsKey(e.Key))
                bindings[e.Key].Stop();
        }

        public static void Register(Control ctrl)
        {
            foreach (var item in ctrl.Keys)
            {
                if (bindings.ContainsKey(item.Key))
                    Cinema.Notify($"{ctrl} overrides key \"{item.Key}\"");
                bindings[item.Key] = item.Value;
            }
        }

        public static void Unregister(Control ctrl)
        {
            foreach (Key key in ctrl.Keys.Keys)
                bindings.Remove(key);
        }
    }
}
