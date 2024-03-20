using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenConstructionSet;
using OpenConstructionSet.Data;
using OpenConstructionSet.Installations;
using OpenConstructionSet.Mods.Context;

namespace OCSPatchers.Patchers
{
    internal class OCSPBiggerBackpacks : OCSPatcherBase
    {
        public override string PatcherName => "Bigger backpacks";

        public override Task ApplyPatch(IModContext context, IInstallation installation)
        {
            // Bigger backpacks
            var backpacks = context.Items.OfType(ItemType.Container);
            HashSet<string> changed = new();
            foreach (var item in backpacks)
            {
                if (item.Name.StartsWith("@") || item.Name.StartsWith("_")) continue;
                if (!item.Values.TryGetValue("slot", out var value)) continue;
                if (value is not int v || v != 12) continue; // backpack
                if (changed.Contains(item.StringId)) continue; // backpack

                //Console.WriteLine("Updating " + item.Name);

                //var cskill = (int)item.Values["combat skill bonus"];
                //item.Values["combat skill bonus"] = (cskill += 10);
                //if (cskill > 0) item.Values["combat skill bonus"] = (int)(cskill+1) / 2;

                //var cspeed = (float)item.Values["combat speed mult"];
                //item.Values["combat speed mult"] = (cspeed = ((float)(cspeed + 0.5)));
                //if (cspeed > 1) item.Values["combat speed mult"] = (float)(1.0 + ((cspeed - 1.0) / 3));

                item.Values["combat skill bonus"] = 0;
                item.Values["combat speed mult"] = (float)1.0;
                item.Values["stealth mult"] = (float)1.0;

                var height = (int)item.Values["storage size height"];
                var width = (int)item.Values["storage size width"];

                changed.Add(item.StringId);

                if (width < 8)
                {
                    if (width == height)
                    {
                        if (width == 3)
                        {
                            item.Values["storage size height"] = 5;
                            item.Values["storage size width"] = 5;
                        }
                        else if (width == 4)
                        {
                            item.Values["storage size height"] = 6;
                            item.Values["storage size width"] = 6;
                        }
                        else if (width == 5)
                        {
                            item.Values["storage size height"] = 7;
                            item.Values["storage size width"] = 7;
                        }
                        else if (width == 6)
                        {
                            item.Values["storage size height"] = 8;
                            item.Values["storage size width"] = 8;
                        }
                    }
                }
                else if (width == 8)
                {
                    item.Values["storage size height"] = 12;
                    item.Values["storage size width"] = 12;
                }
                else if (width == 10)
                {
                    item.Values["storage size height"] = 16;
                    item.Values["storage size width"] = 16;
                }
                else if (width == 12)
                {
                    if (width < height)
                    {
                        item.Values["storage size width"] = item.Values["storage size height"];
                    }
                    else
                    {
                        item.Values["storage size width"] = 20;
                        item.Values["storage size height"] = 20;
                    }
                }
                else if (width > 30)
                {
                    // do nothing
                }
                else if (width > 20)
                {
                    if (width < height)
                    {
                        item.Values["storage size width"] = item.Values["storage size height"];
                    }
                    else
                    {
                        item.Values["storage size width"] = 30;
                        item.Values["storage size height"] = 30;
                    }
                }
                else if (width > 12)
                {
                    if (width < height)
                    {
                        item.Values["storage size width"] = item.Values["storage size height"];
                    }
                    else
                    {
                        item.Values["storage size width"] = 24;
                        item.Values["storage size height"] = 24;
                    }
                }

            }
            return Task.CompletedTask;
        }
    }
}
