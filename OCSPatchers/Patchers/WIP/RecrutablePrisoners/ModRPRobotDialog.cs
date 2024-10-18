using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers.WIP.RecrutablePrisoners
{
    internal class ModRPRobotDialog : ICharacterDialogToAdd
    {
        bool _isCheckedMissingReferenceTarget = false;
        bool _isMissingReferenceTarget = false;

        ModItem? _raceItem;
        string _dialogStringId => "1535101-Reprogrammable skeletons.mod";
        string _dialogForRacesStringId => "1535103-Reprogrammable skeletons.mod";
        string _dialogCategorieName => "dialogue package";
        ModItem? _dialogueItem;

        bool IsValid(IDialogsPatcherData dialogsPatcherData)
        {
            if (_isCheckedMissingReferenceTarget 
                && _isMissingReferenceTarget) return false;

            if (dialogsPatcherData.ModItem.IsDeleted()) return false;
            if (dialogsPatcherData.ModItem.Type != ItemType.Character) return false;

            if (dialogsPatcherData.ModItem == default) return false;

            if (!dialogsPatcherData.ModItem.ReferenceCategories.ContainsKey("weapon level"))
            {
                return false;
            }

            if (!dialogsPatcherData.ModItem.ReferenceCategories.ContainsKey("race")) return false;

            if (!IsValisdSkeletonRace(dialogsPatcherData.ModItem)) return false;

            return true;
        }

        private bool IsValisdSkeletonRace(ModItem modItem)
        {
            var refs = modItem.ReferenceCategories["race"].References;

            if (refs.Count == 0) return false;
            if (refs.Count > 1) return false;

            var raceRef = refs[0];

            if (raceRef == null) return false;
            if (raceRef.IsDeleted()) return false;
            if (raceRef.Target == default) return false;

            _raceItem = raceRef.Target;

            if (_raceItem == default) return false;

            if (!_raceItem.Values.TryGetValue("sounds", out var v1)) return false;
            if (v1 is not int i1) return false;
            if (i1 != 3) return false; // 3 == SKELETON

            if (!_raceItem.Values.TryGetValue("heal stat", out var v2)) return false;
            if (v2 is not int i2) return false;
            if (i2 != (int)Data.Enums.CharacterStats.STAT_ROBOTICS) return false;

            return true;
        }
        public void TryAdd(IDialogsPatcherData dialogsPatcherData)
        {
            if (!IsValid(dialogsPatcherData)) return;

            // set dialogue
            if (!_isCheckedMissingReferenceTarget)
            {
                _isCheckedMissingReferenceTarget = true;

                // usually here check for main dialogue string id but for this dialogue we check dialogue line type where will need to add race to check
                _dialogueItem = dialogsPatcherData.Context.Items.OfType(ItemType.DialogueLine).FirstOrDefault(d => d.StringId == _dialogForRacesStringId);
                if (_dialogueItem == default)
                {
                    _isMissingReferenceTarget = true;
                    return;
                }
            }

            // add dialog reference
            if (!dialogsPatcherData.ModItem.ReferenceCategories.ContainsKey(_dialogCategorieName))
            {
                dialogsPatcherData.ModItem.ReferenceCategories.Add(_dialogCategorieName);
            }
            else if (dialogsPatcherData.ModItem.ReferenceCategories[_dialogCategorieName].References.ContainsKey(_dialogStringId)) return;

            if (new CheckRecruitingDialogsTools().HaveRecruitingDialoguePackage(dialogsPatcherData.ModItem.ReferenceCategories[_dialogCategorieName], dialogsPatcherData)) return;

            dialogsPatcherData.ModItem.ReferenceCategories[_dialogCategorieName].References.Add(_dialogStringId);

            if(!dialogsPatcherData.RecruitingDialogIds.Contains(_dialogStringId)) 
                dialogsPatcherData.RecruitingDialogIds.Add(_dialogStringId); // add target dialog for check

            // add race reference to conditions
            var references = _dialogueItem!.ReferenceCategories["my race"].References;
            if (references.ContainsKey(_raceItem!.StringId)) return;
            references.Add(new ModReference(_raceItem.StringId));
        }
    }
}
