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
// Ignore Spelling: Popup

using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MapExploration.ProSnippets
{
  /// <summary>
  /// Provides utility methods for displaying pop-ups and custom pop-ups in the active map view.
  /// </summary>
  /// <remarks>This class includes methods to show standard pop-ups, custom pop-ups, and dynamic pop-ups with
  /// various configurations, such as window properties and custom commands. These methods rely on the active map view
  /// and will not perform any action if no active map view is available.</remarks>
  public static class ProSnippetsPopup
  {
    #region ProSnippet Group: Popups
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ShowPopup(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    #region Show a pop-up for a feature
    /// <summary>
    /// Displays a pop-up for a specific feature in the active map view.
    /// </summary>
    /// <param name="mapMember">The map member containing the feature for which the pop-up will be displayed. This cannot be <see langword="null"/>.</param>
    /// <param name="objectID">The object ID of the feature for which the pop-up will be displayed.</param>
    public static void ShowPopup(MapMember mapMember, long objectID)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      mapView.ShowPopup(mapMember, objectID);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.#ctor(System.String, System.String)
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup
    #region Show a custom pop-up
    /// <summary>
    /// Displays a custom pop-up with HTML content or a URI in the active map view.
    /// </summary>
    public static void ShowCustomPopup()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create custom popup content
      var popups = new List<PopupContent>
            {
                new PopupContent("<b>This text is bold.</b>", "Custom tooltip from HTML string"),
                new PopupContent(new Uri("http://www.esri.com/"), "Custom tooltip from Uri")
            };
      mapView.ShowCustomPopup(popups);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupDefinition
    // cref: ArcGIS.Desktop.Mapping.PopupDefinition.#ctor()
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowPopup(ArcGIS.Desktop.Mapping.MapMember, System.Int64, ArcGIS.Desktop.Mapping.PopupDefinition)
    #region Show a pop-up for a feature using pop-up window properties
    /// <summary>
    /// Displays a pop-up for a feature using custom pop-up window properties in the active map view.
    /// </summary>
    /// <param name="mapMember">The map member containing the feature.</param>
    /// <param name="objectID">The object ID of the feature.</param>
    public static void ShowPopupWithWindowDef(MapMember mapMember, long objectID)
    {
      if (MapView.Active == null) return;
      // Sample code: https://github.com/ArcGIS/arcgis-pro-sdk-community-samples/blob/master/Map-Exploration/CustomIdentify/CustomIdentify.cs
      var topLeftCornerPoint = new System.Windows.Point(200, 200);
      var popupDef = new PopupDefinition()
      {
        Append = true,      // if true new record is appended to existing (if any)
        Dockable = true,    // if true popup is dockable - if false Append is not applicable
        Position = topLeftCornerPoint,  // Position of top left corner of the popup (in pixels)
        Size = new System.Windows.Size(200, 400)    // size of the popup (in pixels)
      };
      MapView.Active.ShowPopup(mapMember, objectID, popupDef);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.#ctor(System.String, System.String)
    // cref: ArcGIS.Desktop.Mapping.PopupDefinition
    // cref: ArcGIS.Desktop.Mapping.PopupDefinition.#ctor()
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupContent>, System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupCommand>, System.Boolean, ArcGIS.Desktop.Mapping.PopupDefinition)
    #region Show a custom pop-up using pop-up window properties
    /// <summary>
    /// Displays a custom pop-up with window properties in the active map view.
    /// </summary>
    public static void ShowCustomPopupWithWindowDef()
    {
      if (MapView.Active == null) return;

      //Create custom popup content
      var popups = new List<PopupContent>
            {
                new PopupContent("<b>This text is bold.</b>", "Custom tooltip from HTML string"),
                new PopupContent(new Uri("http://www.esri.com/"), "Custom tooltip from Uri")
            };
      // Sample code: https://github.com/ArcGIS/arcgis-pro-sdk-community-samples/blob/master/Framework/DynamicMenu/DynamicFeatureSelectionMenu.cs
      var topLeftCornerPoint = new System.Windows.Point(200, 200);
      var popupDef = new PopupDefinition()
      {
        Append = true,      // if true new record is appended to existing (if any)
        Dockable = true,    // if true popup is dockable - if false Append is not applicable
        Position = topLeftCornerPoint,  // Position of top left corner of the popup (in pixels)
        Size = new System.Windows.Size(200, 400)    // size of the popup (in pixels)
      };
      MapView.Active.ShowCustomPopup(popups, null, true, popupDef);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.#ctor(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    // cref: ArcGIS.Desktop.Mapping.PopupCommand
    // cref: ArcGIS.Desktop.Mapping.PopupCommand.#ctor(System.Action{ArcGIS.Desktop.Mapping.PopupContent},System.Func{ArcGIS.Desktop.Mapping.PopupContent,System.Boolean},System.String,System.Windows.Media.ImageSource)
    // cref: ArcGIS.Desktop.Mapping.PopupCommand.Image
    // cref: ArcGIS.Desktop.Mapping.PopupCommand.Tooltip
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.PopupContent},System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.PopupCommand},System.Boolean)
    #region Show A pop-up With Custom Commands
    /// <summary>
    /// Displays a custom pop-up with custom commands for a specific feature in the active map view.
    /// </summary>
    /// <param name="mapMember">The map member containing the feature.</param>
    /// <param name="objectID">The object ID of the feature.</param>
    public static void ShowCustomPopup(MapMember mapMember, long objectID)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create custom popup content from existing map member and object id
      var popups = new List<PopupContent>();
      popups.Add(new PopupContent(mapMember, objectID));

      //Create a new custom command to add to the popup window
      var commands = new List<PopupCommand>();
      commands.Add(new PopupCommand(
        p => MessageBox.Show(string.Format("Map Member: {0}, ID: {1}", p.MapMember, p.IDString)),
        p => { return p != null; },
        "My custom command",
        System.Windows.Application.Current.Resources["GenericCheckMark16"] as ImageSource));

      mapView.ShowCustomPopup(popups, commands, true);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent.OnCreateHtmlContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.IsDynamicContent
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupContent>)
    #region Show A Dynamic Pop-up
    /// <summary>
    /// Displays a dynamic pop-up for a list of feature IDs in the active map view.
    /// </summary>
    /// <param name="mapMember">The map member containing the features.</param>
    /// <param name="objectIDs">A list of object IDs for which to display dynamic pop-ups.</param>
    public static void ShowDynamicPopup(MapMember mapMember, List<long> objectIDs)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create popup whose content is created the first time the item is requested.
      var popups = new List<PopupContent>();
      foreach (var id in objectIDs)
      {
        popups.Add(new DynamicPopupContent(mapMember, id));
      }

      mapView.ShowCustomPopup(popups);
    }

    internal class DynamicPopupContent : PopupContent
    {
      public DynamicPopupContent(MapMember mapMember, long objectID)
      {
        MapMember = mapMember;
        IDString = objectID.ToString();
        IsDynamicContent = true;
      }

      //Called when the pop-up is loaded in the window.
      protected override Task<string> OnCreateHtmlContent()
      {
        return QueuedTask.Run(() => string.Format("<b>Map Member: {0}, ID: {1}</b>", MapMember, IDString));
      }
    }
    #endregion

  }
}
