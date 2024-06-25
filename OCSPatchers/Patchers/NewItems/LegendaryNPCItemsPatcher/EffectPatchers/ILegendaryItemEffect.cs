using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers
{
    public interface ILegendaryItemEffect
    {
        public string Name { get; }
        public string Description { get; }

        bool TryApplyEffect(ModItem modItem);
    }
}
