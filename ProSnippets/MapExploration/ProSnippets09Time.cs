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

    #region Step forward in time by 1 month
    // cref: ArcGIS.Desktop.Mapping.TimeDelta
    // cref: ArcGIS.Desktop.Mapping.TimeDelta.#ctor(System.Double, ArcGIS.Desktop.Mapping.TimeUnit)
    // cref: ArcGIS.Desktop.Mapping.TimeUnit
    // cref: ArcGIS.Desktop.Mapping.MapView.Time
    // cref: ArcGIS.Desktop.Mapping.TimeRange
    // cref: ArcGIS.Desktop.Mapping.TimeRange.Offset(ArcGIS.Desktop.Mapping.TimeDelta)
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

    #region  Disable time in the map. 
    // cref: ArcGIS.Desktop.Mapping.MapView.Time
    // cref: ArcGIS.Desktop.Mapping.TimeRange.Start
    // cref: ArcGIS.Desktop.Mapping.TimeRange.End
    public static void DisableTime()
    {
      MapView.Active.Time.Start = null;
      MapView.Active.Time.End = null;
    }
    #endregion
  }
}
