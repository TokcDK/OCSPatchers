using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.MoreItemsToSpecificStorage
{
    internal class OCSPFoodStoragesAddMoreFoods : OCSPatcherBase
    {
        public override string PatcherName => "Add more food for food storages";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // Bigger backpacks
            var storages = context.Items.OfType(ItemType.Building).Where(i => !i.IsDeleted());
            HashSet<ModItem> foodStorages = new();
            foreach (var item in storages)
            {
                if (!item.Values.TryGetValue("inventory content name", out var value)) continue;
                if (value is not string v || v != "Food Items") continue;
                if (!item.ReferenceCategories.ContainsKey("limit inventory")) continue;

                var limitInventoryRef = item.ReferenceCategories["limit inventory"].References;

                if (!limitInventoryRef.ContainsKey("43961-rebirth.mod")) continue;
                if (!limitInventoryRef.ContainsKey("50514-Newwworld.mod")) continue;

                if (foodStorages.Contains(item)) continue;

                foodStorages.Add(item);
            }

            // get ass food items
            HashSet<string> foodItems = new();
            foreach (var item in context.Items.Where(i => !i.IsDeleted()))
            {
                if (item.Name.StartsWith('_')) continue;
                if (item.Name.StartsWith('@')) continue;
                if (!item.Values.TryGetValue("item function", out var value)) continue;
                if (value is not int v || v != 3) continue;

                if (foodItems.Contains(item.StringId)) continue;

                foodItems.Add(item.StringId);
            }

            // add missing food items
            foreach (var item in foodStorages)
            {
                var limitInventoryRef = item.ReferenceCategories["limit inventory"].References;

                foreach (var foodItem in foodItems)
                {
                    if (limitInventoryRef.ContainsKey(foodItem)) continue;

                    limitInventoryRef.Add(new ModReference(foodItem));
                }
            }

            return Task.CompletedTask;
        }
    }
}
