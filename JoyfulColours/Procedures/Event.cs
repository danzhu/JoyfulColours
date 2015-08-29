using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Event : Procedure
    {
        public Action Action { get; }

        public Event(Action action = null)
        {
            Action = action;
        }

        protected override void OnStarted(EventArgs e)
        {
            base.OnStarted(e);
            Action?.Invoke();
            Complete();
        }
    }

    public class Event<T> : Procedure
    {
        public Action<T> Action { get; }
        public T Data { get; }

        public Event(Action<T> action, T data)
        {
            Action = action;
            Data = data;
        }

        protected override void OnStarted(EventArgs e)
        {
            base.OnStarted(e);
            Action(Data);
            Complete();
        }
    }
}
