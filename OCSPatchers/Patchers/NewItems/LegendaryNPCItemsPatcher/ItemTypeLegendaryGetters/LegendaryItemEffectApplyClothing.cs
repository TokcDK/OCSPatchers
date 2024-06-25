using OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters
{
    internal partial class OCSPLegendaryNPCItems
    {
        public class LegendaryItemEffectApplyClothing : LegendaryItemEffectApplyBase
        {
            public readonly Dictionary<string, List<ModItem>> _cacheOfAddedLegendaryClothingByOrigin = new();

            protected override string CategoryName => "clothing";

            protected override string GetPatchedItemescription(ILegendaryItemEffect effectPatcher) => $"#000000Эта часть одежды имеет легендарный эффект \"#ff0000{effectPatcher!.Name}#000000\", со следующими эффектами.\r\n{effectPatcher.Description}";

            protected override Dictionary<string, List<ModItem>> AddedItemsCache => _cacheOfAddedLegendaryClothingByOrigin;
        }
    }
}
