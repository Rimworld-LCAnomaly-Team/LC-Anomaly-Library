using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    public class EntityCodexEntryDef : RimWorld.EntityCodexEntryDef
    {
        //public EntityCategoryDef category;

        //public bool startDiscovered;

        //public List<ThingDef> linkedThings = new List<ThingDef>();

        //private List<IncidentDef> linkedIncidents = new List<IncidentDef>();

        //public List<ResearchProjectDef> discoveredResearchProjects = new List<ResearchProjectDef>();

        //public List<IncidentDef> provocationIncidents = new List<IncidentDef>();

        //public EntityDiscoveryType discoveryType;

        //public bool allowDiscoveryWhileMapGenerating;

        //public int orderInCategory = 9999999;

        //public List<AnomalyPlaystyleDef> hideInPlaystyles = new List<AnomalyPlaystyleDef>();

        //private ThingDef useDescriptionFrom;

        //[NoTranslate]
        //private string uiIconPath;

        //public Texture2D icon;

        //public Texture2D silhouette;

        //private const string SilhouetteTexPathSuffix = "_Silhouette";

        //public string Description
        //{
        //    get
        //    {
        //        if (useDescriptionFrom == null)
        //        {
        //            return description;
        //        }

        //        return useDescriptionFrom.description;
        //    }
        //}

        //public bool Discovered => Find.EntityCodex.Discovered((RimWorld.EntityCodexEntryDef) this);

        //public bool Visible
        //{
        //    get
        //    {
        //        if (Current.ProgramState == ProgramState.Playing)
        //        {
        //            return !hideInPlaystyles.Contains(Find.Storyteller.difficulty.AnomalyPlaystyleDef);
        //        }

        //        return true;
        //    }
        //}

        //public override void PostLoad()
        //{
        //    LongEventHandler.ExecuteWhenFinished(delegate
        //    {
        //        icon = (uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(uiIconPath));
        //        silhouette = (uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(uiIconPath + "_Silhouette"));
        //    });
        //}

        //public override void ResolveReferences()
        //{
        //    foreach (ThingDef linkedThing in linkedThings)
        //    {
        //        if (linkedThing.entityCodexEntry == null)
        //        {
        //            linkedThing.entityCodexEntry = (RimWorld.EntityCodexEntryDef)this;
        //            continue;
        //        }

        //        Log.Error("EntityCodexEntryDef " + defName + " is linked to " + linkedThing.defName + " but " + linkedThing.defName + " is already linked to " + linkedThing.entityCodexEntry.defName + ".");
        //    }

        //    foreach (IncidentDef linkedIncident in linkedIncidents)
        //    {
        //        if (linkedIncident.codexEntry == null)
        //        {
        //            linkedIncident.codexEntry = (RimWorld.EntityCodexEntryDef)this;
        //            continue;
        //        }

        //        Log.Error("EntityCodexEntryDef " + defName + " is linked to " + linkedIncident.defName + " but " + linkedIncident.defName + " is already linked to " + linkedIncident.codexEntry.defName + ".");
        //    }
        //}

        //public override IEnumerable<string> ConfigErrors()
        //{
        //    foreach (string item in base.ConfigErrors())
        //    {
        //        yield return item;
        //    }

        //    if (category == null)
        //    {
        //        yield return "category is null.";
        //    }

        //    if (uiIconPath.NullOrEmpty())
        //    {
        //        yield return "missing icon.";
        //    }
            
        //}

        ///// <summary>
        ///// 强转成RimWorld.EntityCodexEntryDef的规则
        ///// </summary>
        ///// <param name="self"></param>
        //public static explicit operator RimWorld.EntityCodexEntryDef(EntityCodexEntryDef self)
        //{
        //    //var target = new RimWorld.EntityCodexEntryDef();
        //    RimWorld.EntityCodexEntryDef target = new RimWorld.EntityCodexEntryDef();

        //    //Def def = (Def)self.MemberwiseClone();
        //    //target = (RimWorld.EntityCodexEntryDef)def;

        //    //Def部分
        //    target.defName = self.defName;

        //    target.label = self.label;

        //    target.description = self.description;

        //    target.ignoreConfigErrors = self.ignoreConfigErrors;

        //    target.ignoreIllegalLabelCharacterConfigError = self.ignoreIllegalLabelCharacterConfigError;

        //    target.modExtensions = self.modExtensions;

        //    target.shortHash = self.shortHash;

        //    target.index = self.index;

        //    target.modContentPack = self.modContentPack;

        //    target.fileName = self.fileName;

        //    var fieldInfo0 = typeof(RimWorld.EntityCodexEntryDef).GetField("cachedLabelCap", BindingFlags.Instance | BindingFlags.NonPublic);
        //    fieldInfo0.SetValue(target, self.cachedLabelCap);

        //    target.generated = self.generated;

        //    target.debugRandomId = self.debugRandomId;

        //    //非Def部分

        //    target.category = self.category;

        //    target.startDiscovered = self.startDiscovered;

        //    target.linkedThings = self.linkedThings;

        //    var fieldInfo1 = typeof(RimWorld.EntityCodexEntryDef).GetField("linkedIncidents", BindingFlags.Instance | BindingFlags.NonPublic);
        //    fieldInfo1.SetValue(target, self.linkedIncidents);

        //    target.discoveredResearchProjects = self.discoveredResearchProjects;

        //    target.provocationIncidents = self.provocationIncidents;

        //    target.discoveryType = self.discoveryType;

        //    target.allowDiscoveryWhileMapGenerating = self.allowDiscoveryWhileMapGenerating;

        //    target.orderInCategory = self.orderInCategory;

        //    target.hideInPlaystyles = self.hideInPlaystyles;

        //    var fieldInfo2 = typeof(RimWorld.EntityCodexEntryDef).GetField("useDescriptionFrom", BindingFlags.Instance | BindingFlags.NonPublic);
        //    fieldInfo2.SetValue(target, self.useDescriptionFrom);

        //    var fieldInfo3 = typeof(RimWorld.EntityCodexEntryDef).GetField("uiIconPath", BindingFlags.Instance | BindingFlags.NonPublic);
        //    fieldInfo3.SetValue(target, self.uiIconPath);

        //    var fieldInfo4 = typeof(RimWorld.EntityCodexEntryDef).GetField("useDescriptionFrom", BindingFlags.Instance | BindingFlags.NonPublic);
        //    fieldInfo4.SetValue(target, self.useDescriptionFrom);

        //    target.description = self.description;

        //    target.icon = self.icon;

        //    target.silhouette = self.silhouette;

        //    //target.Description = self.Description;

        //    //target.Discovered => self.Discovered;

        //    //target.Visible => self.Visible;

        //    return target;
        //}

    }
}
