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

namespace OCSPatchers.Patchers.ModAssistingPatchers
{
    internal class OSCPNPCEnjoysMoreShopping : OCSPatcherBase
    {
        public override string PatcherName => "NPCEnjoysMoreShopping mod tweaks for mods";

        public override string[] ReferenceModNames => new[] { "NPC enjoys more shopping.mod" };

        public override async Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var characters = context.Items.OfType(ItemType.Character).Where(i => !i.IsDeleted());
            Parallel.ForEach(characters, character =>
            {
                if (!character.Values.ContainsKey("wages")) return;

                character.Values["wages"] = 10000;
            });
        }
    }
}
