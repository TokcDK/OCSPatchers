using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    internal class OCSPWIPPatcher : OCSPatcherBase
    {
        public override string PatcherName => throw new NotImplementedException();

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            ////var listOfLimbs = new List<ModReference>();// string ids
            ////var addedLimbs = new HashSet<string>();// string ids
            ////// add limbs from chars
            ////foreach (var character in context.Items.OfType(ItemType.Character))
            ////{
            ////    if (!character.Values.ContainsKey("unique")) continue;
            ////    if (character.Values["unique"] is not bool unique || unique) continue;
            ////    if (!character.ReferenceCategories.ContainsKey("robot limbs")) continue;

            ////    foreach (var reference in character.ReferenceCategories["robot limbs"].References)
            ////    {
            ////        if (addedLimbs.Contains(reference.TargetId)) continue;

            ////        addedLimbs.Add(reference.TargetId);
            ////        listOfLimbs.Add(reference);
            ////    }
            ////}
            ////foreach (var character in context.Items.OfType(ItemType.Character))
            ////{
            ////    if (!character.Values.ContainsKey("unique")) continue;
            ////    if (character.Values["unique"] is not bool unique || unique) continue;
            ////    //if (!character.ReferenceCategories.ContainsKey("robot limbs")) continue;

            ////    if (character.Values["armour grade"] is not int s) continue;

            ////    int limbQuality = 0;
            ////    if (s == 0)//"GEAR_PROTOTYPE"
            ////    {
            ////    }
            ////    else if (s == 1)//"GEAR_CHEAP"
            ////    {
            ////        limbQuality = 20;
            ////    }
            ////    else if (s == 2)//"GEAR_STANDARD"
            ////    {
            ////        limbQuality = 40;
            ////    }
            ////    else if (s == 3)//"GEAR_GOOD"
            ////    {
            ////        limbQuality = 60;
            ////    }
            ////    else if (s == 4)//"GEAR_QUALITY"
            ////    {
            ////        limbQuality = 80;
            ////    }
            ////    else if (s == 5)//"GEAR_MASTER"
            ////    {
            ////        limbQuality = 100;
            ////    }

            ////    float charCombat = (character.Values["combat stats"] is int cc ? cc : 0);
            ////    float charRanged = (character.Values["ranged stats"] is int cr ? cr : 0);
            ////    float charStealth = (character.Values["stealth stats"] is int cs ? cs : 0);


            ////    if (!character.ReferenceCategories.ContainsKey("robot limbs")) character.ReferenceCategories.Add("robot limbs");
            ////    var categoryReferences = character.ReferenceCategories["robot limbs"].References;
            ////    foreach (var limb in listOfLimbs)
            ////    {
            ////        if (categoryReferences.ContainsKey(limb.TargetId)) continue;

            ////        float limbdex1 = (limb.Target!.Values["dexterity mult"] is float ldf ? ldf : 0);
            ////        float limbdex2 = (limb.Target!.Values["dexterity mult 1"] is float ldf1 ? ldf1 : 0);
            ////        float limbDex = (limbdex1 + limbdex2) / 2;

            ////        float limbranged1 = (limb.Target!.Values["ranged mult"] is float lrf ? lrf : 0);
            ////        float limbranged2 = (limb.Target!.Values["ranged mult 1"] is float lrf1 ? lrf1 : 0);
            ////        float limbRanged = (limbranged1 + limbranged2) / 2;

            ////        float limbstr1 = (limb.Target!.Values["strength mult"] is float lsf ? lsf : 0);
            ////        float limbstr2 = (limb.Target!.Values["strength mult 1"] is float lsf1 ? lsf1 : 0);
            ////        float limbStr = (limbstr1 + limbstr2) / 2;

            ////        float limbthie1 = (limb.Target!.Values["thievery mult"] is float ltf ? ltf : 0);
            ////        float limbthie2 = (limb.Target!.Values["thievery mult 1"] is float ltf1 ? ltf1 : 0);
            ////        float limbThie = (limbthie1 + limbthie2) / 2;

            ////        float limbstealth1 = (limb.Target!.Values["stealth mult"] is float lstealthf ? lstealthf : 0);
            ////        float limbstealth2 = (limb.Target!.Values["stealth mult 1"] is float lstealthf1 ? lstealthf1 : 0);
            ////        float limbstealth = (limbstealth1 + limbstealth2) / 2;

            ////        float limbathletics1 = (limb.Target!.Values["athletics mult"] is float lathleticsf ? lathleticsf : 0);
            ////        float limbathletics2 = (limb.Target!.Values["athletics mult 1"] is float lathleticsf1 ? lathleticsf1 : 0);
            ////        float limbathletics = (limbstealth1 + limbstealth2) / 2;

            ////        var limbCombatResult = (limbDex + limbStr + limbathletics) / 3;
            ////        var limbRangedResult = (limbDex + limbRanged + limbathletics) / 3;
            ////        var limbStealthResult = (limbDex + limbThie + limbstealth) / 3;

            ////        int relativeCombat = (int)(charCombat * limbCombatResult);
            ////        int relativeRanged = (int)(charRanged * limbRangedResult);
            ////        int relativeStealth = (int)(charStealth * limbStealthResult);

            ////        var limbRelativeChance = Math.Max(Math.Max(relativeCombat, relativeRanged), relativeStealth)/10;

            ////        categoryReferences.Add(limb.TargetId, limbQuality, limbRelativeChance);
            ////    }
            ////}

            //// crossbows distribute
            ////var ItemsToDistributeList = new List<ModReference>();// string ids
            ////var addedItemsList = new HashSet<string>();// string ids
            ////// add limbs from chars
            ////foreach (var character in context.Items.OfType(ItemType.Character))
            ////{
            ////    if (!character.ReferenceCategories.ContainsKey("crossbows")) continue;

            ////    foreach (var reference in character.ReferenceCategories["crossbows"].References)
            ////    {
            ////        if (addedItemsList.Contains(reference.TargetId)) continue;

            ////        addedItemsList.Add(reference.TargetId);
            ////        ItemsToDistributeList.Add(reference);
            ////    }
            ////}
            ////foreach (var character in context.Items.OfType(ItemType.Character))
            ////{
            ////    //if (!character.ReferenceCategories.ContainsKey("robot limbs")) continue;

            ////    if (character.Values["armour grade"] is not int s) continue;

            ////    int limbQuality = 0;
            ////    if (s == 0)//"GEAR_PROTOTYPE"
            ////    {
            ////    }
            ////    else if (s == 1)//"GEAR_CHEAP"
            ////    {
            ////        limbQuality = 20;
            ////    }
            ////    else if (s == 2)//"GEAR_STANDARD"
            ////    {
            ////        limbQuality = 40;
            ////    }
            ////    else if (s == 3)//"GEAR_GOOD"
            ////    {
            ////        limbQuality = 60;
            ////    }
            ////    else if (s == 4)//"GEAR_QUALITY"
            ////    {
            ////        limbQuality = 80;
            ////    }
            ////    else if (s == 5)//"GEAR_MASTER"
            ////    {
            ////        limbQuality = 100;
            ////    }

            ////    float charCombat = (character.Values["combat stats"] is int cc ? cc : 0);
            ////    float charRanged = (character.Values["ranged stats"] is int cr ? cr : 0);
            ////    float charStealth = (character.Values["stealth stats"] is int cs ? cs : 0);


            ////    if (!character.ReferenceCategories.ContainsKey("robot limbs")) character.ReferenceCategories.Add("robot limbs");
            ////    var categoryReferences = character.ReferenceCategories["robot limbs"].References;
            ////    foreach (var limb in ItemsToDistributeList)
            ////    {
            ////        if (categoryReferences.ContainsKey(limb.TargetId)) continue;

            ////        float limbdex1 = (limb.Target!.Values["dexterity mult"] is float ldf ? ldf : 0);
            ////        float limbdex2 = (limb.Target!.Values["dexterity mult 1"] is float ldf1 ? ldf1 : 0);
            ////        float limbDex = (limbdex1 + limbdex2) / 2;

            ////        float limbranged1 = (limb.Target!.Values["ranged mult"] is float lrf ? lrf : 0);
            ////        float limbranged2 = (limb.Target!.Values["ranged mult 1"] is float lrf1 ? lrf1 : 0);
            ////        float limbRanged = (limbranged1 + limbranged2) / 2;

            ////        float limbstr1 = (limb.Target!.Values["strength mult"] is float lsf ? lsf : 0);
            ////        float limbstr2 = (limb.Target!.Values["strength mult 1"] is float lsf1 ? lsf1 : 0);
            ////        float limbStr = (limbstr1 + limbstr2) / 2;

            ////        float limbthie1 = (limb.Target!.Values["thievery mult"] is float ltf ? ltf : 0);
            ////        float limbthie2 = (limb.Target!.Values["thievery mult 1"] is float ltf1 ? ltf1 : 0);
            ////        float limbThie = (limbthie1 + limbthie2) / 2;

            ////        float limbstealth1 = (limb.Target!.Values["stealth mult"] is float lstealthf ? lstealthf : 0);
            ////        float limbstealth2 = (limb.Target!.Values["stealth mult 1"] is float lstealthf1 ? lstealthf1 : 0);
            ////        float limbstealth = (limbstealth1 + limbstealth2) / 2;

            ////        float limbathletics1 = (limb.Target!.Values["athletics mult"] is float lathleticsf ? lathleticsf : 0);
            ////        float limbathletics2 = (limb.Target!.Values["athletics mult 1"] is float lathleticsf1 ? lathleticsf1 : 0);
            ////        float limbathletics = (limbstealth1 + limbstealth2) / 2;

            ////        var limbCombatResult = (limbDex + limbStr + limbathletics) / 3;
            ////        var limbRangedResult = (limbDex + limbRanged + limbathletics) / 3;
            ////        var limbStealthResult = (limbDex + limbThie + limbstealth) / 3;

            ////        int relativeCombat = (int)(charCombat * limbCombatResult);
            ////        int relativeRanged = (int)(charRanged * limbRangedResult);
            ////        int relativeStealth = (int)(charStealth * limbStealthResult);

            ////        var limbRelativeChance = Math.Max(Math.Max(relativeCombat, relativeRanged), relativeStealth) / 10;

            ////        categoryReferences.Add(limb.TargetId, limbQuality, limbRelativeChance);
            ////    }
            ////}
            ///
            return Task.CompletedTask;
        }
    }
}
