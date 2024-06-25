using OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers;
using OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

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

            protected override void Clear(ModReferenceCollection itemRefs)
            {
                // dont need to clear default clothing to reduce chance of other clothings to appear
            }

            protected override int Set2(int value1)
            {
                return 5; // 5% chance to appear of legendary clothing
            }
            protected override ILegendaryItemEffect[] EffectPatchers { get; } = new ILegendaryItemEffect[]
            {
                // clothing
                new AttackerSharpLegendaryItemEffect(),
                new DefencerSharpLegendaryItemEffect(),
                new RunnerLegendaryItemEffect(),
                new RangerLegendaryItemEffect(),
                new QualityLegendaryItemEffect(),
                new BluntDefenceLegendaryItemEffect(),
                new CutDefenceLegendaryItemEffect(),
                new DexterityLegendaryItemEffect(),
                new CombatSpeedLegendaryItemEffect(),
                new ExtraCoverageLegendaryItemEffect(),
                new UltraComfortabilityLegendaryItemEffect(),
            };
        }
    }
}
