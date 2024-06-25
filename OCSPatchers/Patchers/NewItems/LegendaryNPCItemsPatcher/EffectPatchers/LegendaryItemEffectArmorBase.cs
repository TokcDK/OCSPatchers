using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers
{
    public abstract class LegendaryItemEffectArmorBase : ILegendaryArmorEffect
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public bool TryApplyEffect(ModItem modItem)
        {
            return modItem.Type == ItemType.Armour && TryApplyWeaponEffect(modItem);
        }
        public abstract bool TryApplyWeaponEffect(ModItem modItem);
    }
}
