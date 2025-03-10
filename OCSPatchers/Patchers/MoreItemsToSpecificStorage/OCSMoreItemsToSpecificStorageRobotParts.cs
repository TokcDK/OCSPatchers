﻿using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal class OCSMoreItemsToSpecificStorageRobotParts : OCSMoreItemsToSpecificStorageBase
    {
        public override string PatcherName => "Add more robot parts to robot parts storages";

        protected override string[] LimitInventoryItemIdsToCheck => new string[2] 
        {
            "45557-changes_otto.mod",
            "583-gamedata.base"
        };

        protected override int ItemFunctionIdToAdd => (int)Data.Enums.ItemFunction.ITEM_NO_FUNCTION;

        protected override int InventorySoundIdOptionalToAdd => (int)Data.Enums.InventorySound.ROBOTIC_COMPONENT;

        protected override bool IsValidItemSpecific(ModItem item)
        {
            return _validNameKeywords.Any(i => item.Name.Contains(i)) || _validIds.Contains(item.StringId) || _wasValidInventorySound;
        }

        readonly List<string> _validNameKeywords = new List<string>()
        {
            "CPU",
            "ЦПУ",
            "роцессор",
        };
        readonly List<string> _validIds = new List<string>()
        {
            "43398-changes_otto.mod",
        };
    }
}
