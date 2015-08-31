using JoyfulColours.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JoyfulColours.Procedures
{
    /// <summary>
    /// Represents a general asynchronous action.
    /// </summary>
    public class Procedure
    {
        bool started;
        /// <summary>
        /// Gets a value that indicates whether the <see cref="Procedure"/> has started.
        /// </summary>
        public bool IsStarted => started;

        bool stopping;
        /// <summary>
        /// Gets a value that indicates whether the <see cref="Procedure"/> is in the process
        /// of trying to stop.
        /// </summary>
        public bool IsStopping => stopping;
        
        /// <summary>
        /// Occurs when the <see cref="Procedure"/> is started.
        /// </summary>
        public const string Started = "started";

        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        protected virtual void OnStarted()
        {
            Event.Raise(this, Started);
        }

        /// <summary>
        /// Start the current <see cref="Procedure"/>.
        /// </summary>
        public void Start()
        {
            if (started)
                return;
            started = true;
            stopping = false;
            OnStarted();
        }

        /// <summary>
        /// Occurs when the <see cref="Procedure"/> has completed.
        /// </summary>
        public const string Completed = "completed";

        /// <summary>
        /// Raises the <see cref="Completed"/> event.
        /// </summary>
        protected virtual void OnCompleted()
        {
            Event.Raise(this, Completed);
        }

        /// <summary>
        /// Marks this <see cref="Procedure"/> as completed and raises the
        /// <see cref="Completed"/> event.
        /// </summary>
        public void Complete()
        {
            if (!started)
                return;
            started = false;
            OnCompleted();
        }

        /// <summary>
        /// Occurs when the <see cref="Procedure"/> is skipped.
        /// </summary>
        public const string Skipped = "skipped";

        /// <summary>
        /// Raises the <see cref="Skipped"/> event.
        /// </summary>
        protected virtual void OnSkipped()
        {
            Event.Raise(this, Skipped);
        }

        /// <summary>
        /// Notify the <see cref="Procedure"/> to finish all its actions immediately, then
        /// complete the <see cref="Procedure"/>
        /// </summary>
        public void Skip()
        {
            if (!started)
                return;
            OnSkipped();
            Complete();
        }

        /// <summary>
        /// Occurs when the <see cref="Procedure"/> is notified to stop.
        /// </summary>
        public const string Stopping = "stopping";

        /// <summary>
        /// Raises the <see cref="Stopping"/> event.
        /// </summary>
        protected virtual void OnStopping()
        {
            Event.Raise(this, Stopping);
        }
        
        /// <summary>
        /// Notify the <see cref="Procedure"/> to stop all actions and complete as soon as
        /// possible.
        /// </summary>
        public void Stop()
        {
            if (!started)
                return;
            stopping = true;
            OnStopping();
        }
    }
}
