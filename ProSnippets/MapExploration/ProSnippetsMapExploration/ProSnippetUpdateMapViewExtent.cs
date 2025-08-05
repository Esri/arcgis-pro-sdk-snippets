using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippetsMapExploration
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
    #region Go To Previous Camera
    // cref: ArcGIS.Desktop.Mapping.MapView.HasPreviousCamera
    // cref: ArcGIS.Desktop.Mapping.MapView.PreviousCameraAsync(System.Nullable{System.TimeSpan})
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

    #region Go To Next Camera
    // cref: ArcGIS.Desktop.Mapping.MapView.HasNextCamera
    // cref: ArcGIS.Desktop.Mapping.MapView.NextCameraAsync(System.Nullable{System.TimeSpan})
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

    #region Zoom To Full Extent 
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtent(System.Nullable{System.TimeSpan})
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

    #region Fixed Zoom In
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixed(System.Nullable{System.TimeSpan})
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

    #region Fixed Zoom Out 
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixed(System.Nullable{System.TimeSpan})
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

    #region Zoom To an Extent 
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
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

    #region Zoom To a Point 
    // cref: ArcGIS.Desktop.Mapping.Map.SpatialReference
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry, System.TimeSpan, System.Boolean)
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


    #region Zoom To Selected Features with a timespan
    //Note: Run within QueuedTask
    //Zoom to the map's selected features.
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelected(System.Nullable{System.TimeSpan},System.Boolean)
    public static void ZoomToSelected()
    {

      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      mapView.ZoomToSelected(TimeSpan.FromSeconds(3));

    }

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelectedAsync(System.Nullable{System.TimeSpan},System.Boolean)
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


    #region Zoom To Bookmark by name
    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
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

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
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

    #region Zoom To Visible Layers
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
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

    #region Zoom To Selected Layers
    // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
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


    #region Zoom to ObjectIDs
    //Execute a query on a layer to get a RowCursor.
    //You can pass in null (to get all the features)
    //or a "ArcGIS.Core.Data.QueryFilter" to filter the features.
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.BasicFeatureLayer,System.Collections.Generic.IEnumerable{System.Int64},System.Nullable{System.TimeSpan},System.Boolean)
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
        #endregion
      }
    }

    #region Pan To an Extent 
    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
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

    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
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

    #region Pan To Selected Features
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelected(System.Nullable{System.TimeSpan})
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

    // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelectedAsync(System.Nullable{System.TimeSpan})
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

    #region Pan To Bookmark 
    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
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

    #region Pan To Visible Layers 
    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
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

    #region Pan To Selected Layers Asynchronous
    // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
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
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Camera, System.TimeSpan)
    #region Rotate the map view

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

    // or use the synchronous method
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
