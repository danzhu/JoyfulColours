using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Elements
{
    public class Equipment
    {
        public EquipmentTemplate Template { get; }

        public List<Addon> Addons { get; } = new List<Addon>();

        public Model Parent { get; internal set; }

        public Equipment(EquipmentTemplate template)
        {
            Template = template;

            foreach (AddonTemplate at in template.Addons)
            {
                Addons.Add(new Addon(at));
            }
        }
    }
}
