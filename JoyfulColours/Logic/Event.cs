using JoyfulColours.Interface;
using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Logic
{
    public static class Event
    {
        static Dictionary<object, Dictionary<string, LogicHandler>> units
            = new Dictionary<object, Dictionary<string, LogicHandler>>();

        /// <summary>
        /// Raises an event on the <see cref="object"/> for the engine and scripts to handle.
        /// </summary>
        /// <param name="target">The <see cref="object"/> to raise event against.</param>
        /// <param name="name">The name of the event.</param>
        /// <param name="args">The arguments for the event.</param>
        /// <returns>Result of event execution.</returns>
        public static object Raise(object target, string name, params object[] args)
        {
            object result = Raise(target.GetType(), name, args);
            if (result != null)
                return result;
            
            return units.GetOrDefault(target)?.GetOrDefault(name)?.Handle(target, name, args);
        }

        public static object Raise(Type type, string name, params object[] args)
        {
            return units.GetOrDefault(type)?.GetOrDefault(name)?.Handle(type, name, args);
        }

        public static void Register(object target, string name, LogicEventHandler handler)
        {
            units.GetOrNew(target).GetOrNew(name).Register(handler);
        }

        public static void Register(Type type, string name, LogicEventHandler handler)
        {
            Register((object)type, name, handler);
        }

        public static void Once(object target, string name, LogicEventHandler handler)
        {
            units.GetOrNew(target).GetOrNew(name).Once(handler);
        }

        public static void Once(Type type, string name, LogicEventHandler handler)
        {
            Once((object)type, name, handler);
        }

        public static void Override(object target, string name, LogicEventHandler handler)
        {
            units.GetOrNew(target).GetOrNew(name).Override(handler);
        }

        public static LogicEventHandler Chain(object target)
        {
            return (sender, e) => Raise(target, e.Name, e.Args);
        }

        public static LogicEventHandler Chain(object target, string name)
        {
            return (sender, e) => Raise(target, name, e.Args);
        }

        public static LogicEventHandler Chain(object target, string name, params object[] args)
        {
            return (sender, e) => Raise(target, name, args);
        }
    }
    
    public class LogicHandler
    {
        List<LogicEventHandler> handlers = new List<LogicEventHandler>();
        HashSet<LogicEventHandler> onces = new HashSet<LogicEventHandler>();

        public object Handle(object target, string name, object[] args)
        {
            if (onces.Count == 0 && handlers.Count == 0)
                return null;
            LogicEventArgs e = new LogicEventArgs(name, args);
            
            foreach (LogicEventHandler handler in handlers)
            {
                try
                {
                    handler(target, e);
                }
                catch (Exception ex)
                {
                    Cinema.Notify($"Script error: {ex}");
                    throw; // TODO: Remove this in final production
                }

                if (e.IsHandled)
                    break;
            }

            foreach (LogicEventHandler handler in onces)
                handlers.Remove(handler);
            onces.Clear();

            return e.Result;
        }

        public void Register(LogicEventHandler handler)
        {
            handlers.Add(handler);
        }

        public void Once(LogicEventHandler handler)
        {
            handlers.Add(handler);
            onces.Add(handler);
        }

        public void Override(LogicEventHandler handler)
        {
            handlers.Clear();
            handlers.Add(handler);
        }
    }

    public class LogicEventArgs : EventArgs
    {
        public string Name { get; }
        public object[] Args { get; }
        public object Result { get; set; }
        public bool IsHandled { get; set; }

        public LogicEventArgs(string name, object[] args)
        {
            Name = name;
            Args = args;
        }
    }

    public delegate void LogicEventHandler(object sender, LogicEventArgs e);
}
