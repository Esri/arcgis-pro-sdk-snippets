using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
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
  /// Provides utility methods for interacting with the active map view in ArcGIS Pro.
  /// </summary>
  /// <remarks>This class contains static methods to retrieve information about the active map view, manipulate map
  /// selections, and manage map view overlays. It is designed to simplify common tasks when working with map views in
  /// ArcGIS Pro.</remarks>

  #region ProSnippet Group: MapView
  #endregion

  public static class ProSnippetsMapView
  {
    // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Activate
    // cref: ArcGIS.Desktop.Mapping.IMapPane
    // cref: ArcGIS.Desktop.Mapping.MapView
    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
    #region Find a MapView by its Caption
    /// <summary>
    /// Finds a MapView by its caption.
    /// </summary>
    /// <remarks>This method searches through the open panes in the ArcGIS Pro application to find a MapView with a specific caption.
    public static void FindMapViewByCaption()
    {
      string mapPaneCaption = "USNationalParks";
      IMapPane mapViewPane = FrameworkApplication.Panes.OfType<IMapPane>().FirstOrDefault((p) => p.Caption == mapPaneCaption);
      MapView mapView = null;
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
    /// <summary>
    /// Determines whether the active map view is in a 3D viewing mode.
    /// </summary>
    /// <remarks>This method checks the viewing mode of the currently active map view. If no map view is
    /// active,  the method returns <see langword="false"/>.</remarks>
    /// <returns><see langword="true"/> if the active map view is in a 3D viewing mode  (<see
    /// cref="ArcGIS.Core.CIM.MapViewingMode.SceneLocal"/> or  <see
    /// cref="ArcGIS.Core.CIM.MapViewingMode.SceneGlobal"/>); otherwise, <see langword="false"/>.</returns>
    public static bool IsView3D()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Return whether the viewing mode is SceneLocal or SceneGlobal
      return mapView.ViewingMode == ArcGIS.Core.CIM.MapViewingMode.SceneLocal ||
             mapView.ViewingMode == ArcGIS.Core.CIM.MapViewingMode.SceneGlobal;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.CanSetViewingMode(ArcGIS.Core.CIM.MapViewingMode)
    // cref: ArcGIS.Desktop.Mapping.MapView.SetViewingModeAsync(ArcGIS.Core.CIM.MapViewingMode)
    #region Set ViewingMode
    /// <summary>
    /// Sets the viewing mode of the active map view to SceneLocal.
    /// </summary>
    /// <remarks>This method checks if the active map view can be set to the SceneLocal viewing mode. If it can, the method
    public static void SetViewingModeToSceneLocal()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Check if the view can be set to SceneLocal and if it can set it.
      if (mapView.CanSetViewingMode(ArcGIS.Core.CIM.MapViewingMode.SceneLocal))
        mapView.SetViewingModeAsync(ArcGIS.Core.CIM.MapViewingMode.SceneLocal);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.LinkMode
    // cref: ArcGIS.Desktop.Mapping.LinkMode
    #region Enable View Linking
    /// <summary>
    /// Enables view linking for the active map view.
    /// </summary>
    /// <remarks>This method sets the view linking mode of the active map view to link both the center and scale.
    public static void EnableViewLinking()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Set the view linking mode to Center and Scale.
      MapView.LinkMode = LinkMode.Center | LinkMode.Scale;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ExportScene3DObjects
    // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat
    // cref: ArcGIS.Desktop.Mapping.STLExportSceneContentsFormat
    #region Export the contents of a scene to an exchange format such as STL.
    /// <summary>
    /// Exports the contents of the active scene to a specified exchange format.
    /// </summary>
    /// <remarks>This method exports the 3D objects in the active scene to a stereolithography (STL) file format. It first checks if the
    /// active view is a local scene before proceeding with the export.</remarks>
    public static void ExportSceneContents()
    {
      // Validate the current active view. Only a local scene can be exported.
      bool CanExportScene3DObjects = MapView.Active?.ViewingMode == MapViewingMode.SceneLocal;
      if (CanExportScene3DObjects)
      {
        //Gets the active map view.
        MapView mapView = MapView.Active;
        // Create a scene content export format, export the scene context as a stereolithography format
        var exportFormat = new ExportSceneContentsFormat()
        {
          Extent = mapView.Extent, // sets Extent property
          FolderPath = @"C:\Temp", // sets FolderPath property
          FileName = "my-3d-objects.stl", //sets FileName property
          IsSingleFileOutput = true // sets whether to export to one single file
        };
        // Export the scene content as 3D objects
        mapView.ExportScene3DObjects(exportFormat);
      }
    }
    #endregion

    //cref: ArcGIS.Desktop.Mapping.MapView.CurrentMapTOCContent
    //cref: ArcGIS.Desktop.Mapping.MapTOCContentType
    #region Get the Active Map TOC content type
    /// <summary>
    /// Retrieves the current Table of Contents (TOC) content type for the active map view.
    /// </summary>
    /// <remarks>This method accesses the <see cref="ArcGIS.Desktop.Mapping.MapView.Active"/> instance and
    /// obtains its <see cref="ArcGIS.Desktop.Mapping.MapView.CurrentMapTOCContent"/> property, which indicates the
    /// current <see cref="ArcGIS.Desktop.Mapping.MapTOCContentType"/> displayed in the TOC for the active map. This can
    /// be used to determine what type of content (such as layers, legends, or other supported types) is currently shown
    /// in the TOC panel.</remarks>
    public static void GetAndSetMapTOCContentType1()
    {
      //Get the current active TOC content type for the map view 
      var mapTOCContentType = MapView.Active.CurrentMapTOCContent;
    }
    #endregion

    //cref: ArcGIS.Desktop.Mapping.MapView.CurrentMapTOCContent
    //cref: ArcGIS.Desktop.Mapping.MapView.CanSetMapTOCContent(ArcGIS.Desktop.Mapping.MapTOCContentType)
    //cref: ArcGIS.Desktop.Mapping.MapView.SetMapTOCContentAsync(ArcGIS.Desktop.Mapping.MapTOCContentType)
    //cref: ArcGIS.Desktop.Mapping.MapTOCContentType
    #region Set the Active Map TOC content type
    /// <summary>
    /// Cycles through the available Table of Contents (TOC) content types for the active map view  and sets the next
    /// valid content type. If the next content type is not valid, it defaults to  the "Drawing Order" content type.
    /// </summary>
    /// <remarks>This method retrieves the current TOC content type for the active map view, increments it to 
    /// the next type, and attempts to set it. If the next type is not supported, the TOC content type  is reset to
    /// "Drawing Order". The operation must be performed on the UI thread.</remarks>
    public static void GetAndSetMapTOCContentType2()
    {
      //Get the current active TOC content type for the map view 
      var mapTOCContentType = (int)MapView.Active.CurrentMapTOCContent;
      //increment to the next tab whatever it is
      mapTOCContentType++;
      //Can we set this type on the TOC?
      if (MapView.Active.CanSetMapTOCContent((MapTOCContentType)mapTOCContentType))
        //Set it - must be on the UI! - No QueuedTask
        MapView.Active.SetMapTOCContentAsync((MapTOCContentType)mapTOCContentType);
      else
      {
        mapTOCContentType = (int)MapTOCContentType.DrawingOrder;
        //Set it - must be on the UI! - No QueuedTask
        MapView.Active.SetMapTOCContentAsync((MapTOCContentType)mapTOCContentType);
      }
    }
    #endregion

  }
}
