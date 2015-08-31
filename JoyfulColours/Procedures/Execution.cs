using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Execution : Procedure
    {
        public Action Action { get; }

        public Execution(Action action = null)
        {
            Action = action;
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            Action?.Invoke();
            Complete();
        }
    }

    public class Execution<T> : Procedure
    {
        public Action<T> Action { get; }
        public T Data { get; }

        public Execution(Action<T> action, T data)
        {
            Action = action;
            Data = data;
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            Action(Data);
            Complete();
        }
    }
}
