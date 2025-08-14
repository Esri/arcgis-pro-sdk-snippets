using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
  /// <summary>
  /// Provides utility methods for manipulating and managing time in the active map view.
  /// </summary>
  /// <remarks>This class includes methods to step the map's time forward by a specified interval and to disable
  /// time in the map. These methods operate on the active <see cref="ArcGIS.Desktop.Mapping.MapView"/> and require an
  /// active map view to function.</remarks>
  public static class ProSnippetsTime
  {
    #region ProSnippet Group: Time
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TimeDelta
    // cref: ArcGIS.Desktop.Mapping.TimeDelta.#ctor(System.Double, ArcGIS.Desktop.Mapping.TimeUnit)
    // cref: ArcGIS.Desktop.Mapping.TimeUnit
    // cref: ArcGIS.Desktop.Mapping.MapView.Time
    // cref: ArcGIS.Desktop.Mapping.TimeRange
    // cref: ArcGIS.Desktop.Mapping.TimeRange.Offset(ArcGIS.Desktop.Mapping.TimeDelta)
    #region Step forward in time by 1 month
    /// <summary>
    /// This method steps the current map time forward by one month.
    /// </summary>
    /// <remarks>This method retrieves the active map view and advances its time by one month using a <see cref="ArcGIS.Desktop.Mapping.TimeDelta"/>.</remarks>
    public static void StepMapTime()
    {
      //Get the active view
      MapView mapView = MapView.Active;
      if (mapView == null)
        return;

      //Step current map time forward by 1 month
      TimeDelta timeDelta = new TimeDelta(1, TimeUnit.Months);
      mapView.Time = mapView.Time.Offset(timeDelta);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.Time
    // cref: ArcGIS.Desktop.Mapping.TimeRange.Start
    // cref: ArcGIS.Desktop.Mapping.TimeRange.End
    #region  Disable time in the map. 
    /// <summary>
    /// This method disables time in the active map view by setting the start and end times to null.
    /// </summary>
    /// <remarks>This method demonstrates how to disable time in the active map view.</remarks>
    public static void DisableTime()
    {
      MapView.Active.Time.Start = null;
      MapView.Active.Time.End = null;
    }
    #endregion
  }
}
