using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication.ExtendedProtection;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP
{
    internal class OCSPLegendaryNPCItems : OCSPatcherBase
    {
        public override string PatcherName => "Add legendary characters and items";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            foreach (var modItem in context.Items.OfType(ItemType.SquadTemplate).ToArray()) // to array because will be added new items and for enumerable will error
            {
                if (!IsValidSquadItem(modItem)) continue;
                //if (modItem.ReferenceCategories.Count == 0) continue;

                //var clone = modItem.DeepClone();

                //context.NewItem(clone);

                TryAddLegendaryToTheSquad(modItem, context);
                //break;
            }

            return Task.CompletedTask;
        }
        private bool IsValidSquadItem(ModItem modItem)
        {
            if (!IsValidModItem(modItem)) return false;
            if (!modItem.ReferenceCategories.ContainsKey("AI packages")) return false; // behavour is not set, not referenced, deleted but partially set by some mod?

            return true;
        }

        private void TryAddLegendaryToTheSquad(ModItem modItem, IModContext context)
        {
            //if (modItem.ReferenceCategories.ContainsKey("choosefrom list")) return; // most likely already have legendary?
            if (!modItem.ReferenceCategories.ContainsKey("squad")) return; // missing squad members?

            if (!TryAddLegendaryCharacters(modItem, context)) return;

            modItem.Values.TryAdd("num random chars", 1);
            modItem.Values.TryAdd("num random chars max", 1);
        }

        private bool TryAddLegendaryCharacters(ModItem modItem, IModContext context)
        {
            var listOfMembers = GetListOfValidMembers(modItem);
            if(!modItem.ReferenceCategories.ContainsKey("choosefrom list")) 
                modItem.ReferenceCategories.Add("choosefrom list");

            var choosefromList = modItem.ReferenceCategories["choosefrom list"];
            int addedLegs = 0;
            foreach (var chara in listOfMembers.Values)
            {
                if(!IsValidCharacter(chara)) continue;

                if (chara.Values.TryGetValue("unique", out var v) && v is bool isUnique && isUnique)
                {
                    continue; // skip unique
                }

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
                var modRef = new ModReference(chara.StringId, 36);
                choosefromList.References.Add(modRef);
            }

            return true;
        }

        private bool IsValidModItem(ModItem modItem)
        {
            if (!IsValidItemName(modItem)) return false;
            if (modItem.IsDeleted()) return false;
            if (modItem.StringId.Contains("CL Legendary")) return false; // do not touch from legendary equipment mod

            return true;
        }

        private bool IsValidCharacter(ModItem modItem)
        {
            if (!IsValidModItem(modItem)) return false;

            return true;
        }

        private bool IsValidItemName(ModItem characterItem)
        {
            if (characterItem.Name.StartsWith("_")) return false;
            if (characterItem.Name.StartsWith("@")) return false;
            if (characterItem.Name.StartsWith("#")) return false;

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
                
        readonly Dictionary<string, ModItem> _cacheOfAddedLegendaryCharasByOrigin = new();
        private ModItem? GetLegendayCharacter(ModItem charaModItem, IModContext context)
        {
            if (_cacheOfAddedLegendaryCharasByOrigin.ContainsKey(charaModItem.StringId)) return _cacheOfAddedLegendaryCharasByOrigin[charaModItem.StringId];

            var legendaryCharaCandidate = charaModItem.DeepClone();

            if (!AddLegendaryItemsVariants(legendaryCharaCandidate, context)) return null;

            var legendaryChara = context.NewItem(legendaryCharaCandidate); // add only when legendary weapons was added
            legendaryChara.Values["armour upgrade chance"] = 50;
            legendaryChara.Name = ("#ff0002\"Легендарн/аяый1/\" " + legendaryChara.Name);

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

        Dictionary<string, ModItem> _addedLegendaryCharaStatsItemsCache = new();
        private bool TrySetStatsByReferencedStats(ModItem legendaryChara, IModContext context)
        {
            if (!legendaryChara.ReferenceCategories.ContainsKey("stats")) return false;
            var refs = legendaryChara.ReferenceCategories["stats"].References;
            if (refs.Count == 0) return false;
            var referencedStats = refs.First();
            if(referencedStats.Target == null) return false;


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
            foreach(var s in new string[] 
            { 
                "combat stats",
                "ranged stats",
                "stealth stats",
                "strength",
                "unarmed stats",
            })
            {
                if (!legendaryChara.Values.TryGetValue(s, out var v) || v is not int i || i >= 100) continue;

                legendaryChara.Values[s] = GetNewIntStatValue(i);
            }

        }

        private object GetNewIntStatValue(int i)
        {
            int v1 = (int)(i * 1.5);
            int newValue = v1 > 100 ? 100 : v1 < 30 ? 30 : v1;
            return newValue;
        }

        private void EnforceByReferencedStats(ModItem stats)
        {
            foreach (var s in stats.Values)
            {
                if (s.Value is not int i || i >= 100) continue;

                stats.Values[s.Key] = GetNewIntStatValue(i);
            }
        }

        bool isLegendaryManufacturerSet = false;
        ModItem? _legendaryWeaponManufacturer;
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

        private void MakeWeaponManufacturer(IModContext context)
        {
            if (isLegendaryManufacturerSet) return;

            //52293-rebirth.mod meitou model
            //52288-rebirth.mod meitou manufacturer
            var crestManufacturer = context.Items.OfType(ItemType.WeaponManufacturer).First(i=>i.StringId== "52288-rebirth.mod");
            _legendaryWeaponManufacturer = crestManufacturer.DeepClone();
            _legendaryWeaponManufacturer.Name = "Легендарный кузнец";
            _legendaryWeaponManufacturer.Values["company description"] = (string)"Выкованное однажды оружие неизвестным легендарным кузнецом.";
            _legendaryWeaponManufacturer.Values["cut damage mod"] = (float)1.08;
            _legendaryWeaponManufacturer.Values["price mod"] = (float)1.8;
            if(_legendaryWeaponManufacturer.ReferenceCategories.ContainsKey("weapon types")) 
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

        private bool AddLegendaryItemsVariants(ModItem legendaryChara, IModContext context)
        {
            if (!legendaryChara.ReferenceCategories.ContainsKey("weapons")) return false;

            var validWeapons = new Dictionary<string, ModReference>();

            var weaponsCategory = legendaryChara.ReferenceCategories["weapons"];
            var weaponsRefs = weaponsCategory.References;
            foreach (var weaponRef in weaponsRefs)
            {
                //var wRef = context.Items.OfType(ItemType.Weapon).First(i => i.StringId == weaponRef.TargetId);

                if (weaponRef.Target == default) continue;
                if (!IsValidModItem(weaponRef.Target)) continue;
                if (validWeapons.ContainsKey(weaponRef.TargetId)) continue;

                validWeapons.Add(weaponRef.Target.StringId, weaponRef);
            }

            var newWeaponsList = new List<(string, int,int,int)>();
            foreach (var weaponRef in validWeapons.Values)
            {
                var legendaryWeaponsList = GetLegendaryWeapons(weaponRef.Target, context);
                if(legendaryWeaponsList == null || legendaryWeaponsList.Count==0)
                {
                    continue;
                }

                foreach (var legendaryWeapon in legendaryWeaponsList)
                {
                    newWeaponsList.Add((legendaryWeapon.StringId, weaponRef.Value0, weaponRef.Value1, weaponRef.Value2));
                }
            }

            if (newWeaponsList.Count == 0) return false;

            weaponsRefs.Clear();
            foreach (var weaponToAdd in newWeaponsList)
            {
                if (weaponsRefs.ContainsKey(weaponToAdd.Item1)) continue;

                weaponsRefs.Add(new ModReference(weaponToAdd.Item1, weaponToAdd.Item2, weaponToAdd.Item3, weaponToAdd.Item4));
            }

            return true;
        }

        readonly Dictionary<string, List<ModItem>> _cacheOfAddedLegendaryWeaponsByOrigin = new();
        private List<ModItem> GetLegendaryWeapons(ModItem? weaponModItem, IModContext context)
        {
            if (weaponModItem!.StringId.Contains("CL Legendary")) return new List<ModItem>();

            if (_cacheOfAddedLegendaryWeaponsByOrigin.ContainsKey(weaponModItem.StringId))
            {
                return _cacheOfAddedLegendaryWeaponsByOrigin[weaponModItem.StringId]; // already made legendaries
            }

            var addedLegendaryWeaponsListByOrigin = _cacheOfAddedLegendaryWeaponsByOrigin.ContainsKey(weaponModItem.StringId) ? _cacheOfAddedLegendaryWeaponsByOrigin[weaponModItem.StringId] : new List<ModItem>();

            var legendaryWeapons = new List<ModItem>();
            foreach (var effectData in new ILegendaryItemEffect[]
            {
                new ShieldLegendaryItemEffect(),
                new SharpLegendaryItemEffect(),
            })
            {
                var legendaryWeaponCandidate = weaponModItem.DeepClone(); // create temp copy for mod

                if (!effectData.TryApplyEffect(legendaryWeaponCandidate)) continue;

                legendaryWeaponCandidate.Values["description"] = $"#000000Это оружие имеет легендарный эффект \"#ff0000{effectData.Name}#000000\", со следующими эффектами.\r\n{effectData.Description}";
                legendaryWeaponCandidate.Name += $" \"#ff0000{effectData.Name}#000000\"";

                var legendaryWeapon = context.NewItem(legendaryWeaponCandidate); // add as new only when the mod was applied

                addedLegendaryWeaponsListByOrigin.Add(legendaryWeapon); // add to prevent making variants for the same weapons many times

                legendaryWeapons.Add(legendaryWeapon);
            }
            if (legendaryWeapons.Count > 0)
            {
                _cacheOfAddedLegendaryWeaponsByOrigin.Add(weaponModItem!.StringId, legendaryWeapons);
            }

            return legendaryWeapons;
        }

        readonly Dictionary<string, ModItem> _legendaryArmors = new();
        private ModItem? GetLegendaryArmor(ModItem armorModItem)
        {
            return null;
        }
    }

    interface ILegendaryItemEffect
    {
        string Name { get; }
        string Description { get; }

        bool TryApplyEffect(ModItem modItem);
    }
    interface ILegendaryWeaponEffect : ILegendaryItemEffect
    {
    }
    interface ILegendaryArmorEffect : ILegendaryItemEffect
    {
    }

    internal class ShieldLegendaryItemEffect : ILegendaryWeaponEffect
    {
        public string Name => "Щит";

        public string Description => "#afa68bЗащита #a8b774+20";

        const string KEY_NAME = "defence mod";

        public bool TryApplyEffect(ModItem modItem)
        {
            if (!modItem.Values.ContainsKey(KEY_NAME)) return false;
            if (modItem.Values[KEY_NAME] is not int originValue) return false;

            modItem.Values[KEY_NAME] = originValue + 20;

            return true;
        }
    }

    internal class SharpLegendaryItemEffect : ILegendaryWeaponEffect
    {
        public string Name => "Острота";

        public string Description => "#afa68bРежущий урон #a8b774+20%";

        const string KEY_NAME = "cut damage multiplier";

        public bool TryApplyEffect(ModItem modItem)
        {
            if (!modItem.Values.ContainsKey(KEY_NAME)) return false;
            if (modItem.Values[KEY_NAME] is not float originValue) return false;

            modItem.Values[KEY_NAME] = originValue + (float)0.2;

            return true;
        }
    }
}
