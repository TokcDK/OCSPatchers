using OpenConstructionSet.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCSPatchers.Patchers.LegendaryNPCItemsPatcher.Extensions
{
    public static class PatcherExtensions
    {
        public static bool IsValidModItem(ModItem modItem)
        {
            if (!IsValidItemName(modItem)) return false;
            if (modItem.IsDeleted()) return false;
            if (modItem.StringId.Contains("CL Legendary")) return false; // do not touch from legendary equipment mod

            return true;
        }

        public static bool IsValidItemName(ModItem characterItem)
        {
            if (characterItem.Name.StartsWith("_")) return false;
            if (characterItem.Name.StartsWith("@")) return false;
            if (characterItem.Name.StartsWith("#")) return false;

            return true;
        }
    }
}
