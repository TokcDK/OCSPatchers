using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class HarpoonLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Гарпун";

        public override string Description => $"#afa68bПронзающий урон #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "pierce damage multiplier";

        protected override float VALUE => (float)0.1;
    }
}
