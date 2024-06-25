using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class MinSharpLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Заточенность";

        public override string Description => $"#afa68bМинимальный режущий урон #a8b774+{VALUE8100}%";

        protected override string KEY_NAME => "min cut damage mult";

        protected override float VALUE => (float)0.2;
    }
}
