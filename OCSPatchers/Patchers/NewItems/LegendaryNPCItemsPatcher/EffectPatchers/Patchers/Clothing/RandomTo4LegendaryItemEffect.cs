using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using System.ComponentModel.DataAnnotations;
using static OCSPatchers.Patchers.LegendaryNPCItemsPatcher.ItemTypeLegendaryGetters.OCSPLegendaryNPCItems;

namespace OCSPatchers.Patchers.NewItems.LegendaryNPCItemsPatcher.EffectPatchers.Patchers
{
    internal class RandomTo4LegendaryItemEffect : LegendaryItemEffectClothingBase
    {
        public override string Name => "Многоликий";

        public override string Description => string.Join("\r\n", _selectedEffects.Select(e => e.Description));

        bool _gotEffects = false;
        ILegendaryItemEffect[]? _effects;
        readonly List<ILegendaryItemEffect> _selectedEffects = new();
        readonly Random rnd = new();
        public override bool TryApplyClothingEffect(ModItem modItem, OpenConstructionSet.Mods.Context.IModContext context)
        {
            if (!_gotEffects) 
            {
                _effects = new LegendaryItemEffectApplyClothing().EffectPatchers.Where(e=>e.Name!= Name).ToArray();

                _gotEffects = true;
            }

            // 1st effect 100% add and each second chance is reducing by 2
            int chance = 200;
            int num = 4;
            _selectedEffects.Clear();
            while (num-- > 0 && rnd.Next() <= (chance /= 2))
            {
                var effect = _effects![rnd.Next(0, 3)];

                if (!effect.TryApplyEffect(modItem, context)) continue;

                _selectedEffects.Add(effect);
            }

            return _selectedEffects.Count>0;
        }
    }
}
