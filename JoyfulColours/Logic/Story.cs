using JoyfulColours.Library;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Story : Procedure
    {
        public string ID { get; }

        /// <summary>
        /// Represents if the story has been completed at least once.
        /// </summary>
        public bool IsCompleted { get; }

        public bool Startup { get; set; }
        public bool StartOnce { get; set; }

        public CompiledCode Code { get; set; }
        public ScriptScope Script { get; set; }

        public Story(Loader l)
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
                    Script.SetVariable("story", this);
                    Code.Execute(Script);
                    break;
                case "startup":
                    Startup = i.Bool();
                    break;
                case "once": // TODO: Add flags for game saves (though far, far away...)
                    StartOnce = i.Bool();
                    break;
                default:
                    break;
            }
        }

        protected override void OnStarted(EventArgs e)
        {
            if (StartOnce && IsCompleted)
                return;
            base.OnStarted(e);
        }

        /// <summary>
        /// Convenient method for linking the events of this <see cref="Story"/> to another
        /// <see cref="Procedure"/>. Also optionally add event handlers to
        /// <see cref="Procedure.Started"/> and <see cref="Procedure.Completed"/>.
        /// </summary>
        /// <param name="pro">The <see cref="Procedure"/> to link to.</param>
        /// <param name="started">Event handler for <see cref="Procedure.Started"/>.</param>
        /// <param name="completed">
        /// Event handler for <see cref="Procedure.Completed"/>.
        /// </param>
        public void Link(Procedure pro, EventHandler started = null,
            EventHandler completed = null)
        {
            Started += (sender, e) => pro.Start();
            Skipped += (sender, e) => pro.Skip();
            Stopping += (sender, e) => pro.Stop();
            pro.Completed += (sender, e) => Complete();
            if (started != null)
                Started += started;
            if (completed != null)
                Completed += completed;
        }

        public override string ToString() => $"{nameof(Story)} \"{ID}\"";
    }
}
