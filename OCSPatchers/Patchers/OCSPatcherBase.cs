using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    public abstract class OCSPatcherBase : IOCSPatcher
    {
        public abstract string PatcherName { get; }
        public virtual string[] ReferenceModNames { get; } = Array.Empty<string>();
        public virtual string[] ExcludedModNames { get; } = Array.Empty<string>();

        public abstract Task ApplyPatch(IModContext context, IInstallation installation);
    }    
}
