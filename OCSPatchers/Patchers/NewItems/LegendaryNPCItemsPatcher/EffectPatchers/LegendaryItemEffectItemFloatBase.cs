using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers
{
    public abstract class LegendaryItemEffectItemFloatBase : ILegendaryItemEffect
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
            if (modItem.Values[KEY_NAME] is not float originValue) return false;

            modItem.Values[KEY_NAME] = (float)(originValue + (float)VALUE);

            return true;
        }

        protected abstract string KEY_NAME { get; }
        protected abstract float VALUE { get; }
    }
}
