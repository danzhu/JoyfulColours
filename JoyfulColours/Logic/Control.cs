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
        public bool Startup { get; set; }

        public Dictionary<Key, Procedure> Keys { get; } = new Dictionary<Key, Procedure>();
        
        public Control()
        {

        }
        
        public void RegisterKey(string key, Procedure pro)
        {
            Key k;
            Enum.TryParse(key, out k);
            Keys.Add(k, pro);
        }

        protected override void OnStarted()
        {
            Input.Register(this);
            base.OnStarted();
        }

        protected override void OnCompleted()
        {
            Input.Unregister(this);
            base.OnCompleted();
        }
    }
}
