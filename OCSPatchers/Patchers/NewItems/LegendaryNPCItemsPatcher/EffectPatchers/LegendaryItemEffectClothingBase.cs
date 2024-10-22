using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers
{
    public abstract class LegendaryItemEffectClothingBase : ILegendaryArmorEffect
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public bool TryApplyEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            return modItem.Type == ItemType.Armour && TryApplyClothingEffect(modItem, context);
        }
        public abstract bool TryApplyClothingEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context);
    }
}
