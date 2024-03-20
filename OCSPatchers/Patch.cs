using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using OpenConstructionSet.Installations;
using OCSPatchers.Patchers;

namespace OCSPatchers
{
    internal class Patch
    {
        const string ModName = "OCSPatch";
        const string ModFileName = ModName + ".mod";

        internal async void Apply()
        {
            IInstallation? installation = await SelectInstallation();

            IOCSPatcher[] patchers = new IOCSPatcher[2]
            {
                new OCSPatcherGeneral(),
                new OSCPScarsPathfindingFix(),
            };

            patchers = FilterPatchersByReferencedMods(patchers, installation);

            Console.WriteLine();

            // getlist of excluded mod names where must be merged patch name, referenced and excluded names
            var excludedModNames = GetExcludedModNames(patchers);

            Console.Write("Reading load order... ");
            var baseMods = await ModsToPatch(installation, excludedModNames);


            var context = await BuildModContext(installation, baseMods, patchers);

            foreach (var patcher in patchers)
            {
                patcher.ApplyPatch(context, installation!);
            }

            Console.Write("Saving... ");

            await context.SaveAsync();

            Console.Write("Adding patch to end of load order... ");

            var enabledMods = (await installation!.ReadEnabledModsAsync()).ToList();

            // Remove this mod and then add to the end of the load order
            enabledMods.RemoveAll(s => s == ModFileName);
            enabledMods.Add(ModFileName);

            await installation.WriteEnabledModsAsync(enabledMods);
        }

        private static IOCSPatcher[] FilterPatchersByReferencedMods(IOCSPatcher[] patchers, IInstallation? installation)
        {
            return patchers.Where(patcher => 
            !(patcher.ReferenceModNames.Any(referenceModName => !installation!.Mods.TryFind(referenceModName, out var _))) // exclure patchers where any referenced mod name is missing in load order
            ).ToArray();
        }

        async Task<IModContext> BuildModContext(IInstallation? installation, List<string> baseMods, IOCSPatcher[] patchers, int version = 16)
        {
            // Build mod
            var header = new Header(version, "author", "merged patchers");
            
            foreach(var patcher in patchers) 
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

        private HashSet<string> GetExcludedModNames(IOCSPatcher[] patchers)
        {
            var excluded = new HashSet<string>
            {
                ModName
            };

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

        async Task<List<string>> ModsToPatch(IInstallation? installation, HashSet<string> excluded)
        {
            var mods = new List<string>(await installation!.ReadEnabledModsAsync());

            // Don't patch ourselves or SCAR's mod
            foreach (var name in excluded) mods.Remove(name + ".mod");

            if (mods.Count == 0)
            {
                // No mods found to patch
                Error($"failed!{Environment.NewLine}No mods found to patch");
                return new();
            }

            return mods;
        }

        async Task<IInstallation?> SelectInstallation()
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
        void Error(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
