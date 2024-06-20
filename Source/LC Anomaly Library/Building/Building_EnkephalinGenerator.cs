using Verse;

namespace LCAnomalyLibrary.Building
{
    public class Building_EnkephalinGenerator : Verse.Building
    {
        [Unsaved(false)]
        private CompHeatPusherPowered heatPusher;

        private CompHeatPusherPowered HeatPusher => heatPusher ?? (heatPusher = GetComp<CompHeatPusherPowered>());

        public override bool IsWorking()
        {
            return HeatPusher.ShouldPushHeatNow;
        }
    }
}