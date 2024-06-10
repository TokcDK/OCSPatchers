using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.ReferencesShare
{
    internal class OCSPHairsShare : OCSPReferencesShareBase
    {
        public override string PatcherName => "Hairs share between races";

        const string HAIRS_REFERENCE_CATEGORY_NAME = "hairs";
        const string HAIR_COLORS_REFERENCE_CATEGORY_NAME = "hair colors";
        protected override List<string> ReferenceCategoryNames => new() { HAIRS_REFERENCE_CATEGORY_NAME, HAIR_COLORS_REFERENCE_CATEGORY_NAME };

        protected override bool IsValidReferences(ModReferenceCollection? references)
        {
            return references != null && !references.IsReadOnly && references.Count > 2;
        }
        protected override bool IsValidToAdd(ModItem race, ModReference reference, string categoryName)
        {
            return base.IsValidToAdd(race, reference, categoryName) && IsValidHair(reference, categoryName);
        }

        readonly List<string> _excludeContains = new()
        {
            "ORC-hair",
            "ORC-beard",
            "Land Bat",
        };
        private bool IsValidHair(ModReference reference, string categoryName)
        {
            return (categoryName != HAIRS_REFERENCE_CATEGORY_NAME 
                || 
                (!_excludeContains.Any(i=>reference.Target.Name.Contains(i,StringComparison.InvariantCultureIgnoreCase)) 
                && _hairUsings[reference.TargetId] > 2)); // more of 2 usings
        }

        Dictionary<string,int> _hairUsings = new Dictionary<string,int>();
        protected override void EarlyPreProcess(IModContext context)
        {
            foreach (var raceModItem in context.Items.OfType(ItemType.Race).Where(i => !i.IsDeleted()))
            {
                if (!raceModItem.ReferenceCategories.ContainsKey("hairs"))
                {
                    continue;
                }

                foreach(var hairReference in raceModItem.ReferenceCategories["hairs"].References)
                {
                    if (_hairUsings.ContainsKey(hairReference.TargetId))
                    {
                        _hairUsings[hairReference.TargetId]++;
                    }
                    else
                    {
                        _hairUsings.Add(hairReference.TargetId, 1);
                    }
                }
            }
        }
    }
}
