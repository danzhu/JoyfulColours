using JoyfulColours.Library;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Elements
{
    public class EquipmentTemplate
    {
        public string ID { get; set; }

        public List<AddonTemplate> Addons { get; } = new List<AddonTemplate>();
        
        public EquipmentTemplate(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        public virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "adn":
                    Addons.Add(l.Find(i.String()).Load<AddonTemplate>());
                    break;
            }
        }
    }
}
