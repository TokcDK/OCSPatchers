using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class QualityLegendaryItemEffect : LegendaryItemEffectItemIntegerBase
    {
        public override string Name => "Качество";

        public override string Description => $"#afa68bКачество #a8b774+{VALUE}%";

        protected override string KEY_NAME => "level bonus";

        protected override int VALUE => 50;
    }
}
