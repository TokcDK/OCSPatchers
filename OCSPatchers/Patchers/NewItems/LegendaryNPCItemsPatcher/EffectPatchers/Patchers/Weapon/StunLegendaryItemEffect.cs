using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class StunLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Оглушение";

        public override string Description => "#afa68bОглушающий урон #a8b774+30%";

        protected override string KEY_NAME => "blunt damage multiplier";

        protected override float VALUE => (float)0.3;
    }
}
