using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class NinjaLegendaryItemEffect : ILegendaryArmorEffect
    {
        public string Name => "Ниндзя";

        public string Description => $"#afa68bНавыки скрытности#a8b774+30%, #afa68bЛовкость#a8b774+10%, #afa68bУворот#a8b774+10%, #afa68bАтлетика#a8b774+20%";

        public bool TryApplyEffect(ModItem modItem, IModContext context)
        {
            if (!modItem.Values.TryGetValue("asassination mult", out var v1)
                || v1 is not float asassinationMult) return false;
            if (!modItem.Values.TryGetValue("stealth mult", out var v2)
                || v2 is not float stealthMult) return false;
            if (!modItem.Values.TryGetValue("dexterity mult", out var v3)
                || v3 is not float dexMult) return false;
            if (!modItem.Values.TryGetValue("dodge mult", out var v4)
                || v4 is not float evaMult) return false;
            if (!modItem.Values.TryGetValue("athletics mult", out var v5)
                || v5 is not float atlethicsMult) return false;

            modItem.Values["asassination mult"] = (float)(asassinationMult + 0.3);
            modItem.Values["stealth mult"] = (float)(stealthMult + 0.3);
            modItem.Values["dexterity mult"] = (float)(dexMult + 0.1);
            modItem.Values["dexterity mult"] = (float)(evaMult + 0.1);
            modItem.Values["dexterity mult"] = (float)(atlethicsMult + 0.2);

            return true;
        }
    }
}
