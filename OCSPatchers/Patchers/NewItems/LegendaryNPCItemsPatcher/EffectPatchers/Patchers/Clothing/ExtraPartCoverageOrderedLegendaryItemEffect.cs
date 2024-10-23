using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class ExtraPartCoverageOrderedLegendaryItemEffect : ExtraPartCoverageRandomLegendaryItemEffect
    {
        //public override string Name => "Экстра покрытие";

        //public override string Description => $"#afa68b Покрывает больше частей тела #a8b774";

        readonly Dictionary<int, List<int>> _slotCoverageOrder = new()
        {
            {
                (int)Data.Enums.AttachSlot.ATTACH_HAT, new()
                {
                    4,
                    6,
                    5,
                    0,
                    1,
                    2,
                    3,
                }
            },
            {
                (int)Data.Enums.AttachSlot.ATTACH_BODY, new()
                {
                    6,
                    5,
                    4,
                    0,
                    1,
                    2,
                    3,
                }
            },
            {
                (int)Data.Enums.AttachSlot.ATTACH_BELT, new()
                {
                    5,
                    6,
                    4,
                    0,
                    1,
                    2,
                    3,
                }
            },
            {
                (int)Data.Enums.AttachSlot.ATTACH_SHIRT, new()
                {
                    6,
                    5,
                    0,
                    1,
                    4,
                    2,
                    3,
                }
            },
            {
                (int)Data.Enums.AttachSlot.ATTACH_LEGS, new()
                {
                    2,
                    3,
                    5,
                    6,
                    4,
                    0,
                    1,
                }
            },
            {
                (int)Data.Enums.AttachSlot.ATTACH_BOOTS, new()
                {
                    2,
                    3,
                    5,
                    6,
                    4,
                    0,
                    1,
                }
            },
        };
        public override bool TryApplyClothingEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!modItem.ReferenceCategories.ContainsKey("part coverage")) return false;
            if (!modItem.Values.TryGetValue("slot", out var v) 
                || v is not int id 
                || !_slotCoverageOrder.ContainsKey(id)) return false;

            _order = _slotCoverageOrder[id];

            var partCoverageRefs = modItem.ReferenceCategories["part coverage"].References;

            return SetExtraPartsCoverage(partCoverageRefs, context);
        }

        List<int>? _order;
        private bool SetExtraPartsCoverage(OpenConstructionSet.Mods.Context.ModReferenceCollection partCoverageRefs, OpenConstructionSet.Mods.Context.IModContext context)
        {
            bool isAnyCoverageAdded = false;

            int extraCoverageAmount = 200;
            foreach (var part in _order!) 
            {
                if (extraCoverageAmount <= 0) break; // break if no extra coverage available

                // set part reference from exist parts or add new
                ModReference? partItemRef;
                string partStringId = CoveragePartsStringIds[part];
                if (!partCoverageRefs.ContainsKey(partStringId))
                {
                    // add new coverage part
                    partItemRef = new ModReference(partStringId, 0);
                    partCoverageRefs.Add(partItemRef);
                }
                else partItemRef = partCoverageRefs[partStringId]; // reference exist part ref

                int requiredCoverageAmount = 100 - partItemRef.Value0; // calculate coverage amount required to add for current

                if (requiredCoverageAmount == 0) continue; // skip if no coverage required, already 100% covered

                // calc available coverage
                int availableCoverage = extraCoverageAmount >= requiredCoverageAmount ? requiredCoverageAmount : extraCoverageAmount;

                extraCoverageAmount -= requiredCoverageAmount; // recalc extra coverage amount

                partItemRef.Value0 = partItemRef.Value0 + availableCoverage; // fill coverage

                isAnyCoverageAdded = true; // mark as added coverage
            }

            return isAnyCoverageAdded;
        }
    }
}
