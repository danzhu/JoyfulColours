using JoyfulColours.Elements;
using JoyfulColours.Library;
using JoyfulColours.Procedures;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Logic
{
    public class Interaction : Procedure
    {
        public InteractionTemplate Template { get; }

        public Model Sender { get; }
        public Model Receiver { get; }
        public object Data { get; set; }
        public object Result { get; set; }

        public ScriptScope Script { get; }

        public Interaction(InteractionTemplate template, Model sender, Model receiver = null)
        {
            Template = template;
            Sender = sender;
            Receiver = receiver;

            Script = template.Code.Load("interaction", this);
        }

        protected override void OnStarted(EventArgs e)
        {
            if (Receiver != null)
                Receiver.Interact(this);
            base.OnStarted(e);
        }
    }
}
