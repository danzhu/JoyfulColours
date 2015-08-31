using JoyfulColours.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Concurrence : Procedure
    {
        public bool WaitForAll { get; set; }

        IEnumerable<Procedure> procedures;
        HashSet<Procedure> incompleted;

        public Concurrence(IEnumerable<Procedure> pros, bool wait = true)
        {
            procedures = pros;
            WaitForAll = wait;
        }

        protected override void OnStarted()
        {
            incompleted = new HashSet<Procedure>(procedures);
            foreach (Procedure pro in procedures)
            {
                Event.Once(pro, Completed, (sender, ex) =>
                {
                    incompleted.Remove(pro);
                    if (incompleted.Count == 0 || !WaitForAll)
                        Complete();
                });
                pro.Start();
            }
            base.OnStarted();
        }

        protected override void OnSkipped()
        {
            foreach (Procedure pro in procedures)
                if (incompleted.Contains(pro))
                    pro.Skip();
            base.OnSkipped();
            Complete();
        }

        protected override void OnStopping()
        {
            base.OnStopping();
            foreach (Procedure pro in incompleted)
            {
                pro.Stop();
            }
        }
    }

}
