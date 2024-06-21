using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP
{
    internal class OCSPRemoveNpcOverpoweredStealthStats : OCSPatcherBase
    {
        // for mods like brokeback with many overpowered content where so big stats made ti prevent any chance to use stealth skills on npc
        public override string PatcherName => "RemoveNpcOverpoweredStealthStats";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var statKeyNames = new string[] { 
                //stealth
                "assassin", 
                "stealth", 
                "thievery",
                //science
                "medic",
                "robotics",
                "science", 
                "engineer",
                //trade
                "armour smith",
                "bow smith",
                "cooking",
                "farming",
                "labouring",
                "weapon smith", 
            };

            foreach (var modItem in context.Items.OfType(ItemType.Stats))
            {
                foreach(var keyName in statKeyNames)
                {
                    if (!modItem.Values.TryGetValue(keyName, out var obj)) continue;
                    if (obj is not float statValue) continue;
                    if (statValue < 111) continue;

                    modItem.Values[keyName] = 100;
                }
            }
            return Task.CompletedTask;
        }

        // maybe will be using for hard values changing for example set different new value depending on other stats
        interface IStatToReduce
        {
            public string? KeyName { get; }
            public float GetNewValue(ModItem? modItem);
        }

        public abstract class StatToReduceBase : IStatToReduce
        {
            public abstract string? KeyName { get; }

            public virtual float GetNewValue(ModItem? modItem)
            {
                return 100;
            }
        }

        public class StatAssassin : StatToReduceBase
        {
            public override string? KeyName => "assassin";
        }
    }
}
