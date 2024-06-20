using HarmonyLib;
using LCAnomalyLibrary.Comp;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Patch
{
    [HarmonyPatch(typeof(InvisibilityUtility), nameof(InvisibilityUtility.IsPsychologicallyInvisible))]
    public class Patch_InvisibilityUtility_IsPsychologicallyInvisible
    {
        private static bool Prefix(Pawn pawn, ref bool __result)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                HediffComp hediffComp = hediffs[i].TryGetComp<HediffComp>();

                if (hediffComp != null)
                {
                    if (hediffComp is HediffComp_Invisibility)
                    {
                        if (!((HediffComp_Invisibility)hediffComp).PsychologicallyVisible)
                        {
                            __result = true;
                            return false;
                        }
                    }
                    else if (hediffComp is LC_HediffComp_FakeInvisibility)
                    {
                        if (!((LC_HediffComp_FakeInvisibility)hediffComp).PsychologicallyVisible)
                        {
                            //Log.Warning("deep dark invisibility");
                            __result = true;
                            return false;
                        }
                    }
                }
            }

            __result = false;
            return false;
        }
    }
}