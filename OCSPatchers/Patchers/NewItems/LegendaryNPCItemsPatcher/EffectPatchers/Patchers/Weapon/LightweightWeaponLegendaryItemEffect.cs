using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using static OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters.OCSPLegendaryNPCItems;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class LightweightWeaponLegendaryItemEffect : LegendaryItemEffectWeaponBase
    {
        public override string Name => "Облегченное";

        public override string Description => "Вес легче вдвое.";

        public override bool TryApplyWeaponEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!modItem.Values.TryGetValue("weight mult", out var v1)
                || v1 is not float weightMult) return false;

            modItem.Values["weight mult"] = (float)Math.Round(weightMult / 2, 1); // 

            return true;
        }
    }
}
