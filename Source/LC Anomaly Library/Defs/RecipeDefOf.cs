using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    [DefOf]
    public static class RecipeDefOf
    {
        [MayRequireAnomaly]
        public static RecipeDef ExtractCogito;

        static RecipeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
        }
    }
}