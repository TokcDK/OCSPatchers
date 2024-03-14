using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    internal class OSCPScarsPathfindingFix : OCSPatcherBase
    {
        public override string PatcherName => "OCSP SCAR's pathfinding fix";

        public override string[] ReferenceModNames => new[] { "SCAR's pathfinding fix.mod" };

        public override async void ApplyPatch(IModContext context)
        {
            var (waterAvoidance, pathfindAcceleration, version) = await ReadScarsMod();
        }

        async Task<(float waterAvoidance, float pathFindAcceleration, int version)> ReadScarsMod()
        {
            if (!installation!.Mods.TryFind(ReferenceModName, out var referenceMod))
            {
                // Not found
                Error($"Unable to find {ReferenceModName}");
                return (0, 0, 0);
            }

            ModFileData referenceData;

            try
            {
                referenceData = await referenceMod.ReadDataAsync();
            }
            catch (Exception ex)
            {
                Error($"Unable to load {ReferenceModName}{Environment.NewLine}Error: {ex}");
                return (0, 0, 0);
            }

            // Extract core values from the Greenlander race item
            var greenlander = referenceData.Items.Find(i => i.Name == "Greenlander");

            if (greenlander is null)
            {
                Error("Could not find Greenlander");
                return (0, 0, 0);
            }

            var pathfindAcceleration = greenlander.Values["pathfind acceleration"];
            var waterAvoidance = greenlander.Values["water avoidance"];

            return ((float)waterAvoidance, (float)pathfindAcceleration, referenceData.Header.Version);
        }
    }
}
