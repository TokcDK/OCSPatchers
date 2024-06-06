using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCSPatchers.Data;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP
{
    internal class OCSPResetToAOMAnims : OCSPatcherBase
    {
        public override string PatcherName => "Set animations to animations from animations overhaul";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var aomAinms = new Dictionary<Enums.CharacterStats, string>()
            {
                { Enums.CharacterStats.STAT_SCIENCE, "1535118-AnimationOverhaul.mod" },
                { Enums.CharacterStats.STAT_SMITHING_WEAPON, "1535117-AnimationOverhaul.mod" },
                { Enums.CharacterStats.STAT_SMITHING_ARMOUR, "1535117-AnimationOverhaul.mod" },
                { Enums.CharacterStats.STAT_SMITHING_BOW, "1535117-AnimationOverhaul.mod" },
                { Enums.CharacterStats.STAT_COOKING, "1535114-AnimationOverhaul.mod" },
            };
            var aomAinmsReferences = new Dictionary<Enums.CharacterStats, ModItem>();

            foreach(var aom in aomAinms)
            {
                var animationItem = context.Items.OfType(ItemType.Animation).FirstOrDefault(i => i.StringId == aom.Value);
                if (animationItem == default) continue;

                aomAinmsReferences.Add(aom.Key, animationItem);
            }
            aomAinms = null;

            foreach (var item in context.Items.OfType(ItemType.BuildingFunctionality))
            {
                if (!item.Values.ContainsKey("stat used")) continue;
                if (!item.Values.ContainsKey("function")) continue;

                var statUsedValue = (Enums.CharacterStats)item.Values["stat used"];
                if (!aomAinmsReferences.ContainsKey(statUsedValue)) continue;
                 
                var functionValue = (Enums.BuildingFunction)item.Values["function"];
                if (functionValue != Enums.BuildingFunction.BF_CRAFTING) continue;

                var animationItem = aomAinmsReferences[statUsedValue];
                string animationStringId = animationItem.StringId;

                if (!item.ReferenceCategories.ContainsKey("animation")) continue;

                var animationsCategoryReferences = item.ReferenceCategories["animation"].References;

                if (animationsCategoryReferences.ContainsKey(animationStringId)) continue;
                if(animationsCategoryReferences.Any(i=>i.TargetId.Contains("AnimationOverhaul.mod"))) continue;

                animationsCategoryReferences.Clear();
                animationsCategoryReferences.Add(animationItem);
            }

            return Task.CompletedTask;
        }
    }
}
