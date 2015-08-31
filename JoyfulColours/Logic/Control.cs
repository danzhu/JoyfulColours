using JoyfulColours.Interface;
using JoyfulColours.Library;
using JoyfulColours.Logic;
using JoyfulColours.Procedures;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JoyfulColours
{
    public class Control : Procedure
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

        public string Name { get; set; }

        public Dictionary<Key, Procedure> Keys { get; } = new Dictionary<Key, Procedure>();

        public Control(Loader l)
        {
            foreach (Instruction i in l.Parse())
            {
                switch (i.Type)
                {
                    case "name":
                        Name = i.String();
                        break;
                    case "startup":
                        if (i.Bool())
                            Event.Once(typeof(Game), Game.Loaded, (sender, e) => Start());
                        break;
                }
            }
        }

        public void RegisterKey(string key, Procedure pro)
        {
            Key k;
            Enum.TryParse(key, out k);
            Keys.Add(k, pro);
        }

        protected override void OnStarted()
        {
            foreach (var item in Keys)
            {
                if (bindings.ContainsKey(item.Key))
                    Cinema.Notify($"{this} overrides key \"{item.Key}\"");
                bindings[item.Key] = item.Value;
            }
            base.OnStarted();
        }

        protected override void OnCompleted()
        {
            foreach (Key key in Keys.Keys)
                bindings.Remove(key);
            base.OnCompleted();
        }
    }
}
