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
using System.Threading.Tasks;

namespace ProSnippets.EditingSnippets
{
  public static partial class ProSnippetsEditing
  {
    #region ProSnippet Group: Edit Templates
    #endregion

    public static async Task EditingTemplatesSnippets()
    {
      #region ignore - Variable initialization

      var activeMap = MapView.Active.Map;
      var featureLayer = activeMap.Layers.OfType<FeatureLayer>().FirstOrDefault();
      var annotationLayer = activeMap.GetLayersAsFlattenedList().OfType<AnnotationLayer>().FirstOrDefault();

      #endregion

      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDLAYERS
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      #region Find edit template by name on a layer
      // Finds and retrieves specific editing templates by name from layers in the active map.
      await QueuedTask.Run(() =>
      {
        var mainTemplate = activeMap.FindLayers("main").FirstOrDefault()?.GetTemplate("Distribution");
        var mhTemplate = activeMap.FindLayers("Manhole").FirstOrDefault()?.GetTemplate("Active");
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
      #region Find table templates belonging to a standalone table
      // retrieve editing templates associated with standalone tables in the active map.
      await QueuedTask.Run(() =>
      {
        //Get a particular table template
        var tableTemplate = activeMap.FindStandaloneTables("Address Points").FirstOrDefault()?.GetTemplate("Residences");
        //Get all the templates of a standalone table
        var ownersTableTemplates = activeMap.FindStandaloneTables("Owners").FirstOrDefault()?.GetTemplates();
        var statisticsTableTemplates = activeMap.GetStandaloneTablesAsFlattenedList().First(l => l.Name.Equals("Trading Statistics")).GetTemplates();
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
      #region Current template
      // Retrieves the currently active editing template.
      EditingTemplate template = EditingTemplate.Current;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateDefaultToolAsync()
      #region Activate template and its default editing tool
      await QueuedTask.Run(() =>
      {
        // Get all templates for the layer
        var templates = featureLayer.GetTemplates();
        if (templates.Count == 0)
          return;
        // Get the first template - alternatively get a specific template
        var template = templates.First();

        // Activate the default tool
        template.ActivateDefaultToolAsync();
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateToolAsync(System.String)
      #region Activate template and a specific editing tool
      {
        await QueuedTask.Run(() =>
        {
          // DAML ID of the tool to activate - for example "esri_editing_SketchTwoPointLineTool" Pro Tool or a custom tool
          string _editToolname = "esri_editing_SketchTwoPointLineTool";

          // Get all templates for the layer
          var templates = featureLayer.GetTemplates();
          if (templates.Count == 0)
            return;
          // Get the first template - alternatively get a specific template
          var template = templates.First();

          // Confirm the tool is available in the template
          if (template.ToolIDs.FirstOrDefault(_editToolname) == null)
            return;

          // Activate the tool
          template.ActivateToolAsync(_editToolname);
        });
      }

      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateLastSelectedToolAsync()
      #region Activate template and it's last selected tool
      {
        await QueuedTask.Run(() =>
        {
          // Get all templates for the layer
          var templates = featureLayer.GetTemplates();
          if (templates.Count == 0)
            return;
          // Get the first template - alternatively get a specific template
          var template = templates.First();

          // Activate the last selected/used tool for the template
          template.ActivateLastSelectedToolAsync();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateAsync()
      #region Activate template without changing current tool
      {
        await QueuedTask.Run(() =>
        {
          // Get all templates for the layer
          var templates = featureLayer.GetTemplates();
          if (templates.Count == 0)
            return;
          // Get the first template - alternatively get a specific template
          var template = templates.First();

          // Activate the template without changing the current tool
          template.ActivateAsync();
        });
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
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
      await QueuedTask.Run(() =>
            {
              var templateName = "Distribution"; // name of the template to update
              var toolDamlPlugInID = "esri_editing_SketchTwoPointLineTool"; // DAML ID of the tool to set as default
              var toolContentGUID = "e2096d13-b437-4bc1-94ea-4494c3260f72"; // Example GUID, replace with actual GUID from DAML
                                                                            // retrieve the edit template form the layer by name
              var template = featureLayer?.GetTemplate(templateName) as EditingTemplate;
              // get the definition of the layer
              var layerDef = featureLayer?.GetDefinition() as CIMFeatureLayer;
              if (template == null || layerDef == null)
                return;
              if (template.DefaultToolID != toolDamlPlugInID)
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

                //templateDef.ToolProgID = toolContentGUID;
                templateDef.DefaultToolGUID = toolContentGUID;

                // set the definition back to 
                template.SetDefinition(templateDef);

                // update the layer definition too
                if (updateLayerDef)
                  featureLayer.SetDefinition(layerDef);
              }
            });

      #endregion

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
      await QueuedTask.Run(() =>
         {
           //hide all tools except line tool on a given feature layer
           var editTemplates = featureLayer.GetTemplates();
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

             allTools.AddRange(cimEditTemplate.GetExcludedToolIDs().ToList());

             cimEditTemplate.SetExcludedToolIDs(allTools.ToArray());
             cimEditTemplate.AllowToolID("esri_editing_SketchLineTool");
             newCIMEditingTemplates.Add(cimEditTemplate);
           }
           //update the layer templates
           var layerDef = featureLayer.GetDefinition() as CIMFeatureLayer;
           // Set AutoGenerateFeatureTemplates to false for template changes to stick
           layerDef.AutoGenerateFeatureTemplates = false;
           layerDef.FeatureTemplates = newCIMEditingTemplates.ToArray();
           featureLayer.SetDefinition(layerDef);
         });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      #region Create New Template using layer.CreateTemplate
      await QueuedTask.Run(() =>
        {
          var newFieldValues = new object[] { "Value1", 123, DateTime.Now };
          // Creates a new editing template for the specified feature layer with the provided field values.
          var insp = new Inspector();
          insp.LoadSchema(featureLayer);

          insp["Field1"] = newFieldValues[0];
          insp["Field2"] = newFieldValues[1];
          insp["Field3"] = newFieldValues[2];

          var tags = new[] { "Polygon", "tag1", "tag2" };

          // set defaultTool using a daml-id 
          string defaultTool = "esri_editing_SketchCirclePolygonTool";

          // tool filter is the tools to filter OUT
          var toolFilter = new[] { "esri_editing_SketchTracePolygonTool" };

          // create a new template  
          var newTemplate = featureLayer.CreateTemplate("My new template", "description", insp, defaultTool, tags, toolFilter);
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
      // Creates new table templates for the specified standalone table.
      await QueuedTask.Run(() =>
      {
        var table = activeMap.GetStandaloneTablesAsFlattenedList().FirstOrDefault();
        var tableTemplate = table.GetTemplate("Template1");

        var definition = tableTemplate.GetDefinition();
        definition.Description = "New definition";
        definition.Name = "New name";
        //Create new table template using this definition
        table.CreateTemplate(definition);

        //You can also create a new table template using this extension method. You can use this method the same way you use the layer.CreateTemplate method.
        table.CreateTemplate("New template name", "Template description", tags: ["tag 1", "tag 2"]);
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
      // Updates the definition of a specified table template by modifying its name and description.
      var table = activeMap.GetStandaloneTablesAsFlattenedList().FirstOrDefault();
      await QueuedTask.Run(() =>
        {
          // get a table template
          var tableTemplate = table.GetTemplate("Template1") ?? throw new Exception("No table template named 'Template1' found");
          // get the definition
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
      // Creates a new annotation template for the specified annotation layer.
      await QueuedTask.Run(() =>
        {
          Inspector insp = null;
          // get the anno feature class
          var fc = annotationLayer.GetFeatureClass() as ArcGIS.Core.Data.Mapping.AnnotationFeatureClass;

          // get the feature class CIM definition which contains the labels, symbols
          var cimDefinition = fc.GetDefinition() as ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition;
          var labels = cimDefinition.GetLabelClassCollection();
          var symbols = cimDefinition.GetSymbolCollection();

          // make sure there are labels, symbols
          if (labels.Count == 0 || symbols.Count == 0)
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
          insp.LoadSchema(annotationLayer);

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
          var newTemplate = annotationLayer.CreateTemplate("new annotation template", "description", insp, defaultTool, tags, toolFilter);
        });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Desktop.Editing.Templates.EditingTemplate)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
      #region Remove a table template
      // Removes predefined editing templates associated with the specified standalone table.
      await QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");
        //Removing a table template
        table.RemoveTemplate(tableTemplate);
        //Removing a template by name
        table.RemoveTemplate("Template2");
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs.IncomingTemplate
      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.ACTIVATETOOLASYNC
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs},System.Boolean)
      #region Active Template Changed
      // Subscribes to the active template changed event and handles changes to the editing template.
      ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent.Subscribe(async activeTemplateChangedEventArgs
        =>
      {
        // return if incoming template is null
        if (activeTemplateChangedEventArgs.IncomingTemplate == null)
          return;

        // Activate two-point line tool for Freeway template in the Layers map
        if (activeTemplateChangedEventArgs.IncomingTemplate.Name == "Freeway" && activeTemplateChangedEventArgs.IncomingMapView.Map.Name == "Layers")
          await activeTemplateChangedEventArgs.IncomingTemplate.ActivateToolAsync("esri_editing_SketchTwoPointLineTool");
      });
      #endregion    
    }
  }
}
