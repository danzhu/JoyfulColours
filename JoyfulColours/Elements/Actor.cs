using JoyfulColours.Animations;
using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    // TODO: Add AI (though it should be a long time after)
    public class Actor : Model
    {
        public Movement Movement { get; set; }

        public Actor(ActorTemplate template) : base(template) { }
        
        public override string ToString()
            => $"{nameof(Actor)} \"{Template.ID}\" ({Template.Name})";
    }
}
