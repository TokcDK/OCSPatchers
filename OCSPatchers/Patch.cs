using OCSPatchers.Patchers;
using OCSPatchers.Patchers.ModAssistingPatchers;
using OCSPatchers.Patchers.MoreItemsToSpecificStorage;
using OCSPatchers.Patchers.NewItems;
using OCSPatchers.Patchers.ReferencesShare;
using OCSPatchers.Patchers.Tweaks;
using OCSPatchers.Patchers.WIP;
using OCSPatchers.Patchers.WIP.RecrutablePrisoners;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Xml.Linq;

namespace OCSPatchers
{
    internal class Patch
    {
        static string ModName { get; set; } = "OCSPatch";

        static string ModFileName => ModName + ".mod";

        internal static async Task Apply()
        {
            var patchers = new List<IOCSPatcher>
            {
                ////patchers, parameters change, can be recreated many times in one playthrough but maybe will need import
                //new OCSPatcherGeneral(),
                //new OSCPScarsPathfindingFix(),
                //new OSCPNPCEnjoysMoreShopping(),
                //new OCSPAnimationsShare(),
                //new OCSPHairsShare(),
                //new OCSPStackableItems1000(),
                ////////////new OCSPBiggerBackpacks(),
                ////////////new OCSPEveryoneHasName(),
                //new OCSPResetToAOMAnims(),
                //new OCSPHubGenesisOwnTheHubCopySquads(),
                //new OCSMoreItemsToSpecificStorageFood(),
                //new OCSMoreItemsToSpecificStorageBolts(),
                //new OCSMoreItemsToSpecificStorageRobotParts(),
                //new OCSMoreItemsToSpecificStorageResearch(),
                //new OCSMoreItemsToSpecificStorageFish(),
                //new OCSMoreItemsToSpecificStorageFish1(),

                //new OCSPRecrutablePrisonersDialogsAdd(),
                //new OCSPRemoveNpcOverpoweredStats(),

                ////// new items, recommended to create only one for one playthrough or create with new name new each time
                // new OCSPLegendaryNPCItems(), // new items here
                new OSCPReplicaItemsOfSpecific2B(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCBrokeback(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCTrinity(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCXenoblade2Pyra(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCXenoblade2Mythra(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCSonyaBlade(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCFFVIICloudStrife(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCFF7Yuffie(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCFFVIIYuffieKisaragi(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCFFVIITIFA(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCFFVIIAerith(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCFFLightning(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCWitcher3VES(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCTheWitcher(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCTheWitchertweaks(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCSekirotweaks(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCMAI(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCUnderworld(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCCyberpunk2077V(), // for specific purposes only! new items!
                new OSCPReplicaItemsOfSpecificNPCAshleyGraham(), // for specific purposes only! new items!
            };

            var patchersByPatchFileName = SortByPatchFileName(patchers);

            foreach(var patchData in patchersByPatchFileName)
            {
                Console.WriteLine(".........");

                ModName = patchData.Key;
                patchers = patchData.Value;

                Console.WriteLine($"Creating {ModFileName}..");

                IInstallation? installation = await SelectInstallation();
                patchers = FilterPatchersByReferencedMods(patchers, installation);

                Console.WriteLine();

                // getlist of excluded mod names where must be merged patch name, referenced and excluded names
                var excludedModNames = GetExcludedModNames(patchers, new List<string>(patchersByPatchFileName.Keys));

                //remove patch file, ocs reading it and i get already modified recodrs instead of mods
                string patchModFileName = Path.Combine(installation!.Mods.Path, ModName, ModFileName);
                //if (File.Exists(patchModFileName)) File.Delete(patchModFileName);

                Console.WriteLine("Reading load order... ");
                var baseMods = await ModsToPatch(installation, excludedModNames);

                Console.WriteLine("Build context... ");
                var context = await BuildModContext(installation, baseMods, patchers);

                Console.WriteLine("Apply patchers... ");
                foreach (var patcher in patchers)
                {
                    Console.WriteLine($"Apply {patcher.PatcherName}");
                    try
                    {
                        await patcher.ApplyPatch(context, installation!);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An exception occured while the patch appling:\nPatcher:{patcher.PatcherName}\nException:{ex}");
                    }
                }

                Console.WriteLine($"Saving {ModFileName}... ");

                await context.SaveAsync();

                Console.WriteLine($"Adding {ModFileName} to end of load order... ");

                var enabledMods = (await installation!.ReadEnabledModsAsync()).ToList();

                // Remove this mod and then add to the end of the load order
                enabledMods.RemoveAll(s => s == ModFileName);
                enabledMods.Add(ModFileName);

                await installation.WriteEnabledModsAsync(enabledMods);
            }

            Console.WriteLine();
            Console.WriteLine(".........");
            Console.WriteLine("All is finished!");
            Console.ReadKey();
        }

        private static Dictionary<string, List<IOCSPatcher>> SortByPatchFileName(IEnumerable<IOCSPatcher> patchers)
        {
            var patchersByPatchFileName = new Dictionary<string, List<IOCSPatcher>>();
            foreach (var patcher in patchers)
            {
                if (!patchersByPatchFileName.ContainsKey(patcher.PatchFileNameWithoutExtension))
                {
                    patchersByPatchFileName.Add(patcher.PatchFileNameWithoutExtension, new List<IOCSPatcher>() { patcher });
                }
                else
                {
                    patchersByPatchFileName[patcher.PatchFileNameWithoutExtension].Add(patcher);
                }
            }

            return patchersByPatchFileName;
        }

        private static List<IOCSPatcher> FilterPatchersByReferencedMods(IEnumerable<IOCSPatcher> patchers, IInstallation? installation)
        {
            return patchers.Where(patcher =>
            !(patcher.ReferenceModNames.Any(referenceModName => !installation!.Mods.TryFind(referenceModName, out var _))) // exclude patchers where any referenced mod name is missing in load order
            ).ToList();
        }

        static async Task<IModContext> BuildModContext(IInstallation? installation, List<string> baseMods, IEnumerable<IOCSPatcher> patchers, int version = 16)
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

        static private HashSet<string> GetExcludedModNames(IEnumerable<IOCSPatcher> patchers, List<string> patchNames)
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

            // add current patch and later in load order
            for (int i = patchNames.IndexOf(ModName); i < patchNames.Count; i++)
            {
                excluded.Add(patchNames[i]);
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
