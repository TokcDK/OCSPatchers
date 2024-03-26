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

namespace OCSPatchers.Patchers
{
    internal class OCSPStackableItems1000 : OCSPatcherBase
    {
        public override string PatcherName => "Stacked items 1000";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var items = context.Items.OfType(ItemType.Item);
            foreach (var item in items)
            {
                if (!CheckAsItem(item)) 
                    CheckAsStorage(item);
            }
            return Task.CompletedTask;
        }

        private void CheckAsStorage(ModItem item)
        {
            if (!item.Values.ContainsKey("stackable bonus mult")) return;

            item.Values["stackable bonus mult"] = 1000;
        }

        private bool CheckAsItem(ModItem item)
        {
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
