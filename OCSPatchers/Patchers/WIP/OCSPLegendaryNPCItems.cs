using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using static OCSPatchers.Data.Enums;

namespace OCSPatchers.Patchers.WIP
{
    internal class OCSPLegendaryNPCItems : OCSPatcherBase
    {
        public override string PatcherName => "Add legendary characters and items";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var legendaryCharas = new Dictionary<string, ModItem>();

            foreach (var modItem in context.Items.OfType(ItemType.SquadTemplate))
            {
                TryAddLegendary(modItem, legendaryCharas);
            }

            return Task.CompletedTask;
        }

        private void TryAddLegendary(ModItem modItem, Dictionary<string, ModItem> legendaryCharas)
        {
            if (modItem.ReferenceCategories.ContainsKey("choosefrom list")) return; // most likely already have legendary?
            if (modItem.ReferenceCategories.ContainsKey("squad")) return; // missing squad members?

            if(!TryFillLegendary(modItem, legendaryCharas)) return;

            modItem.Values.Add("num random chars", 1);
            modItem.Values.Add("num random chars max", 1);
        }

        private bool TryFillLegendary(ModItem modItem, Dictionary<string, ModItem> legendaryCharas)
        {
            var listOfMembers = GetListOfValidMembers(modItem);
            modItem.ReferenceCategories.Add("choosefrom list");
            var choosefromList = modItem.ReferenceCategories["choosefrom list"];
            int addedLegs = 0;
            foreach (var chara in listOfMembers.Values)
            {
                var legCharacter = GetLegendayCharacter(chara, legendaryCharas);
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

        private ModItem GetLegendayCharacter(ModItem chara, Dictionary<string, ModItem> legendaryCharas)
        {
            if (legendaryCharas.ContainsKey(chara.StringId)) return legendaryCharas[chara.StringId];

            var legendaryChara = chara.DeepClone();

            legendaryChara.Values["armour upgrade chance"] = 50;

            return legendaryChara;
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
