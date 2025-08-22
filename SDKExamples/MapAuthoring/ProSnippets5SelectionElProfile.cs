/*

   Copyright 2025 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Mapping.Symbology;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippets5SelectionElProfile
  {
    #region ProSnippet Group: SelectionSet
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary``1(System.Collections.Generic.Dictionary{``0,System.Collections.Generic.List{System.Int64}})
    #region Translate From Dictionary to SelectionSet
    /// <summary>
    /// Creates a selection set from a dictionary of map members and their associated object IDs.
    /// </summary>
    /// <param name="us_zips_layer">The <see cref="MapMember"/> representing the layer to which the object IDs belong.</param>
    public static void TranslateDictionaryToSelectionSet(MapMember us_zips_layer)
    {
      //Create a selection set from a list of object ids
      //using FromDictionary
      var addToSelection = new Dictionary<MapMember, List<long>>();
      addToSelection.Add(us_zips_layer, new List<long> { 1506, 2696, 2246, 1647, 948 });
      //Create a SelectionSet object
      var selSet = ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary(addToSelection);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1
    #region Translate from SelectionSet to Dictionary
    /// <summary>
    /// Converts the specified <see cref="SelectionSet"/> into a dictionary representation.
    /// </summary>
    /// <param name="selectionSet">The <see cref="SelectionSet"/> to be converted. Must not be <see langword="null"/>.</param>
    public static void TranslateSelectionSetToDictionary(SelectionSet selectionSet)
    {
      var selSetDict = selectionSet.ToDictionary();

      // convert to the dictionary and only include those that are of type FeatureLayer
      var selSetDictFeatureLayer = selectionSet.ToDictionary<FeatureLayer>();
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.Contains(ArcIGS.Desktop.Mapping.MapMember)
    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.Item(ArcIGS.Desktop.Mapping.MapMember)
    #region Get OIDS from a SelectionSet for a given MapMember
    /// <summary>
    /// Retrieves the object IDs (OIDs) from the specified <see cref="SelectionSet"/> for the given <see
    /// cref="MapMember"/>.
    /// </summary>
    /// <param name="selSet">The selection set containing the selected features.</param>
    /// <param name="us_zips_layer">The map member for which the object IDs are to be retrieved.</param>
    public static void GetOidsFromSelectionSet(SelectionSet selSet, MapMember us_zips_layer)
    {
      if (selSet.Contains(us_zips_layer))
      {
        var oids = selSet[us_zips_layer];
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1
    #region Get OIDS from a SelectionSet for a given MapMember by Name
    /// <summary>
    /// Retrieves the object IDs (OIDs) from a <see cref="SelectionSet"/> for a map layer with the specified name.
    /// </summary>
    /// <param name="selSet">The <see cref="SelectionSet"/> containing the selected features. Must not be <see langword="null"/>.</param>
    public static void GetOidsFromSelectionSetByName(SelectionSet selSet)
    {
      var kvp = selSet.ToDictionary().Where(kvp => kvp.Key.Name == "LayerName").FirstOrDefault();
      var oidList = kvp.Value;
    }
    #endregion


    #region ProSnippet Group: Selection Options
    #endregion
    // cref: ArcGIS.Desktop.Core.ApplicationOptions.SelectionOptions
    // cref: ArcGIS.Desktop.Core.SelectionOptions
    // cref:ArcGIS.Desktop.Mapping.SelectionMethod
    // cref:ArcGIS.Desktop.Mapping.SelectionCombinationMethod
    #region Get/Set Selection Options
    /// <summary>
    /// Configures and retrieves various selection options for the application.
    /// </summary>
    public static void GetSetSelectionOptions()
    {
      var options = ApplicationOptions.SelectionOptions;

      QueuedTask.Run(() =>
      {
        var defaultColor = options.DefaultSelectionColor;
        //Set the selection color to red
        var color = options.SelectionColor as CIMRGBColor;
        options.SetSelectionColor(ColorFactory.Instance.CreateRGBColor(255, 0, 0));

        //Set the selection fill color and fill hatch
        var defaultFill = options.DefaultSelectionFillColor;
        var fill = options.SelectionFillColor;
        var isHatched = options.IsSelectionFillHatched;
        options.SetSelectionFillColor(ColorFactory.Instance.CreateRGBColor(100, 100, 0));
        if (!isHatched)
          options.SetSelectionFillIsHatched(true);
        //Toggle the selection Chip and Graphic
        var showSelectionChip = options.ShowSelectionChip;
        options.SetShowSelectionChip(!showSelectionChip);

        var showSelectionGraphic = options.ShowSelectionGraphic;
        options.SetShowSelectionGraphic(!showSelectionGraphic);

        //Get the value indicating whether to save the layer and standalone table selection with the map.
        var saveSelection = options.SaveSelection;
        options.SetSaveSelection(!saveSelection);
        //Get/Set the selection tolerance
        var defaultTol = options.DefaultSelectionTolerance;
        var tol = options.SelectionTolerance;
        options.SetSelectionTolerance(2 * defaultTol);

        // extension methods available for selection methods and combination methods
        var selMethod = options.SelectionMethod;
        options.SetSelectionMethod(SelectionMethod.Contains);

        var combMethod = options.CombinationMethod;
        options.SetCombinationMethod(SelectionCombinationMethod.Add);

        // note that the following SelectionCombinationMethod is not supported
        //options.SetCombinationMethod(SelectionCombinationMethod.XOR);
      });
    }
    #endregion



    #region ProSnippet Group: Elevation Profile
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline})
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint})
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline})
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint})
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
    #region Get Elevation profile from the default ground surface
    /// <summary>
    /// Retrieves elevation profile data for a polyline or a collection of map points from the default ground surface.
    /// </summary>  
    /// <param name="lineGeom">A <see cref="Polyline"/> representing the path for which the elevation profile is to be calculated. Must be a
    /// valid polyline geometry.</param>
    /// <param name="mapPoints">An <see cref="IEnumerable{MapPoint}"/> representing a collection of points for which the elevation profile is to
    /// be calculated. Each point must be valid and located within the map's spatial reference.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The resulting elevation profile is returned as a
    /// polyline with Z-values.</returns>
    public static async Task GetElevationProfile(Polyline lineGeom, IEnumerable<MapPoint> mapPoints)
    {
      // get the elevation profile for a polyline / set of polylines
      var result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync([lineGeom]);
      if (result.Status == SurfaceZsResultStatus.Ok)
      {
        var polylineZ = result.Polyline;

        // process the polylineZ
      }

      // get the elevation profile for a set of points
      result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(mapPoints);
      if (result.Status == SurfaceZsResultStatus.Ok)
      {
        var polylineZ = result.Polyline;

        // process the polylineZ
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
    #region Get Elevation profile from a specific surface
    /// <summary>
    /// Generates elevation profiles for a specified elevation surface layer using either a polyline geometry or a
    /// collection of map points.
    /// </summary> 
    /// <param name="eleLayer">The <see cref="ElevationSurfaceLayer"/> from which elevation profiles will be generated. This layer must be
    /// valid and accessible.</param>
    /// <param name="lineGeom">A <see cref="Polyline"/> geometry representing the path for which the elevation profile will be calculated.</param>
    /// <param name="mapPoints">A collection of <see cref="MapPoint"/> objects representing individual points for which the elevation profile
    /// will be calculated.</param>
    /// <returns></returns>
    public static async Task GetElevationProfileFromSpecificSurface(ElevationSurfaceLayer eleLayer, Polyline lineGeom, IEnumerable<MapPoint> mapPoints)
    {

      // get the elevation profile for a polyline / set of polylines
      // use the specific elevation surface layer
      var zResult = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync([lineGeom], eleLayer);
      if (zResult.Status == SurfaceZsResultStatus.Ok)
      {
        var polylineZ = zResult.Polyline;

        // process the polylineZ
      }

      // get the elevation profile for a set of points
      // use the specific elevation surface layer
      zResult = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(mapPoints, eleLayer);
      if (zResult.Status == SurfaceZsResultStatus.Ok)
      {
        var polylineZ = zResult.Polyline;

        // process the polylineZ
      }
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
    #region Interpolate a line between two points and calculate the elevation profile 
    /// <summary>
    /// Interpolates a line between two points and calculates the elevation profile along the line.
    /// </summary>  
    /// <param name="eleLayer">The elevation surface layer to use for calculating the elevation profile. If null, the default ground elevation
    /// surface is used.</param>
    /// <param name="startPt">The starting point of the line. Must be a valid <see cref="MapPoint"/>.</param>
    /// <param name="endPt">The ending point of the line. Must be a valid <see cref="MapPoint"/>.</param>
    /// <returns></returns>
    public static async Task InterpolateLineBetweenTwoPointsAndCalculateElProfile(ElevationSurfaceLayer eleLayer, MapPoint startPt, MapPoint endPt)
    {
      int numPoints = 20;//Or any number of points you want to interpolate

      // use the default ground elevation surface
      var result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(startPt, endPt, numPoints);
      if (result.Status == SurfaceZsResultStatus.Ok)
      {
        var polylineZ = result.Polyline;

        // process the polylineZ
      }
      // use a specific elevation surface
      result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(startPt, endPt, numPoints, eleLayer);
      if (result.Status == SurfaceZsResultStatus.Ok)
      {
        var polylineZ = result.Polyline;

        // process the polylineZ
      }
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
    #region Show Elevation profile graph with the default ground surface
    /// <summary>
    /// Displays the elevation profile graph for the specified polyline or collection of map points using the default
    /// ground surface layer.
    /// </summary>
    /// <param name="lineGeom">A <see cref="Polyline"/> representing the geometry for which the elevation profile graph will be displayed.</param>
    /// <param name="pts">A collection of <see cref="MapPoint"/> objects representing the points for which the elevation profile graph
    /// will be displayed.</param>
    public static void ShowElevationProfileGraph(Polyline lineGeom, IEnumerable<MapPoint> pts)
    {
      if (!MapView.Active.CanShowElevationProfileGraph())
        return;

      // show the elevation profile for a polyline 
      // use the default ground surface layer
      MapView.Active.ShowElevationProfileGraph([lineGeom]);

      // show the elevation profile for a set of points
      // use the default ground surface layer
      MapView.Active.ShowElevationProfileGraph(pts);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.SurfaceLayer
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.Densify
    #region Show Elevation profile graph with a specific surface
    /// <summary>
    /// Displays an elevation profile graph for a specified surface layer using either a polyline geometry or a
    /// collection of map points.
    /// </summary>  
    /// <param name="eleLayer">The elevation surface layer to use for generating the elevation profile.</param>
    /// <param name="lineGeom">The polyline geometry representing the path for which the elevation profile will be generated.</param>
    /// <param name="pts">A collection of map points representing the locations for which the elevation profile will be generated.</param>
    public static void ShowElevationProfileGraphWithSpecificSurface(ElevationSurfaceLayer eleLayer, Polyline lineGeom, IEnumerable<MapPoint> pts)
    {
      if (!MapView.Active.CanShowElevationProfileGraph())
        return;

      // set up the parameters
      var profileParams = new ElevationProfileParameters();
      profileParams.SurfaceLayer = eleLayer;
      profileParams.Densify = true;

      // show the elevation profile for a polyline using the params
      MapView.Active.ShowElevationProfileGraph([lineGeom], profileParams);

      // show the elevation profile for a set of points using the params
      MapView.Active.ShowElevationProfileGraph(pts, profileParams);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationProfileParameters)
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.SurfaceLayer
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.Densify
    #region Show Elevation profile graph between two points 
    /// <summary>
    /// Displays an elevation profile graph between two specified points on the map.
    /// </summary>
    /// <param name="eleLayer">The elevation surface layer to use for the profile graph. This parameter is optional and can be null.</param>
    /// <param name="startPt">The starting point of the elevation profile graph. Must be a valid <see cref="MapPoint"/>.</param>
    /// <param name="endPt">The ending point of the elevation profile graph. Must be a valid <see cref="MapPoint"/>.</param>
    public static void ShowElevationProfileGraphBetweenTwoPoints(ElevationSurfaceLayer eleLayer, MapPoint startPt, MapPoint endPt)
    {
      int numPoints = 20;

      if (!MapView.Active.CanShowElevationProfileGraph())
        return;

      // show the elevation profile 
      // use the default ground elevation surface
      MapView.Active.ShowElevationProfileGraph(startPt, endPt, numPoints);

      // find a specific elevation surface layer
      var tinLayer = MapView.Active.Map.GetElevationSurfaceLayers().FirstOrDefault(l => l.Name == "TIN");

    // set up the params
    var elevProfileParams = new ElevationProfileParameters();
    elevProfileParams.SurfaceLayer = tinLayer;
      elevProfileParams.Densify = false;

      // show the elevation profile using the params
      MapView.Active.ShowElevationProfileGraph(startPt, endPt, numPoints, elevProfileParams);
      }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
    // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
    // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(ArcGIS.Desktop.Mapping.ElevationProfileResult)
    #region Show Elevation profile graph using an ElevationProfileResult
    /// <summary>
    /// Displays an elevation profile graph for the specified start and end points using elevation data from the map
    /// surface.
    /// </summary>
    /// <param name="startPt">The starting point of the elevation profile, specified as a <see cref="MapPoint"/>.</param>
    /// <param name="endPt">The ending point of the elevation profile, specified as a <see cref="MapPoint"/>.</param>
    /// <returns></returns>
    public static async Task ShowElevationProfileGraphUsingElevationProfileResult(MapPoint startPt, MapPoint endPt)
    {

      var elevProfileResult = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(startPt, endPt, 10);
      if (elevProfileResult.Status != SurfaceZsResultStatus.Ok)
        return;

      if (!MapView.Active.CanShowElevationProfileGraph())
        return;

      // show the elevation profile using the result
      MapView.Active.ShowElevationProfileGraph(elevProfileResult);
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ElevationProfileGraphAdded
    // cref: ArcGIS.Desktop.Mapping.MapView.ElevationProfileGraphRemoved
    // cref: ArcGIS.Desktop.Mapping.MapView.GetElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ContentLoaded
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.Geometry
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ElevationProfileStatistics
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileStatistics
    #region Access the ElevationProfileGraph when added
    /// <summary>
    /// Subscribes to the <see cref="MapView.ElevationProfileGraphAdded"/> and  <see
    /// cref="MapView.ElevationProfileGraphRemoved"/> events for the specified <see cref="MapView"/>.
    /// </summary>
    /// <param name="mapView">The <see cref="MapView"/> instance to subscribe to elevation profile graph events.</param>
    public static void SubscribeToAddedRemovedEventsElevationProfileGraph(MapView mapView)
    {
      // subscribe to the Added, Removed events for the elevation profile graph
      mapView.ElevationProfileGraphAdded += Mv_ElevationProfileGraphAdded;
      mapView.ElevationProfileGraphRemoved += Mv_ElevationProfileGraphRemoved;
    }
    /// <summary>
    /// Handles the event triggered when an elevation profile graph is removed.
    /// </summary>
    /// <param name="sender">The source of the event, typically the object that raised the event.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    public static void Mv_ElevationProfileGraphRemoved(object sender, EventArgs e)
    {
      //TODO: handle the removal of the elevation profile graph
    }
    /// <summary>
    /// Handles the event triggered when an elevation profile graph is added to the active map view.
    /// </summary>
    /// <param name="sender">The source of the event. Typically, the object that raised the event.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    public static void Mv_ElevationProfileGraphAdded(object sender, EventArgs e)
    {
      // get the elevation profile graph from the view
      // this will be non-null since we are in a ElevationProfileGraphAdded handler
      var mv = MapView.Active;
      var graph = mv.GetElevationProfileGraph();

      // subscribe to the ContentLoaded event
      graph.ContentLoaded += Graph_ContentLoaded;
    }
    /// <summary>
    /// Handles the event triggered when the elevation profile graph content is loaded.
    /// </summary>
    /// <param name="sender">The source of the event, expected to be an instance of <see
    /// cref="ArcGIS.Desktop.Mapping.ElevationProfileGraph"/>.</param>
    /// <param name="e">An <see cref="EventArgs"/> object containing no event data.</param>
    public static void Graph_ContentLoaded(object sender, EventArgs e)
    {
      // get the elevation profile graph
      var graph = sender as ArcGIS.Desktop.Mapping.ElevationProfileGraph;

      // get the elevation profile geometry
      var polyline = graph.Geometry;
      // get the elevation profile statistics
      var stats = graph.ElevationProfileStatistics;
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapView.GetElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.Geometry
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ElevationProfileStatistics
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileStatistics
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.IsReversed
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.IsExpanded
    #region Access the ElevationProfileGraph
    /// <summary>
    /// Accesses the elevation profile graph associated with the active map view and performs various operations on it.
    /// </summary>
  
    public static void AccessElevationProfileGraph()
    {

      var elevProfileGraph = MapView.Active.GetElevationProfileGraph();
      // Elevation profile graph will be null if no profile graph is displayed
      if (elevProfileGraph == null)
        return;

      // get the elevation profile geometry and stats
      var polyline = elevProfileGraph.Geometry;
      var stats = elevProfileGraph.ElevationProfileStatistics;

      // reverse the graph
      elevProfileGraph.IsReversed = !elevProfileGraph.IsReversed;

      // collapse the graph
      elevProfileGraph.IsExpanded = false;
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapView.GetElevationProfileGraph
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.CanExport
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ExportToImage(System.String)
    // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ExportToCSV(System.String)
    #region Export Elevation Profile Graph
    /// <summary>
    /// Exports the currently displayed elevation profile graph to an image and a CSV file.
    /// </summary>
    public static void ExportElevationProfileGraph()
    {

      var elevProfileGraph = MapView.Active.GetElevationProfileGraph();
      // Elevation profile graph will be null if no profile graph is displayed
      if (elevProfileGraph == null)
        return;

      if (elevProfileGraph.CanExport)
      {
        elevProfileGraph.ExportToImage("c:\\temp\\myprofileImage.png");
        elevProfileGraph.ExportToCSV("c:\\temp\\myprofile.csv");
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ElevationProfileOptions
    // cref: ArcGIS.Desktop.Core.ExploratoryAnalysisOptions.CanSetElevationProfileOptions(ArcGIS.Desktop.Core.ElevationProfileOptions)
    // cref: ArcGIS.Desktop.Core.ExploratoryAnalysisOptions.SetElevationProfileOptionsAsync(ArcGIS.Desktop.Core.ElevationProfileOptions)
    // cref: ArcGIS.Desktop.Core.ExploratoryAnalysisOptions.GetDefaultElevationProfileOptions()
    #region Customize Elevation Profile Graph Display
    /// <summary>
    /// Customizes the display options for the elevation profile graph in exploratory analysis.
    /// </summary>
    public static async Task CustomizeElevationProfileGraphDisplay()
    {
      // customize the elevation profile graph options
      var options = new ElevationProfileOptions()
      {
        LineColor = ColorFactory.Instance.CreateRGBColor(0, 0, 100),
        ShowAverageSlope = false,
        ShowMaximumSlope = false
      };

      var eaOptions = ApplicationOptions.ExploratoryAnalysisOptions;
      if (eaOptions.CanSetElevationProfileOptions(options))
        await eaOptions.SetElevationProfileOptionsAsync(options);


      // or reset to default options
      var defaultOptions = eaOptions.GetDefaultElevationProfileOptions();
      if (eaOptions.CanSetElevationProfileOptions(defaultOptions))
        await eaOptions.SetElevationProfileOptionsAsync(defaultOptions);
    }
    #endregion
  }
}
