﻿using System;
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
    internal class OCSPAnimationsShare : OCSPReferencesShareBase
    {
        public override string PatcherName => "Animations share between races";

        const string ANIMATIONS_REFERENCE_CATEGORY_NAME = "animation files";
        protected override List<string> ReferenceCategoryNames => new() { ANIMATIONS_REFERENCE_CATEGORY_NAME };

        protected override List<string> ExcludedToShareReferenceIDs => new List<string>() 
        { 
            "5008210-The Matrix Trinity!.mod" // unique trinity recruit animations
        };

        // Get all races where editor limits are set i.e. it is not an animal race
        readonly List<string> _animStrIDs = new List<string>()
        {
            //Animations Overhaul Crafting
            "1535098-AnimationOverhaul.mod",
            "1535113-AnimationOverhaul.mod",
            //Great Anims
            "10-Great Anims Mod.mod",
            //More Combat Animations
            "1535143-More Combat Animation.mod",
            //Military craft
            "1535133-Military craft.mod",
        };

        protected override void PreProcess(ModItem raceModItem, int soundsID)
        {
            // add some base animatio ids?
            if (!raceModItem.ReferenceCategories.ContainsKey(ANIMATIONS_REFERENCE_CATEGORY_NAME))
                raceModItem.ReferenceCategories.Add(ANIMATIONS_REFERENCE_CATEGORY_NAME);

            var animFiles = raceModItem.ReferenceCategories[ANIMATIONS_REFERENCE_CATEGORY_NAME];
            foreach (var animRef in _animStrIDs)
            {
                if (!animFiles.References.ContainsKey(animRef)) animFiles.References.Add(animRef);
            }
        }
    }
}
