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

namespace OCSPatchers.Patchers.Tweaks
{
    internal class OCSPStackableItems1000 : OCSPatcherBase
    {
        public override string PatcherName => "Stacked items 1000";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            foreach (var item in context.Items.OfType(ItemType.Item))
            {
                TryParseAsStackableItem(item);
            }
            foreach (var item in context.Items.OfType(ItemType.Building))
            {
                TryParseAsStackableItem(item);
            }
            foreach (var item in context.Items.OfType(ItemType.Container))
            {
                TryParseAsStackableItem(item);
            }
            return Task.CompletedTask;
        }

        private bool TryParseAsStackingStorage(ModItem item)
        {
            if (!item.Values.ContainsKey("stackable bonus mult")) return false;

            if (item.Values.ContainsKey("stackable bonus minimum"))
            {
                item.Values["stackable bonus minimum"] = 100;
                item.Values["stackable bonus mult"] = 100;
            }
            else
            {
                item.Values["stackable bonus mult"] = 1000;
            }

            return true;
        }

        private bool TryParseAsStackableItem(ModItem item)
        {
            if(item.IsDeleted()) return false;
            if(string.IsNullOrWhiteSpace(item.Name)) return false;
            if(!item.Values.ContainsKey("itemtype limit") // building
                && !item.Values.ContainsKey("value") // item
                ) return false;

            TryParseAsStackingStorage(item);

            if (!item.Values.TryGetValue("stackable", out var value)) return false;
            if (!item.Values.TryGetValue("slot", out var slotValue)) return false;
            if (slotValue is not int i || i != 7) return false; // attach slot none
            if (value is not int v || v >= 1000) return false; // already max

            //Console.WriteLine("Updating " + item.Name);
            item.Values["stackable"] = 1000;

            return true;
        }
    }
}
