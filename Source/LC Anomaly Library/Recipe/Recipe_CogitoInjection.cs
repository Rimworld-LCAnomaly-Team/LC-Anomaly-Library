using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Recipe
{
    public class Recipe_CogitoInjection : Recipe_Surgery
    {
        public override bool CompletableEver(Pawn surgeryTarget)
        {
            if (surgeryTarget.health.hediffSet.HasHediff(Defs.HediffDefOf.CogitoInjected))
            {
                return false;
            }

            return base.CompletableEver(surgeryTarget);
        }

        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (thing.MapHeld == null)
            {
                return false;
            }

            if (thing.MapHeld.listerThings.ThingsOfDef(Defs.ThingDefOf.Cogito).Count == 0)
            {
                return false;
            }

            if (thing is Pawn pawn && pawn.health.hediffSet.HasHediff(Defs.HediffDefOf.CogitoInjected))
            {
                return false;
            }

            return base.AvailableOnNow(thing, part);
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            Hediff hediff = HediffMaker.MakeHediff(Defs.HediffDefOf.CogitoInjected, pawn);
            hediff.Severity = 1f;
            pawn.health.AddHediff(hediff);
            OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
            if (IsViolationOnPawn(pawn, part, Faction.OfPlayer))
            {
                ReportViolation(pawn, billDoer, pawn.HomeFaction, -1, Defs.HistoryEventDefOf.CogitoInjected);
            }
        }
    }
}