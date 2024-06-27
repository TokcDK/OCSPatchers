using OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers;
using OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using OCSPatchers.Patchers.LegendaryNPCItemsPatcher.Extensions;

namespace OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters
{
    public abstract class LegendaryItemEffectApplyBase
    {
        public bool TryGetItems(ModItem legendaryChara, IModContext context)
        {
            if (!legendaryChara.ReferenceCategories.ContainsKey(CategoryName)) return false;

            var validItems = new Dictionary<string, ModReference>();

            var itemCategory = legendaryChara.ReferenceCategories[CategoryName];
            var itemRefs = itemCategory.References;
            foreach (var itemRef in itemRefs)
            {
                //var wRef = context.Items.OfType(ItemType.Weapon).First(i => i.StringId == weaponRef.TargetId);

                if (itemRef.Target == default) continue;
                if (!PatcherExtensions.IsValidModItem(itemRef.Target)) continue;
                if (validItems.ContainsKey(itemRef.TargetId)) continue;

                validItems.Add(itemRef.Target.StringId, itemRef);
            }

            var newItemsList = new List<(string, int, int, int)>();
            foreach (var itemRef in validItems.Values)
            {
                var legendaryItemsList = GetLegendaryItems(itemRef.Target, context);
                if (legendaryItemsList == null || legendaryItemsList.Count == 0)
                {
                    continue;
                }

                foreach (var legendaryItem in legendaryItemsList)
                {
                    newItemsList.Add((legendaryItem.StringId, Set1(itemRef.Value0), Set2(itemRef.Value1), Set3(itemRef.Value2)));
                }
            }

            if (newItemsList.Count == 0) return false;

            Clear(itemRefs);
            foreach (var itemToAdd in newItemsList)
            {
                if (itemRefs.ContainsKey(itemToAdd.Item1)) continue;

                itemRefs.Add(new ModReference(itemToAdd.Item1, itemToAdd.Item2, itemToAdd.Item3, itemToAdd.Item4));
            }

            return true;
        }

        protected virtual int Set3(int value2)
        {
            return value2;
        }

        protected virtual int Set2(int value1)
        {
            return value1;
        }

        protected virtual int Set1(int value0)
        {
            return value0;
        }

        protected virtual void Clear(ModReferenceCollection itemRefs)
        {
            itemRefs.Clear();
        }

        private List<ModItem> GetLegendaryItems(ModItem? sourceModItem, IModContext context)
        {
            if (sourceModItem!.StringId.Contains("CL Legendary")) return new List<ModItem>();

            if (AddedItemsCache.ContainsKey(sourceModItem.StringId))
            {
                return AddedItemsCache[sourceModItem.StringId]; // already made legendaries
            }

            var legendaryItems = new List<ModItem>();
            foreach (var effectPatcher in EffectPatchers)
            {
                var legendaryItemCandidate = sourceModItem.DeepClone(); // create temp copy for mod

                if (!effectPatcher.TryApplyEffect(legendaryItemCandidate)) continue;

                legendaryItemCandidate.Values["description"] = GetPatchedItemDescription(effectPatcher);
                legendaryItemCandidate.Name = $"#ff0000{legendaryItemCandidate.Name} \"{effectPatcher.Name}\"";

                var legendaryItem = context.NewItem(legendaryItemCandidate); // add as new only when the mod was applied

                legendaryItems.Add(legendaryItem);
            }
            if (legendaryItems.Count > 0)
            {
                AddedItemsCache.Add(sourceModItem!.StringId, legendaryItems);
            }

            return legendaryItems;
        }

        protected abstract string CategoryName { get; }
        protected abstract string GetPatchedItemDescription(ILegendaryItemEffect effectPatcher);

        protected abstract Dictionary<string, List<ModItem>> AddedItemsCache { get; }

        protected virtual ILegendaryItemEffect[] EffectPatchers { get; } = Array.Empty<ILegendaryItemEffect>();
    }
}
