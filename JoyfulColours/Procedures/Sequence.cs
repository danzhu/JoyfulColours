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

            for (int i = 0; i < pros.Count - 1; i++)
                pros[i].Completed += Advance;
            pros.Last().Completed += (sender, e) => Complete();
        }
        
        private void Advance(object sender, EventArgs e)
        {
            if (!IsStarted)
                return;
            if (IsStopping && position == StopIndex)
            {
                Complete();
                return;
            }
            Procedure pro = procedures[++position];
            pro.Start();
            if (IsStopping)
                pro.Stop();
        }

        protected override void OnStarted(EventArgs e)
        {
            procedures[position = 0].Start();
            base.OnStarted(e);
        }

        protected override void OnSkipped(EventArgs e)
        {
            for (int i = position; i < procedures.Count; i++)
                procedures[i].Skip();
            base.OnSkipped(e);
        }

        protected override void OnStopping(EventArgs e)
        {
            base.OnStopping(e);
            Current.Stop();
        }
    }
}
