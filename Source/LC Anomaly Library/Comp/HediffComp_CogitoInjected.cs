using LCAnomalyLibrary.Misc;
using LCAnomalyLibrary.Util;
using RimWorld;
using System.Linq;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// Cogito被注射状态的Hediff
    /// </summary>
    public class HediffComp_CogitoInjected : HediffComp
    {
        public HediffCompProperties_CogitoInjected Props => (HediffCompProperties_CogitoInjected)props;

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            TurnIntoAnomaly();
        }

        /// <summary>
        /// 注射提取后变成异常的方法
        /// </summary>
        private void TurnIntoAnomaly()
        {
            EffecterDefOf.MeatExplosionSmall.SpawnMaintained(Pawn.Position, Pawn.MapHeld);

            //判定生成大罪生物
            if (ExpansionActive.SevenSinEntityActive)
            {
                var list1 = ExtractUtil.Get_AnomlayLvl2DefList_SevenSin("ZAYIN").
                    Union(ExtractUtil.Get_AnomlayLvl2DefList_SevenSin("TETH")).ToList();

                if (list1.Count > 0)
                {
                    if (Rand.Chance(Props.turnSevenSinEnitityChance))
                    {
                        Pawn pawn = PawnGenerator.GeneratePawn(list1.RandomElement(), Faction.OfEntities);
                        pawn.Name = Pawn.Name;
                        GenSpawn.Spawn(pawn, Pawn.Position, Pawn.MapHeld);

                        Log.Message($"Congito注射提取：{Pawn.Name}变成了大罪生物{pawn.def.defName.Translate()}");
                        Pawn.DeSpawn();
                        return;
                    }
                    else
                    {
                        Log.Message("Congito注射提取：概率判定不生成大罪生物");
                    }
                }
                else
                {
                    Log.Warning("Congito注射提取：ZAYIN和TETH的大罪生物列表为空");
                }
            }

            //判定生成异想体
            var list2 = ExtractUtil.Get_AnomlayLvl2DefList_Cogito("ZAYIN").
                Union(ExtractUtil.Get_AnomlayLvl2DefList_Cogito("TETH")).ToList();
            if (list2.Count > 0)
                ((LC_FX_Escaping)GenSpawn.Spawn(list2.RandomElement(), Pawn.Position, Pawn.MapHeld)).InitWith(null, false);
            else
                Log.Warning("ZAYIN和TETH的非工具类异想体列表为空");

            Pawn.DeSpawn();
        }
    }
}
