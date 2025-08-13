﻿using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippetsLayers
  {
    #region ProSnippet Group: Create Layer
    #endregion
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer(System.Uri, ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32, System.String)
    #region Create and add a layer to the active map
    /// <summary>
    /// Create and add a layer to the active map.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static async Task AddLayerAsync(Map map)
    {
      //* string url = @"c:\data\project.gdb\DEM";  //Raster dataset from a FileGeodatabase
      //* string url = @"c:\connections\mySDEConnection.sde\roads";  //FeatureClass of a SDE
      //* string url = @"c:\connections\mySDEConnection.sde\States\roads";  //FeatureClass within a FeatureDataset from a SDE
      //* string url = @"c:\data\roads.shp";  //Shapefile
      //* string url = @"c:\data\imagery.tif";  //Image from a folder
      //* string url = @"c:\data\mySDEConnection.sde\roads";  //.lyrx or .lpkx file
      //* string url = @"c:\data\CAD\Charlottesville\N1W1.dwg\Polyline";  //FeatureClass in a CAD dwg file
      //* string url = @"C:\data\CAD\UrbanHouse.rvt\Architectural\Windows"; //Features in a Revit file
      //* string url = @"http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Census_USA/MapServer";  //map service
      //* string url = @"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0";  //FeatureLayer off a map service or feature service

      string url = @"c:\data\project.gdb\roads";  //FeatureClass of a FileGeodatabase

      Uri uri = new Uri(url);
      await QueuedTask.Run(() => LayerFactory.Instance.CreateLayer(uri, MapView.Active.Map));

      #endregion
    }

    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.DefinitionQuery
    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.RendererDefinition
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    #region Create layer with create-params
    /// <summary>
    /// Creates a feature layer in the specified map using predefined creation parameters.
    /// </summary>
    /// <remarks>This method demonstrates how to create a feature layer with specific properties, such as
    /// visibility, scale range,  definition query, and renderer definition. The layer is created using the <see
    /// cref="ArcGIS.Desktop.Mapping.LayerFactory"/>  with a <see
    /// cref="ArcGIS.Desktop.Mapping.FeatureLayerCreationParams"/> object that defines these properties.</remarks>
    /// <param name="map">The map in which the feature layer will be created. Must not be <see langword="null"/>.</param>
    public static void CreateLayerWithCreateParams(Map map)
    {
      QueuedTask.Run(() =>
      {
        var flyrCreatnParam = new FeatureLayerCreationParams(new Uri(@"c:\data\world.gdb\cities"))
        {
          Name = "World Cities",
          IsVisible = false,
          MinimumScale = 1000000,
          MaximumScale = 5000,
          DefinitionQuery = new DefinitionQuery(whereClause: "Population > 100000", name: "More than 100k"),
          RendererDefinition = new SimpleRendererDefinition()
          {
            SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(
            CIMColor.CreateRGBColor(255, 0, 0), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
          }
        };
        var featureLayer = LayerFactory.Instance.CreateLayer<FeatureLayer>(flyrCreatnParam, map);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.LayerDocument
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(System.String)
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.GetCIMLayerDocument()
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMLayerDocument)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    #region Create FeatureLayer and add to Map using LayerCreationParams
   /// <summary>
   /// Creates a feature layer from a layer file (.lyrx) and adds it to the active map.
   /// </summary>
   /// <remarks>This method uses a layer file to create a <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> 
   /// and adds it to the active map. The layer file is read to obtain a  <see
   /// cref="ArcGIS.Core.CIM.CIMLayerDocument"/>, which is then used to initialize  <see
   /// cref="ArcGIS.Desktop.Mapping.LayerCreationParams"/>. The layer is created on a  background thread using <see
   /// cref="ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer{T}"/>.</remarks>
    public static void CreateLayerWithParams()
    {
      QueuedTask.Run(() => {
        //Get the LayerDocument from a lyrx file
        var layerDoc = new LayerDocument(@"E:\Data\SDK\Default2DPointSymbols.lyrx");
        //Get the CIMLayerDocument from the LayerDocument and use it to create LayerCreationParams
        var createParams = new LayerCreationParams(layerDoc.GetCIMLayerDocument());
        //Create a FeatureLayer using the LayerCreationParams
        LayerFactory.Instance.CreateLayer<FeatureLayer>(createParams, MapView.Active.Map);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    #region Create FeatureLayer and set to not display in Map.

    /// <summary>
    /// Creates a feature layer with specific visibility options and adds it to the active map.
    /// </summary>
    public static void CreateLayerWithOptions()
    {
      //The catalog path of the feature layer to add to the map
      var featureClassUriVisibility = new Uri(@"C:\Data\Admin\AdminData.gdb\USA\cities");
      //Define the Feature Layer's parameters.
      var layerParamsVisibility = new FeatureLayerCreationParams(featureClassUriVisibility)
      {
        //Set visibility
        IsVisible = false,
      };
      //Create the layer with the feature layer parameters and add it to the active map
      var createdFC = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParamsVisibility, MapView.Active.Map);
    }
    #endregion
    //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
    //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
    //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.RendererDefinition
    //cref: ArcGIS.Desktop.Mapping.SimpleRendererDefinition
    //cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    //cref: ArcGIS.Desktop.Mapping.LayerFactory
    /// <summary>
    /// Creates a feature layer with a simple renderer and adds it to the active map.
    /// </summary>
    #region Create FeatureLayer with a Renderer
    private static void CreateFeatureLayerWithRenderer()
    {
      QueuedTask.Run(() =>
      {
        //Define a simple renderer to draw the Point US Cities feature class.
        var simpleRender = new SimpleRendererDefinition
        {
          SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 4.0, SimpleMarkerStyle.Circle).MakeSymbolReference()

        };
        //The catalog path of the feature layer to add to the map
        var featureClassUri = new Uri(@"C:\Data\Admin\AdminData.gdb\USA\cities");
        //Define the Feature Layer's parameters.
        var layerParams = new FeatureLayerCreationParams(featureClassUri)
        {
          //Set visibility
          IsVisible = true,
          //Set Renderer
          RendererDefinition = simpleRender,
        };
        //Create the layer with the feature layer parameters and add it to the active map
        var createdFCWithRenderer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
      });
    }

    #endregion
    //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
    //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
    //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.DefinitionQuery
    //cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)

    #region Create FeatureLayer with a Query Definition
    /// <summary>
    /// Creates a feature layer with a definition query and adds it to the active map.
    /// </summary>
    public static void CreateFeatureLayerWithQueryDefinition()
    {
      QueuedTask.Run(() =>
      {
        //The catalog path of the feature layer to add to the map
        var featureClassUriDefinition = new Uri(@"C:\Data\Admin\AdminData.gdb\USA\cities");
        //Define the Feature Layer's parameters.
        var layerParamsQueryDefn = new FeatureLayerCreationParams(featureClassUriDefinition)
        {
          IsVisible = true,
          DefinitionQuery = new DefinitionQuery(whereClause: "STATE_NAME = 'California'", name: "CACities")
        };

        //Create the layer with the feature layer parameters and add it to the active map
        var createdFCWithQueryDefn = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParamsQueryDefn, MapView.Active.Map);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(System.Collections.Generic.IEnumerable{System.Uri},ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32)
    #region Create multiple layers
    /// <summary>
    /// Creates multiple layers in the active map using a list of URIs for various datasets.
    /// </summary>
    public static void CreateMultipleLayers()
    {
      QueuedTask.Run(() =>
      {
        //Define a list of dataset URIs for the layers to be created  
        var uriShp = new Uri(@"c:\data\roads.shp");
        var uriSde = new Uri(@"c:\MyDataConnections\MySDE.sde\Census");
        var uri = new Uri(@"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0");
        // Create a list of URIs to be used for creating multiple layers
        var uris = new List<Uri>() { uriShp, uriSde, uri };
        // Create multiple layers using the LayerFactory
        var layers = LayerFactory.Instance.CreateLayers(uris, MapView.Active.Map);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.#ctor(System.Collections.Generic.IEnumerable{System.Uri})
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(ArcGIS.Desktop.Mapping.BulkLayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    #region Create multiple layers with BulkLayerCreationParams
    /// <summary>
    /// Creates multiple layers in the active map using BulkLayerCreationParams.
    /// </summary>
    /// <remarks>
    /// BulkLayerCreationParams allows you to specify additional properties such as the index and visibility of the layers.
    /// </remarks>
    public static void CreateMultipleLayersWithBulkLayerCreationParams()
    {
      QueuedTask.Run(() => {
        //Uris to the datasets for the layers to be created
        var uriShp = new Uri(@"c:\data\roads.shp");
        var uriSde = new Uri(@"c:\MyDataConnections\MySDE.sde\Census");
        var uri = new Uri(@"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0");
        // Create a list of URIs to be used for creating multiple layers
        var uris = new List<Uri>() { uriShp, uriSde, uri }; ;

        // set the index and visibility
        var blkParams = new BulkLayerCreationParams(uris);
        blkParams.MapMemberPosition = MapMemberPosition.Index;
        blkParams.MapMemberIndex = 2;
        blkParams.IsVisible = false;
        // Create multiple layers using the BulkLayerCreationParams
        var layers = LayerFactory.Instance.CreateLayers(blkParams, MapView.Active.Map);
      });  
   }
    #endregion
    //cref:ArcGIS.Desktop.Mapping.BulkLayerCreationParams
    //cref:ArcGIS.Desktop.Mapping.BulkLayerCreationParams.#ctor(System.Collections.Generic.IEnumerable{System.Uri})
    //cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(ArcGIS.Desktop.Mapping.BulkLayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    //cref: ArcGIS.Desktop.Core.ItemFactory.Create(System.String,ArcGIS.Desktop.Core.ItemFactory.ItemType)

    #region Add a GeoPackage to the Map
    /// <summary>
    /// Adds a GeoPackage to the active map by creating layers from its spatial data and tables.
    /// </summary>
    /// <remarks>
    /// By using the <see cref="ArcGIS.Desktop.Mapping.BulkLayerCreationParams"/>, this method efficiently adds all spatial data layers from the GeoPackage to the map.
    /// </remarks>
    public static void AddAGeoPackageToMap()
    {
      QueuedTask.Run(() => {      
        string pathToGeoPackage = @"C:\Data\Geopackage\flooding.gpkg";
        //Create lists to hold the URIs of the layers and tables in the geopackage
        var layerUris = new List<Uri>();
        var tableUris = new List<Uri>();
        //Create an item from the geopackage
        var item = ItemFactory.Instance.Create(pathToGeoPackage, ItemFactory.ItemType.PathItem);
        var children = item.GetItems();
        //Collect the table and spatial data in the geopackage
        foreach (var child in children)
        {
          var childPath = child.Path;

          if (child.TypeID == "sqlite_table")
            tableUris.Add(new Uri(childPath));
          else
            layerUris.Add(new Uri(childPath));
        }
        //Add the spatial data in the geopackage using the BulkLayerCreationParams
        if (layerUris.Count > 0)
        {
          BulkLayerCreationParams bulklcp = new BulkLayerCreationParams(layerUris);
          LayerFactory.Instance.CreateLayers(bulklcp, MapView.Active.Map);
        }
        // add the tables separately
        foreach (var tableUri in tableUris)
        {
          StandaloneTableFactory.Instance.CreateStandaloneTable(tableUri, MapView.Active.Map);
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.TopologyLayer
    // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.AddAssociatedLayers
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    #region Create TopologyLayer with an Uri pointing to a Topology dataset
    /// <summary>
    /// Creates a Topology layer from a Topology dataset and adds it to the active map.
    /// </summary>
    ///  <remarks>
    ///  TopologyLayerCreationParams is used to specify the properties of the Topology layer, such as its name and whether to add associated layers.
    ///  </remarks>
    public static void CreateTopologyLayerFromTopologyDataset()
    {
      QueuedTask.Run(() =>
      {
        var path = @"D:\Data\CommunitySamplesData\Topology\GrandTeton.gdb\BackCountry\Backcountry_Topology";
        var lcp = new TopologyLayerCreationParams(new Uri(path));
        lcp.Name = "GrandTeton_Backcountry";
        lcp.AddAssociatedLayers = true;
        var topoLayer = LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.TopologyLayer>(lcp, MapView.Active.Map);        
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.TopologyLayer.GetTopology
    // cref: ArcGIS.Desktop.Mapping.TopologyLayer
    // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
    // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.AddAssociatedLayers
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    #region Create Topology Layer using Topology dataset
    /// <summary>
    /// Creates a Topology layer from an existing Topology dataset and adds it to the active map.
    /// </summary>
    public static void CreateTopologyLayerFromExistingTopologyLayer()
    {
      QueuedTask.Run(() =>
      {
        //Get the Topology of another Topology layer
        var existingTopology = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TopologyLayer>().FirstOrDefault();
        if (existingTopology != null)
        {
          var topology = existingTopology.GetTopology();
          //Configure the settings for a new Catalog layer using the CatalogDataset of an existing layer
          var topologyLyrParams = new TopologyLayerCreationParams(topology);
          topologyLyrParams.Name = "NewTopologyLayerFromAnotherTopologyLayer";
          topologyLyrParams.AddAssociatedLayers = true;
          LayerFactory.Instance.CreateLayer<TopologyLayer>(topologyLyrParams, MapView.Active.Map);
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.CatalogLayer
    // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.DefinitionQuery
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    #region Create Catalog Layer using Uri to a Catalog Feature Class
    /// <summary>
    /// Creates a Catalog layer using a URI to a Catalog Feature Class and adds it to the active map.
    /// </summary>
    public static void CreateCatalogLayerUsingURIToCatalogFeatureClass()
    {
      QueuedTask.Run(() =>
      {
        var createParams = new CatalogLayerCreationParams(new Uri(@"C:\CatalogLayer\CatalogDS.gdb\HurricaneCatalogDS"));
        //Set the definition query
        createParams.DefinitionQuery = new DefinitionQuery("Query1", "cd_itemname = 'PuertoRico'");
        //Set name of the new Catalog Layer
        createParams.Name = "PuertoRico";
        //Create Layer
        var catalogLayer = LayerFactory.Instance.CreateLayer<CatalogLayer>(createParams, MapView.Active.Map);        
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.CatalogLayer
    // cref: ArcGIS.Desktop.Mapping.CatalogLayer.GetCatalogDataset
    // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.#ctor(ArcGIS.Core.Data.Mapping.CatalogDataset)
    // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.DefinitionQuery
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    #region Create Catalog Layer using CatalogDataset
    /// <summary>
    /// Creates a Catalog layer using a CatalogDataset from an existing Catalog layer and adds it to the active map.
    /// </summary>
    public static void CreateCatalogLayerUsingCatalogDataset()
    {
      QueuedTask.Run(() =>
      {
        //Get the CatalogDataset of another Catalog layer
        var existingCatalogLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<CatalogLayer>().FirstOrDefault();
        if (existingCatalogLayer != null)
        {
          var catalogDataset = existingCatalogLayer.GetCatalogDataset();
          //Configure the settings for a new Catalog layer using the CatalogDataset of an existing layer
          var catalogLyrParams = new CatalogLayerCreationParams(catalogDataset);
          catalogLyrParams.Name = "NewCatalogLayerFromAnotherCatalogLayer";
          catalogLyrParams.DefinitionQuery = new DefinitionQuery("Query1", "cd_itemname = 'Asia'");
          LayerFactory.Instance.CreateLayer<CatalogLayer>(catalogLyrParams, MapView.Active.Map);
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.LayerTemplatePackages
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Desktop.Core.Item)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    /// <summary>
    ///  Adds Map Notes to the active map using the <see cref="ArcGIS.Desktop.Mapping.Map.LayerTemplatePackages"/> collection.
    /// </summary>
    #region Add MapNotes to the active map
    public static async void MapNotesAPI()
    {      
      //Gets the collection of layer template packages installed with Pro for use with maps
      var items = MapView.Active.Map.LayerTemplatePackages;
      //Iterate through the collection of items to add each Map Note to the active map
      foreach (var item in items)
      {
        //Create a parameter item for the map note
        var layer_params = new LayerCreationParams(item);
        layer_params.IsVisible = false;
        await QueuedTask.Run(() =>
        {
          //Create a feature layer for the map note
          var layer = LayerFactory.Instance.CreateLayer<Layer>(layer_params, MapView.Active.Map);
        });
      }      
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
    // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.RendererDefinition
    // cref: ArcGIS.Desktop.Mapping.SimpleRendererDefinition
    #region Apply Symbology from a Layer in the TOC
    /// <summary>
    /// Applies symbology from an existing layer in the TOC to a new feature layer created from a geodatabase.
    /// </summary>
    /// <remarks
    /// Gets an existing feature layer from the map, retrieves its renderer, and applies that renderer to a new feature layer created from a geodatabase.
    /// </remarks>
    public static void ApplySymbologyFromALayer()
    {
      //Note: Call within QueuedTask.Run()
      if (MapView.Active.Map == null) return;

      //Get an existing Layer. This layer has a symbol you want to use in a new layer.
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>()
            .Where(l => l.ShapeType == esriGeometryType.esriGeometryPoint).FirstOrDefault();
      //This is the renderer to use in the new Layer
      var renderer = lyr.GetRenderer() as CIMSimpleRenderer;
      //Set the DataConnection for the new layer
      Geodatabase geodatabase = new Geodatabase(
        new FileGeodatabaseConnectionPath(new Uri(@"E:\Data\Admin\AdminData.gdb")));
      FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("Cities");
      var dataConnection = featureClass.GetDataConnection();
      //Create the definition for the new feature layer
      var featureLayerParams = new FeatureLayerCreationParams(dataConnection)
      {
        RendererDefinition = new SimpleRendererDefinition(renderer.Symbol),
        IsVisible = true,
      };
      //create the new layer
      LayerFactory.Instance.CreateLayer<FeatureLayer>(
        featureLayerParams, MapView.Active.Map);      
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams.SubtypeLayers
    // cref: ArcGIS.Desktop.Mapping.SubtypeFeatureLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.SubtypeFeatureLayerCreationParams.#ctor(ArcGIS.Desktop.Mapping.RendererDefinition, System.Int32)
    // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams.SubtypeLayers
    // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams.DefinitionQuery
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayer
    #region Create a new SubTypeGroupLayer
    /// <summary>
    /// Creates a new SubTypeGroupLayer with multiple subtypes and adds it to the active map.
    /// </summary>
    public static void CreateSubTypeLayers()
    {

      var subtypeGroupLayerCreateParam = new SubtypeGroupLayerCreationParams
      (
          new Uri(@"c:\data\SubtypeAndDomain.gdb\Fittings")
      );

      // Define Subtype layers
      var rendererDefn1 = new UniqueValueRendererDefinition(new List<string> { "type" });
      var renderDefn2 = new SimpleRendererDefinition()
      {
        SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(
                CIMColor.CreateRGBColor(255, 0, 0), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
      };
      subtypeGroupLayerCreateParam.SubtypeLayers = new List<SubtypeFeatureLayerCreationParams>()
      {
        //define first subtype layer with unique value renderer
        new SubtypeFeatureLayerCreationParams(new UniqueValueRendererDefinition(new List<string> { "type" }), 1),

        //define second subtype layer with simple symbol renderer
        new SubtypeFeatureLayerCreationParams(renderDefn2, 2)
      };

      // Define additional parameters
      subtypeGroupLayerCreateParam.DefinitionQuery = new DefinitionQuery(whereClause: "Enabled = 1", name: "IsActive");
      subtypeGroupLayerCreateParam.IsVisible = true;
      subtypeGroupLayerCreateParam.MinimumScale = 50000;

      SubtypeGroupLayer subtypeGroupLayer2 = LayerFactory.Instance.CreateLayer<SubtypeGroupLayer>(
                    subtypeGroupLayerCreateParam, MapView.Active.Map);
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.LayerDocument
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(System.String)
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.GetCIMLayerDocument
    // cref: ArcGIS.Core.CIM.CIMLayerDocument.LayerDefinitions
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.Save(System.String)
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.AsJson()
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.Load(System.String)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    #region Create layer from a lyrx file
    /// <summary>
    /// Creates a layer from a lyrx file and adds it to the specified map.
    /// </summary>
    /// <param name="map"></param>
    public static void CreateLayerFromALyrxFile(Map map)
    {
      var lyrDocFromLyrxFile = new LayerDocument(@"d:\data\cities.lyrx");
      var cimLyrDoc = lyrDocFromLyrxFile.GetCIMLayerDocument();

      //modifying its renderer symbol to red
      var cimSimpleRenderer = ((CIMFeatureLayer)cimLyrDoc.LayerDefinitions[0]).Renderer as CIMSimpleRenderer;
      cimSimpleRenderer?.Symbol.Symbol.SetColor(new CIMRGBColor() { R = 255 });

      //optionally save the updates out as a file
      lyrDocFromLyrxFile.Save(@"c:\data\cities_red.lyrx");

      //get a json representation of the layer document and you want store away...
      var aJSONString = lyrDocFromLyrxFile.AsJson();

      //... and load it back when needed
      lyrDocFromLyrxFile.Load(aJSONString);
      cimLyrDoc = lyrDocFromLyrxFile.GetCIMLayerDocument();

      //create a layer and add it to a map
      var lcp = new LayerCreationParams(cimLyrDoc);
      var lyr = LayerFactory.Instance.CreateLayer<FeatureLayer>(lcp, map);      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.LayerDocument
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(System.String)
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.GetCIMLayerDocument
    // cref: ArcGIS.Core.CIM.CIMLayerDocument.LayerDefinitions
    // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.Renderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    #region Apply Symbology to a layer from a Layer file
    /// <summary>
    /// Applies symbology from a layer file (.lyrx) to multiple feature layers.
    /// </summary>
    /// <param name="featureLayers"></param>
    /// <param name="layerFile"></param>
    /// <returns></returns>
    /// <remarks
    /// The CIMLayerDocument is obtained from the layer file, and the renderer is extracted from the first layer definition.
    /// SetRenderer is then called on each feature layer to apply the renderer from the layer file.
    /// </remarks>
    public static async Task ModifyLayerSymbologyFromLyrFileAsync(IEnumerable<FeatureLayer> featureLayers, string layerFile)
    {
      await QueuedTask.Run(() =>
      {
        foreach (var featureLayer in featureLayers)
        {
          //Get the Layer Document from the lyrx file
          var lyrDocFromLyrxFile = new LayerDocument(layerFile);
          var cimLyrDoc = lyrDocFromLyrxFile.GetCIMLayerDocument();

          //Get the renderer from the layer file
          var rendererFromLayerFile = ((CIMFeatureLayer)cimLyrDoc.LayerDefinitions[0]).Renderer as CIMUniqueValueRenderer;

          //Apply the renderer to the feature layer
          //Note: If working with a raster layer, use the SetColorizer method.
          featureLayer?.SetRenderer(rendererFromLayerFile);          
        }
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMInternetServerConnection
    // cref: ArcGIS.Core.CIM.CIMWMSServiceConnection
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    #region Add a WMS service
    /// <summary>
    /// Adds a WMS layer to the active map by creating a connection to a WMS service.
    /// </summary>
    /// <returns></returns>
    public static async Task AddWMSLayerAsync()
    {
      {
        // Create a connection to the WMS server
        var serverConnection = new CIMInternetServerConnection { URL = "URL of the WMS service" };
        var connection = new CIMWMSServiceConnection { ServerConnection = serverConnection };

        // Add a new layer to the map
        var layerParams = new LayerCreationParams(connection);
        await QueuedTask.Run(() =>
        {
          var layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
        });
      }
    }
    #endregion

    // cref: ArcGIS.Core.CIM.CIMStandardDataConnection
    // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.WorkspaceConnectionString
    // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.WorkspaceFactory
    // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.Dataset
    // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.DatasetType
    // cref: ArcGIS.Core.CIM.esriDatasetType
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    #region Add a WFS Service
    /// <summary>
    /// Adds a WFS service to the active map by creating a connection to a WFS server.
    /// </summary>
    public static async void AddWFSService()
      {
        CIMStandardDataConnection cIMStandardDataConnection = new CIMStandardDataConnection()
        {
          WorkspaceConnectionString = @"SWAPXY=TRUE;SWAPXYFILTER=FALSE;URL=http://sampleserver6.arcgisonline.com/arcgis/services/SampleWorldCities/MapServer/WFSServer;VERSION=2.0.0",
          WorkspaceFactory = WorkspaceFactory.WFS,
          Dataset = "Continent",
          DatasetType = esriDatasetType.esriDTFeatureClass
        };

        // Add a new layer to the map
        var layerPamsDC = new LayerCreationParams(cIMStandardDataConnection);
        await QueuedTask.Run(() =>
        {
          Layer layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerPamsDC, MapView.Active.Map);
        });      
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMProjectServerConnection.URL
    // cref: ArcGIS.Core.CIM.CIMWMTSServiceConnection.ServerConnection
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
    #region Add a WMTS service
    /// <summary>
    /// Adds a WMTS layer to the active map by creating a connection to a WMTS service.
    /// </summary>
    /// <returns></returns>
    public static async Task AddWMTSLayerAsync()
    {
      // Create a connection to the WMS server
      var serverConnection = new CIMProjectServerConnection
      {
        URL = "URL of the WMTS service.xml",
        ServerType = ServerType.WMTS,
      };
      var service_connection = new CIMWMTSServiceConnection
      {
        ServerConnection = serverConnection,
        LayerName = "AdminBoundaries", // Specify the layer name you want to add
      };

      // Add a new layer to the map
      var layerParams = new LayerCreationParams(service_connection);
      layerParams.MapMemberPosition = MapMemberPosition.AddToBottom;
      var map = MapView.Active.Map;
      await QueuedTask.Run(() =>
      {
        var layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, map);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
    // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem.ServerConnection
    // cref: ArcGIS.Core.CIM.CIMAGSSServiceConnection.ServerConnection
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
    #region Connect to an AGS Service using a .ags File
    /// <summary>
    /// Adds a service layer to the active map by creating a connection to an ArcGIS Server service using a .ags file.
    /// </summary>
    /// <returns></returns>
    public static async Task AddServiceLayerAsync()
    {
      //This workflow would work for 
      var agsFilePath = @"C:\Data\ServerConnectionFiles\AcmeSampleService.ags";

      //ServerConnectionProjectItem supports .ags, .wms, .wmts, .wfs, and .wcs files
      var server_conn_item = ItemFactory.Instance.Create(agsFilePath)
        as ArcGIS.Desktop.Catalog.ServerConnectionProjectItem;

      //Get the server connection - passwords are never returned
      var serverConnection = server_conn_item.ServerConnection as CIMProjectServerConnection;

      //Add to an AGS service connection
      var service_connection = new CIMAGSServiceConnection()
      {
        URL = "URL to the AGS _service_ on the AGS _server_",
        ServerConnection = serverConnection
      };

      // Add a new layer to the map
      var layerParams = new LayerCreationParams(service_connection);
      layerParams.MapMemberPosition = MapMemberPosition.AddToBottom;
      var map = MapView.Active.Map;
      await QueuedTask.Run(() =>
      {
        var layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, map);
      });      
    }
    #endregion
    // cref: ArcGIS.Core.Data.ServiceConnectionProperties.#ctor(System.Uri)
    // cref: ArcGIS.Core.Data.ServiceConnectionProperties.URL
    // cref: ArcGIS.Core.Data.ServiceConnectionProperties.User
    // cref: ArcGIS.Core.Data.ServiceConnectionProperties.Password
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(System.Uri)
    #region Connect to an AGS Service using ServiceConnectionProperties
    /// <summary>
    /// Connects to an ArcGIS Server service using ServiceConnectionProperties and adds a service layer to the active map.
    /// </summary>
    /// <returns></returns>
    public static async Task AddServiceLayerUsingServiceConnectionProperties()
    {
      //Connect to the AGS service. Note: the connection will persist for the
      //duration of the Pro session.
      var serverUrl = "https://sampleserver6.arcgisonline.com/arcgis/rest/services";

      var username = "user1";
      var password = "user1";

      await QueuedTask.Run(() =>
      {
        //at any point before creating a layer, first make a connection to the server
        //if one has not been already established for the current session
        var uri = new Uri(serverUrl);
        var props = new ServiceConnectionProperties(uri)
        {
          User = username
        };
        //It is preferred that you use the Windows Credential Manager to store the password

        //However, it can be set as clear text if you so choose
        props.Password = password; //not recommended

        //Establish a connection to the server at any point in time _before_
        //creating a layer from a service on the server
        var gdb = new Geodatabase(props);
        gdb.Dispose();//you do not need the geodatabase object after you have connected.
      });

      //later in the session, as needed...create a layer from one of the services
      //on the server
      var serviceUrl = "https://sampleserver6.arcgisonline.com/arcgis/rest/services/Wildfire_secure/MapServer";
      var map = MapView.Active.Map;

      await QueuedTask.Run(() =>
      {
        var lc = new LayerCreationParams(new Uri(serviceUrl));
        LayerFactory.Instance.CreateLayer<MapImageLayer>(lc, map);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapMemberPosition
    // cref: ArcGIS.Desktop.Mapping.WMSLayer
    // cref: ArcGIS.Desktop.Mapping.ServiceCompositeSubLayer
    // cref: ArcGIS.Desktop.Mapping.WMSSubLayer
    // cref: ArcGIS.Desktop.Mapping.WMSSubLayer.GetStyleNames
    // cref: ArcGIS.Desktop.Mapping.WMSSubLayer.SetStyleName(System.String)
    #region Adding and changing styles for WMS Service Layer
    /// <summary>
    /// Adds a WMS layer to the active map and demonstrates how to toggle the visibility of a sublayer and apply an existing style to a WMS sublayer.
    /// </summary>
    /// <returns></returns>
    public static async Task StyleWMSLayer()
    {
      var serverConnection = new CIMInternetServerConnection { URL = "https://spritle.esri.com/arcgis/services/sanfrancisco_sld/MapServer/WMSServer" };
      var connection = new CIMWMSServiceConnection { ServerConnection = serverConnection };
      LayerCreationParams parameters = new LayerCreationParams(connection);
      parameters.MapMemberPosition = MapMemberPosition.AddToBottom;
      await QueuedTask.Run(() =>
      {
        var compositeLyr = LayerFactory.Instance.CreateLayer<WMSLayer>(parameters, MapView.Active.Map);
        //wms layer in ArcGIS Pro always has a composite layer inside it
        var wmsLayers = compositeLyr.Layers[0] as ServiceCompositeSubLayer;
        //each wms sublayer belongs in that composite layer
        var highwayLayerWMSSub = wmsLayers.Layers[1] as WMSSubLayer;
        //toggling a sublayer's visibility
        if ((highwayLayerWMSSub != null))
        {
          bool visibility = highwayLayerWMSSub.IsVisible;
          highwayLayerWMSSub.SetVisibility(!visibility);
        }
        //applying an existing style to a wms sub layer
        var pizzaLayerWMSSub = wmsLayers.Layers[0] as WMSSubLayer;
        var currentStyles = pizzaLayerWMSSub.GetStyleNames();
        pizzaLayerWMSSub.SetStyleName(currentStyles[1]);
      });      
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection
    // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.WorkspaceConnectionString
    // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.GeometryType
    // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.OIDFields
    // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.Srid
    // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.SqlQuery
    // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.Dataset
    // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    #region Create a query layer
    /// <summary>
    /// Creates a query layer from a SQL query and adds it to the active map.
    /// </summary>
    /// <returns></returns>
    public static async Task AddQueryLayerAsync()
    {
      await QueuedTask.Run(() =>
      {
        Map map = MapView.Active.Map;
        Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri(@"C:\Connections\mySDE.sde")));
        CIMSqlQueryDataConnection sqldc = new CIMSqlQueryDataConnection()
        {
          WorkspaceConnectionString = geodatabase.GetConnectionString(),
          GeometryType = esriGeometryType.esriGeometryPolygon,
          OIDFields = "OBJECTID",
          Srid = "102008",
          SqlQuery = "select * from MySDE.dbo.STATES",
          Dataset = "States"
        };
        var lcp = new LayerCreationParams(sqldc)
        {
          Name = "States"
        };
        FeatureLayer flyr = LayerFactory.Instance.CreateLayer<FeatureLayer>(lcp, map);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition
    // cref: ArcGIS.Core.CIM.ClassificationMethod
    // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition.#ctor(System.String,ArcGIS.Core.CIM.ClassificationMethod,System.Int32,ArcGIS.Core.CIM.CIMColorRamp,ArcGIS.Core.CIM.CIMSymbolReference)
    #region Create a feature layer with class breaks renderer with defaults
    /// <summary>
    /// Creates a feature layer with a class breaks renderer using default settings and adds it to the active map.
    /// </summary>
    /// <returns></returns>
    public static async Task AddFeatureLayerClassBreaksAsync()
    {
      await QueuedTask.Run(() =>
      {
        var featureLayerCreationParams = new FeatureLayerCreationParams(new Uri(@"c:\data\countydata.gdb\counties"))
        {
          Name = "Population Density (sq mi) Year 2010",
          RendererDefinition = new GraduatedColorsRendererDefinition("POP10_SQMI")
        };
        LayerFactory.Instance.CreateLayer<FeatureLayer>(
          featureLayerCreationParams,
          MapView.Active.Map
        );
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition
    // cref: ArcGIS.Core.CIM.ClassificationMethod
    // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition.#ctor
    // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition.#ctor
    // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition.#ctor
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ClassificationField
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ClassificationMethod
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.BreakCount
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ColorRamp
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.SymbolTemplate
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ExclusionClause
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ExclusionSymbol
    // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ExclusionLabel
    // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
    // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchColorRamps(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    #region Create a feature layer with class breaks renderer
    /// <summary>
    /// Creates a feature layer with a class breaks renderer using specific settings and adds it to the active map.
    /// </summary>
    /// <returns></returns>
    public static async Task AddFeatureLayerClassBreaksExAsync()
    {
      string colorBrewerSchemesName = "ColorBrewer Schemes (RGB)";
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Greens (Continuous)";
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        GraduatedColorsRendererDefinition gcDef = new GraduatedColorsRendererDefinition()
        {
          ClassificationField = "CROP_ACR07",
          ClassificationMethod = ArcGIS.Core.CIM.ClassificationMethod.NaturalBreaks,
          BreakCount = 6,
          ColorRamp = colorRamp.ColorRamp,
          SymbolTemplate = SymbolFactory.Instance.ConstructPolygonSymbol(
                                  ColorFactory.Instance.GreenRGB, SimpleFillStyle.Solid, null).MakeSymbolReference(),
          ExclusionClause = "CROP_ACR07 = -99",
          ExclusionSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(
                                  ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, null).MakeSymbolReference(),
          ExclusionLabel = "No yield",
        };
        var featureLayerCreationParams = new FeatureLayerCreationParams((new Uri(@"c:\Data\CountyData.gdb\Counties")))
        {
          Name = "Crop",
          RendererDefinition = gcDef
        };
        LayerFactory.Instance.CreateLayer<FeatureLayer>(featureLayerCreationParams, MapView.Active.Map);
      });
    }
    #endregion

    #region ProSnippet Group: Basemap Layers
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.SetBasemapLayers(ArcGIS.Desktop.Mapping.Basemap)
    /// <summary>
    /// Updates the specified map's basemap layers to use the predefined "Gray" basemap.
    /// </summary>
    /// <remarks>This method sets the basemap layers of the provided map to the "Gray" basemap, which is a
    /// predefined basemap in ArcGIS. The "Gray" basemap provides a neutral background suitable for emphasizing thematic
    /// data.</remarks>
    /// <param name="aMap">The map instance whose basemap layers will be updated. Cannot be <see langword="null"/>.</param>
    #region Update a map's basemap layer
    public static void BaseMap(Map aMap)
    {
      aMap.SetBasemapLayers(Basemap.Gray);
    }
    #endregion

   // cref: ArcGIS.Desktop.Mapping.Map.SetBasemapLayers(ArcGIS.Desktop.Mapping.Basemap)
    #region Remove basemap layer from a map
    /// <summary>
    /// Removes the basemap layer from the specified map.
    /// </summary>
    /// <remarks>This method sets the basemap of the provided map to <see cref="Basemap.None"/>, effectively
    /// removing any existing basemap layers.</remarks>
    /// <param name="aMap">The map from which the basemap layer will be removed. Cannot be null.</param>
    public static void RemoveBaseMap(Map aMap) 
    { 
      aMap.SetBasemapLayers(Basemap.None);
    }
    #endregion

    #region ProSnippet Group: Working with Layers
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
    #region Get a list of layers filtered by layer type from a map
    /// <summary>
    /// Retrieves all layers of a specified type from the given map.
    /// </summary>
    /// <remarks>This method filters the layers in the map to include only those of the specified type. The
    /// layers are returned as a flattened list.</remarks>
    /// <param name="aMap">The map from which layers will be retrieved. Cannot be null.</param>
    public static void GetLayersOfType(Map aMap)
    {
      List<FeatureLayer> featureLayerList = aMap.GetLayersAsFlattenedList().OfType<FeatureLayer>().ToList();
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.ShapeType
    //cref: ArcGIS.Core.CIM.esriGeometryType
    #region Get a layer of a certain geometry type
    /// <summary>
    /// Retrieves the first feature layer from the active map that matches the specified geometry type.
    /// </summary>
    /// <remarks>This method searches through all layers in the active map and returns the first feature layer
    /// with a geometry type of <see cref="ArcGIS.Core.CIM.esriGeometryType.esriGeometryPoint"/>. If no matching layer
    /// is found, the result will be <see langword="null"/>.</remarks>
    public static void GetLayersOfSpecifiedGeometryType()
    {
      //Get an existing Layer. This layer has a symbol you want to use in a new layer.
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>()
              .Where(l => l.ShapeType == esriGeometryType.esriGeometryPoint).FirstOrDefault();
    }
    #endregion

      

      // cref: ArcGIS.Desktop.Mapping.Map.FindLayer(System.String,System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.Map.FindLayers(System.String,System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
      #region Find a layer
    /// <summary>
    /// Finds layers within the specified map based on their name or URI.
    /// </summary>
    /// <remarks>This method demonstrates how to locate layers in a map using various approaches: - By name:
    /// Retrieves a read-only list of layers matching the specified name. - By URI: Searches for a specific layer using
    /// its unique URI. - Using LINQ: Queries the flattened list of layers to check for the existence of a layer with a
    /// specific name.  Use this method to understand how to interact with layers in a map, whether by name, URI, or
    /// custom queries.</remarks>
    /// <param name="map">The map in which to search for layers. Cannot be null.</param>
    public static void FindLayer(Map map)
    {
      //Finds layers by name and returns a read only list of Layers
      IReadOnlyList<Layer> layers = map.FindLayers("cities", true);

      //Finds a layer using a URI.
      //The Layer URI you pass in helps you search for a specific layer in a map
      var lyrFindLayer = MapView.Active.Map.FindLayer("CIMPATH=map/u_s__states__generalized_.xml");

      //This returns a collection of layers of the "name" specified. You can use any Linq expression to query the collection.  
      var lyrExists = MapView.Active.Map.GetLayersAsFlattenedList()
                         .OfType<FeatureLayer>().Any(f => f.Name == "U.S. States (Generalized)");
    }
    #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.FindStandaloneTable(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      #region Find a standalone table
    /// <summary>
    /// Finds standalone tables within the specified map based on predefined criteria.
    /// </summary>
    /// <remarks>This method demonstrates how to locate standalone tables in a map, whether they are direct
    /// children of the map or nested within group layers. It utilizes various approaches to search for tables,
    /// including by name or path.</remarks>
    /// <param name="map">The map instance in which to search for standalone tables. Cannot be <see langword="null"/>.</param>
    public static void FindStandaloneTable(Map map)
    {
      // these routines find a standalone table whether it is a child of the Map or a GroupLayer
      var tblFind = map.FindStandaloneTable("CIMPATH=map/address_audit.xml");

      IReadOnlyList<StandaloneTable> tables = map.FindStandaloneTables("addresses");

      // this method finds a standalone table as a child of the map only
      var table = map.StandaloneTables.FirstOrDefault(t => t.Name == "Addresses");
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
    #region Find a layer using partial name search
    /// <summary>
    /// Searches for layers in the active map whose names contain the specified partial name.
    /// </summary>
    /// <remarks>This method retrieves all layers in the active map and filters them based on whether their
    /// names  contain the specified partial name. The results are logged to the debug output.</remarks>
    /// <param name="partialName">The partial name to search for. The search is case-insensitive and uses the current culture.</param>
    public static void FindLayersWithPartialName(string partialName)
    {

       Map map = MapView.Active.Map;
      IEnumerable<Layer> matches = map.GetLayersAsFlattenedList().Where(l => l.Name.IndexOf(partialName, StringComparison.CurrentCultureIgnoreCase) >= 0);
      List<Layer> layers = new List<Layer>();
      foreach (Layer l in matches)
        layers.Add(l);
      System.Diagnostics.Debug.WriteLine($"Found {layers.Count} layers with name containing '{partialName}'");
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Layer.IsVisible
    // cref: ArcGIS.Desktop.Mapping.Layer.SetVisibility(System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.IsEditable
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetEditable(System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsSnappable
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetSnappable(System.Boolean)
    #region Change layer visibility, editability, snappability
    /// <summary>
    /// Modifies the specified layer's properties to ensure it is visible, editable, and snappable.
    /// </summary>
    /// <remarks>This method sets the visibility of the provided <see cref="ArcGIS.Desktop.Mapping.Layer"/> to <see langword="true"/>
    /// if it is not already visible. If the layer is a <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/>, it also ensures that the layer is
    /// editable and snappable by setting the respective properties to <see langword="true"/> if they are not already
    /// enabled.</remarks>
    /// <param name="layer">The layer whose properties will be modified. Must be a valid <see cref="ArcGIS.Desktop.Mapping.Layer"/> instance.</param>
    public static void ChangeProperties(Layer layer)
    {
      if (!layer.IsVisible)
        layer.SetVisibility(true);

      if (layer is FeatureLayer featureLayer)
      {
        if (!featureLayer.IsEditable)
          featureLayer.SetEditable(true);

        if (!featureLayer.IsSnappable)
          featureLayer.SetSnappable(true);
      }      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.LayerDocument
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(ArcGIS.Desktop.Mapping.MapMember)
    // cref: ArcGIS.Desktop.Mapping.LayerDocument.Save(System.String)
    #region Create a Lyrx file
/// <summary>
/// Creates a .lyrx file for the specified layer.
/// </summary>
/// <remarks>The .lyrx file is saved to the path <c>c:\Data\MyLayerDocument.lyrx</c>. Ensure that the application 
/// has write permissions to the specified directory.</remarks>
/// <param name="layer">The layer for which the .lyrx file will be created. This parameter cannot be null.</param>
    public static void CreateLyrx(Layer layer)
    {
      LayerDocument layerDocument = new LayerDocument(layer);
      layerDocument.Save(@"c:\Data\MyLayerDocument.lyrx");      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SelectionCount
    #region Count the features selected on a layer
    /// <summary>
    /// Counts the number of features currently selected in the first feature layer of the specified map.
    /// </summary>
    /// <remarks>This method retrieves the first feature layer in the specified map and determines the count
    /// of selected features. If no feature layer is found, the selection count will not be calculated.</remarks>
    /// <param name="map">The map containing the layers to check for selected features.</param>
    public static void CountSelectedFeatures(Map map)
    {
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      var noFeaturesSelected = lyr.SelectionCount;
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.FeatureTable
    // cref: ArcGIS.Core.CIM.CIMFeatureTable
    // cref: ArcGIS.Core.CIM.CIMDisplayTable.DisplayField
    // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.FeatureTable
    #region Access the display field for a layer
    /// <summary>
    /// Retrieves the display field for the first feature layer in the active map.
    /// </summary>
    /// <remarks>The display field is the field used as the label to represent rows in the underlying source
    /// table of the feature layer. This method accesses the display field by obtaining the CIM definition of the
    /// feature layer and its associated feature table.</remarks>
    public static void AccessDisplayField()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM definition from the layer
        var cimFeatureDefinition = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
        // get the view of the source table underlying the layer
        var cimDisplayTable = cimFeatureDefinition.FeatureTable;
        // this field is used as the 'label' to represent the row
        var displayField = cimDisplayTable.DisplayField;
      });      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsLabelVisible
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetLabelVisibility(System.Boolean)
    #region Enable labeling on a layer
    /// <summary>
    /// Toggles the label visibility for the specified feature layer.
    /// </summary>
    /// <remarks>This method switches the label visibility state of the provided feature layer.  If labels are
    /// currently visible, they will be hidden; if labels are hidden, they will be made visible.</remarks>
    /// <param name="featureLayer">The feature layer whose label visibility is to be toggled. Cannot be <see langword="null"/>.</param>
    public static void EnableLabeling(FeatureLayer featureLayer)
    {
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // toggle the label visibility
        featureLayer.SetLabelVisibility(!featureLayer.IsLabelVisible);
      });      
    }
    #endregion
    //cref: ArcGIS.Desktop.Mapping.ElevationTypeDefinition
    //cref: ArcGIS.Desktop.Mapping.Layer.GetElevationTypeDefinition
    //cref: ArcGIS.Desktop.Mapping.Layer.SetElevationTypeDefinition
    //cref: ArcGIS.Desktop.Mapping.Layer.CanSetElevationTypeDefinition(ArcGIS.Desktop.Mapping.ElevationTypeDefinition)
    //cref: ArcGIS.Desktop.Mapping.Layer.SetElevationTypeDefinition(ArcGIS.Desktop.Mapping.ElevationTypeDefinition)
    //cref: ArcGIS.Desktop.Mapping.LayerElevationType
    //cref: ArcGIS.Desktop.Mapping.ElevationTypeDefinition.CartographicOffset
    //cref: ArcGIS.Desktop.Mapping.ElevationTypeDefinition.VerticalExaggeration

    #region Set Elevation Mode for a layer
    /// <summary>
    /// Sets the elevation mode for the first feature layer in the active map.
    /// </summary>
    /// <remarks>This method retrieves the first <see cref="FeatureLayer"/> from the active map and modifies
    /// its elevation mode. The elevation mode is set using an <see cref="ElevationTypeDefinition"/> object, which
    /// specifies properties such as the elevation type, cartographic offset, and vertical exaggeration. The method
    /// ensures that the elevation type definition can be applied to the layer before setting it.  The elevation type
    /// can be one of the following: <list type="bullet"> <item><see cref="LayerElevationType.OnGround"/></item>
    /// <item><see cref="ArcGIS.Desktop.Mapping.LayerElevationType.RelativeToGround"/></item> <item><see
    /// cref="ArcGIS.Desktop.Mapping.LayerElevationType.RelativeToScene"/></item> <item><see
    /// cref="ArcGIS.Desktop.Mapping.LayerElevationType.AtAbsoluteHeight"/></item> </list> Optional properties such as <see
    /// cref="ArcGIS.Desktop.Mapping.ElevationTypeDefinition.CartographicOffset"/> and  <see
    /// cref="ArcGIS.Desktop.Mapping.ElevationTypeDefinition.VerticalExaggeration"/> can also be specified.</remarks>
    public static void SetElevationMode()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      QueuedTask.Run( () => {
        ElevationTypeDefinition elevationTypeDefinition = featureLayer.GetElevationTypeDefinition();
        elevationTypeDefinition.ElevationType = LayerElevationType.OnGround;
        //elevationTypeDefinition.ElevationType = LayerElevationType.RelativeToGround;
        //elevationTypeDefinition.ElevationType = LayerElevationType.RelativeToScene;
        //elevationTypeDefinition.ElevationType = LayerElevationType.AtAbsoluteHeight;
        //..so on.
        //Optional: Specify the cartographic offset
        elevationTypeDefinition.CartographicOffset = 1000;
        //Optional: Specify the VerticalExaggeration
        elevationTypeDefinition.VerticalExaggeration = 2;
        if (featureLayer.CanSetElevationTypeDefinition(elevationTypeDefinition))
          featureLayer.SetElevationTypeDefinition(elevationTypeDefinition);
      });      
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.IsFlattened
    // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition(ArcGIS.Core.CIM.CIMBaseLayer)
    #region Move a layer in the 2D group to the 3D Group in a Local Scene
    /// <summary>
    /// Moves a feature layer from the 2D group to the 3D group in a local scene.
    /// </summary>
    /// <remarks>This method identifies the first feature layer in the active map's flattened layer list and
    /// updates its definition  to move it to the 3D group in a local scene. The operation is performed asynchronously
    /// using a queued task.</remarks>
    public static void MoveLayerTo3D()
    {
      //The layer in the 2D group to move to the 3D Group in a Local Scene
      var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      QueuedTask.Run(() =>
      {
        //Get the layer's definition
        var lyrDefn = layer.GetDefinition() as CIMBasicFeatureLayer;
        //setting this property moves the layer to 3D group in a scene
        lyrDefn.IsFlattened = false;
        //Set the definition back to the layer
        layer.SetDefinition(lyrDefn);
      });      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapMember.GetDataConnection()
    // cref: ArcGIS.Core.CIM.CIMStandardDataConnection
    // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.WorkspaceConnectionString
    // cref: ArcGIS.Desktop.Mapping.MapMember.SetDataConnection(ArcGIS.Core.CIM.CIMDataConnection, System.Boolean)
    #region Reset the URL of a feature service layer 
    /// <summary>
    /// Resets the data connection of a feature service layer to use a new connection string.
    /// </summary>
    /// <remarks>This method updates the workspace connection string of the specified feature service layer.
    /// Ensure that <paramref name="dataConnectionLayer"/> is a valid feature service layer before calling this
    /// method.</remarks>
    /// <param name="dataConnectionLayer">The <see cref="ArcGIS.Desktop.Mapping.Layer"/> whose data connection will be updated. Must be a feature service layer.</param>
    /// <param name="newConnectionString">The new connection string to set for the feature service layer's workspace.</param>
    public static void ResetDataConnectionFeatureService(Layer dataConnectionLayer, string newConnectionString)
    {
      CIMStandardDataConnection dataConnection = dataConnectionLayer.GetDataConnection() as CIMStandardDataConnection;
      dataConnection.WorkspaceConnectionString = newConnectionString;
      dataConnectionLayer.SetDataConnection(dataConnection);
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Layer.FindAndReplaceWorkspacePath(System.String, System.String, System.Boolean)
    // cref: ArcGIS.Core.Data.Datastore.GetConnectionString()
    #region Change the underlying data source of a feature layer - same workspace type
    /// <summary>
    /// Replaces the underlying data source of the first feature layer in the active map with a new workspace path.
    /// </summary>
    /// <remarks>This method retrieves the first feature layer in the active map and updates its data source
    /// to a new workspace path. The replacement is performed asynchronously using a queued task. The new workspace path
    /// must point to a valid  dataset with the same name as the original dataset unless the <paramref name="validate"/>
    /// parameter of  <see cref="ArcGIS.Desktop.Mapping.Layer.FindAndReplaceWorkspacePath"/> is set to <see
    /// langword="false"/>.</remarks>
    public static void ReplaceDataSource()
    {
      //This is the existing layer for which we want to switch the underlying datasource
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      QueuedTask.Run(() =>
      {
        var connectionStringToReplace = lyr.GetFeatureClass().GetDatastore().GetConnectionString();
        string databaseConnectionPath = @"Path to the .sde connection file to replace with";
        //If the new SDE connection did not have a dataset with the same name as in the feature layer,
        //pass false for the validate parameter of the FindAndReplaceWorkspacePath method to achieve this. 
        //If validate is true and the SDE did not have a dataset with the same name, 
        //FindAndReplaceWorkspacePath will return failure
        lyr.FindAndReplaceWorkspacePath(connectionStringToReplace, databaseConnectionPath, true);
      });      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.ChangeVersion(ArcGIS.Core.Data.VersionBase,ArcGIS.Core.Data.VersionBase)
    #region Change Geodatabase Version of layers off a specified version in a map
    /// <summary>
    /// Changes the geodatabase version of all layers in the active map to a different version.
    /// </summary>
    /// <remarks>This method retrieves the current geodatabase version from the first feature layer in the
    /// active map and switches all layers to a different version, if available. The method assumes that at least one
    /// other version exists in the geodatabase. If no alternate version is found, no changes are made.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task ChangeGDBVersion2Async()
    {
      await QueuedTask.Run(() =>
      {
        //Getting the current version name from the first feature layer of the map
        FeatureLayer flyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();  //first feature layer
        Datastore dataStore = flyr.GetFeatureClass().GetDatastore();  //getting datasource
        Geodatabase geodatabase = dataStore as Geodatabase; //casting to Geodatabase
        if (geodatabase == null)
          return;

        VersionManager versionManager = geodatabase.GetVersionManager();
        ArcGIS.Core.Data.Version currentVersion = versionManager.GetCurrentVersion();
        string currentVersionName = currentVersion.GetName();

        //Getting all available versions except the current one
        List<ArcGIS.Core.Data.Version> versions = [];
        foreach (string versionName in versionManager.GetVersionNames())
        {
          if (versionName != currentVersionName)
            break;
          versions.Add(versionManager.GetVersion(versionName));
        }

        //Assuming there is at least one other version we pick the first one from the list
        ArcGIS.Core.Data.Version toVersion = versions.FirstOrDefault();
        if (toVersion != null)
        {
          //Changing version
          MapView.Active.Map.ChangeVersion(currentVersion, toVersion);
        }
      });
    }
    #endregion
    // cref: ArcGIS.Core.Data.QueryFilter
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.Search(ArcGIS.Core.Data.QueryFilter, ArcGIS.Desktop.Mapping.TimeRange, ArcGIS.Desktop.Mapping.RangeExtent, ArcGIS.Core.CIM.CIMFloorFilterSettings)
    #region Querying a feature layer
    /// <summary>
    /// Executes a query on the first selected feature layer in the active map view and retrieves the count of features
    /// that match the specified criteria.
    /// </summary>
    /// <remarks>This method uses a query filter with a predefined where clause to search for features in the
    /// selected feature layer. The count of matching features is calculated and logged to the debug output.</remarks>
    public static async void SearchAndGetFeatureCount()
    {
      var count = await QueuedTask.Run(() =>
      {
        QueryFilter qf = new QueryFilter()
        {
          WhereClause = "Class = 'city'"
        };

        //Getting the first selected feature layer of the map view
        var flyr = (FeatureLayer)MapView.Active.GetSelectedLayers()
                    .OfType<FeatureLayer>().FirstOrDefault();
        using (RowCursor rows = flyr.Search(qf)) //execute
        {
          //Looping through to count
          int i = 0;
          while (rows.MoveNext()) i++;

          return i;
        }
      });
      System.Diagnostics.Debug.WriteLine(String.Format(
         "Total features that matched the search criteria: {0}", count));
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.#ctor
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetFeatureOutline(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.FeatureOutlineType)
    // cref: ArcGIS.Desktop.Mapping.FeatureOutlineType
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.InsertDefinitionQuery(ArcGIS.Desktop.Mapping.DefinitionQuery,System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetActiveDefinitionQuery(System.String)
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.CanSetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.SetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
    #region Querying a feature layer with a spatial filter
    /// <summary>
    /// Applies a spatial filter to a feature layer based on the geometry of selected features in another feature layer.
    /// </summary>
    /// <remarks>This method creates a definition query for the specified <paramref name="layerToQuery"/>
    /// using the spatial geometry of selected features in <paramref name="spatialDefnLayer"/> and an optional SQL where
    /// clause. The spatial filter geometry is derived from the selected features in <paramref
    /// name="spatialDefnLayer"/>. The definition query is then applied to <paramref name="layerToQuery"/> and set as
    /// the active query.
    /// For example, you can create a Definition Query on a National Parks layer(layerToQuery) with a SQL WhereClause set to find parks with more 500,000 annual visitors. To this DefinitionQuery, you can apply a spatial definition derived from the USA States layer - where the selected feature is California.</remarks>
    /// <param name="layerToQuery">The feature layer to which the spatial filter and definition query will be applied. Cannot be null.</param>
    /// <param name="spatialDefnLayer">The feature layer whose selected features will provide the spatial filter geometry. Cannot be null.</param>
    /// <param name="whereClause">An optional SQL where clause to further refine the definition query. Can be empty or null.</param>
    public static async void QueryWithSpatialFilter(FeatureLayer layerToQuery, FeatureLayer spatialDefnLayer, string whereClause)
    {
      if (layerToQuery == null) return;
      if (spatialDefnLayer == null) return;

      await QueuedTask.Run(() =>
      {
        try
        {
          if (MapView.Active == null) return;

          // Set the spatial filter geometry
          //Get the geometry from the selected features in the feature layer
          var spatialClauseGeom = spatialDefnLayer.GetFeatureOutline(MapView.Active, FeatureOutlineType.Selected);
          //Create the definition query
          DefinitionQuery definitionQuery = new DefinitionQuery
          {
            WhereClause = whereClause,
            Name = $"{layerToQuery.Name}"
          };
          //Setting the spatial filter to the Definition Query
          if (definitionQuery.CanSetFilterGeometry(spatialClauseGeom))
          {
            definitionQuery.SetFilterGeometry(spatialClauseGeom);
          }

          layerToQuery.InsertDefinitionQuery(definitionQuery);
          layerToQuery.SetActiveDefinitionQuery(definitionQuery.Name);         
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine($"Error querying feature layer: {ex.Message}");
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.#ctor
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.InsertDefinitionQuery(ArcGIS.Desktop.Mapping.DefinitionQuery,System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetActiveDefinitionQuery(System.String)
    #region Apply A Definition Query Filter to a Feature Layer
    /// <summary>
    /// Applies a definition query filter to a feature layer within the specified map.
    /// </summary>
    /// <remarks>This method retrieves the "USNationalParks" feature layer from the provided map and applies a
    /// definition query to filter features based on the specified criteria. The definition query is named
    /// "CaliforniaParks" and filters features where the "STATE_ABBR" attribute equals "CA". The query is then set as
    /// the active definition query for the layer. <para> If the feature layer does not exist or is not named
    /// "USNationalParks", the method will throw an exception. </para></remarks>
    /// <param name="map">The map containing the feature layers to which the definition query will be applied.</param>
    public static void ApplyDefinitionQueryToFeatureLayer(Map map)
    {
      var us_parks = map.GetLayersAsFlattenedList()
            .OfType<FeatureLayer>().First(l => l.Name == "USNationalParks");

      QueuedTask.Run(() =>
      {
        var def_query = new DefinitionQuery("CaliforniaParks",
                              "STATE_ABBR = 'CA'");

        us_parks.InsertDefinitionQuery(def_query);
        //Set it active
        us_parks.SetActiveDefinitionQuery(def_query.Name);

        //or....also - set it active when it is inserted
        //us_parks.InsertDefinitionQuery(def_query, true);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.#ctor
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.InsertDefinitionQuery(ArcGIS.Desktop.Mapping.DefinitionQuery,System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetActiveDefinitionQuery(System.String)
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.CanSetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.SetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetFeatureOutline(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.FeatureOutlineType)
    // cref: ArcGIS.Desktop.Mapping.FeatureOutlineType
    #region Apply A Definition Query Filter With a Filter Geometry to a Feature Layer
    /// <summary>
    /// Applies a definition query with a filter geometry to the "Great Lakes" feature layer.
    /// </summary>
    /// <remarks>This method demonstrates how to create and apply a definition query to filter features in a
    /// feature layer. The query filters the "Great Lakes" layer to include only specific features based on their names
    /// and intersects the layer's geometry with a filter geometry derived from the "US_States" layer. The filter
    /// geometry is created using the visible features of the "US_States" layer.  The definition query is applied to the
    /// "Great Lakes" layer and set as the active query.</remarks>
    /// <param name="mapView">The <see cref="ArcGIS.Desktop.Mapping.MapView"/> instance representing the current map view. This parameter cannot be <see
    /// langword="null"/>.</param>
    public static void ApplyDefintionQueryWithFilterGeometry (MapView mapView)
    {
      if (mapView == null) return;
      var map = mapView.Map;
      var greatLakes = map.GetLayersAsFlattenedList()
            .OfType<FeatureLayer>().First(l => l.Name == "Great Lakes");
      var usa_states = map.GetLayersAsFlattenedList()
      .OfType<FeatureLayer>().First(l => l.Name == "US_States");

      QueuedTask.Run(() =>
      {
        //name must be unique
        var def_query = new DefinitionQuery("GreatLakes",
                              "NAME in ('Huron','Michigan','Erie')");

        //create a filter geometry - in this example we will use the outline geometry
        //of all visible features from a us states layer...the filter geometry will be
        //intersected with the layer feature geometry when added to the def query
        var filter_geom = usa_states.GetFeatureOutline(mapView, FeatureOutlineType.Visible);
        //other options...
        //var filter_geom = usa_states.GetFeatureOutline(mapView, FeatureOutlineType.All);
        //var filter_geom = usa_states.GetFeatureOutline(mapView, FeatureOutlineType.Selected);

        //Geometry must have a valid SR and be point, multi-point, line, or poly
        if (def_query.CanSetFilterGeometry(filter_geom))
        {
          def_query.SetFilterGeometry(filter_geom);
        }

        //Apply the def query
        greatLakes.InsertDefinitionQuery(def_query);
        //Set it active
        greatLakes.SetActiveDefinitionQuery(def_query.Name);

        //or....also - set it active when it is inserted
        //greatLakes.InsertDefinitionQuery(def_query, true);
      });
     }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetDefinitionQuery(System.String)
    #region Apply A Definition Query Filter to a Feature Layer Option 2
    /// <summary>
    /// Applies a definition query to the "USNationalParks" feature layer within the specified map.
    /// </summary>
    /// <remarks>This method filters the "USNationalParks" feature layer to include only features where the
    /// "STATE_ABBR" field equals "CA". The definition query is applied asynchronously using a queued task.</remarks>
    /// <param name="map">The map containing the feature layers to which the definition query will be applied.</param>
    public static void ApplyDefinitionQueryToFeatureLayerOption2(Map map)
    {
      var us_parks = map.GetLayersAsFlattenedList()
            .OfType<FeatureLayer>().First(l => l.Name == "USNationalParks");

      QueuedTask.Run(() =>
      {
        //inserts a new definition query and makes it active
        //it will be assigned a unique name
        us_parks.SetDefinitionQuery("STATE_ABBR = 'CA'");
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.DefinitionQueries
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.Name
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.WhereClause
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.GeometryUri
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.SpatialReference
    // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.GetFilterGeometry()
    #region Retrieve the Definition Query Filters for a Feature Layer
/// <summary>
/// Retrieves and logs the definition query filters for a feature layer named "USNationalParks" in the specified map.
/// </summary>
/// <remarks>This method enumerates the definition queries associated with the "USNationalParks" feature layer, if
/// any exist,  and logs their properties, including the name, where clause, geometry URI, spatial reference, and filter
/// geometry type. The operation is performed asynchronously using a queued task.</remarks>
/// <param name="map">The map containing the feature layers to search for the "USNationalParks" layer.</param>
    public static void RetrieveDefinitionQueryFilters(Map map)
    { 
      var us_parks = map.GetLayersAsFlattenedList()
            .OfType<FeatureLayer>().First(l => l.Name == "USNationalParks");

      QueuedTask.Run(() =>
      {
        //enumerate the layer's definition queries - if any
        var def_queries = us_parks.DefinitionQueries;
        foreach (var def_qry in def_queries)
        {
          var geom_uri = def_qry.GeometryUri ?? "null";
          var sr_wkid = def_qry.SpatialReference?.Wkid.ToString() ?? "null";
          var geom = def_qry.GetFilterGeometry();
          var geom_type = geom?.GeometryType.ToString() ?? "null";

          System.Diagnostics.Debug.WriteLine($" def_qry.Name: {def_qry.Name}");
          System.Diagnostics.Debug.WriteLine($" def_qry.WhereClause: {def_qry.WhereClause}");
          System.Diagnostics.Debug.WriteLine($" def_qry.GeometryUri: {geom_uri}");
          System.Diagnostics.Debug.WriteLine($" def_qry.SpatialReference: {sr_wkid}");
          System.Diagnostics.Debug.WriteLine($" def_qry.FilterGeometry: {geom_type}");
          System.Diagnostics.Debug.WriteLine("");
        }
      });      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetFeatureOutline(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.FeatureOutlineType)
    // cref: ArcGIS.Desktop.Mapping.FeatureOutlineType
    #region Get Feature Outlines from a Feature Layer
    /// <summary>
    /// Retrieves feature outlines from the "Great Lakes" feature layer in the specified map view.
    /// </summary>
    /// <remarks>This method demonstrates how to obtain feature outlines for all features, selected features,
    /// or visible features within the "Great Lakes" feature layer. The outlines are retrieved using the <see
    /// cref="FeatureOutlineType"/> enumeration. The method requires a valid <see cref="MapView"/> instance and operates
    /// asynchronously using the <see cref="QueuedTask.Run"/> method.</remarks>
    /// <param name="mapView">The map view containing the "Great Lakes" feature layer. Cannot be <see langword="null"/>.</param>
    public static void GetLayerFeatureOutline(MapView mapView)
    {
      if (mapView == null) return;
      var map = mapView?.Map;
      if (map == null) return;

      var greatLakes = map.GetLayersAsFlattenedList()
            .OfType<FeatureLayer>().First(l => l.Name == "Great Lakes");
      var michigan = map.GetBookmarks().First(b => b.Name == "Michigan");

      QueuedTask.Run(() =>
      {

        //get all features - multiple feature geometries are always returned as a
        //single multi-part
        var all_features_outline = greatLakes.GetFeatureOutline(mapView, FeatureOutlineType.All);

        //or get just the outline of selected features
        var qry = new QueryFilter()
        {
          SubFields = "*",
          WhereClause = "NAME in ('Huron','Michigan','Erie')"
        };
        greatLakes.Select(qry);
        var sel_features_outline = greatLakes.GetFeatureOutline(
            mapView, FeatureOutlineType.Selected);
        greatLakes.ClearSelection();

        //or just the visible features
        mapView?.ZoomTo(michigan);
        var visible_features_outline = greatLakes.GetFeatureOutline(
            mapView, FeatureOutlineType.Visible);
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMRotationVisualVariable
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.VisualVariables
    #region Get the attribute rotation field of a layer
    /// <summary>
    /// Retrieves the rotation field used by the renderer of the first feature layer in the active map.
    /// </summary>
    /// <remarks>This method identifies the first feature layer in the active map and retrieves the rotation
    /// field associated with its renderer, if available. The rotation field is determined by evaluating the Arcade
    /// expression stored in the visual variable for the Z dimension.</remarks>
    public static void GetRotationFieldOfRenderer()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      QueuedTask.Run(() =>
      {
        var cimRenderer = featureLayer.GetRenderer() as CIMUniqueValueRenderer;
        //Gets the visual variables from the renderer
        var cimRotationVariable = cimRenderer.VisualVariables.OfType<CIMRotationVisualVariable>().FirstOrDefault();
        //Gets the visual variable info for the z dimension
        var rotationInfoZ = cimRotationVariable.VisualVariableInfoZ;
        //The Arcade expression used to evaluate and return the field name for the rotation
        var rotationExpression = rotationInfoZ.ValueExpressionInfo.Expression; // this expression stores the field name  
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMRotationVisualVariable
    // cref: ArcGIS.Core.CIM.CIMSimpleRenderer.VisualVariables
    #region Find connected attribute field for rotation
    /// <summary>
    /// Identifies the attribute field connected to the rotation visual variable in the active map's feature layer.
    /// </summary>
    /// <remarks>This method retrieves the first feature layer in the active map and examines its renderer to
    /// find the rotation visual variable. The rotation visual variable is used to describe heading rotation, and the
    /// method extracts the associated field name from the visual variable's Z-direction expression.</remarks>
    public static void FindConnectedAttribute()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM renderer from the layer
        var cimRenderer = featureLayer.GetRenderer() as ArcGIS.Core.CIM.CIMSimpleRenderer;
        // get the collection of connected attributes for rotation
        var cimRotationVariable = cimRenderer.VisualVariables.OfType<ArcGIS.Core.CIM.CIMRotationVisualVariable>().FirstOrDefault();
        // the z direction is describing the heading rotation
        var rotationInfoZ = cimRotationVariable.VisualVariableInfoZ;
        var rotationExpression = rotationInfoZ.Expression; // this expression stores the field name  
      });
      
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.ScaleSymbols
    #region Toggle "Scale layer symbols when reference scale is set"
    /// <summary>
    /// Enables the scaling of symbols in the first feature layer based on the map's reference scale.
    /// </summary>
    /// <remarks>This method retrieves the first feature layer in the active map and modifies its
    /// configuration to scale symbols according to the map's reference scale. This setting ensures that symbols adjust
    /// their size dynamically when the map's reference scale is set.</remarks>
    public static void ScaleSymbols()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM layer definition
        var cimFeatureLayer = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMFeatureLayer;
        // turn on the option to scale the symbols in this layer based in the map's reference scale
        cimFeatureLayer.ScaleSymbols = true;
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.LayerCacheType
    // cref: ArcGIS.Desktop.Mapping.Layer.SetCacheOptions(ArcGIS.Desktop.Mapping.LayerCacheType)
    // cref: ArcGIS.Desktop.Mapping.Layer.SetDisplayCacheMaxAge(System.TimeSpan)
    #region Set the layer cache
    /// <summary>
    /// Configures the display cache settings for the first feature layer in the active map.
    /// </summary>
    /// <remarks>This method retrieves the first <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> from the active map and sets its
    /// display cache  maximum age to 2 minutes. The display cache controls how long cached data is retained for
    /// rendering purposes.</remarks>
    public static void SetLayerCache()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // change from the default 5 min to 2 min
        featureLayer.SetDisplayCacheMaxAge(TimeSpan.FromMinutes(2));
      });
      #endregion
    }
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.UseSelectionSymbol
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.SelectionColor
    // cref: ArcGIS.Desktop.Mapping.MapView.SelectFeatures(ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Mapping.SelectionCombinationMethod, System.Boolean, System.Boolean)
    #region Change the layer selection color
    /// <summary>
    /// Changes the selection color of the first visible feature layer in the active map to red.
    /// </summary>
    /// <remarks>This method retrieves the first feature layer in the active map, modifies its selection color
    /// to red, disables the default selection symbol, and ensures the layer is visible. It then performs a selection
    /// operation using the current map extent.</remarks>
    public static void ChangeSelectionColor()
    {

      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM definition of the layer
        var layerDef = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
        // disable the default symbol
        layerDef.UseSelectionSymbol = false;
        // assign a new color
        layerDef.SelectionColor = ColorFactory.Instance.RedRGB;
        // apply the definition to the layer
        featureLayer.SetDefinition(layerDef);

        if (!featureLayer.IsVisible)
          featureLayer.SetVisibility(true);
        //Do a selection

        MapView.Active.SelectFeatures(MapView.Active.Extent);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.RemoveLayers(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.Layer>)
    // cref: ArcGIS.Desktop.Mapping.Map.RemoveLayer(ArcGIS.Desktop.Mapping.Layer)
    #region Removes all layers that are unchecked
    /// <summary>
    /// Removes all layers from the active map that are not visible, including both individual layers and group layers.
    /// </summary>
    /// <remarks>This method iterates through the layers in the active map and removes those that are
    /// unchecked (i.e., not visible).  It handles both individual layers and group layers, ensuring that empty and
    /// unchecked group layers are also removed. The operation is performed asynchronously using queued tasks to ensure
    /// thread safety.</remarks>
    public static async void RemoveAllUncheckedLayers()
    {
      var map = MapView.Active.Map;
      if (map == null)
        return;
      //Get the group layers first
      IReadOnlyList<GroupLayer> groupLayers = map.Layers.OfType<GroupLayer>().ToList();
      //Iterate and remove the layers within the group layers that are unchecked.
      foreach (var groupLayer in groupLayers)
      {
        //Get layers that not visible within the group
        var layers = groupLayer.Layers.Where(l => l.IsVisible == false).ToList();
        //Remove all the layers that are not visible within the group
        await QueuedTask.Run(() => map.RemoveLayers(layers));
      }

      //Group Layers that are empty and are unchecked
      foreach (var group in groupLayers)
      {
        if (group.Layers.Count == 0 && group.IsVisible == false) //No layers in the group
        {
          //remove the group
          await QueuedTask.Run(() => map.RemoveLayer(group));
        }
      }

      //Get Layers that are NOT Group layers and are unchecked
      var notAGroupAndUnCheckedLayers = map.Layers.Where(l => !(l is GroupLayer) && l.IsVisible == false).ToList();
      //Remove all the non group layers that are not visible
      await QueuedTask.Run(() => map.RemoveLayers(notAGroupAndUnCheckedLayers));
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.RemoveLayer(ArcGIS.Desktop.Mapping.Layer)
    // cref: ArcGIS.Desktop.Mapping.GroupLayer
    // cref: ArcGIS.Desktop.Mapping.CompositeLayer.Layers
    #region Remove empty groups
/// <summary>
/// Removes all empty group layers from the specified map.
/// </summary>
/// <remarks>This method iterates through all group layers in the provided map and removes any group layer  that
/// does not contain any child layers. The operation is performed asynchronously on the  ArcGIS Pro QueuedTask to ensure
/// thread safety.</remarks>
/// <param name="map">The map from which empty group layers will be removed. Cannot be null.</param>
    public static async void RemoveEmptyGroups(Map map)
    {
      //Get the group layers
      IReadOnlyList<GroupLayer> groupLayers = map.Layers.OfType<GroupLayer>().ToList();
      foreach (var group in groupLayers)
      {
        if (group.Layers.Count == 0) //No layers in the group
        {
          //remove the group
          await QueuedTask.Run(() => map.RemoveLayer(group));
        }
      }
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMMap.GeneralPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry
    // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.Abbreviation
    // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.Text
    // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.MaplexAbbreviationType
    // cref: ArcGIS.Core.CIM.MaplexAbbreviationType
    // cref: ArcGIS.Core.CIM.CIMMaplexDictionary
    // cref: ArcGIS.Core.CIM.CIMMaplexDictionary.Name
    // cref: ArcGIS.Core.CIM.CIMMaplexDictionary.MaplexDictionary
    // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.Dictionaries
    #region Create and apply Abbreviation Dictionary in the Map Definition to a layer
    /// <summary>
    /// Creates and applies a Maplex abbreviation dictionary to the active map's labeling engine.
    /// </summary>
    /// <remarks>This method defines a collection of abbreviation rules and associates them with the Maplex
    /// labeling engine  of the active map. The abbreviation dictionary specifies how certain text strings should be
    /// abbreviated  when labels are generated. The dictionary is applied to the map's definition, enabling consistent
    /// labeling  behavior across layers.</remarks>
    public static void CreateDictionary()
    {
      //Get the map's definition
      var mapDefn = MapView.Active.Map.GetDefinition();
      //Get the Map's Maplex labelling engine properties
      var mapDefnPlacementProps = mapDefn.GeneralPlacementProperties as CIMMaplexGeneralPlacementProperties;

      //Define the abbreviations we need in an array            
      List<CIMMaplexDictionaryEntry> abbreviationDictionary = new List<CIMMaplexDictionaryEntry>
            {
                new CIMMaplexDictionaryEntry {
                Abbreviation = "Hts",
                Text = "Heights",
                MaplexAbbreviationType = MaplexAbbreviationType.Ending

             },
                new CIMMaplexDictionaryEntry
                {
                    Abbreviation = "Ct",
                    Text = "Text",
                    MaplexAbbreviationType = MaplexAbbreviationType.Ending

                }
                //etc
            };
      //The Maplex Dictionary - can hold multiple Abbreviation collections
      var maplexDictionary = new List<CIMMaplexDictionary>
            {
                new CIMMaplexDictionary {
                    Name = "NameEndingsAbbreviations",
                    MaplexDictionary = abbreviationDictionary.ToArray()
                }

            };
      //Set the Maplex Label Engine Dictionary property to the Maplex Dictionary collection created above.
      mapDefnPlacementProps.Dictionaries = maplexDictionary.ToArray();
      //Set the Map definition 
      MapView.Active.Map.SetDefinition(mapDefn);
    }
    /// <summary>
    /// Applies an abbreviation dictionary to the labeling properties of the first feature layer in the active map.
    /// </summary>
    /// <remarks>This method modifies the labeling properties of the first feature layer in the active map to
    /// use a predefined abbreviation dictionary. It enables label abbreviation, disables label stacking, and updates
    /// the layer's definition accordingly. The method must be called within the context of an active map and requires
    /// at least one feature layer to be present.</remarks>
    public static void ApplyDictionary()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().First();

      QueuedTask.Run(() =>
      {
        //Creates Abbreviation dictionary and adds to Map Definition                                
        CreateDictionary();
        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Modify label Placement props to use abbreviation dictionary 
        CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        theLabelClass.MaplexLabelPlacementProperties.DictionaryName = "NameEndingsAbbreviations";
        theLabelClass.MaplexLabelPlacementProperties.CanAbbreviateLabel = true;
        theLabelClass.MaplexLabelPlacementProperties.CanStackLabel = false;
        //Set the labelClasses back
        lyrDefn.LabelClasses = listLabelClasses.ToArray();
        //set the layer's definition
        featureLayer.SetDefinition(lyrDefn);
      });
    }

    #endregion
  }
}

