using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class DexterityLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Ловкость";

        public override string Description => $"#afa68b{Name} #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "dexterity mult";

        protected override float VALUE => (float)0.5;
    }
}
