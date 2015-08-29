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

            pro.Completed += (sender, e) =>
            {
                if (!IsStopping)
                    pro.Start();
                else
                    Complete();
            };
        }

        protected override void OnStarted(EventArgs e)
        {
            Procedure.Start();
            base.OnStarted(e);
        }

        protected override void OnStopping(EventArgs e)
        {
            base.OnStopping(e);
            Procedure.Stop();
        }
    }
}
