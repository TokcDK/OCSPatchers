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
        //readonly List<ModItem?> _parts = new();
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

            // возможно лучше заменить на добавление покрытия экстра части в зависимости от того, к чему добавляем, к рубахе, штанам, броне, шлему
            // для шлема это голова - грудь - живот - правая рука - левая рука - правая нога - левая нога
            // для штанов это правая нога - левая нога - живот - грудь - голова - правая рука - левая рука
            // для брони это грудь - живот - правая рука - левая рука - правая нога - левая нога - голова
            // для рубашки это грудь - живот - правая рука - левая рука - голова - правая нога - левая нога
            // может быть комбинирование с увеличением покрытия существующих и добавлением доп частей в зависимости от покрытия существующих
            // может быть улучшить первый вариант с увеличением покрытия, когда при 100% у всех уже существующих добавлять одну экстра часть
            SetExtraPartsCoverage(partCoverageRefs, context);

            return true;
        }

        private void SetExtraPartsCoverage(OpenConstructionSet.Mods.Context.ModReferenceCollection partCoverageRefs, OpenConstructionSet.Mods.Context.IModContext context)
        {
            //GetAllCoveragePartIds(context);

            // setup extra coverage for extra parts which is not presented in list
            int chance = 80;
            int cnt = _coveragePartsStringIds.Count;
            int initCoverage = 5;
            for (int i = 0; i < cnt; i++)
            {
                if (_rnd.Next(0, 70) > (chance -= 10)) break; // first will be 100% added and then other will reduce chance for each step

                var partIndex = _rnd.Next(0, cnt);

                string stringId = _coveragePartsStringIds[partIndex];

                if (partCoverageRefs.ContainsKey(stringId))
                {
                    continue;
                }

                partCoverageRefs.Add(new ModReference(stringId, _rnd.Next((initCoverage--) > 0 ? initCoverage : 1, 10) * 10)); // 10%-100% coverage, init 50%
            
                if(partCoverageRefs.Count == 7)
                {
                    // all parts added
                    break;
                }
            }
        }

        //private void GetAllCoveragePartIds(OpenConstructionSet.Mods.Context.IModContext context)
        //{
        //    if (!_isSetExtraPartCoverageItems)
        //    {
        //        _isSetExtraPartCoverageItems = true;

        //        foreach (var coveragePartStringId in _coveragePartsStringIds)
        //        {
        //            _parts.Add(context.Items.OfType(ItemType.LocationalDamage).First(i => i.StringId == coveragePartStringId));
        //        }
        //    }
        //}
    }
}
