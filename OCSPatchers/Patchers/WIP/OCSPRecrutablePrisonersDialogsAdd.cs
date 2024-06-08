using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP
{
    internal class OCSPRecrutablePrisonersDialogsAdd : OCSPatcherBase
    {
        public override string PatcherName => "Add dialog patch to npc from recrutable prisoners";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            var dialogsToAdd = new List<ICharacterDialogToAdd>
            {
                new ModRPNinjaDialog(),
            };
            foreach (var modItem in context.Items.OfType(ItemType.Character))
            {
                foreach (var dialog in dialogsToAdd)
                {
                    dialog.TryAdd(modItem, context);
                }
            }

            return Task.CompletedTask;
        }
    }

    public interface ITryingToAdd
    {
        public void TryAdd(ModItem modItem, IModContext context);
    }
    public interface ICharacterDialogToAdd : ITryingToAdd
    {
    }

    internal class ModRPNinjaDialog : ICharacterDialogToAdd
    {
        bool IsValid(ModItem modItem, IModContext context)
        {
            if (modItem.Type != ItemType.Character) return false;

            if (HaveNinjaName(modItem)) return true;

            if (HaveNinjaReferences(modItem)) return true;

            return false;
        }

        private bool HaveNinjaReferences(ModItem modItem)
        {
            if (modItem.ReferenceCategories.ContainsKey("dialogue package"))
            {
                var refs = modItem.ReferenceCategories["dialogue package"].References;

                if (refs.Any(r => r.Target != default && r.Target.Name.StartsWith("MODRP"))) return false; ;
                if (refs.Any(r => HaveNinjaName(r.Target))) return true;
            }

            if (modItem.ReferenceCategories.ContainsKey("stats")
                && modItem.ReferenceCategories["stats"].References.Any(r => HaveNinjaName(r.Target))
                ) return true;

            return false;
        }

        private bool HaveNinjaName(ModItem? modItem)
        {
            if (modItem == default) return false;

            string name = modItem.Name;
            if (name.Contains("Ninja", StringComparison.InvariantCultureIgnoreCase)) return true;
            if (name.Contains("Jonin", StringComparison.InvariantCultureIgnoreCase)) return true;
            if (name.Contains("Ниндзя", StringComparison.InvariantCultureIgnoreCase)) return true;
            if (name.Contains("Джонин", StringComparison.InvariantCultureIgnoreCase)) return true;
            if (name.Contains("Йоунин", StringComparison.InvariantCultureIgnoreCase)) return true;

            return false;
        }

        string _dialogStringId => "5007284-RecruitPrisoners.mod";
        public void TryAdd(ModItem modItem, IModContext context)
        {
            if (!_itemFound && _failedToFindModItem) return;
            if (!IsValid(modItem, context)) return;

            if (!_itemFound && !_failedToFindModItem)
            {
                var dialogReference = context.Items.OfType(ItemType.DialoguePackage).FirstOrDefault(i => i.StringId == _dialogStringId);
                if (dialogReference == default)
                {
                    _failedToFindModItem = true;
                    return;
                }

                _itemFound = true;
            }

            if (!modItem.ReferenceCategories.ContainsKey("dialogue package")) return;
            if (modItem.ReferenceCategories["dialogue package"].References.ContainsKey(_dialogStringId)) return;

            modItem.ReferenceCategories["dialogue package"].References.Add(_dialogStringId);
        }

        bool _failedToFindModItem = false;
        bool _itemFound = false;
        //ModItem? m;
    }
}
