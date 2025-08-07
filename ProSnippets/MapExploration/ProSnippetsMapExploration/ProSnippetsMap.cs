using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
  /// <summary>
  /// Provides utility methods for interacting with the active map in ArcGIS Pro.
  /// </summary>
  /// <remarks>This class contains static methods to retrieve information about the active map, manipulate map
  /// selections, and manage map view overlays. It is designed to simplify  common tasks when working with maps in
  /// ArcGIS Pro.</remarks>
  public static class ProSnippetsMap
  {
    #region ProSnippet Group: Maps
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView
    // cref: ArcGIS.Desktop.Mapping.MapView.Active
    // cref: ArcGIS.Desktop.Mapping.MapView.Map
    // cref: ArcGIS.Desktop.Mapping.Map.Name
    #region Get the active map's name
    /// <summary>
    /// This method retrieves the name of the currently active map in ArcGIS Pro.
    /// </summary>
    /// <returns></returns>
    /// <remarks>This method demonstrates how to retrieve the name of the currently active map in ArcGIS Pro.</remarks>
    public static string GetActiveMapName()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return null;

      //Return the name of the map currently displayed in the active map view.
      return mapView.Map.Name;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.SetSelection(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
    #region Clear all selection in an Active map
    /// <summary>
    /// This method clears all selections in the active map.
    /// </summary>
    /// <remarks>This method demonstrates how to use the <see cref="ArcGIS.Desktop.Mapping.Map.SetSelection"/> method
    /// to clear all selections in the active map. If there is no active map, the method performs no action.</remarks>
    public static void ClearSelectionMap()
    {
      QueuedTask.Run(() =>
      {
        if (MapView.Active.Map != null)
        {
          MapView.Active.Map.SetSelection(null);
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SelectionEnvironment.SelectionTolerance
    // cref: ArcGIS.Desktop.Mapping.Map.GetDefaultExtent()
    // cref: ArcGIS.Desktop.Mapping.MapView.MapToScreen(ArcGIS.Core.Geometry.MapPoint)
    // cref: ArcGIS.Desktop.Mapping.MapView.ScreenToMap(System.Windows.Point)
    #region Calculate Selection tolerance in map units
    /// <summary>
    /// This method calculates the selection tolerance in map units based on the current map view's selection tolerance in pixels.
    /// </summary>
    /// <remarks>This method demonstrates how to convert the selection tolerance from pixels to map units by using the map's scale.</remarks>
    public static void CalculateSelectionTolerance()
    {
      //Selection tolerance for the map in pixels
      var selectionTolerance = SelectionEnvironment.SelectionTolerance;
      QueuedTask.Run(() =>
      {
        //Get the map center
        var mapExtent = MapView.Active.Map.GetDefaultExtent();
        var mapPoint = mapExtent.Center;
        //Map center as screen point
        var screenPoint = MapView.Active.MapToScreen(mapPoint);
        //Add selection tolerance pixels to get a "radius".
        var radiusScreenPoint = new System.Windows.Point((screenPoint.X + selectionTolerance), screenPoint.Y);
        var radiusMapPoint = MapView.Active.ScreenToMap(radiusScreenPoint);
        //Calculate the selection tolerance distance in map units.
        var searchRadius = GeometryEngine.Instance.Distance(mapPoint, radiusMapPoint);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapViewOverlayControl
    // cref: ArcGIS.Desktop.Mapping.MapViewOverlayControl.#ctor(System.Windows.FrameworkElement, System.Boolean, System.Boolean, System.Boolean, ArcGIS.Desktop.Mapping.OverlayControlRelativePosition, System.Double, System.Double)
    // cref: ArcGIS.Desktop.Mapping.MapView.AddOverlayControl(IMapViewOverlayControl)
    // cref: ArcGIS.Desktop.Mapping.MapView.RemoveOverlayControl(IMapViewOverlayControl)
    #region MapView Overlay Control
    /// <summary>
    /// This method demonstrates how to add a custom overlay control (a progress bar) to the active map view in ArcGIS Pro.
    /// </summary>
    /// <remarks>The overlay control is displayed for a short duration (3 seconds) before being removed from the map view.</remarks>
    public static async void AddMapViewOverlayControl()
    {
      //Create a Progress Bar user control
      var progressBarControl = new System.Windows.Controls.ProgressBar();
      //Configure the progress bar
      progressBarControl.Minimum = 0;
      progressBarControl.Maximum = 100;
      progressBarControl.IsIndeterminate = true;
      progressBarControl.Width = 300;
      progressBarControl.Value = 10;
      progressBarControl.Height = 25;
      progressBarControl.Visibility = System.Windows.Visibility.Visible;
      //Create a MapViewOverlayControl. 
      var mapViewOverlayControl = new MapViewOverlayControl(progressBarControl, true, true, true, OverlayControlRelativePosition.BottomCenter, .5, .8);
      //Add to the active map
      MapView.Active.AddOverlayControl(mapViewOverlayControl);
      await QueuedTask.Run(() =>
      {
        //Wait 3 seconds to remove the progress bar from the map.
        Thread.Sleep(3000);

      });
      //Remove from active map
      MapView.Active.RemoveOverlayControl(mapViewOverlayControl);
    }
    #endregion
  }
}
