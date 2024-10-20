using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal class OCSMoreItemsToSpecificStorageFood : OCSMoreItemsToSpecificStorageBase
    {
        public override string PatcherName => "Add more food for food storages";

        protected override string[] LimitInventoryItemIdsToCheck => new string[2] 
        {
            "43961-rebirth.mod",
            "50514-Newwworld.mod"
        };

        protected override int ItemFunctionIdToAdd => (int)Data.Enums.ItemFunction.ITEM_FOOD;
    }
}
