using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class AnimalKillerLegendaryItemEffect : LegendaryItemEffectItemFloatBase
    {
        public override string Name => "Убийца животных";

        public override string Description => $"#afa68bУрон животным #a8b774+{VALUE*100}%";

        protected override string KEY_NAME => "animal damage mult";

        protected override float VALUE => (float)0.5;
    }
}
