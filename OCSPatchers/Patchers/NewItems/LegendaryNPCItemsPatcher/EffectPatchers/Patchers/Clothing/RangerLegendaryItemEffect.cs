using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class RangerLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Стрелок";

        public override string Description => $"#afa68bСтрельба #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "ranged skill mult";

        protected override float VALUE => (float)0.5;
    }
}
