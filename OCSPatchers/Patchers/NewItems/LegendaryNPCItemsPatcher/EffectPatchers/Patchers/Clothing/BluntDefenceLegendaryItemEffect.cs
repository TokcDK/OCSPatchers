using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class BluntDefenceLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Защита от удара";

        public override string Description => $"#afa68b{Name} #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "blunt def bonus";

        protected override float VALUE => (float)0.3;
    }
}
