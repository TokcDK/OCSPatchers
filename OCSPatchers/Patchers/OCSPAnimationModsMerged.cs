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

namespace OCSPatchers.Patchers
{
    internal class OCSPAnimationModsMerged : OCSPatcherBase
    {
        public override string PatcherName => "Animations mods merge";


        readonly Dictionary<string, int> _raceIDMod = new Dictionary<string, int>()
        {
            // 2b has human appearance
            {"10-2B.mod",0 },
            {"76-2B.mod",0 }
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

        public override void ApplyPatch(IModContext context, IInstallation installation)
        {

            var referenceCategoriesRefs = new Dictionary<int, Dictionary<string, HashSet<ModReference>>>();

            var races = context.Items.OfType(ItemType.Race);
            foreach (var race in races)
            {
                // Animation mods merge
                if (race.Values.TryGetValue("male mesh", out var meshValue)
                    && meshValue is FileValue meshFile
                    && !string.IsNullOrEmpty(meshFile.Path)
                    && !meshFile.Path.Contains(@"\animal\")
                    )
                {
                    Console.WriteLine("Updating " + race.Name);
                    if (!race.ReferenceCategories.ContainsKey("animation files")) race.ReferenceCategories.Add("animation files");

                    var animFiles = race.ReferenceCategories["animation files"];
                    foreach (var animRef in _animStrIDs)
                    {
                        if (!animFiles.References.ContainsKey(animRef)) animFiles.References.Add(animRef);
                    }

                    // hairs beards heads animations GET by sounds value

                    if (!race.Values.ContainsKey("sounds")) continue;
                    if (race.Values["sounds"] is not int soundsID) continue;
                    soundsID = getRaceSoundsID(race, soundsID);
                    referenceCategoriesRefs.TryAdd(soundsID, new());
                    var parent = referenceCategoriesRefs[soundsID];

                    foreach (var propertyName in new[] { "hair colors", "hairs"/*, "heads female", "heads male"*/, "animation files" })
                    {
                        if (!race.ReferenceCategories.ContainsKey(propertyName)) continue;

                        parent.TryAdd(propertyName, new());

                        var propCat = parent[propertyName];
                        var propRefs = race.ReferenceCategories[propertyName].References;
                        foreach (var propref in propRefs)
                        {
                            if (!propCat.Contains(propref)) propCat.Add(propref);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// some races like in 2b mod using human appearance attachments
        /// </summary>
        int getRaceSoundsID(ModItem race, int inputSoundsID)
        {
            if (_raceIDMod!.ContainsKey(race.StringId)) return _raceIDMod[race.StringId];

            return inputSoundsID;
        }
    }
}
