using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers
{
    public abstract class LegendaryItemEffectWeaponBase : ILegendaryWeaponEffect
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        const string KEY_NAME = "defence mod";

        public bool TryApplyEffect(ModItem modItem)
        {
            return modItem.Type == ItemType.Weapon && TryApplyWeaponEffect(modItem);
        }
        public abstract bool TryApplyWeaponEffect(ModItem modItem);
    }
}
