using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class WeatherProtectionLegendaryItemEffect : ILegendaryArmorEffect
    {
        public virtual string Name => "Защита от погоды";

        public virtual string Description => $"#afa68b{Name}#a8b774 50%";

        public bool TryApplyEffect(ModItem modItem, IModContext context)
        {
            return SetWeatherResist(modItem);
        }

        protected virtual float ResistAmount { get; } = (float)0.5;

        protected bool SetWeatherResist(ModItem modItem)
        {
            if (!modItem.Values.TryGetValue("weather protection amount", out var v1)
                || v1 is not float weatherProtectionAmount) return false;

            var protectons = new int[]
            {
                (int)Data.Enums.WeatherAffecting.WA_DUSTSTORM,
                (int)Data.Enums.WeatherAffecting.WA_ACID,
                (int)Data.Enums.WeatherAffecting.WA_BURNING,
                (int)Data.Enums.WeatherAffecting.WA_GAS,
                (int)Data.Enums.WeatherAffecting.WA_RAIN,
            };

            int valueIndex = 0;
            foreach (var protectonId in protectons)
            {
                string valueName = $"weather protection{valueIndex++}";
                if (!modItem.Values.ContainsKey(valueName))
                {
                    modItem.Values.Add(valueName, protectonId);
                }
                else
                {
                    modItem.Values[valueName] = protectonId;
                }
            }

            modItem.Values["weather protection amount"] = ResistAmount;

            return true;
        }
    }
}
