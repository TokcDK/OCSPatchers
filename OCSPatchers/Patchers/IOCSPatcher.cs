using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    public interface IOCSPatcher
    {
        string PatcherName { get; }
        string[] ReferenceModNames { get; }
        string[] ExcludedModNames { get; }

        void ApplyPatch(IModContext context);
    }
}
