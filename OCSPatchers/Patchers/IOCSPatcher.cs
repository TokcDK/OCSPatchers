using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    public interface IOCSPatcher
    {
        string PatchFileNameWithoutExtension { get; }
        string PatcherName { get; }
        string[] ReferenceModNames { get; }
        string[] ExcludedModNames { get; }

        Task ApplyPatch(IModContext context, IInstallation installation);
    }
}
