using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class PenetratingLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Пробивание";

        public override string Description => $"Пробивание брони #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "armour penetration";

        protected override float VALUE => (float)0.5;
    }
}
