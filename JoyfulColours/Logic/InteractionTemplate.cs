using JoyfulColours.Library;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Logic
{
    public class InteractionTemplate
    {
        public string Name { get; set; }

        public CompiledCode Code { get; set; }

        public InteractionTemplate(Loader l)
        {
            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        protected virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "name":
                    Name = i.String();
                    break;
                case "script":
                    Code = l.Find(i.String()).Load<CompiledCode>();
                    break;
                default:
                    break;
            }
        }
    }
}
