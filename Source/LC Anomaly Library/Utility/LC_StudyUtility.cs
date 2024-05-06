using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse;
using LCAnomalyLibrary.Comp;
using UnityEngine;
using LCAnomalyLibrary.Building;

namespace LCAnomalyLibrary.Utility
{
    public static class LC_StudyUtility
    {
        private static readonly HashSet<Pawn> tmpReservers = new HashSet<Pawn>();

        public static void TargetHoldingPlatformForEntity(Pawn carrier, Thing entity, bool transferBetweenPlatforms = false, Thing sourcePlatform = null)
        {
            Log.Message("LC_StudyUtility:::进入找收容单元方法-第1层");
            Find.Targeter.BeginTargeting(TargetingParameters.ForBuilding(), delegate (LocalTargetInfo t)
            {
                Log.Message("LC_StudyUtility:::进入找收容单元方法-第2层");
                if (carrier != null && !CanReserveForTransfer(t))
                {
                    Log.Message("LC_StudyUtility:::进入找收容单元方法-第3层");
                    Messages.Message("MessageHolderReserved".Translate(t.Thing.Label), MessageTypeDefOf.RejectInput);
                }
                else
                {
                    Log.Message("LC_StudyUtility:::进入找收容单元方法-第4层");
                    foreach (Thing item in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
                    {
                        Log.Message("LC_StudyUtility:::进入找收容单元方法-第5层");
                        Comp_LC_ContainingUnitTarget compHoldingPlatformTarget;
                        if (item is Building_LC_ContainingUnit building_HoldingPlatform && entity != building_HoldingPlatform.HeldPawn && (compHoldingPlatformTarget = building_HoldingPlatform.HeldPawn?.TryGetComp<Comp_LC_ContainingUnitTarget>()) != null && compHoldingPlatformTarget.targetHolder == t.Thing)
                        {
                            Messages.Message("MessageHolderReserved".Translate(t.Thing.Label), MessageTypeDefOf.RejectInput);
                            return;
                        }
                    }
                    Log.Message("LC_StudyUtility:::进入找收容单元方法-第6层");
                    Comp_LC_ContainingUnitTarget compHoldingPlatformTarget2 = entity.TryGetComp<Comp_LC_ContainingUnitTarget>();
                    if (compHoldingPlatformTarget2 != null)
                    {
                        Log.Message("LC_StudyUtility:::进入找收容单元方法-第7层");
                        compHoldingPlatformTarget2.targetHolder = t.Thing;
                    }

                    if (carrier != null)
                    {
                        Log.Message("LC_StudyUtility:::进入找收容单元方法-第8层");
                        Verse.AI.Job job = (transferBetweenPlatforms ? JobMaker.MakeJob(Defs.JobDefOf.LC_TransferBetweenEntityContainers, sourcePlatform, t, entity) : JobMaker.MakeJob(Defs.JobDefOf.LC_CarryToEntityContainer, t, entity));
                        job.count = 1;
                        carrier.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }

                    if (t.Thing != null && !t.Thing.SafelyContains(entity))
                    {
                        Log.Message("LC_StudyUtility:::进入找收容单元方法-第9层");
                        Messages.Message("MessageTargetBelowMinimumContainmentStrength".Translate(t.Thing.Label, entity.Label), MessageTypeDefOf.ThreatSmall);
                    }
                }
            }, delegate (LocalTargetInfo t)
            {
                if (ValidateTarget(t))
                {
                    GenDraw.DrawTargetHighlight(t);
                }
            }, ValidateTarget, null, null, BaseContent.ClearTex, playSoundOnAction: true, delegate (LocalTargetInfo t)
            {
                Comp_LC_EntityContainer compEntityHolder = t.Thing?.TryGetComp<Comp_LC_EntityContainer>();
                if (compEntityHolder == null)
                {
                    TaggedString label = "ChooseEntityHolder".Translate().CapitalizeFirst() + "...";
                    Widgets.MouseAttachedLabel(label);
                }
                else
                {
                    Pawn pawn = null;
                    Pawn reserver;
                    if (carrier != null)
                    {
                        pawn = t.Thing.Map.reservationManager.FirstRespectedReserver(t.Thing, carrier);
                    }
                    else if (t.Thing is Building_LC_ContainingUnit p && AlreadyReserved(p, out reserver))
                    {
                        pawn = reserver;
                    }

                    TaggedString label;
                    if (pawn != null)
                    {
                        label = string.Format("{0}: {1}", "EntityHolderReservedBy".Translate(), pawn.LabelShortCap);
                    }
                    else
                    {
                        label = "FloatMenuContainmentStrength".Translate() + ": " + StatDefOf.ContainmentStrength.Worker.ValueToString(compEntityHolder.ContainmentStrength, finalized: false);
                        label += "\n" + ("FloatMenuContainmentRequires".Translate(entity).CapitalizeFirst() + ": " + StatDefOf.MinimumContainmentStrength.Worker.ValueToString(entity.GetStatValue(StatDefOf.MinimumContainmentStrength), finalized: false)).Colorize(t.Thing.SafelyContains(entity) ? Color.white : Color.red);
                    }

                    Widgets.MouseAttachedLabel(label);
                }
            }, delegate
            {
                foreach (Verse.Building item2 in entity.MapHeld.listerBuildings.AllBuildingsColonistOfGroup(ThingRequest.ForDef(Defs.ThingDefOf.LC_ContaningUnit).group))
                {
                    if (ValidateTarget(item2) && (carrier == null || CanReserveForTransfer(item2)))
                    {
                        GenDraw.DrawArrowPointingAt(item2.DrawPos);
                    }
                }
            });
            bool CanReserveForTransfer(LocalTargetInfo t)
            {
                if (transferBetweenPlatforms)
                {
                    if (t.HasThing)
                    {
                        return carrier.CanReserve(t.Thing);
                    }

                    return false;
                }

                return true;
            }

            bool ValidateTarget(LocalTargetInfo t)
            {
                if (t.HasThing && t.Thing.TryGetComp(out Comp_LC_EntityContainer comp) && comp.HeldPawn == null)
                {
                    if (carrier != null)
                    {
                        return carrier.CanReserveAndReach(t.Thing, PathEndMode.Touch, Danger.Some);
                    }

                    return true;
                }

                return false;
            }
        }

        public static bool TryFindResearchBench(Pawn pawn, out Building_ResearchBench bench)
        {
            bench = (Building_ResearchBench)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Some), 9999f, (Thing t) => pawn.CanReserve(t) && (t.TryGetComp<CompPowerTrader>()?.PowerOn ?? true));
            return bench != null;
        }

        public static bool HoldingPlatformAvailableOnCurrentMap()
        {
            Map currentMap = Find.CurrentMap;
            if (currentMap == null)
            {
                return false;
            }

            foreach (Verse.Building item in currentMap.listerBuildings.allBuildingsColonist)
            {
                if (item.TryGetComp<Comp_LC_EntityContainer>(out var comp) && comp.Available && !AlreadyReserved(item, out var _))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsAnomalyResearchCategory(this KnowledgeCategoryDef def)
        {
            if (ModsConfig.AnomalyActive && def != null)
            {
                if (def != KnowledgeCategoryDefOf.Basic)
                {
                    return def == KnowledgeCategoryDefOf.Advanced;
                }

                return true;
            }

            return false;
        }

        public static bool AlreadyReserved(Thing p, out Pawn reserver)
        {
            tmpReservers.Clear();
            p.Map.reservationManager.ReserversOf(p, tmpReservers);
            reserver = tmpReservers.FirstOrDefault();
            if (reserver != null)
            {
                return true;
            }

            //TODO NULL!
            //Log.Warning(ThingRequest.ForDef(Defs.ThingDefOf.LC_ContaningUnit).ToString());
            //foreach (Thing item in p.Map.listerThings.ThingsInGroup(ThingRequest.ForDef(Defs.ThingDefOf.LC_ContaningUnit).group))
            //{
            //    if (item.TryGetComp<Comp_LC_ContainingUnitTarget>().targetHolder == p)
            //    {
            //        Log.Warning("6");
            //        reserver = item as Pawn;
            //        return true;
            //    }
            //}

            return false;
        }
    }
}
