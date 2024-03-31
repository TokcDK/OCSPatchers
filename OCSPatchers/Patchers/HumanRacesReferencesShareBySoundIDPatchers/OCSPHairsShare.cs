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
    }
}
