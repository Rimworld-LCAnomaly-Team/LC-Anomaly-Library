using Verse;

namespace LCAnomalyLibrary.Util
{
    public static class Curves
    {
        public static readonly SimpleCurve JoinEscapeChanceFromEscapeIntervalCurve =
        [
            new CurvePoint(120f, 0.33f),
            new CurvePoint(60f, 0.5f),
            new CurvePoint(10f, 0.9f)
        ];
    }
}