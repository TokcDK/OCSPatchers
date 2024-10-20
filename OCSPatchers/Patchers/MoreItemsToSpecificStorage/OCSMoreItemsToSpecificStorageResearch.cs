using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal class OCSMoreItemsToSpecificStorageResearch : OCSMoreItemsToSpecificStorageBase
    {
        public override string PatcherName => "Add more research items for research items storages";

        protected override string[] LimitInventoryItemIdsToCheck => new string[2] 
        {
            "43951-rebirth.mod",
            "43953-rebirth.mod"
        };

        protected override int ItemFunctionIdToAdd => (int)Data.Enums.ItemFunction.ITEM_BLUEPRINT;

        protected override bool IsValidItemFunction(ModItem item)
        {
            return base.IsValidItemFunction(item) || IsValidIntValue(item, "inventory sound", (int)Data.Enums.InventorySound.BLUEPRINTS); //|| IsValidBoolValue(item, "artifact", true);
        }

        protected override bool IsValidInventorySound(ModItem item) => true;
    }
}
