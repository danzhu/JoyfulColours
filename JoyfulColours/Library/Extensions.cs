using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Library
{
    public static class Extensions
    {
        public static ScriptScope Load(this CompiledCode code, string id, object val)
        {
            if (code == null)
                return null;
            ScriptScope scope = Game.Engine.CreateScope();
            scope.SetVariable(id, val);
            code.Execute(scope);
            return scope;
        }
    }
}
