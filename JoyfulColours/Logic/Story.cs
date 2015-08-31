using JoyfulColours.Library;
using JoyfulColours.Logic;
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
        
        public bool StartOnce { get; set; }
        
        public Story(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        public virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "startup":
                    if (i.Bool())
                        Event.Once(typeof(Game), Game.Loaded, (sender, e) => Start());
                    break;
                case "once": // TODO: Add flags for game saves (though far, far away...)
                    StartOnce = i.Bool();
                    break;
                default:
                    break;
            }
        }

        protected override void OnStarted()
        {
            if (StartOnce && IsCompleted)
                return;
            base.OnStarted();
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
        public void Link(Procedure pro, LogicEventHandler started = null,
            LogicEventHandler completed = null)
        {
            Event.Register(this, Started, (sender, e) => pro.Start());
            Event.Register(this, Skipped, (sender, e) => pro.Skip());
            Event.Register(this, Stopping, (sender, e) => pro.Stop());
            Event.Register(pro, Completed, (sender, e) => Complete());
            if (started != null)
                Event.Register(this, Started, started);
            if (completed != null)
                Event.Register(this, Completed, completed);
        }

        public override string ToString() => $"{nameof(Story)} \"{ID}\"";
    }
}
