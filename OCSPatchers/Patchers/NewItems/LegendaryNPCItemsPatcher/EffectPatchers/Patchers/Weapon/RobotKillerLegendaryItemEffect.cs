using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class RobotKillerLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Убийца роботов";

        public override string Description => $"Урон против роботов #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "robot damage mult";

        protected override float VALUE => (float)0.5;
    }
}
