using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Judgement : Procedure
    {
        public Func<bool> Condition { get; }

        public bool Result { get; set; }
        public Procedure True { get; }
        public Procedure False { get; }

        public Judgement(Func<bool> cond, Procedure proTrue, Procedure proFalse = null)
        {
            Condition = cond;
            True = proTrue;
            False = proFalse;

            True.Completed += (sender, e) => Complete();
            if (False != null)
                False.Completed += (sender, e) => Complete();
        }

        protected override void OnStarted(EventArgs e)
        {
            if (Result = Condition())
                True.Start();
            else if (False != null)
                False.Start();
            else
                Complete();
            base.OnStarted(e);
        }

        protected override void OnStopping(EventArgs e)
        {
            base.OnStopping(e);
            if (Result)
                True.Stop();
            else if (False != null)
                False.Stop();
            else
                Complete();
        }
    }
}
