using JoyfulColours.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Loop : Procedure
    {
        public Procedure Procedure { get; }

        public Loop(Procedure pro)
        {
            Procedure = pro;
        }

        protected override void OnStarted()
        {
            Event.Once(Procedure, Completed, (sender, e) =>
            {
                if (!IsStopping)
                    Procedure.Start();
                else
                    Complete();
            });
            Procedure.Start();
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            base.OnStopping();
            Procedure.Stop();
        }
    }
}
