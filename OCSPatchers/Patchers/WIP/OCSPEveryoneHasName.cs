using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    internal class OCSPEveryoneHasName : OCSPatcherBase
    {
        public override string PatcherName => "HolyNationRacismFix";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var characters = context.Items.OfType(ItemType.Character);
            foreach (var character in characters)
            {
                if (!character.Values.ContainsKey("named") 
                    || character.Values["named"] is not bool isNamed
                    || isNamed
                    ) continue;

                if (character.Values.ContainsKey("unique") 
                    && character.Values["unique"] is bool isUnique 
                    && isUnique
                    ) continue;

                character.Values["named"] = true;
            }

            return Task.CompletedTask;
        }
    }
}
