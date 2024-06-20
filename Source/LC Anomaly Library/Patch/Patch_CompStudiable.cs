using HarmonyLib;
using LCAnomalyLibrary.Comp;
using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Patch
{
    [HarmonyPatch(typeof(CompStudiable), nameof(CompStudiable.Study))]
    public class Patch_CompStudiable_Study
    {
        //TODO Patch位置好像不对，但不急，先实现功能
        static void Postfix(CompStudiable __instance, Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0f)
        {
            LC_CompEntity entity = __instance.Pawn.TryGetComp<LC_CompEntity>();

            if(entity != null)
                entity.Notify_Studied(studier);
        }
    }
}
