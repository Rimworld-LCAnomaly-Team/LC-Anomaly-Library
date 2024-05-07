using HarmonyLib;
using LCAnomalyLibrary.Comp;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace LCAnomalyLibrary.Patch
{

    [HarmonyPatch(typeof(CompHoldingPlatformTarget), nameof(CompHoldingPlatformTarget.Notify_HeldOnPlatform))]
    public class Patch_CompHoldingPlatformTarget
    {
        static bool Prefix(ThingOwner newOwner, CompHoldingPlatformTarget __instance)
        {
            __instance.targetHolder = null;
            Pawn pawn = null;
            if (__instance.parent is Pawn pawn2)
            {
                pawn2.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
                PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn2);
                pawn = pawn2;
            }

            if (newOwner != null)
            {
                if (__instance.Props.heldPawnKind != null)
                {
                    Pawn pawn3 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(__instance.Props.heldPawnKind, Faction.OfEntities, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, 0f));
                    newOwner.TryAdd(pawn3);
                    pawn3.TryGetComp<CompHoldingPlatformTarget>()?.Notify_HeldOnPlatform(newOwner);
                    pawn = pawn3;


                    if (__instance.Props.heldPawnKind == PawnKindDefOf.Revenant)
                    {
                        CompBiosignatureOwner compBiosignatureOwner = __instance.parent.TryGetComp<CompBiosignatureOwner>();
                        if (compBiosignatureOwner != null)
                        {
                            pawn3.TryGetComp<CompRevenant>().biosignature = compBiosignatureOwner.biosignature;
                        }

                        if (pawn3.TryGetComp<CompStudiable>(out var comp))
                        {
                            comp.lastStudiedTick = Find.TickManager.TicksGame;
                        }
                    }
                    //TODO 测试代码 充满冗余，需要正式分函数处理
                    else
                    {
                        LC_CompEgg egg = __instance.parent.GetComp<LC_CompEgg>();
                        if (egg != null)
                        {
                            if (egg.ShouldTransferBiosignature)
                            {
                                CompBiosignatureOwner compBiosignatureOwner = __instance.parent.TryGetComp<CompBiosignatureOwner>();
                                if (compBiosignatureOwner != null)
                                {
                                    pawn3.TryGetComp<LC_CompEntity>().biosignature = compBiosignatureOwner.biosignature;
                                    Log.Warning("Patch_CompHoldingPlatformTarget:::传递生物特征成功");
                                }

                                if (pawn3.TryGetComp<CompStudiable>(out var comp))
                                {
                                    comp.lastStudiedTick = Find.TickManager.TicksGame;
                                }
                            }

                        }
                    }

                    Find.HiddenItemsManager.SetDiscovered(pawn3.def);
                    __instance.parent.Destroy();
                }

                __instance.containmentMode = EntityContainmentMode.Study;
            }

            if (pawn != null && __instance.HeldPlatform != null)
            {
                pawn.GetLord()?.Notify_PawnLost(pawn, PawnLostCondition.MadePrisoner);
                pawn.TryGetComp<CompActivity>()?.Notify_HeldOnPlatform();
                Find.StudyManager.UpdateStudiableCache(__instance.HeldPlatform, __instance.HeldPlatform.Map);
                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CapturingEntities, KnowledgeAmount.Total);
                LessonAutoActivator.TeachOpportunity(ConceptDefOf.ContainingEntities, OpportunityType.Important);
            }

            return false;
        }

    }

}