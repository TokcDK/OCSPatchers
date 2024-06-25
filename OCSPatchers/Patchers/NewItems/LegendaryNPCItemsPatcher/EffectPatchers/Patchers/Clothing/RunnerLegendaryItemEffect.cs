using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class RunnerLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Бегун";

        public override string Description => $"#afa68bАтлетика #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "athletics mult";

        protected override float VALUE => (float)0.5;
    }
}
