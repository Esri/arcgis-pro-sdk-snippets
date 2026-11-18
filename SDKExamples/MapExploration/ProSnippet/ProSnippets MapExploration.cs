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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace ProSnippets.MapExplorationSnippets
{
  /// <summary>
  /// Provides a collection of methods for exploring and interacting with map views in ArcGIS Pro.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with creating text symbols in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsMapExploration
  {
    /// <summary>
    /// Demonstrates various map exploration functionalities in ArcGIS Pro, including zooming, panning, and managing
    /// bookmarks.
    /// </summary>
    /// <remarks>This method provides code snippets for interacting with the map view in ArcGIS Pro. It
    /// includes examples of how to: <list type="bullet"> <item><description>Find and activate a map view by its
    /// caption.</description></item> <item><description>Check and set the viewing mode of a map
    /// view.</description></item> <item><description>Export scene contents to a 3D format.</description></item>
    /// <item><description>Navigate through camera positions and zoom to various extents.</description></item>
    /// <item><description>Manage bookmarks, including creating, renaming, and removing them.</description></item>
    /// <item><description>Interact with table views, such as setting view modes and managing
    /// selections.</description></item> <item><description>Display pop-ups and manage overlays on the map
    /// view.</description></item> </list> The method is intended for use within the ArcGIS Pro SDK environment and
    /// requires an active map view and table view.</remarks>
    public static async Task MapExplorationProSnippetsAsync()
    {
      #region ignore - Variable initialization
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
      {
        return;
      }

      double xMin = 0.0,
             yMin = 0.0,
             xMax = 0.0,
             yMax = 0.0,
             dx = 0.0,
             dy = 0.0,
             x = 0.0,
             y = 0.0,
             buffer_size = 0.0;
      double heading = 0.0;
      ArcGIS.Core.Geometry.SpatialReference spatialReference = SpatialReferences.WebMercator;
      string bookmarkName = "MyBookmark";

      IEnumerable<long> objectIDs = [];
      FeatureLayer featureLayer = mapView.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      Layer layer = mapView.Map.Layers.FirstOrDefault();

      int zoomLevel = 0;
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
      {
        return;
      }

      MapMember mapMember = null;
      long objectID = 0;

      Camera camera = new();
      string cameraName = "MyCamera";
      Bookmark oldBookmark = null;
      string newBookmarkName = "MyNewBookmark";

      string imagePath = @"C:\Data\Images\MyImage.png";
      ArcGIS.Core.Geometry.Envelope envelope = EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);

      TimeSpan length = TimeSpan.FromSeconds(2);
      TimeSpan afterTime = TimeSpan.FromSeconds(1);
      TimeSpan atTime = TimeSpan.FromSeconds(3);
      ArcGIS.Desktop.Mapping.Range range = new();

      double transparency = 0.5;

      System.Windows.Point clientPoint = new(0, 0);
      ArcGIS.Core.Geometry.Geometry geometry = null;
      CIMLineSymbol _lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2.0, SimpleLineStyle.Solid);
      #endregion Variable initialization

      #region ProSnippet Group: Map View
      #endregion

      //cref: ArcGIS.Desktop.Mapping.MapView.CurrentMapTOCContent
      //cref: ArcGIS.Desktop.Mapping.MapTOCContentType
      //cref: ArcGIS.Desktop.Mapping.MapView.Active
      #region Get the Active Map TOC content type
      {

        //Get the current active TOC content type for the map view
        var mapTOCContentType = MapView.Active.CurrentMapTOCContent;

      }
      #endregion

      //cref: ArcGIS.Desktop.Mapping.MapView.CurrentMapTOCContent
      //cref: ArcGIS.Desktop.Mapping.MapView.CanSetMapTOCContent(ArcGIS.Desktop.Mapping.MapTOCContentType)
      //cref: ArcGIS.Desktop.Mapping.MapView.SetMapTOCContentAsync(ArcGIS.Desktop.Mapping.MapTOCContentType)
      //cref: ArcGIS.Desktop.Mapping.MapTOCContentType
      //cref: ArcGIS.Desktop.Mapping.MapView.Active
      #region Set the Active Map TOC content type
      {

        //Get the current active TOC content type for the map view
        var mapTOCContentType = (int)MapView.Active.CurrentMapTOCContent;
        //increment to the next tab whatever it is
        mapTOCContentType++;
        //Can we set this type on the TOC?
        if (MapView.Active.CanSetMapTOCContent((MapTOCContentType)mapTOCContentType))
        {
          //Set it - must be on the UI! - No QueuedTask
          MapView.Active.SetMapTOCContentAsync((MapTOCContentType)mapTOCContentType);
        }
        else
        {
          mapTOCContentType = (int)MapTOCContentType.DrawingOrder;
          //Set it - must be on the UI! - No QueuedTask
          MapView.Active.SetMapTOCContentAsync((MapTOCContentType)mapTOCContentType);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Activate()
      // cref: ArcGIS.Desktop.Mapping.IMapPane
      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
      // cref: ArcGIS.Desktop.Mapping.IMapPane.Caption
      // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers()
      #region Find a MapView by its Caption
      {
        string mapPaneCaption = "USNationalParks";
        IMapPane mapViewPane = FrameworkApplication.Panes.OfType<IMapPane>().FirstOrDefault((p) => p.Caption == mapPaneCaption);
        mapView = null;
        if (mapViewPane != null)
        {
          // activate the MapPane
          (mapViewPane as Pane).Activate();

          if (mapView != null)
          {
            // get the layers selected in the map's TOC
            var selectedLayers = mapView.GetSelectedLayers();
          }
        }
      }
      #endregion Find a MapView by its Caption

      // cref: ArcGIS.Desktop.Mapping.MapView.ViewingMode
      // cref: ArcGIS.Core.CIM.MapViewingMode
      #region Test if the view is 3D
      {
        //Determine whether the viewing mode is SceneLocal or SceneGlobal
        var result = mapView.ViewingMode == MapViewingMode.SceneLocal ||
                     mapView.ViewingMode == MapViewingMode.SceneGlobal;

        // Use the result variable as needed
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.CanSetViewingMode(ArcGIS.Core.CIM.MapViewingMode)
      // cref: ArcGIS.Desktop.Mapping.MapView.SetViewingModeAsync(ArcGIS.Core.CIM.MapViewingMode)
      #region Set ViewingMode
      {
        //Check if the view can be set to SceneLocal and if it can set it.
        var result = mapView.CanSetViewingMode(MapViewingMode.SceneLocal);
        if (result)
        {
          mapView.SetViewingModeAsync(MapViewingMode.SceneLocal);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.LinkMode
      // cref: ArcGIS.Desktop.Mapping.LinkMode
      #region Enable View Linking
      {
        //Set the view linking mode to Center and Scale.
        MapView.LinkMode = LinkMode.Center | LinkMode.Scale;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ExportScene3DObjects(ArcGIS.Desktop.Mapping.ExportSceneContentsFormat)
      // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat
      // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat.Extent
      // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat.FileName
      // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat.FolderPath
      // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat.IsSingleFileOutput
      // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat.SaveTextures
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      // cref: ArcGIS.Desktop.Mapping.MapView.Extent
      // cref: ArcGIS.Desktop.Mapping.MapView.ViewingMode
      #region Export the contents of a scene to an exchange format such as glTF and STL.
      {
        // Validate the current active view. Only a local scene can be exported.
        bool CanExportScene3DObjects = MapView.Active?.ViewingMode == MapViewingMode.SceneLocal;
        if (CanExportScene3DObjects)
        {
          // Create a scene content export format, export the scene context as a glTF file
          var exportFormat = new ExportSceneContentsFormat()
          {
            Extent = mapView.Extent, // sets Extent property
            FolderPath = @"C:\Temp", // sets FolderPath property
                                     //sets FileName property.The export format is determined by the output file extension (e.g.,.stl, .gltf)
            FileName = "my-3d-objects.gltf",
            IsSingleFileOutput = true, // sets whether to export to one single file
            SaveTextures = true //sets whether to save textures
          };
          // Export the scene content as 3D objects
          mapView.ExportScene3DObjects(exportFormat);
        }
      }
      #endregion

      #region ProSnippet Group: Update MapView Extent (Zoom, Pan etc)
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.HasPreviousCamera()
      // cref: ArcGIS.Desktop.Mapping.MapView.PreviousCameraAsync(System.Nullable{System.TimeSpan})
      #region Go To Previous Camera
      {
        //Zoom to the selected layers in the TOC
        if (mapView.HasPreviousCamera())
        {
          mapView.PreviousCameraAsync();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.HasNextCamera()
      // cref: ArcGIS.Desktop.Mapping.MapView.NextCameraAsync(System.Nullable{System.TimeSpan})
      #region Go To Next Camera
      {
        //Zoom to the selected layers in the TOC
        if (mapView.HasNextCamera())
        {
          mapView.NextCameraAsync();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtent(System.Nullable{System.TimeSpan})
      #region Zoom To Full Extent
      {
        await QueuedTask.Run(() =>
        {
          //Zoom to the map's full extent
          mapView.ZoomToFullExtent();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtentAsync(System.Nullable{System.TimeSpan})
      #region Zoom To Full Extent Async
      {
        //Zoom to the map's full extent
        mapView.ZoomToFullExtentAsync(TimeSpan.FromSeconds(2));
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixed(System.Nullable{System.TimeSpan})
      #region Fixed Zoom In
      {
        await QueuedTask.Run(() =>
        {
          //Zoom in the map view by a fixed amount.
          mapView.ZoomInFixed();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixedAsync(System.Nullable{System.TimeSpan})
      #region Fixed Zoom In Async
      {
        //Zoom in the map view by a fixed amount.
        mapView.ZoomInFixedAsync();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixed(System.Nullable{System.TimeSpan})
      #region Fixed Zoom Out
      {
        await QueuedTask.Run(() =>
        {
          //Zoom out in the map view by a fixed amount.
          mapView.ZoomOutFixed();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixedAsync(System.Nullable{System.TimeSpan})
      #region Fixed Zoom Out Async
      {
        //Zoom in the map view by a fixed amount.
        mapView.ZoomOutFixedAsync();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
      #region Zoom To an Extent
      {
        envelope = EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);

        //Zoom the view to a given extent.
        mapView.ZoomToAsync(envelope, TimeSpan.FromSeconds(2));
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.SpatialReference
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Zoom To a Point
      {
        await QueuedTask.Run(() =>
        {
          //Create a point
          var pt = MapPointBuilderEx.CreateMapPoint(x, y, spatialReference);
          //Buffer it - for purpose of zoom
          var poly = GeometryEngine.Instance.Buffer(pt, buffer_size);

          //do we need to project the buffer polygon?
          if (!mapView.Map.SpatialReference.IsEqual(poly.SpatialReference))
          {
            //project the polygon
            poly = GeometryEngine.Instance.Project(poly, mapView.Map.SpatialReference);
          }

          //Zoom - add in a delay for animation effect
          mapView.ZoomTo(poly, new TimeSpan(0, 0, 0, 3));
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelected(System.Nullable{System.TimeSpan},System.Boolean)
      #region Zoom To Selected Features with a timespan
      {
        await QueuedTask.Run(() =>
        {
          //Zoom to the map's selected features.
          mapView.ZoomToSelected(TimeSpan.FromSeconds(3));
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelectedAsync(System.Nullable{System.TimeSpan},System.Boolean)
      #region Zoom Async To Selected Features with a timespan
      {
        //Zoom to the map's selected features.
        mapView.ZoomToSelectedAsync(TimeSpan.FromSeconds(2));
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.Bookmark
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
      // cref: ArcGIS.Desktop.Mapping.Bookmark.Name
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Zoom To Bookmark by name
      {
        await QueuedTask.Run(() =>
        {
          //Get the first bookmark with the given name.
          var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
          if (bookmark == null)
          {
            // Manage the error - bookmark not found
          }

          //Zoom the view to the bookmark.
          mapView.ZoomTo(bookmark);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.Bookmark
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
      // cref: ArcGIS.Desktop.Mapping.Bookmark.Name
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Zoom Async To Bookmark by name
      {
        await QueuedTask.Run(() =>
        {
          //Get the first bookmark with the given name.
          var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
          if (bookmark == null)
          {
            // Manage the error - bookmark not found
          }

          //Zoom the view to the bookmark.
          mapView.ZoomToAsync(bookmark, TimeSpan.FromSeconds(2));
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.Layer.IsVisible
      // cref: ArcGIS.Desktop.Mapping.Map.Layers
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Zoom To Visible Layers
      {
        await QueuedTask.Run(() =>
        {
          //Zoom to all visible layers in the map.
          var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
          mapView.ZoomTo(visibleLayers);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers()
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
      #region Zoom To Selected Layers
      {
        //Zoom to the selected layers in the TOC
        var selectedLayers = mapView.GetSelectedLayers();
        mapView.ZoomToAsync(selectedLayers);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.BasicFeatureLayer,System.Collections.Generic.IEnumerable{System.Int64},System.Nullable{System.TimeSpan},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.Search(ArcGIS.Core.Data.QueryFilter,ArcGIS.Desktop.Mapping.TimeRange,ArcGIS.Desktop.Mapping.RangeExtent,ArcGIS.Core.CIM.CIMFloorFilterSettings)
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      #region Zoom to ObjectIDs
      {
        using var rowCursor = featureLayer.Search();
        var objectIds = new List<long>();
        while (rowCursor.MoveNext())
        {
          using var feature = rowCursor.Current as Feature;
          objectIds.Add(feature.GetObjectID());
        }
        if (objectIds.Count > 0)
        {
          MapView.Active.ZoomTo(featureLayer, objectIds, TimeSpan.FromSeconds(2));
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
      #region Pan To an Extent
      {
        await QueuedTask.Run(() =>
        {
          //Pan the view to a given extent.
          envelope = EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);
          mapView.PanTo(envelope);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
      #region Pan Async To an Extent
      {
        //Create the envelope
        envelope = EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);

        //Pan the view to a given extent.
        mapView.PanToAsync(envelope, TimeSpan.FromSeconds(2));
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelected(System.Nullable{System.TimeSpan})
      #region Pan To Selected Features
      {
        await QueuedTask.Run(() =>
        {
          //Pan to the map's selected features.
          mapView.PanToSelected();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelectedAsync(System.Nullable{System.TimeSpan})
      #region Pan AsyncTo Selected Features
      {
        //Pan to the map's selected features.
        mapView.PanToSelectedAsync(TimeSpan.FromSeconds(2));
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
      // cref: ArcGIS.Desktop.Mapping.Bookmark.Name
      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Pan To Bookmark
      {
        await QueuedTask.Run(() =>
        {
          //Get the first bookmark with the given name.
          var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
          if (bookmark == null)
          {
            // Manage the error - bookmark not found
          }

          //Pan the view to the bookmark.
          mapView.PanTo(bookmark);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
      // cref: ArcGIS.Desktop.Mapping.Bookmark.Name
      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Pan To Bookmark Async
      {
        //Get the first bookmark with the given name.
        var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
        if (bookmark == null)
        {
          // Manage the error - bookmark not found
        }

        //Pan the view to the bookmark.
        mapView.PanToAsync(bookmark, TimeSpan.FromSeconds(2));
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
      // cref: ArcGIS.Desktop.Mapping.Layer.IsVisible
      // cref: ArcGIS.Desktop.Mapping.Map.Layers
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Pan To Visible Layers
      {
        await QueuedTask.Run(() =>
        {
          //Pan to all visible layers in the map.
          var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
          mapView.PanTo(visibleLayers);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers()
      // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
      #region Pan To Selected Layers Asynchronous
      {
        //Pan to the selected layers in the TOC
        var selectedLayers = mapView.GetSelectedLayers();
        mapView.PanToAsync(selectedLayers);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.Camera
      // cref: ArcGIS.Desktop.Mapping.Camera.Heading
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Camera,System.Nullable{System.TimeSpan})
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Camera,System.Nullable{System.TimeSpan})
      #region Rotate the map view
      {
        //Get the camera for the view, adjust the heading and zoom to the new camera position.
        camera = mapView.Camera;
        camera.Heading = heading;
        await mapView.ZoomToAsync(camera, TimeSpan.Zero);
        //Or synchronously
        mapView.ZoomTo(camera, TimeSpan.Zero);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.MapView.Extent
      #region Expand Extent
      {
        await QueuedTask.Run(() =>
        {
          //Expand the current extent by the given ratio.
          var extent = mapView.Extent;
          var newExtent = GeometryEngine.Instance.Expand(extent, dx, dy, true);
          mapView.ZoomTo(newExtent);
        });
      }
      #endregion

      #region ProSnippet Group: Maps
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      // cref: ArcGIS.Desktop.Mapping.Map.Name
      #region Get the active map's name
      {
        //Return the name of the map currently displayed in the active map view.
        var result = mapView.Map.Name;
        // Use the result variable as needed
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.SetSelection(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Clear all selection in an Active map
      {
        await QueuedTask.Run(() =>
        {
          mapView.Map?.SetSelection(null);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SelectionEnvironment.SelectionTolerance
      // cref: ArcGIS.Desktop.Mapping.Map.GetDefaultExtent()
      // cref: ArcGIS.Desktop.Mapping.MapView.MapToScreen(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Desktop.Mapping.MapView.ScreenToMap(System.Windows.Point)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Calculate Selection tolerance in map units
      {
        //Selection tolerance for the map in pixels
        var selectionTolerance = SelectionEnvironment.SelectionTolerance;
        await QueuedTask.Run(() =>
        {
          //Get the map center
          var mapExtent = mapView.Map.GetDefaultExtent();
          var mapPoint = mapExtent.Center;
          //Map center as screen point
          var screenPoint = mapView.MapToScreen(mapPoint);
          //Add selection tolerance pixels to get a "radius".
          var radiusScreenPoint = new System.Windows.Point(screenPoint.X + selectionTolerance, screenPoint.Y);
          var radiusMapPoint = mapView.ScreenToMap(radiusScreenPoint);
          //Calculate the selection tolerance distance in map units.
          var searchRadius = GeometryEngine.Instance.Distance(mapPoint, radiusMapPoint);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapViewOverlayControl
      // cref: ArcGIS.Desktop.Mapping.MapView.AddOverlayControl(IMapViewOverlayControl)
      // cref: ArcGIS.Desktop.Mapping.MapView.RemoveOverlayControl(IMapViewOverlayControl)
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      #region MapView Overlay Control
      {
        //Create a Progress Bar user control
        System.Windows.Controls.ProgressBar progressBarControl = new()
        {
          //Configure the progress bar
          Minimum = 0,
          Maximum = 100,
          IsIndeterminate = true,
          Width = 300,
          Value = 10,
          Height = 25,
          Visibility = System.Windows.Visibility.Visible
        };
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

      #region ProSnippet Group: Layers
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.Layers
      // cref: ArcGIS.Desktop.Mapping.MapView.SelectLayers(System.Collections.Generic.IReadOnlyCollection<ArcGIS.Desktop.Mapping.Layer>)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Select all feature layers in TOC
      {
        //Zoom to the selected layers in the TOC
        var featureLayers = mapView.Map.Layers.OfType<FeatureLayer>();
        mapView.SelectLayers(featureLayers.ToList());
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetSelection()
      // cref: ArcGIS.Desktop.Mapping.MapView.FlashFeature(ArcGIS.Desktop.Mapping.SelectionSet)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Flash selected features
      {
        await QueuedTask.Run(() =>
        {
          //Get the selected features from the map and filter out the standalone table selection.
          var selectedFeatures = mapView.Map.GetSelection();

          //Flash the collection of features.
          mapView.FlashFeature(selectedFeatures);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Layer.IsVisibleInView(ArcGIS.Desktop.Mapping.MapView)
      #region Check if layer is visible in the current map view
      {
        if (layer == null)
        {
          // no layers in the map, leave
        }
        if (mapView == null)
        {
          // no active map view, leave
        }
        bool isLayerVisibleInView = layer.IsVisibleInView(mapView);
        if (isLayerVisibleInView)
        {
          //Do Something
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.SelectLayers(System.Collections.Generic.IReadOnlyCollection<ArcGIS.Desktop.Mapping.Layer>)
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      #region Select a layer and open its layer properties page
      {
        // select it in the TOC
        List<Layer> layersToSelect = [featureLayer];
        MapView.Active.SelectLayers(layersToSelect);

        // now execute the layer properties command
        var wrapper = FrameworkApplication.GetPlugInWrapper("esri_mapping_selectedLayerPropertiesButton");
        var command = wrapper as ICommand;
        if (command == null)
        {
          // the command is not found, leave
        }

        // execute the command
        if (command.CanExecute(null))
        {
          command.Execute(null);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.ClearSelection()
      #region Clear selection for a specific layer
      {
        if (featureLayer != null)
        {
          await QueuedTask.Run(() =>
          {
            featureLayer.ClearSelection();
          });
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.FrameworkExtender.GetMapTableView(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Core.CIM.CIMTableView.DisplaySubtypeDomainDescriptions
      // cref: ArcGIS.Core.CIM.CIMTableView.SelectionMode
      // cref: ArcGIS.Core.CIM.CIMTableView.ShowOnlyContingentValueFields
      // cref: ArcGIS.Core.CIM.CIMTableView.HighlightInvalidContingentValueFields
      // cref: ArcGIS.Desktop.Core.FrameworkExtender.OpenTablePane(ArcGIS.Core.CIM.CIMMapTableView)
      // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Display Table pane for Map Member
      {
        mapMember = MapView.Active.Map.GetLayersAsFlattenedList().OfType<MapMember>().FirstOrDefault();
        //Gets or creates the CIMMapTableView for a MapMember.
        var mapTableView = FrameworkApplication.Panes.GetMapTableView(mapMember);
        //Configure the table view
        mapTableView.DisplaySubtypeDomainDescriptions = false;
        mapTableView.SelectionMode = false;
        mapTableView.ShowOnlyContingentValueFields = true;
        mapTableView.HighlightInvalidContingentValueFields = true;
        //Open the table pane using the configured tableView. If a table pane is already open it will be activated.
        //You must be on the UI thread to call this function.
        var tablePane = FrameworkApplication.Panes.OpenTablePane(mapTableView);
      }
      #endregion

      #region ProSnippet Group: TableView
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.SetViewMode(TableViewMode)
      // cref: ArcGIS.Desktop.Mapping.TableViewMode
      #region Set Table ViewingMode
      {
        // change to "selected record" mode
        tableView.SetViewMode(TableViewMode.eSelectedRecords);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.SetZoomLevel(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.TableView.ZoomLevel
      #region Set ZoomLevel
      {
        // change zoom level
        if (tableView.ZoomLevel > zoomLevel)
        {
          tableView.SetZoomLevel(zoomLevel);
        }
        else
        {
          tableView.SetZoomLevel(zoomLevel * 2);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.ShowFieldAlias
      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleFieldAlias
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleFieldAlias()
      #region Toggle Field Alias
      {
        // set the value
        tableView.ShowFieldAlias = true;

        // OR toggle it
        if (tableView.CanToggleFieldAlias)
        {
          tableView.ToggleFieldAlias();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.ShowSubtypeDomainDescriptions
      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleSubtypeDomainDescriptions
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleSubtypeDomainDescriptionsAsync()
      #region Toggle Subtype Descriptions
      {
        // set the value
        tableView.ShowSubtypeDomainDescriptions = true;

        // OR toggle it
        if (tableView.CanToggleSubtypeDomainDescriptions)
        {
          tableView.ToggleSubtypeDomainDescriptionsAsync();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRowIndex
      #region Get the active row
      {
        // get the active row index
        int rowIndex = tableView.ActiveRowIndex;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRowIndex
      // cref: ArcGIS.Desktop.Mapping.TableView.BringIntoView(System.Int32,System.Int32)
      #region Change the active row
      {
        // get the active row index
        int rowIndex = tableView.ActiveRowIndex;

        // move to a different row
        var newIndex = 10 + rowIndex;
        tableView.BringIntoView(newIndex);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveObjectId
      #region Get the active object ID
      {
        // get the active objectID
        long? OID = tableView.ActiveObjectId;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.GetObjectIdAsync(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.TableView.GetRowIndexAsync(System.Int64, System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRowIndex
      #region Translate between rowIndex and objectID
      {
        // get the active row index
        int rowIndex = tableView.ActiveRowIndex;
        // increase
        int newIndex = rowIndex + 10;
        // get the objectID
        long newOID = await tableView.GetObjectIdAsync(newIndex);

        // get the rowIndex for a specific objectID
        //   2nd parameter indicates if the search only occurs for the pages loaded
        //   if pass false, then in the worst case, a full table scan will occur to
        //    find the objectID.
        long OID = 100;
        var idx = tableView.GetRowIndexAsync(OID, true);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedObjectIds()
      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedRowIndexes()
      #region Get selected rows or row indexes
      {
        await QueuedTask.Run(() =>
        {
          // get the set of selected objectIDs
          var selOids = tableView.GetSelectedObjectIds();
          // get the set of selected row indexes
          var selRows = tableView.GetSelectedRowIndexes();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.Select(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedRowIndexes()
      #region Change selected rows
      {
        await QueuedTask.Run(() =>
        {
          // set of selected OIDS
          var newoids = new List<long>();
          newoids.AddRange([10, 15, 17]);
          tableView.Select(newoids, true);

          // add to set of selected row indexes
          var selRows = tableView.GetSelectedRowIndexes();
          var newRows = new List<long>(selRows);
          newRows.AddRange([21, 35]);
          tableView.Select(newRows, false);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanSelectAll
      // cref: ArcGIS.Desktop.Mapping.TableView.SelectAll()
      #region Select all rows
      {
        if (tableView.CanSelectAll)
        {
          tableView.SelectAll();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleRowSelection
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleRowSelection()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSwitchSelection
      // cref: ArcGIS.Desktop.Mapping.TableView.SwitchSelection()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanClearSelection
      // cref: ArcGIS.Desktop.Mapping.TableView.ClearSelection()
      #region Toggle, Switch, Clear Selection
      {
        // toggle the active rows selection
        if (tableView.CanToggleRowSelection)
        {
          tableView.ToggleRowSelection();
        }

        // switch the selection
        if (tableView.CanSwitchSelection)
        {
          tableView.SwitchSelection();
        }

        // clear the selection
        if (tableView.CanClearSelection)
        {
          tableView.ClearSelection();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanZoomToSelected
      // cref: ArcGIS.Desktop.Mapping.TableView.ZoomToSelected()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanPanToSelected
      // cref: ArcGIS.Desktop.Mapping.TableView.PanToSelected()
      #region Zoom or Pan To Selected Rows
      {
        if (tableView.CanZoomToSelected)
        {
          tableView.ZoomToSelected();
        }

        if (tableView.CanPanToSelected)
        {
          tableView.PanToSelected();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanDeleteSelected
      // cref: ArcGIS.Desktop.Mapping.TableView.DeleteSelected()
      #region Delete Selected Rows
      {
        if (tableView.CanDeleteSelected)
        {
          tableView.DeleteSelected();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanGetHighlightedObjectIds
      // cref: ArcGIS.Desktop.Mapping.TableView.GetHighlightedObjectIds()
      #region Get highlighted row indexes
      {
        await QueuedTask.Run(() =>
        {
          IReadOnlyList<long> highlightedOIDs = null;
          if (tableView.CanGetHighlightedObjectIds)
          {
            // get the set of selected objectIDs
            highlightedOIDs = tableView.GetHighlightedObjectIds();
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanHighlight
      // cref: ArcGIS.Desktop.Mapping.TableView.Highlight(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedObjectIds()
      #region Change highlighted rows
      {
        await QueuedTask.Run(() =>
        {
          // get list of current selected objectIDs
          IReadOnlyList<long> selectedObjectIds = tableView.GetSelectedObjectIds();
          List<long> idsToHighlight = [];

          // add the first two selected objectIds to highlight
          if (selectedObjectIds.Count >= 2)
          {
            idsToHighlight.Add(selectedObjectIds[0]);
            idsToHighlight.Add(selectedObjectIds[1]);
          }

          // highlight
          if (tableView.CanHighlight)
          {
            tableView.Highlight(idsToHighlight, true);
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleRowHighlight
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleRowHighlight()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSwitchHighlight
      // cref: ArcGIS.Desktop.Mapping.TableView.SwitchHighlight()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanClearHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.ClearHighlighted()
      #region Toggle, Switch, Clear Highlights
      {
        // toggle the active rows selection
        if (tableView.CanToggleRowHighlight)
        {
          tableView.ToggleRowHighlight();
        }

        // switch highlighted rows
        if (tableView.CanSwitchHighlight)
        {
          tableView.SwitchHighlight();
        }

        // clear the highlights
        if (tableView.CanClearHighlighted)
        {
          tableView.ClearHighlighted();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanZoomToHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.ZoomToHighlighted()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanPanToHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.PanToHighlighted()
      #region Zoom or Pan To Highlighted Rows
      {
        if (tableView.CanZoomToHighlighted)
        {
          tableView.ZoomToHighlighted();
        }

        if (tableView.CanPanToHighlighted)
        {
          tableView.PanToHighlighted();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanDeleteHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.DeleteHighlighted()
      #region Delete Highlighted Rows
      {
        if (tableView.CanDeleteHighlighted)
        {
          tableView.DeleteHighlighted();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.GetFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.GetFieldIndex(System.String)
      // cref: ArcGIS.Desktop.Mapping.TableView.GetField(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.FieldDescription
      #region Field Access
      {
        // field access
        var flds = tableView.GetFields();
        var fldIdx = tableView.GetFieldIndex("STATE_NAME");
        var fldDesc = tableView.GetField(fldIdx);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveFieldIndex
      // cref: ArcGIS.Desktop.Mapping.TableView.GetField(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.TableView.SetActiveField(System.String)
      // cref: ArcGIS.Desktop.Mapping.TableView.SetActiveField(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.FieldDescription
      // cref: ArcGIS.Desktop.Mapping.FieldDescription.Name
      #region Get or set the Active Field
      {
        // get active field, active field name
        var activeFieldIdx = tableView.ActiveFieldIndex;
        var fldDesc = tableView.GetField(activeFieldIdx);
        var fldName = fldDesc.Name;

        // set active field by name
        tableView.SetActiveField("STATE_NAME");

        // or set active field by index
        tableView.SetActiveField(3);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.SetSelectedFields(List<string>)
      #region Select Fields
      {
        // get selected fields
        var selectedfields = tableView.GetSelectedFields();

        // set selected fields
        tableView.SetSelectedFields(["CITY_FIPS", "STATE_FIPS"]);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanResetFieldOrder
      // cref: ArcGIS.Desktop.Mapping.TableView.ResetFieldOrder()
      // cref: ArcGIS.Desktop.Mapping.TableView.SetFieldOrderAsync(List<string>)
      #region Set Field Order
      {
        if (tableView.CanResetFieldOrder)
        {
          tableView.ResetFieldOrder();

          List<string> fldOrder =
          [
            "STATE_NAME",
            "STATE_FIPS"
          ];
          tableView.SetFieldOrderAsync(fldOrder);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.GetHiddenFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanShowAllFields
      // cref: ArcGIS.Desktop.Mapping.TableView.ShowAllFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.SetHiddenFields(List<string>)
      // cref: ArcGIS.Desktop.Mapping.TableView.CanHideSelectedFields
      // cref: ArcGIS.Desktop.Mapping.TableView.HideSelectedFields()
      #region Show or Hide Fields
      {
        // get list of hidden fields
        var hiddenFields = tableView.GetHiddenFields();

        // show all fields
        if (tableView.CanShowAllFields)
        {
          tableView.ShowAllFields();
        }

        // hide only "CITY_FIPS", "STATE_FIPS"
        if (tableView.CanShowAllFields)
        {
          // show all fields
          tableView.ShowAllFields();
          tableView.SetHiddenFields(["CITY_FIPS", "STATE_FIPS"]);
        }

        // add "STATE_NAME to set of hidden fields
        tableView.SetHiddenFields(["STATE_NAME"]);

        // hide selected fields
        if (tableView.CanHideSelectedFields)
        {
          tableView.HideSelectedFields();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.GetFrozenFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.ClearAllFrozenFieldsAsync()
      // cref: ArcGIS.Desktop.Mapping.TableView.SetFrozenFieldsAsync(List<string>)
      #region Freeze Fields
      {
        // get list of frozen fields
        var frozenfields = tableView.GetFrozenFields();

        // unfreeze all fields
        await tableView.ClearAllFrozenFieldsAsync();

        // freeze a set of fields
        tableView.SetFrozenFieldsAsync(["CITY_FIPS", "STATE_FIPS"]);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanSortDescending
      // cref: ArcGIS.Desktop.Mapping.TableView.SortDescending()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSortAscending
      // cref: ArcGIS.Desktop.Mapping.TableView.SortAscending()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanCustomSort
      // cref: ArcGIS.Desktop.Mapping.TableView.CustomSort()
      // cref: ArcGIS.Desktop.Mapping.TableView.SortAsync(System.Collections.Generic.IReadOnlyDictionary{System.String,ArcGIS.Core.CIM.FieldSortInfo})
      #region Sort
      {
        // sort the active field descending
        if (tableView.CanSortDescending)
        {
          tableView.SortDescending();
        }

        // sort the active field ascending
        if (tableView.CanSortAscending)
        {
          tableView.SortAscending();
        }

        // perform a custom sort programmatically
        if (tableView.CanCustomSort)
        {
          // sort fields
          Dictionary<string, FieldSortInfo> dict = new()
          {
            { "STATE_NAME", FieldSortInfo.Asc },
            { "CITY_NAME", FieldSortInfo.Desc }
          };
          tableView.SortAsync(dict);
        }

        // perform a custom sort via the UI
        if (tableView.CanCustomSort)
        {
          tableView.CustomSort();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanFind
      // cref: ArcGIS.Desktop.Mapping.TableView.Find()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanFindAndReplace
      // cref: ArcGIS.Desktop.Mapping.TableView.FindAndReplace()
      #region Find and Replace
      {
        // launch the find UI
        if (tableView.CanFind)
        {
          tableView.Find();
        }

        // or launch the find and replace UI
        if (tableView.CanFindAndReplace)
        {
          tableView.FindAndReplace();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanGoTo
      // cref: ArcGIS.Desktop.Mapping.TableView.GoTo()
      #region GoTo TableView
      {
        // launch the GoTo UI
        if (tableView.CanGoTo)
        {
          tableView.GoTo();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableView.CanRefresh
      // cref: ArcGIS.Desktop.Mapping.TableView.Refresh()
      #region Refresh
      {
        // refresh
        if (tableView.CanRefresh)
        {
          tableView.Refresh();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
      // cref: ArcGIS.Desktop.Mapping.ITablePane
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx.Caption
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane.Caption
      #region Change table View caption
      {
        // find all the table panes (table panes hosting map data)
        var tablePanes = FrameworkApplication.Panes.OfType<ITablePane>();
        var tablePane = tablePanes.FirstOrDefault(p => p is ITablePaneEx { Caption: "oldCaption" });
        if (tablePane is ITablePaneEx tablePaneEx)
        {
          tablePaneEx.Caption = "newCaption";
        }

        // find all the external table panes (table panes hosting external data)
        var externalPanes = FrameworkApplication.Panes.OfType<IExternalTablePane>();
        var externalTablePane = externalPanes.FirstOrDefault(p => p.Caption == "oldCaption");
        if (externalTablePane != null)
        {
          externalTablePane.Caption = "newCaption";
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
      // cref: ArcGIS.Desktop.Mapping.ITablePane
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx.TableView
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane.TableView
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane.Caption
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx.Caption
      #region Get TableView from table pane
      {
        // find all the table panes (table panes hosting map data)
        var tablePanes = FrameworkApplication.Panes.OfType<ITablePane>();
        var tablePane = tablePanes.FirstOrDefault(p => p is ITablePaneEx { Caption: "caption" });
        if (tablePane is ITablePaneEx tablePaneEx)
        {
          tableView = tablePaneEx.TableView;
        }

        // if it's not found, maybe it's an external table pane
        if (tableView == null)
        {
          // find all the external table panes (table panes hosting external data)
          var externalPanes = FrameworkApplication.Panes.OfType<IExternalTablePane>();
          var externalTablePane = externalPanes.FirstOrDefault(p => p.Caption == "caption");
          if (externalTablePane != null)
          {
            tableView = externalTablePane.TableView;
          }
        }
      }
      #endregion

      #region ProSnippet Group: Features
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition()
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition(ArcGIS.Core.CIM.CIMBaseLayer)
      // cref: ArcGIS.Desktop.Mapping.Map.Layers
      // cref: ArcGIS.Desktop.Mapping.MapMember.Name
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Mask feature
      {
        await QueuedTask.Run(() =>
        {
          //Get the layer to be masked
          var lineLyrToBeMasked = mapView.Map.Layers.FirstOrDefault(lyr => lyr.Name == "TestLine") as FeatureLayer;
          //Get the layer's definition
          var lyrDefn = lineLyrToBeMasked.GetDefinition();
          //Create an array of Masking layers (polygon only)
          //Set the LayerMasks property of the Masked layer
          lyrDefn.LayerMasks = ["CIMPATH=map3/testpoly.xml"];
          //Re-set the Masked layer's definition
          lineLyrToBeMasked.SetDefinition(lyrDefn);
        });
      }
      #endregion

      #region ProSnippet Group: Bookmarks
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Desktop.Mapping.MapView, System.String)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Create a new bookmark using the active map view
      {
        await QueuedTask.Run(() =>
        {
          //Adding a new bookmark using the active view.
          mapView.Map.AddBookmark(mapView, bookmarkName);
        });
      }
      #endregion

      // cref: ArcGIS.Core.CIM.CIMBookmark
      // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
      // cref: ArcGIS.Core.CIM.CIMBookmark.Name
      // cref: ArcGIS.Core.CIM.CIMBookmark.ThumbnailImagePath
      // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Core.CIM.CIMBookmark)
      // cref: ArcGIS.Desktop.Mapping.Camera.Heading
      // cref: ArcGIS.Desktop.Mapping.Camera.Pitch
      // cref: ArcGIS.Desktop.Mapping.Camera.Roll
      // cref: ArcGIS.Desktop.Mapping.Camera.Scale
      // cref: ArcGIS.Desktop.Mapping.Camera.X
      // cref: ArcGIS.Desktop.Mapping.Camera.Y
      // cref: ArcGIS.Desktop.Mapping.Camera.Z
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Add New Bookmark from CIMBookmark
      {
        await QueuedTask.Run(() =>
        {
          //Set properties for Camera
          CIMViewCamera cimCamera = new()
          {
            X = camera.X,
            Y = camera.Y,
            Z = camera.Z,
            Scale = camera.Scale,
            Pitch = camera.Pitch,
            Heading = camera.Heading,
            Roll = camera.Roll
          };

          //Create new CIM bookmark and populate its properties
          var cimBookmark = new CIMBookmark() { Camera = cimCamera, Name = cameraName, ThumbnailImagePath = "" };

          //Add a new bookmark for the active map.
          mapView.Map.AddBookmark(cimBookmark);
        });
      }
      #endregion

      #region Get the collection of bookmarks for the project
      {
        //Get the collection of bookmarks for the project.
        var result = Project.Current.GetBookmarks();
        // Use the bookmarks (if any)
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Get Map Bookmarks
      {
        await QueuedTask.Run(() =>
        {
          //Return the collection of bookmarks for the map.
          var result = mapView.Map.GetBookmarks();
          // Use the bookmarks (if any)
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.MoveBookmark(ArcGIS.Desktop.Mapping.Bookmark,System.Int32)
      // cref: ArcGIS.Desktop.Mapping.Bookmark.Name
      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Move Bookmark to the Top
      {
        await QueuedTask.Run(() =>
        {
          var map = mapView.Map;
          //Find the first bookmark with the name
          var bookmark = map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
          if (bookmark == null)
          {
            //Bookmark not found
          }

          //Move the bookmark to the top of the list
          map.MoveBookmark(bookmark, 0);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Bookmark.Rename(System.String)
      // cref: ArcGIS.Desktop.Mapping.Bookmark.Name
      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Rename Bookmark
      {
        oldBookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
        oldBookmark.Rename(newBookmarkName);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveBookmark(ArcGIS.Desktop.Mapping.Bookmark)
      // cref: ArcGIS.Desktop.Mapping.Bookmark
      // cref: ArcGIS.Desktop.Mapping.Bookmark.Name
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Remove bookmark with a given name
      {
        await QueuedTask.Run(() =>
        {
          //Find the first bookmark with the name
          var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
          if (bookmark == null)
          {
            //Bookmark not found
          }

          //Remove the bookmark
          mapView.Map.RemoveBookmark(bookmark);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Bookmark.SetThumbnail(System.Windows.Media.Imaging.BitmapSource)
      #region Change the thumbnail for a bookmark
      {
        //Set the thumbnail to an image on disk, i.e. C:\Pictures\MyPicture.png.
        BitmapImage image = new(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        oldBookmark.SetThumbnail(image);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Bookmark.Update(ArcGIS.Desktop.Mapping.MapView)
      #region Update Bookmark
      {
        await QueuedTask.Run(() =>
        {
          //Update the bookmark using the active map view.
          oldBookmark.Update(mapView);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Bookmark.GetDefinition()
      // cref: ArcGIS.Core.CIM.CIMBookmark
      // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
      // cref: ArcGIS.Core.CIM.CIMBookmark.Location
      // cref: ArcGIS.Desktop.Mapping.Bookmark.SetDefinition(ArcGIS.Core.CIM.CIMBookmark)
      #region Update Extent for a Bookmark
      {
        await QueuedTask.Run(() =>
        {
          //Get the bookmark's definition
          var bookmarkDef = oldBookmark.GetDefinition();

          //Modify the bookmark's location
          bookmarkDef.Location = envelope;

          //Clear the camera as it is no longer valid.
          bookmarkDef.Camera = null;

          //Set the bookmark definition
          oldBookmark.SetDefinition(bookmarkDef);
        });
      }
      #endregion

      #region ProSnippet Group: Time
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TimeDelta
      // cref: ArcGIS.Desktop.Mapping.TimeUnit
      // cref: ArcGIS.Desktop.Mapping.MapView.Time
      // cref: ArcGIS.Desktop.Mapping.TimeRange.Offset(ArcGIS.Desktop.Mapping.TimeDelta)
      #region Step forward in time by 1 month
      {
        //Step current map time forward by 1 month
        TimeDelta timeDelta = new(1, TimeUnit.Months);
        mapView.Time = mapView.Time.Offset(timeDelta);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.Time
      // cref: ArcGIS.Desktop.Mapping.TimeRange.Start
      // cref: ArcGIS.Desktop.Mapping.TimeRange.End
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      #region  Disable time in the map.
      {
        MapView.Active.Time.Start = null;
        MapView.Active.Time.End = null;
      }
      #endregion

      #region ProSnippet Group: Animations
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation.Duration
      // cref: ArcGIS.Desktop.Mapping.Animation.ScaleDuration(System.Double)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Set Animation Length
      {
        var animation = mapView.Map.Animation;
        var duration = animation.Duration;
        if (duration == TimeSpan.Zero)
        {
          return;
        }

        var factor = length.TotalSeconds / duration.TotalSeconds;
        animation.ScaleDuration(factor);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation.Duration
      // cref: ArcGIS.Desktop.Mapping.Animation.ScaleDuration(System.TimeSpan,System.TimeSpan,System.Double)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Scale Animation
      {
        var animation = mapView.Map.Animation;
        var duration = animation.Duration;
        if (duration == TimeSpan.Zero || duration <= afterTime)
        {
          // Nothing to scale, leave
        }

        var factor = length.TotalSeconds / (duration.TotalSeconds - afterTime.TotalSeconds);
        animation.ScaleDuration(afterTime, duration, factor);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
      // cref: ArcGIS.Desktop.Mapping.CameraTrack
      // cref: ArcGIS.Desktop.Mapping.Track.Keyframes
      // cref: ArcGIS.Desktop.Mapping.CameraKeyframe
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Camera Keyframes
      {
        var animation = mapView.Map.Animation;
        var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
        var result = cameraTrack.Keyframes.OfType<CameraKeyframe>().ToList();
        //Use the camera keyframes (if any)
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
      // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCameraAtTime(System.TimeSpan)
      // cref: ArcGIS.Desktop.Mapping.Animation.Duration
      // cref: ArcGIS.Desktop.Mapping.Map.Animation
      // cref: ArcGIS.Desktop.Mapping.MapView.Animation
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Interpolate Camera
      {
        //Return the collection representing the camera for each frame in animation.
        await QueuedTask.Run(() =>
        {
          var animation = mapView.Map.Animation;

          var cameras = new List<Camera>();
          //We will use ticks here rather than milliseconds to get the highest precision possible.
          var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
          for (int i = 0; i < animation.NumberOfFrames; i++)
          {
            var time = TimeSpan.FromTicks(i * ticksPerFrame);
            //Because of rounding for ticks the last calculated time may be greater than the duration.
            if (time > animation.Duration)
            {
              time = animation.Duration;
            }

            cameras.Add(mapView.Animation.GetCameraAtTime(time));
          }
          // Use cameras
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TimeRange
      // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
      // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCurrentTimeAtTime(System.TimeSpan)
      // cref: ArcGIS.Desktop.Mapping.Animation.Duration
      // cref: ArcGIS.Desktop.Mapping.Map.Animation
      // cref: ArcGIS.Desktop.Mapping.MapView.Animation
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Interpolate Time
      {
        //Return the collection representing the map time for each frame in animation.
        await QueuedTask.Run(() =>
        {
          var animation = mapView.Map.Animation;

          var timeRanges = new List<TimeRange>();
          //We will use ticks here rather than milliseconds to get the highest precision possible.
          var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
          for (int i = 0; i < animation.NumberOfFrames; i++)
          {
            var time = TimeSpan.FromTicks(i * ticksPerFrame);
            //Because of rounding for ticks the last calculated time may be greater than the duration.
            if (time > animation.Duration)
            {
              time = animation.Duration;
            }

            timeRanges.Add(mapView.Animation.GetCurrentTimeAtTime(time));
          }
          // Use timeRanges;
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Range
      // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
      // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCurrentRangeAtTime(System.TimeSpan)
      // cref: ArcGIS.Desktop.Mapping.Animation.Duration
      // cref: ArcGIS.Desktop.Mapping.Map.Animation
      // cref: ArcGIS.Desktop.Mapping.MapView.Animation
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Interpolate Range
      {
        //Return the collection representing the map time for each frame in animation.
        await QueuedTask.Run(() =>
        {
          var animation = mapView.Map.Animation;

          var ranges = new List<ArcGIS.Desktop.Mapping.Range>();
          //We will use ticks here rather than milliseconds to get the highest precision possible.
          var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
          for (int i = 0; i < animation.NumberOfFrames; i++)
          {
            var time = TimeSpan.FromTicks(i * ticksPerFrame);
            //Because of rounding for ticks the last calculated time may be greeting than the duration.
            if (time > animation.Duration)
            {
              time = animation.Duration;
            }

            ranges.Add(mapView.Animation.GetCurrentRangeAtTime(time));
          }
          // Use ranges
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
      // cref: ArcGIS.Desktop.Mapping.CameraTrack
      // cref: ArcGIS.Desktop.Mapping.CameraTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Camera,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
      // cref: ArcGIS.Core.CIM.AnimationTransition
      // cref: ArcGIS.Desktop.Mapping.MapView.Camera
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Create Camera Keyframe
      {
        var animation = mapView.Map.Animation;
        var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
        cameraTrack.CreateKeyframe(mapView.Camera, atTime, AnimationTransition.FixedArc);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
      // cref: ArcGIS.Desktop.Mapping.TimeTrack
      // cref: ArcGIS.Core.CIM.AnimationTransition
      // cref: ArcGIS.Desktop.Mapping.TimeTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.TimeRange,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      // cref: ArcGIS.Desktop.Mapping.MapView.Time
      #region Create Time Keyframe
      {
        var animation = mapView.Map.Animation;
        var timeTrack = animation.Tracks.OfType<TimeTrack>().First(); //There will always be only 1 TimeTrack in the animation.
        timeTrack.CreateKeyframe(mapView.Time, atTime, AnimationTransition.Linear);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
      // cref: ArcGIS.Desktop.Mapping.RangeTrack
      // cref: ArcGIS.Core.CIM.AnimationTransition
      // cref: ArcGIS.Desktop.Mapping.RangeTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Range,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Create Range Keyframe
      {
        var animation = mapView.Map.Animation;
        var rangeTrack = animation.Tracks.OfType<RangeTrack>().First(); //There will always be only 1 RangeTrack in the animation.
        rangeTrack.CreateKeyframe(range, atTime, AnimationTransition.Linear);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Animation
      // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
      // cref: ArcGIS.Desktop.Mapping.LayerTrack
      // cref: ArcGIS.Core.CIM.AnimationTransition
      // cref: ArcGIS.Desktop.Mapping.LayerTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Layer,System.TimeSpan,System.Boolean,System.Double,ArcGIS.Core.CIM.AnimationTransition)
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Create Layer Keyframe
      {
        var animation = mapView.Map.Animation;
        var layerTrack = animation.Tracks.OfType<LayerTrack>().First(); //There will always be only 1 LayerTrack in the animation.
        layerTrack.CreateKeyframe(layer, atTime, true, transparency, AnimationTransition.Linear);
      }
      #endregion

      #region ProSnippet Group: Graphic overlay
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.CIM.CIMSymbolReference,System.Double)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.BlueRGB
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.RedRGB
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Desktop.Mapping.MapView.Extent
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.UpdateOverlay(System.IDisposable,ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.CIM.CIMSymbolReference)
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Graphic Overlay
      {
        // get the current MapView and point
        var myextent = mapView.Extent;
        var point = myextent.Center;
        IDisposable _graphic = null;

        // add point graphic to the overlay at the center of the mapView
        await QueuedTask.Run(() =>
        {
          _graphic = mapView.AddOverlay(point,
            SymbolFactory.Instance.ConstructPointSymbol(
                    ColorFactory.Instance.RedRGB, 30.0, SimpleMarkerStyle.Star).MakeSymbolReference());
        });

        // update the overlay with new point graphic symbol
        MessageBox.Show("Now to update the overlay...");
        await QueuedTask.Run(() =>
        {
          mapView.UpdateOverlay(_graphic, point, SymbolFactory.Instance.ConstructPointSymbol(
                                        ColorFactory.Instance.BlueRGB, 20.0, SimpleMarkerStyle.Circle).MakeSymbolReference());
        });

        // clear the overlay display by disposing of the graphic
        MessageBox.Show("Now to clear the overlay...");
        _graphic.Dispose();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Core.CIM.CIMGraphic,System.Double)
      // cref: ArcGIS.Core.CIM.CIMPictureGraphic
      // cref: ArcGIS.Core.CIM.CIMPictureGraphic.Shape
      #region Graphic Overlay with CIMPictureGraphic
      {
        // Use SourceURL for the URL to the image content. For
        // a local file, use a file path. For a web/internet file
        // location, use its URL
        //
        // Supported image types are:
        // png, jpg, tiff, bmp, gif, svg

        var pictureGraphic = new CIMPictureGraphic
        {
          SourceURL = @"C:\Images\MyImage.png",
          Shape = envelope
        };

        IDisposable _graphic = mapView.AddOverlay(pictureGraphic);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Core.CIM.CIMGraphic,System.Double)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.BlackRGB
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String,System.String)
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Add overlay graphic with text
      {
        IDisposable _graphic = null;

        //define the text symbol
        var textSymbol = new CIMTextSymbol();
        //define the text graphic
        var textGraphic = new CIMTextGraphic();

        await QueuedTask.Run(() =>
        {
          //Create a simple text symbol
          textSymbol = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 8.5, "Corbel", "Regular");
          //Sets the geometry of the text graphic
          textGraphic.Shape = geometry;
          //Sets the text string to use in the text graphic
          textGraphic.Text = "This is my line";
          //Sets symbol to use to draw the text graphic
          textGraphic.Symbol = textSymbol.MakeSymbolReference();
          //Draw the overlay text graphic
          _graphic = mapView.AddOverlay(textGraphic);
        });
      }
      #endregion

      #region ProSnippet Group: Mapping Options
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.SelectionOptions
      // cref: ArcGIS.Desktop.Core.SelectionOptions
      // cref:ArcGIS.Desktop.Mapping.SelectionMethod
      // cref:ArcGIS.Desktop.Mapping.SelectionCombinationMethod
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.CreateRGBColor(System.Double,System.Double,System.Double,System.Double)
      #region Get/Set Selection Options
      {
        var options = ApplicationOptions.SelectionOptions;

        await QueuedTask.Run(() =>
        {
          var defaultColor = options.DefaultSelectionColor;

          var color = options.SelectionColor as CIMRGBColor;
          options.SetSelectionColor(ColorFactory.Instance.CreateRGBColor(255, 0, 0));

          var defaultFill = options.DefaultSelectionFillColor;
          var fill = options.SelectionFillColor;
          var isHatched = options.IsSelectionFillHatched;
          options.SetSelectionFillColor(ColorFactory.Instance.CreateRGBColor(100, 100, 0));
          if (!isHatched)
          {
            options.SetSelectionFillIsHatched(true);
          }

          var showSelectionChip = options.ShowSelectionChip;
          options.SetShowSelectionChip(!showSelectionChip);

          var showSelectionGraphic = options.ShowSelectionGraphic;
          options.SetShowSelectionGraphic(!showSelectionGraphic);

          var saveSelection = options.SaveSelection;
          options.SetSaveSelection(!saveSelection);

          var defaultTol = options.DefaultSelectionTolerance;
          var tol = options.SelectionTolerance;
          options.SetSelectionTolerance(2 * defaultTol);

          // extension methods available
          var selMethod = options.SelectionMethod;
          options.SetSelectionMethod(SelectionMethod.Contains);

          var combMethod = options.CombinationMethod;
          options.SetCombinationMethod(SelectionCombinationMethod.Add);

          // note that the following SelectionCombinationMethod is not supported
          //options.SetCombinationMethod(SelectionCombinationMethod.XOR);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.TableOptions
      // cref: ArcGIS.Desktop.Core.TableOptions
      // cref:ArcGIS.Core.CIM.TableRowHeightType
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.CreateRGBColor(System.Double,System.Double,System.Double,System.Double)
      #region Get/Set Table Options
      {
        var options = ApplicationOptions.TableOptions;

        var hideAddNewRow = options.HideAddNewRow;
        options.HideAddNewRow = !hideAddNewRow;

        var overrides = options.HonorSelectionColorOverrides;
        options.HonorSelectionColorOverrides = !overrides;

        var activateMapView = options.ActivateMapViewAfterOperations;
        options.ActivateMapViewAfterOperations = !activateMapView;

        var defaultFontTName = options.DefaultFontName;
        var fontName = options.FontName;
        if (options.IsValidFontName("Arial"))
        {
          options.FontName = "Arial";
        }

        var defaultFontSize = options.DefaultFontSize;
        var fontSize = options.FontSize;
        if (options.IsValidFontSize(10))
        {
          options.FontSize = 10;
        }

        var heightType = options.ColumnHeaderHeightType;
        options.ColumnHeaderHeightType = TableRowHeightType.Double;

        var rowHeightType = options.RowHeightType;
        options.RowHeightType = TableRowHeightType.Single;

        var defaultColor = options.DefaultHighlightColor;
        var color = options.HighlightColor;
        await QueuedTask.Run(() =>
        {
          options.SetHighlightColor(ColorFactory.Instance.CreateRGBColor(0, 0, 255));
        });
      }
      #endregion

    }
  }
  //Class for pop-up snippets
  public partial class ProSnippetsMapExploration
  {
    public static void MapExplorationProSnippets2()
    {
      #region ignore - Variable initialization
      //Get the active map view.
      var mapView = MapView.Active;

      MapMember mapMember = mapView.Map.Layers.FirstOrDefault(lyr => lyr.Name == "Cities") as MapMember;
      IEnumerable<long> objectIDs = [5, 6, 11];
      long objectID = 0;
      #endregion

      #region ProSnippet Group: Popups
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ShowPopup(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
      #region Show a pop-up for a feature
      {
        mapView.ShowPopup(mapMember, objectID);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PopupContent
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.PopupContent})
      #region Show a custom pop-up
      {
        //Create custom popup content
        List<PopupContent> popups =
            [
                new("<b>This text is bold.</b>", "Custom tooltip from HTML string"),
                new(new Uri("https://www.esri.com/"), "Custom tooltip from Uri")
            ];
        mapView.ShowCustomPopup(popups);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PopupDefinition
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowPopup(ArcGIS.Desktop.Mapping.MapMember, System.Int64, ArcGIS.Desktop.Mapping.PopupDefinition)
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Append
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Dockable
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Position
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Size
      #region Show a pop-up for a feature using pop-up window properties
      {
        if (mapView == null)
        {
          return;
        }
        // Sample code: https://github.com/ArcGIS/arcgis-pro-sdk-community-samples/blob/master/Map-Exploration/CustomIdentify/CustomIdentify.cs
        var topLeftCornerPoint = new System.Windows.Point(200, 200);
        var popupDef = new PopupDefinition()
        {
          Append = true,      // if true new record is appended to existing (if any)
          Dockable = true,    // if true popup is dockable - if false Append is not applicable
          Position = topLeftCornerPoint,  // Position of top left corner of the popup (in pixels)
          Size = new System.Windows.Size(200, 400)    // size of the popup (in pixels)
        };
        mapView.ShowPopup(mapMember, objectID, popupDef);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PopupContent
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupContent>, System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupCommand>, System.Boolean, ArcGIS.Desktop.Mapping.PopupDefinition)
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Append
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Dockable
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Position
      // cref: ArcGIS.Desktop.Mapping.PopupDefinition.Size
      #region Show a custom pop-up using pop-up window properties
      {
        if (mapView == null)
        {
          return;
        }

        //Create custom popup content
        List<PopupContent> popups =
        [
            new("<b>This text is bold.</b>", "Custom tooltip from HTML string"),
                new(new Uri("https://www.esri.com/"), "Custom tooltip from Uri")
        ];
        // Sample code: https://github.com/ArcGIS/arcgis-pro-sdk-community-samples/blob/master/Framework/DynamicMenu/DynamicFeatureSelectionMenu.cs
        var topLeftCornerPoint = new System.Windows.Point(200, 200);
        var popupDef = new PopupDefinition()
        {
          Append = true,      // if true new record is appended to existing (if any)
          Dockable = true,    // if true popup is dockable - if false Append is not applicable
          Position = topLeftCornerPoint,  // Position of top left corner of the popup (in pixels)
          Size = new System.Windows.Size(200, 400)    // size of the popup (in pixels)
        };
        mapView.ShowCustomPopup(popups, null, true, popupDef);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PopupContent
      // cref: ArcGIS.Desktop.Mapping.PopupCommand
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.PopupContent},System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.PopupCommand},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.PopupContent.IDString
      // cref: ArcGIS.Desktop.Mapping.PopupContent.MapMember
      #region Show A pop-up With Custom Commands
      {
        //Create custom popup content from existing map member and object id
        List<PopupContent> popups = [new PopupContent(mapMember, objectID)];

        //Create a new custom command to add to the popup window
        List<PopupCommand> commands =
        [
          new PopupCommand(
                p => MessageBox.Show(string.Format("Map Member: {0}, ID: {1}", p.MapMember, p.IDString)),
                p => { return p != null; },
                "My custom command",
                System.Windows.Application.Current.Resources["GenericCheckMark16"] as ImageSource),
            ];

        mapView.ShowCustomPopup(popups, commands, true);
      }
      #endregion
    }

    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupContent>)
    // cref: ArcGIS.Desktop.Mapping.MapView.Active
    #region Show A Dynamic Pop-up
    public static void ShowDynamicPopup(MapMember mapMember, List<long> objectIDs)
    {
      MapView mapView = MapView.Active;
      if (mapView == null)
      {
        return;
      }
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
  //class for all the Map Tool snippets
  public partial class ProSnippetsMapExploration
  {
    #region ProSnippet Group: Tools
    #endregion
    // cref: ArcGIS.Desktop.Mapping.MapTool.IsSketchTool
    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchOutputMode
    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchType
    #region Change symbol for a sketch tool

    internal class SketchTool_WithSymbol : MapTool
    {
      public SketchTool_WithSymbol()
      {
        IsSketchTool = true;
        SketchOutputMode = SketchOutputMode.Map; //Changing the Sketch Symbol is only supported with map sketches.
        SketchType = SketchGeometryType.Rectangle;
      }

      protected override Task OnToolActivateAsync(bool hasMapViewChanged)
      {
        return QueuedTask.Run(() =>
        {
          //Set the Sketch Symbol if it hasn't already been set.
          if (SketchSymbol != null)
          {
            return;
          }

          var polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.CreateRGBColor(24, 69, 59),
                              SimpleFillStyle.Solid,
                            SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 1.0, SimpleLineStyle.Dash));
          SketchSymbol = polygonSymbol.MakeSymbolReference();
        });
      }
    }

    #endregion

    #region Create a tool to the return coordinates of the point clicked in the map

    internal class GetMapCoordinates : MapTool
    {
      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
        {
          e.Handled = true; //Handle the event args to get the call to the corresponding async method
        }
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        var mapView = MapView.Active;
        if (mapView == null)
          return Task.FromResult(0);

        return QueuedTask.Run(() =>
        {
          //Convert the clicked point in client coordinates to the corresponding map coordinates.
          var mapPoint = mapView.ClientToMap(e.ClientPoint);
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(string.Format("X: {0} Y: {1} Z: {2}",
                      mapPoint.X, mapPoint.Y, mapPoint.Z), "Map Coordinates");
        });
      }
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.IsSketchTool
    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchOutputMode
    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchType
    #region Create a tool to identify the features that intersect the sketch geometry

    internal class CustomIdentify : MapTool
    {
      public CustomIdentify()
      {
        IsSketchTool = true;
        SketchType = SketchGeometryType.Rectangle;

        //To perform a interactive selection or identify in 3D or 2D, sketch must be created in screen coordinates.
        SketchOutputMode = SketchOutputMode.Screen;
      }

      protected override Task<bool> OnSketchCompleteAsync(ArcGIS.Core.Geometry.Geometry geometry)
      {
        var mapView = MapView.Active;
        if (mapView == null)
          return Task.FromResult(true);

        return QueuedTask.Run(() =>
        {
          //Get all the features that intersect the sketch geometry and flash them in the view.
          var results = mapView.GetFeatures(geometry);
          mapView.FlashFeature(results);

          var debug = System.String.Join("\n", results.ToDictionary()
                        .Select(kvp => System.String.Format("{0}: {1}", kvp.Key.Name, kvp.Value.Count())));
          System.Diagnostics.Debug.WriteLine(debug);
          return true;
        });
      }
    }

    #endregion

    internal class Resource1
    {
      public static byte[] red_cursor = new byte[256];
    }

    // cref: ArcGIS.Desktop.Framework.Contracts.Tool.Cursor
    // cref: ArcGIS.Desktop.Mapping.MapTool.IsSketchTool
    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchOutputMode
    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchType
    #region Change the cursor of a Tool
    internal class CustomMapTool : MapTool
    {
      public CustomMapTool()
      {
        IsSketchTool = true;
        SketchType = SketchGeometryType.Rectangle;
        SketchOutputMode = SketchOutputMode.Map;
        //A custom cursor file as an embedded resource
        var cursorEmbeddedResource = new Cursor(new MemoryStream(Resource1.red_cursor));
        //A built in system cursor
        var systemCursor = System.Windows.Input.Cursors.ArrowCD;
        //Set the "CustomMapTool's" Cursor property to either one of the cursors defined above
        Cursor = cursorEmbeddedResource;
        //or
        Cursor = systemCursor;
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.ControlID
    #region Tool with an Embeddable Control

    // Using the Visual Studio SDK templates, add a MapTool and an EmbeddableControl
    // The EmbeddableControl is registered in the "esri_embeddableControls" category in the config.daml file
    //
    //  <categories>
    //    <updateCategory refID = "esri_embeddableControls" >
    //      <insertComponent id="mapTool_EmbeddableControl" className="EmbeddableControl1ViewModel">
    //        <content className = "EmbeddableControl1View" />
    //      </insertComponent>
    //    <updateCategory>
    //  </categories>
    internal class MapTool_WithControl : MapTool
    {
      public MapTool_WithControl()
      {
        // substitute this string with the daml ID of the embeddable control you added
        ControlID = "mapTool_EmbeddableControl";
      }

      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        e.Handled = true;
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        //Get the instance of the ViewModel
        var vm = EmbeddableControl;
        if (vm == null)
        {
          return Task.FromResult(0);
        }

        var mapView = MapView.Active;
        if (mapView == null)
          return Task.FromResult(0);

        // cast vm to your viewModel in order to access your properties

        //Get the map coordinates from the click point and set the property on the ViewMode.
        return QueuedTask.Run(() =>
        {
          var mapPoint = mapView.ClientToMap(e.ClientPoint);
          string clickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
        });
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.OverlayControlID
    #region Tool with an Overlay Embeddable Control

    // Using the Visual Studio SDK templates, add a MapTool and an EmbeddableControl
    // The EmbeddableControl is registered in the "esri_embeddableControls" category in the config.daml file
    //
    //  <categories>
    //    <updateCategory refID = "esri_embeddableControls" >
    //      <insertComponent id="mapTool_EmbeddableControl" className="EmbeddableControl1ViewModel">
    //        <content className = "EmbeddableControl1View" />
    //      </insertComponent>
    //    <updateCategory>
    //  </categories>

    internal class MapTool_WithOverlayControl : MapTool
    {
      public MapTool_WithOverlayControl()
      {
        // substitute this string with the daml ID of the embeddable control you added
        OverlayControlID = "mapTool_EmbeddableControl";
      }

      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        e.Handled = true;
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        //Get the instance of the ViewModel
        var vm = OverlayEmbeddableControl;
        if (vm == null)
        {
          return Task.FromResult(0);
        }

        var mapView = MapView.Active;
        if (mapView == null)
          return Task.FromResult(0);

        // cast vm to your viewModel in order to access your properties

        //Get the map coordinates from the click point and set the property on the ViewMode.
        return QueuedTask.Run(() =>
        {
          var mapPoint = mapView.ClientToMap(e.ClientPoint);
          string clickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
        });
      }
    }
    #endregion
  }
}
