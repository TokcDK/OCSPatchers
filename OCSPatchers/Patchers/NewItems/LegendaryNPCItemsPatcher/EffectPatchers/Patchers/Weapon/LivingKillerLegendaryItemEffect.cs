using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class LivingKillerLegendaryItemEffect : ILegendaryItemEffect
    {
        public string Name => "Убийца живого";

        public string Description => $"Урон против живых #a8b774+{50}%";

        readonly ILegendaryItemEffect _humanKiller = new HumanKillerLegendaryItemEffect();
        readonly ILegendaryItemEffect _animalKiller = new AnimalKillerLegendaryItemEffect();
        public bool TryApplyEffect(ModItem modItem)
        {
            return _humanKiller.TryApplyEffect(modItem) && _animalKiller.TryApplyEffect(modItem);
        }
    }
}
