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
            foreach (var modItem in context.Items.OfType(ItemType.SquadTemplate))
            {
                TryAddLegendary(modItem, context);
            }

            return Task.CompletedTask;
        }

        private void TryAddLegendary(ModItem modItem, IModContext context)
        {
            if (modItem.ReferenceCategories.ContainsKey("choosefrom list")) return; // most likely already have legendary?
            if (modItem.ReferenceCategories.ContainsKey("squad")) return; // missing squad members?

            if (!TryFillLegendary(modItem, context)) return;

            modItem.Values.Add("num random chars", 1);
            modItem.Values.Add("num random chars max", 1);
        }

        private bool TryFillLegendary(ModItem modItem, IModContext context)
        {
            var listOfMembers = GetListOfValidMembers(modItem);
            modItem.ReferenceCategories.Add("choosefrom list");
            var choosefromList = modItem.ReferenceCategories["choosefrom list"];
            int addedLegs = 0;
            foreach (var chara in listOfMembers.Values)
            {
                var legCharacter = GetLegendayCharacter(chara, context);
                if (legCharacter == null) continue;

                choosefromList.References.Add(legCharacter);
                addedLegs += 1;
            }

            if (addedLegs == 0)
            {
                modItem.ReferenceCategories.RemoveByKey("choosefrom list");
                return false;
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

        readonly Dictionary<string, ModItem> _legendaryCharas = new();
        private ModItem? GetLegendayCharacter(ModItem charaModItem, IModContext context)
        {
            if (_legendaryCharas.ContainsKey(charaModItem.StringId)) return _legendaryCharas[charaModItem.StringId];

            var legendaryChara = charaModItem.DeepClone();

            legendaryChara.Values["armour upgrade chance"] = 50;

            AddLegendaryItemsVariants(legendaryChara, context);

            // reset weapon manufacturer for the character here

            return legendaryChara;
        }

        private void AddLegendaryItemsVariants(ModItem legendaryChara, IModContext context)
        {
            var validWeapons = new Dictionary<string, ModReference>();
            foreach (var weaponRef in legendaryChara.ReferenceCategories["weapons"].References)
            {
                if (weaponRef.Target == default) continue;
                if (validWeapons.ContainsKey(weaponRef.TargetId)) continue;

                validWeapons.Add(weaponRef.Target.StringId, weaponRef);
            }

            var newWeaponsList = new List<(string, int,int,int)>();
            foreach (var weaponRef in validWeapons.Values)
            {
                var legendaryWeaponsList = GetLegendaryWeapons(weaponRef.Target, context);
                if(legendaryWeaponsList.Count==0)
                {
                    newWeaponsList.Add((weaponRef.TargetId, weaponRef.Value0, weaponRef.Value1, weaponRef.Value2));
                }
                else
                {
                    foreach (var legendaryWeapon in legendaryWeaponsList)
                    {
                        newWeaponsList.Add((legendaryWeapon.StringId, weaponRef.Value0, weaponRef.Value1, weaponRef.Value2));
                    }
                }
            }

            if (newWeaponsList.Count == 0) return;

            var refs = legendaryChara.ReferenceCategories["weapons"].References;
            refs.Clear();
            foreach (var weaponToAdd in newWeaponsList)
            {
                if (refs.ContainsKey(weaponToAdd.Item1)) continue;

                refs.Add(new ModReference(weaponToAdd.Item1, weaponToAdd.Item2, weaponToAdd.Item3, weaponToAdd.Item4));
            }
        }

        readonly Dictionary<string, List<ModItem?>> _legendaryWeapons = new();
        private List<ModItem?> GetLegendaryWeapons(ModItem? weaponModItem, IModContext context)
        {
            if (_legendaryWeapons.ContainsKey(weaponModItem.StringId))
            {
                return _legendaryWeapons[weaponModItem.StringId];
            }

            foreach(var effectData in new ILegendaryItemEffect[]
            {
                new ShieldLegendaryItemEffect(),
                new SharpLegendaryItemEffect(),
            })
            {
                var weaponClone = weaponModItem.DeepClone();
                context.Items.AddFrom(weaponClone);

                if (!effectData.TryApplyEffect(weaponClone)) continue;

                weaponClone.Values["Description"] = $"#000000Это оружие имеет легендарный эффект #ff0000『{effectData.Name}』#000000, со следующими эффектами.\r\n{effectData.Description}";
            }

            return null;
        }

        readonly Dictionary<string, ModItem> _legendaryArmors = new();
        private ModItem? GetLegendaryArmor(ModItem? armorModItem)
        {
            return null;
        }
    }

    interface ILegendaryItemEffect
    {
        string Name { get; }
        string Description { get; }

        bool TryApplyEffect(ModItem? modItem);
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

        public bool TryApplyEffect(ModItem? modItem)
        {
            if (!modItem.Values.ContainsKey("defence mod")) return false;

            modItem.Values["defence mod"] = (int)modItem.Values["defence mod"] + 20;

            return true;
        }
    }

    internal class SharpLegendaryItemEffect : ILegendaryWeaponEffect
    {
        public string Name => "Острота";

        public string Description => "#afa68bРежущий урон #a8b774+20%";

        public bool TryApplyEffect(ModItem? modItem)
        {
            if (!modItem.Values.ContainsKey("cut damage multiplier")) return false;

            modItem.Values["cut damage multiplier"] = (int)modItem.Values["cut damage multiplier"] + 0.2;

            return true;
        }
    }
}
