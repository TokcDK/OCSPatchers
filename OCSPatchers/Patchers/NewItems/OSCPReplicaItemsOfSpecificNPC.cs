using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.ModAssistingPatchers
{
    internal class OSCPReplicaItemsOfSpecificNPC : OCSPatcherBase
    {
        public override string PatchFileNameWithoutExtension => "2B_tweaks_replica";
        public override string PatcherName => "Make replica of all armors and weapons of the characters";

        readonly string[] NpcStringIDsToCheck = new string[2] { "14-2B.mod", "75-2B.mod" };

        public override async Task ApplyPatch(IModContext context, IInstallation installation)
        {
            if (NpcStringIDsToCheck.Length == 0) return;

            foreach (var npcStringID in NpcStringIDsToCheck)
            {
                var npc = context.Items.OfType(ItemType.Character).FirstOrDefault(i => i.StringId == npcStringID);

                if (npc == null) continue;

                ReplicaNpcItems(npc, context);
            }
        }

        readonly string[] _categoryNames = new string[3] { "clothing", "weapons", "crossbows" };
        private void ReplicaNpcItems(ModItem npc, IModContext context)
        {
            if (!npc.ReferenceCategories.ContainsKey("race")) return; // not need to parse if no races for some reason

            var npcRaces = npc.ReferenceCategories["race"].References.Where(r => r != null).Select(r => r.TargetId).ToArray(); ;
            if (npcRaces.Length == 0) return; // not need to parse if no races for some reason

            foreach (var categoryName in _categoryNames)
            {
                if (!npc.ReferenceCategories.ContainsKey(categoryName)) return;

                var categoryReferences = npc.ReferenceCategories[categoryName].References;

                ReplicaItemsWithName(categoryReferences, npcRaces, context);
            }
        }

        private void ReplicaItemsWithName(ModReferenceCollection categoryReferences, string[] npcRaces, IModContext context)
        {
            foreach (var reference in categoryReferences)
            {
                var clothingItem = reference.Target;

                if (clothingItem == null) continue;

                var replicaItem = clothingItem.DeepClone();

                clothingItem.Name = $"{clothingItem.Name} (Реплика)";

                context.NewItem(replicaItem);

                if (!replicaItem.ReferenceCategories.ContainsKey("races"))
                    replicaItem.ReferenceCategories.Add("races");

                var racesCategory = replicaItem.ReferenceCategories["races"].References;
                foreach (var raceId in npcRaces)
                {
                    racesCategory.Add(new ModReference(raceId)); // specify the unique race for the item
                }
            }
        }
    }
}
