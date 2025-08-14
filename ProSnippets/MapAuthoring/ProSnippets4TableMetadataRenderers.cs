using ArcGIS.Core.CIM;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippets3TableMetadataRenderers
  {
    #region ProSnippet Group: Renderers
    #endregion
    // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition
    // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition.#ctor(System.Collections.Generic.List<System.String>, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.CIMColorRamp, ArcGIS.Core.CIM.CIMSymbolReference, System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    #region Set unique value renderer to the selected feature layer of the active map
    /// <summary>
    /// Sets a unique value renderer for the specified feature layer.
    /// </summary>
    /// <param name="featureLayer">The feature layer to which the unique value renderer will be applied.  This layer must be part of the active map
    /// and cannot be null.</param>
    /// <returns></returns>
    public static async Task SetUniqueValueRendererAsync(FeatureLayer featureLayer)
    {
      await QueuedTask.Run(() =>
      {
        //field to be used to retrieve unique values
        var fields = new List<string> { "Type" };
        //constructing a point symbol as a template symbol
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(
            ColorFactory.Instance.GreenRGB, 16.0, SimpleMarkerStyle.Pushpin);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //constructing renderer definition for unique value renderer
        UniqueValueRendererDefinition uniqueValueRendererDef =
      new UniqueValueRendererDefinition(fields, symbolPointTemplate);

        //creating a unique value renderer
        CIMUniqueValueRenderer uniqueValueRenderer = featureLayer.CreateRenderer(uniqueValueRendererDef) as CIMUniqueValueRenderer;

        //setting the renderer to the feature layer
        featureLayer.SetRenderer(uniqueValueRenderer);
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMUniqueValue
    // cref: ArcGIS.Core.CIM.CIMUniqueValue.FieldValues
    // cref: ArcGIS.Core.CIM.CIMUniqueValueClass
    // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Editable
    // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Label
    // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Patch
    // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Symbol
    // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Visible
    // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Values
    // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup
    // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup.Classes
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.UseDefaultSymbol
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultLabel
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultSymbol
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Groups
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Fields
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    #region Create a UniqueValueRenderer to specify symbols to values 
    /// <summary>
    /// Creates and applies a unique value renderer to the specified feature layer.
    /// </summary>
    /// <param name="featureLayer">The feature layer to which the unique value renderer will be applied. This parameter cannot be null.</param>
    /// <returns>A task representing the asynchronous operation of creating and applying the renderer.</returns>
    public static Task UniqueValueRenderer(FeatureLayer featureLayer)
    {
      return QueuedTask.Run(() =>
      {
        //The goal is to construct the CIMUniqueValueRenderer which will be applied to the feature layer.
        // To do this, the following are the objects we need to set the renderer up with the fields and symbols.
        // As a reference, this is the USCities dataset. Snippet will create a unique value renderer that applies 
        // specific symbols to all the cities in California and Alabama.  The rest of the cities will use a default symbol.

        // First create a "CIMUniqueValueClass" for the cities in Alabama.
        List<CIMUniqueValue> listUniqueValuesAlabama = new List<CIMUniqueValue> { new CIMUniqueValue { FieldValues = new string[] { "Alabama" } } };
        CIMUniqueValueClass alabamaUniqueValueClass = new CIMUniqueValueClass
        {
          Editable = true,
          Label = "Alabama",
          Patch = PatchShape.Default,
          Symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB).MakeSymbolReference(),
          Visible = true,
          Values = listUniqueValuesAlabama.ToArray()
        };
        // Create a "CIMUniqueValueClass" for the cities in California.
        List<CIMUniqueValue> listUniqueValuescalifornia = new List<CIMUniqueValue> { new CIMUniqueValue { FieldValues = new string[] { "California" } } };
        CIMUniqueValueClass californiaUniqueValueClass = new CIMUniqueValueClass
        {
          Editable = true,
          Label = "California",
          Patch = PatchShape.Default,
          Symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlueRGB).MakeSymbolReference(),
          Visible = true,
          Values = listUniqueValuescalifornia.ToArray()
        };
        //Create a list of the above two CIMUniqueValueClasses
        List<CIMUniqueValueClass> listUniqueValueClasses = new List<CIMUniqueValueClass>
          {
                        alabamaUniqueValueClass, californiaUniqueValueClass
          };
        //Create a list of CIMUniqueValueGroup
        CIMUniqueValueGroup uvg = new CIMUniqueValueGroup
        {
          Classes = listUniqueValueClasses.ToArray(),
        };
        List<CIMUniqueValueGroup> listUniqueValueGroups = new List<CIMUniqueValueGroup> { uvg };
        //Create the CIMUniqueValueRenderer
        CIMUniqueValueRenderer uvr = new CIMUniqueValueRenderer
        {
          UseDefaultSymbol = true,
          DefaultLabel = "all other values",
          DefaultSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreyRGB).MakeSymbolReference(),
          Groups = listUniqueValueGroups.ToArray(),
          Fields = new string[] { "STATE_NAME" }
        };
        //Set the feature layer's renderer.
        featureLayer.SetRenderer(uvr);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition
    // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.Radius
    // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.WeightField
    // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.ColorRamp
    // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.RendereringQuality
    // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.UpperLabel
    // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.LowerLabel
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
    // cref: ArcGIS.Core.CIM.CIMHeatMapRenderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    #region Create a Heatmap Renderer
    /// <summary>
    /// Creates and applies a heatmap renderer to the specified feature layer.
    /// </summary>
    /// <param name="featureLayer">The feature layer to which the heatmap renderer will be applied.  This layer must contain a valid field for
    /// weighting the heatmap values.</param>
    public static async void CreateHeatMapRenderer(FeatureLayer featureLayer)
    {
      string colorBrewerSchemesName = "ArcGIS Colors";
      //Get the style project item that contains the color ramps
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      //Search for the color ramp in the style
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        //defining a heatmap renderer that uses values from Population field as the weights
        HeatMapRendererDefinition heatMapDef = new HeatMapRendererDefinition()
        {
          Radius = 20,
          WeightField = "Population",
          ColorRamp = colorRamp.ColorRamp,
          RendereringQuality = 8,
          UpperLabel = "High Density",
          LowerLabel = "Low Density"
        };

        CIMHeatMapRenderer heatMapRndr = featureLayer.CreateRenderer(heatMapDef) as CIMHeatMapRenderer;
        featureLayer.SetRenderer(heatMapRndr);
      });

    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition
    // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition.#ctor(System.String, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.CIMColorRamp, System.String, System.String, System.Double, System.Double)
    // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition.ShowNullValues
    // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition.NullValueLabel
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
    // cref: ArcGIS.Core.CIM.CIMClassBreaksRenderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    #region Create an unclassed Renderer
    /// <summary>
    /// Creates and applies an unclassed colors renderer to the specified feature layer.
    /// </summary>
    /// <param name="featureLayer">The feature layer to which the unclassed colors renderer will be applied. This parameter cannot be null.</param>
    public static async void CreateUnclassedRenderer(FeatureLayer featureLayer)
    {
      string colorBrewerSchemesName = "ArcGIS Colors";
      //Get the style project item that contains the color ramps
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      //Search for the color ramp in the style
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 16.0, SimpleMarkerStyle.Diamond);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //defining an unclassed renderer with custom upper and lower stops
        //all features with value >= 5,000,000 will be drawn with the upper color from the color ramp
        //all features with value <= 50,000 will be drawn with the lower color from the color ramp
        UnclassedColorsRendererDefinition unclassRndrDef = new UnclassedColorsRendererDefinition
                              ("Population", symbolPointTemplate, colorRamp.ColorRamp, "Highest", "Lowest", 5000000, 50000)
        {

          //drawing features with null values with a different symbol
          ShowNullValues = true,
          NullValueLabel = "Unknown"
        };
        // Create a point symbol for null values
        CIMPointSymbol nullSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 16.0, SimpleMarkerStyle.Circle);
        unclassRndrDef.NullValueSymbol = nullSym.MakeSymbolReference();
        //Create the unclassed renderer using the definition
        CIMClassBreaksRenderer cbRndr = featureLayer.CreateRenderer(unclassRndrDef) as CIMClassBreaksRenderer;
        //Set the renderer to the feature layer
        featureLayer.SetRenderer(cbRndr);
      });

    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition
    // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.#ctor(System.String, ArcGIS.Core.CIM.CIMSymbolReference, System.Double, System.Double, System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.UpperSizeStop
    // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.LowerSizeStop
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
    // cref: ArcGIS.Core.CIM.CIMProportionalRenderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    #region Create a Proportion Renderer with max and min symbol size capped
    public static async void CreateProportionalRenderer(FeatureLayer featureLayer)
    {

      string colorBrewerSchemesName = "ArcGIS Colors";
      //Get the style project item that contains the color ramps
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      //Search for the color ramp in the style
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        //Creating a point symbol to be used as a template symbol for the proportional renderer
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //minimum symbol size is capped to 4 point while the maximum symbol size is set to 50 point
        //Creating a proportional renderer definition that uses the "Population" field
        ProportionalRendererDefinition prDef = new ProportionalRendererDefinition("POPULATION", symbolPointTemplate, 4, 50, true)
        {
          //setting upper and lower size stops to stop symbols growing or shrinking beyond those thresholds
          UpperSizeStop = 5000000,  //features with values >= 5,000,000 will be drawn with maximum symbol size
          LowerSizeStop = 50000    //features with values <= 50,000 will be drawn with minimum symbol size
        };
        // Create a proportional renderer using the definition
        CIMProportionalRenderer propRndr = featureLayer.CreateRenderer(prDef) as CIMProportionalRenderer;
        // Set the renderer to the feature layer
        featureLayer.SetRenderer(propRndr);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition
    // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.#ctor(System.String, ArcGIS.Core.CIM.esriUnits, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.SymbolShapes, ArcGIS.Core.CIM.ValueRepresentations)
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
    // cref: ArcGIS.Core.CIM.CIMProportionalRenderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    #region Create a True Proportion Renderer
    /// <summary>
    /// Creates and applies a proportional renderer to the specified feature layer.
    /// </summary>
    /// <param name="featureLayer">The feature layer to which the proportional renderer will be applied.  This parameter cannot be null.</param>
    public static async void CreateTrueProportionalRenderer(FeatureLayer featureLayer)
    {
      string colorBrewerSchemesName = "ArcGIS Colors";
      //Get the style project item that contains the color ramps
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      //Search for the color ramp in the style
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        //Creating a point symbol to be used as a template symbol for the proportional renderer
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //Defining proportional renderer where size of symbol will be same as its value in field used in the renderer.
        ProportionalRendererDefinition prDef = new ProportionalRendererDefinition("POPULATION", esriUnits.esriMeters, symbolPointTemplate, SymbolShapes.Square, ValueRepresentations.Radius);
        //Create a Proportional renderer using the definition
        CIMProportionalRenderer propRndr = featureLayer.CreateRenderer(prDef) as CIMProportionalRenderer;
        // Set the renderer to the feature layer
        featureLayer.SetRenderer(propRndr);

      });
    }
    #endregion
    #region ProSnippet Group: Attribute Table - ITablePane
    #endregion

    // cref: ArcGIS.Desktop.Mapping.ITablePane
    // cref: ArcGIS.Desktop.Mapping.ITablePane.ZoomLevel
    // cref: ArcGIS.Desktop.Mapping.ITablePane.SetZoomLevel
    #region Set zoom level for Attribute Table
    /// <summary>
    /// Adjusts the zoom level of the currently active attribute table pane.
    /// </summary>
    public static void SetTablePaneZoom()
    {
      //Check if the active pane is an ITablePane
      if (FrameworkApplication.Panes.ActivePane is ITablePane tablePane)
      {
        //Get the current zoom level of the table pane
        var currentZoomLevel = tablePane.ZoomLevel;
        // Set a new zoom level, for example, increase it by 50
        var newZoomLevel = currentZoomLevel + 50;
        // Set the new zoom level to the table pane
        tablePane.SetZoomLevel(newZoomLevel);
      }
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.ITablePane
    // cref: ArcGIS.Desktop.Mapping.ITablePane.MapMember
    // cref: ArcGIS.Desktop.Mapping.ITablePane.ActiveObjectID
    // cref: ArcGIS.Desktop.Mapping.ITablePane.ActiveColumn
    #region Retrieve the values of selected cell in the attribute table
    /// <summary>
    /// Retrieves the contents of the currently active cell in the attribute table.
    /// </summary>
    public static void ActiveCellContents()
    {
      {
        if (FrameworkApplication.Panes.ActivePane is ITablePane tablePane)
        {
          var mapMember = tablePane.MapMember;
          //Get the active row's object ID from the table pane
          var oid = tablePane.ActiveObjectID;
          if (oid.HasValue && oid.Value != -1 && mapMember != null)
          {
            //Get the field of the active column
            var activeField = tablePane.ActiveColumn;
            QueuedTask.Run(() =>
            {
              // TODO: Use core objects to retrieve record and get value
            });
          }
        }
      }
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.ITablePane
    // cref: ArcGIS.Desktop.Mapping.ITablePane.BringIntoView
    #region Move to a particular row
    /// <summary>
    /// Scrolls the active attribute table pane to a specific row.
    /// </summary>
    public static void MoveToRow()
      {
      // Check if the active pane is an ITablePane  
      if (FrameworkApplication.Panes.ActivePane is ITablePane tablePane)
        {
          // move to first row
          tablePane.BringIntoView(0);

          // move to sixth row
          tablePane.BringIntoView(5);
        }
      }
    #endregion

    #region ProSnippet Group: Working with Standalone Tables
    #endregion

    public void StandaloneTables1()
    {
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableFactory.CreateStandaloneTable(System.Uri, ArcGIS.Desktop.Mapping.IStandaloneTableContainerEdit, System.Int32, System.String)
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableCreationParams
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableCreationParams.#ctor(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableCreationParams.DefinitionQuery
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableFactory.CreateStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTableCreationParams, ArcGIS.Desktop.Mapping.IStandaloneTableContainerEdit)
      #region Create a StandaloneTable

      //container can be a map or group layer
      var container = MapView.Active.Map;
      //var container =  MapView.Active.Map.GetLayersAsFlattenedList()
      //                                  .OfType<GroupLayer>().First();
      QueuedTask.Run(() =>
      {
        //use a local path
        var table = StandaloneTableFactory.Instance.CreateStandaloneTable(
      new Uri(@"C:\Temp\Data\SDK.gdb\EarthquakeDamage", UriKind.Absolute),
      container);
        //use a URI to a feature service table endpoint
        var table2 = StandaloneTableFactory.Instance.CreateStandaloneTable(
    new Uri(@"https://bexdog.esri.com/server/rest/services/FeatureServer" + "/2", UriKind.Absolute),
    container);
        //Use an item
        var item = ItemFactory.Instance.Create(@"C:\Temp\Data\SDK.gdb\ParcelOwners");
        var tableCreationParams = new StandaloneTableCreationParams(item);
        var table3 = StandaloneTableFactory.Instance.CreateStandaloneTable(tableCreationParams, container);

        //use table creation params
        var table_params = new StandaloneTableCreationParams(item)
        {
          // At 2.x - DefinitionFilter = new CIMDefinitionFilter()
          //{
          //  //optional - use a filter
          //  DefinitionExpression = "LAND_USE = 3"
          //}
          DefinitionQuery = new DefinitionQuery(whereClause: "LAND_USE = 3", name: "Landuse")
        };
        var table4 = StandaloneTableFactory.Instance.CreateStandaloneTable(table_params,
                                 container);

      });

      #endregion

    }

    public void StandaloneTables2()
    {
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.Map.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.StandaloneTables
      // cref: ArcGIS.Desktop.Core.FrameworkExtender.OpenTablePane(ArcGIS.Desktop.Framework.PaneCollection,ArcGIS.Desktop.Mapping.MapMember, ArcGIS.Desktop.Mapping.TableViewMode)
      #region Retrieve a table from its container
      var container = MapView.Active.Map;

      //the map standalone table collection
      var table = container.GetStandaloneTablesAsFlattenedList()
                              .FirstOrDefault(tbl => tbl.Name == "EarthquakeDamage");

      //or from a group layer
      var grp_layer = MapView.Active.Map.FindLayers("GroupLayer1").First() as GroupLayer;
      var table2 = grp_layer.FindStandaloneTables("EarthquakeDamage").First();
      //or         grp_layer.GetStandaloneTablesAsFlattenedList().First()
      //or         grp_layer.StandaloneTables.Where(...).First(), etc.

      //show the table in a table view 
      //use FrameworkApplication.Current.Dispatcher.BeginInvoke if not on the UI thread
      FrameworkApplication.Panes.OpenTablePane(table2);

      #endregion
    }

    public void StandaloneTables3()
    {
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.MoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable, System.Int32)
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.Map.MoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable, ArcGIS.Desktop.Mapping.CompositeLayerWithTables, System.Int32)
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.MoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable, System.Int32)
      #region Move a Standalone table

      //get the first group layer that has at least one table
      var grp_layer = MapView.Active.Map.GetLayersAsFlattenedList()
        .OfType<GroupLayer>().First(g => g.StandaloneTables.Count > 0);
      var map = MapView.Active.Map;//assumes non-null
      QueuedTask.Run(() =>
      {
        //move the first table to the bottom of the container
        grp_layer.MoveStandaloneTable(grp_layer.StandaloneTables.First(), -1);

        //move the last table in the map standalone tables to a group
        //layer and place it at position 3. If 3 is invalid, the table
        //will be placed at the bottom of the target container
        //assumes the map has at least one standalone table...
        var table = map.StandaloneTables.Last();
        map.MoveStandaloneTable(table, grp_layer, 3);

        //move a table from a group layer to the map standalone tables
        //collection - assumes a table called 'Earthquakes' exists
        var table2 = grp_layer.FindStandaloneTables("Earthquakes").First();
        //move to the map container
        map.MoveStandaloneTable(table2, 0);//will be placed at the top
      });

      #endregion

    }

    public void StandaloneTables4()
    {
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable)
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveStandaloneTables(System.IEnumerable<ArcGIS.Desktop.Mapping.StandaloneTable>)
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.RemoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable)
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.RemoveStandaloneTables(System.IEnumerable<ArcGIS.Desktop.Mapping.StandaloneTable>)
      #region Remove a Standalone table

      //get the first group layer that has at least one table
      var grp_layer = MapView.Active.Map.GetLayersAsFlattenedList()
        .OfType<GroupLayer>().First(g => g.StandaloneTables.Count > 0);
      var map = MapView.Active.Map;//assumes non-null

      QueuedTask.Run(() =>
      {
        //get the tables from the map container
        var tables = map.GetStandaloneTablesAsFlattenedList();
        //delete the first...
        if (tables.Count() > 0)
        {
          map.RemoveStandaloneTable(tables.First());
          //or delete all of them
          map.RemoveStandaloneTables(tables);
        }

        //delete a table from a group layer
        //assumes it has at least one table...
        grp_layer.RemoveStandaloneTable(grp_layer.StandaloneTables.First());
      });

      #endregion
    }

    #region ProSnippet Group: Metadata
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetMetadata()
    // cref: ArcGIS.Desktop.Mapping.Map.GetCanEditMetadata()
    // cref: ArcGIS.Desktop.Mapping.Map.SetMetadata(System.String)
    #region Get and Set Map Metadata
    /// <summary>
    /// Retrieves the metadata of the specified map, allows modifications, and updates the map with the modified
    /// metadata.
    /// </summary>
    /// <param name="map">The map whose metadata is being retrieved and updated. Cannot be <see langword="null"/>.</param>
    public static void MapLayerMetadata(Map map)
    {
      QueuedTask.Run(() => {
        //Get map's metadata
        var mapMetadata = map.GetMetadata();
        //TODO:Make edits to metadata using the retrieved mapMetadata string.

        //Set the modified metadata back to the map.
        if (map.GetCanEditMetadata())
          map.SetMetadata(mapMetadata);
      });
    }
    #endregion

      // cref: ArcGIS.Desktop.Mapping.MapMember.GetUseSourceMetadata()
      // cref: ArcGIS.Desktop.Mapping.MapMember.SetUseSourceMetadata(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.MapMember.SupportsMetadata
      // cref: ArcGIS.Desktop.Mapping.MapMember.GetMetadata()
      // cref: ArcGIS.Desktop.Mapping.MapMember.GetCanEditMetadata()
      // cref: ArcGIS.Desktop.Mapping.MapMember.SetMetadata(System.String)
      #region Layer Metadata
    /// <summary>
    /// Configures and manages metadata for the layers or tables in the specified map.
    /// </summary>
    /// <param name="map">The map containing the layers or tables whose metadata will be managed. Cannot be null.</param>
    public static void LayerMetadata(Map map)
    {
      //Search for only layers/tables here if needed.
      MapMember mapMember = map.GetLayersAsFlattenedList().FirstOrDefault(); 
      if (mapMember == null) return;
      QueuedTask.Run(() => {
        //Gets whether or not the MapMember stores its own metadata or uses metadata retrieved
        //from its source. This method must be called on the MCT. Use QueuedTask.Run
        bool doesUseSourceMetadata = mapMember.GetUseSourceMetadata();

        //Sets whether or not the MapMember will use its own metadata or the metadata from
        //its underlying source (if it has one). This method must be called on the MCT.
        //Use QueuedTask.Run
        mapMember.SetUseSourceMetadata(true);

        //Does the MapMember supports metadata
        var supportsMetadata = mapMember.SupportsMetadata;

        //Get MapMember metadata
        var metadatstring = mapMember.GetMetadata();
        //TODO:Make edits to metadata using the retrieved mapMetadata string.

        //Set the modified metadata back to the mapmember (layer, table..)
        if (mapMember.GetCanEditMetadata())
          mapMember.SetMetadata(metadatstring);
      });
    }
    #endregion
    
  }
}
