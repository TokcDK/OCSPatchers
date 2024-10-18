using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP.RecrutablePrisoners
{
    internal class ModRPNinjaDialog : ICharacterDialogToAdd
    {
        bool _failedToFindModItem = false;
        bool _itemFound = false;
        bool _isDialogToAddAdded = false;
        string _dialogToAddStringId = "5007284-RecruitPrisoners.mod";

        bool IsValid(IDialogsPatcherData dialogsPatcherData)
        {
            if (dialogsPatcherData.ModItem.IsDeleted()) return false;
            if (dialogsPatcherData.ModItem.Type != ItemType.Character) return false;

            if (HaveNinjaName(dialogsPatcherData.ModItem)) return true;

            if (HaveNinjaReferences(dialogsPatcherData)) return true;

            return false;
        }

        private bool HaveNinjaReferences(IDialogsPatcherData dialogsPatcherData)
        {
            if (dialogsPatcherData.ModItem.ReferenceCategories.ContainsKey("dialogue package"))
            {
                var refs = dialogsPatcherData.ModItem.ReferenceCategories["dialogue package"].References;

                if (refs.Any(r => r.Target != default 
                && (r.Target.Name.StartsWith("MODRP") || dialogsPatcherData.RecruitingDialogIds.Contains(r.TargetId))
                )) return false;
                if (refs.Any(r => HaveNinjaName(r.Target))) return true;
            }

            if (dialogsPatcherData.ModItem.ReferenceCategories.ContainsKey("stats")
                && dialogsPatcherData.ModItem.ReferenceCategories["stats"].References.Any(r => HaveNinjaName(r.Target))
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

        public void TryAdd(IDialogsPatcherData dialogsPatcherData)
        {
            if (!_isDialogToAddAdded)
            {
                _isDialogToAddAdded = true;

                if (!dialogsPatcherData.RecruitingDialogIds.Contains(_dialogToAddStringId))
                {
                    dialogsPatcherData.RecruitingDialogIds.Add(_dialogToAddStringId);
                }
            }

            if (!_itemFound && _failedToFindModItem) return;
            if (!IsValid(dialogsPatcherData)) return;

            if (!_itemFound && !_failedToFindModItem)
            {
                var dialogReference = dialogsPatcherData.Context.Items.OfType(ItemType.DialoguePackage).FirstOrDefault(i => i.StringId == _dialogToAddStringId);
                if (dialogReference == default)
                {
                    _failedToFindModItem = true;
                    return;
                }

                _itemFound = true;
            }

            if (!dialogsPatcherData.ModItem.ReferenceCategories.ContainsKey("dialogue package"))
            {
                dialogsPatcherData.ModItem.ReferenceCategories.Add("dialogue package");
            }

            if (dialogsPatcherData.ModItem.ReferenceCategories["dialogue package"].References.ContainsKey(_dialogToAddStringId)) return;

            dialogsPatcherData.ModItem.ReferenceCategories["dialogue package"].References.Add(_dialogToAddStringId);
        }

        //ModItem? m;
    }
}
