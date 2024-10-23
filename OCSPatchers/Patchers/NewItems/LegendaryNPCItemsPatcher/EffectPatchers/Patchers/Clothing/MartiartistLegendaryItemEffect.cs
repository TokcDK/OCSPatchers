using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class MartiartistLegendaryItemEffect : ILegendaryArmorEffect
    {
        public string Name => "Рукопашник";

        public string Description => $"#afa68b Урон #a8b774+20% и полная защита кулаков";

        public bool TryApplyEffect(ModItem modItem, IModContext context)
        {
            if (!modItem.Values.TryGetValue("damage output mult", out var v1)
                || v1 is not float dmgMult) return false;
            if (!modItem.Values.TryGetValue("fist injury mult", out var v2)
                || v2 is not float fistsInjMult) return false;
            if (!modItem.Values.TryGetValue("unarmed bonus", out var v3)
                || v3 is not int unarmedBonus) return false;

            modItem.Values["damage output mult"] = (float)(dmgMult + 0.2);
            modItem.Values["fist injury mult"] = (float)0;
            modItem.Values["unarmed bonus"] = unarmedBonus + 20;

            return true;
        }
    }
}
