using OCSPatchers.Patchers.LegendaryNPCItemsPatcher.Extensions;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters;
using static OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters.OCSPLegendaryNPCItems;
using System.Linq;
using System;

namespace OCSPatchers.Patchers
{
    internal partial class OCSPLegendaryNPCItems : OCSPatcherBase
    {
        bool isLegendaryManufacturerSet = false; // determine if manufacturer is set
        ModItem? _legendaryWeaponManufacturer; // manufacturer item reference

        // cache of added items for repeat using by original stringId
        readonly Dictionary<string, ModItem> _cacheOfAddedLegendaryCharasByOrigin = new();
        readonly Dictionary<string, ModItem> _addedLegendaryCharaStatsItemsCache = new();

        public override string PatcherName => "Add legendary characters and items";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            MakeWeaponManufacturer(context); // add first to make it always with first and second ids in patch mod

            ParseSquadTemplates(context);

            RemoveWeaponManufacturerIfNoLegendariesAdded(context);

            return Task.CompletedTask;
        }

        #region SquadSetup
        private void ParseSquadTemplates(IModContext context)
        {
            SaveReferencedSquads(context);

            foreach (var modItem in context.Items.OfType(ItemType.SquadTemplate).ToArray()) // to array because will be added new items and for enumerable will error
            {
                if (!IsValidSquadItem(modItem)) continue;

                TryAddLegendaryToTheSquad(modItem, context);
            }
        }

        readonly HashSet<string> _referencedSquads = new();
        private void SaveReferencedSquads(IModContext context)
        {
            var categoryNames = new string[2]
            {
                "bar squads",
                "residents",
            };

            foreach (var modItem in context.Items.OfType(ItemType.Town))
            {
                if (modItem.IsDeleted()) continue;

                foreach (var categoryName in categoryNames)
                {
                    if (!modItem.ReferenceCategories.ContainsKey(categoryName)) continue;

                    foreach (var squadRef in modItem.ReferenceCategories[categoryName].References)
                    {
                        if (squadRef.Target == null) continue;
                        if (_referencedSquads.Contains(squadRef.TargetId)) continue;

                        _referencedSquads.Add(squadRef.TargetId);
                    }
                }
            }
        }

        private bool IsValidSquadItem(ModItem modItem)
        {
            if (!IsValidModItem(modItem)) return false;

            // required some items to exist for the squad to be valid
            if (!modItem.ReferenceCategories.ContainsKey("AI packages")) return false; // behavour is not set, not referenced, deleted but partially set by some mod?
            if (!modItem.Values.ContainsKey("force speed")) return false; // 

            // squad must be referenced by town to be used
            if (!_referencedSquads.Contains(modItem.StringId)) return false;

            return true;
        }

        private void TryAddLegendaryToTheSquad(ModItem modItem, IModContext context)
        {
            //if (modItem.ReferenceCategories.ContainsKey("choosefrom list")) return; // most likely already have legendary?
            if (!modItem.ReferenceCategories.ContainsKey("squad")) return; // missing squad members?

            if (!TryAddLegendaryCharacters(modItem, context)) return;

            if (!modItem.Values.TryGetValue("num random chars max", out var minValue) || (minValue is int min && min == 0))
            {
                modItem.Values["num random chars"] = 0;
            }
            if (!modItem.Values.TryGetValue("num random chars max", out var maxValue) || (minValue is int max && max == 0))
            {
                modItem.Values["num random chars max"] = 1;
            }
        } 
        #endregion

        #region SafeChecks
        public bool IsValidModItem(ModItem modItem)
        {
            return PatcherExtensions.IsValidModItem(modItem);
        }

        public bool IsValidItemName(ModItem characterItem)
        {
            return PatcherExtensions.IsValidItemName(characterItem);
        }
        #endregion


        #region CharacterSetup

        public bool IsValidCharacter(ModItem modItem)
        {
            if (modItem.Values.TryGetValue("unique", out var v) && v is bool isUnique && isUnique)
            {
                return false; // skip unique
            }
            if (modItem.ReferenceCategories.ContainsKey("vendors")
                && modItem.ReferenceCategories["vendors"].References.Count > 0)
                return false; // skip traders

            if (!IsValidModItem(modItem)) return false;

            return true;
        }
        private bool TryAddLegendaryCharacters(ModItem modItem, IModContext context)
        {
            var listOfMembers = GetListOfValidMembers(modItem);
            if (!modItem.ReferenceCategories.ContainsKey("choosefrom list"))
                modItem.ReferenceCategories.Add("choosefrom list");

            var choosefromList = modItem.ReferenceCategories["choosefrom list"];
            int addedLegs = 0;
            foreach (var chara in listOfMembers.Values)
            {
                if (!IsValidCharacter(chara)) continue;

                var legCharacter = GetLegendayCharacter(chara, context);
                if (legCharacter == null) continue;

                bool isExistChara = listOfMembers.ContainsKey(legCharacter.StringId);

                choosefromList.References.Add(legCharacter, isExistChara ? 30 : 1); // 30% chance for usual extra chars and 1% for legendaries
                addedLegs += 1;
            }

            if (addedLegs == 0 && choosefromList.References.Count == 0)
            {
                modItem.ReferenceCategories.RemoveByKey("choosefrom list"); // remove empty list where was not added any char
                return false;
            }

            foreach(var chara in listOfMembers.Values)
            {
                if (!IsValidCharacter(chara)) continue;

                // add original characters to prevent legendary appear in mos cases
                if (choosefromList.References.ContainsKey(chara.StringId)) continue;

                choosefromList.References.Add(chara, 40);
            }

            return true;
        }

        private Dictionary<string, ModItem> GetListOfValidMembers(ModItem modItem)
        {
            var listOfMembers = new Dictionary<string, ModItem>();

            foreach (var modItemRef in modItem.ReferenceCategories["squad"].References)
            {
                if (modItemRef.Target == default) continue;
                if (listOfMembers.ContainsKey(modItemRef.Target.StringId)) continue;
                if (modItemRef.Target.Values["unique"] is bool isUnique && isUnique) continue;

                listOfMembers.Add(modItemRef.Target.StringId, modItemRef.Target);
            }

            return listOfMembers;
        }

        private ModItem? GetLegendayCharacter(ModItem charaModItem, IModContext context)
        {
            if (_cacheOfAddedLegendaryCharasByOrigin.ContainsKey(charaModItem.StringId)) return _cacheOfAddedLegendaryCharasByOrigin[charaModItem.StringId];

            var legendaryCharaCandidate = charaModItem.DeepClone();

            if (!AddLegendaryItemsVariants(legendaryCharaCandidate, context)) return null;

            var legendaryChara = context.NewItem(legendaryCharaCandidate); // add only when legendary weapons was added
            legendaryChara.Values["armour upgrade chance"] = 80;

            if (legendaryChara.Values.TryGetValue("named", out var v) && v is bool isNamed && isNamed)
            {
            }
            else legendaryChara.Name = "#ff0002\"Легендарн/аяый1/\" " + legendaryChara.Name;

            // reset weapon manufacturer for the character here, maybe apply here mods for weapons manufacturer, maybe add different manufacturers
            ReSetWeaponManufacturer(legendaryChara, context);

            EnforceStats(legendaryChara, context);

            _cacheOfAddedLegendaryCharasByOrigin.Add(charaModItem.StringId, legendaryChara);

            return legendaryChara;
        }

        private void EnforceStats(ModItem legendaryChara, IModContext context)
        {
            if (TrySetStatsByReferencedStats(legendaryChara, context)) return;

            EnforceStatsByValues(legendaryChara);
        }

        private bool TrySetStatsByReferencedStats(ModItem legendaryChara, IModContext context)
        {
            if (!legendaryChara.ReferenceCategories.ContainsKey("stats")) return false;
            var refs = legendaryChara.ReferenceCategories["stats"].References;
            if (refs.Count == 0) return false;
            var referencedStats = refs.First();
            if (referencedStats.Target == null) return false;


            if (_addedLegendaryCharaStatsItemsCache.ContainsKey(referencedStats.Target.StringId))
            {
                refs.Clear();

                var refStats = _addedLegendaryCharaStatsItemsCache[referencedStats.Target.StringId];
                var newModRef = new ModReference(refStats.StringId);
                refs.Add(newModRef);

                return true;
            }


            var stats = context.NewItem(referencedStats.Target.DeepClone());
            EnforceByReferencedStats(stats);
            refs.Clear();
            var newModRef1 = new ModReference(stats.StringId);
            refs.Add(newModRef1);
            _addedLegendaryCharaStatsItemsCache.Add(referencedStats.Target.StringId, stats);

            return true;
        }

        private void EnforceStatsByValues(ModItem legendaryChara)
        {
            var values = legendaryChara.Values;
            foreach (var keyName in new string[]
            {
                "combat stats",
                "ranged stats",
                "stealth stats",
                "strength",
                "unarmed stats",
            })
            {
                int value = Convert.ToInt32(values[keyName]);
                values[keyName] = (int)Math.Ceiling(GetNewStatValue(value));
            }

        }

        private float GetNewStatValue(float i)
        {
            float v1 = (float)(i * 1.5);
            float newValue = v1 > 100 ? 100 : v1 < 30 ? 30 : v1;
            return newValue;
        }

        private void EnforceByReferencedStats(ModItem stats)
        {
            var keys = stats.Values.Keys.Select(v => v).ToArray();
            foreach (var key in keys)
            {
                var o = stats.Values[key];
                if (o is not float && o is not int) continue; // can be other type value

                float value = Convert.ToSingle(o);

                stats.Values[key] = GetNewStatValue(value);
            }
        }

        private void ReSetWeaponManufacturer(ModItem legendaryChara, IModContext context)
        {
            MakeWeaponManufacturer(context);

            if (!legendaryChara.ReferenceCategories.ContainsKey("weapon level"))
            {
                legendaryChara.ReferenceCategories.Add(new ModReferenceCategory("weapon level"));
            }
            var weaponLevelReference = legendaryChara.ReferenceCategories["weapon level"].References;
            weaponLevelReference.Clear();
            weaponLevelReference.Add(new ModReference(_legendaryWeaponManufacturer!.StringId));
        }

        private void RemoveWeaponManufacturerIfNoLegendariesAdded(IModContext context)
        {
            if (_cacheOfAddedLegendaryCharasByOrigin.Count > 0) return;

            var model = _legendaryWeaponManufacturer!.ReferenceCategories["weapon models"].References.First();
            context.Items.RemoveByKey(model.Target!.StringId); // remove model
            context.Items.RemoveByKey(_legendaryWeaponManufacturer.StringId); // remove manufacturer
        }

        private void MakeWeaponManufacturer(IModContext context)
        {
            if (isLegendaryManufacturerSet) return;

            //52293-rebirth.mod meitou model
            //52288-rebirth.mod meitou manufacturer
            var crestManufacturer = context.Items.OfType(ItemType.WeaponManufacturer).First(i => i.StringId == "52288-rebirth.mod");
            _legendaryWeaponManufacturer = crestManufacturer.DeepClone();
            _legendaryWeaponManufacturer.Name = "Легендарный кузнец";
            _legendaryWeaponManufacturer.Values["company description"] = "Выкованное однажды оружие неизвестным легендарным кузнецом.";
            _legendaryWeaponManufacturer.Values["cut damage mod"] = (float)1.08;
            _legendaryWeaponManufacturer.Values["price mod"] = (float)1.8;
            if (_legendaryWeaponManufacturer.ReferenceCategories.ContainsKey("weapon types"))
                _legendaryWeaponManufacturer.ReferenceCategories["weapon types"].References.Clear(); // can be values
            var weaponModels = _legendaryWeaponManufacturer.ReferenceCategories["weapon models"];
            weaponModels.References.Clear();

            // set legendary model
            var meitouWeaponModels = context.Items.OfType(ItemType.MaterialSpecsWeapon).First(i => i.StringId == "52293-rebirth.mod");
            var legendaryModel = meitouWeaponModels.DeepClone();
            legendaryModel.Values["description"] = "Легендарное снаряжение обладает особыми эффектами.";
            legendaryModel.Name = "Легендарное оружие";
            legendaryModel = context.NewItem(legendaryModel);


            weaponModels.References.Add(new ModReference(legendaryModel.StringId, 80, 100));
            _legendaryWeaponManufacturer = context.NewItem(_legendaryWeaponManufacturer);

            isLegendaryManufacturerSet = true;
        }
        #endregion


        #region items
        readonly LegendaryItemEffectApplyBase _weaponsApply = new LegendaryItemEffectApplyWeapons();
        readonly LegendaryItemEffectApplyBase _clothingApply = new LegendaryItemEffectApplyClothing();
        private bool AddLegendaryItemsVariants(ModItem legendaryChara, IModContext context)
        {
            bool ret1 = _weaponsApply.TryGetItems(legendaryChara, context);
            bool ret2 = _clothingApply.TryGetItems(legendaryChara, context);

            return ret1 || ret2;
        } 
        #endregion
    }
}
