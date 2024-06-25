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

        private List<ModItem> GetLegendaryItems(ModItem? weaponModItem, IModContext context)
        {
            if (weaponModItem!.StringId.Contains("CL Legendary")) return new List<ModItem>();

            if (AddedItemsCache.ContainsKey(weaponModItem.StringId))
            {
                return AddedItemsCache[weaponModItem.StringId]; // already made legendaries
            }

            var addedLegendaryWeaponsListByOrigin = AddedItemsCache.ContainsKey(weaponModItem.StringId) ? AddedItemsCache[weaponModItem.StringId] : new List<ModItem>();

            var legendaryWeapons = new List<ModItem>();
            foreach (var effectPatcher in EffectPatchers)
            {
                var legendaryWeaponCandidate = weaponModItem.DeepClone(); // create temp copy for mod

                if (!effectPatcher.TryApplyEffect(legendaryWeaponCandidate)) continue;

                legendaryWeaponCandidate.Values["description"] = GetPatchedItemescription(effectPatcher);
                legendaryWeaponCandidate.Name += $" \"#ff0000{effectPatcher.Name}#000000\"";

                var legendaryWeapon = context.NewItem(legendaryWeaponCandidate); // add as new only when the mod was applied

                addedLegendaryWeaponsListByOrigin.Add(legendaryWeapon); // add to prevent making variants for the same weapons many times

                legendaryWeapons.Add(legendaryWeapon);
            }
            if (legendaryWeapons.Count > 0)
            {
                AddedItemsCache.Add(weaponModItem!.StringId, legendaryWeapons);
            }

            return legendaryWeapons;
        }

        protected abstract string CategoryName { get; }
        protected abstract string GetPatchedItemescription(ILegendaryItemEffect effectPatcher);

        protected abstract Dictionary<string, List<ModItem>> AddedItemsCache { get; }

        protected virtual ILegendaryItemEffect[] EffectPatchers { get; } = new ILegendaryItemEffect[]
        {
                // weapon
                new ShieldLegendaryItemEffect(),
                new SharpLegendaryItemEffect(),
                new StunLegendaryItemEffect(),
                new PenetratingLegendaryItemEffect(),
                new JaggedLegendaryItemEffect(),
                new AnimalKillerLegendaryItemEffect(),
                new HumanKillerLegendaryItemEffect(),
                new RobotKillerLegendaryItemEffect(),
                new LivingKillerLegendaryItemEffect(),
                // clothing
                new AttackerSharpLegendaryItemEffect(),
                new DefencerSharpLegendaryItemEffect(),
                new RunnerLegendaryItemEffect(),
                new RangerLegendaryItemEffect(),
                new QualityLegendaryItemEffect(),
                new BluntDefenceLegendaryItemEffect(),
                new CutDefenceLegendaryItemEffect(),
                new DexterityLegendaryItemEffect(),
                new CombatSpeedLegendaryItemEffect(),
        };
    }
}
