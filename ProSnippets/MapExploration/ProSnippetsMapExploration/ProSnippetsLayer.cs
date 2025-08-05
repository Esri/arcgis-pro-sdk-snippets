using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProSnippetsMapExploration
{
  /// <summary>
  /// Provides utility methods for interacting with layers in the active map view within the ArcGIS Pro application.
  /// </summary>
  /// <remarks>This class includes methods for selecting, flashing, and managing layers in the Table of Contents
  /// (TOC), as well as interacting with layer properties and table panes. All methods are designed to work with the
  /// active map view and require that a map view is currently open and active in the application.</remarks>
  public static class ProSnippetsLayer
  {
    #region ProSnippet Group: Layers
    #endregion

    #region Select all feature layers in TOC
    // cref: ArcGIS.Desktop.Mapping.Map.Layers
    // cref: ArcGIS.Desktop.Mapping.MapView.SelectLayers(System.Collections.Generic.IReadOnlyCollection<ArcGIS.Desktop.Mapping.Layer>)
    public static void SelectAllFeatureLayersInTOC()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Zoom to the selected layers in the TOC
      var featureLayers = mapView.Map.Layers.OfType<FeatureLayer>();
      mapView.SelectLayers(featureLayers.ToList());
    }
    #endregion

    #region Flash selected features
    // cref: ArcGIS.Desktop.Mapping.Map.GetSelection()
    // cref: ArcGIS.Desktop.Mapping.MapView.FlashFeature(ArcGIS.Desktop.Mapping.SelectionSet)
    public static Task FlashSelectedFeaturesAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;

        //Get the selected features from the map and filter out the standalone table selection.
        var selectedFeatures = mapView.Map.GetSelection();

        //Flash the collection of features.
        mapView.FlashFeature(selectedFeatures);
      });
    }
    #endregion

    #region Check if layer is visible in the current map view
    // cref: ArcGIS.Desktop.Mapping.Layer.IsVisibleInView(ArcGIS.Desktop.Mapping.MapView)
    private static void CheckLayerVisibilityInView(Layer layer)
    {
      var mapView = MapView.Active;
      if (mapView == null) return;
      bool isLayerVisibleInView = layer.IsVisibleInView(mapView);
      if (isLayerVisibleInView)
      {
        //Do Something
      }
    }
    #endregion

    #region Select a layer and open its layer properties page
    // cref: ArcGIS.Desktop.Mapping.MapView.SelectLayers(System.Collections.Generic.IReadOnlyCollection<ArcGIS.Desktop.Mapping.Layer>)
    private static void GetLayerPropertiesDialog()
    {
      // get the layer you want
      var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();

      // select it in the TOC
      List<Layer> layersToSelect = new List<Layer>();
      layersToSelect.Add(layer);
      MapView.Active.SelectLayers(layersToSelect);

      // now execute the layer properties command
      var wrapper = FrameworkApplication.GetPlugInWrapper("esri_mapping_selectedLayerPropertiesButton");
      var command = wrapper as ICommand;
      if (command == null)
        return;

      // execute the command
      if (command.CanExecute(null))
        command.Execute(null);
    }
    #endregion

    #region Clear selection for a specific layer
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.ClearSelection()
    private static void ClearSelection()
    {
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      QueuedTask.Run(() =>
      {
        lyr.ClearSelection();
      });
    }
    #endregion

    #region Display Table pane for Map Member
    // cref: ArcGIS.Desktop.Core.FrameworkExtender.GetMapTableView(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Desktop.Mapping.MapMember)
    // cref: ArcGIS.Core.CIM.CIMTableView.DisplaySubtypeDomainDescriptions
    // cref: ArcGIS.Core.CIM.CIMTableView.SelectionMode
    // cref: ArcGIS.Core.CIM.CIMTableView.ShowOnlyContingentValueFields
    // cref: ArcGIS.Core.CIM.CIMTableView.HighlightInvalidContingentValueFields
    // cref: ArcGIS.Desktop.Core.FrameworkExtender.OpenTablePane(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Core.CIM.CIMMapTableView)
    private static void OpenTablePane()
    {
      var mapMember = MapView.Active.Map.GetLayersAsFlattenedList().OfType<MapMember>().FirstOrDefault();
      //Gets or creates the CIMMapTableView for a MapMember.
      var tableView = FrameworkApplication.Panes.GetMapTableView(mapMember);
      //Configure the table view
      tableView.DisplaySubtypeDomainDescriptions = false;
      tableView.SelectionMode = false;
      tableView.ShowOnlyContingentValueFields = true;
      tableView.HighlightInvalidContingentValueFields = true;
      //Open the table pane using the configured tableView. If a table pane is already open it will be activated.
      //You must be on the UI thread to call this function.
      var tablePane = FrameworkApplication.Panes.OpenTablePane(tableView);
    }
    #endregion
  }
}
