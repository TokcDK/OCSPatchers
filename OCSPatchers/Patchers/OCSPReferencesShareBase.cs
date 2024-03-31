using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    internal abstract class OCSPReferencesShareBase : OCSPatcherBase
    {
        readonly Dictionary<string, int> _raceIDOverrides = new Dictionary<string, int>()
        {
            // 2b has human appearance
            {"10-2B.mod",0 },
            {"76-2B.mod",0 }
        };

        const string SOUNDS_VALUE_NAME = "sounds";
        protected abstract List<string> ReferenceCategoryNames { get; }

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            Dictionary<int, Dictionary<string, HashSet<ModReference>>> referenceCategoriesSharingRecordsBySoundIDData = new();

            foreach (var raceModItem in context.Items.OfType(ItemType.Race).Where(i => !i.IsDeleted()))
            {
                if (IsAnimal(raceModItem)) continue;

                // get by sounds value
                if (!TryGetSoundsID(raceModItem, out int soundsID)) continue;

                //// add some base animatio ids?
                //if (race.ReferenceCategories.ContainsKey(ANIMATIONS_REFERENCE_CATEGORY_NAME))
                //    race.ReferenceCategories.Add(ANIMATIONS_REFERENCE_CATEGORY_NAME);
                //var animFiles = race.ReferenceCategories[ANIMATIONS_REFERENCE_CATEGORY_NAME];
                //foreach (var animRef in _animStrIDs)
                //{
                //    if (!animFiles.References.ContainsKey(animRef)) animFiles.References.Add(animRef);
                //}

                referenceCategoriesSharingRecordsBySoundIDData.TryAdd(soundsID, new());
                var referencesCategoryBySoundIDData = referenceCategoriesSharingRecordsBySoundIDData[soundsID];

                foreach (var referenceCategoryName in ReferenceCategoryNames)
                {
                    if (!raceModItem.ReferenceCategories.ContainsKey(referenceCategoryName)) continue;

                    referencesCategoryBySoundIDData.TryAdd(referenceCategoryName, new());

                    var referencesByCategoryData = referencesCategoryBySoundIDData[referenceCategoryName];
                    var raceCategoryReferences = raceModItem.ReferenceCategories[referenceCategoryName].References;
                    foreach (var reference in raceCategoryReferences)
                    {
                        if (!referencesByCategoryData.Contains(reference)) referencesByCategoryData.Add(reference);
                    }
                }
            }

            // hairs beards heads animations ADD by sounds value
            foreach (var race in context.Items.OfType(ItemType.Race).Where(i => !i.IsDeleted()))
            {
                if (!TryGetSoundsID(race, out int soundsID)) continue;

                if (!referenceCategoriesSharingRecordsBySoundIDData.ContainsKey(soundsID)) continue;

                if (IsAnimal(race)) continue;

                var itemsBySoundsID = referenceCategoriesSharingRecordsBySoundIDData[soundsID];
                foreach (var categoryReferences in itemsBySoundsID)
                {
                    if (!race.ReferenceCategories.ContainsKey(categoryReferences.Key)) race.ReferenceCategories.Add(categoryReferences.Key);

                    var refList = race.ReferenceCategories[categoryReferences.Key].References;
                    //if (refList.Count < 2) continue; // breaking patch file!! ??? //the check must exclude races where only one unique hair
                    //if (refList.Count < 2) continue; // breaking patch file!! ??? //the check must exclude races where only one unique hair

                    foreach (var reference in categoryReferences.Value)
                    {
                        if (!IsValidToAdd(race, reference, categoryReferences.Key)
                            || refList.ContainsKey(reference.TargetId)) continue;

                        refList.Add(reference.TargetId, GetVal0(race, categoryReferences.Key), GetVal1(race, categoryReferences.Key), GetVal2(race, categoryReferences.Key));
                    }
                }
            }
            return Task.CompletedTask;
        }


        private bool TryGetSoundsID(ModItem race, out int soundsID)
        {
            soundsID = -1;

            if (!race.Values.ContainsKey(SOUNDS_VALUE_NAME)) return false;
            if (race.Values[SOUNDS_VALUE_NAME] is not int sID) return false;
            soundsID = getRaceSoundsID(race, sID);

            return true;
        }

        private bool IsAnimal(ModItem race)
        {
            if (!race.Values.TryGetValue("male mesh", out var meshValue)
                    || meshValue is not FileValue meshFile
                    || string.IsNullOrEmpty(meshFile.Path)
                    || meshFile.Path.Contains(@"\animal\") // exclude animal
                    )
            {
                return true;
            }

            return false;
        }

        int GetVal2(ModItem race, string categoryName)
        {
            return 0;
        }

        int GetVal1(ModItem race, string categoryName)
        {
            return 0;
        }

        int GetVal0(ModItem race, string categoryName)
        {
            return 0;
        }

        bool IsValidToAdd(ModItem race, ModReference reference, string categoryName)
        {
            //if (categoryName == "robot limbs" && race.Values.ContainsKey("unique") && race.Values["unique"] is bool b && b)
            //{
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// some races like in 2b mod using human appearance attachments
        /// </summary>
        int getRaceSoundsID(ModItem race, int inputSoundsID)
        {
            if (_raceIDOverrides!.ContainsKey(race.StringId)) return _raceIDOverrides[race.StringId];

            return inputSoundsID;
        }
    }
}
