using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class SharpLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Острота";

        public override string Description => "#afa68bРежущий урон #a8b774+20%";

        protected override string KEY_NAME => "cut damage multiplier";

        protected override float VALUE => (float)0.2;
    }
}
