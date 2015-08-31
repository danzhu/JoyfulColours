using JoyfulColours.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Sequence : Procedure
    {
        public Procedure Current => procedures[position];

        protected IList<Procedure> procedures;

        protected int position;
        public int Position => position;

        public int StopIndex { get; set; }

        public Sequence(IList<Procedure> pros, int stopIndex = -1)
        {
            procedures = pros;
            StopIndex = stopIndex;
        }
        
        private void Advance(object sender, LogicEventArgs e)
        {
            if (IsStopping && position == StopIndex || position == procedures.Count - 1)
            {
                Complete();
                return;
            }
            Procedure pro = procedures[++position];
            Event.Once(pro, Completed, Advance);
            pro.Start();
            if (IsStopping)
                pro.Stop();
        }

        protected override void OnStarted()
        {
            position = 0;
            Procedure pro = procedures[0];
            Event.Once(pro, Completed, Advance);
            procedures[position = 0].Start();
            base.OnStarted();
        }

        protected override void OnSkipped()
        {
            for (int i = position; i < procedures.Count; i++)
                procedures[i].Skip();
            base.OnSkipped();
        }

        protected override void OnStopping()
        {
            base.OnStopping();
            Current.Stop();
        }
    }
}
