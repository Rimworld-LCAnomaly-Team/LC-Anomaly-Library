using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using LCAnomalyLibrary.Comp;
using System.Text;

namespace LCAnomalyLibrary.Building
{
    public class Building_QliphothIndicator : Verse.Building
    {
        private bool initalized;

        protected int qliphothCounter;
        public int QliphothCounter
        {
            get => qliphothCounter;
            set
            {
                if(value ==  qliphothCounter) 
                    return;

                if(value < 0) 
                { 
                    qliphothCounter = 0;
                }
                else
                {
                    qliphothCounter = value;
                    Log.Message($"逆卡巴拉计数器设备值变更为：{qliphothCounter}");
                }
            }
        }

        private CompFacility facilityComp;

        private CompPowerTrader powerComp;

        private CompCableConnection cableConnection;

        public CompFacility FacilityComp => facilityComp ?? (facilityComp = GetComp<CompFacility>());

        public CompPowerTrader Power => powerComp ?? (powerComp = GetComp<CompPowerTrader>());

        public List<Thing> Platforms => FacilityComp.LinkedBuildings;

        public CompCableConnection CableConnection => cableConnection ?? (cableConnection = GetComp<CompCableConnection>());

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                Initialize();
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            FacilityComp.OnLinkAdded -= OnLinkAdded;
            FacilityComp.OnLinkRemoved -= OnLinkRemoved;
            initalized = false;
        }

        private void Initialize()
        {
            if (initalized)
            {
                return;
            }

            initalized = true;
            FacilityComp.OnLinkAdded += OnLinkAdded;
            FacilityComp.OnLinkRemoved += OnLinkRemoved;
            foreach (Thing platform in Platforms)
            {
                if (platform is Building_HoldingPlatform building_HoldingPlatform)
                {
                    building_HoldingPlatform.innerContainer.OnContentsChanged += RebuildCables;
                }
            }

            RebuildCables();
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            if (!initalized)
            {
                Initialize();
            }
        }

        private void OnLinkRemoved(CompFacility facility, Thing thing)
        {
            if (thing is Building_HoldingPlatform building_HoldingPlatform)
            {
                building_HoldingPlatform.innerContainer.OnContentsChanged -= RebuildCables;
                RebuildCables();
            }
        }

        private void OnLinkAdded(CompFacility facility, Thing thing)
        {
            if (thing is Building_HoldingPlatform building_HoldingPlatform)
            {
                building_HoldingPlatform.innerContainer.OnContentsChanged += RebuildCables;
                RebuildCables();
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(250))
            {
                //TODO 这里可以更新计数器
                UpdateQliphothCounter();
                //containedBioferrite = Mathf.Min(containedBioferrite + BioferritePerDay / 60000f * 250f, 60f);
            }
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (stringBuilder.Length != 0)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.Append("QliphothCounter".Translate());
            stringBuilder.Append($"：{qliphothCounter}");
            return stringBuilder.ToString();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
        }

        public override void Notify_DefsHotReloaded()
        {
            base.Notify_DefsHotReloaded();
            RebuildCables();
        }

        private void RebuildCables()
        {
            CableConnection.RebuildCables(Platforms, (Thing thing) => thing is Building_HoldingPlatform building_HoldingPlatform && building_HoldingPlatform.Occupied);
            UpdateQliphothCounter();
        }

        /// <summary>
        /// 更新逆卡巴拉计数器
        /// </summary>
        private void UpdateQliphothCounter()
        {
            foreach (Thing thing in Platforms)
            {
                var platform = thing as Building_HoldingPlatform;
                if (platform != null)
                {
                    var entity = platform.HeldPawn.TryGetComp<LC_CompEntity>();
                    if (entity != null)
                    {
                        QliphothCounter = entity.QliphothCountCurrent;
                    }
                    else
                    {
                        QliphothCounter = 0;
                    }
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref qliphothCounter, "qliphothCounterCurrent", 0);
        }
    }
}
