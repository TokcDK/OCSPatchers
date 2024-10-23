using OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers;
using OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers;
using OpenConstructionSet.Mods;

namespace OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters
{
    internal partial class OCSPLegendaryNPCItems
    {
        public class LegendaryItemEffectApplyWeapons : LegendaryItemEffectApplyBase
        {
            public readonly Dictionary<string, List<ModItem>> _cacheOfAddedLegendaryWeaponsByOrigin = new();

            protected override string CategoryName => "weapons";

            protected override string GetPatchedItemDescription(ILegendaryItemEffect effectPatcher) => $"#ffffffЭто оружие имеет легендарный эффект \"#ff0000{effectPatcher!.Name}#ffffff\", со следующими эффектами.\r\n{effectPatcher.Description}";

            protected override Dictionary<string, List<ModItem>> AddedItemsCache => _cacheOfAddedLegendaryWeaponsByOrigin;
            public override ILegendaryItemEffect[] EffectPatchers { get; } = new ILegendaryItemEffect[]
            {
                // weapon
                //new RandomTo4WeaponLegendaryItemEffect(),
                new ShieldLegendaryItemEffect(),
                new SharpLegendaryItemEffect(),
                new StunLegendaryItemEffect(),
                new PenetratingLegendaryItemEffect(),
                new JaggedLegendaryItemEffect(),
                new AnimalKillerLegendaryItemEffect(),
                new HumanKillerLegendaryItemEffect(),
                new RobotKillerLegendaryItemEffect(),
                new LivingKillerLegendaryItemEffect(),
                new HarpoonLegendaryItemEffect(),
                new MinSharpLegendaryItemEffect(),
                new BluntConvertedWeaponLegendaryItemEffect(),
                new CutConvertedWeaponLegendaryItemEffect(),
            };
        }
    }
}
