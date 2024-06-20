using HarmonyLib;
using LCAnomalyLibrary.Comp;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace LCAnomalyLibrary.Patch
{
    [HarmonyPatch(typeof(CompHoldingPlatformTarget), nameof(CompHoldingPlatformTarget.Notify_HeldOnPlatform))]
    public class Patch_CompHoldingPlatformTarget_Notify_HeldOnPlatform
    {
        static bool Prefix(ThingOwner newOwner, CompHoldingPlatformTarget __instance)
        {
            //Log.Warning("Patch_CompHoldingPlatformTarget.Notify_HeldOnPlatform 注入成功");

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
                                    //Log.Warning("Patch_CompHoldingPlatformTarget:::传递生物特征成功");
                                }

                                if (pawn3.TryGetComp<CompStudiable>(out var comp))
                                {
                                    comp.lastStudiedTick = Find.TickManager.TicksGame;
                                }
                            }
                        }
                    }

                    //调用独有的绑上平台的回调方法
                    LC_CompEntity tmpComp = pawn3.TryGetComp<LC_CompEntity>();
                    if (tmpComp != null)
                        tmpComp.Notify_Holded();
                    else
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

    [HarmonyPatch(typeof(CompHoldingPlatformTarget), nameof(CompHoldingPlatformTarget.Escape))]
    public class Patch_CompHoldingPlatformTarget_Escape
    {
        static bool Prefix(bool initiator, CompHoldingPlatformTarget __instance)
        {
            //Log.Warning("Patch_CompHoldingPlatformTarget.Escape 注入成功");

            List<Pawn> list = new List<Pawn>();
            List<Verse.Building> list2 = new List<Verse.Building> { __instance.HeldPlatform };
            __instance.HeldPlatform.EjectContents();
            if (!(__instance.parent is Pawn pawn))
            {
                Log.Warning("return");
                return false;
            }

            pawn.health.overrideDeathOnDownedChance = 0f;
            list.Add(pawn);
            __instance.isEscaping = true;
            if (__instance.Props.lookForTargetOnEscape && !pawn.Downed)
            {
                Pawn enemyTarget = (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && (int)x.def.race.intelligence >= 1, 0f, 9999f, default(IntVec3), float.MaxValue, canBashDoors: true, canTakeTargetsCloserThanEffectiveMinRange: true, canBashFences: true);
                pawn.mindState.enemyTarget = enemyTarget;
            }

            ThingComp compEntity = pawn.TryGetComp<CompRevenant>();
            if(compEntity != null)
            {
                Log.Warning("Revenant escaped");
                ((CompRevenant)compEntity).revenantState = RevenantState.Escape;
                pawn.GetInvisibilityComp()?.BecomeVisible(instant: true);
            }
            else
            {
                compEntity = pawn.TryGetComp<LC_CompEntity>();
                if(compEntity != null)
                {
                    //Log.Warning("LC Entity escaped");
                    ((LC_CompEntity)compEntity).Notify_Escaped();
                }
            }

            if (!initiator)
            {
                return false;
            }

            Room room = pawn.GetRoom();
            if (room == null)
            {
                return false;
            }

            foreach (Building_HoldingPlatform item in room.ContainedAndAdjacentThings.Where((Thing x) => x is Building_HoldingPlatform).ToList())
            {
                Pawn heldPawn = item.HeldPawn;
                if (heldPawn == null || heldPawn == pawn)
                {
                    continue;
                }

                CompHoldingPlatformTarget compHoldingPlatformTarget = heldPawn.TryGetComp<CompHoldingPlatformTarget>();
                if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
                {
                    float num = ContainmentUtility.InitiateEscapeMtbDays(heldPawn);
                    if (!(num <= 0f) && Rand.Chance(Util.Curves.JoinEscapeChanceFromEscapeIntervalCurve.Evaluate(num)))
                    {
                        list.Add(heldPawn);
                        list2.Add(compHoldingPlatformTarget.HeldPlatform);
                        compHoldingPlatformTarget.Escape(initiator: false);
                    }
                }
            }

            if(compEntity is LC_CompEntity)
            {
                if (!((LC_CompEntity)compEntity).Props.shoundNotifyWhenEscape)
                    return false;
            }
            Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("LetterLabelEscapingFromHoldingPlatform".Translate(), "LetterEscapingFromHoldingPlatform".Translate(list.Select((Pawn p) => p.LabelCap).ToLineList("  - ")), LetterDefOf.ThreatBig, list2));
            
            return false;
        }

    }

    [HarmonyPatch(typeof(CompHoldingPlatformTarget), "CaptivityTick")]
    public class Patch_CompHoldingPlatformTarget_CaptivityTick()
    {
        static bool Prefix(Pawn pawn, CompHoldingPlatformTarget __instance)
        {
            pawn.mindState.entityTicksInCaptivity++;
            if (__instance.targetHolder is Building_HoldingPlatform building_HoldingPlatform && building_HoldingPlatform != __instance.HeldPlatform && building_HoldingPlatform.Occupied)
            {
                __instance.targetHolder = null;
            }

            if (__instance.parent.IsHashIntervalTick(2500))
            {
                float num = ContainmentUtility.InitiateEscapeMtbDays(pawn);
                if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
                {
                    LC_CompEntity compEntity = pawn.TryGetComp<LC_CompEntity>();
                    if (compEntity != null)
                    {
                        Log.Warning("Patch：检测到一次实体随机出逃，因为是LC实体所以没有执行");
                        return false;
                    }
                    else
                    {
                        __instance.Escape(initiator: true);
                    }
                }
            }

            return false;
        }
    }
}