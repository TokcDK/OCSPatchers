using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers
{
    public abstract class LegendaryItemEffectItemIntegerBase : ILegendaryItemEffect
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public bool TryApplyEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            return TryApplyItemEffect(modItem);
        }
        bool TryApplyItemEffect(ModItem modItem)
        {
            if (!modItem.Values.ContainsKey(KEY_NAME)) return false;
            if (modItem.Values[KEY_NAME] is not int originValue) return false;

            int newValue = (int)(originValue + (int)VALUE);
            modItem.Values[KEY_NAME] = newValue > 100 ? 100 : newValue;

            return true;
        }

        protected abstract string KEY_NAME { get; }
        protected abstract int VALUE { get; }
    }
}
