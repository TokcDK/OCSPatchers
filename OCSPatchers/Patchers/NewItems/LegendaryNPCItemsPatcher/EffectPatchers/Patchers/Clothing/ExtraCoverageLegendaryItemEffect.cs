using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class ExtraCoverageLegendaryItemEffect : LegendaryItemEffectClothingBase
    {
        public override string Name => "Экстра покрытие";

        public override string Description => $"#afa68b{Name} #a8b774+50%";

        public override bool TryApplyClothingEffect(ModItem modItem)
        {
            if (!modItem.ReferenceCategories.ContainsKey("part coverage")) return false;

            var partCoverageRefs = modItem.ReferenceCategories["part coverage"].References;

            foreach (var partCoverageRef in partCoverageRefs)
            {
                int newValue = (int)(partCoverageRef.Value0 * 1.5);
                partCoverageRef.Value0 = newValue > 100 ? 100 : newValue;
            }

            return true;
        }
    }
}
