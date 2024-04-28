using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP
{
    internal class OCSPWeaponBlueprintsDistribute : OCSPatcherBase
    {
        public override string PatcherName => "HolyNationRacismFix";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // get all weapon ids
            var allWeaponStringIds = new Dictionary<string, ModItem>();
            var items = context.Items.OfType(ItemType.Weapon);
            foreach (var item in items)
            {
                if (!item.Name.StartsWith("_")) continue; // skip type
                if (!item.Name.StartsWith("@")) continue;

                if (!item.Values.ContainsKey("slot")) continue;
                if (item.Values["slot"] is not int slotNum) continue;
                if (slotNum != 14) continue; // ATTACH_WEAPON slot index

                if (allWeaponStringIds.ContainsKey(item.StringId)) continue;

                allWeaponStringIds.Add(item.StringId, item);
            }

            //get all weapons and armors from research items
            var allWeaponStringIdsHavingBluePrint = new Dictionary<string, ModItem>();
            items = context.Items.OfType(ItemType.Research);
            foreach (var item in items)
            {
                // need blueprint only items
                if (!item.Values.ContainsKey("blueprint only")) continue;
                if (item.Values["blueprint only"] is not bool isBluePrintOnly) continue;
                if (!isBluePrintOnly) continue;

                if (!item.ReferenceCategories.ContainsKey("enable weapon type")) continue; //is not weapon blueprint

                var refs = item.ReferenceCategories["enable weapon type"].References;
                foreach (var refItem in refs)
                {
                    if (refItem.Target == null) continue;
                    if (allWeaponStringIdsHavingBluePrint.ContainsKey(refItem.Target.StringId)) continue;

                    allWeaponStringIdsHavingBluePrint.Add(refItem.Target.StringId, refItem.Target);
                }
            }

            var templateResearchItem = context.Items.First(i=>i.StringId == "2111-gamedata.base");
            foreach(var itemData in allWeaponStringIds)
            {
                if (allWeaponStringIdsHavingBluePrint.ContainsKey(itemData.Key)) continue; // already have blueprint

                var item = itemData.Value;

                // here create new research item and set data based on item's values: name,money,requirements, level
                var newItem = new ModItem(templateResearchItem);
                newItem.Name = item.Name;
                newItem.Values["money"] = (int)item.Values["money"] * 10;

            }

            return Task.CompletedTask;
        }
    }
}
