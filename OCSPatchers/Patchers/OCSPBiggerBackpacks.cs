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

namespace OCSPatchers.Patchers
{
    internal class OCSPBiggerBackpacks : OCSPatcherBase
    {
        public override string PatcherName => "Bigger backpacks";

        const string STORAGE_SIZE_HEIGHT_KEY = "storage size height";
        const string STORAGE_SIZE_WIDTH_KEY = "storage size width";
        const int MAX_SIZE = 30;
        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // Bigger backpacks
            var backpacks = context.Items.OfType(ItemType.Container);
            HashSet<string> changed = new();
            foreach (var item in backpacks)
            {
                if (item.Name.StartsWith("@") || item.Name.StartsWith("_")) continue;
                if (!item.Values.TryGetValue("slot", out var value)) continue; // have no slot
                if (value is not int v || v != 12) continue; // is not backpack?
                if (!item.Values.ContainsKey(STORAGE_SIZE_HEIGHT_KEY)) continue; // missing height
                if (!item.Values.ContainsKey(STORAGE_SIZE_WIDTH_KEY)) continue; // missing width
                if (changed.Contains(item.StringId)) continue; // already added

                //Console.WriteLine("Updating " + item.Name);

                //var cskill = (int)item.Values["combat skill bonus"];
                //item.Values["combat skill bonus"] = (cskill += 10);
                //if (cskill > 0) item.Values["combat skill bonus"] = (int)(cskill+1) / 2;

                //var cspeed = (float)item.Values["combat speed mult"];
                //item.Values["combat speed mult"] = (cspeed = ((float)(cspeed + 0.5)));
                //if (cspeed > 1) item.Values["combat speed mult"] = (float)(1.0 + ((cspeed - 1.0) / 3));

                // reset skill mods
                //item.Values["combat skill bonus"] = 0;
                //item.Values["combat speed mult"] = (float)1.0;
                //item.Values["stealth mult"] = (float)1.0;

                var height = (int)item.Values[STORAGE_SIZE_HEIGHT_KEY];
                var width = (int)item.Values[STORAGE_SIZE_WIDTH_KEY];

                changed.Add(item.StringId);

                item.Values[STORAGE_SIZE_HEIGHT_KEY] = GetRoundSize(height);
                item.Values[STORAGE_SIZE_WIDTH_KEY] = GetRoundSize(width);

                //if (width < 8)
                //{
                //    if (width == height)
                //    {
                //        if (width == 3)
                //        {
                //            SetBackpackSquareSizeTo(5,item);
                //        }
                //        else if (width == 4)
                //        {
                //            SetBackpackSquareSizeTo(6, item);
                //        }
                //        else if (width == 5)
                //        {
                //            SetBackpackSquareSizeTo(7, item);
                //        }
                //        else if (width == 6)
                //        {
                //            SetBackpackSquareSizeTo(8, item);
                //        }
                //    }
                //}
                //else if (width == 8)
                //{
                //    SetBackpackSquareSizeTo(12, item);
                //}
                //else if (width == 10)
                //{
                //    SetBackpackSquareSizeTo(16, item);
                //}
                //else if (width == 12)
                //{
                //    ResizeWidthToHeightOr(item, width, height, 20);
                //}
                //else if (width > 30)
                //{
                //    // do nothing
                //}
                //else if (width > 20)
                //{
                //    ResizeWidthToHeightOr(item, width, height, 30);
                //}
                //else if (width > 12)
                //{
                //    ResizeWidthToHeightOr(item, width, height, 24);
                //}

            }
            return Task.CompletedTask;
        }

        private int GetRoundSize(int size)
        {
            int extraSize = (int)Math.Round(size * 0.5);
            int resultSize = size + extraSize;
            if (resultSize <= MAX_SIZE) return resultSize;

            return size;
        }

        //private void SetBackpackSquareSizeTo(int size, ModItem item)
        //{
        //    item.Values[STORAGE_SIZE_HEIGHT_KEY] = size;
        //    item.Values[STORAGE_SIZE_WIDTH_KEY] = size;
        //}

        //void ResizeWidthToHeightOr(ModItem item, int width, int height, int size)
        //{
        //    if (width < height)
        //    {
        //        item.Values[STORAGE_SIZE_WIDTH_KEY] = height;
        //    }
        //    else
        //    {
        //        SetBackpackSquareSizeTo(size, item);
        //    }
        //}
    }
}
