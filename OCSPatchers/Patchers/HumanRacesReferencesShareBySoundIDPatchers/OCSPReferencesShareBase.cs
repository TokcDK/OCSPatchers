using System.Linq;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.ReferencesShare
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
        protected virtual List<string> ExcludedToShareReferenceIDs { get; } = new List<string>();
        protected virtual bool IsValidReferences(ModReferenceCollection? references) { return true; }

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // <Sound ID, <CategoryName, <TargetId, reference>>>
            Dictionary<int, Dictionary<string, Dictionary<string, ModReference>>> referenceCategoriesSharingRecordsBySoundIDData = new();

            GetData(context, referenceCategoriesSharingRecordsBySoundIDData);

            ShareReferencesFromData(context, referenceCategoriesSharingRecordsBySoundIDData);

            return Task.CompletedTask;
        }

        private void ShareReferencesFromData(IModContext context, Dictionary<int, Dictionary<string, Dictionary<string, ModReference>>> referenceCategoriesSharingRecordsBySoundIDData)
        {
            // add by sounds value
            var races = context.Items.OfType(ItemType.Race).Where(i => !i.IsDeleted());
            foreach (var raceModItem in races)
            {
                if (IsAnimal(raceModItem)) continue;
                if (!TryGetSoundsID(raceModItem, out int soundsID)) continue;
                if (!referenceCategoriesSharingRecordsBySoundIDData.ContainsKey(soundsID)) continue;

                var categoriesBySoundsID = referenceCategoriesSharingRecordsBySoundIDData[soundsID];
                foreach (var categoryReferencesData in categoriesBySoundsID)
                {
                    if (!raceModItem.ReferenceCategories.ContainsKey(categoryReferencesData.Key)) continue; // skip instead of add new
                    //if (!raceModItem.ReferenceCategories.ContainsKey(categoryReferences.Key)) raceModItem.ReferenceCategories.Add(categoryReferences.Key);

                    var categoryReferences = raceModItem.ReferenceCategories[categoryReferencesData.Key].References;
                    if (!IsValidReferences(categoryReferences)) continue;
                    //if (refList.Count < 2) continue; // breaking patch file!! ??? //the check must exclude races where only one unique hair

                    foreach (var modReference in categoryReferencesData.Value)
                    {
                        if (!IsValidToAdd(raceModItem, modReference.Value, categoryReferencesData.Key)) continue;
                        if (categoryReferences.ContainsKey(modReference.Value.TargetId)) continue;

                        categoryReferences.Add(new ModReference(modReference.Value));
                    }
                }
            }
        }

        private void GetData(IModContext context, Dictionary<int, Dictionary<string, Dictionary<string, ModReference>>> referenceCategoriesSharingRecordsBySoundIDData)
        {
            var races = context.Items.OfType(ItemType.Race).Where(i => !i.IsDeleted());
            foreach (var raceModItem in races)
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
                        if (!referencesByCategoryData.ContainsKey(reference.TargetId)) referencesByCategoryData.Add(reference.TargetId, reference);
                    }
                }
            }
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

            return !ExcludedToShareReferenceIDs.Contains(reference.TargetId);
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
