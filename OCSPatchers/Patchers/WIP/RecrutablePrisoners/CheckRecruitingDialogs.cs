using OpenConstructionSet.Data;
using OpenConstructionSet.Mods.Context;
using OpenConstructionSet.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCSPatchers.Patchers.WIP.RecrutablePrisoners
{
    internal class CheckRecruitingDialogsTools
    {
        HashSet<string> _checkedRefs = new HashSet<string>();

        internal bool HaveRecruitingDialoguePackage(ModReferenceCategory dialoguePackageModReferenceCategory, IDialogsPatcherData dialogsPatcherData, bool isRoot = true)
        {
            if (dialoguePackageModReferenceCategory.References.Count == 0) return false;

            foreach (var reference in dialoguePackageModReferenceCategory.References)
            {
                if (reference.Target == null) continue;
                if (dialogsPatcherData.RecruitingDialogIds.Contains(reference.TargetId)) return true;

                if (HaveRecruitingDialogue(reference.Target, dialogsPatcherData))
                {
                    if (!dialogsPatcherData.RecruitingDialogIds.Contains(reference.Target.StringId))
                    {
                        dialogsPatcherData.RecruitingDialogIds.Add(reference.Target.StringId);
                    }

                    return true;
                }
            }

            return false;
        }

        bool HaveRecruitingDialogue(ModItem? dialogModItem, IDialogsPatcherData dialogsPatcherData)
        {
            if (_checkedRefs.Contains(dialogModItem.StringId)) return false;

            _checkedRefs.Add(dialogModItem.StringId);

            if (dialogModItem!.ReferenceCategories.ContainsKey("dialogs"))
            {
                if (HaveRecruitingDialoguePackage(dialogModItem!.ReferenceCategories["dialogs"], dialogsPatcherData, false)) return true;
            }

            if (dialogModItem!.ReferenceCategories.ContainsKey("lines"))
            {
                foreach (var reference in dialogModItem!.ReferenceCategories["lines"].References)
                {
                    if(HaveRecruitingDialogueLine(reference.Target, dialogsPatcherData)) return true;
                }
            }

            return false;
        }

        bool HaveRecruitingDialogueLine(ModItem? dialogModItem, IDialogsPatcherData dialogsPatcherData)
        {
            if (_checkedRefs.Contains(dialogModItem.StringId)) return false;

            _checkedRefs.Add(dialogModItem.StringId);

            if (dialogModItem!.ReferenceCategories.ContainsKey("effects"))
            {
                if (HaveJoinSquadEffect(dialogModItem.ReferenceCategories["effects"].References)) return true;
            }

            if (dialogModItem!.ReferenceCategories.ContainsKey("lines"))
            {
                foreach (var reference in dialogModItem!.ReferenceCategories["lines"].References)
                {
                    if(HaveRecruitingDialogueLine(reference.Target, dialogsPatcherData)) return true;
                }
            }

            return false;
        }

        bool HaveJoinSquadEffect(ModReferenceCollection effectsCategoryReferences)
        {
            foreach (var reference in effectsCategoryReferences)
            {
                if (reference.Target == null) continue;
                if (reference.Target.Type != ItemType.DialogAction) continue;
                if (!reference.Target.Values.ContainsKey("action name")) continue;

                int i = (int)reference.Target.Values["action name"];
                if (i == 3) return true; // join squad with edit
                if (i == 18) return true; // join squad fast
            }

            return false;
        }
    }
}
