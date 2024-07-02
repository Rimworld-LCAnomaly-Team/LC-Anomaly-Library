using LCAnomalyLibrary.GameComponent;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class CompCogitoBucketSpawner : LC_CompRequireThingSpawner
    {
        public override IEnumerable<Verse.Gizmo> CompGetGizmosExtra()
        {
            foreach(var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: SelfExplode",
                    action = delegate
                    {
                        parent.GetComp<CompExplosive>()?.StartWick();
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "DEV: WaringPoints +10",
                    action = delegate
                    {
                        Current.Game.GetComponent<GameComponent_LC>().CurWarningPoints += 10;
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "DEV: WaringPoints -10",
                    action = delegate
                    {
                        Current.Game.GetComponent<GameComponent_LC>().CurWarningPoints -= 10;
                    }
                };
            }

            if (HasRequireThingInstalled)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "ExtractInstantFromNerveLabel".Translate();
                command_Action.defaultDesc = "ExtractInstantFromNerveDesc".Translate();
                command_Action.icon = PropsSpawner.thingToSpawn.uiIcon;
                command_Action.action = delegate
                {
                    innerContainer.ClearAndDestroyContents();

                    Thing thing = ThingMaker.MakeThing(Defs.ThingDefOf.Cogito);
                    thing.stackCount = 10;
                    GenPlace.TryPlaceThing(thing, parent.PositionHeld, parent.MapHeld, ThingPlaceMode.Near);
                };
                yield return command_Action;
            }
        }
    }
}
