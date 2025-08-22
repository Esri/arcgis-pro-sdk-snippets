/*

   Copyright 2025 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data.Analyst3D;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippets3DAnalystLayers
  {
    #region ProSnippet Group: Layer Methods for TIN, Terrain, LasDataset
    #endregion
    // cref: ArcGIS.Desktop.Mapping.TinLayer
    // cref: ArcGIS.Desktop.Mapping.TerrainLayer
    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer
    #region Retrieve layers
    /// <summary>
    /// Retrieves the first TIN, Terrain, and LAS dataset layers from the active map.
    /// </summary>
    public static void GetLayers()
    {
      // find the first TIN layer
      var tinLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TinLayer>().FirstOrDefault();

      // find the first Terrain layer
      var terrainLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TerrainLayer>().FirstOrDefault();

      // find the first LAS dataset layer
      var lasDatasetLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<LasDatasetLayer>().FirstOrDefault();

      // find the set of surface layers
      var surfacelayers = MapView.Active.Map.GetLayersAsFlattenedList().OfType<SurfaceLayer>();
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
    /// <summary>
    /// Retrieves the dataset, the extent and spatial reference, from the specified 3D layers.
    /// </summary>
    /// <param name="tinLayer">The <see cref="TinLayer"/> from which the spatial extent and spatial reference will be retrieved.</param>
    /// <param name="terrainLayer">The <see cref="TerrainLayer"/> from which the spatial extent and spatial reference will be retrieved.</param>
    /// <param name="lasDatasetLayer">The <see cref="LasDatasetLayer"/> from which the spatial extent and spatial reference will be retrieved.</param>
    public static void GetDatasetObjects(TinLayer tinLayer, TerrainLayer terrainLayer, LasDatasetLayer lasDatasetLayer)
    {
      QueuedTask.Run(() =>
      {
        using (var tin = tinLayer.GetTinDataset())
        {
          using (var tinDef = tin.GetDefinition())
          {
            Envelope extent = tinDef.GetExtent();
            SpatialReference sr = tinDef.GetSpatialReference();
          }
        }

        using (var terrain = terrainLayer.GetTerrain())
        {
          using (var terrainDef = terrain.GetDefinition())
          {
            Envelope extent = terrainDef.GetExtent();
            SpatialReference sr = terrainDef.GetSpatialReference();
          }
        }

        using (var lasDataset = lasDatasetLayer.GetLasDataset())
        {
          using (var lasDatasetDef = lasDataset.GetDefinition())
          {
            Envelope extent = lasDatasetDef.GetExtent();
            SpatialReference sr = lasDatasetDef.GetSpatialReference();
          }
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.TinLayer
    #region Create a TinLayer
    /// <summary>
    /// Creates a TIN (Triangulated Irregular Network) layer and adds it to the specified map.
    /// </summary>
    /// <param name="map">The map to which the TIN layer will be added.</param>
    public static void SurfaceLayerCreation(Map map)
    {
      string tinPath = @"d:\Data\Tin\TinDataset";
      var tinURI = new Uri(tinPath);

      var tinCP = new TinLayerCreationParams(tinURI);
      tinCP.Name = "My TIN Layer";
      tinCP.IsVisible = false;
      QueuedTask.Run(() =>
      {
        //Create the layer to the TIN
        var tinLayer = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP, map);
      });
    }
    #endregion


    // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
    // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.TinDataset)
    // cref: ArcGIS.Desktop.Mapping.TinLayer
    #region Create a TinLayer from a dataset
    /// <summary>
    /// Creates a TIN layer from the specified TIN dataset and adds it to the given map.
    /// </summary>
    /// <param name="map">The map to which the TIN layer will be added.</param>
    /// <param name="tinDataset">The TIN dataset used to create the TIN layer.</param>
    public static void CreateTinLayerFromDataset(Map map, ArcGIS.Core.Data.Analyst3D.TinDataset tinDataset)
    {
      var tinCP_ds = new TinLayerCreationParams(tinDataset);
      tinCP_ds.Name = "My TIN Layer";
      tinCP_ds.IsVisible = false;

      //Create the layer to the TIN
      QueuedTask.Run(() => { var tinLayer_ds = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP_ds, map); });
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
    /// <summary>
    /// Creates a TIN (Triangulated Irregular Network) layer with specified renderers and adds it to the active map.
    /// </summary>
    /// <param name="tinDataset">The TIN dataset used to create the layer. This dataset must be valid and accessible.</param>
    public static void CreateTinLayerWithRenderer(ArcGIS.Core.Data.Analyst3D.TinDataset tinDataset)
    {
      var tinCP_renderers = new TinLayerCreationParams(tinDataset);
      tinCP_renderers.Name = "My TIN layer";
      tinCP_renderers.IsVisible = true;

      // define the node renderer - use defaults
      var node_rd = new TinNodeRendererDefinition();

      // define the face/surface renderer
      var face_rd = new TinFaceClassBreaksRendererDefinition();
      face_rd.ClassificationMethod = ClassificationMethod.NaturalBreaks;
      // accept default color ramp, breakCount

      // set up the renderer dictionary
      var rendererDict = new Dictionary<SurfaceRendererTarget, TinRendererDefinition>();
      rendererDict.Add(SurfaceRendererTarget.Points, node_rd);
      rendererDict.Add(SurfaceRendererTarget.Surface, face_rd);

      // assign the dictionary to the creation params
      tinCP_renderers.RendererDefinitions = rendererDict;
      QueuedTask.Run(() =>
      {       // create the layer
        var tinLayer_rd = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP_renderers, MapView.Active.Map);
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.TerrainLayer
    #region Create a TerrainLayer
    /// <summary>
    /// Creates a terrain layer in the specified map using predefined parameters.
    /// </summary>
    /// <param name="map">The map to which the terrain layer will be added.</param>
    public static void CreateTerrainLayer(Map map)
    {
      QueuedTask.Run(() =>
      {
        string terrainPath = @"d:\Data\Terrain\filegdb_Containing_A_Terrain.gdb\FeatureDataset\Terrain_name";
        var terrainURI = new Uri(terrainPath);

        var terrainCP = new TerrainLayerCreationParams(terrainURI);
        terrainCP.Name = "My Terrain Layer";
        terrainCP.IsVisible = false;
        QueuedTask.Run(() =>
        {
          //Create the layer to the terrain
          var terrainLayer = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP, map);
        });
      });
    }
    #endregion


    // cref: ArcGIS.Core.Data.Analyst3D.Terrain
    // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.Terrain)
    // cref: ArcGIS.Desktop.Mapping.TerrainLayer
    #region Create a TerrainLayer from a dataset
    /// <summary>
    /// Creates a terrain layer from the specified terrain dataset.
    /// </summary>
    /// <param name="terrain">The terrain dataset used to create the terrain layer. Cannot be null.</param>
    public static void CreateTerrainLayerFromDataset(ArcGIS.Core.Data.Analyst3D.Terrain terrain, Map map)
    {
      var terrainCP_ds = new TerrainLayerCreationParams(terrain);
      terrainCP_ds.Name = "My Terrain Layer";
      terrainCP_ds.IsVisible = true;

      //Create the layer to the terrain
      QueuedTask.Run(() =>
      {
        var terrainLayer_ds = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP_ds, map);
      });
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
    /// <summary>
    /// Creates a terrain layer with predefined renderers and adds it to the specified map.
    /// </summary>
    /// <param name="terrain">The terrain data source used to create the terrain layer.</param>
    /// <param name="map">The map to which the terrain layer will be added.</param>
    public static void CreateTerrainaLayerWithRenderers(ArcGIS.Core.Data.Analyst3D.Terrain terrain, Map map)
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
      QueuedTask.Run(() =>
      {
        //Create the layer to the terrain
        var terrainLayer_rd = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP_renderers, map);
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(System.Uri)
    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
    #region Create a LasDatasetLayer
    /// <summary>
    /// Creates a LAS dataset layer and adds it to the specified map.
    /// </summary>
    /// <param name="map">The map to which the LAS dataset layer will be added.</param>
    public static void CreateLasDatasetLayer(Map map)
    {
      string lasPath = @"d:\Data\LASDataset.lasd";
      var lasURI = new Uri(lasPath);

      var lasCP = new LasDatasetLayerCreationParams(lasURI);
      lasCP.Name = "My LAS Layer";
      lasCP.IsVisible = false;
      QueuedTask.Run(() =>
      {
        //Create the layer to the LAS dataset
        var lasDatasetLayer = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP, map);
      });
    }
    #endregion

    // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.LasDataset)
    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
    #region Create a LasDatasetLayer from a LasDataset
    /// <summary>
    /// Creates a new LAS dataset layer from the specified LAS dataset and adds it to the given map.
    /// </summary>
    /// <param name="map">The map to which the LAS dataset layer will be added.</param>
    /// <param name="lasDataset">The LAS dataset used to create the layer.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="lasDataset"/> is null.</exception>
    public static void CreateLasDatasetLayerFromDataset(Map map, ArcGIS.Core.Data.Analyst3D.LasDataset lasDataset)
    {
      if (lasDataset == null)
        throw new ArgumentNullException(nameof(lasDataset));

      var lasCP_ds = new LasDatasetLayerCreationParams(lasDataset);
      lasCP_ds.Name = "My LAS Layer";
      lasCP_ds.IsVisible = false;
      QueuedTask.Run(() =>
      {
        //Create the layer to the LAS dataset
        var lasDatasetLayer_ds = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP_ds, map);
      });
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
    /// <summary>
    /// Creates a LAS dataset layer with specified renderers and adds it to the provided map.
    /// </summary>
    /// <param name="lasDataset">The LAS dataset to be used for creating the layer. Cannot be null.</param>
    /// <param name="map">The map to which the LAS dataset layer will be added. Cannot be null.</param>
    public static void CreateLasDatasetLayerWithRenderers(ArcGIS.Core.Data.Analyst3D.LasDataset lasDataset, Map map)
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
      QueuedTask.Run(() =>
      {
        //Create the layer to the LAS dataset
        var lasDatasetLayer_rd = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP_renderers, map);
      });
    }
    #endregion

    #region ProSnippet Group: Renderers for TinLayer, TerrainLayer, LasDatasetLayer
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetRenderers
    // cref: ArcGIS.Core.CIM.CIMTinRenderer
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetRenderersAsDictionary
    // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
    #region Get Renderers
    /// <summary>
    /// Retrieves the renderers associated with the specified <see cref="SurfaceLayer"/>.
    /// </summary>
    /// <param name="surfaceLayer">The <see cref="SurfaceLayer"/> for which the renderers are to be retrieved.  This parameter cannot be <see
    /// langword="null"/>.</param>
    public static void GetRenderers(SurfaceLayer surfaceLayer)
    {
      QueuedTask.Run(() => {
        // get the list of renderers
        IReadOnlyList<CIMTinRenderer> renderers = surfaceLayer.GetRenderers();

        // get the renderers as a dictionary
        Dictionary<SurfaceRendererTarget, CIMTinRenderer> dict = surfaceLayer.GetRenderersAsDictionary();
      });
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
    /// <summary>
    /// Creates and applies a simple node renderer to the first TIN layer in the active map.
    /// </summary>
    /// <param name="nodeSymbol">The <see cref="CIMPointSymbol"/> used to define the appearance of the nodes in the renderer.</param>
    public static void CreateSimpleNodeRenderer(TinLayer tinLayer, CIMPointSymbol nodeSymbol)
    {
      // applies to TIN layers only
      var nodeRendererDef = new TinNodeRendererDefinition();
      nodeRendererDef.Description = "Nodes";
      nodeRendererDef.Label = "Nodes";
      nodeRendererDef.SymbolTemplate = nodeSymbol.MakeSymbolReference();

      QueuedTask.Run(() => {
        if (tinLayer.CanCreateRenderer(nodeRendererDef))
        {
          CIMTinRenderer renderer = tinLayer.CreateRenderer(nodeRendererDef);
          if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
            tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
        }
      });
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
    /// <summary>
    /// Configures and applies an elevation node renderer with equal breaks to the specified TIN layer.
    /// </summary>
    /// <param name="tinLayer">The <see cref="TinLayer"> to which the renderer will be applied. Must be a valid TIN layer.</param>
    /// <param name="nodeSymbol">The <see cref="CIMPointSymbol"> used to symbolize the nodes in the renderer. Cannot be <see langword="null"/>.</param>
    public static void CreateEqualBreaksNodeRenderer(TinLayer tinLayer, CIMPointSymbol nodeSymbol)
    {
      // applies to TIN layers only
      var equalBreaksNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
      equalBreaksNodeRendererDef.BreakCount = 7;
      QueuedTask.Run(() => {
        if (tinLayer.CanCreateRenderer(equalBreaksNodeRendererDef))
        {
          CIMTinRenderer renderer = tinLayer.CreateRenderer(equalBreaksNodeRendererDef);
          if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
            tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
        }
      });
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
    /// <summary>
    /// Configures and applies a defined interval node renderer to the specified TIN layer.
    /// </summary>
    /// <param name="tinLayer">The <see cref="TinLayer"> to which the renderer will be applied. Must be a valid TIN layer.</param>
    /// <param name="nodeSymbol">The <see cref="CIMPointSymbol"> used to symbolize the nodes in the renderer. Cannot be <see langword="null"/>.</param>
    public static void CreateDefinedIntervalNodeRenderer(TinLayer tinLayer, CIMPointSymbol nodeSymbol)
    {
      // applies to TIN layers only
      var defiendIntervalNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
      defiendIntervalNodeRendererDef.ClassificationMethod = ClassificationMethod.DefinedInterval;
      defiendIntervalNodeRendererDef.IntervalSize = 4;
      defiendIntervalNodeRendererDef.SymbolTemplate = nodeSymbol.MakeSymbolReference();
      QueuedTask.Run(() => {
        if (tinLayer.CanCreateRenderer(defiendIntervalNodeRendererDef))
        {
          CIMTinRenderer renderer = tinLayer.CreateRenderer(defiendIntervalNodeRendererDef);
          if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
            tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
        }
      });
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
    /// <summary>
    /// Configures and applies a standard deviation-based elevation node renderer to the specified TIN layer.
    /// </summary>
    /// <param name="tinLayer">The TIN layer to which the standard deviation node renderer will be applied. Must be a valid TIN layer.</param>
    public static void CreateStandardDeviationNodeRenderer(TinLayer tinLayer)
    {
      // applies to TIN layers only
      var stdDevNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
      stdDevNodeRendererDef.ClassificationMethod = ClassificationMethod.StandardDeviation;
      stdDevNodeRendererDef.DeviationInterval = StandardDeviationInterval.OneHalf;
      stdDevNodeRendererDef.ColorRamp = ColorFactory.Instance.GetColorRamp("Cyan to Purple");
      QueuedTask.Run(() => {
        if (tinLayer.CanCreateRenderer(stdDevNodeRendererDef))
        {
          CIMTinRenderer renderer = tinLayer.CreateRenderer(stdDevNodeRendererDef);
          if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
            tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
        }
      });
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
    /// <summary>
    /// Configures and applies a simple edge renderer to the specified surface layer.
    /// </summary>
    /// <param name="surfaceLayer">The surface layer to which the edge renderer will be applied. Must be a TIN or LAS dataset layer.</param>
    /// <param name="lineSymbol">The line symbol used to define the appearance of the edges in the renderer.</param>
    public static void CreateSimpleEdgeRenderer(SurfaceLayer surfaceLayer, CIMLineSymbol lineSymbol)
    {
      // applies to TIN or LAS dataset layers only
      var edgeRendererDef = new TinEdgeRendererDefintion();
      edgeRendererDef.Description = "Edges";
      edgeRendererDef.Label = "Edges";
      edgeRendererDef.SymbolTemplate = lineSymbol.MakeSymbolReference();
      QueuedTask.Run(() =>
      {
        if (surfaceLayer.CanCreateRenderer(edgeRendererDef))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(edgeRendererDef);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
        }
      });
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
    /// <summary>
    /// Configures and applies an edge type renderer to the specified <see cref="SurfaceLayer"/>.
    /// </summary>
    /// <param name="surfaceLayer">The surface layer to which the edge type renderer will be applied. Cannot be null.</param>
    /// <param name="hardEdgeSymbol">The symbol used to represent hard edges in the renderer. Cannot be null.</param>
    /// <param name="softEdgeSymbol">The symbol used to represent soft edges in the renderer. Cannot be null.</param>
    /// <param name="outsideEdgeSymbol">The symbol used to represent outside edges in the renderer. Cannot be null.</param>
    public static void CreateEdgeTypeRenderer(SurfaceLayer surfaceLayer, CIMLineSymbol hardEdgeSymbol, CIMLineSymbol softEdgeSymbol, CIMLineSymbol outsideEdgeSymbol)
    {
      var breaklineRendererDef = new TinBreaklineRendererDefinition();
      // use default symbol for regular edge but specific symbols for hard,soft,outside
      breaklineRendererDef.HardEdgeSymbol = hardEdgeSymbol.MakeSymbolReference();
      breaklineRendererDef.SoftEdgeSymbol = softEdgeSymbol.MakeSymbolReference();
      breaklineRendererDef.OutsideEdgeSymbol = outsideEdgeSymbol.MakeSymbolReference();
      QueuedTask.Run(() => {
        if (surfaceLayer.CanCreateRenderer(breaklineRendererDef))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(breaklineRendererDef);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
        }
      });
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
    /// <summary>
    /// Creates and applies a contour renderer to the specified surface layer using the provided line symbols.
    /// </summary>
    /// <param name="surfaceLayer">The surface layer to which the contour renderer will be applied. Must support contour rendering.</param>
    /// <param name="contourLineSymbol">The line symbol used to render standard contour lines. Cannot be null.</param>
    /// <param name="indexLineSymbol">The line symbol used to render index contour lines. Cannot be null.</param>
    public static void CreateContourRenderer(SurfaceLayer surfaceLayer, CIMLineSymbol contourLineSymbol, CIMLineSymbol indexLineSymbol)
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
      QueuedTask.Run(() => {
        if (surfaceLayer.CanCreateRenderer(contourDef))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(contourDef);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Contours))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Contours);
        }
      });
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
    /// <summary>
    /// Creates and applies a simple face renderer to the specified surface layer using the provided polygon symbol.
    /// </summary>
    /// <param name="surfaceLayer">The surface layer to which the renderer will be applied. Must not be <see langword="null"/>.</param>
    /// <param name="polySymbol">The polygon symbol used to define the appearance of the renderer. Must not be <see langword="null"/>.</param>
    public static void CreateSimpleFaceRenderer(SurfaceLayer surfaceLayer, CIMPolygonSymbol polySymbol)
    {
      var simpleFaceRendererDef = new TinFaceRendererDefinition();
      simpleFaceRendererDef.SymbolTemplate = polySymbol.MakeSymbolReference();
      QueuedTask.Run(() => {
        if (surfaceLayer.CanCreateRenderer(simpleFaceRendererDef))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(simpleFaceRendererDef);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
        }
      });
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
    /// <summary>
    /// Configures and applies an aspect face renderer to the specified surface layer.
    /// </summary>
    /// <param name="surfaceLayer">The <see cref="ArcGIS.Desktop.Mapping.SurfaceLayer"/> to which the aspect face renderer will be applied.</param>
    /// <param name="polygonSymbol">The <see cref="ArcGIS.Core.CIM.CIMPolygonSymbol"/> used as the symbol template for the renderer.</param>
    public static void CreateAspectFaceRenderer(SurfaceLayer surfaceLayer, CIMPolygonSymbol polygonSymbol)
    {
      var aspectFaceRendererDef = new TinFaceClassBreaksAspectRendererDefinition();
      aspectFaceRendererDef.SymbolTemplate = polygonSymbol.MakeSymbolReference();
      // accept default color ramp

      QueuedTask.Run(() =>
      {
        if (surfaceLayer.CanCreateRenderer(aspectFaceRendererDef))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(aspectFaceRendererDef);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
        }
      });
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
    /// <summary>
    /// Configures and applies a slope face renderer to the specified surface layer.
    /// </summary>
    /// <param name="surfaceLayer">The <see cref="SurfaceLayer"/> to which the slope face renderer will be applied.</param>
    public static void CreateSlopeFaceRenderer(SurfaceLayer surfaceLayer)
    {
      var slopeFaceClassBreaksEqual = new TinFaceClassBreaksRendererDefinition(TerrainDrawCursorType.FaceSlope);
      // accept default breakCount, symbolTemplate, color ramp
      QueuedTask.Run(() => {
        if (surfaceLayer.CanCreateRenderer(slopeFaceClassBreaksEqual))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(slopeFaceClassBreaksEqual);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
        }
      });
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
    /// <summary>
    /// Configures and applies a slope face renderer to the specified surface layer using a quantile classification
    /// method.
    /// </summary>
    /// <param name="surfaceLayer">The surface layer to which the renderer will be applied. Must not be <see langword="null"/>.</param>
    public static void CreateSlidersFaceRenderer(SurfaceLayer surfaceLayer) {
      var slopeFaceClassBreaksQuantile = new TinFaceClassBreaksRendererDefinition(TerrainDrawCursorType.FaceSlope);
      slopeFaceClassBreaksQuantile.ClassificationMethod = ClassificationMethod.Quantile;
      // accept default breakCount, symbolTemplate, color ramp
      QueuedTask.Run(() =>
      {
        if (surfaceLayer.CanCreateRenderer(slopeFaceClassBreaksQuantile))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(slopeFaceClassBreaksQuantile);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
    // cref: ArcGIS.Core.CIM.ClassificationMethod
    // cref: ArcGIS.Desktop.Mapping.StandardDeviationInterval
    // cref: ArcGIS.Core.CIM.CIMTinRenderer
    // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
    // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
    #region Elevation Face Renderer - Equal Interval
    /// <summary>
    /// Creates and applies an elevation face renderer to the specified surface layer using an equal interval
    /// classification.
    /// </summary>
    /// <param name="surfaceLayer">The surface layer to which the elevation face renderer will be applied. Must not be <see langword="null"/>.</param>
    public static void CreateElevationFaceRenderer(SurfaceLayer surfaceLayer) {

      var elevFaceClassBreaksEqual = new TinFaceClassBreaksRendererDefinition();
      // accept default breakCount, symbolTemplate, color ramp
      QueuedTask.Run(() => {
        if (surfaceLayer.CanCreateRenderer(elevFaceClassBreaksEqual))
        {
          CIMTinRenderer renderer = surfaceLayer.CreateRenderer(elevFaceClassBreaksEqual);
          if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
            surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
        }
      });
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
    #region Dirty Area Renderer 
    /// <summary>
    /// Configures and applies a dirty area renderer to the specified terrain layer.
    /// </summary>
    /// <param name="terrainLayer">The <see cref="TerrainLayer"/> to which the dirty area renderer will be applied. Must not be <see
    /// langword="null"/>.</param>
    public static void CreateDirtyAreaRenderer(TerrainLayer terrainLayer)
    {
    var dirtyAreaRendererDef = new TerrainDirtyAreaRendererDefinition();
    // accept default labels, symbolTemplate

      if (terrainLayer == null)
        return;
      QueuedTask.Run(() => {
        if (terrainLayer.CanCreateRenderer(dirtyAreaRendererDef))
        {
          CIMTinRenderer renderer = terrainLayer.CreateRenderer(dirtyAreaRendererDef);
          if (terrainLayer.CanSetRenderer(renderer, SurfaceRendererTarget.DirtyArea))
            terrainLayer.SetRenderer(renderer, SurfaceRendererTarget.DirtyArea);
        }
      });
     
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
    // applies to Terrain layers only
    /// <summary>
    /// Creates and applies a terrain point class breaks renderer to the specified terrain layer.
    /// </summary>
    /// <param name="terrainLayer">The terrain layer to which the renderer will be applied. Must be a valid terrain layer.</param>
    public static void CreateTerrainPointClassBreaksRenderer(TerrainLayer terrainLayer)
    {
      var terrainPointClassBreaks = new TerrainPointClassBreaksRendererDefinition();
      // accept defaults
      QueuedTask.Run(() => {
        if (terrainLayer.CanCreateRenderer(terrainPointClassBreaks))
        {
          CIMTinRenderer renderer = terrainLayer.CreateRenderer(terrainPointClassBreaks);
          if (terrainLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
            terrainLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
        }
      });
      
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
      // applies to LAS dataset layers only
      /// <summary>
      /// Configures a LAS dataset layer to use a unique value renderer based on point classifications.
      /// </summary>
      /// <param name="lasDatasetLayer">The LAS dataset layer to apply the renderer to. Must not be <see langword="null"/>.</param>
      public static void CreateLASPointsClassificationUVR(LasDatasetLayer lasDatasetLayer)
    {
      var lasPointsClassificationRendererDef = new LasUniqueValueRendererDefinition(LasAttributeType.Classification);
      // accept the defaults for color ramp, symbolTemplate, symbol scale factor

      if (lasDatasetLayer == null)
        return;
      QueuedTask.Run(() => {
        if (lasDatasetLayer.CanCreateRenderer(lasPointsClassificationRendererDef))
        {
          CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsClassificationRendererDef);
          if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
            lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
        }
      });
      
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
    // applies to LAS dataset layers only
    /// <summary>
    /// Configures a LAS dataset layer to use a unique value renderer based on return numbers.
    /// </summary>
    /// <param name="lasDatasetLayer">The LAS dataset layer to which the unique value renderer will be applied. This parameter cannot be null.</param>
    public static void CreateLASPointsReturnsUVR(LasDatasetLayer lasDatasetLayer)
    {
      var lasPointsReturnsRendererDef = new LasUniqueValueRendererDefinition(LasAttributeType.ReturnNumber);
      lasPointsReturnsRendererDef.ModulateUsingIntensity = true;
      lasPointsReturnsRendererDef.SymbolScaleFactor = 1.0;
      // accept the defaults for color ramp, symbolTemplate
      QueuedTask.Run(() => {
        if (lasDatasetLayer.CanCreateRenderer(lasPointsReturnsRendererDef))
        {
          CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsReturnsRendererDef);
          if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
            lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
        }
      });
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
    // applies to LAS dataset layers only
    /// <summary>
    /// Configures and applies an elevation-based stretch renderer to a LAS dataset layer.
    /// </summary>
    /// <param name="lasDatasetLayer">The <see cref="LasDatasetLayer"/> to which the elevation stretch renderer will be applied.</param>
    public static void CreateLASPointsElevationStretchRenderer(LasDatasetLayer lasDatasetLayer)
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
      QueuedTask.Run(() => {
        if (lasDatasetLayer.CanCreateRenderer(elevLasStretchStdDevRendererDef))
        {
          CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(elevLasStretchStdDevRendererDef);
          if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
            lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
        }
      });
      
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
    // applies to LAS dataset layers only
    /// <summary>
    /// Configures and applies a classified elevation renderer to a LAS dataset layer.
    /// </summary>
    /// <param name="lasDatasetLayer">The LAS dataset layer to which the classified elevation renderer will be applied.</param>
    public static void CreateLASPointsClassifiedElevationRenderer(LasDatasetLayer lasDatasetLayer)
    {
      var lasPointsClassBreaksRendererDef = new LasPointClassBreaksRendererDefinition();
      lasPointsClassBreaksRendererDef.ClassificationMethod = ClassificationMethod.NaturalBreaks;
      lasPointsClassBreaksRendererDef.ModulateUsingIntensity = true;
      // increase the symbol size by a factor
      lasPointsClassBreaksRendererDef.SymbolScaleFactor = 1.0;
      QueuedTask.Run(() => {
        if (lasDatasetLayer.CanCreateRenderer(lasPointsClassBreaksRendererDef))
        {
          CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsClassBreaksRendererDef);
          if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
            lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
        }
      });
    }
      #endregion
    // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.RemoveRenderer(ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
    // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
    #region Remove an edge renderer
    /// <summary>
    /// Removes the edge renderer from the first <see cref="SurfaceLayer"/> in the active map.
    /// </summary>
    public static void RemoveEdgeRenderer()
    {
      var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<SurfaceLayer>().FirstOrDefault();
      if (layer == null)
        return;

      QueuedTask.Run(() =>
      {
        layer.RemoveRenderer(SurfaceRendererTarget.Edges);
      });
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
    // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.DataElementsOnly
    // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.SuperNode
    // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchEdges
    // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeCursor
    // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
    // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter
    // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.#ctor
    // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
    // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.DataElementsOnly
    // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.FilterByEdgeType
    // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.EdgeType
    // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchTriangles
    // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleCursor
    // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
    // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter
    // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter.#ctor
    // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
    // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.DataElementsOnly
    #region Seach for TIN Nodes, Edges, Triangles
    /// <summary>
    /// Searches for TIN nodes, edges, and triangles within the specified envelope.
    /// </summary>
    /// <param name="tinLayer">The TIN layer to search.</param>
    /// <param name="envelope">The envelope used for filtering search results.</param>
    public static void TinLayer_Search(TinLayer tinLayer, Envelope envelope)
    {
      QueuedTask.Run( () =>
      {
        // search all "inside" nodes
        using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(null))
        {
          while (nodeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
            {
              //Use the node
            }
          }
        }

        // search "inside" nodes with an extent
        ArcGIS.Core.Data.Analyst3D.TinNodeFilter nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
        nodeFilter.FilterEnvelope = envelope;
        using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(nodeFilter))
        {
          while (nodeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
            {
              //use the node
            }
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
            using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
            {
              //Use the node
            }
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
            using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
            {
              //Use the edge
            }
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
            using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
            {
              //Use the edge
            }
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
            using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = triangleCursor.Current)
            {
              //use the triangle
            }
          }
        }
      });     
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
    /// <summary>
    /// Configures the display filter for a LAS dataset layer to control which points are visible based on specific
    /// criteria.
    /// </summary>
    /// <param name="lasDatasetLayer">The <see cref="LasDatasetLayer"/> instance for which the display filter is being set.</param>
    public static void SetLasDisplayFilter(LasDatasetLayer lasDatasetLayer)
    {
      QueuedTask.Run(() => { 
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
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetActiveSurfaceConstraints 
    // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint
    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetActiveSurfaceConstraints(List<System.String>) 
    #region Active Surface Constraints
    /// <summary>
    /// Activates all surface constraints for the specified LAS dataset layer.
    /// </summary>
    /// <param name="lasDatasetLayer">The <see cref="LasDatasetLayer"/> for which all surface constraints will be activated.</param>
    public static void SetActiveSurfaceContraints(LasDatasetLayer lasDatasetLayer)
    {
      QueuedTask.Run(() => {
        var activeSurfaceConstraints = lasDatasetLayer.GetActiveSurfaceConstraints();

        // clear all surface constraints (i.e. none are active)
        lasDatasetLayer.SetActiveSurfaceConstraints(null);

        // set all surface constraints active
        using (var lasDataset = lasDatasetLayer.GetLasDataset())
        {
          var surfaceConstraints = lasDataset.GetSurfaceConstraints();
          var names = surfaceConstraints.Select(sc => sc.DataSourceName).ToList();
          lasDatasetLayer.SetActiveSurfaceConstraints(names);
        }
      });
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
    /// <summary>
    /// Searches for LAS points within a specified <see cref="LasDatasetLayer"/>.
    /// </summary>
    /// <param name="lasDatasetLayer">The LAS dataset layer to search. Cannot be <c>null</c>.</param>
    /// <param name="envelope">The spatial extent used to filter the search. If <c>null</c>, all points in the layer are searched.</param>
    public static void LasDatasetLayer_Search(LasDatasetLayer lasDatasetLayer, Envelope envelope)
    {
      QueuedTask.Run(() => {
        // searching on the LasDatasetLayer will honor any LasPointDisplayFilter
        // search all points
        using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(null))
        {
          while (ptCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
            {
              //Use the point
            }
          }
        }

        // search within an extent
        ArcGIS.Core.Data.Analyst3D.LasPointFilter pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
        pointFilter.FilterGeometry = envelope;
        using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(pointFilter))
        {
          while (ptCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
            {
              //Use the point
            }
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
            using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
            {
              //Use the point
            }
          }
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SearchPoints(LasPointFilter)
    // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
    // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNextArray
    // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
    // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
    // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
    #region Search using pre initialized arrays
    /// <summary>
    /// Searches for LAS points in a LAS dataset layer using pre-initialized arrays for efficient data retrieval.
    /// </summary>
    /// <param name="lasDatasetLayer">The <see cref="LasDatasetLayer"/> representing the LAS dataset to search.</param>
    /// <param name="envelope">The <see cref="Envelope"/> defining the spatial extent for the filtered search.</param>
    public static void SearchUsingPreInitializedArrays(LasDatasetLayer lasDatasetLayer, Envelope envelope)
    {
      QueuedTask.Run(() => {
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
      });
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
    /// <summary>
    /// Configures Eye Dome Lighting (EDL) settings for the specified LAS dataset layer.
    /// </summary>
    /// <param name="lasDatasetLayer">The <see cref="LasDatasetLayer"/> instance for which Eye Dome Lighting settings will be configured.</param>
    public static void LasDatasetLayer_EDL(LasDatasetLayer lasDatasetLayer)
    {
      QueuedTask.Run(() =>
      {
        // get current EDL settings
        bool isEnabled = lasDatasetLayer.IsEyeDomeLightingEnabled;
        var radius = lasDatasetLayer.EyeDomeLightingRadius;
        var strength = lasDatasetLayer.EyeDomeLightingStrength;

        // set EDL values
        lasDatasetLayer.SetEyeDomeLightingEnabled(true);
        lasDatasetLayer.SetEyeDomeLightingStrength(65.0);
        lasDatasetLayer.SetEyeDomeLightingRadius(2.0);
      });
    }
    #endregion

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
    /// <summary>
    /// Calculates the line of sight between an observer point and a target point, determining visibility and
    /// obstructions.
    /// </summary>
    public static void GetLineOfSight()
    {
      TinLayer tinLayer = null;
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

      // add obstruction feature class if appropriate
      //losParams.ObstructionsMultipatchFeatureClass = obsFeatureClass;   // multipatch

      // set output spatial reference
      losParams.OutputSpatialReference = MapView.Active.Map.SpatialReference;

      LineOfSightResult results = null;
      try
      {
        QueuedTask.Run(() => {
          if (tinLayer.CanGetLineOfSight(losParams))
            results = tinLayer.GetLineOfSight(losParams);
        });       
      }
      catch (Exception ex)
      {
        // log exception message
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
    /// <summary>
    /// Gets elevation, slope, and aspect values from a TIN layer at a specified location.
    /// </summary>
    /// <param name="tinLayer">The TIN layer to query.</param>
    /// <param name="mapPoint">The location to retrieve surface values from.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task GetElevationSlopeAspectFromTIN(TinLayer tinLayer, MapPoint mapPoint)
    {
      await QueuedTask.Run(() =>
      {
        // get elevation, slope and aspect values
        SurfaceValues values = tinLayer.GetSurfaceValues(mapPoint);
        var elev = values.Elevation;
        var slopeRadians = values.Slope;
        var slopeDegrees = values.SlopeDegrees;
        var slopePercent = values.SlopePercent;
        var aspectRadians = values.Aspect;
        var aspectDegrees = values.AspectDegrees;
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.Layer.CanGetZs()
    // cref: ArcGIS.Desktop.Mapping.Layer.GetZs(ArcGIS.Core.Geometry.Geometry)
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult.Status
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult.Geometry
    // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
    #region Get Z values from a TIN Layer
    /// <summary>
    /// Gets Z values from a TIN layer for a map point and polyline.
    /// </summary>
    /// <param name="tinLayer">The TIN layer to query.</param>
    /// <param name="mapPoint">The map point to retrieve Z value for.</param>
    /// <param name="polyline">The polyline to retrieve Z values for.</param>
    public static async void GetZValues(TinLayer tinLayer, MapPoint mapPoint, Polyline polyline)
    {
      await QueuedTask.Run(() =>
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
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.TinLayer.CanInterpolateShape
    // cref: ArcGIS.Desktop.Mapping.TinLayer.InterpolateShape(ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
    // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
    #region Interpolate Shape
    /// <summary>
    /// Interpolates Z values for the specified geometry using a TIN layer.
    /// </summary>
    /// <param name="tinLayer">The TIN layer to use for interpolation.</param>
    /// <param name="mapPoint">The map point to interpolate.</param>
    /// <param name="polyline">The polyline to interpolate.</param>
    /// <param name="polygon">The polygon to interpolate.</param>
    public static async void Interpolation(TinLayer tinLayer, MapPoint mapPoint, Polyline polyline, Polygon polygon)
    {
        await QueuedTask.Run(() =>
        {
          Geometry output = null;
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

        });
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.TinLayer.InterpolateShapeVertices(ArcGIS.Core.Geometry.Multipart, ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
    // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
    #region Interpolate Shape vertices
    /// <summary>
    /// Interpolates the Z-values of the vertices in the specified polyline using the given TIN surface.
    /// </summary>
    /// <param name="tinLayer">The TIN surface layer used for interpolation. Must not be null.</param>
    /// <param name="polyline">The polyline whose vertex Z-values will be interpolated. Must not be null.</param>

    public async static void InterpolateShapeVertices(TinLayer tinLayer, Polyline polyline)
    {
        await QueuedTask.Run(() =>
        {
          // interpolate z values at the geometry vertices only
          Geometry output = tinLayer.InterpolateShapeVertices(polyline, SurfaceInterpolationMethod.NaturalNeighbor);
          if (output != null)
          {
            // process the output
          }

          // or use a different interpolation method
          output = tinLayer.InterpolateShapeVertices(polyline, SurfaceInterpolationMethod.Linear);
        });
    }
    #endregion

      
        // cref: ArcGIS.Desktop.Mapping.TinLayer.InterpolateZ(System.Double, System.Double, ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
        // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
        #region Interpolate Z at an x,y location
    /// <summary>
    /// Interpolates the Z value at a specified X, Y location using the given <see cref="TinLayer"/>.
    /// </summary>
    /// <param name="tinLayer">The <see cref="TinLayer"/> used for interpolation. Must not be null.</param>
    /// <param name="x">The X coordinate of the location where the Z value is interpolated.</param>
    /// <param name="y">The Y coordinate of the location where the Z value is interpolated.</param>
    public async static void InterpolateZAtLocation(TinLayer tinLayer, double x, double y)
    {
        await QueuedTask.Run(() =>
        {
          // interpolate values at the specified x,y location
          double z = tinLayer.InterpolateZ(x, y, SurfaceInterpolationMethod.NaturalNeighborZNearest);

          // or use a different interpolation method
          z = tinLayer.InterpolateZ(x, y, SurfaceInterpolationMethod.Linear);
        });
    }
    #endregion

      
        // cref: ArcGIS.Desktop.Mapping.TinLayer.GetSurfaceLength(ArcGIS.Core.Geometry.Multipart,ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod)
        // cref: ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod
        // cref: ArcGIS.Desktop.Mapping.TinLayer.GetSurfaceLength(ArcGIS.Core.Geometry.Multipart,ArcGIS.Desktop.Mapping.SurfaceInterpolationMethod,System.Double,System.Double)
        #region Get 3D length of multipart by interpolating heights
    /// <summary>
    /// Calculates the 3D length of a multipart geometry by interpolating heights using a specified surface
    /// interpolation method.
    /// </summary>
    /// <param name="tinLayer">The <see cref="TinLayer"/> used for surface interpolation and 3D length calculation. Cannot be null.</param>
    /// <param name="polygon">The <see cref="Polygon"/> geometry for which the 3D length is calculated. Cannot be null.</param>
    /// <param name="polyline">The <see cref="Polyline"/> geometry for which the 3D length is calculated. Cannot be null.</param>
    public async static void Get3DLengthOfMultipart(TinLayer tinLayer, Polygon polygon, Polyline polyline)
    {
        await QueuedTask.Run(() =>
        {
          // interpolate heights and calculate the sum of 3D distances between the vertices
          double length3d = tinLayer.GetSurfaceLength(polygon, SurfaceInterpolationMethod.NaturalNeighbor);

          // or use a different interpolation method
          length3d = tinLayer.GetSurfaceLength(polyline, SurfaceInterpolationMethod.NaturalNeighborZNearest);


          // densify the shape before interpolating
          length3d = tinLayer.GetSurfaceLength(polygon, SurfaceInterpolationMethod.NaturalNeighbor, 0.01, 0);
        });
      }
    #endregion
  }
}





