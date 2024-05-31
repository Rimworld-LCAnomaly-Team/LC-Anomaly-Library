using RimWorld;
using System;
using Verse;

namespace LCAnomalyLibrary.Misc
{
    public abstract class LC_FX_Dying : Thing
    {
        protected int bioSignature;

        protected int completeTick;

        protected bool isEscaping;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref completeTick, "completeTick", 0);
            Scribe_Values.Look(ref bioSignature, "bioSignature", 0);
        }

        /// <summary>
        /// 对对应的Pawn初始化效果
        /// </summary>
        /// <param name="targetPawn"></param>
        public abstract void InitWith(Pawn targetPawn);


        public override void Tick()
        {
            //动画播完就执行操作
            if (Find.TickManager.TicksGame >= completeTick)
            {
                Complete();
            }
        }

        public virtual void Complete()
        {
            throw new NotImplementedException("LC_FX_Dying:Complete():::Not implemented yet");
        }
    }
}
