/*

   Copyright 2025 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       https://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Editing.Templates;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippetsEditing
{
  public class ProSnippetsEditingTemplates : MapTool
  {

    #region ProSnippet Group: Edit Templates
    #endregion

    public static void Templates()
    {
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDLAYERS
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      #region Find edit template by name on a layer
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        //get the templates
        var map = ArcGIS.Desktop.Mapping.MapView.Active.Map;
        if (map == null)
          return;

        var mainTemplate = map.FindLayers("main").FirstOrDefault()?.GetTemplate("Distribution");
        var mhTemplate = map.FindLayers("Manhole").FirstOrDefault()?.GetTemplate("Active");
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
      #region Find table templates belonging to a standalone table
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        var map = ArcGIS.Desktop.Mapping.MapView.Active.Map;
        if (map == null)
          return;
        //Get a particular table template
        var tableTemplate = map.FindStandaloneTables("Address Points").FirstOrDefault()?.GetTemplate("Residences");
        //Get all the templates of a standalone table
        var ownersTableTemplates = map.FindStandaloneTables("Owners").FirstOrDefault()?.GetTemplates();
        var statisticsTableTemplates = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().First(l => l.Name.Equals("Trading Statistics")).GetTemplates();
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
      #region Current template
      EditingTemplate template = EditingTemplate.Current;
      #endregion


      QueuedTask.Run(() =>
      {
        // Get the active map view
        var mapView = MapView.Active;
        if (mapView == null)
          return;

        // Get the first line layer in the map - alternatively get a specific layer
        var featureLayer = mapView.Map.Layers.OfType<FeatureLayer>().FirstOrDefault(l => l.ShapeType == esriGeometryType.esriGeometryLine || l.ShapeType == esriGeometryType.esriGeometryPolyline);
        if (featureLayer == null)
          return;

        // Get the templates for the layer
        var templates = featureLayer.GetTemplates();
        if (templates.Count == 0)
          return;

        // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateToolAsync(System.String)
        #region Trigger template editing tool
        // DAML ID of the tool to activate - for example "esri_editing_SketchTwoPointLineTool" Pro Tool or a custom tool
        string _editToolname = "esri_editing_SketchTwoPointLineTool";

        // Get the first template - alternatively get a specific template
        var template = templates.First();

        // Confirm the tool is available in the template
        if (template.ToolIDs.FirstOrDefault(_editToolname) == null)
          return;

        // Activate the tool
        template.ActivateToolAsync(_editToolname);
        #endregion
      });

    }

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.DefaultToolID
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.AutoGenerateFeatureTemplates
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.GetDefinition
    // cref: ArcGIS.Core.CIM.CIMEditingTemplate
    // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
    // cref: ArcGIS.Core.CIM.CIMRowTemplate
    // cref: ArcGIS.Core.CIM.CIMEditingTemplate.DefaultToolGUID
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.GetDefinition()
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.SetDefinition(ArcGIS.Core.CIM.CIMEditingTemplate)
    #region Change Default Edit tool for a template
    public Task ChangeTemplateDefaultToolAsync(ArcGIS.Desktop.Mapping.FeatureLayer flayer,
                      string toolContentGUID, string templateName)
    {
      return ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {

        // retrieve the edit template form the layer by name
        var template = flayer?.GetTemplate(templateName) as ArcGIS.Desktop.Editing.Templates.EditingTemplate;
        // get the definition of the layer
        var layerDef = flayer?.GetDefinition() as ArcGIS.Core.CIM.CIMFeatureLayer;
        if ((template == null) || (layerDef == null))
          return;

        if (template.DefaultToolID != this.ID)
        {
          bool updateLayerDef = false;
          if (layerDef.AutoGenerateFeatureTemplates)
          {
            layerDef.AutoGenerateFeatureTemplates = false;
            updateLayerDef = true;
          }

          // retrieve the CIM edit template definition
          var templateDef = template.GetDefinition();

          // assign the GUID from the tool DAML definition, for example
          // <tool id="TestConstructionTool_SampleSDKTool" categoryRefID="esri_editing_construction_polyline" ….>
          //   <tooltip heading="">Tooltip text<disabledText /></tooltip>
          //   <content guid="e58239b3-9c69-49e5-ad4d-bb2ba29ff3ea" />
          // </tool>
          // then the toolContentGUID would be "e58239b3-9c69-49e5-ad4d-bb2ba29ff3ea"

          //At 2.x -
          //templateDef.ToolProgID = toolContentGUID;
          templateDef.DefaultToolGUID = toolContentGUID;

          // set the definition back to 
          template.SetDefinition(templateDef);

          // update the layer definition too
          if (updateLayerDef)
            flayer.SetDefinition(layerDef);
        }
      });
    }

    #endregion

    protected void FilterTemplateTools()
    {
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate
      // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
      // cref: ArcGIS.Core.CIM.CIMRowTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.GetDefinition()
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateDefaultToolAsync
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ToolIDs
      // cref: ArcGIS.Core.CIM.CIMExtensions.GetExcludedToolIDs(ArcGIS.Core.CIM.CIMEditingTemplate)
      // cref: ArcGIS.Core.CIM.CIMExtensions.SetExcludedToolIDs(ArcGIS.Core.CIM.CIMEditingTemplate,System.String[])
      // cref: ArcGIS.Core.CIM.CIMExtensions.AllowToolID(ArcGIS.Core.CIM.CIMEditingTemplate,System.String)
      // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.FeatureTemplates
      // cref: ARCGIS.DESKTOP.MAPPING.LAYER.GETDEFINITION
      // cref: ARCGIS.DESKTOP.MAPPING.LAYER.SETDEFINITION
      #region Hide or show editing tools on templates
      QueuedTask.Run(() =>
      {
        //hide all tools except line tool on layer
        var featLayer = MapView.Active.Map.FindLayers("Roads").First();

        var editTemplates = featLayer.GetTemplates();
        var newCIMEditingTemplates = new List<CIMEditingTemplate>();

        foreach (var et in editTemplates)
        {
          //initialize template by activating default tool
          et.ActivateDefaultToolAsync();
          var cimEditTemplate = et.GetDefinition();
          //get the visible tools on this template
          var allTools = et.ToolIDs.ToList();
          //add the hidden tools on this template
          allTools.AddRange(cimEditTemplate.GetExcludedToolIDs().ToList());
          //hide all the tools then allow the line tool

          //At 2.x -
          //allTools.AddRange(cimEditTemplate.GetExcludedToolDamlIds().ToList());
          allTools.AddRange(cimEditTemplate.GetExcludedToolIDs().ToList());

          //At 2.x - 
          //cimEditTemplate.SetExcludedToolDamlIds(allTools.ToArray());
          //cimEditTemplate.AllowToolDamlID("esri_editing_SketchLineTool");

          cimEditTemplate.SetExcludedToolIDs(allTools.ToArray());
          cimEditTemplate.AllowToolID("esri_editing_SketchLineTool");
          newCIMEditingTemplates.Add(cimEditTemplate);
        }
        //update the layer templates
        var layerDef = featLayer.GetDefinition() as CIMFeatureLayer;
        // Set AutoGenerateFeatureTemplates to false for template changes to stick
        layerDef.AutoGenerateFeatureTemplates = false;
        layerDef.FeatureTemplates = newCIMEditingTemplates.ToArray();
        featLayer.SetDefinition(layerDef);
      });
      #endregion
    }

    public void CreateTemplate()
    {
      string value1 = "";
      string value2 = "";
      string value3 = "";

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      #region Create New Template using layer.CreateTemplate

      var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      if (layer == null)
        return;
      QueuedTask.Run(() =>
      {
        var insp = new Inspector();
        insp.LoadSchema(layer);

        insp["Field1"] = value1;
        insp["Field2"] = value2;
        insp["Field3"] = value3;

        var tags = new[] { "Polygon", "tag1", "tag2" };

        // set defaultTool using a daml-id 
        string defaultTool = "esri_editing_SketchCirclePolygonTool";

        // tool filter is the tools to filter OUT
        var toolFilter = new[] { "esri_editing_SketchTracePolygonTool" };

        // create a new template  
        var newTemplate = layer.CreateTemplate("My new template", "description", insp, defaultTool, tags, toolFilter);
      });
      #endregion

      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.GETDEFINITION
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Description
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Name
      // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
      // cref: ArcGIS.Core.CIM.CIMRowTemplate
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Core.CIM.CIMEditingTemplate)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      #region Create New Table Template using table.CreateTemplate
      var table = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().FirstOrDefault();
      if (table == null)
        return;
      QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");

        var definition = tableTemplate.GetDefinition();
        definition.Description = "New definition";
        definition.Name = "New name";
        //Create new table template using this definition
        table.CreateTemplate(definition);

        //You can also create a new table template using this extension method. You can use this method the same way you use the layer.CreateTemplate method.
        table.CreateTemplate("New template name", "Template description", tags: new string[] { "tag 1", "tag 2" });
      });
      #endregion

      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.GETDEFINITION
      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.SETDEFINITION(ArcGIS.Core.CIM.CIMEditingTemplate)
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Description
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Name
      // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
      // cref: ArcGIS.Core.CIM.CIMRowTemplate
      #region Update a Table Template
      QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");

        var definition = tableTemplate.GetDefinition();
        definition.Description = "New definition";
        definition.Name = "New name";
        // update the definition
        tableTemplate.SetDefinition(definition);
      });
      #endregion

      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition.GetSymbolCollection
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition.GetLabelClassCollection
      // cref: ARCGIS.DESKTOP.MAPPING.FEATURELAYER.GETFEATURECLASS
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
      #region Create Annotation Template

      // get an anno layer
      AnnotationLayer annoLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<AnnotationLayer>().FirstOrDefault();
      if (annoLayer == null)
        return;

      QueuedTask.Run(() =>
      {
        Inspector insp = null;
        // get the anno feature class
        var fc = annoLayer.GetFeatureClass() as ArcGIS.Core.Data.Mapping.AnnotationFeatureClass;

        // get the featureclass CIM definition which contains the labels, symbols
        var cimDefinition = fc.GetDefinition() as ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition;
        var labels = cimDefinition.GetLabelClassCollection();
        var symbols = cimDefinition.GetSymbolCollection();

        // make sure there are labels, symbols
        if ((labels.Count == 0) || (symbols.Count == 0))
          return;

        // find the label class required
        //   typically you would use a subtype name or some other characteristic
        // in this case lets just use the first one

        var label = labels[0];

        // each label has a textSymbol
        // the symbolName *should* be the symbolID to be used
        var symbolName = label.TextSymbol.SymbolName;
        int symbolID = -1;
        if (!int.TryParse(symbolName, out symbolID))
        {
          // int.TryParse fails - attempt to find the symbolName in the symbol collection
          foreach (var symbol in symbols)
          {
            if (symbol.Name == symbolName)
            {
              symbolID = symbol.ID;
              break;
            }
          }
        }
        // no symbol?
        if (symbolID == -1)
          return;

        // load the schema
        insp = new Inspector();
        insp.LoadSchema(annoLayer);

        // ok to assign these fields using the inspector[fieldName] methodology
        //   these fields are guaranteed to exist in the annotation schema
        insp["AnnotationClassID"] = label.ID;
        insp["SymbolID"] = symbolID;

        // set up some additional annotation properties
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        annoProperties.FontSize = 36;
        annoProperties.TextString = "My Annotation feature";
        annoProperties.VerticalAlignment = VerticalAlignment.Top;
        annoProperties.HorizontalAlignment = HorizontalAlignment.Justify;

        insp.SetAnnotationProperties(annoProperties);

        var tags = new[] { "Annotation", "tag1", "tag2" };

        // use daml-id rather than guid
        string defaultTool = "esri_editing_SketchStraightAnnoTool";

        // tool filter is the tools to filter OUT
        var toolFilter = new[] { "esri_editing_SketchCurvedAnnoTool" };

        // create a new template 
        var newTemplate = annoLayer.CreateTemplate("new anno template", "description", insp, defaultTool, tags, toolFilter);
      });

      #endregion

    }

    public void RemoveTemplate()
    {
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Desktop.Editing.Templates.EditingTemplate)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
      #region Remove a table template
      var table = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().FirstOrDefault();
      if (table == null)
        return;
      QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");
        //Removing a table template
        table.RemoveTemplate(tableTemplate);
        //Removing a template by name
        table.RemoveTemplate("Template2");
      });
      #endregion
    }

    public void TemplateChanged()
    {
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs.IncomingTemplate
      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.ACTIVATETOOLASYNC
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs},System.Boolean)
      #region Active Template Changed

      ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent.Subscribe(OnActiveTemplateChanged);

      async void OnActiveTemplateChanged(ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs args)
      {
        // return if incoming template is null
        if (args.IncomingTemplate == null)
          return;

        // Activate two-point line tool for Freeway template in the Layers map
        if (args.IncomingTemplate.Name == "Freeway" && args.IncomingMapView.Map.Name == "Layers")
          await args.IncomingTemplate.ActivateToolAsync("esri_editing_SketchTwoPointLineTool");
      }
      #endregion
    }

  }
}
