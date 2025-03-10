﻿using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class ExtraCoverageLegendaryItemEffect : LegendaryItemEffectClothingBase
    {
        public override string Name => "Экстра покрытие";

        public override string Description => $"#afa68b {Name} #a8b774+50%";

        protected readonly Random _rnd = new();
        public override bool TryApplyClothingEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!modItem.ReferenceCategories.ContainsKey("part coverage")) return false;

            // increase coverage of exist parts
            var partCoverageRefs = modItem.ReferenceCategories["part coverage"].References;

            foreach (var partCoverageRef in partCoverageRefs)
            {
                TryIncreaseCoverageOfThePart(partCoverageRef);
            }

            return _isIncreasedAnyExistPartCoverage;
        }

        private void TryIncreaseCoverageOfThePart(ModReference partCoverageRef)
        {
            int newValue = (int)(partCoverageRef.Value0 * 1.5);
            if (!_isIncreasedAnyExistPartCoverage && newValue > partCoverageRef.Value0)
            {
                _isIncreasedAnyExistPartCoverage = true;
            }

            partCoverageRef.Value0 = newValue > 100 ? 100 : newValue;
        }

        protected bool _isIncreasedAnyExistPartCoverage = false;
    }
}
