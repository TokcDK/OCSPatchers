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
    internal class OCSPHolyNationRacismFix : OCSPatcherBase
    {
        public override string PatcherName => "HolyNationRacismFix";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // get required dialog lines

            var dialogues = context.Items.OfType(ItemType.DialogueLine).Where(d => d.Key.EndsWith("Dialogue.mod")).ToArray();
            //var priestSeesRobotBeastDialogueLine = dialogues["Priest sees robot/ beast"];
            //var youStopDialogueLine = dialogues["You Stop!"];

            // set valid races to "target race" in required dialog lines by some conditions like "sounds" value == "HIVE" or "SKELETON" or maybe HUMAN with extra condition for races like Cannibal
            var races = context.Items.OfType(ItemType.Race);
            foreach (var race in races)
            {
            }

            return Task.CompletedTask;
        }
    }
}
