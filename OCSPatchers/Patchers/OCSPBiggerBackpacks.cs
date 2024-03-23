using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    internal class OCSPBiggerBackpacks : OCSPatcherBase
    {
        public override string PatcherName => "Bigger backpacks";

        const string STORAGE_SIZE_HEIGHT_KEY = "storage size height";
        const string STORAGE_SIZE_WIDTH_KEY = "storage size width";
        const int MAX_SIZE = 30;
        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // Bigger backpacks
            var backpacks = context.Items.OfType(ItemType.Container);
            HashSet<string> changed = new();
            foreach (var item in backpacks)
            {
                if (item.Name.StartsWith("@") || item.Name.StartsWith("_")) continue;
                if (!item.Values.TryGetValue("slot", out var value)) continue; // have no slot
                if (value is not int v || v != 12) continue; // is not backpack?
                if (!item.Values.ContainsKey(STORAGE_SIZE_HEIGHT_KEY)) continue; // missing height
                if (!item.Values.ContainsKey(STORAGE_SIZE_WIDTH_KEY)) continue; // missing width
                if (changed.Contains(item.StringId)) continue; // already added

                //Console.WriteLine("Updating " + item.Name);

                // reset skill mods
                //item.Values["combat skill bonus"] = 0;
                //item.Values["combat speed mult"] = (float)1.0;
                //item.Values["stealth mult"] = (float)1.0;

                var height = (int)item.Values[STORAGE_SIZE_HEIGHT_KEY];
                var width = (int)item.Values[STORAGE_SIZE_WIDTH_KEY];

                changed.Add(item.StringId);

                item.Values[STORAGE_SIZE_HEIGHT_KEY] = GetRoundSize(height);
                item.Values[STORAGE_SIZE_WIDTH_KEY] = GetRoundSize(width);

            }
            return Task.CompletedTask;
        }

        private int GetRoundSize(int size)
        {
            return 30;

            int extraSize = (int)Math.Round(size * 0.5);
            int resultSize = size + extraSize;
            if (resultSize <= MAX_SIZE) return resultSize;

            return size;
        }
    }
}
