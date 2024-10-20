using System.Collections.Generic;
using System.Linq;
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
        protected abstract int ItemFunctionIdToAdd { get; }
        protected virtual int InventorySoundIdOptionalToAdd { get; } = -1;

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

                if (!IsValidItemFunction(item)) continue;
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

        protected virtual bool IsValidItemFunction(ModItem item)
        {
            return IsValidIntValue(item, "item function", ItemFunctionIdToAdd);
        }

        protected virtual bool IsValidInventorySound(ModItem item)
        {
            if (InventorySoundIdOptionalToAdd == -1) return true;

            return _wasValidInventorySound = IsValidIntValue(item, "inventory sound", InventorySoundIdOptionalToAdd);
        }
        protected bool _wasValidInventorySound = false;

        protected bool IsValidIntValue(ModItem item, string valueName, int intValue)
        {
            if (!item.Values.TryGetValue(valueName, out var value)) return false;
            if (value is not int v || v != intValue) return false;

            return true;
        }
        protected bool IsValidBoolValue(ModItem item, string valueName, bool boolValue)
        {
            if (!item.Values.TryGetValue(valueName, out var value)) return false;
            if (value is not bool v || v != boolValue) return false;

            return true;
        }

        protected virtual bool IsValidItemSpecific(ModItem item) => true;

        protected virtual bool IsValidInventoryItems(ModReferenceCollection limitInventoryRef)
        {
            return !(LimitInventoryItemIdsToCheck.Any(id => !limitInventoryRef.ContainsKey(id)));
        }
    }
}
