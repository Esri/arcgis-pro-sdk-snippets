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

namespace MapExploration.ProSnippets
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

    // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Desktop.Mapping.MapView, System.String)
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Create a new bookmark using the active map view
    /// <summary>
    /// Creates and adds a new bookmark to the active map using the specified name.
    /// </summary>
    /// <param name="name">The name of the bookmark to be created. This value cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the created <see cref="Bookmark"/> if the
    /// operation succeeds; otherwise, <see langword="null"/> if there is no active map view.</returns>

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

    // cref: ArcGIS.Core.CIM.CIMBookmark
    // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
    // cref: ArcGIS.Core.CIM.CIMBookmark.Name
    // cref: ArcGIS.Core.CIM.CIMBookmark.ThumbnailImagePath
    // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Core.CIM.CIMBookmark)
    #region Add New Bookmark from CIMBookmark
    /// <summary>
    /// Asynchronously adds a new bookmark to the active map using the specified camera and name.
    /// </summary>
    /// <param name="camera">The <see cref="Camera"/> object that defines the spatial location, scale, and orientation for the bookmark.</param>
    /// <param name="name">The name of the bookmark to be added. This value cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the newly created <see cref="Bookmark"/>
    /// object,  or <see langword="null"/> if there is no active map view.</returns>
    public static Task<Bookmark> AddBookmarkFromCameraAsync(Camera camera, string name)
    {
      return QueuedTask.Run(() =>
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

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Get the collection of bookmarks for the project
    /// <summary>
    /// Asynchronously retrieves the collection of bookmarks for the current project.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a  <see
    /// cref="ReadOnlyObservableCollection{T}"/> of <see cref="Bookmark"/> objects  representing the bookmarks in the
    /// current project.</returns>

    public static Task<ReadOnlyObservableCollection<Bookmark>> GetProjectBookmarksAsync()
    {
      //Get the collection of bookmarks for the project.
      return QueuedTask.Run(() => Project.Current.GetBookmarks());
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Get Map Bookmarks
    /// <summary>
    /// Asynchronously retrieves the collection of bookmarks for the currently active map.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is a  <see
    /// cref="ReadOnlyObservableCollection{T}"/> of <see cref="Bookmark"/> objects  for the active map, or <see
    /// langword="null"/> if no map view is active.</returns>
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

    // cref: ArcGIS.Desktop.Mapping.Map.MoveBookmark(ArcGIS.Desktop.Mapping.Bookmark,System.Int32)
    #region Move Bookmark to the Top
    /// <summary>
    /// Moves the specified bookmark to the top of the bookmark list in the given map.
    /// </summary>
    /// <param name="map">The map containing the bookmark to move. Cannot be <see langword="null"/>.</param>
    /// <param name="name">The name of the bookmark to move. Case-sensitive. Cannot be <see langword="null"/> or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

    // cref: ArcGIS.Desktop.Mapping.Bookmark.Rename(System.String)
    #region Rename Bookmark
    /// <summary>
    /// Renames the specified bookmark asynchronously.
    /// </summary>
    /// <param name="bookmark">The bookmark to rename. Cannot be <see langword="null"/>.</param>
    /// <param name="newName">The new name for the bookmark. Cannot be <see langword="null"/> or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static Task RenameBookmarkAsync(Bookmark bookmark, string newName)
    {
      return QueuedTask.Run(() => bookmark.Rename(newName));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Map.RemoveBookmark(ArcGIS.Desktop.Mapping.Bookmark)
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Remove bookmark with a given name
    /// <summary>
    /// Removes a bookmark with the specified name from the given map.
    /// </summary>
    /// <param name="map">The map from which the bookmark will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="name">The name of the bookmark to remove. This parameter is case-sensitive and cannot be <see langword="null"/> or
    /// empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>

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

    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.Bookmark.SetThumbnail(System.Windows.Media.Imaging.BitmapSource)
    #region Change the thumbnail for a bookmark
    /// <summary>
    /// Asynchronously sets the thumbnail image for the specified bookmark using an image file from the provided path.
    /// </summary>
    /// <param name="bookmark">The bookmark for which the thumbnail will be set. Cannot be <see langword="null"/>.</param>
    /// <param name="imagePath">The file path to the image to be used as the thumbnail. Must be a valid path to an image file.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static Task SetThumbnailAsync(Bookmark bookmark, string imagePath)
    {
      //Set the thumbnail to an image on disk, i.e. C:\Pictures\MyPicture.png.
      BitmapImage image = new(new Uri(imagePath, UriKind.RelativeOrAbsolute));
      return QueuedTask.Run(() => bookmark.SetThumbnail(image));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Bookmark.Update(ArcGIS.Desktop.Mapping.MapView)
    #region Update Bookmark
    /// <summary>
    /// Updates the specified bookmark to reflect the current state of the active map view.
    /// </summary>
    /// <param name="bookmark">The bookmark to update. This cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

    // cref: ArcGIS.Desktop.Mapping.Bookmark.GetDefinition
    // cref: ArcGIS.Core.CIM.CIMBookmark
    // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
    // cref: ArcGIS.Core.CIM.CIMBookmark.Location
    // cref: ArcGIS.Desktop.Mapping.Bookmark.SetDefinition(ArcGIS.Core.CIM.CIMBookmark)
    #region Update Extent for a Bookmark
    /// <summary>
    /// Updates the extent of the specified bookmark to the given envelope.
    /// </summary>
    /// <param name="bookmark">The bookmark whose extent is to be updated. Cannot be null.</param>
    /// <param name="envelope">The new extent to apply to the bookmark. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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
