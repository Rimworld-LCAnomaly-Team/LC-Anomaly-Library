using LCAnomalyLibrary.GameComponent;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class CompCogitoBucketSpawner : ThingComp
    {
        protected int ticksUntilSpawn;

        public CompProperties_CogitoBucketSpawner PropsSpawner => (CompProperties_CogitoBucketSpawner)props;

        protected bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;

        public Thing ThingRequireInstalled;
        public bool HasRequireThingInstalled => ThingRequireInstalled != null;


        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (!respawningAfterLoad)
            {
                ResetCountdown();
            }
        }

        public override void CompTick()
        {
            if(HasRequireThingInstalled)
                TickInterval(1);
        }

        public override void CompTickRare()
        {
            if(HasRequireThingInstalled)
                TickInterval(250);
        }

        private void TickInterval(int interval)
        {
            if (!parent.Spawned)
            {
                return;
            }

            CompCanBeDormant comp = parent.GetComp<CompCanBeDormant>();
            if (comp != null)
            {
                if (!comp.Awake)
                {
                    return;
                }
            }
            else if (parent.Position.Fogged(parent.Map))
            {
                return;
            }

            if (!PropsSpawner.requiresPower || PowerOn)
            {
                ticksUntilSpawn -= interval;
                CheckShouldSpawn();
            }
        }

        private void CheckShouldSpawn()
        {
            if (ticksUntilSpawn <= 0)
            {
                ResetCountdown();
                TryDoSpawn();
            }
        }

        public bool TryDoSpawn()
        {
            if (!parent.Spawned)
            {
                return false;
            }

            if (PropsSpawner.spawnMaxAdjacent >= 0)
            {
                int num = 0;
                for (int i = 0; i < 9; i++)
                {
                    IntVec3 c = parent.Position + GenAdj.AdjacentCellsAndInside[i];
                    if (!c.InBounds(parent.Map))
                    {
                        continue;
                    }

                    List<Thing> thingList = c.GetThingList(parent.Map);
                    for (int j = 0; j < thingList.Count; j++)
                    {
                        if (thingList[j].def == PropsSpawner.thingToSpawn)
                        {
                            num += thingList[j].stackCount;
                            if (num >= PropsSpawner.spawnMaxAdjacent)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            if (TryFindSpawnCell(parent, PropsSpawner.thingToSpawn, PropsSpawner.spawnCount, out var result))
            {
                Thing thing = ThingMaker.MakeThing(PropsSpawner.thingToSpawn);
                thing.stackCount = PropsSpawner.spawnCount;
                if (thing == null)
                {
                    Log.Error("Could not spawn anything for " + parent);
                }

                if (PropsSpawner.inheritFaction && thing.Faction != parent.Faction)
                {
                    thing.SetFaction(parent.Faction);
                }

                GenPlace.TryPlaceThing(thing, result, parent.Map, ThingPlaceMode.Direct, out var lastResultingThing);
                if (PropsSpawner.spawnForbidden)
                {
                    lastResultingThing.SetForbidden(value: true);
                }

                if (PropsSpawner.showMessageIfOwned && parent.Faction == Faction.OfPlayer)
                {
                    Messages.Message("MessageCompSpawnerSpawnedItem".Translate(PropsSpawner.thingToSpawn.LabelCap), thing, MessageTypeDefOf.PositiveEvent);
                }

                return true;
            }

            return false;
        }

        public static bool TryFindSpawnCell(Thing parent, ThingDef thingToSpawn, int spawnCount, out IntVec3 result)
        {
            foreach (IntVec3 item in GenAdj.CellsAdjacent8Way(parent).InRandomOrder())
            {
                if (!item.Walkable(parent.Map))
                {
                    continue;
                }

                Verse.Building edifice = item.GetEdifice(parent.Map);
                if ((edifice != null && thingToSpawn.IsEdifice()) || (edifice is Building_Door building_Door && !building_Door.FreePassage) || (parent.def.passability != Traversability.Impassable && !GenSight.LineOfSight(parent.Position, item, parent.Map)))
                {
                    continue;
                }

                bool flag = false;
                List<Thing> thingList = item.GetThingList(parent.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    Thing thing = thingList[i];
                    if (thing.def.category == ThingCategory.Item && (thing.def != thingToSpawn || thing.stackCount > thingToSpawn.stackLimit - spawnCount))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    result = item;
                    return true;
                }
            }

            result = IntVec3.Invalid;
            return false;
        }

        private void ResetCountdown()
        {
            ticksUntilSpawn = PropsSpawner.spawnIntervalRange.RandomInRange;
        }

        public override void PostExposeData()
        {
            string text = (PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (PropsSpawner.saveKeysPrefix + "_"));
            Scribe_Values.Look(ref ticksUntilSpawn, text + "ticksUntilSpawn", 0);
            Scribe_Deep.Look(ref ThingRequireInstalled, "ThingRequireInstalled");
        }

        public override IEnumerable<Verse.Gizmo> CompGetGizmosExtra()
        {
            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "DEV: Spawn " + PropsSpawner.thingToSpawn.label;
                command_Action.icon = PropsSpawner.thingToSpawn.uiIcon;
                command_Action.action = delegate
                {
                    ResetCountdown();
                    TryDoSpawn();
                };
                yield return command_Action;

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
                    defaultLabel = "DEV: ForceInstallNerve",
                    action = delegate
                    {
                        if (!HasRequireThingInstalled)
                            ThingRequireInstalled = ThingMaker.MakeThing(Defs.ThingDefOf.BrainSpinalNerve);
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "DEV: ForceRemoveNerve",
                    action = delegate
                    {
                        if (HasRequireThingInstalled)
                        {
                            var thing = ThingRequireInstalled;
                            ThingRequireInstalled = null;

                            GenPlace.TryPlaceThing(thing, parent.PositionHeld, parent.MapHeld, ThingPlaceMode.Near);
                        }
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
                command_Action.icon = ThingRequireInstalled.def.uiIcon;
                command_Action.action = delegate
                {
                    ThingRequireInstalled.Destroy();
                    ThingRequireInstalled = null;

                    Thing thing = ThingMaker.MakeThing(Defs.ThingDefOf.Cogito);
                    thing.stackCount = 10;
                    GenPlace.TryPlaceThing(thing, parent.PositionHeld, parent.MapHeld, ThingPlaceMode.Near);
                };
                yield return command_Action;
            }


        }

        public override string CompInspectStringExtra()
        {
            if (PropsSpawner.writeTimeLeftToSpawn && (!PropsSpawner.requiresPower || PowerOn))
            {
                if (HasRequireThingInstalled)
                    return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(PropsSpawner.thingToSpawn, null, PropsSpawner.spawnCount)).Resolve() + ": " + ticksUntilSpawn.ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor);
                else
                    return "RequireThingNotInstalledText".Translate() + PropsSpawner.thingRequire.label.Translate();
            }

            return null;
        }
    }
}
