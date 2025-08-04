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
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippetsEditing
{
  public static class ProSnippetsSnapping
  {
    public class Snippets
    {

      #region ProSnippet Group: Snapping
      #endregion


      private async void Snapping()
      {
        Map myMap = null;
        FeatureLayer fLayer = null;
        IEnumerable<FeatureLayer> layerList = null;

        // cref: ArcGIS.Desktop.Mapping.Snapping.IsEnabled
        #region Configure Snapping - Turn Snapping on or off

        //enable snapping
        ArcGIS.Desktop.Mapping.Snapping.IsEnabled = true;

        // disable snapping
        ArcGIS.Desktop.Mapping.Snapping.IsEnabled = false;
        #endregion

        // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.SnapMode>)
        // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(ArcGIS.Desktop.Mapping.SnapMode, System.Boolean)
        // cref: ArcGIS.Desktop.Mapping.Snapping.SnapModes
        // cref: ArcGIS.Desktop.Mapping.Snapping.GetSnapMode(ArcGIS.Desktop.Mapping.SnapMode)
        #region Configure Snapping - Application SnapModes

        // set only Point and Edge snapping modes, clear everything else
        //At 2.x - ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(SnapMode.Point, SnapMode.Edge);
        ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(
          new List<SnapMode>() { SnapMode.Point, SnapMode.Edge });

        // clear all snap modes
        //At 2.x - ArcGIS.Desktop.Mapping.Snapping.SetSnapModes();
        ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(null);


        // set snap modes one at a time
        ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.Edge, true);
        ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.End, true);
        ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.Intersection, true);

        // get current snap modes
        var snapModes = ArcGIS.Desktop.Mapping.Snapping.SnapModes;

        // get state of a specific snap mode
        bool isOn = ArcGIS.Desktop.Mapping.Snapping.GetSnapMode(SnapMode.Vertex);

        #endregion

        // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsSnappable
        // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetSnappable(System.Boolean)
        // cref: ARCGIS.CORE.CIM.CIMGEOFEATURELAYERBASE.SNAPPABLE
        #region Configure Snapping - Layer Snappability

        // is the layer snappable?
        bool isSnappable = fLayer.IsSnappable;

        // set snappability for a specific layer - needs to run on the MCT
        await QueuedTask.Run(() =>
        {
          // use an extension method
          fLayer.SetSnappable(true);

          // or use the CIM directly
          //var layerDef = fLayer.GetDefinition() as ArcGIS.Core.CIM.CIMGeoFeatureLayerBase;
          //layerDef.Snappable = true;
          //fLayer.SetDefinition(layerDef);
        });


        // turn all layers snappability off
        layerList = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>();
        await QueuedTask.Run(() =>
        {
          foreach (var layer in layerList)
          {
            layer.SetSnappable(false);
          }
        });
        #endregion

        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.GetSnapMode(ArcGIS.Desktop.Mapping.SnapMode)
        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.SetSnapMode(ArcGIS.Desktop.Mapping.SnapMode, System.Boolean)
        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes
        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.Edge
        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.End
        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.#ctor(System.Boolean)
        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.Vertex
        // cref: ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(ArcGIS.Desktop.Mapping.Layer)
        // cref: ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(IEnumerable{Layer})
        // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(Layer,Boolean)
        // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IEnumerable{Layer},Boolean)
        // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(Layer,LayerSnapModes)
        // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(Layer,SnapMode,Boolean)
        // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IEnumerable{Layer},LayerSnapModes)
        // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IDictionary{Layer,LayerSnapModes},Boolean)      
        // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.Intersection
        #region Configure Snapping - LayerSnapModes

        layerList = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>();

        // configure by layer
        foreach (var layer in layerList)
        {
          // find the state of the snapModes for the layer
          var lsm = ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(layer);
          bool vertexOn = lsm.Vertex;
          // or use 
          vertexOn = lsm.GetSnapMode(SnapMode.Vertex);

          bool edgeOn = lsm.Edge;
          // or use 
          edgeOn = lsm.GetSnapMode(SnapMode.Edge);

          bool endOn = lsm.End;
          // or use 
          endOn = lsm.GetSnapMode(SnapMode.End);

          // update a few snapModes 
          //   turn Vertex off
          lsm.SetSnapMode(SnapMode.Vertex, false);
          // intersections on
          lsm.SetSnapMode(SnapMode.Intersection, true);

          // and set back to the layer
          ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, lsm);


          // assign a single snap mode at once
          ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, SnapMode.Vertex, false);


          // turn ALL snapModes on
          ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, true);
          // turn ALL snapModes off
          ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, false);
        }


        // configure for a set of layers

        // set Vertex, edge, end on for a set of layers, other snapModes false
        var vee = new LayerSnapModes(false)
        {
          Vertex = true,
          Edge = true,
          End = true
        };
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layerList, vee);


        // ensure intersection is on for a set of layers without changing any other snapModes

        // get the layer snapModes for the set of layers
        var dictLSM = ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(layerList);
        foreach (var layer in dictLSM.Keys)
        {
          var lsm = dictLSM[layer];
          lsm.Intersection = true;
        }
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(dictLSM);


        // set all snapModes off for a list of layers
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layerList, false);


        #endregion

        // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapModes
        // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(ArcGIS.Desktop.Mapping.SnapMode, System.Boolean)
        // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IDictionary{Layer,LayerSnapModes},Boolean)      
        // cref: ARCGIS.DESKTOP.MAPPING.FEATURELAYER.SETSNAPPABLE
        #region Configure Snapping - Combined Example

        // interested in only snapping to the vertices of a specific layer of interest and not the vertices of other layers
        //  all other snapModes should be off.

        // snapping must be on
        ArcGIS.Desktop.Mapping.Snapping.IsEnabled = true;

        // turn all application snapModes off
        //At 2.x - ArcGIS.Desktop.Mapping.Snapping.SetSnapModes();
        ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(null);

        // set application snapMode vertex on 
        ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.Vertex, true);

        // ensure layer snapping is on
        await QueuedTask.Run(() =>
        {
          fLayer.SetSnappable(true);
        });

        // set vertex snapping only
        var vertexOnly = new LayerSnapModes(false) { Vertex = true };

        // set vertex only for the specific layer, clearing all others
        var dict = new Dictionary<Layer, LayerSnapModes>();
        dict.Add(fLayer, vertexOnly);
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(dict, true);  // true = reset other layers
        #endregion

        // cref: ArcGIS.Desktop.Mapping.Snapping.GetOptions(ArcGIS.Desktop.Mapping.Map)
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions
        // cref: ArcGIS.Desktop.Mapping.Snapping.SetOptions(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Snapping.SnappingOptions)
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions.IsSnapToSketchEnabled
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions.XYTolerance
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions.IsZToleranceEnabled
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions.ZTolerance
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions.SnapTipDisplayParts
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions.SnapTipColor
        // cref: ArcGIS.Core.CIM.SnapTipDisplayPart
        // cref: ArcGIS.Desktop.Mapping.SnappingOptions.SnapTipDisplayParts
        #region Snap Options

        //Set snapping options via get/set options
        var snapOptions = ArcGIS.Desktop.Mapping.Snapping.GetOptions(myMap);
        //At 2.x - snapOptions.SnapToSketchEnabled = true;
        snapOptions.IsSnapToSketchEnabled = true;
        snapOptions.XYTolerance = 100;
        //At 2.x - snapOptions.ZToleranceEnabled = true;
        snapOptions.IsZToleranceEnabled = true;
        snapOptions.ZTolerance = 0.6;

        //turn on snap tip display parts
        snapOptions.SnapTipDisplayParts = (int)SnapTipDisplayPart.SnapTipDisplayLayer + (int)SnapTipDisplayPart.SnapTipDisplayType;

        //turn off all snaptips
        //snapOptions.SnapTipDisplayParts = (int)SnapTipDisplayPart.SnapTipDisplayNone;

        //turn on layer display only
        //snapOptions.SnapTipDisplayParts = (int)SnapTipDisplayPart.SnapTipDisplayLayer;

        //At 2.x - snapOptions.GeometricFeedbackColor = ColorFactory.Instance.RedRGB;
        snapOptions.SnapTipColor = ColorFactory.Instance.RedRGB;

        ArcGIS.Desktop.Mapping.Snapping.SetOptions(myMap, snapOptions);

        #endregion
      }

    }

    public class CSVData
    {
      public Double X, Y, StopOrder, FacilityID;
    }
  }
}
