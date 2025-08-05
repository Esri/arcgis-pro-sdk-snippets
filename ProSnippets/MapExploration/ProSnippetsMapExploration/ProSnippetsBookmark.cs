using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProSnippetsMapExploration
{
  /// <summary>
  /// Provides utility methods for managing bookmarks in the active map view.
  /// </summary>
  /// <remarks>This class includes methods to create, retrieve, update, rename, and remove bookmarks in the active map.
  /// </remarks>
  public static class ProSnippetsBookmark
  {
    #region ProSnippet Group: Bookmarks
    #endregion

    #region Create a new bookmark using the active map view
    // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Desktop.Mapping.MapView, System.String)
    // cref: ArcGIS.Desktop.Mapping.Bookmark

    public static Task<Bookmark> AddBookmarkAsync(string name)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Adding a new bookmark using the active view.
        return mapView.Map.AddBookmark(mapView, name);
      });
    }
    #endregion

    #region Add New Bookmark from CIMBookmark
    // cref: ArcGIS.Core.CIM.CIMBookmark
    // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
    // cref: ArcGIS.Core.CIM.CIMBookmark.Name
    // cref: ArcGIS.Core.CIM.CIMBookmark.ThumbnailImagePath
    // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Core.CIM.CIMBookmark)
    public static Task<Bookmark> AddBookmarkFromCameraAsync(Camera camera, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Set properties for Camera
        CIMViewCamera cimCamera = new CIMViewCamera()
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
        var cimBookmark = new CIMBookmark() { Camera = cimCamera, Name = name, ThumbnailImagePath = "" };

        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Add a new bookmark for the active map.
        return mapView.Map.AddBookmark(cimBookmark);
      });
    }
    #endregion

    #region Get the collection of bookmarks for the project
    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark

    public static Task<ReadOnlyObservableCollection<Bookmark>> GetProjectBookmarksAsync()
    {
      //Get the collection of bookmarks for the project.
      return QueuedTask.Run(() => Project.Current.GetBookmarks());
    }
    #endregion

    #region Get Map Bookmarks
    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    public static Task<ReadOnlyObservableCollection<Bookmark>> GetActiveMapBookmarksAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Return the collection of bookmarks for the map.
        return mapView.Map.GetBookmarks();
      });
    }
    #endregion

    #region Move Bookmark to the Top
    // cref: ArcGIS.Desktop.Mapping.Map.MoveBookmark(ArcGIS.Desktop.Mapping.Bookmark,System.Int32)
    public static Task MoveBookmarkToTopAsync(Map map, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Find the first bookmark with the name
        var bookmark = map.GetBookmarks().FirstOrDefault(b => b.Name == name);
        if (bookmark == null)
          return;

        //Remove the bookmark
        map.MoveBookmark(bookmark, 0);
      });
    }
    #endregion

    #region Rename Bookmark
    // cref: ArcGIS.Desktop.Mapping.Bookmark.Rename(System.String)
    public static Task RenameBookmarkAsync(Bookmark bookmark, string newName)
    {
      return QueuedTask.Run(() => bookmark.Rename(newName));
    }
    #endregion

    #region Remove bookmark with a given name
    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Map.RemoveBookmark(ArcGIS.Desktop.Mapping.Bookmark)
    // cref: ArcGIS.Desktop.Mapping.Bookmark

    public static Task RemoveBookmarkAsync(Map map, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Find the first bookmark with the name
        var bookmark = map.GetBookmarks().FirstOrDefault(b => b.Name == name);
        if (bookmark == null)
          return;

        //Remove the bookmark
        map.RemoveBookmark(bookmark);
      });
    }
    #endregion

    #region Change the thumbnail for a bookmark
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.Bookmark.SetThumbnail(System.Windows.Media.Imaging.BitmapSource)

    public static Task SetThumbnailAsync(Bookmark bookmark, string imagePath)
    {
      //Set the thumbnail to an image on disk, ie. C:\Pictures\MyPicture.png.
      BitmapImage image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
      return QueuedTask.Run(() => bookmark.SetThumbnail(image));
    }
    #endregion

    #region Update Bookmark
    // cref: ArcGIS.Desktop.Mapping.Bookmark.Update(ArcGIS.Desktop.Mapping.MapView)
    public static Task UpdateBookmarkAsync(Bookmark bookmark)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;

        //Update the bookmark using the active map view.
        bookmark.Update(mapView);
      });
    }
    #endregion

    #region Update Extent for a Bookmark
    // cref: ArcGIS.Desktop.Mapping.Bookmark.GetDefinition
    // cref: ArcGIS.Core.CIM.CIMBookmark
    // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
    // cref: ArcGIS.Core.CIM.CIMBookmark.Location
    // cref: ArcGIS.Desktop.Mapping.Bookmark.SetDefinition(ArcGIS.Core.CIM.CIMBookmark)
    public static Task UpdateBookmarkExtentAsync(Bookmark bookmark, ArcGIS.Core.Geometry.Envelope envelope)
    {
      return QueuedTask.Run(() =>
      {
        //Get the bookmark's definition
        var bookmarkDef = bookmark.GetDefinition();

        //Modify the bookmark's location
        bookmarkDef.Location = envelope;

        //Clear the camera as it is no longer valid.
        bookmarkDef.Camera = null;

        //Set the bookmark definition
        bookmark.SetDefinition(bookmarkDef);
      });
    }
    #endregion
  }
}
