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
    internal class OSCPScarsPathfindingFix : OCSPatcherBase
    {
        public override string PatcherName => "OCSP SCAR's pathfinding fix";

        public override string[] ReferenceModNames => new[] { "SCAR's pathfinding fix.mod" };

        public override async Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var (waterAvoidance, pathfindAcceleration, version) = await ReadScarsMod(installation);

            var races = context.Items.OfType(ItemType.Race);
            foreach (var race in races)
            {
                // Scar pathfinding fix
                if (race.Values.TryGetValue("editor limits", out var value)
                    && value is FileValue file
                    && !string.IsNullOrEmpty(file.Path))
                {

                    Console.WriteLine("Updating " + race.Name);
                    race.Values["pathfind acceleration"] = pathfindAcceleration;

                    // avoid changing for races that like water
                    if (race.Values.ContainsKey("water avoidance") && (float)race.Values["water avoidance"] > 0)
                    {
                        race.Values["water avoidance"] = waterAvoidance;
                    }
                }
            }
        }

        async Task<(float waterAvoidance, float pathFindAcceleration, int version)> ReadScarsMod(IInstallation installation)
        {
            string refModName = ReferenceModNames[0];
            if (!installation!.Mods.TryFind(refModName, out var referenceMod))
            {
                // Not found
                Error($"Unable to find {refModName}");
                return (0, 0, 0);
            }

            ModFileData referenceData;

            try
            {
                referenceData = await referenceMod.ReadDataAsync();
            }
            catch (Exception ex)
            {
                Error($"Unable to load {refModName}{Environment.NewLine}Error: {ex}");
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

        void Error(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
