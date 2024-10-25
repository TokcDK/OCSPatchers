using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems
{
    internal class OSCPReplicaItemsOfSpecificNPC2B : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "2B_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[2] { "14-2B.mod", "75-2B.mod" };
    }
}
