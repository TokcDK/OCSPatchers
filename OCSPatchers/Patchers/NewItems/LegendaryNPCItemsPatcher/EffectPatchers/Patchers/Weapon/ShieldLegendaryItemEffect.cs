using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class ShieldLegendaryItemEffect : LegendaryItemEffectItemIntegerBase
    {
        public override string Name => "Щит";

        public override string Description => "#afa68bЗащита #a8b774+20";

        protected override int VALUE => 20;

        protected override string KEY_NAME => "defence mod";
    }
}
