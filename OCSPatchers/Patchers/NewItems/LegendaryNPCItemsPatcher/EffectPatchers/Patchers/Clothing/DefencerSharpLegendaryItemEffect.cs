using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class DefencerSharpLegendaryItemEffect : LegendaryItemEffectItemIntegerBase
    {
        public override string Name => "Защитник";

        public override string Description => "#afa68bЗащита #a8b774+10";

        protected override string KEY_NAME => "combat def bonus";

        protected override int VALUE => 10;
    }
}
