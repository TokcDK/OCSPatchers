using OpenConstructionSet;
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

        protected override int ItemFunctionNumToAdd => 0;

        protected override bool IsValidItemSpecific(ModItem item)
        {
            return _validNameKeywords.Any(i => item.Name.Contains(i));
        }

        List<string> _validNameKeywords = new List<string>()
        {
            "CPU",
            "ЦПУ",
            "роцессор",
        };
    }
}
