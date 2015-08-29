using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Elements
{
    public class AddonTemplate
    {
        public string ID { get; }

        public string Node { get; set; }

        public ModelObject Model { get; set; }

        public AddonTemplate(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
            {
                switch (i.Type)
                {
                    case "node":
                        Node = i.String();
                        break;
                    case "model":
                        Model = l.Find(i.String()).Load<ModelObject>();
                        break;
                }
            }
        }
    }
}
