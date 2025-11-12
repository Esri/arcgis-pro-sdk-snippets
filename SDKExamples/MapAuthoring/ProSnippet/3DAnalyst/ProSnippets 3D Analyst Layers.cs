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
using ArcGIS.Core.Data.Analyst3D;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProSnippets.MapAuthoringSnippets.ThreeDAnalyst
{ 
  /// <summary>
  /// Provides methods for working with 3D Analyst layers in ArcGIS Pro, including TIN, Terrain, and LAS dataset layers.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with 3D Layers in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsMapAuthoring
  {

    /// <summary>
    /// Demonstrates various operations and functionalities for working with 3D Analyst layers in ArcGIS Pro.
    /// </summary>
    /// <remarks>This method provides examples of how to retrieve, create, and manipulate different types of
    /// 3D layers such as TIN, Terrain, and LAS dataset layers. It includes operations for creating layers from
    /// datasets, applying renderers, searching for nodes, edges, and points, and performing spatial analyses like line
    /// of sight and surface interpolation. The method is intended to be run within a QueuedTask context in ArcGIS
    /// Pro.</remarks>
    public static async void ProSnippets3DAnalystLayers()
    {
     
      #region ignore - Variable initialization
      Map map = MapView.Active.Map;

      TinLayer tinLayer = map.GetLayersAsFlattenedList().OfType<TinLayer>().FirstOrDefault();
      TerrainLayer terrainLayer = map.GetLayersAsFlattenedList().OfType<TerrainLayer>().FirstOrDefault();
      LasDatasetLayer lasDatasetLayer = map.GetLayersAsFlattenedList().OfType<LasDatasetLayer>().FirstOrDefault();
      SurfaceLayer surfaceLayer = map.GetLayersAsFlattenedList().OfType<SurfaceLayer>().FirstOrDefault();

      TinDataset tinDataset = null;
      Terrain terrain = null;
      LasDataset lasDataset = null;

      CIMPointSymbol nodeSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 2.0);
      CIMLineSymbol lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2.0, SimpleLineStyle.Solid);
      CIMLineSymbol hardEdgeSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2.0, SimpleLineStyle.Solid);
      CIMLineSymbol softEdgeSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2.0, SimpleLineStyle.Solid);
      CIMLineSymbol outsideEdgeSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2.0, SimpleLineStyle.Solid);
      CIMLineSymbol contourLineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2.0, SimpleLineStyle.Solid);
      CIMLineSymbol indexLineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2.0, SimpleLineStyle.Solid);
      CIMPolygonSymbol polySymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB);

      Envelope envelope = EnvelopeBuilderEx.CreateEnvelope();
      MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint();
      Polyline polyline = PolylineBuilderEx.CreatePolyline();
      Polygon polygon = PolygonBuilderEx.CreatePolygon();

      double x = 0.0, 
             y = 0.0;
      #endregion
      #region ProSnippet Group: Layer Methods for TIN, Terrain, LasDataset
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinLayer
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer
      #region Retrieve layers
      {
        // find the first TIN layer
        tinLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TinLayer>().FirstOrDefault();

        // find the first Terrain layer
        terrainLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TerrainLayer>().FirstOrDefault();

        // find the first LAS dataset layer
        lasDatasetLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<LasDatasetLayer>().FirstOrDefault();

        // find the first surface layer
        surfaceLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<SurfaceLayer>().FirstOrDefault();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinLayer.GetTinDataset
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition.GetExtent
      // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition.GetSpatialReference
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer.GetTerrain
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetExtent
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetSpatialReference
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetLasDataset
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition.GetExtent
      // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition.GetSpatialReference
      #region Retrieve dataset objects
      {
        //Note: Needs QueuedTask to run
        {
          using (var tin = tinLayer.GetTinDataset())
          {
            using var tinDef = tin.GetDefinition();
            Envelope extent = tinDef.GetExtent();
            SpatialReference sr = tinDef.GetSpatialReference();
          }

          using (terrain = terrainLayer.GetTerrain())
          {
            using var terrainDef = terrain.GetDefinition();
            Envelope extent = terrainDef.GetExtent();
            SpatialReference sr = terrainDef.GetSpatialReference();
          }

          using (lasDataset = lasDatasetLayer.GetLasDataset())
          {
            using var lasDatasetDef = lasDataset.GetDefinition();
            Envelope extent = lasDatasetDef.GetExtent();
            SpatialReference sr = lasDatasetDef.GetSpatialReference();
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      #region Create a TinLayer
      {
        string tinPath = @"d:\Data\Tin\TinDataset";
        var tinURI = new Uri(tinPath);

        var tinCP = new TinLayerCreationParams(tinURI);
        tinCP.Name = "My TIN Layer";
        tinCP.IsVisible = false;
        // Note: Needs QueuedTask to run
        {
          //Create the layer to the TIN
          tinLayer = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP, map);
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.TinDataset)
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      #region Create a TinLayer from a dataset
      {
        var tinCP_ds = new TinLayerCreationParams(tinDataset);
        tinCP_ds.Name = "My TIN Layer";
        tinCP_ds.IsVisible = false;

        //Create the layer to the TIN
        // Note: Needs QueuedTask to run
        {
          var tinLayer_ds = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP_ds, map);
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.TinDataset)
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.RendererDefinitions
      // cref: ArcGIS.Desktop.Mapping.TinRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.ClassificationMethod
      #region Create a TinLayer with renderers
      {
        var tinCP_renderers = new TinLayerCreationParams(tinDataset)
        {
          Name = "My TIN layer",
          IsVisible = true
        };

        // define the node renderer - use defaults
        var node_rd = new TinNodeRendererDefinition();

        // define the face/surface renderer
        var face_rd = new TinFaceClassBreaksRendererDefinition();
        face_rd.ClassificationMethod = ClassificationMethod.NaturalBreaks;
        // accept default color ramp, breakCount

        // set up the renderer dictionary
        var rendererDict = new Dictionary<SurfaceRendererTarget, TinRendererDefinition>
      {
        { SurfaceRendererTarget.Points, node_rd },
        { SurfaceRendererTarget.Surface, face_rd }
      };

        // assign the dictionary to the creation params
        tinCP_renderers.RendererDefinitions = rendererDict;
        // Note: Needs QueuedTask to run
        {
          // create the layer
          var tinLayer_rd = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP_renderers, MapView.Active.Map);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      #region Create a TerrainLayer
      {
        // Note: Needs QueuedTask to run
        {
          string terrainPath = @"d:\Data\Terrain\filegdb_Containing_A_Terrain.gdb\FeatureDataset\Terrain_name";
          var terrainURI = new Uri(terrainPath);

          var terrainCP = new TerrainLayerCreationParams(terrainURI);
          terrainCP.Name = "My Terrain Layer";
          terrainCP.IsVisible = false;
          // Note: Needs QueuedTask to run
          {
            //Create the layer to the terrain
            terrainLayer = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP, map);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.Terrain)
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      #region Create a TerrainLayer from a dataset
      {
        var terrainCP_ds = new TerrainLayerCreationParams(terrain);
        terrainCP_ds.Name = "My Terrain Layer";
        terrainCP_ds.IsVisible = true;

        //Create the layer to the terrain
        // Note: Needs QueuedTask to run
        {
          var terrainLayer_ds = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP_ds, map);
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.Terrain)
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.RendererDefinitions
      // cref: ArcGIS.Desktop.Mapping.TinRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition.#ctor
      #region Create a TerrainLayer with renderers
      {
        var terrainCP_renderers = new TerrainLayerCreationParams(terrain);
        terrainCP_renderers.Name = "My LAS Layer";
        terrainCP_renderers.IsVisible = true;

        // define the edge type renderer - use defaults
        var edgeRD = new TinBreaklineRendererDefinition();

        // define the face/surface renderer
        var faceRD = new TinFaceClassBreaksRendererDefinition();
        faceRD.ClassificationMethod = ClassificationMethod.NaturalBreaks;
        // accept default color ramp, breakCount

        // define the dirty area renderer - use defaults
        var dirtyAreaRD = new TerrainDirtyAreaRendererDefinition();

        // add renderers to dictionary
        var t_dict = new Dictionary<SurfaceRendererTarget, TinRendererDefinition>();
        t_dict.Add(SurfaceRendererTarget.Edges, edgeRD);
        t_dict.Add(SurfaceRendererTarget.Surface, faceRD);
        t_dict.Add(SurfaceRendererTarget.DirtyArea, dirtyAreaRD);

        // assign dictionary to creation params
        terrainCP_renderers.RendererDefinitions = t_dict;
        // Note: Needs QueuedTask to run
        {
          //Create the layer to the terrain
          var terrainLayer_rd = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP_renderers, map);
        }
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region Create a LasDatasetLayer
      {
        string lasPath = @"d:\Data\LASDataset.lasd";
        var lasURI = new Uri(lasPath);

        var lasCP = new LasDatasetLayerCreationParams(lasURI);
        lasCP.Name = "My LAS Layer";
        lasCP.IsVisible = false;
        // Note: Needs QueuedTask to run
        {
          //Create the layer to the LAS dataset
          lasDatasetLayer = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP, map);
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.LasDataset)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region Create a LasDatasetLayer from a LasDataset
      {
        if (lasDataset == null)
          throw new ArgumentNullException(nameof(lasDataset));

        var lasCP_ds = new LasDatasetLayerCreationParams(lasDataset);
        lasCP_ds.Name = "My LAS Layer";
        lasCP_ds.IsVisible = false;
        // Note: Needs QueuedTask to run
        {
          //Create the layer to the LAS dataset
          var lasDatasetLayer_ds = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP_ds, map);
        }
      }
      #endregion


      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.LasDataset)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.RendererDefinitions
      // cref: ArcGIS.Desktop.Mapping.TinRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion
      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region Create a LasDatasetLayer with renderers
      {
        var lasCP_renderers = new LasDatasetLayerCreationParams(lasDataset);
        lasCP_renderers.Name = "My LAS Layer";
        lasCP_renderers.IsVisible = false;

        // create a point elevation renderer
        var ptR = new LasStretchRendererDefinition();
        // accept all defaults

        // create a simple edge renderer
        var edgeR = new TinEdgeRendererDefintion();
        // accept all defaults

        // add renderers to dictionary
        var l_dict = new Dictionary<SurfaceRendererTarget, TinRendererDefinition>();
        l_dict.Add(SurfaceRendererTarget.Points, ptR);
        l_dict.Add(SurfaceRendererTarget.Edges, edgeR);

        // assign dictionary to creation params
        lasCP_renderers.RendererDefinitions = l_dict;
        // Note: Needs QueuedTask to run
        {
          //Create the layer to the LAS dataset
          var lasDatasetLayer_rd = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP_renderers, map);
        }
      }
      #endregion

      #region ProSnippet Group: Renderers for TinLayer, TerrainLayer, LasDatasetLayer
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetRenderers
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetRenderersAsDictionary
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Get Renderers
      {
        // Note: Needs QueuedTask to run
        {
          // get the list of renderers
          IReadOnlyList<CIMTinRenderer> renderers = surfaceLayer.GetRenderers();

          // get the renderers as a dictionary
          Dictionary<SurfaceRendererTarget, CIMTinRenderer> dict = surfaceLayer.GetRenderersAsDictionary();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Label
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Description
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      #region Simple Node Renderer
      {
        // applies to TIN layers only
        var nodeRendererDef = new TinNodeRendererDefinition();
        nodeRendererDef.Description = "Nodes";
        nodeRendererDef.Label = "Nodes";
        nodeRendererDef.SymbolTemplate = nodeSymbol.MakeSymbolReference();

        // Note: Needs QueuedTask to run
        {
          if (tinLayer.CanCreateRenderer(nodeRendererDef))
          {
            CIMTinRenderer renderer = tinLayer.CreateRenderer(nodeRendererDef);
            if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
              tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.BreakCount
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeElevationRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Node Renderer - Equal Breaks
      {
        // applies to TIN layers only
        var equalBreaksNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
        equalBreaksNodeRendererDef.BreakCount = 7;
        // Note: Needs QueuedTask to run
        {
          if (tinLayer.CanCreateRenderer(equalBreaksNodeRendererDef))
          {
            CIMTinRenderer renderer = tinLayer.CreateRenderer(equalBreaksNodeRendererDef);
            if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
              tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeElevationRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Node Renderer - Defined Interval
      {
        // applies to TIN layers only
        var defiendIntervalNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
        defiendIntervalNodeRendererDef.ClassificationMethod = ClassificationMethod.DefinedInterval;
        defiendIntervalNodeRendererDef.IntervalSize = 4;
        defiendIntervalNodeRendererDef.SymbolTemplate = nodeSymbol.MakeSymbolReference();
        // Note: Needs QueuedTask to run
        {
          if (tinLayer.CanCreateRenderer(defiendIntervalNodeRendererDef))
          {
            CIMTinRenderer renderer = tinLayer.CreateRenderer(defiendIntervalNodeRendererDef);
            if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
              tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.StandardDeviationInterval
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeElevationRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Node Renderer - Standard Deviation
      {
        // applies to TIN layers only
        var stdDevNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
        stdDevNodeRendererDef.ClassificationMethod = ClassificationMethod.StandardDeviation;
        stdDevNodeRendererDef.DeviationInterval = StandardDeviationInterval.OneHalf;
        stdDevNodeRendererDef.ColorRamp = ColorFactory.Instance.GetColorRamp("Cyan to Purple");
        // Note: Needs QueuedTask to run
        {
          if (tinLayer.CanCreateRenderer(stdDevNodeRendererDef))
          {
            CIMTinRenderer renderer = tinLayer.CreateRenderer(stdDevNodeRendererDef);
            if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
              tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion
      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Description
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Label
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinEdgeRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Simple Edge Renderer
      {
        // applies to TIN or LAS dataset layers only
        var edgeRendererDef = new TinEdgeRendererDefintion();
        edgeRendererDef.Description = "Edges";
        edgeRendererDef.Label = "Edges";
        edgeRendererDef.SymbolTemplate = lineSymbol.MakeSymbolReference();
        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(edgeRendererDef))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(edgeRendererDef);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.HardEdgeSymbol
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.SoftEdgeSymbol
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.OutsideEdgeSymbol
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.RegularEdgeSymbol
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinBreaklineRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Edge Type Renderer
      {
        var breaklineRendererDef = new TinBreaklineRendererDefinition();
        // use default symbol for regular edge but specific symbols for hard,soft,outside
        breaklineRendererDef.HardEdgeSymbol = hardEdgeSymbol.MakeSymbolReference();
        breaklineRendererDef.SoftEdgeSymbol = softEdgeSymbol.MakeSymbolReference();
        breaklineRendererDef.OutsideEdgeSymbol = outsideEdgeSymbol.MakeSymbolReference();
        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(breaklineRendererDef))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(breaklineRendererDef);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.Label
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.ContourInterval
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.IndexLabel
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.IndexSymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.ContourFactor
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.ReferenceHeight
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinContourRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Contour Renderer
      {
        var contourDef = new TinContourRendererDefinition();

        // now customize with a symbol
        contourDef.Label = "Contours";
        contourDef.SymbolTemplate = contourLineSymbol.MakeSymbolReference();
        contourDef.ContourInterval = 6;

        contourDef.IndexLabel = "Index Contours";
        contourDef.IndexSymbolTemplate = indexLineSymbol.MakeSymbolReference();
        contourDef.ContourFactor = 4;
        contourDef.ReferenceHeight = 7;
        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(contourDef))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(contourDef);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Contours))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Contours);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Simple Face Renderer
      {
        var simpleFaceRendererDef = new TinFaceRendererDefinition();
        simpleFaceRendererDef.SymbolTemplate = polySymbol.MakeSymbolReference();
        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(simpleFaceRendererDef))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(simpleFaceRendererDef);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksAspectRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksAspectRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksAspectRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Aspect Face Renderer 
      {
        var aspectFaceRendererDef = new TinFaceClassBreaksAspectRendererDefinition();
        aspectFaceRendererDef.SymbolTemplate = polySymbol.MakeSymbolReference();
        // accept default color ramp

        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(aspectFaceRendererDef))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(aspectFaceRendererDef);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition.#ctor(ArcGIS.Core.CIM.TerrainDrawCursorType, ArcGIS.Core.CIM.ClassificationMethod, System.Int32, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Slope Face Renderer - Equal Interval
      {
        var slopeFaceClassBreaksEqual = new TinFaceClassBreaksRendererDefinition(TerrainDrawCursorType.FaceSlope);
        // accept default breakCount, symbolTemplate, color ramp
        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(slopeFaceClassBreaksEqual))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(slopeFaceClassBreaksEqual);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.TerrainDrawCursorType
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Slope Face Renderer - Quantile
      {
        var slopeFaceClassBreaksQuantile = new TinFaceClassBreaksRendererDefinition(TerrainDrawCursorType.FaceSlope);
        slopeFaceClassBreaksQuantile.ClassificationMethod = ClassificationMethod.Quantile;
        // accept default breakCount, symbolTemplate, color ramp
        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(slopeFaceClassBreaksQuantile))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(slopeFaceClassBreaksQuantile);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Face Renderer - Equal Interval
      {
        var elevFaceClassBreaksEqual = new TinFaceClassBreaksRendererDefinition();
        // accept default breakCount, symbolTemplate, color ramp
        // Note: Needs QueuedTask to run
        {
          if (surfaceLayer.CanCreateRenderer(elevFaceClassBreaksEqual))
          {
            CIMTinRenderer renderer = surfaceLayer.CreateRenderer(elevFaceClassBreaksEqual);
            if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
              surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition.#ctor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTerrainDirtyAreaRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      #region Dirty Area Renderer 
      {
        var dirtyAreaRendererDef = new TerrainDirtyAreaRendererDefinition();
        // accept default labels, symbolTemplate

        if (terrainLayer == null)
          return;
        // Note: Needs QueuedTask to run
        {
          if (terrainLayer.CanCreateRenderer(dirtyAreaRendererDef))
          {
            CIMTinRenderer renderer = terrainLayer.CreateRenderer(dirtyAreaRendererDef);
            if (terrainLayer.CanSetRenderer(renderer, SurfaceRendererTarget.DirtyArea))
              terrainLayer.SetRenderer(renderer, SurfaceRendererTarget.DirtyArea);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TerrainPointClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TerrainPointClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTerrainPointElevationRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Terrain Point Class Breaks Renderer
      {
        var terrainPointClassBreaks = new TerrainPointClassBreaksRendererDefinition();
        // accept defaults
        // Note: Needs QueuedTask to run
        {
          if (terrainLayer.CanCreateRenderer(terrainPointClassBreaks))
          {
            CIMTinRenderer renderer = terrainLayer.CreateRenderer(terrainPointClassBreaks);
            if (terrainLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
              terrainLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.#ctor(ArcGIS.Desktop.Mapping.LasAttributeType,System.Boolean,ArcGIS.Core.CIM.CIMSymbolReference,System.Double,ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Desktop.Mapping.LasAttributeType
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASUniqueValueRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region LAS Points Classification Unique Value Renderer
      {
        var lasPointsClassificationRendererDef = new LasUniqueValueRendererDefinition(LasAttributeType.Classification);
        // accept the defaults for color ramp, symbolTemplate, symbol scale factor

        if (lasDatasetLayer == null)
          return;
        // Note: Needs QueuedTask to run
        {
          if (lasDatasetLayer.CanCreateRenderer(lasPointsClassificationRendererDef))
          {
            CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsClassificationRendererDef);
            if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
              lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.#ctor(ArcGIS.Desktop.Mapping.LasAttributeType,System.Boolean,ArcGIS.Core.CIM.CIMSymbolReference,System.Double,ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Desktop.Mapping.LasAttributeType
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.ModulateUsingIntensity
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.SymbolScaleFactor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASUniqueValueRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region LAS Points Returns Unique Value Renderer
      {
        var lasPointsReturnsRendererDef = new LasUniqueValueRendererDefinition(LasAttributeType.ReturnNumber);
        lasPointsReturnsRendererDef.ModulateUsingIntensity = true;
        lasPointsReturnsRendererDef.SymbolScaleFactor = 1.0;
        // accept the defaults for color ramp, symbolTemplate
        // Note: Needs QueuedTask to run
        {
          if (lasDatasetLayer.CanCreateRenderer(lasPointsReturnsRendererDef))
          {
            CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsReturnsRendererDef);
            if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
              lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.#ctor(ArcGIS.Core.CIM.LASStretchAttribute,ArcGIS.Core.CIM.LASStretchType,System.Double,ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Core.CIM.LASStretchAttribute
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASStretchRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.StretchType
      // cref: ArcGIS.Core.CIM.LASStretchType
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.NumberOfStandardDeviations
      #region LAS Points Elevation Stretch Renderer
      {
        var elevLasStretchRendererDef = new LasStretchRendererDefinition(ArcGIS.Core.CIM.LASStretchAttribute.Elevation);
        // accept the defaults for color ramp, etc

        if (lasDatasetLayer.CanCreateRenderer(elevLasStretchRendererDef))
        {
          CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(elevLasStretchRendererDef);
          if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
            lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
        }
        // OR use a stretch renderer with stretchType standard Deviations
        var elevLasStretchStdDevRendererDef = new LasStretchRendererDefinition(ArcGIS.Core.CIM.LASStretchAttribute.Elevation);
        elevLasStretchStdDevRendererDef.StretchType = LASStretchType.StandardDeviations;
        elevLasStretchStdDevRendererDef.NumberOfStandardDeviations = 2;
        // accept the defaults for color ramp,  etc
        // Note: Needs QueuedTask to run
        {
          if (lasDatasetLayer.CanCreateRenderer(elevLasStretchStdDevRendererDef))
          {
            CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(elevLasStretchStdDevRendererDef);
            if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
              lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.ClassificationMethod
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition.ModulateUsingIntensity
      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition.SymbolScaleFactor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASPointElevationRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region LAS Points Classified Elevation Renderer
      {
        var lasPointsClassBreaksRendererDef = new LasPointClassBreaksRendererDefinition();
        lasPointsClassBreaksRendererDef.ClassificationMethod = ClassificationMethod.NaturalBreaks;
        lasPointsClassBreaksRendererDef.ModulateUsingIntensity = true;
        // increase the symbol size by a factor
        lasPointsClassBreaksRendererDef.SymbolScaleFactor = 1.0;
        // Note: Needs QueuedTask to run
        {
          if (lasDatasetLayer.CanCreateRenderer(lasPointsClassBreaksRendererDef))
          {
            CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsClassBreaksRendererDef);
            if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
              lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
          }
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.RemoveRenderer(ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Remove an edge renderer
      {
        // Note: Needs QueuedTask to run
        {
          surfaceLayer.RemoveRenderer(SurfaceRendererTarget.Edges);
        }
      }
      #endregion

      #region ProSnippet Group: TIN Layer Searching
      #endregion
      // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchNodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.SuperNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetSuperNodeExtent
      // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.FilterByEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.EdgeType
      // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchTriangles
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.DataElementsOnly
      #region Search for TIN Nodes, Edges, Triangles
      {
        // Note: Needs QueuedTask to run
        {
          // search all "inside" nodes
          using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(null))
          {
            while (nodeCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current;
              //Use the node
            }
          }

          // search "inside" nodes with an extent
          ArcGIS.Core.Data.Analyst3D.TinNodeFilter nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
          nodeFilter.FilterEnvelope = envelope;
          using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(nodeFilter))
          {
            while (nodeCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current;
              //use the node
            }
          }

          // search for super nodes only
          var supernodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
          supernodeFilter.FilterEnvelope = tinLayer.GetTinDataset().GetSuperNodeExtent();
          supernodeFilter.DataElementsOnly = false;
          supernodeFilter.SuperNode = true;
          using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(nodeFilter))
          {
            while (nodeCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current;
              //Use the node
            }
          }

          // search all edges within an extent
          //    this could include outside or edges attached to super nodes depending upon the extent
          ArcGIS.Core.Data.Analyst3D.TinEdgeFilter edgeFilterAll = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
          edgeFilterAll.FilterEnvelope = envelope;
          edgeFilterAll.DataElementsOnly = false;
          using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinLayer.SearchEdges(edgeFilterAll))
          {
            while (edgeCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current;
              //Use the edge
            }
          }

          // search for hard edges in the TIN
          var edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
          edgeFilter.FilterByEdgeType = true;
          edgeFilter.EdgeType = ArcGIS.Core.Data.Analyst3D.TinEdgeType.HardEdge;
          using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinLayer.SearchEdges(edgeFilter))
          {
            while (edgeCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current;
              //Use the edge
            }
          }

          // search for "inside" triangles in an extent
          ArcGIS.Core.Data.Analyst3D.TinTriangleFilter triangleFilter = new ArcGIS.Core.Data.Analyst3D.TinTriangleFilter();
          triangleFilter.FilterEnvelope = envelope;
          triangleFilter.DataElementsOnly = true;
          using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleCursor = tinLayer.SearchTriangles(triangleFilter))
          {
            while (triangleCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = triangleCursor.Current;
              //use the triangle
            }
          }
        }
      }
      #endregion

      #region ProSnippet Group: LAS Dataset Layer Display Filter
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetDisplayFilter
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(ArcGIS.Desktop.Mapping.LasPointDisplayFilterType)
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilterType
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(List<System.Int32>)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(List<ArcGIS.Core.Data.Analyst3D.LasReturnType>)
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.Returns
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.KeyPoints
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.SyntheticPoints
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.NotFlagged
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.WithheldPoints
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(ArcGIS.Desktop.Mapping.LasPointDisplayFilter)
      #region Get and Set Display Filter
      {
        // Note: Needs QueuedTask to run
        {
          // get the current display filter
          LasPointDisplayFilter ptFilter = lasDatasetLayer.GetDisplayFilter();
          // display only ground points
          lasDatasetLayer.SetDisplayFilter(LasPointDisplayFilterType.Ground);

          // display first return points
          lasDatasetLayer.SetDisplayFilter(LasPointDisplayFilterType.FirstReturnPoints);

          // set display filter to a set of classification codes
          List<int> classifications = new List<int>() { 4, 5, 7, 10 };
          lasDatasetLayer.SetDisplayFilter(classifications);

          // set display filter to a set of returns
          List<ArcGIS.Core.Data.Analyst3D.LasReturnType> returns = new List<ArcGIS.Core.Data.Analyst3D.LasReturnType>()
              { ArcGIS.Core.Data.Analyst3D.LasReturnType.ReturnFirstOfMany};
          lasDatasetLayer.SetDisplayFilter(returns);

          // set up a display filter
          var newDisplayFilter = new LasPointDisplayFilter();
          newDisplayFilter.Returns = new List<ArcGIS.Core.Data.Analyst3D.LasReturnType>()
              { ArcGIS.Core.Data.Analyst3D.LasReturnType.ReturnFirstOfMany, ArcGIS.Core.Data.Analyst3D.LasReturnType.ReturnLastOfMany};
          newDisplayFilter.ClassCodes = new List<int>() { 2, 4 };
          newDisplayFilter.KeyPoints = true;
          newDisplayFilter.WithheldPoints = false;
          newDisplayFilter.SyntheticPoints = false;
          newDisplayFilter.NotFlagged = false;
          lasDatasetLayer.SetDisplayFilter(returns);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetActiveSurfaceConstraints 
      // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetActiveSurfaceConstraints(List<System.String>) 
      #region Active Surface Constraints
      {
        // Note: Needs QueuedTask to run
        {
          var activeSurfaceConstraints = lasDatasetLayer.GetActiveSurfaceConstraints();

          // clear all surface constraints (i.e. none are active)
          lasDatasetLayer.SetActiveSurfaceConstraints(null);

          // set all surface constraints active
          using (lasDataset = lasDatasetLayer.GetLasDataset())
          {
            var surfaceConstraints = lasDataset.GetSurfaceConstraints();
            var names = surfaceConstraints.Select(sc => sc.DataSourceName).ToList();
            lasDatasetLayer.SetActiveSurfaceConstraints(names);
          }
        }
      }
      #endregion

      #region ProSnippet Group: LAS Dataset Layer Searching
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SearchPoints(LasPointFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.ClassCodes
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNext
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.Current
      // cref: ArcGIS.Core.Data.Analyst3D.LasPoint
      #region Search for LAS Points
      {
        // Note: Needs QueuedTask to run
        {
          // searching on the LasDatasetLayer will honor any LasPointDisplayFilter
          // search all points
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(null))
          {
            while (ptCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current;
              //Use the point
            }
          }

          // search within an extent
          ArcGIS.Core.Data.Analyst3D.LasPointFilter pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
          pointFilter.FilterGeometry = envelope;
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(pointFilter))
          {
            while (ptCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current;
              //Use the point
            }
          }

          // search within an extent and limited to specific classification codes
          pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
          pointFilter.FilterGeometry = envelope;
          pointFilter.ClassCodes = new List<int> { 4, 5 };
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(pointFilter))
          {
            while (ptCursor.MoveNext())
            {
              using ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current;
              //Use the point
            }
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SearchPoints(LasPointFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNextArray
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
      #region Search using pre initialized arrays
      {
        // Note: Needs QueuedTask to run
        {
          // search all points and process with a set size of array retrieving only coordinates
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(null))
          {
            int count;
            Coordinate3D[] lasPointsRetrieved = new Coordinate3D[10000];
            while (ptCursor.MoveNextArray(lasPointsRetrieved, null, null, null, out count))
            {
              var points = lasPointsRetrieved.ToList();

              // Use the points
            }
          }

          // search within an extent
          // use MoveNextArray retrieving coordinates, fileIndex and pointIds
          ArcGIS.Core.Data.Analyst3D.LasPointFilter filter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
          filter.FilterGeometry = envelope;
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(filter))
          {
            int count;
            Coordinate3D[] lasPointsRetrieved = new Coordinate3D[50000];
            int[] fileIndexes = new int[50000];
            double[] pointIds = new double[50000];
            while (ptCursor.MoveNextArray(lasPointsRetrieved, null, fileIndexes, pointIds, out count))
            {
              var points = lasPointsRetrieved.ToList();
              //Use the points
            }
          }
        }
      }
      #endregion

      #region ProSnippet Group: LAS Dataset Layer Eye Dome Lighting
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.IsEyeDomeLightingEnabled
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.EyeDomeLightingRadius
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.EyeDomeLightingStrength
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetEyeDomeLightingEnabled(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetEyeDomeLightingStrength(System.Double)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetEyeDomeLightingRadius(System.Double)
      #region Eye Dome Lighting
      {
        // Note: Needs QueuedTask to run
        {
          // get current EDL settings
          bool isEnabled = lasDatasetLayer.IsEyeDomeLightingEnabled;
          var radius = lasDatasetLayer.EyeDomeLightingRadius;
          var strength = lasDatasetLayer.EyeDomeLightingStrength;

          // set EDL values
          lasDatasetLayer.SetEyeDomeLightingEnabled(true);
          lasDatasetLayer.SetEyeDomeLightingStrength(65.0);
          lasDatasetLayer.SetEyeDomeLightingRadius(2.0);
        }
      }
      #endregion

      #region ProSnippet Group: LAS Dataset Layer Selection
      #endregion


      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetSelectVisiblePoints(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetSelectableClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetSelectVisiblePoints()
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetSelectableClassCodes()
      #region Selection Settings
      {
        // must be on MCT 

        // configure the LAS selection settings

        // set visible points and a specific set of classification codes selectable
        lasDatasetLayer.SetSelectVisiblePoints(true);
        lasDatasetLayer.SetSelectableClassCodes(new List<int>() { 3, 4, 5 });

        // this example sets all classification codes selectable
        lasDatasetLayer.SetSelectableClassCodes(new List<int>());


        // get the current LAS selection settings
        var canSelectVisible = lasDatasetLayer.GetSelectVisiblePoints();
        var selectableClassCodes = lasDatasetLayer.GetSelectableClassCodes();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.VisiblePoints
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.FilterGeometry
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SelectAsync(ArcGIS.Desktop.Mapping.LasPointSelectionFilter, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
      #region  Select using LasPointSelectionFilter
      {
        // must be on MCT 

        // create the filter
        var filter = new LasPointSelectionFilter();
        // set the filter geometry
        // don't set VisiblePoints, ClassCodes - use the existing layer values
        filter.FilterGeometry = polygon;

        // perform the selection
        var selCount = await lasDatasetLayer.SelectAsync(filter, SelectionCombinationMethod.New);

        // set up a second filter and configure the VisiblePoints, ClassCodes
        // note that the ClassCodes is using only (4,5)
        // whereas the layers setting is (3,4,5)
        var filter2 = new LasPointSelectionFilter();
        filter2.VisiblePoints = true;
        filter2.ClassCodes = new List<int>() { 4, 5 };
        filter2.FilterGeometry = polygon;

        // perform the selection
        selCount = await lasDatasetLayer.SelectAsync(filter2, SelectionCombinationMethod.New);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasPointClusterSelectionFilter
      // cref: ArcGIS.Desktop.Mapping.LasPointClusterSelectionFilter.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasPointClusterSelectionFilter.SearchRadius
      // cref: ArcGIS.Desktop.Mapping.LasPointClusterSelectionFilter.MaximumNumberOfPoints
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.VisiblePoints
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.FilterGeometry
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SelectAsync(ArcGIS.Desktop.Mapping.LasPointSelectionFilter, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
      #region Select using LasPointClusterSelectionFilter 
      {
        // must be on MCT 

        var clusterFilter = new LasPointClusterSelectionFilter();
        clusterFilter.VisiblePoints = true;
        clusterFilter.ClassCodes = new List<int>();  // empty list means all classification codes
        clusterFilter.FilterGeometry = polygon;

        clusterFilter.SearchRadius = 0.5; // meters
        clusterFilter.MaximumNumberOfPoints = 500;

        var clusterSelCount = await lasDatasetLayer.SelectAsync(clusterFilter, SelectionCombinationMethod.New);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasPointPlaneSelectionFilter
      // cref: ArcGIS.Desktop.Mapping.LasPointPlaneSelectionFilter.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasPointPlaneSelectionFilter.ClusteringDistance
      // cref: ArcGIS.Desktop.Mapping.LasPointPlaneSelectionFilter.MaximumDistance
      // cref: ArcGIS.Desktop.Mapping.LasPointPlaneSelectionFilter.PlaneTolerance
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.VisiblePoints
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.FilterGeometry
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SelectAsync(ArcGIS.Desktop.Mapping.LasPointSelectionFilter, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
      #region Select using LasPointPlaneSelectionFilter  
      {
        // must be on MCT 

        var planeFilter = new LasPointPlaneSelectionFilter();
        planeFilter.VisiblePoints = true;
        planeFilter.ClassCodes = new List<int>();  // empty list means all classification codes
        planeFilter.FilterGeometry = polygon;

        planeFilter.ClusteringDistance = 0.3; // meters
        planeFilter.MaximumDistance = 500;
        planeFilter.PlaneTolerance = 0.152; // meters

        var planeSelCount = await lasDatasetLayer.SelectAsync(planeFilter, SelectionCombinationMethod.New);
      }

      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasPointRailSelectionFilter
      // cref: ArcGIS.Desktop.Mapping.LasPointRailSelectionFilter.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasPointRailSelectionFilter.ApplyWindCorrection
      // cref: ArcGIS.Desktop.Mapping.LasPointRailSelectionFilter.SearchRadius
      // cref: ArcGIS.Desktop.Mapping.LasPointRailSelectionFilter.RailThickness
      // cref: ArcGIS.Desktop.Mapping.LasPointRailSelectionFilter.MaximumLength
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.VisiblePoints
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.FilterGeometry
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SelectAsync(ArcGIS.Desktop.Mapping.LasPointSelectionFilter, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
      #region Select using LasPointRailSelectionFilter
      {
        // must be on MCT 

        var railFilter = new LasPointRailSelectionFilter();
        railFilter.VisiblePoints = true;
        railFilter.ClassCodes = new List<int>();  // empty list means all classification codes
        railFilter.FilterGeometry = polygon;

        // configure a few of the properties and accept defaults for others
        railFilter.SearchRadius = 0.5; // meters
        railFilter.RailThickness = 0.2; // meters
        railFilter.MaximumLength = 225; // meters

        var railSelCount = await lasDatasetLayer.SelectAsync(railFilter, SelectionCombinationMethod.New);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasPointPipelineSelectionFilter
      // cref: ArcGIS.Desktop.Mapping.LasPointPipelineSelectionFilter.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasPointPipelineSelectionFilter.ApplyWindCorrection
      // cref: ArcGIS.Desktop.Mapping.LasPointPipelineSelectionFilter.MinimumLength
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.VisiblePoints
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasPointSelectionFilter.FilterGeometry
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SelectAsync(ArcGIS.Desktop.Mapping.LasPointSelectionFilter, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
      #region Select using LasPointPipelineSelectionFilter 
      {
        // must be on MCT 

        var pipelineFilter = new LasPointPipelineSelectionFilter();

        pipelineFilter.VisiblePoints = true;
        pipelineFilter.ClassCodes = new List<int>();  // empty list means all classification codes
        pipelineFilter.FilterGeometry = polygon;

        // configure a few of the properties and accept defaults for others
        pipelineFilter.ApplyWindCorrection = true;
        pipelineFilter.MinimumLength = 25;  // meters

        var pipelineSelCount = await lasDatasetLayer.SelectAsync(pipelineFilter, SelectionCombinationMethod.New);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.HasSelection
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SelectionCount
      #region Determine if Layer has selection
      {
        // get selection count
        var count = lasDatasetLayer.SelectionCount;
        // does the layer have a selection
        var hasSelection = lasDatasetLayer.HasSelection;
      }

      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.ClearSelection()
      #region Clear Selection
      {
        // must be on MCT
        lasDatasetLayer.ClearSelection();

      }
      #endregion

      // nc   exclude from 3.6


      //#region ProSnippet Group: LAS Dataset Layer Classification
      //#endregion

      //  // cref: ArcGIS.Desktop.Mapping.LasClassFlagEditType
      //  // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.CanClassifyLasPoints(LasPointClassificationDescription)
      //  // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.ClassifyLasPoints(LasPointClassificationDescription, System.Boolean)
      //  //#region Classify LAS Points

      //  //// build the LasPointClassificationDescription
      //  //// assign a new classification code to be assigned to the selected LAS points
      //  //// don't alter any of the classification flags
      //  //var edits = new LasPointClassificationDescription();
      //  //edits.ClassCode = 6;

      //  //if (lasDatasetLayer.CanClassifyLasPoints(edits))
      //  //  lasDatasetLayer.ClassifyLasPoints(edits, true);


      //  //// alternatively set up the LasPointClassificationDescription to modify classification flags
      //  //var edits2 = new LasPointClassificationDescription();
      //  //edits2.KeyPoints = LasClassFlagEditType.Set;
      //  //edits2.SyntheticPoints = LasClassFlagEditType.Clear;
      //  //// don't change the OverlapPoints, WithheldPoints flags

      //  //// apply the edit
      //  //if (lasDatasetLayer.CanClassifyLasPoints(edits2))
      //  //  lasDatasetLayer.ClassifyLasPoints(edits2, true);



      //  //// or perform both a classification code and classification flag edit
      //  //var edits3 = new LasPointClassificationDescription();
      //  //edits3.ClassCode = 5;
      //  //edits3.KeyPoints = LasClassFlagEditType.Set;
      //  //edits3.SyntheticPoints = LasClassFlagEditType.Clear;
      //  //// be explicit about not changing the other flags
      //  //edits3.WithheldPoints = LasClassFlagEditType.NoChange;
      //  //edits3.OverlapPoints = LasClassFlagEditType.NoChange;

      //  //// apply the edit
      //  //if (lasDatasetLayer.CanClassifyLasPoints(edits3))
      //  //  lasDatasetLayer.ClassifyLasPoints(edits3, true);

      //  //#endregion
      //}



      #region ProSnippet Group: Line of Sight
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.#ctor
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.ObserverPoint
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.TargetPoint
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.ObserverHeightOffset
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.TargetHeightOffset
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.ObstructionsMultipatchFeatureClass
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.OutputSpatialReference
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanGetLineOfSight(ArcGIS.Desktop.Mapping.LineOfSightParams)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetLineOfSight(ArcGIS.Desktop.Mapping.LineOfSightParams)
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.IsTargetVisibleFromObserverPoint
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.VisibleLine
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.InvisibleLine
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.ObstructionPoint
      #region Get Line of Sight
      {
        tinLayer = null;
        MapPoint observerPoint = null;
        MapPoint targetPoint = null;
        CIMPointSymbol obstructionPointSymbol = null;
        CIMLineSymbol visibleLineSymbol = null;
        CIMLineSymbol invisibleLineSymbol = null;

        var losParams = new LineOfSightParams();
        losParams.ObserverPoint = observerPoint;
        losParams.TargetPoint = targetPoint;

        // add offsets if appropriate
        // losParams.ObserverHeightOffset = observerOffset;
        // losParams.TargetHeightOffset = targerOffset;

        // add obstruction feature class if appropriate (multipatch)
        //losParams.ObstructionsMultipatchFeatureClass = obsFeatureClass;

        // set output spatial reference
        losParams.OutputSpatialReference = MapView.Active.Map.SpatialReference;

        LineOfSightResult results = null;
        try
        {
          // Note: Needs QueuedTask to run
          {
            if (tinLayer.CanGetLineOfSight(losParams))
              results = tinLayer.GetLineOfSight(losParams);
          }
        }
        catch (Exception)
        {
          // handle exception
        }

        if (results != null)
        {
          bool targetIsVisibleFromObserverPoint = results.IsTargetVisibleFromObserverPoint;
          //These properties are not used. They will always be false
          // results.IsTargetVisibleFromVisibleLine;
          // results.IsTargetVisibleFromInvisibleLine;

          if (results.VisibleLine != null)
            MapView.Active.AddOverlay(results.VisibleLine, visibleLineSymbol.MakeSymbolReference());
          if (results.InvisibleLine != null)
            MapView.Active.AddOverlay(results.VisibleLine, invisibleLineSymbol.MakeSymbolReference());
          if (results.ObstructionPoint != null)
            MapView.Active.AddOverlay(results.ObstructionPoint, obstructionPointSymbol.MakeSymbolReference());
        }
      }
      #endregion

      #region ProSnippet Group: TIN Layer Functionalities
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinLayer.GetSurfaceValues(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Desktop.Mapping.SurfaceValues
      // cref: ArcGIS.Desktop.Mapping.SurfaceValues.Elevation
      // cref: ArcGIS.Desktop.Mapping.SurfaceValues.Slope
      // cref: ArcGIS.Desktop.Mapping.SurfaceValues.SlopeDegrees
      // cref: ArcGIS.Desktop.Mapping.SurfaceValues.SlopePercent
      // cref: ArcGIS.Desktop.Mapping.SurfaceValues.Aspect
      // cref: ArcGIS.Desktop.Mapping.SurfaceValues.AspectDegrees
      #region Get Elevation, Slope, Aspect from TIN layer at a location
      {
        // Note: Needs QueuedTask to run
        {
          // get elevation, slope and aspect values
          SurfaceValues values = tinLayer.GetSurfaceValues(mapPoint);
          var elev = values.Elevation;
          var slopeRadians = values.Slope;
          var slopeDegrees = values.SlopeDegrees;
          var slopePercent = values.SlopePercent;
          var aspectRadians = values.Aspect;
          var aspectDegrees = values.AspectDegrees;
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Layer.CanGetZs()
      // cref: ArcGIS.Desktop.Mapping.Layer.GetZs(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult.Status
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult.Geometry
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
      #region Get Z values from a TIN Layer
      {
        // Note: Needs QueuedTask to run
        {
          if (tinLayer.CanGetZs())
          {
            // get z value for a mapPoint
            var zResult = tinLayer.GetZs(mapPoint);
            if (zResult.Status == SurfaceZsResultStatus.Ok)
            {
              // cast to a mapPoint
              var mapPointZ = zResult.Geometry as MapPoint;
              var z = mapPointZ.Z;
            }

            // get z values for a polyline
            zResult = tinLayer.GetZs(polyline);
            if (zResult.Status == SurfaceZsResultStatus.Ok)
            {
              // cast to a Polyline
              var polylineZ = zResult.Geometry as Polyline;
            }
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinLayer.CanInterpolateShape
      // cref: ArcGIS.Desktop.Mapping.TinLayer.InterpolateShape(ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
      // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
      #region Interpolate Shape
      {
        // Note: Needs QueuedTask to run
        {
          ArcGIS.Core.Geometry.Geometry output = null;
          // interpolate z values for a geometry
          if (tinLayer.CanInterpolateShape(polyline))
            output = tinLayer.InterpolateShape(polyline, SurfaceInterpolationMethod.NaturalNeighbor);

          if (output != null)
          {
            // process the output
          }

          // densify the shape before interpolating
          if (tinLayer.CanInterpolateShape(polygon))
            output = tinLayer.InterpolateShape(polygon, SurfaceInterpolationMethod.Linear, 0.01, 0);

          if (output != null)
          {
            // process the output
          }
        }
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.TinLayer.InterpolateShapeVertices(ArcGIS.Core.Geometry.Multipart, ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
      // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
      #region Interpolate Shape vertices
      {
        // Note: Needs QueuedTask to run
        {
          // interpolate z values at the geometry vertices only
          Geometry output = tinLayer.InterpolateShapeVertices(polyline, SurfaceInterpolationMethod.NaturalNeighbor);
          if (output != null)
          {
            // process the output
          }

          // or use a different interpolation method
          output = tinLayer.InterpolateShapeVertices(polyline, SurfaceInterpolationMethod.Linear);
        }
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.TinLayer.InterpolateZ(System.Double, System.Double, ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
      // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
      #region Interpolate Z at an x,y location
      {
        // Note: Needs QueuedTask to run
        {
          // interpolate values at the specified x,y location
          double z = tinLayer.InterpolateZ(x, y, SurfaceInterpolationMethod.NaturalNeighborZNearest);

          // or use a different interpolation method
          z = tinLayer.InterpolateZ(x, y, SurfaceInterpolationMethod.Linear);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinLayer.GetSurfaceLength(ArcGIS.Core.Geometry.Multipart,ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
      // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
      // cref: ArcGIS.Desktop.Mapping.TinLayer.GetSurfaceLength(ArcGIS.Core.Geometry.Multipart,ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod,System.Double,System.Double)
      #region Get 3D length of multipart by interpolating heights
      {
        // Note: Needs QueuedTask to run
        {
          // interpolate heights and calculate the sum of 3D distances between the vertices
          double length3d = tinLayer.GetSurfaceLength(polygon, SurfaceInterpolationMethod.NaturalNeighbor);

          // or use a different interpolation method
          length3d = tinLayer.GetSurfaceLength(polyline, SurfaceInterpolationMethod.NaturalNeighborZNearest);

          // densify the shape before interpolating
          length3d = tinLayer.GetSurfaceLength(polygon, SurfaceInterpolationMethod.NaturalNeighbor, 0.01, 0);
        }
      }
      #endregion
    }
  }
}