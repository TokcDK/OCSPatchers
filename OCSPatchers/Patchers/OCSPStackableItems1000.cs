using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    internal class OCSPStackableItems1000 : OCSPatcherBase
    {
        public override string PatcherName => throw new NotImplementedException();

        public override void ApplyPatch(IModContext context, IInstallation installation)
        {
            var items = context.Items.OfType(ItemType.Item);
            foreach (var item in items)
            {
                if (!item.Values.TryGetValue("stackable", out var value)) continue;
                if (!item.Values.TryGetValue("slot", out var slotValue)) continue;
                if (slotValue is not int i || i != 7) continue; // attach slot none
                if (value is not int v || v >= 1000) continue; // already max

                Console.WriteLine("Updating " + item.Name);
                item.Values["stackable"] = 1000;
            }
        }
    }
}
