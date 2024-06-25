using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class AttackerSharpLegendaryItemEffect : LegendaryItemEffectArmorBase
    {
        public override string Name => "Атакер";

        public override string Description => "#afa68bАтака #a8b774+10";

        const string KEY_NAME = "combat attk bonus";

        public override bool TryApplyWeaponEffect(ModItem modItem)
        {
            if (!modItem.Values.ContainsKey(KEY_NAME)) return false;
            if (modItem.Values[KEY_NAME] is not int originValue) return false;

            modItem.Values[KEY_NAME] = (float)(originValue + 10);

            return true;
        }
    }
}
