using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods;
using OpenConstructionSet.Mods.Context;
using System.Linq;
using System.Runtime.InteropServices;

namespace OCSPatchers.Patchers.NewItems
{
    internal class OSCPReplicaItemsOfSpecific2B : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "2B_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[2] { "14-2B.mod", "75-2B.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCBrokeback : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Brokeback_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[]
        {
            "13-Brokeback.mod", // такеру
            "14-Brokeback.mod", // big man
        };
    }
    internal class OSCPReplicaItemsOfSpecificNPCTrinity : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "The Matrix Trinity!_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "5008207-The Matrix Trinity!.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCXenoblade2Pyra : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Xenoblade2 Pyra_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "12-Xenoblade2 Pyra.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCXenoblade2Mythra : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Xenoblade2 Mythra_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "10-Xenoblade2 Mythra.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCSonyaBlade : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Sonya Blade!_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "5008187-Sonya Blade!.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCFFVIICloudStrife : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "FFVII Cloud Strife_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "5007299-FFVII Cloud Strife.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCFF7Yuffie : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "FF7 Yuffie_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "5008505-FF7 Yuffie.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCFFVIIYuffieKisaragi : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "FFVII Yuffie Kisaragi_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "24-FFVII Yuffie Kisaragi.mod" };
    }
    internal class OSCPReplicaItemsOfSpecificNPCFFVIITIFA : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "TIFA_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[2] { "5007299-TIFA.mod", "5007314-TIFA.mod", };
    }
    internal class OSCPReplicaItemsOfSpecificNPCFFVIIAerith : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "FFVII Aerith_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "5008351-FFVII Aerith.mod", };
    }
    internal class OSCPReplicaItemsOfSpecificNPCFFLightning : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Lightning_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "15-Lightning.mod", };
    }
    internal class OSCPReplicaItemsOfSpecificNPCWitcher3VES : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Witcher 3 VES_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[1] { "5007305-Witcher 3 VES.mod", };
    }
    internal class OSCPReplicaItemsOfSpecificNPCTheWitcher : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "The Witcher_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[] {
                "5008279-The Witcher.mod",
                "5008346-The Witcher.mod",
                "5008280-The Witcher.mod",
            };
    }
    internal class OSCPReplicaItemsOfSpecificNPCTheWitchertweaks : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "The Witcher tweaks_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[] {
                "18-The Witcher tweaks.mod",
            };
    }
    internal class OSCPReplicaItemsOfSpecificNPCSekirotweaks : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Sekiro tweaks_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[] {
                "49-Sekiro.mod",
            };
    }
    internal class OSCPReplicaItemsOfSpecificNPCMAI : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "MAI_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[] {
                "5007305-MAI.mod",
            };
    }
    internal class OSCPReplicaItemsOfSpecificNPCUnderworld : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Underworld_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[] {
                "15-Underworld.mod",
            };
    }
    internal class OSCPReplicaItemsOfSpecificNPCCyberpunk2077V : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Cyberpunk 2077 V_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[] {
                "15-Cyberpunk 2077 V.mod",
            };
    }
    internal class OSCPReplicaItemsOfSpecificNPCAshleyGraham : OSCPReplicaItemsOfSpecificNPC
    {
        public override string PatchFileNameWithoutExtension => "Ashley Graham_tweaks_replica";
        protected override string[] NpcStringIDsToCheck => new string[] {
                "26-Ashley Graham.mod",
            };
    }
}
