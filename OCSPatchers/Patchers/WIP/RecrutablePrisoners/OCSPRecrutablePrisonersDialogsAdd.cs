using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP.RecrutablePrisoners
{
    internal class OCSPRecrutablePrisonersDialogsAdd : OCSPatcherBase
    {
        public override string PatcherName => "Add dialog patch to npc from recrutable prisoners";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var dialogsToAdd = new List<ICharacterDialogToAdd>
            {
                new ModRPNinjaDialog(),
                new ModRPRobotDialog(),
            };
            var dialogsPatcherData = new IDialogsPatcherData(context);
            foreach (var modItem in context.Items.OfType(ItemType.Character))
            {
                dialogsPatcherData.ModItem = modItem;

                foreach (var dialog in dialogsToAdd)
                {
                    dialog.TryAdd(dialogsPatcherData);
                }
            }

            return Task.CompletedTask;
        }
    }

    public class IDialogsPatcherData
    {
        public IDialogsPatcherData(IModContext? context)
        {
            Context = context;
        }
        public IModContext? Context { get; }

        public ModItem? ModItem { get; set; }

        public HashSet<string> RecruitingDialogIds { get; } = new();
    }

    public interface ITryingToAdd
    {
        public void TryAdd(IDialogsPatcherData dialogsPatcherData);
    }
    public interface ICharacterDialogToAdd : ITryingToAdd
    {
    }
}
