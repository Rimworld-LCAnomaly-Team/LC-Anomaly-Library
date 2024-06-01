using Verse;

namespace LCAnomalyLibrary.Defs
{

    public abstract class ThingDef_LCAnomalyBase : ThingDef
    {
        public readonly string anomalyLevelTag;
    }

    public abstract class PawnKindDef_LCAnomalyBase : PawnKindDef
    {
        public readonly string anomalyLevelTag;
    }

    public class ThingDef_AnomalyEntity : PawnKindDef_LCAnomalyBase
    {
    }

    public class ThingDef_AnomalyEgg : ThingDef_LCAnomalyBase
    {
    }

    public class ThingDef_AnomalyTool : ThingDef_LCAnomalyBase
    {
    }

    public class ThingDef_AnomalyEntity_Spawn : ThingDef_LCAnomalyBase
    {
    }

    public class PawnKindDef_AnomalyEntity_SevenSin : ThingDef_AnomalyEntity
    {
    }
}
