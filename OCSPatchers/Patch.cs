using OCSPatchers.Patchers;
using OCSPatchers.Patchers.ModAssistingPatchers;
using OCSPatchers.Patchers.MoreItemsToSpecificStorage;
using OCSPatchers.Patchers.ReferencesShare;
using OCSPatchers.Patchers.Tweaks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers
{
    internal class Patch
    {
        const string ModName = "OCSPatch";
        const string ModFileName = ModName + ".mod";

        internal static async Task Apply()
        {
            IInstallation? installation = await SelectInstallation();

            IOCSPatcher[] patchers = new IOCSPatcher[]
            {
                new OCSPatcherGeneral(),
                new OSCPScarsPathfindingFix(),
                new OSCPNPCEnjoysMoreShopping(),
                new OCSPAnimationsShare(),
                new OCSPHairsShare(),
                new OCSPStackableItems1000(),
                //new OCSPBiggerBackpacks(),
                new OCSPHubGenesisOwnTheHubCopySquads(),
                new OCSMoreItemsToSpecificStorageFood(),
                new OCSMoreItemsToSpecificStorageBolts(),
            };

            patchers = FilterPatchersByReferencedMods(patchers, installation);

            Console.WriteLine();

            // getlist of excluded mod names where must be merged patch name, referenced and excluded names
            var excludedModNames = GetExcludedModNames(patchers);

            //remove patch file, ocs reading it and i get already modified recodrs instead of mods
            string patchModFileName = Path.Combine(installation!.Mods.Path, ModName, ModFileName);
            if (File.Exists(patchModFileName)) File.Delete(patchModFileName);

            Console.WriteLine("Reading load order... ");
            var baseMods = await ModsToPatch(installation, excludedModNames);

            Console.WriteLine("Build context... ");
            var context = await BuildModContext(installation, baseMods, patchers);

            Console.WriteLine("Apply patchers... ");
            foreach (var patcher in patchers)
            {
                Console.WriteLine($"Apply {patcher.PatcherName}");
                await patcher.ApplyPatch(context, installation!);
            }

            Console.WriteLine("Saving... ");

            await context.SaveAsync();

            Console.WriteLine("Adding patch to end of load order... ");

            var enabledMods = (await installation!.ReadEnabledModsAsync()).ToList();

            // Remove this mod and then add to the end of the load order
            enabledMods.RemoveAll(s => s == ModFileName);
            enabledMods.Add(ModFileName);

            await installation.WriteEnabledModsAsync(enabledMods);

            Console.WriteLine("Finished!");
            Console.ReadKey();
        }

        private static IOCSPatcher[] FilterPatchersByReferencedMods(IOCSPatcher[] patchers, IInstallation? installation)
        {
            return patchers.Where(patcher =>
            !(patcher.ReferenceModNames.Any(referenceModName => !installation!.Mods.TryFind(referenceModName, out var _))) // exclure patchers where any referenced mod name is missing in load order
            ).ToArray();
        }

        static async Task<IModContext> BuildModContext(IInstallation? installation, List<string> baseMods, IOCSPatcher[] patchers, int version = 16)
        {
            // Build mod
            var header = new Header(version, "author", "merged patchers");

            foreach (var patcher in patchers)
                foreach (var referenceModName in patcher.ReferenceModNames)
                    header.References.Add(referenceModName);

            header.Dependencies.AddRange(baseMods);

            var options = new ModContextOptions(ModFileName,
                installation: installation!,
                baseMods: baseMods,
                header: header,
                loadGameFiles: ModLoadType.Base,
                //loadEnabledMods: ModLoadType.Active,
                throwIfMissing: false);

            return await new ContextBuilder().BuildAsync(options);
        }

        static private HashSet<string> GetExcludedModNames(IOCSPatcher[] patchers)
        {
            var excluded = new HashSet<string>
            {
                ModName
            };

            // fill excluded from patchers
            foreach (var patcher in patchers)
            {
                foreach (var list in new[] { patcher.ReferenceModNames, patcher.ExcludedModNames })
                {
                    foreach (var modName in list)
                    {
                        var name = modName.EndsWith(".mod") ? modName.Remove(modName.Length - 4) : modName;

                        if (excluded.Contains(name)) continue;

                        excluded.Add(name);
                    }
                }
            }

            return excluded;
        }

        static async Task<List<string>> ModsToPatch(IInstallation? installation, HashSet<string> excluded)
        {
            var list = await installation!.ReadEnabledModsAsync();
            var mods = new List<string>(list);

            // Don't patch ourselves or SCAR's mod
            foreach (var name in excluded) if (mods.Contains(name + ".mod")) mods.Remove(name + ".mod");

            if (mods.Count == 0)
            {
                // No mods found to patch
                Error($"failed!{Environment.NewLine}No mods found to patch");
                return new();
            }

            return mods;
        }

        static async Task<IInstallation?> SelectInstallation()
        {
            var installations = await new InstallationService().DiscoverAllInstallationsAsync().ToDictionaryAsync(i => i.Identifier);

            if (installations.Count == 0)
            {
                Error("Unable to find game");
                return null;
            }
            else if (installations.Count == 1)
            {
                // One installation so use it
                return installations.Values.First();
            }
            else
            {
                // Display the installations to the user
                var keys = installations.Keys.ToList();

                Console.WriteLine("Multiple installations found");

                for (var i = 0; i < keys.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {keys[i]}");
                }

                Console.Write("Please select which to use: ");

                // Get the user to chose
                var selection = keys[int.Parse(Console.ReadLine() ?? "1") - 1];

                Console.WriteLine($"Using the {selection} installation");

                return installations[selection];
            }
        }
        static void Error(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
