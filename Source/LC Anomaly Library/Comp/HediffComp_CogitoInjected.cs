using LCAnomalyLibrary.Misc;
using LCAnomalyLibrary.Util;
using RimWorld;
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
            //判定概率生成大罪生物或异想体
            if (Rand.Chance(Props.turnSevenSinEnitityChance))
            {
                var list = ExtractUtil.Get_AnomlayLvl2DefList_SevenSin("ZAYIN").
                    Union(ExtractUtil.Get_AnomlayLvl2DefList_SevenSin("TETH")).ToList();

                if(list.Count > 0)
                {
                    Pawn pawn = PawnGenerator.GeneratePawn(list.RandomElement(), Faction.OfEntities);
                    GenSpawn.Spawn(pawn, Pawn.Position, Pawn.MapHeld);
                }
                else
                    Log.Error("ZAYIN和TETH的大罪生物列表为空");
            }
            else
            {
                var list = ExtractUtil.Get_AnomlayLvl2DefList_Cogito("ZAYIN").
                    Union(ExtractUtil.Get_AnomlayLvl2DefList_Cogito("TETH")).ToList();

                if (list.Count > 0)
                    ((LC_FX_Escaping)GenSpawn.Spawn(list.RandomElement(), Pawn.Position, Pawn.MapHeld)).InitWith(null, false);
                else
                    Log.Error("ZAYIN和TETH的非工具类异想体列表为空");
            }

            Pawn.Kill(null);
        }
    }
}
