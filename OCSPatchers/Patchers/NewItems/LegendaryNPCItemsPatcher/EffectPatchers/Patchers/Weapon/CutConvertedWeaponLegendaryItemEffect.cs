using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using static OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters.OCSPLegendaryNPCItems;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class CutConvertedWeaponLegendaryItemEffect : LegendaryItemEffectWeaponBase
    {
        public override string Name => "Затупливание";

        public override string Description => "Другие типы урона наносятся как ударный урон.\nНет кровотечения, но четверть кровотечения наносится как ударный урон.";

        public override bool TryApplyWeaponEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!modItem.Values.TryGetValue("cut damage multiplier", out var v1)
                || v1 is not float cutDamageMult) return false;
            if (!modItem.Values.TryGetValue("pierce damage multiplier", out var v2)
                || v2 is not float pierceDamageMult) return false;
            if (!modItem.Values.TryGetValue("blunt damage multiplier", out var v3)
                || v3 is not float bluntDamageMult) return false;
            if (!modItem.Values.TryGetValue("bleed mult", out var v4)
                || v3 is not float bleedMult) return false;

            modItem.Values["cut damage multiplier"] = (float)0; // reset cut damage
            modItem.Values["pierce damage multiplier"] = (float)0; // reset damage
            modItem.Values["bleed mult"] = (float)0; // no bleed for blunt
            modItem.Values["blunt damage multiplier"] = (float)Math.Round(bluntDamageMult + cutDamageMult + pierceDamageMult + (bleedMult / 4), 2); // set other types of damage to blunt

            return true;
        }
    }
}
