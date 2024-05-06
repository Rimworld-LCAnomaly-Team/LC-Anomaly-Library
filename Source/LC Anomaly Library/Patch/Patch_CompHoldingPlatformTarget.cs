using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Patch
{
    public class Patch_CompHoldingPlatformTarget
    {

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(CompHoldingPlatformTarget), "Notify_HeldOnPlatform", new Type[] { typeof(ThingOwner) })]
        public static IEnumerable<CodeInstruction> SomePatch(IEnumerable<CodeInstruction> instructions)
        {

            return instructions;


        }
    }


    
}