using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class CombatSpeedLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Боевые импланты";

        public override string Description => $"#afa68bСкорость битвы #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "combat speed mult";

        protected override float VALUE => (float)0.5;
    }
}
