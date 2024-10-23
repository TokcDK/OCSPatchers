using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using static OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters.OCSPLegendaryNPCItems;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class BluntConvertedWeaponLegendaryItemEffect : LegendaryItemEffectWeaponBase
    {
        public override string Name => "Заострение";

        public override string Description => "Другие типы урона наносятся как режущий урон.\nКровотечение больше на четверть от других типов урона.";

        public override bool TryApplyWeaponEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!modItem.Values.TryGetValue("cut damage multiplier", out var v1)
                || v1 is not float cutDamageMult) return false;
            if (!modItem.Values.TryGetValue("pierce damage multiplier", out var v2)
                || v2 is not float pierceDamageMult) return false;
            if (!modItem.Values.TryGetValue("blunt damage multiplier", out var v3)
                || v3 is not float bluntDamageMult) return false;
            if (!modItem.Values.TryGetValue("bleed mult", out var v4)
                || v4 is not float bleedMult) return false;

            modItem.Values["blunt damage multiplier"] = (float)0; // reset damage
            modItem.Values["pierce damage multiplier"] = (float)0; // reset damage
            modItem.Values["cut damage multiplier"] = (float)Math.Round(cutDamageMult + bluntDamageMult + pierceDamageMult, 2); // set other types of damage to blunt
            modItem.Values["bleed mult"] = (float)Math.Round(bleedMult + (float)((bluntDamageMult + pierceDamageMult) / 4), 2);

            return true;
        }
    }
}
