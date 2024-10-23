using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class DurableNanofiberLegendaryItemEffect : ILegendaryArmorEffect
    {
        public string Name => "Прочное нановолокно";

        public string Description => $"#afa68bСопротивление урону#a8b774+50%\nСпециальная структупа предотвращает ударные посттравмы от порезов.";

        public bool TryApplyEffect(ModItem modItem, IModContext context)
        {
            if (!modItem.Values.TryGetValue("blunt def bonus", out var v1)
                || v1 is not float bluntResMult) return false;
            if (!modItem.Values.TryGetValue("cut def bonus", out var v2)
                || v2 is not float cutResMult) return false;
            if (!modItem.Values.TryGetValue("cut into stun", out var v3)
                || v3 is not float cutIntoStunMult) return false;
            if (!modItem.Values.TryGetValue("pierce def mult", out var v4)
                || v4 is not float pierceDefMult) return false;

            modItem.Values["blunt def bonus"] = (float)(bluntResMult + 0.3);
            modItem.Values["cut def bonus"] = (float)(cutResMult + 0.3);
            modItem.Values["cut into stun"] = (float)(0);
            modItem.Values["pierce def mult"] = (float)(pierceDefMult + 0.3);

            return true;
        }
    }
}
