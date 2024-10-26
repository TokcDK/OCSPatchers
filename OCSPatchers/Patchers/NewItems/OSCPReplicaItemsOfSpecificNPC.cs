using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems
{
    internal abstract class OSCPReplicaItemsOfSpecificNPC : OCSPatcherBase
    {
        public override string PatchFileNameWithoutExtension => "OCSreplicaSelectedNPCItems";
        public override string PatcherName { get; } = "Make replica of all armors and weapons of the characters";

        protected virtual string[] NpcStringIDsToCheck { get; } = Array.Empty<string>();

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

            var npcRaces = npc.ReferenceCategories["race"].References.Where(r => r != null).Select(r => r.TargetId).ToArray();
            if (npcRaces.Length == 0) return; // not need to parse if no races for some reason

            foreach (var categoryName in _categoryNames)
            {
                if (!npc.ReferenceCategories.ContainsKey(categoryName)) return;

                var categoryReferences = npc.ReferenceCategories[categoryName].References;

                if (!ReplicaItemsWithName(categoryReferences, npcRaces, context)) continue;

                npc.Values["armour grade"] = (int)Data.Enums.ArmorGrades.GEAR_MASTER; // set the grade for the npc to be for uniques
            }
        }

        HashSet<string> _replicatedItems = new HashSet<string>();
        private bool ReplicaItemsWithName(ModReferenceCollection categoryReferences, string[] npcRaceIds, IModContext context)
        {
            bool isChangedAny = false;

            var itemsToReplicateReferencesList = categoryReferences.Select(i => i).ToArray();

            foreach (var itemReference in itemsToReplicateReferencesList)
            {
                var itemToReplicate = itemReference.Target;

                if (itemToReplicate == null) continue;

                // добавить также!  если уже была пропарсена, то проверяем только расы для добавления
                if (_replicatedItems.Contains(itemToReplicate.StringId)) continue;

                var uniqueItem = context.NewItem(itemToReplicate);// the item will be unique item for the npc

                if(!itemToReplicate.Name.EndsWith(" (Реплика)"))
                {
                    itemToReplicate.Name = $"{itemToReplicate.Name} (Реплика)"; // we make replica from original item because many of references to this item from craft facilities and researching
                }

                if (!uniqueItem.ReferenceCategories.ContainsKey("races"))
                    uniqueItem.ReferenceCategories.Add("races");

                var racesCategory = uniqueItem.ReferenceCategories["races"].References;
                foreach (var raceId in npcRaceIds)
                {
                    if(racesCategory.ContainsKey(raceId)) continue;

                    racesCategory.Add(new ModReference(raceId)); // specify the unique race for the item
                }

                categoryReferences.Remove(itemReference); // remove original item from the npc
                categoryReferences.Add(new ModReference(uniqueItem.StringId, itemReference.Value0, itemReference.Value1));

                if(!_replicatedItems.Contains(uniqueItem.StringId))
                    _replicatedItems.Add(uniqueItem.StringId); // for case if one items using by many characters
                if(!_replicatedItems.Contains(itemReference.TargetId))
                    _replicatedItems.Add(itemReference.TargetId); // for case if one items using by many characters

                isChangedAny = true;
            }

            return isChangedAny;
        }
    }
}
