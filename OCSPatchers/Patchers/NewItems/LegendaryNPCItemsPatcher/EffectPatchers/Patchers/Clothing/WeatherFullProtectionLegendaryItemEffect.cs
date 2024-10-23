using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class WeatherFullProtectionLegendaryItemEffect : WeatherProtectionLegendaryItemEffect
    {
        public override string Name => "Полная защита от погоды";

        public override string Description => $"#afa68b{Name}#a8b774";

        protected override float ResistAmount => 1;
    }
}
