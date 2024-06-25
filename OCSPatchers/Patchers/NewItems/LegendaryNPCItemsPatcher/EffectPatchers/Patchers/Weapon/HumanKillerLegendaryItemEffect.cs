using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class HumanKillerLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Убийца людей";

        public override string Description => $"Урон против людей #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "human damage mult";

        protected override float VALUE => (float)0.5;
    }
}
