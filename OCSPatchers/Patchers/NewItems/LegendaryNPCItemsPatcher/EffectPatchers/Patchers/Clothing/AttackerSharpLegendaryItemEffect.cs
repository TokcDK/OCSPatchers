using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class AttackerSharpLegendaryItemEffect : LegendaryItemEffectItemIntegerBase
    {
        public override string Name => "Атакер";

        public override string Description => "#afa68bАтака #a8b774+10";

        protected override string KEY_NAME => "combat attk bonus";

        protected override int VALUE => 10;
    }
}
