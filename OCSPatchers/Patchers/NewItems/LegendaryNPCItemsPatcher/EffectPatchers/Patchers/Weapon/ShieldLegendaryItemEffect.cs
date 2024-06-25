using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class ShieldLegendaryItemEffect : LegendaryItemEffectWeaponBase
    {
        public override string Name => "Щит";

        public override string Description => "#afa68bЗащита #a8b774+20";

        const string KEY_NAME = "defence mod";

        public override bool TryApplyWeaponEffect(ModItem modItem)
        {
            if (!modItem.Values.ContainsKey(KEY_NAME)) return false;
            if (modItem.Values[KEY_NAME] is not int originValue) return false;

            modItem.Values[KEY_NAME] = originValue + 20;

            return true;
        }
    }
}
