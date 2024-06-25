using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers
{
    public abstract class LegendaryItemEffectItemIntegerBase : ILegendaryItemEffect
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public bool TryApplyEffect(ModItem modItem)
        {
            return TryApplyItemEffect(modItem);
        }
        bool TryApplyItemEffect(ModItem modItem)
        {
            if (!modItem.Values.ContainsKey(KEY_NAME)) return false;
            if (modItem.Values[KEY_NAME] is not int originValue) return false;

            modItem.Values[KEY_NAME] = (int)(originValue + (int)VALUE);

            return true;
        }

        protected abstract string KEY_NAME { get; }
        protected abstract int VALUE { get; }
    }
}
