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
            var statKeyNames = new string[3] { "assassin", "stealth", "thievery" };

            foreach (var modItem in context.Items.OfType(ItemType.Stats))
            {
                foreach(var keyName in statKeyNames)
                {
                    if (!modItem.Values.TryGetValue(keyName, out var obj)) continue;
                    if (obj is not int statValue) continue;
                    if (statValue < 111) continue;

                    modItem.Values[keyName] = 100;
                }
            }
            return Task.CompletedTask;
        }
    }
}
