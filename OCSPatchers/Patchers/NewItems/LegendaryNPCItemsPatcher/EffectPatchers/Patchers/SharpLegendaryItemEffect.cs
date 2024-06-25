using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class SharpLegendaryItemEffect : LegendaryItemEffectWeaponBase
    {
        public override string Name => "Острота";

        public override string Description => "#afa68bРежущий урон #a8b774+20%";

        const string KEY_NAME = "cut damage multiplier";

        public override bool TryApplyWeaponEffect(ModItem modItem)
        {
            if (modItem.Type != ItemType.Weapon) return false;
            if (!modItem.Values.ContainsKey(KEY_NAME)) return false;
            if (modItem.Values[KEY_NAME] is not float originValue) return false;

            modItem.Values[KEY_NAME] = originValue + (float)0.2;

            return true;
        }
    }
}
