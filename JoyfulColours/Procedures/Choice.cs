using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Procedures
{
    public class Choice : Procedure
    {
        public IList<Procedure> Consequences { get; }

        public int Result { get; set; } = -1;

        public Choice(IList<Procedure> cons)
        {
            Consequences = cons;
        }

        protected override void OnCompleted()
        {
            if (Result >= 0 && Result < Consequences?.Count)
                Consequences[Result].Start();
            base.OnCompleted();
        }

        public void Complete(int result)
        {
            Result = result;
            Complete();
        }
    }
}
