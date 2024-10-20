using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal class OCSMoreItemsToSpecificStorageFish1 : OCSMoreItemsToSpecificStorageFish
    {
        public override string PatcherName => "Add more raw fish for raw fish storages";

        protected override string[] LimitInventoryItemIdsToCheck => new string[2] 
        {
            "50514-Newwworld.mod", // рыбища
            "50517-Newwworld.mod" // рыбешка
        };

        protected override bool IsValidItemSpecific(ModItem item)
        {
            return HaveNoIngredients(item) && !_meatNameKeywords.Any(i => item.Name.Contains(i, StringComparison.InvariantCultureIgnoreCase)) && base.IsValidItemSpecific(item);
        }
        readonly List<string> _meatNameKeywords = new List<string>()
        {
            "meat",
            "мясо",
            "мясн",
        };

        private bool HaveNoIngredients(ModItem item)
        {
            return !item.ReferenceCategories.ContainsKey("ingredients");
        }
    }
}
