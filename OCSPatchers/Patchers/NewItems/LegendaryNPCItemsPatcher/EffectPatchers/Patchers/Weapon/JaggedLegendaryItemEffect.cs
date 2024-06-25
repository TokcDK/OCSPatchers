using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class JaggedLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Зазубрины";

        public override string Description => "#afa68bУрон от кровотечения #a8b774+50%";

        protected override string KEY_NAME => "bleed mult";

        protected override float VALUE => (float)0.5;
    }
}
