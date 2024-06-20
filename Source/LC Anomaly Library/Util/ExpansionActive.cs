using Verse;

namespace LCAnomalyLibrary.Util
{
    public static class ExpansionActive
    {
        public static bool SevenSinEntityActive => ModsConfig.IsActive("DarthCY.LC.SevenSinEntity");

        public static bool MeatLanternActive => ModsConfig.IsActive("DarthCY.LC.MeatLantern");
    }
}
