using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Recipe
{
    public class Recipe_ExtractCogito : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            Pawn pawn = thing as Pawn;

            if (pawn == null)
            {
                return false;
            }

            if (pawn.Faction == Faction.OfEntities)
            {
                return false;
            }

            if (pawn.health.hediffSet.HasHediff(Defs.HediffDefOf.CogitoExtracted))
            {
                return false;
            }

            return base.AvailableOnNow(thing, part);
        }

        public override AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
        {
            if (thing is Pawn pawn && pawn.DevelopmentalStage.Baby())
            {
                return "TooSmall".Translate();
            }

            return base.AvailableReport(thing, part);
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            Hediff hediff = HediffMaker.MakeHediff(Defs.HediffDefOf.CogitoExtracted, pawn);
            hediff.Severity = 1f;
            pawn.health.AddHediff(hediff);
            OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
            if (IsViolationOnPawn(pawn, part, Faction.OfPlayer))
            {
                ReportViolation(pawn, billDoer, pawn.HomeFaction, -1, Defs.HistoryEventDefOf.CogitoExtracted);
            }
        }

        protected override void OnSurgerySuccess(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (!GenPlace.TryPlaceThing(ThingMaker.MakeThing(Defs.ThingDefOf.Cogito), pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near))
            {
                Log.Error("Could not drop cogito near " + pawn.PositionHeld);
            }
        }
    }
}