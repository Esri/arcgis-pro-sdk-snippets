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
  /// <remarks>This class contains static methods that operate on the active map view.  If no active map view is
  /// available, the methods will return default values (e.g., <see langword="false"/>). Many methods in this class rely
  /// on asynchronous operations and should be awaited where applicable.</remarks>
  public static class ProSnippetsUpdateMapViewExtent
  {
    // cref: ArcGIS.Desktop.Mapping.MapView.HasPreviousCamera
    // cref: ArcGIS.Desktop.Mapping.MapView.PreviousCameraAsync(System.Nullable{System.TimeSpan})
    #region Go To Previous Camera
    /// <summary>
    /// This method zooms to the previous camera position in the active map view's camera history, if available.
    /// </summary>
    /// <returns></returns>
    /// <remarks>This method checks if there is a previous camera position in the active map view's history.
    /// If a previous camera exists, it asynchronously navigates to that position. If no previous camera is
    /// present or if there is no active map view, the method returns <see langword="false"/>.</remarks>
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
    /// This method zooms to the next camera position in the active map view's camera history, if available.
    /// </summary>
    /// <returns></returns>
    /// <remarks>This method checks if there is a next camera position in the active map view's history.
    /// If a next camera exists, it asynchronously navigates to that position. If no next camera is
    /// present or if there is no active map view, the method returns <see langword="false"/>.</remarks>
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
    /// This method zooms the active map view to the full extent of the map.
    /// </summary>
    /// <returns></returns>
    /// <remarks>This method zooms the active map view to the full extent of the map.</remarks>
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
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtentAsync(System.Nullable{System.TimeSpan})
    #region Zoom To Full Extent Async
    /// <summary>
    /// This method asynchronously zooms the active map view to the full extent of the map.
    /// </summary>
    /// <returns></returns>
    /// <remarks>This method asynchronously zooms the active map view to the full extent of the map.</remarks>
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
    /// <remarks>This method must be called within the context of a queued task. If no active map view is
    /// available, the method will return <see langword="false"/>.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation was successful;  otherwise, <see langword="false"/> if there is no active map view.</returns>
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
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixedAsync(System.Nullable{System.TimeSpan})
    #region Fixed Zoom In Async
    /// <summary>
    /// This method asynchronously zooms in the active map view by a fixed amount.
    /// </summary>
    /// <returns></returns>
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
    /// This method zooms out the active map view by a fixed amount.
    /// </summary>
    /// <returns></returns>
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
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixedAsync(System.Nullable{System.TimeSpan})
    #region Fixed Zoom Out Async
    /// <summary>
    /// This method asynchronously zooms out the active map view by a fixed amount.
    /// </summary>
    /// <returns></returns>
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
    /// This method zooms the active map view to a specified extent defined by the given coordinates and spatial reference.
    /// </summary>
    /// <param name="xMin"></param>
    /// <param name="yMin"></param>
    /// <param name="xMax"></param>
    /// <param name="yMax"></param>
    /// <param name="spatialReference"></param>
    /// <returns></returns>
    /// <remarks>This method retrieves the active map view and zooms to the specified extent. If no active map view is
    /// available, the method returns <see langword="false"/>.</remarks>
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
    /// <remarks>This method retrieves the active map view and zooms to a buffered area around the specified
    /// point. If the spatial reference of the buffer does not match the map's spatial reference, the buffer is
    /// projected to match the map.</remarks>
    /// <param name="x">The x-coordinate of the point in the specified spatial reference system.</param>
    /// <param name="y">The y-coordinate of the point in the specified spatial reference system.</param>
    /// <param name="spatialReference">The spatial reference of the input coordinates.</param>
    /// <param name="buffer_size">The buffer size, in the same units as the spatial reference, to apply around the point for zooming.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the zoom
    /// operation succeeds; otherwise, <see langword="false"/>.</returns>
    /// <remarks>This method creates a point at the specified coordinates, buffers it by the given size, and then
    /// zooms the active map view to that buffered area. If the spatial reference of the buffer polygon does not
    /// equal the spatial reference of the active map, it projects the polygon to match the map's spatial reference.
    /// </remarks>
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
    /// <remarks>This method animates the zoom operation over a duration of 3 seconds.  If no map view is
    /// active, the method does nothing.</remarks>
    public static void ZoomToSelected()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;
      QueuedTask.Run(() =>
      {
        mapView.ZoomToSelected(TimeSpan.FromSeconds(3));
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelectedAsync(System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Selected Features Async with a timespan
    /// <summary>
    /// Zooms the active map view to the currently selected features.
    /// </summary>
    /// <remarks>This method retrieves the active map view and performs a zoom operation to fit the selected
    /// features within the view. If no map view is active, the method does nothing.</remarks>
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
    /// <remarks>This method searches for a bookmark with the specified name in the active map's collection of
    /// bookmarks. If a matching bookmark is found, the map view will zoom to that bookmark. If no active map view
    /// exists or the bookmark is not found, the method returns <see langword="false"/>.</remarks>
    /// <param name="bookmarkName">The name of the bookmark to zoom to. This value is case-sensitive.</param>
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
    #region Zoom To Bookmark by name Async
    /// <summary>
    /// This method asynchronously zooms the active map view to the specified bookmark by name.
    /// </summary>
    /// <param name="bookmarkName"></param>
    /// <returns></returns>
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
    /// This method zooms the active map view to include all visible layers in the map.
    /// </summary>
    /// <returns></returns>
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
    /// <remarks>This method operates on the active map view. If no map view is active, the operation does
    /// nothing and returns <see langword="false"/>.</remarks>
    /// <returns>A task that represents the asynchronous zoom operation. The task result is <see langword="true"/> if the zoom
    /// operation was initiated successfully; otherwise, <see langword="false"/>.</returns>
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
    /// <remarks>This method searches the specified feature layer for the provided object IDs and zooms the
    /// active map view to the extent of those objects. If no object IDs are found, the map view will not
    /// change.</remarks>
    /// <param name="objectIDs">A collection of object IDs to which the map view will zoom. This collection must not be null or empty.</param>
    /// <param name="featureLayer">The feature layer containing the objects to zoom to. This parameter must not be null.</param>
    /// <param name="queryFilter">A query filter to refine the search for features. This parameter can be null if no filtering is required.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="objectIDs"/> or <paramref name="featureLayer"/> is null.</exception>
    public static void ZoomToObjectIDs(IEnumerable<long> objectIDs, FeatureLayer featureLayer, QueryFilter queryFilter)
    {
      using (var rowCursor = featureLayer.Search())
      {
        var objectIds = new List<long>();
        while (rowCursor.MoveNext())
        {
          using (var feature = rowCursor.Current as Feature)
          {
            objectIds.Add(feature.GetObjectID());
          }
        }
        if (objectIds.Count > 0)
        {
          MapView.Active.ZoomTo(featureLayer, objectIds, TimeSpan.FromSeconds(2));
        }
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
    #region Pan To an Extent 
    /// <summary>
    /// This method pans the active map view to a specified extent defined by the given coordinates and spatial reference.
    /// </summary>
    /// <remarks>This method creates an envelope using the specified coordinates and spatial reference, and
    /// pans the active map view to that extent over a duration of 2 seconds. If no active map view is available, the
    /// method returns <see langword="false"/> immediately.</remarks>
    /// <param name="xMin">The minimum X-coordinate of the extent.</param>
    /// <param name="yMin">The minimum Y-coordinate of the extent.</param>
    /// <param name="xMax">The maximum X-coordinate of the extent.</param>
    /// <param name="yMax">The maximum Y-coordinate of the extent.</param>
    /// <param name="spatialReference">The spatial reference of the specified extent. This must match the spatial reference of the active map view.</param>
    /// <returns></returns>
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
    #region Pan To an Extent Async
    /// <summary>
    /// Pans the active map view to the specified extent.
    /// </summary>
    /// <remarks>This method creates an envelope using the specified coordinates and spatial reference, and
    /// pans the active map view to that extent over a duration of 2 seconds. If no active map view is available, the
    /// method returns <see langword="false"/> immediately.</remarks>
    /// <param name="xMin">The minimum X-coordinate of the extent.</param>
    /// <param name="yMin">The minimum Y-coordinate of the extent.</param>
    /// <param name="xMax">The maximum X-coordinate of the extent.</param>
    /// <param name="yMax">The maximum Y-coordinate of the extent.</param>
    /// <param name="spatialReference">The spatial reference of the specified extent. This must match the spatial reference of the active map view.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the map view was
    /// successfully panned; otherwise, <see langword="false"/> if no active map view is available.</returns>
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
    /// <remarks>This method retrieves the active map view and pans it to the features currently selected in
    /// the map.  If no map view is active, the method returns <see langword="false"/>.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the operation
    /// succeeds;  otherwise, <see langword="false"/>.</returns>
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
    #region Pan To Selected Features Async
    /// <summary>
    /// Pans the active map view to the currently selected features.
    /// </summary>
    /// <remarks>This method pans the active map view to center on the features currently selected in the map.
    /// If no map view is active, the method returns <see langword="false"/> without performing any action.</remarks>
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
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Pan To Bookmark 
    /// <summary>
    /// This method pans the active map view to the specified bookmark by name.
    /// </summary>
    /// <param name="bookmarkName"></param>
    /// <returns></returns>
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

    #region Pan To Bookmark Async
    /// <summary>
    /// This method asynchronously pans the active map view to the specified bookmark by name.
    /// </summary>
    /// <param name="bookmarkName"></param>
    /// <returns></returns>
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
    /// This method pans the active map view to include all visible layers in the map.
    /// </summary>
    /// <returns></returns>
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
    /// This method pans the active map view to the layers currently selected in the table of contents (TOC).
    /// </summary>
    /// <returns></returns>
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
    #region Rotate the map view Async
    /// <summary>
    /// This method rotates the active map view to the specified heading.
    /// </summary>
    /// <param name="heading"></param>
    /// <returns></returns>
    public static Task<bool> RotateViewAsync(double heading)
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

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Camera, System.TimeSpan)
    #region Rotate the map view
    /// <summary>
    /// This method rotates the active map view to the specified heading.
    /// </summary>
    /// <param name="heading"></param>
    /// <returns></returns>
    public static Task<bool> RotateView(double heading)
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
    /// This method expands the current extent of the active map view by the specified ratios in the x and y directions.
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <returns></returns>
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
