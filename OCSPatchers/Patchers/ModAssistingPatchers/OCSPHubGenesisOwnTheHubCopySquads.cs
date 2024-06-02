using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.ModAssistingPatchers
{
    // wip, require manual squads edits in FCS after
    internal class OCSPHubGenesisOwnTheHubCopySquads : OCSPatcherBase
    {
        public override string PatcherName => "Genesis Own The Hub compatibility";

        readonly List<string> _hubIds = new()
        {
            "18919-Newwworld.mod", // The Hub
            "11-Own the Hub.mod", // The Hub Owned
            "5007344-Genesis.mod", // The Hub O1
            "10-Buy the Hub - Genesis.mod", // The Hub O1 owned
            "5007990-Genesis.mod", // The Hub FT2
            "11-overall patch.mod", // The Hub FT2 owned
            "5007991-Genesis.mod", // The Hub FT3
            "12-overall patch.mod", // The Hub FT3 owned
        };
        readonly List<string> _modsToExcludeSquadsCopyFrom = new()
        {
            "gamedata.base",
            "Dialogue.mod",
            "Newwworld.mod",
            "rebirth.mod",
            "Own the Hub.mod",
            "Genesis.mod",
            "Buy the Hub - Genesis.mod",
        };
        readonly Dictionary<string, string> _stringIdsAddOnlyFor = new()
        {
            { "14-overall patch.mod", "owned" }, // id, string in name
        };

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // get all the Hub instances
            var hubWorldStateInstances = context.Items
                .OfType(ItemType.Town)
                .Where(i => !i.IsDeleted())
                .Where(i => _hubIds.Contains(i.StringId))
                .ToArray();

            // set lists to fill
            var theHubSquads = new Dictionary<string, ModReference>();
            var theHubResidents = new Dictionary<string, ModReference>();
            var listsData = new (Dictionary<string, ModReference> List, string id)[]
                        {
                            (theHubSquads, "bar squads"),
                            (theHubResidents, "residents"),
                        };

            FillOverallSquadsResidentsLists(hubWorldStateInstances, listsData);

            //AddMissingSquadsResidentsToInstances(ref hubWorldStateInstances, listsData);
            foreach (var theHubInstance in hubWorldStateInstances)
            {
                foreach (var listData in listsData)
                {
                    var refsGroup = theHubInstance.ReferenceCategories[listData.id];
                    var refs = refsGroup.References;

                    foreach (var reference in listData.List)
                    {
                        if (refs.ContainsKey(reference.Key)) continue;
                        if (reference.Value.IsDeleted()) continue;

                        if (_stringIdsAddOnlyFor.ContainsKey(reference.Key))
                        {
                            // skip when specific record must be added to specific town instance
                            var str = _stringIdsAddOnlyFor[reference.Key];
                            if (string.IsNullOrEmpty(str) || !theHubInstance.Name.Contains(str, StringComparison.InvariantCultureIgnoreCase))
                            {
                                continue;
                            }
                        }

                        refs.Add(new ModReference(reference.Value));
                    }
                }
            }

            return Task.CompletedTask;
        }

        //private void AddMissingSquadsResidentsToInstances(ref ModItem[] hubWorldStateInstances, (Dictionary<string, ModReference> List, string id)[] listsData)
        //{
        //    foreach (var theHubInstance in hubWorldStateInstances)
        //    {
        //        foreach (var listData in listsData)
        //        {
        //            var refsGroup = theHubInstance.ReferenceCategories[listData.id];
        //            var refs = refsGroup.References;

        //            foreach (var reference in listData.List)
        //            {
        //                if (refs.ContainsKey(reference.Key)) continue;
        //                if (reference.Value.IsDeleted()) continue;

        //                refs.Add(reference.Value);
        //            }
        //        }
        //    }
        //}

        private void FillOverallSquadsResidentsLists(ModItem[] hubWorldStateInstances, (Dictionary<string, ModReference> List, string id)[] listsData)
        {
            foreach (var theHubInstance in hubWorldStateInstances)
            {
                foreach (var listData in listsData)
                {
                    foreach (var reference in theHubInstance.ReferenceCategories[listData.id].References)
                    {
                        if (listData.List.ContainsKey(reference.TargetId)) continue;
                        if (reference.IsDeleted()) continue;
                        var modName = reference.TargetId.Substring(reference.TargetId.IndexOf('-') + 1);
                        if (_modsToExcludeSquadsCopyFrom.Contains(modName)) continue;

                        listData.List.Add(reference.Key, reference);
                    }
                }
            }
        }
    }
}
