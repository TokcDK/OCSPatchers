﻿using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// used https://github.com/KenshiReclaimer/KenshiLib/blob/7a50eb6c525a90e142188aa8290c9a019fcd8406/Include/kenshi/Enums.h
// used https://github.com/alexliyu7352/kenshi_editor/blob/112702c543e084efe91442b0bec915ad32cf3050/forgotten_construction_set/enum/BuildingFunction.cs

namespace OCSPatchers.Data
{
    internal static class Enums
    {
        public enum ArmorGrades
        {
            GEAR_PROTOTYPE,
            GEAR_CHEAP,
            GEAR_STANDARD,
            GEAR_GOOD,
            GREAT_QUALITY,
            GEAR_MASTER,
        }

        public enum WeatherAffecting
        {
            WA_NONE,
            WA_DUSTSTORM,
            WA_ACID,
            WA_BURNING,
            WA_GAS,
            WA_RAIN
        };

        public enum AttachSlot
        {
            ATTACH_WEAPON,
            ATTACH_BACK,
            ATTACH_HAIR,
            ATTACH_HAT,
            ATTACH_EYES,
            ATTACH_BODY,
            ATTACH_LEGS,
            ATTACH_NONE,
            ATTACH_SHIRT,
            ATTACH_BOOTS,
            ATTACH_GLOVES,
            ATTACH_NECK,
            ATTACH_BACKPACK,
            ATTACH_BEARD,
            ATTACH_BELT,
            ATTACH_LEFT_ARM = 0x32,
            ATTACH_RIGHT_ARM = 0x33,
            ATTACH_LEFT_LEG = 0x34,
            ATTACH_RIGHT_LEG = 0x35
        };

        public enum CharacterStats
        {
            STAT_NONE,
            STAT_STRENGTH,
            STAT_MELEE_ATTACK,
            STAT_LABOURING,
            STAT_SCIENCE,
            STAT_ENGINEERING,
            STAT_ROBOTICS,
            STAT_SMITHING_WEAPON,
            STAT_SMITHING_ARMOUR,
            STAT_MEDIC,
            STAT_THIEVING,
            STAT_TURRETS,
            STAT_FARMING,
            STAT_COOKING,
            STAT_HIVEMEDIC,
            STAT_VET,
            STAT_STEALTH,
            STAT_ATHLETICS,
            STAT_DEXTERITY,
            STAT_MELEE_DEFENCE,
            STAT_WEAPONS,
            STAT_TOUGHNESS,
            STAT_ASSASSINATION,
            STAT_SWIMMING,
            STAT_PERCEPTION,
            STAT_KATANAS,
            STAT_SABRES,
            STAT_HACKERS,
            STAT_HEAVYWEAPONS,
            STAT_BLUNT,
            STAT_MARTIALARTS,
            STAT_MASSCOMBAT,
            STAT_DODGE,
            STAT_SURVIVAL,
            STAT_POLEARMS,
            STAT_CROSSBOWS,
            STAT_FRIENDLY_FIRE,
            STAT_LOCKPICKING,
            STAT_SMITHING_BOW,
            STAT_END,
            _PrimaryWeaponDamage,
            _PrimaryWeaponSpeed,
            _SecondaryWeaponDamage,
            _SecondaryWeaponSpeed,
            _MaxCarryWeight,
            _StrengthXPRateWalk,
            _StrengthXPRateCombat,
            _AttackSpeedHeavyWeapons,
            _DamageResistance,
            _ToughnessXPRate,
            _KnockoutTime,
            _ToughnessKnockoutPoint,
            _WoundDeteriorationSpeed,
            _MaxRunSpeed,
            _CurrentRunSpeed,
            _AthleticsXPBonus,
            _TurretAccuracy,
            _TurretRateOfFire,
            _TurretFriendlyFireAvoidance,
            _BuildingRate,
            _RepairingRate,
            _Mining,
            _Farming,
            _UsingMachinery,
            _encumbrance,
            _combatSpeed
        }

        public enum BuildingFunction
        {
            BF_ANY,
            BF_MINE,
            BF_RESOURCE_STORAGE,
            BF_RESEARCH,
            BF_REFINERY,
            BF_GENERATOR,
            BF_BED,
            BF_TRAINING,
            BF_CAGE,
            BF_SHOP,
            BF_CRAFTING,
            BF_CORPSE_DISPOSAL,
            BF_TURRET,
            BF_GENERAL_STORAGE,
            BF_ITEM_FURNACE,
            BF_LIGHT,
            BF_TABLE,
            BF_CHAIR,
            BF_FLUFF,
            BF_SHELL_WITH_INTERIOR,
            BF_WALL,
            BF_GATE,
            BF_DOOR,
            BF_BATTERY,
            BF_THRONE,
            BF_SKELETON_BED,
            BF_RAIN_COLLECTOR,
            BF_MINE_NATURAL,
            BF_PROPELLER,
            BF_ENGINE,
            BF_LIQUID_TANK,
            BF_BOATHULL,
            BF_CONTROL_STEERING,
            BF_ALTERNATOR,
            BF_ENGINE_STARTER,
            BF_GENERATOR_WIND
        }

        public enum ItemFunction
        {
            ITEM_NO_FUNCTION,
            ITEM_FIRSTAID,
            ITEM_MEDRIGGING,
            ITEM_FOOD,
            ITEM_CONTAINER,
            ITEM_WEAPON,
            ITEM_CLOTHING,
            ITEM____,
            ITEM_NARCOTIC,
            ITEM_TOOL,
            ITEM_ANYTHING,
            ITEM_BLUEPRINT,
            ITEM_ROBOTREPAIR,
            ITEM_BOOK,
            ITEM_MONEY,
            ITEM_FOOD_RESTRICTED,
            ITEM_AMMO,
            ITEM_SEVERED_LIMB
        };
        public enum InventorySound
        {
            BACKPACK = 0,
            BUILDING_MATERIAL = 1,
            FABRIC = 2,
            FLOUR = 3,
            FOOD = 4,
            KITS = 5,
            LEATHER = 6,
            LUXURY = 7,
            NARCOTICS = 8,
            ORE = 9,
            POTATO = 10,
            ROBOTIC_COMPONENT = 11,
            RUM = 12,
            STEEL_BAR = 13,
            TOOLS = 14,
            WATER = 15,
            WHEAT = 16,
            ARMOR_PLATING = 17,
            BLUEPRINTS = 18,
            MEAT = 19,
        };
    }
}
