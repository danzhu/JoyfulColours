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
        public string ID { get; }

        public bool Startup { get; set; }

        public Dictionary<Key, Procedure> Keys { get; } = new Dictionary<Key, Procedure>();

        public CompiledCode Code { get; set; }
        public ScriptScope Script { get; set; }

        public Control(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);

            if (Startup)
                Start();
        }

        public virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "script":
                    Code = l.Find(i.String()).Load<CompiledCode>();
                    Script = Game.Engine.CreateScope();
                    Script.SetVariable("control", this);
                    Code.Execute(Script);
                    break;
                case "startup":
                    Startup = i.Bool();
                    break;
            }
        }

        public void RegisterKey(string key, Procedure pro)
        {
            Key k;
            Enum.TryParse(key, out k);
            Keys.Add(k, pro);
        }

        protected override void OnStarted(EventArgs e)
        {
            Input.Register(this);
            base.OnStarted(e);
        }

        protected override void OnCompleted(EventArgs e)
        {
            Input.Unregister(this);
            base.OnCompleted(e);
        }

        public override string ToString() => $"{nameof(Control)} {ID}";
    }
}
