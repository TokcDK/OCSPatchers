using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class ExtraPartCoverageLegendaryItemEffect : ExtraCoverageLegendaryItemEffect
    {
        public override string Name => "Экстра покрытие";

        public override string Description => $"#afa68b Покрывает больше частей тела #a8b774";

        bool _isSetExtraPartCoverageItems = false;
        readonly List<ModItem?> _parts = new();
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
            if (partCoverageRefs.Count == 7)
            {
                return false; // all already covered
            }

            SetExtraPartsCoverage(partCoverageRefs, context);

            return true;
        }

        private void SetExtraPartsCoverage(OpenConstructionSet.Mods.Context.ModReferenceCollection partCoverageRefs, OpenConstructionSet.Mods.Context.IModContext context)
        {
            GetAllCoveragePartIds(context);

            // setup extra coverage for extra parts which is not presented in list
            int chance = 400;
            int cnt = _coveragePartsStringIds.Count;
            int initCoverage = 5;
            for (int i = 0; i < cnt; i++)
            {
                if (_rnd.Next(0, 200) <= (chance /= 2)) break;

                var partIndex = _rnd.Next(0, cnt);

                string stringId = _coveragePartsStringIds[partIndex];

                if (partCoverageRefs.ContainsKey(stringId))
                {
                    continue;
                }

                partCoverageRefs.Add(new ModReference(stringId, _rnd.Next((initCoverage--) > 0 ? initCoverage : 1, 10) * 10)); // 10%-100% coverage, init 50%
            }
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
