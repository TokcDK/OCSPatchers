using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class ExtraCoverageLegendaryItemEffect : LegendaryItemEffectClothingBase
    {
        public override string Name => "Экстра покрытие";

        public override string Description => $"#afa68b Покрывает больше частей тела #a8b774";

        bool _isSetExtraPartCoverageItems = false;
        readonly List<ModItem?> _parts = new();
        readonly Random _rnd = new();
        readonly List<string> _coveragePartsStringIds = new()
        {
                    "28-gamedata.quack", // left arm
                    "29-gamedata.quack", //right arm
                    "30-gamedata.quack", //left leg
                    "31-gamedata.quack", // right leg
                    "32-gamedata.quack", // head
                    "100-gamedata.quack", // belly
                    "101-gamedata.quack", // chest
                };
        public override bool TryApplyClothingEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!modItem.ReferenceCategories.ContainsKey("part coverage")) return false;

            // increase coverage of exist parts
            var partCoverageRefs = modItem.ReferenceCategories["part coverage"].References;

            var existCoveragePartIds = new HashSet<string>();
            foreach (var partCoverageRef in partCoverageRefs)
            {
                TryIncreaseCoverageOfThePart(partCoverageRef);

                existCoveragePartIds.Add(partCoverageRef.TargetId);
            }

            SetExtraPartsCoverage(partCoverageRefs, context, existCoveragePartIds);

            return true;
        }

        private void TryIncreaseCoverageOfThePart(ModReference partCoverageRef, double extraMultCoverage = 0)
        {
            double multiplier = _rnd.NextDouble() + (1 + extraMultCoverage); // random between no extra coverage to double coverage
            int newValue = (int)(partCoverageRef.Value0 * multiplier);
            if (!_isIncreasedAnyExistPartCoverage && newValue > partCoverageRef.Value0)
            {
                _isIncreasedAnyExistPartCoverage = true;
            }

            partCoverageRef.Value0 = newValue > 100 ? 100 : newValue;
        }

        bool _isIncreasedAnyExistPartCoverage = false;
        private void SetExtraPartsCoverage(OpenConstructionSet.Mods.Context.ModReferenceCollection partCoverageRefs, OpenConstructionSet.Mods.Context.IModContext context, HashSet<string> existCoveragePartIds)
        {
            GetAllCoveragePartIds(context);

            // vaiant when item have all parts coverage and no parts was covered
            if (existCoveragePartIds.Count == 7)
            {
                TryAddAtleastOnePartExtraCoverage(partCoverageRefs);

                return;
            }

            // setup extra coverage for extra parts which is not presented in list
            int chance = 100 + (_isIncreasedAnyExistPartCoverage ? 0 : 100); // chance for first extra part will be 100% if no coverage was increased for exist coverage parts
            int cnt = _coveragePartsStringIds.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (_rnd.Next(0, 100) > (chance /= 2)) break;

                var partIndex = _rnd.Next(0, cnt);

                string stringId = _coveragePartsStringIds[partIndex];

                if (existCoveragePartIds.Contains(stringId))
                {
                    continue;
                }

                partCoverageRefs.Add(new ModReference(stringId, _rnd.Next(1, 10) * 10));
            }
        }

        private void TryAddAtleastOnePartExtraCoverage(ModReferenceCollection partCoverageRefs)
        {
            // add extra coverage to atleast one part of 7 if not 100% all
            if (_isIncreasedAnyExistPartCoverage)
            {
                return;
            }

            var firstWhereNotAllCovered = partCoverageRefs.FirstOrDefault(r => r.Value0 < 100);
            if (firstWhereNotAllCovered == default)
            {
                return;
            }

            TryIncreaseCoverageOfThePart(firstWhereNotAllCovered, 0.1);
        }

        private void GetAllCoveragePartIds(OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!_isSetExtraPartCoverageItems)
            {
                _isSetExtraPartCoverageItems = true;

                foreach (var coveragePartStringId in _coveragePartsStringIds)
                {
                    _parts.Add(context.Items.OfType(ItemType.LocationalDamage).First(i => i.StringId == coveragePartStringId));
                }
            }
        }
    }
}
