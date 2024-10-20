using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal class OCSMoreItemsToSpecificStorageFish : OCSMoreItemsToSpecificStorageBase
    {
        public override string PatcherName => "Add more fish products for fish products storages";

        protected override string[] LimitInventoryItemIdsToCheck => new string[2] 
        {
            "98578-ModPack.mod", // сырая рыба
            "50518-Newwworld.mod" // вяленая рыба
        };
        protected override bool IsValidItemSpecific(ModItem item)
        {
            return IconHaveFishName(item) || _validNameKeywords.Any(i => item.Name.Contains(i, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool IconHaveFishName(ModItem item)
        {
            if (!item.Values.TryGetValue("icon", out var value)) return false;
            if (value is not string v || !(_validNameKeywords.Any(i => v.Contains(i, StringComparison.InvariantCultureIgnoreCase)))) return false;

            return true;
        }

        readonly List<string> _validNameKeywords = new List<string>()
        {
            "fish",
            "рыба",
            "рыбища",
            "рыбёшка",
        };

        protected override int ItemFunctionIdToAdd => (int)Data.Enums.ItemFunction.ITEM_FOOD;
    }
}
