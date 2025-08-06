using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Offline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippetsMaps
  {
    #region ProSnippet Group: Maps
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapView.Active
    // cref: ArcGIS.Desktop.Mapping.MapView.Map
    // cref: ArcGIS.Desktop.Mapping.Map
    #region Get the active map
/// <summary>
/// Retrieves the currently active map in the application.
/// </summary>
/// <remarks>This method returns the map associated with the active <see cref="ArcGIS.Desktop.Mapping.MapView"/>. If no map view is
/// active, the method will return <see langword="null"/>.</remarks>
/// <returns>The active <see cref=Map"/> instance if a map is currently active; otherwise, <see langword="null"/>.</returns>
    public static Map? GetActiveMap()
    {
      //This is how you get the active map
      var map = MapView.Active?.Map;
      return map;
    }
    
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMap(System.String,ArcGIS.Core.CIM.MapType,ArcGIS.Core.CIM.MapViewingMode,ArcGIS.Desktop.Mapping.Basemap)
    // cref: ArcGIS.Desktop.Mapping.Map
    #region Create a new map with a default basemap layer
    /// <summary>
    /// Creates a new map with the specified name and a default basemap layer.
    /// </summary>
    /// <remarks>The map is created using the project default basemap. This method must be called within the context
    /// of a queued task.</remarks>
    /// <param name="mapName">The name of the map to create. This value cannot be <see langword="null"/> or empty.</param>
    /// <returns>A task that represents the asynchronous operation of creating the map.</returns>
    public static async void CreateMapAsync(string mapName)
    {
      await QueuedTask.Run(() =>
      {
        var map = MapFactory.Instance.CreateMap(mapName, ArcGIS.Core.CIM.MapType.Map, ArcGIS.Core.CIM.MapViewingMode.Map, Basemap.ProjectDefault);
          //TODO: use the map...
      });     
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapProjectItem
    // cref: ArcGIS.Desktop.Mapping.MapProjectItem.GetMap()
    // cref: ArcGIS.Desktop.Mapping.Map
    // cref: ArcGIS.Desktop.Core.FrameworkExtender.CreateMapPaneAsync(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapViewingMode, ArcGIS.Desktop.Mapping.TimeRange)
    #region Find a map within a project and open it
    /// <summary>
    /// Opens an existing map in the specified project by its name.
    /// </summary>
    /// <remarks>If a map with the specified name is not found in the project, no action is taken. Ensure the
    /// map name is correct and exists in the project before calling this method.</remarks>
    /// <param name="project">The ArcGIS Pro project containing the map to open. Cannot be <see langword="null"/>.</param>
    /// <param name="mapName">The name of the map to open. The comparison is case-insensitive and uses the current culture.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the map is opened in a map view.</returns>
    public static async Task OpenExistingMapAsync(Project project, string mapName)
    {
      var map = await QueuedTask.Run(() =>
      {
        //Finding the first project item with name matches with mapName
        MapProjectItem? mpi = project.GetItems<MapProjectItem>()
          .FirstOrDefault(m => m.Name.Equals(mapName, StringComparison.CurrentCultureIgnoreCase));
        var mapFromItem = mpi?.GetMap();  
        return mapFromItem;
      });

      if (map != null)
      {
        //Open the map in a new map pane
        //Must be called from the UI thread
        await ProApp.Panes.CreateMapPaneAsync(map);
      }
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapFactory.CanCreateMapFrom(ArcGIS.Desktop.Core.Item)
    // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMapFromItem(ArcGIS.Desktop.Core.Item)
    // cref: ArcGIS.Desktop.Mapping.Map
    // cref: ArcGIS.Desktop.Core.FrameworkExtender.CreateMapPaneAsync(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapViewingMode, ArcGIS.Desktop.Mapping.TimeRange)
    #region Open a webmap
    /// <summary>
    /// Opens a web map from the Project pane's Portal tab.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static async Task<Map> OpenWebMapAsync(Map map)
    {     
      //Assume we get the selected webmap from the Project pane's Portal tab
      if (Project.Current.SelectedItems.Count > 0)
      {
        if (MapFactory.Instance.CanCreateMapFrom(Project.Current.SelectedItems[0]))
        {
          map = MapFactory.Instance.CreateMapFromItem(Project.Current.SelectedItems[0]);
          await ProApp.Panes.CreateMapPaneAsync(map);
        }
      }
      return map;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.IMapPane
    #region Get Map Panes
    /// <summary>
    /// Gets all the map panes in the application.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IMapPane> GetMapPanes()
    {
      //Sorted by Map Uri
      return ProApp.Panes.OfType<IMapPane>().OrderBy((mp) => mp.MapView.Map.URI ?? mp.MapView.Map.Name);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.IMapPane
    // cref: ArcGIS.Desktop.Mapping.IMapPane.MapView
    // cref: ArcGIS.Desktop.Mapping.Map.URI
    #region Get the Unique List of Maps From the Map Panes
    /// <summary>
    /// Gets a unique list of maps from all the map panes in the application.
    /// </summary>
    /// <returns></returns>
    public static IReadOnlyList<Map> GetMapsFromMapPanes()
    {
      //Gets the unique list of Maps from all the MapPanes.
      //Note: The list of maps retrieved from the MapPanes
      //maybe less than the total number of Maps in the project.
      //It depends on what maps the user has actually opened.
      var mapPanes = ProApp.Panes.OfType<IMapPane>()
                  .GroupBy((mp) => mp.MapView.Map.URI).Select(grp => grp.FirstOrDefault());
      List<Map> uniqueMaps = new List<Map>();
      foreach (var pane in mapPanes)
        uniqueMaps.Add(pane.MapView.Map);
      return uniqueMaps;
    }
    #endregion
    // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Caption
    // cref: ArcGIS.Desktop.Mapping.Map.SetName
    #region Change the Map name and caption of the active pane
    /// <summary>
    /// Changes the name of the active map and the caption of the active pane.
    /// </summary>
    public static void ModifyMapAndPane()
    {
      QueuedTask.Run(() =>
      {
        //Note: call within QueuedTask.Run()
        MapView.Active.Map.SetName("Test");
      });
      ProApp.Panes.ActivePane.Caption = "Caption";
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapFactory.CanConvertMap(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapConversionType)
    // cref: ArcGIS.Desktop.Mapping.MapConversionType
    // cref: ArcGIS.Desktop.Mapping.MapFactory.ConvertMap(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapConversionType, System.Boolean)
    #region Convert Map to Local Scene
    /// <summary>
    /// Converts a map to a local scene.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Task ConvertMapToScene(Map map)
    {
      return QueuedTask.Run(() =>
      {     
        //Note: Run within the context of QueuedTask.Run
        bool canConvertMap = MapFactory.Instance.CanConvertMap(map, MapConversionType.SceneLocal);
        if (canConvertMap)
          MapFactory.Instance.ConvertMap(map, MapConversionType.SceneLocal, true);        
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetBasemapsAsync(ArcGIS.Desktop.Core.ArcGISPortal)
    // cref: ArcGIS.Desktop.Core.Portal.PortalItem
    // cref: ArcGIS.Desktop.Mapping.Map.SetBasemapLayers(ArcGIS.Desktop.Mapping.Basemap)
    #region Get Basemaps
    /// <summary>
    /// Gets the basemaps available in the current project and/or active portal and sets one of them as the basemap for the active map.
    /// </summary>
    public static async void GetBasemaps()
    {
      //Basemaps stored locally in the project. This is usually an empty collection
      string localBasemapTypeID = "cim_map_basemap";
      var localBasemaps = await QueuedTask.Run(() =>
      {
        var mapContainer = Project.Current.GetProjectItemContainer("Map");
        return mapContainer.GetItems().Where(i => i.TypeID == localBasemapTypeID).ToList();
      });

      //portal basemaps. If there is no current active portal, the usual default
      //is arcgis online
      var portal = ArcGISPortalManager.Current.GetActivePortal();
      var portalBaseMaps = await portal.GetBasemapsAsync();

      //use one of them...local or portal...
      //var map = MapView.Active.Map;
      //QueuedTask.Run(() => map?.SetBasemapLayers(portalBaseMaps[0]));     
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.SaveAsFile(System.String, System.Boolean)
    #region Save Map as MapX
    /// <summary>
    /// Saves the map as a MapX file.
    /// </summary>
    /// <param name="map"></param>
    public static void SaveAsMapX(Map map)
    {  
      map.SaveAsFile(@"C:\Data\MyMap.mapx", true);
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.SaveAsWebMapFile(System.String)
    #region Save 2D Map as WebMap on Disk
    /// <summary>
    /// Saves the 2D map as a WebMap file on disk.
    /// </summary>
    /// <param name="map"></param>
    public static void SaveAsWebMapMapFile(Map map)
    { 
      //2D maps only
      //Must be on the QueuedTask.Run(...)
      if (map.DefaultViewingMode == MapViewingMode.Map)
        //Only webmap compatible layers will be saved out to the file
        map.SaveAsWebMapFile(@"C:\Data\MyMap.json");
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.SetClipGeometry(ArcGIS.Core.Geometry.Polygon, ArcGIS.Core.CIM.CIMLineSymbol)
    #region Clip Map to the provided clip polygon
    /// <summary>
    /// Set map clipping to the provided clip polygon.
    /// </summary>
    public static void ClipMap()
    {
      QueuedTask.Run(() =>
      {       
        //Run within QueuedTask
        var map = MapView.Active.Map;
        //A layer to use for the clip extent
        var lyrOfInterest = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().Where(l => l.Name == "TestPoly").FirstOrDefault();
        //Get the polygon to use to clip the map
        var extent = lyrOfInterest.QueryExtent();
        var polygonForClipping = PolygonBuilderEx.CreatePolygon(extent);
        //Clip the map using the layer's extent
        map.SetClipGeometry(polygonForClipping,
              SymbolFactory.Instance.ConstructLineSymbol(
              SymbolFactory.Instance.ConstructStroke(
                ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Dash)));        
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.ClearClipGeometry()
    #region Clear the current map clip geometry 
    /// <summary>
    /// Clear the current map clip geometry.
    /// </summary>
    public static void ClearClipMap()
    {
      QueuedTask.Run(() =>
      {
        var map = MapView.Active.Map;
        //Clear the Map clip.
        //If no clipping is set then this is a no-op.
        map.ClearClipGeometry();        
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetClipGeometry()
    // cref: ArcGIS.Core.CIM.ClippingMode.MapExtent
    // cref: ArcGIS.Core.CIM.ClippingMode.CustomShape
    #region Get the map clipping geometry
    /// <summary>
    /// Get the current map clipping geometry.
    /// </summary>
    public static void GetClipMapGeometry()
    {
      QueuedTask.Run(() =>
      {        
        var map = MapView.Active.Map;
        //If clipping is set to ArcGIS.Core.CIM.ClippingMode.None or ArcGIS.Core.CIM.ClippingMode.MapSeries null is returned
        //If clipping is set to ArcGIS.Core.CIM.ClippingMode.MapExtent the ArcGIS.Core.CIM.CIMMap.CustomFullExtent is returned.
        //Otherwise, if clipping is set to ArcGIS.Core.CIM.ClippingMode.CustomShape the custom clip polygon is returned.
        var poly = map.GetClipGeometry();
        //You can use the polygon returned
        //For example: We make a polygon graphic element and add it to a Graphics Layer.
        var gl = map.GetLayersAsFlattenedList().OfType<GraphicsLayer>().FirstOrDefault();
        if (gl == null) return;
        var polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(CIMColor.CreateRGBColor(255, 255, 0));
        var cimGraphicElement = new CIMPolygonGraphic
        {
          Polygon = poly,
          Symbol = polygonSymbol.MakeSymbolReference()
        };
        gl.AddElement(cimGraphicElement);        
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetLocationUnitFormat()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.DisplayName
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.UnitCode
    #region Get the Current Map Location Unit
    /// <summary>
    /// Gets the current map location unit format.
    /// </summary>
    /// <param name="map"></param>
    public static void GetCurrentMapLocationUnit(Map map)
    {      
      //var map = MapView.Active.Map;
      //Must be on the QueuedTask.Run()

      //Get the current location unit
      var loc_unit = map.GetLocationUnitFormat();
      var line = $"{loc_unit.DisplayName}, {loc_unit.UnitCode}";
      System.Diagnostics.Debug.WriteLine(line);      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableLocationUnitFormats()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    #region Get the Available List of Map Location Units
    /// <summary>
    /// Retrieves the list of available location unit formats for the specified map.
    /// </summary>
    /// <remarks>This method must be called within the context of a <see cref="QueuedTask.Run"/> operation.
    /// The available location unit formats depend on the spatial reference of the map. For example, linear location
    /// unit formats are excluded if the map's spatial reference is geographic.</remarks>
    /// <param name="map">The map for which to retrieve the available location unit formats. Cannot be <see langword="null"/>.</param>
    public static void GetAvailableMapLocationUnits(Map map)
    {
      QueuedTask.Run(() => {
        //Linear location unit formats are not included if the map sr
        //is geographic.
        var loc_units = map.GetAvailableLocationUnitFormats();
      });           
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetLocationUnitFormat()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.FormatLocation(ArcGIs.Core.Geometry.Coordinate2D, ArcGIS.Core.Geometry.SpatialReference)
    #region Format a Location Using the Current Map Location Unit
    /// <summary>
    /// Formats the current camera location of the specified map view using the map's location unit format.
    /// </summary>
    /// <remarks>This method retrieves the current camera location of the map view, formats it using the map's
    /// location unit format,  and outputs the formatted location string to the debug console. The formatting is
    /// performed asynchronously on a queued task.</remarks>
    /// <param name="ArcGIS.Desktop.Mapping.MapView">The map view whose camera location will be formatted.</param>
    public static void FormatALocationUsingMapLocationUnit(MapView mapView)
    {
      var map = mapView.Map;

      QueuedTask.Run(() =>
      {
        //Get the current view camera location
        var center_pt = new Coordinate2D(mapView.Camera.X, mapView.Camera.Y);
        //Get the current location unit
        var loc_unit = map.GetLocationUnitFormat();

        //Format the camera location
        var str = loc_unit.FormatLocation(center_pt, map.SpatialReference);
        System.Diagnostics.Debug.WriteLine($"Formatted location: {str}");
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableLocationUnitFormats()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    // cref: ArcGIS.Desktop.Mapping.Map.SetLocationUnitFormat(ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat)
    #region Set the Location Unit for the Current Map
    /// <summary>
    /// Sets the location unit format for the specified map to the last available unit format.
    /// </summary>
    /// <remarks>This method retrieves the list of available location unit formats for the map associated with
    /// the provided <paramref name="mapView"/>  and sets the location unit format to the last unit in the list. The
    /// operation is performed asynchronously using a queued task.</remarks>
    /// <param name="mapView">The <see cref="ArcGIS.Desktop.Mapping.MapView"/> instance representing the map view whose location unit format is to be set.</param>
    public static void SetLocationUnitForMap(MapView mapView)
    {
      var map = mapView.Map;

      QueuedTask.Run(() =>
      {
        //Get the list of available location unit formats
        //for the current map
        var loc_units = map.GetAvailableLocationUnitFormats();

        //arbitrarily use the last unit in the list
        map.SetLocationUnitFormat(loc_units.Last());
      });    
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationUnitFormat()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    #region Get the Current Map Elevation Unit
    /// <summary>
    /// Retrieves the elevation unit format for the specified map.
    /// </summary>
    /// <remarks>This method returns the elevation unit format associated with the provided map. If the map is
    /// not a scene,  the default project distance unit will be returned. The elevation unit format includes details
    /// such as the  display name and unit code.</remarks>
    /// <param name="map">The map for which to retrieve the elevation unit format. Must be a valid instance of <see
    /// cref="ArcGIS.Desktop.Mapping.Map"/>.</param>
    public static void GetMapElevationUnit(Map map)
    {
      QueuedTask.Run(() => {
        var elev_unit = map.GetElevationUnitFormat();
        var line = $"{elev_unit.DisplayName}, {elev_unit.UnitCode}";
        System.Diagnostics.Debug.WriteLine(line);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableElevationUnitFormats()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    #region Get the Available List of Map Elevation Units
    /// <summary>
    /// Retrieves the available elevation unit formats for the specified map.
    /// </summary>
    /// <remarks>If the specified map is not a scene, the method returns the list of current project distance
    /// units. This method must be called within the context of a queued task.</remarks>
    /// <param name="map">The map for which to retrieve the elevation unit formats. Cannot be null.</param>
    public static void GetMapElevationUnits(Map map)
    {
      QueuedTask.Run( () => {
        //If the map is not a scene, the list of current
        //Project distance units will be returned
        var elev_units = map.GetAvailableElevationUnitFormats();
      });      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationUnitFormat()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.FormatValue(System.Double)
    #region Format an Elevation Using the Current Map Elevation Unit
    /// <summary>
    /// Formats the elevation of the map view's camera using the current map elevation unit.
    /// </summary>
    /// <remarks>This method retrieves the elevation unit format for the specified map and formats the
    /// camera's elevation value accordingly. If the map is not a scene, the default project distance unit is used. The
    /// formatted elevation is written to the debug output.</remarks>
    /// <param name="mapView">The <see cref="ArcGIS.Desktop.Mapping.MapView"/> instance whose camera elevation will be formatted.</param>
    public static void FormatElevationUsingMapUnit(MapView mapView)
    {
      var map = mapView.Map;

      QueuedTask.Run(() =>
      {
        //Get the current elevation unit. If the map is not
        //a scene the default Project distance unit is returned
        var elev_unit = map.GetElevationUnitFormat();

        //Format the view camera elevation
        var str = elev_unit.FormatValue(mapView.Camera.Z);

        System.Diagnostics.Debug.WriteLine($"Formatted elevation: {str}");
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableElevationUnitFormats()
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
    // cref: ArcGIS.Desktop.Mapping.Map.SetElevationUnitFormat( ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat)
    #region Set the Elevation Unit for the Current Map
    /// <summary>
    /// Sets the elevation unit format for the specified map.
    /// </summary>
    /// <remarks>This method sets the elevation unit format for the provided map. It is only applicable to
    /// maps that are scenes. Attempting to set the elevation unit format on a map that is not a scene will result in an
    /// <see cref="InvalidOperationException" />. The elevation unit format is chosen arbitrarily from the list of
    /// available formats for the map.</remarks>
    /// <param name="map">The map for which the elevation unit format will be set. Must be a scene.</param>
    public static void SetElevationUnitForMap(Map map)
    {

      QueuedTask.Run(() =>
      {
        //Trying to set the elevation unit on a map other than
        //a scene will throw an InvalidOperationException
        if (map.IsScene)
        {
          //Get the list of available elevation unit formats
          //for the current map
          var loc_units = map.GetAvailableElevationUnitFormats();
          //arbitrarily use the last unit in the list
          map.SetElevationUnitFormat(loc_units.Last());
        }
      });
    }
    #endregion

    #region ProSnippet Group: Offline Map
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanGenerateReplicas(ArcGIS.Desktop.Mapping.Map)
    #region Check Map Has Sync-Enabled Content
    /// <summary>
    /// Checks whether the active map contains sync-enabled content that can be used for offline workflows.
    /// </summary>
    /// <remarks>This method evaluates the active map to determine if it contains content that supports
    /// synchronization for offline use. It performs the check asynchronously on a background thread using the ArcGIS
    /// Pro QueuedTask framework.</remarks>
    public static void CheckMapHasSyncEnabledContent()
    {
      //namespace ArcGIS.Desktop.Mapping.Offline
      var map = MapView.Active.Map;

      //await if needed...
      QueuedTask.Run(() =>
      {
        var hasSyncEnabledContent = GenerateOfflineMap.Instance.GetCanGenerateReplicas(map);
        if (hasSyncEnabledContent)
        {
          //TODO - use status...
        }
      });      
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanGenerateReplicas(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GenerateReplicas(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams)
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams.#ctor
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams.Extent
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams.DestinationFolder
    #region Generate Replicas for Sync-Enabled Content
    /// <summary>
    /// Generates replicas for sync-enabled content in the active map, allowing the content to be taken offline.
    /// </summary>
    /// <remarks>This method checks whether the active map contains sync-enabled content that can be taken
    /// offline.  If such content exists, it generates replicas and stores the offline content locally in a SQLite
    /// database. The spatial reference of the extent must match the spatial reference of the map. The destination
    /// folder for the offline content can be specified, but if left blank, it defaults to the project's offline maps
    /// location.</remarks>
    public static void GenerateReplicasSyncEnabledContent()
    {
      //namespace ArcGIS.Desktop.Mapping.Offline
      var extent = MapView.Active.Extent;
      var map = MapView.Active.Map;

      //await if needed...
      QueuedTask.Run(() =>
      {
        //Check map has sync-enabled content that can be taken offline
        var hasSyncEnabledContent = GenerateOfflineMap.Instance.GetCanGenerateReplicas(map);
        if (hasSyncEnabledContent)
        {
          //Generate Replicas and take the content offline
          //sync-enabled content gets copied local into a SQLite DB
          var gen_params = new GenerateReplicaParams()
          {
            Extent = extent, //SR of extent must match map SR

            //DestinationFolder can be left blank, if specified,
            //it must exist. Defaults to project offline maps location
            DestinationFolder = @"C:\Data\Offline"
          };
          //Sync-enabled layer content will be resourced to point to the
          //local replica content.
          GenerateOfflineMap.Instance.GenerateReplicas(map, gen_params);

        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanSynchronizeReplicas(ArcGIS.Desktop.Mapping.Map)
    #region Check Map Has Local Syncable Content
    /// <summary>
    /// Determines whether the active map contains local content that can be synchronized with replicas.
    /// </summary>
    /// <remarks>This method checks the synchronization capability of the currently active map in the
    /// application. It uses the <see
    /// cref="ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanSynchronizeReplicas(ArcGIS.Desktop.Mapping.Map)"/>
    /// method to evaluate whether the map has local syncable content.</remarks>
    public static void CheckMapHasLocalSyncableContent()
    {

      //namespace ArcGIS.Desktop.Mapping.Offline
      var map = MapView.Active.Map;

      //await if needed...
      QueuedTask.Run(() =>
      {
        //Check map has local syncable content
        var canSyncContent = GenerateOfflineMap.Instance.GetCanSynchronizeReplicas(map);
        if (canSyncContent)
        {
          //TODO - use status
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanSynchronizeReplicas(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.SynchronizeReplicas(ArcGIS.Desktop.Mapping.Map)
    #region Synchronize Replicas for Syncable Content
    /// <summary>
    /// Performs a bi-directional sync between all replica content in the map.
    /// </summary>
    /// <remarks>This method checks whether the active map contains local syncable content and, if so,
    /// synchronizes the replicas. Synchronization involves pushing changes made locally to the parent replica and
    /// pulling changes from the parent replica to the local replica. Note that unsaved edits are not included in the
    /// synchronization process.</remarks>
    public static void SynchronizeReplicas()
    {
      //namespace ArcGIS.Desktop.Mapping.Offline
      var map = MapView.Active.Map;

      //await if needed...
      QueuedTask.Run(() =>
      {
        //Check map has local syncable content
        var canSyncContent = GenerateOfflineMap.Instance.GetCanSynchronizeReplicas(map);
        if (canSyncContent)
        {
          //Sync Replicas - changes since last sync are pushed to the
          //parent replica. Parent changes are pulled to the client.
          //Unsaved edits are _not_ sync'd. 
          GenerateOfflineMap.Instance.SynchronizeReplicas(map);
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanRemoveReplicas(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.RemoveReplicas(ArcGIS.Desktop.Mapping.Map)
    #region Remove Replicas for Syncable Content
    /// <summary>
    /// Removes all replicas from the map content.
    /// </summary>
    /// <remarks>This method checks whether the active map contains local syncable content and, if so, removes
    /// the replicas. Any unsynchronized changes will be lost during this operation. To preserve changes, synchronize
    /// the replicas  before calling this method. After removal, local syncable content is re-sourced to point to the
    /// original service.</remarks>
    public static void RemoveReplicas()
    {
      //namespace ArcGIS.Desktop.Mapping.Offline
      var extent = MapView.Active.Extent;
      var map = MapView.Active.Map;

      //await if needed...
      QueuedTask.Run(() =>
      {
        //Check map has local syncable content
        //Either..
        //var canSyncContent = GenerateOfflineMap.Instance.GetCanSynchronizeReplicas(map);
        //Or...both accomplish the same thing...
        var canRemove = GenerateOfflineMap.Instance.GetCanRemoveReplicas(map);
        if (canRemove)
        {
          //Remove Replicas - any unsync'd changes are lost
          //Call sync _first_ to push any outstanding changes if
          //needed. Local syncable content is re-sourced
          //to point to the service
          GenerateOfflineMap.Instance.RemoveReplicas(map);
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanExportRasterTileCache(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetExportRasterTileCacheScales(ArcGIS.Desktop.Mapping.Map, ArcGIs.Core.Geometry.Envelope)
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.#ctor
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.Extent
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.MaximumUserDefinedScale
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.DestinationFolder
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.ExportRasterTileCache(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams)
    #region Export Map Raster Tile Cache Content
    /// <summary>
    /// Exports raster tile cache content for the active map to a specified location.
    /// </summary>
    /// <remarks>This method checks if the active map contains exportable raster content and, if so, exports
    /// the raster tile cache  using the specified extent and a user-defined maximum scale level. The export destination
    /// folder can be configured  in the export parameters; if not set, the output defaults to the offline maps location
    /// specified in the project  properties or the current project folder.  Exporting raster tile cache content can be
    /// a time-consuming operation depending on the extent and scale level  selected, as well as network speed. Ensure
    /// that the active map and extent are properly configured before calling  this method.</remarks>
    public static void ExportMapRasterTileCacheContent()
    {
      //namespace ArcGIS.Desktop.Mapping.Offline
      var extent = MapView.Active.Extent;
      var map = MapView.Active.Map;

      //await if needed...
      QueuedTask.Run(() =>
      {
        //Does the map have any exportable raster content?
        var canExport = GenerateOfflineMap.Instance.GetCanExportRasterTileCache(map);
        if (canExport)
        {
          //Check the available LOD scale ranges
          var scales = GenerateOfflineMap.Instance.GetExportRasterTileCacheScales(map, extent);
          //Pick the desired LOD scale
          var max_scale = scales[scales.Count() / 2];

          //Configure the export parameters
          var export_params = new ExportTileCacheParams()
          {
            Extent = extent,//Use same extent as was used to retrieve scales
            MaximumUserDefinedScale = max_scale
            //DestinationFolder = .... (optional)
          };
          //If DestinationFolder is not set, output defaults to project
          //offline maps location set in the project properties. If that is 
          //not set, output defaults to the current project folder location.

          //Do the export. Depending on the MaximumUserDefinedScale and the
          //area of the extent requested, this can take minutes for tile packages
          //over 1 GB or less if your network speed is slow...
          GenerateOfflineMap.Instance.ExportRasterTileCache(map, export_params);
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanExportVectorTileCache(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetExportVectorTileCacheScales(ArcGIS.Desktop.Mapping.Map, ArcGIs.Core.Geometry.Envelope)
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.#ctor
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.Extent
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.MaximumUserDefinedScale
    // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.DestinationFolder
    // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.ExportVectorTileCache(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams)
    #region Export Map Vector Tile Cache Content
    /// <summary>
    /// Exports the vector tile cache for the active map to a specified location.
    /// </summary>
    /// <remarks>This method checks if the active map contains exportable vector tile content and, if so,
    /// exports the vector tile cache using the specified extent and level of detail (LOD) scale. The export operation
    /// can take significant time depending on the size of the requested area and the maximum user-defined scale.  The
    /// exported tile cache is saved to the destination folder specified in the export parameters. If no destination
    /// folder is set, the output defaults to the offline maps location configured in the project properties, or the
    /// current project folder if no offline maps location is configured.  Ensure that the active map and its extent are
    /// properly configured before calling this method.</remarks>
    public static void ExportMapVectorTileCache()
    {

      //namespace ArcGIS.Desktop.Mapping.Offline
      var extent = MapView.Active.Extent;
      var map = MapView.Active.Map;

      //await if needed...
      QueuedTask.Run(() =>
      {
        //Does the map have any exportable vector tile content?
        var canExport = GenerateOfflineMap.Instance.GetCanExportVectorTileCache(map);
        if (canExport)
        {
          //Check the available LOD scale ranges
          var scales = GenerateOfflineMap.Instance.GetExportVectorTileCacheScales(map, extent);
          //Pick the desired LOD scale
          var max_scale = scales[scales.Count() / 2];

          //Configure the export parameters
          var export_params = new ExportTileCacheParams()
          {
            Extent = extent,//Use same extent as was used to retrieve scales
            MaximumUserDefinedScale = max_scale,
            DestinationFolder = @"C:\Data\Offline"
          };
          //If DestinationFolder is not set, output defaults to project
          //offline maps location set in the project properties. If that is 
          //not set, output defaults to the current project folder location.

          //Do the export. Depending on the MaximumUserDefinedScale and the
          //area of the extent requested, this can take minutes for tile packages
          //over 1 GB or less if your network speed is slow...
          GenerateOfflineMap.Instance.ExportVectorTileCache(map, export_params);
        }
      });
      #endregion
    }
  }
}
