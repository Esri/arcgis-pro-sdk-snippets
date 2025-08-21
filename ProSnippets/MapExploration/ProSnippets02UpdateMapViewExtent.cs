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
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
  /// <summary>
  /// Provides utility methods for interacting with the active map view in ArcGIS Pro,  including zooming, panning, and
  /// navigating to specific extents, features, or bookmarks.
  /// </summary>

  #region ProSnippet Group: Update MapView Extent (Zoom, Pan etc)
  #endregion
  public static class ProSnippetsUpdateMapViewExtent
  {
    // cref: ArcGIS.Desktop.Mapping.MapView.HasPreviousCamera
    // cref: ArcGIS.Desktop.Mapping.MapView.PreviousCameraAsync(System.Nullable{System.TimeSpan})
    #region Go To Previous Camera
    /// <summary>
    /// Navigates the active map view to the previous camera position, if available.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the map view
    /// successfully navigates to the previous camera position; otherwise, <see langword="false"/>.</returns>
    public static Task<bool> ZoomToPreviousCameraAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the selected layers in the TOC
      if (mapView.HasPreviousCamera())
        return mapView.PreviousCameraAsync();

      return Task.FromResult(false);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.HasNextCamera
    // cref: ArcGIS.Desktop.Mapping.MapView.NextCameraAsync(System.Nullable{System.TimeSpan})
    #region Go To Next Camera
    /// <summary>
    /// Advances the active map view to the next camera position, if available.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the map view
    /// successfully zooms to the next camera position; otherwise, <see langword="false"/>.</returns>
    public static Task<bool> ZoomToNextCameraAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the selected layers in the TOC
      if (mapView.HasNextCamera())
        return mapView.NextCameraAsync();

      return Task.FromResult(false);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtent(System.Nullable{System.TimeSpan})
    #region Zoom To Full Extent 
    /// <summary>
    /// Zooms the active map view to the full extent of the map.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful; otherwise, <see langword="false"/> if there is no active map view or the operation
    /// fails.</returns>
    public static Task<bool> ZoomToFullExtent()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom to the map's full extent
        return mapView.ZoomToFullExtent();
      });
    }

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtentAsync(System.Nullable{System.TimeSpan})
    /// <summary>
    /// Zooms the active map view to the full extent of the map.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was initiated successfully; otherwise, <see langword="false"/> if there is no active map view.</returns>
    public static Task<bool> ZoomToFullExtentAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the map's full extent
      return mapView.ZoomToFullExtentAsync(TimeSpan.FromSeconds(2));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixed(System.Nullable{System.TimeSpan})
    #region Fixed Zoom In
    /// <summary>
    /// Zooms in the active map view by a fixed amount.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful; otherwise, <see langword="false"/> if there is no active map view.</returns>
    public static Task<bool> ZoomInFixed()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom in the map view by a fixed amount.
        return mapView.ZoomInFixed();
      });
    }

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixedAsync(System.Nullable{System.TimeSpan})
    /// <summary>
    /// Zooms in the active map view by a fixed amount.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful;  otherwise, <see langword="false"/> if there is no active map view.</returns>
    public static Task<bool> ZoomInFixedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom in the map view by a fixed amount.
      return mapView.ZoomInFixedAsync();
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixed(System.Nullable{System.TimeSpan})
    #region Fixed Zoom Out 
    /// <summary>
    /// Zooms out the active map view by a fixed amount.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful;  otherwise, <see langword="false"/> if there is no active map view.</returns>
    public static Task<bool> ZoomOutFixed()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom out in the map view by a fixed amount.
        return mapView.ZoomOutFixed();
      });
    }

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixedAsync(System.Nullable{System.TimeSpan})
    /// <summary>
    /// Zooms out the active map view by a fixed amount.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful;  otherwise, <see langword="false"/> if there is no active map view.</returns>
    public static Task<bool> ZoomOutFixedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom in the map view by a fixed amount.
      return mapView.ZoomOutFixedAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To an Extent 
    /// <summary>
    /// Zooms the active map view to the specified extent.
    /// </summary>
    /// langword="false"/>. The zoom operation animates over a duration of 2 seconds.</remarks>
    /// <param name="xMin">The minimum X-coordinate of the extent.</param>
    /// <param name="yMin">The minimum Y-coordinate of the extent.</param>
    /// <param name="xMax">The maximum X-coordinate of the extent.</param>
    /// <param name="yMax">The maximum Y-coordinate of the extent.</param>
    /// <param name="spatialReference">The spatial reference of the extent. This defines the coordinate system of the specified coordinates.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation succeeds; otherwise, <see langword="false"/>.</returns>
    public static Task<bool> ZoomToExtent(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Create the envelope
      var envelope = ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);

      //Zoom the view to a given extent.
      return mapView.ZoomToAsync(envelope, TimeSpan.FromSeconds(2));
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.SpatialReference
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry, System.TimeSpan, System.Boolean)
    #region Zoom To a Point 
    /// <summary>
    /// Zooms the active map view to a specified point with a buffer around it.
    /// </summary>
    /// <param name="x">The x-coordinate of the point in the specified spatial reference system.</param>
    /// <param name="y">The y-coordinate of the point in the specified spatial reference system.</param>
    /// <param name="spatialReference">The spatial reference of the input coordinates.</param>
    /// <param name="buffer_size">The buffer size, in the same units as the spatial reference, to apply around the point for zooming.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation succeeds; otherwise, <see langword="false"/>.</returns>
    public static Task<bool> ZoomToPoint(double x, double y, ArcGIS.Core.Geometry.SpatialReference spatialReference, double buffer_size)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      return QueuedTask.Run(() =>
      {
        //Create a point
        var pt = MapPointBuilderEx.CreateMapPoint(x, y, spatialReference);
        //Buffer it - for purpose of zoom
        var poly = GeometryEngine.Instance.Buffer(pt, buffer_size);

        //do we need to project the buffer polygon?
        if (!MapView.Active.Map.SpatialReference.IsEqual(poly.SpatialReference))
        {
          //project the polygon
          poly = GeometryEngine.Instance.Project(poly, MapView.Active.Map.SpatialReference);
        }

        //Zoom - add in a delay for animation effect
        return mapView.ZoomTo(poly, new TimeSpan(0, 0, 0, 3));
      });
    }

    #endregion


    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelected(System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Selected Features with a timespan
    /// <summary>
    /// Zooms the active map view to the currently selected features.
    /// </summary>
    public static void ZoomToSelected()
    {
      QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;
        //Zoom to the map's selected features.
        mapView.ZoomToSelected(TimeSpan.FromSeconds(3));
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelectedAsync(System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom Async To Selected Features with a timespan
    /// <summary>
    /// Zooms the active map view to the currently selected features.
    /// </summary>
    public static void ZoomToSelectedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;
      //Zoom to the map's selected features.
      mapView.ZoomToSelectedAsync(TimeSpan.FromSeconds(2));
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Zoom To Bookmark by name
    /// <summary>
    /// Zooms the active map view to the specified bookmark by name.
    /// </summary>
    /// <param name="bookmarkName">The name of the bookmark to zoom to. This is case-sensitive and must match the name of an existing bookmark in
    /// the active map.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful; otherwise, <see langword="false"/> if the bookmark was not found or no active map view
    /// is available.</returns>
    public static Task<bool> ZoomToBookmark(string bookmarkName)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Get the first bookmark with the given name.
        var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
        if (bookmark == null)
          return false;

        //Zoom the view to the bookmark.
        return mapView.ZoomTo(bookmark);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Zoom Async To Bookmark by name
    /// <summary>
    /// Zooms the active map view to the specified bookmark by name.
    /// </summary>
    /// <param name="bookmarkName">The name of the bookmark to zoom to. This is case-sensitive and must match the name of an existing bookmark in
    /// the active map.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation succeeds;  otherwise, <see langword="false"/> if the bookmark is not found or there is no active map
    /// view.</returns>
    public static async Task<bool> ZoomToBookmarkAsync(string bookmarkName)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Get the first bookmark with the given name.
      var bookmark = await QueuedTask.Run(() => mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName));
      if (bookmark == null)
        return false;

      //Zoom the view to the bookmark.
      return await mapView.ZoomToAsync(bookmark, TimeSpan.FromSeconds(2));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Visible Layers
    /// <summary>
    /// Zooms the active map view to include all visible layers in the map.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful; otherwise, <see langword="false"/>.</returns>
    public static Task<bool> ZoomToAllVisibleLayersAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom to all visible layers in the map.
        var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
        return mapView.ZoomTo(visibleLayers);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Selected Layers
    /// <summary>
    /// Zooms the active map view to the layers currently selected in the Table of Contents (TOC).
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"> if the zoom
    /// operation succeeds; otherwise, <see langword="false">.</returns>
    public static Task<bool> ZoomToTOCSelectedLayersAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the selected layers in the TOC
      var selectedLayers = mapView.GetSelectedLayers();
      return mapView.ZoomToAsync(selectedLayers);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.BasicFeatureLayer,System.Collections.Generic.IEnumerable{System.Int64},System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom to ObjectIDs
    /// <summary>
    /// Zooms the active map view to the specified object IDs within the given feature layer.
    /// </summary>
    /// <param name="objectIDs">A collection of object IDs to zoom to. If <see langword="null"/> or empty, all features in the layer will be
    /// considered.</param>
    /// <param name="featureLayer">The feature layer containing the features to zoom to. This parameter cannot be <see langword="null"/>.</param>
    /// <param name="queryFilter">An optional query filter to limit the features considered. If <see langword="null"/>, all features in the layer will
    /// be included.</param>
    public static void ZoomToObjectIDs(IEnumerable<long> objectIDs, FeatureLayer featureLayer, QueryFilter queryFilter)
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
    /// <summary>
    /// Pans the active map view to the specified extent.
    /// </summary>
    /// <param name="xMin">The minimum X-coordinate of the extent.</param>
    /// <param name="yMin">The minimum Y-coordinate of the extent.</param>
    /// <param name="xMax">The maximum X-coordinate of the extent.</param>
    /// <param name="yMax">The maximum Y-coordinate of the extent.</param>
    /// <param name="spatialReference">The spatial reference of the specified extent. This must match the spatial reference of the active map view.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the map view was
    /// successfully panned; otherwise, <see langword="false"/> if no active map view is available.</returns>
    public static Task<bool> PanToExtent(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Pan the view to a given extent.
        var envelope = ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);
        return mapView.PanTo(envelope);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
    #region Pan Async To an Extent 
    /// <summary>
    /// Pans the active map view to the specified extent.
    /// </summary>
    /// <param name="xMin">The minimum X-coordinate of the extent.</param>
    /// <param name="yMin">The minimum Y-coordinate of the extent.</param>
    /// <param name="xMax">The maximum X-coordinate of the extent.</param>
    /// <param name="yMax">The maximum Y-coordinate of the extent.</param>
    /// <param name="spatialReference">The spatial reference of the extent. Must match the spatial reference of the map view.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the pan
    /// operation succeeds; otherwise, <see langword="false"/>.</returns>
    public static Task<bool> PanToExtentAsync(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Create the envelope
      var envelope = ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);

      //Pan the view to a given extent.
      return mapView.PanToAsync(envelope, TimeSpan.FromSeconds(2));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelected(System.Nullable{System.TimeSpan})
    #region Pan To Selected Features
    /// <summary>
    /// Pans the active map view to the currently selected features.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the operation
    /// succeeds;  otherwise, <see langword="false"/> if there is no active map view or no selected features to pan to.</returns>
    public static Task<bool> PanToSelected()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Pan to the map's selected features.
        return mapView.PanToSelected();
      });
    }
    #endregion
    
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelectedAsync(System.Nullable{System.TimeSpan})
    #region Pan AsyncTo Selected Features
    /// <summary>
    /// Pans the active map view to the currently selected features.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the operation
    /// succeeds; otherwise, <see langword="false"/> if there is no active map view or the operation fails.</returns>

    public static Task<bool> PanToSelectedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Pan to the map's selected features.
      return mapView.PanToSelectedAsync(TimeSpan.FromSeconds(2));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Pan To Bookmark 
    /// <summary>
    /// Pans the active map view to the specified bookmark by name.
    /// </summary>
    /// <param name="bookmarkName">The name of the bookmark to pan to. This is case-sensitive and must match the name of an existing bookmark in
    /// the active map.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the map view
    /// successfully panned to the bookmark;  otherwise, <see langword="false"/> if the bookmark was not found or no
    /// active map view exists.</returns>
    public static Task<bool> PanToBookmark(string bookmarkName)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Get the first bookmark with the given name.
        var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
        if (bookmark == null)
          return false;

        //Pan the view to the bookmark.
        return mapView.PanTo(bookmark);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Pan To Bookmark 
    /// <summary>
    /// Pans the active map view to the specified bookmark by name.
    /// </summary>
    /// <param name="bookmarkName">The name of the bookmark to pan to. This is case-sensitive and must match an existing bookmark in the active
    /// map.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the map view
    /// successfully panned to the bookmark; otherwise, <see langword="false"/> if the bookmark was not found or no
    /// active map view exists.</returns>
    public static async Task<bool> PanToBookmarkAsync(string bookmarkName)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Get the first bookmark with the given name.
      var bookmark = await QueuedTask.Run(() => mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName));
      if (bookmark == null)
        return false;

      //Pan the view to the bookmark.
      return await mapView.PanToAsync(bookmark, TimeSpan.FromSeconds(2));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
    #region Pan To Visible Layers 
    /// <summary>
    /// Pans the active map view to include all visible layers in the map.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the operation
    /// successfully pans to the visible layers; otherwise, <see langword="false"/> if there is no active map view.</returns>
    public static Task<bool> PanToAllVisibleLayersAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Pan to all visible layers in the map.
        var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
        return mapView.PanTo(visibleLayers);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
    #region Pan To Selected Layers Asynchronous
    /// <summary>
    /// Pans the active map view to the layers currently selected in the Table of Contents (TOC).
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the operation
    /// successfully pans to the selected layers; otherwise, <see langword="false"/> if no active map view is available.</returns>
    public static Task<bool> PanToTOCSelectedLayersAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Pan to the selected layers in the TOC
      var selectedLayers = mapView.GetSelectedLayers();
      return mapView.PanToAsync(selectedLayers);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.Camera
    // cref: ArcGIS.Desktop.Mapping.Camera
    // cref: ArcGIS.Desktop.Mapping.Camera.Heading
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Camera, System.TimeSpan)
    #region Rotate the map view
    /// <summary>
    /// Rotates the active map view to the specified heading.
    /// </summary>
    /// <param name="heading">The desired heading angle, in degrees, to rotate the map view. Valid values are typically between 0 and 360.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the rotation was
    /// successful; otherwise, <see langword="false"/> if no active map view is available.</returns>
    public static Task<bool> RotateView(double heading)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Get the camera for the view, adjust the heading and zoom to the new camera position.
      var camera = mapView.Camera;
      camera.Heading = heading;
      return mapView.ZoomToAsync(camera, TimeSpan.Zero);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.Camera
    // cref: ArcGIS.Desktop.Mapping.Camera
    // cref: ArcGIS.Desktop.Mapping.Camera.Heading
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Camera, System.TimeSpan)
    #region Rotate the map view
    public static Task<bool> RotateViewAsync(double heading)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Get the camera for the view, adjust the heading and zoom to the new camera position.
        var camera = mapView.Camera;
        camera.Heading = heading;
        return mapView.ZoomTo(camera, TimeSpan.Zero);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.MapView.Extent
    #region Expand Extent 
    /// <summary>
    /// Expands the current extent of the active map view by the specified horizontal and vertical ratios.
    /// </summary>
    /// <param name="dx">The horizontal expansion ratio. Must be greater than 0 to expand the extent.</param>
    /// <param name="dy">The vertical expansion ratio. Must be greater than 0 to expand the extent.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the extent was
    /// successfully expanded; otherwise, <see langword="false"/> if there is no active map view or the operation fails.</returns>
    public static Task<bool> ExpandExtentAsync(double dx, double dy)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Expand the current extent by the given ratio.
        var extent = mapView.Extent;
        var newExtent = ArcGIS.Core.Geometry.GeometryEngine.Instance.Expand(extent, dx, dy, true);
        return mapView.ZoomTo(newExtent);
      });
    }
    #endregion

  }
}
