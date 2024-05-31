using LCAnomalyLibrary.Defs;
using LCAnomalyLibrary.Misc;
using LCAnomalyLibrary.Util;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class HediffComp_CogitoInjected : HediffComp
    {
        public HediffCompProperties_CogitoInjected Props => (HediffCompProperties_CogitoInjected)props;

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            TurnIntoAnomaly();
        }

        private void TurnIntoAnomaly()
        {
            ExtractUtil.CheckHasInitial();
            var list = ExtractUtil.AnomlayLvl2DefList_Cogito["ZAYIN"].Union(ExtractUtil.AnomlayLvl2DefList_Cogito["TETH"]).ToList();

            if (list.Count > 0)
            {
                ((LC_FX_Escaping)GenSpawn.Spawn(list.RandomElement(), Pawn.Position, Pawn.MapHeld)).InitWith(null, false);
                Pawn.Kill(null);
            }
            else
            {
                Log.Error("列表为空");
            }

        }
    }
}
