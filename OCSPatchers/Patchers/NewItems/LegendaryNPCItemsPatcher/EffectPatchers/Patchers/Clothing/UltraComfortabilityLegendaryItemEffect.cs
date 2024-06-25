using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class UltraComfortabilityLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Сверхудобство";

        public override string Description => $"#afa68bУворот #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "dodge mult";

        protected override float VALUE => (float)0.5;
    }
}
