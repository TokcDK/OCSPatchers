using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal class OCSMoreItemsToSpecificStorageBolts : OCSMoreItemsToSpecificStorageBase
    {
        public override string PatcherName => "Add more bolts for bolts storages";

        protected override string[] LimitInventoryItemIdsToCheck => new string[2] 
        {
            "96014-rebirth.mod",
            "95872-Newwworld.mod"
        };

        protected override int ItemFunctionNumToAdd => 16;

        protected override string ValidItemContentName => "Bolts";
    }
}
