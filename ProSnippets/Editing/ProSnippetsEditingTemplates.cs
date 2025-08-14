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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public class ProSnippetsEditingTemplates : MapTool
  {

    #region ProSnippet Group: Edit Templates
    #endregion

    // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDLAYERS
    // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
    #region Find edit template by name on a layer
    /// <summary>
    /// Finds and retrieves specific editing templates by name from layers in the active map.
    /// </summary>
    /// <remarks>This method demonstrates how to locate and retrieve editing templates associated with
    /// specific layers in the active map. It uses the names of the layers and templates to perform the lookup. The
    /// method runs asynchronously on the QueuedTask thread to ensure thread safety when interacting with the
    /// map.</remarks>
    public static void FindTemplateByName()
    {
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        //get the templates
        var map = ArcGIS.Desktop.Mapping.MapView.Active.Map;
        if (map == null)
          return;

        var mainTemplate = map.FindLayers("main").FirstOrDefault()?.GetTemplate("Distribution");
        var mhTemplate = map.FindLayers("Manhole").FirstOrDefault()?.GetTemplate("Active");
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
    // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES(System.String)
    // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
    #region Find table templates belonging to a standalone table
    /// <summary>
    /// Demonstrates how to retrieve editing templates associated with standalone tables in the active map.
    /// </summary>
    /// <remarks>This method shows examples of how to find specific templates or retrieve all templates for
    /// standalone tables in the active map. It uses the ArcGIS Pro SDK to interact with map layers and their associated
    /// templates.  The method runs asynchronously on the ArcGIS Pro QueuedTask to ensure thread safety when accessing
    /// map data.</remarks>
    public static void FindTableTemplates()
    {
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
    #region Current template
    /// <summary>
    /// Retrieves the currently active editing template.
    /// </summary>
    /// <remarks>The returned template can be used to access or modify properties of the active editing
    /// template. Ensure that a template is active before invoking this method to avoid a <see langword="null"/>
    /// result.</remarks>
    /// <returns>The <see cref="EditingTemplate"/> object that represents the current editing template,  or <see
    /// langword="null"/> if no template is currently active.</returns>
    public static EditingTemplate GetCurrentTemplate()
    {
      EditingTemplate template = EditingTemplate.Current;
      return template;
    }
    #endregion


    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateToolAsync(System.String)
    #region Trigger template editing tool
    /// <summary>
    /// Activates a specified editing tool for the given feature layer's template in the provided map view.
    /// </summary>
    /// <remarks>This method activates the first available template for the specified feature layer and
    /// attempts to activate a predefined editing tool associated with that template. If no templates are available or
    /// the specified tool is not found in the template, the method will exit without performing any action.  The tool
    /// to be activated is identified by its DAML ID, which must be available in the template's tool
    /// collection.</remarks>
    /// <param name="mapView">The <see cref="MapView"/> where the editing tool will be activated.</param>
    /// <param name="featureLayer">The <see cref="FeatureLayer"/> whose template will be used to activate the tool.</param>
    public static void ActivateTemplateTool(MapView mapView, FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
        {
          // Get the templates for the layer
          var templates = featureLayer.GetTemplates();
          if (templates.Count == 0)
            return;

          // DAML ID of the tool to activate - for example "esri_editing_SketchTwoPointLineTool" Pro Tool or a custom tool
          string _editToolname = "esri_editing_SketchTwoPointLineTool";

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
    /// <summary>
    /// Changes the default editing tool for a specified template in the given feature layer.
    /// </summary>
    /// <remarks>This method updates the default tool for the specified template in the feature layer. If the
    /// feature layer is configured to auto-generate feature templates, this setting will be disabled to allow the
    /// manual update. <para> The <paramref name="toolContentGUID"/> should match the <c>content</c> GUID defined in the
    /// tool's DAML configuration. For example: <code> <tool id="TestConstructionTool_SampleSDKTool"
    /// categoryRefID="esri_editing_construction_polyline">   <content guid="e58239b3-9c69-49e5-ad4d-bb2ba29ff3ea" />
    /// </tool> </code> </para> <para> If the specified template or feature layer definition cannot be retrieved, the
    /// operation will not proceed. </para></remarks>
    /// <param name="flayer">The feature layer containing the template whose default tool is to be changed. Cannot be <see langword="null"/>.</param>
    /// <param name="toolContentGUID">The GUID of the tool to set as the default for the template. This corresponds to the <c>content</c> GUID in the
    /// tool's DAML definition.</param>
    /// <param name="templateName">The name of the template within the feature layer for which the default tool is to be updated. Cannot be <see
    /// langword="null"/> or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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
    public void FilterTemplateTools(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
      {
        //hide all tools except line tool on layer
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
    #region Create New Template using layer.CreateTemplate
    /// <summary>
    /// Creates a new editing template for the specified feature layer with the provided field values.
    /// </summary>
    /// <remarks>This method creates a new editing template for the specified <see cref="BasicFeatureLayer"/>
    /// using  the provided field values. The template is configured with default tags, a default tool, and a tool 
    /// filter to exclude specific tools. The method runs asynchronously on the QueuedTask thread. <para> The field
    /// values in <paramref name="newFieldValues"/> must match the schema of the feature layer.  Ensure that the array
    /// contains the correct number of values and that the values are in the expected  order. </para></remarks>
    /// <param name="layer">The feature layer for which the new template will be created. Cannot be null.</param>
    /// <param name="newFieldValues">An array of field values to initialize the template's attributes. The array must contain values  corresponding
    /// to the fields expected by the layer's schema.</param>
    public static void CreateNewTemplate(BasicFeatureLayer layer, object[] newFieldValues)
    {
      QueuedTask.Run(() =>
      {
        var insp = new Inspector();
        insp.LoadSchema(layer);

        insp["Field1"] = newFieldValues[0];
        insp["Field2"] = newFieldValues[1];
        insp["Field3"] = newFieldValues[2];

        var tags = new[] { "Polygon", "tag1", "tag2" };

        // set defaultTool using a daml-id 
        string defaultTool = "esri_editing_SketchCirclePolygonTool";

        // tool filter is the tools to filter OUT
        var toolFilter = new[] { "esri_editing_SketchTracePolygonTool" };

        // create a new template  
        var newTemplate = layer.CreateTemplate("My new template", "description", insp, defaultTool, tags, toolFilter);
      });
    }
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
    /// <summary>
    /// Creates new table templates for the specified standalone table.
    /// </summary>
    /// <remarks>This method demonstrates two approaches to creating table templates: <list type="bullet">
    /// <item> Using an existing template's definition, modifying its properties (e.g., name and description), and
    /// creating a new template based on the modified definition. </item> <item> Using an extension method to create a
    /// new template directly by specifying its name, description, and optional tags. </item> Both approaches require
    /// the operation to be executed within a queued task to ensure thread safety in the ArcGIS Pro environment.
    /// </remarks></remarks>
    /// <param name="table">The standalone table for which the templates will be created.</param>
    public static void CreateNewTableTemplate(StandaloneTable table)
    {
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
    }
    #endregion

    // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.GETDEFINITION
    // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.SETDEFINITION(ArcGIS.Core.CIM.CIMEditingTemplate)
    // cref: ArcGIS.Core.CIM.CIMEditingTemplate
    // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Description
    // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Name
    // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
    // cref: ArcGIS.Core.CIM.CIMRowTemplate
    #region Update a Table Template
    /// <summary>
    /// Updates the definition of a specified table template by modifying its name and description.
    /// </summary>
    /// <remarks>This method retrieves a table template named "Template1" from the provided <paramref
    /// name="table"/>.  If the template exists, its definition is updated with a new name and description.  The
    /// changes are applied asynchronously using a queued task to ensure thread safety.</remarks>
    /// <param name="table">The <see cref="StandaloneTable"/> containing the template to be updated. Cannot be <see langword="null"/>.</param>
    public static void UpdateTableTemplate(StandaloneTable table)
    {
      // get a table template
      var tableTemplate = table.GetTemplate("Template1");
      if (tableTemplate == null)
        return;
      // get the definition
      var definition = tableTemplate.GetDefinition();
      definition.Description = "New definition";
      definition.Name = "New name";
      // update the definition
      tableTemplate.SetDefinition(definition);

      QueuedTask.Run(() =>
        {
          var tableTemplate = table.GetTemplate("Template1");

          var definition = tableTemplate.GetDefinition();
          definition.Description = "New definition";
          definition.Name = "New name";
          // update the definition
          tableTemplate.SetDefinition(definition);
        });
    }
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
    /// <summary>
    /// Creates a new annotation template for the specified annotation layer.
    /// </summary>
    /// <remarks>This method initializes an annotation template using the first label class and associated
    /// symbol from the annotation feature class definition. It sets up default annotation properties, such as font
    /// size, text string, and alignment, and assigns them to the template. The template is created with a default tool,
    /// tags, and a tool filter to exclude specific tools. <para> Ensure that the annotation layer contains at least one
    /// label class and one symbol in its definition. If no label classes or symbols are available, the method will exit
    /// without creating a template. </para> <para> This method runs asynchronously on the QueuedTask scheduler.
    /// </para></remarks>
    /// <param name="annoLayer">The <see cref="AnnotationLayer"/> for which the annotation template will be created.</param>
    public void CreateAnnotationTemplate(AnnotationLayer annoLayer)
    {
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Desktop.Editing.Templates.EditingTemplate)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
    #region Remove a table template
    /// <summary>
    /// Removes predefined editing templates associated with the specified standalone table.
    /// </summary>
    /// <remarks>This method removes editing templates from the provided <see cref="StandaloneTable"/>. 
    /// Templates can be removed either by directly referencing the template object or by specifying the template
    /// name.</remarks>
    /// <param name="table">The standalone table from which the templates will be removed.</param>
    public static void RemoveTemplate(StandaloneTable table)
    {
      QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");
        //Removing a table template
        table.RemoveTemplate(tableTemplate);
        //Removing a template by name
        table.RemoveTemplate("Template2");
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs
    // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs.IncomingTemplate
    // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.ACTIVATETOOLASYNC
    // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs},System.Boolean)
    #region Active Template Changed

    /// <summary>
    /// Subscribes to the active template changed event and handles changes to the editing template.
    /// </summary>
    /// <remarks>This method subscribes to the <see
    /// cref="ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent"/> to monitor changes in the active editing
    /// template. When the active template changes, the method evaluates the incoming template and performs specific
    /// actions based on its properties. For example, if the template is named "Freeway" and belongs to the "Layers"
    /// map, the two-point line tool is activated for the template.</remarks>
    public static void TemplateChanged()
    {

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
    }

    #endregion
  }
}
