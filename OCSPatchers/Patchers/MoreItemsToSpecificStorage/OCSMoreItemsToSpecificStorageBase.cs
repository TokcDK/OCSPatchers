using System.Collections.Generic;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal abstract class OCSMoreItemsToSpecificStorageBase : OCSPatcherBase
    {
        protected abstract string[] LimitInventoryItemIdsToCheck { get; }
        protected abstract int ItemFunctionNumToAdd { get; }

        const string VALUE_NAME_HAS_INVENTORY = "has inventory";
        const string CATEGORY_NAME = "limit inventory";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // get all valid storages
            var storages = context.Items.OfType(ItemType.Building).Where(i => !i.IsDeleted());
            HashSet<ModItem> SpecificItemStorages = new();
            foreach (var item in storages)
            {
                if (!item.Values.ContainsKey(VALUE_NAME_HAS_INVENTORY)) continue;
                if (item.Values[VALUE_NAME_HAS_INVENTORY] is not bool hasInventory) continue;
                if (!hasInventory) continue;

                if (!item.ReferenceCategories.ContainsKey(CATEGORY_NAME)) continue;

                var limitInventoryRef = item.ReferenceCategories[CATEGORY_NAME].References;

                if (!IsValidInventoryItems(limitInventoryRef)) continue;

                if (SpecificItemStorages.Contains(item)) continue;

                SpecificItemStorages.Add(item);
            }

            // get all valid items to add
            HashSet<string> itemsToAdd = new();
            foreach (var item in context.Items.Where(i => !i.IsDeleted()))
            {
                if (item.Name.StartsWith('_')) continue;
                if (item.Name.StartsWith('@')) continue;
                if (!item.Values.TryGetValue("item function", out var value)) continue;
                if (value is not int v || v != ItemFunctionNumToAdd) continue;
                if (!IsValidItemSpecific(item)) continue;

                if (itemsToAdd.Contains(item.StringId)) continue;

                itemsToAdd.Add(item.StringId);
            }

            // add missing items
            foreach (var item in SpecificItemStorages)
            {
                var limitInventoryRef = item.ReferenceCategories[CATEGORY_NAME].References;

                foreach (var itemToAdd in itemsToAdd)
                {
                    if (limitInventoryRef.ContainsKey(itemToAdd)) continue;

                    limitInventoryRef.Add(new ModReference(itemToAdd));
                }
            }

            return Task.CompletedTask;
        }

        protected virtual bool IsValidItemSpecific(ModItem item) => true;

        private bool IsValidInventoryItems(ModReferenceCollection limitInventoryRef)
        {
            return !(LimitInventoryItemIdsToCheck.Any(id => !limitInventoryRef.ContainsKey(id)));
        }
    }
}
