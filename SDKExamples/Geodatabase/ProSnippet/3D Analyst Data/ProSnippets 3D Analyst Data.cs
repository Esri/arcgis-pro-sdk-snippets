using ActiproSoftware.Windows.Shapes;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Analyst3D;
using ArcGIS.Core.Data.Exceptions;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.KnowledgeGraph.DataModelling.Interfaces;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Internal.Mapping.Ribbon;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippets.GeoDatabase.ThreeDAnalystData
{
  public static partial class ProSnippetsGeoDatabase
  {
    /// <summary>
    /// This methods has a collection of code snippets related to working with ArcGIS Pro 3D Analyst Data.
    /// </summary>
    /// <remarks>
    /// The code snippets in this class are intended to be used as examples of how to work with 3D Analyst Data in ArcGIS Pro.
    /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
    /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
    /// Crefs are used for internal purposes only. Please ignore them in the context of this example.
    public static async void ProSnippets3DAnalystData()
    {
      #region ignore - Variable initialization
      MapView mapView = MapView.Active;
      Map map = mapView?.Map; //Reference to the active map
      // find the first TIN layer
      var tinLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TinLayer>().FirstOrDefault();

      // find the first Terrain layer
      var terrainLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TerrainLayer>().FirstOrDefault();

      // find the first LAS dataset layer
      var lasDatasetLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<LasDatasetLayer>().FirstOrDefault();

      // find the set of surface layers
      var surfaceLayers = MapView.Active.Map.GetLayersAsFlattenedList().OfType<SurfaceLayer>();
      SurfaceLayer surfaceLayer = surfaceLayers.FirstOrDefault();

      //Retrieve data set objects from the layers 
      ArcGIS.Core.Geometry.Envelope extent;
      ArcGIS.Core.Geometry.SpatialReference sr;
      TinDataset tinDataset;
      using (tinDataset = tinLayer.GetTinDataset())
      {
        using (var tinDef = tinDataset.GetDefinition())
        {
          extent = tinDef.GetExtent();
          sr = tinDef.GetSpatialReference();
        }
      }
      Terrain terrainDataset;
      using (terrainDataset = terrainLayer.GetTerrain())
      {
        using (var terrainDefFromDataset = terrainDataset.GetDefinition())
        {
          extent = terrainDefFromDataset.GetExtent();
          sr = terrainDefFromDataset.GetSpatialReference();
        }
      }
      TerrainDefinition terrainDef = terrainDataset.GetDefinition();
      LasDataset lasDataset;
      using (lasDataset = lasDatasetLayer.GetLasDataset())
      {
        using (var lasDatasetDefFromDataset = lasDataset.GetDefinition())
        {
          extent = lasDatasetDefFromDataset.GetExtent();
          sr = lasDatasetDefFromDataset.GetSpatialReference();
        }
      }
      LasDatasetDefinition lasDatasetDefinition = lasDataset.GetDefinition();
      //Node Symbol
      CIMPointSymbol nodeSymbol = SymbolFactory.Instance.ConstructPointSymbol();
      //Line Symbol
      CIMLineSymbol lineSymbol = SymbolFactory.Instance.ConstructLineSymbol();

      CIMLineSymbol hardEdgeSymbol = SymbolFactory.Instance.ConstructLineSymbol();
      CIMLineSymbol softEdgeSymbol = SymbolFactory.Instance.ConstructLineSymbol();
      CIMLineSymbol outsideEdgeSymbol = SymbolFactory.Instance.ConstructLineSymbol();

      CIMLineSymbol contourLineSymbol = SymbolFactory.Instance.ConstructLineSymbol();
      CIMLineSymbol indexLineSymbol = SymbolFactory.Instance.ConstructLineSymbol();

      CIMPolygonSymbol polySymbol = SymbolFactory.Instance.ConstructPolygonSymbol();
      //Examples to create MapPoints
      MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint(0, 0, 2, 3, 1);
      MapPoint observerPoint = MapPointBuilderEx.CreateMapPoint(0, 0, 2, 3, 1);
      MapPoint targetPoint = MapPointBuilderEx.CreateMapPoint(0, 0, 2, 3, 1); ;

      //Process to create a Polyline
      // list of points
      List<MapPoint> pointsForPolyline = new List<MapPoint>
      {
        MapPointBuilderEx.CreateMapPoint(0, 0, 2, 3, 1),
        MapPointBuilderEx.CreateMapPoint(1, 1, 5, 6),
        MapPointBuilderEx.CreateMapPoint(2, 1, 6),
        MapPointBuilderEx.CreateMapPoint(0, 0)
      };
      Polyline polyline = PolylineBuilderEx.CreatePolyline(pointsForPolyline);

      //Process to create a Polygon
      List<MapPoint> pointsForPolygon = new List<MapPoint>
      {
        MapPointBuilderEx.CreateMapPoint(0, 0, 2, 3, 1),
        MapPointBuilderEx.CreateMapPoint(1, 1, 5, 6),
        MapPointBuilderEx.CreateMapPoint(2, 1, 6),
        MapPointBuilderEx.CreateMapPoint(0, 0)
      };
      Polygon polygon = PolygonBuilderEx.CreatePolygon(pointsForPolygon);
      CIMPointSymbol obstructionPointSymbol = SymbolFactory.Instance.ConstructPointSymbol();
      CIMLineSymbol visibleLineSymbol = SymbolFactory.Instance.ConstructLineSymbol();
      CIMLineSymbol invisibleLineSymbol = SymbolFactory.Instance.ConstructLineSymbol();

      //Node
      ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinDataset.SearchNodes(null);
      ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current;

      //Edge
      ArcGIS.Core.Data.Analyst3D.TinEdge edge = tinDataset.GetEdgeByIndex(45);

      //Triangle
      ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleCursor = tinDataset.SearchTriangles(null);
      ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = triangleCursor.Current;

      Geodatabase geodatabase = new Geodatabase(
         new FileGeodatabaseConnectionPath(new Uri(@"E:\Data\Admin\AdminData.gdb")));
      FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("Cities");

      TinEditor tinEditor = new TinEditor(MapView.Active.Extent); //Use an appropriate envelope

      MapPoint[] points = //Define some points
      [];

      MapPoint[] pointsZ = //Define some z-aware points
      [];
      Envelope envelope = EnvelopeBuilderEx.CreateEnvelope(0, 0, 100, 100, sr);
      //z aware point
      MapPoint pointZ = MapPointBuilderEx.CreateMapPoint(1, 1, 5, sr);
      //Mulipoint with z values
      List<Coordinate2D> coords2D = new List<Coordinate2D>()
        {
          new Coordinate2D(0, 0),
          new Coordinate2D(0, 1),
          new Coordinate2D(1, 1),
          new Coordinate2D(1, 0)
        };
      Multipoint multipointZ = MultipointBuilderEx.CreateMultipoint(coords2D, SpatialReferences.WGS84);

      #endregion
      #region ProSnippet Group: TIN
      #endregion
      // cref: ArcGIS.Core.Data.FileSystemDatastoreType.Tin
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
      // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
      // cref: ArcGIS.Core.Data.FileSystemDatastore.OpenDataset``1(System.String)
      #region Open a TIN Dataset
      {
        //Note: This snippet requires to be run on the MCT (Main CIM Thread)
        string path = @"d:\Data\Tin";
        var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.Tin);

        using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
        {
          // TIN is in a folder at d:\Data\Tin\TinDataset

          string dsName = "TinDataset";

          using (var dataset = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.TinDataset>(dsName))
          {
            //Do something with the dataset
          }
        }
      }
      #endregion
      // cref: ArcGIS.Core.Data.FileSystemDatastoreType.Tin
      // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition
      // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
      // cref: ArcGIS.Core.Data.FileSystemDatastore.GetDefinition``1(System.String)
      #region Get a TIN Defintion
      {
        //Note: This snippet requires to be run on the MCT (Main CIM Thread)
        string path = @"d:\Data\Tin";
        var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.Tin);

        using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
        {
          // TIN is in a folder at d:\Data\Tin\TinDataset

          string dsName = "TinDataset";

          using (var def = dataStore.GetDefinition<ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition>(dsName))
          {
            //Do something with the definition
          }
        }
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetSuperNodeExtent
      #region Get Super Node Extent
      {
        var superNodeExtent = tinDataset.GetSuperNodeExtent();
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetDataArea
      #region Get Data Area
      {
        var dataArea = tinDataset.GetDataArea();
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNodeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetOutsideNodeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetEdgeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetOutsideEdgeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetOutsideTriangleCount
      #region Element Counts
      {
        var nodeCount = tinDataset.GetNodeCount();
        var outsideNodeCount = tinDataset.GetOutsideNodeCount();
        var edgeCount = tinDataset.GetEdgeCount();
        var outsideEdgecount = tinDataset.GetOutsideEdgeCount();
        var triCount = tinDataset.GetTriangleCount();
        var outsideTriCount = tinDataset.GetOutsideTriangleCount();
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetIsEmpty
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.HasHardEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.HasSoftEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.UsesConstrainedDelaunay
      #region Tin Properties
      {
        var isEmpty = tinDataset.GetIsEmpty();
        var hasHardEdges = tinDataset.HasHardEdges();
        var hasSoftEdges = tinDataset.HasSoftEdges();

        var isConstrainedDelaunay = tinDataset.UsesConstrainedDelaunay();
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNodeByIndex(System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetEdgeByIndex(System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleByIndex(System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      #region Access TIN Elements By Index
      {
        using (ArcGIS.Core.Data.Analyst3D.TinNode nodeFromIndex = tinDataset.GetNodeByIndex(23))
        {
          //Do something with the node
        }

        using (ArcGIS.Core.Data.Analyst3D.TinEdge edgeFromIndex = tinDataset.GetEdgeByIndex(45))
        {
          //Do something with the edge
        }
        using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangleFromIndex = tinDataset.GetTriangleByIndex(22))
        {
          //Do something with the triangle
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.SearchNodes(ArcGIS.Core.Data.Analyst3D.TinNodeFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor.MoveNext
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor.Current
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.SuperNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilterType
      #region Search for TIN Nodes
      {
        // search all nodes that intersect the data extent
        using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursorsearch = tinDataset.SearchNodes(null))
        {
          while (nodeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinNode nodeCurrent = nodeCursor.Current)
            {
              // do something with the node
            }
          }
        }

        // search within an extent
        ArcGIS.Core.Data.Analyst3D.TinNodeFilter nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
        nodeFilter.FilterEnvelope = envelope; //or use any other appropriate envelope
        using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursorSearch = tinDataset.SearchNodes(nodeFilter))
        {
          while (nodeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinNode nodeCurrent = nodeCursor.Current)
            {
              // do something with the node
            }
          }
        }

        // search all "inside" nodes
        nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
        nodeFilter.FilterType = ArcGIS.Core.Data.Analyst3D.TinFilterType.InsideDataArea;
        using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursorSearch = tinDataset.SearchNodes(nodeFilter))
        {
          while (nodeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinNode nodeCurrent = nodeCursor.Current)
            {
              // do something with the node
            }
          }
        }

        // search for super nodes only
        nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
        nodeFilter.FilterEnvelope = tinDataset.GetSuperNodeExtent();
        nodeFilter.SuperNode = true;
        using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursorSearch = tinDataset.SearchNodes(nodeFilter))
        {
          while (nodeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinNode nodeCurrent = nodeCursor.Current)
            {
              // do something with the node
            }
          }
        }
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.SearchEdges(ArcGIS.Core.Data.Analyst3D.TinEdgeFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.FilterByEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.EdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilterType
      #region Search for TIN Edges
      {
        // search all single edges that intersect the data extent
        using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(null))
        {
          while (edgeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinEdge edgeCurrent = edgeCursor.Current)
            {
              // do something with the edge
            }
          }
        }

        // search within an extent
        ArcGIS.Core.Data.Analyst3D.TinEdgeFilter edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
        edgeFilter.FilterEnvelope = envelope;
        using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(edgeFilter))
        {
          while (edgeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinEdge edgeCurrent = edgeCursor.Current)
            {
              // do something with the edge
            }
          }
        }

        // search all "inside" edges
        edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
        edgeFilter.FilterType = ArcGIS.Core.Data.Analyst3D.TinFilterType.InsideDataArea;
        using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(edgeFilter))
        {
          while (edgeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinEdge edgeCurrent = edgeCursor.Current)
            {
              // do something with the edge
            }
          }
        }

        // search for hard edges
        edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
        edgeFilter.FilterByEdgeType = true;
        edgeFilter.EdgeType = ArcGIS.Core.Data.Analyst3D.TinEdgeType.HardEdge;
        using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(edgeFilter))
        {
          while (edgeCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinEdge edgeCurrent = edgeCursor.Current)
            {
              // do something with the edge
            }
          }
        }
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.SearchTriangles(ArcGIS.Core.Data.Analyst3D.TinTriangleFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilterType
      #region Search for TIN Triangles
      {
        // search all triangles that intersect the data extent
        using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleSearchCursor = tinDataset.SearchTriangles(null))
        {
          while (triangleCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangleCurrent = triangleCursor.Current)
            {

            }
          }
        }

        // search within an extent
        ArcGIS.Core.Data.Analyst3D.TinTriangleFilter triangleFilter = new ArcGIS.Core.Data.Analyst3D.TinTriangleFilter();
        triangleFilter.FilterEnvelope = envelope;
        using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleSearchCursor = tinDataset.SearchTriangles(triangleFilter))
        {
          while (triangleCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangleCurrent = triangleCursor.Current)
            {

            }
          }
        }

        // search all "inside" triangles
        triangleFilter = new ArcGIS.Core.Data.Analyst3D.TinTriangleFilter();
        triangleFilter.FilterType = ArcGIS.Core.Data.Analyst3D.TinFilterType.InsideDataArea;
        using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleSearchCursor = tinDataset.SearchTriangles(triangleFilter))
        {
          while (triangleCursor.MoveNext())
          {
            using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangleCurrent = triangleCursor.Current)
            {

            }
          }
        }
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNearestNode(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNearestEdge(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleByPoint(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNaturalNeighbors(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleNeighborhood(ArcGIS.Core.Geometry.MapPoint)
      #region Access TIN Elements by MapPoint
      {
        // "identify" the closest node, edge, triangle
        using (var nearestNode = tinDataset.GetNearestNode(mapPoint))
        {
          // do something with the node
        }

        using (var nearestEdge = tinDataset.GetNearestEdge(mapPoint))
        {
          // do something with the edge
        }
        using (var triangleByPoint = tinDataset.GetTriangleByPoint(mapPoint))
        {
          // do something with the triangle
        }

        // get the set of natural neighbors 
        // (set of nodes that "mapPoint" would connect with to form triangles if it was added to the TIN)
        IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinNode> naturalNeighbors = tinDataset.GetNaturalNeighbors(mapPoint);

        // get the set of triangles whose circumscribed circle contains "mapPoint" 
        IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinTriangle> triangles = tinDataset.GetTriangleNeighborhood(mapPoint);
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.Coordinate3D
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.ToMapPoint
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.IsInsideDataArea
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.GetAdjacentNodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.GetIncidentEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.GetIncidentTriangles
      #region TIN Nodes
      {
        // is the node "inside"
        var isInsideNode = node.IsInsideDataArea;

        // get all other nodes connected to "node" 
        IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinNode> adjNodes = node.GetAdjacentNodes();

        // get all edges that share "node" as a from node. 
        IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinEdge> edges = node.GetIncidentEdges();

        // get all triangles that share "node"
        IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinTriangle> triangles = node.GetIncidentTriangles();
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.Nodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.ToPolyline
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.Length
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.IsInsideDataArea
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.EdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.GetNextEdgeInTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.GetPreviousEdgeInTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.GeNeighbor
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.LeftTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.RightTriangle
      #region TIN Edges
      {
        // nodes of the edge
        var nodes = edge.Nodes;

        // edge geometry
        polyline = edge.ToPolyline();
        // edge length
        var length = edge.Length;
        // is the edge "inside"
        var isInsideEdge = edge.IsInsideDataArea;
        // edge type - regular/hard/soft
        var edgeType = edge.EdgeType;

        // get next (clockwise) edge in the triangle
        var nextEdge = edge.GetNextEdgeInTriangle();
        // get previous (anti-clockwise) edge in the triangle
        var prevEdge = edge.GetPreviousEdgeInTriangle();

        // get opposite edge
        var oppEdge = edge.GetNeighbor();

        // get left triangle
        var leftTriangle = edge.LeftTriangle;
        // get right triangle
        var rightTriangle = edge.RightTriangle;
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Nodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Edges
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.ToPolygon
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Length
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Area
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.IsInsidedataArea
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Aspect
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Slope
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetCentroid
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetNormal
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetAdjacentTriangles
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetPointsBetweenZs
      #region TIN Triangles
      {
        // nodes, edges of the triangle
        var triNnodes = triangle.Nodes;
        var triEdges = triangle.Edges;

        // triangle geometry
        polygon = triangle.ToPolygon();
        // triangle length
        var triLength = triangle.Length;
        // triangle area 
        var triArea = triangle.Area;
        // is the triangle "inside"
        var isInsideTriangle = triangle.IsInsideDataArea;

        // triangle aspect and slope  (radians)
        var aspect = triangle.Aspect;
        var slope = triangle.Slope;

        // get centroid
        var centroid = triangle.GetCentroid();

        // get normal
        var normal = triangle.GetNormal();

        // get adjacent triangles
        var adjTriangles = triangle.GetAdjacentTriangles();

        // get area of triangle that falls between the z values
        double minZ = 1.0;
        double maxZ = 3.0;
        IReadOnlyList<Coordinate3D> coords = triangle.GetPointsBetweenZs(minZ, maxZ);
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.#ctor(ArcGIS.Core.Geometry.Envelope)
      #region Create TinEditor from envelope
      {
        //Example envelope
        TinEditor tinEditorFromEnvelope = new TinEditor(envelope);
        bool isInEditMode = tinEditorFromEnvelope.IsInEditMode;  // isInEditMode = true
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.#ctor(ArcGIS.Core.Data.Analyst3D.TinDataset)
      #region Create TinEditor from TinDataset
      {
        TinEditor tinEditorFromDataset = new TinEditor(tinDataset);
        bool isInEditMode = tinEditorFromDataset.IsInEditMode;  // isInEditMode = true
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.CreateFromFeatureClass(ArcGIS.Core.Data.FeatureClass,ArcGIS.Core.Data.QueryFilter,ArcGIS.Core.Data.Field, ArcGIS.Core.Data.Field,ArcGIS.Core.Data.Analyst3D.TinSurfaceType)
      #region Create TinEditor from feature class
      {
        var fields = featureClass.GetDefinition().GetFields();

        // Use the z-values from the geometries as the height field
        ArcGIS.Core.Data.Field heightField = fields.First(f => f.FieldType == FieldType.Geometry);

        // Set the vertices from the geometries as TIN nodes
        TinEditor tinEditorFromFC = TinEditor.CreateFromFeatureClass(featureClass, null, heightField, null, TinSurfaceType.MassPoint);
        bool isInEditMode = tinEditorFromFC.IsInEditMode;  // isInEditMode = true

        // Use the object ids as tag values
        ArcGIS.Core.Data.Field tagField = fields.First(f => f.FieldType == FieldType.OID);

        // Set the lines from the geometries as TIN edges
        var tinEditorFromFCEdges = TinEditor.CreateFromFeatureClass(featureClass, null, heightField, tagField, TinSurfaceType.HardLine);
        isInEditMode = tinEditorFromFCEdges.IsInEditMode;  // isInEditMode = true

        // Only use certain geometries in the TIN
        ArcGIS.Core.Data.QueryFilter filter = new ArcGIS.Core.Data.QueryFilter()
        {
          ObjectIDs = new List<long> { 2, 6, 7, 8, 9, 10, 14, 17, 21, 22 }
        };
        tinEditor = TinEditor.CreateFromFeatureClass(featureClass, filter, heightField, tagField, TinSurfaceType.HardLine);
        isInEditMode = tinEditor.IsInEditMode;  // isInEditMode = true
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddFromFeatureClass
      #region Add geometries from feature class
      {
        var fields = featureClass.GetDefinition().GetFields();

        // Use the z-values from the geometries as the height field
        Field heightField = fields.First(f => f.FieldType == FieldType.Geometry);

        // Set the vertices from the geometries as TIN nodes
        //Get TinEditor in various ways as shown above before using it here
        tinEditor.AddFromFeatureClass(featureClass, null, heightField, null, TinSurfaceType.MassPoint);

        // Use the object ids as tag values
        Field tagField = fields.First(f => f.FieldType == FieldType.OID);

        // Set the lines from the geometries as TIN edges
        tinEditor.AddFromFeatureClass(featureClass, null, heightField, tagField, TinSurfaceType.HardLine);

        // Only use certain geometries in the TIN
        QueryFilter filter = new QueryFilter()
        {
          ObjectIDs = new List<long> { 2, 6, 7, 8, 9, 10, 14, 17, 21, 22 }
        };
        tinEditor.AddFromFeatureClass(featureClass, filter, heightField, tagField, TinSurfaceType.HardLine);
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddGeometry
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddGeometryZ
      #region Add a geometry
      {
        // Add a point as a node with no tag value at height = 10. Points and multipoints can only be added as mass points.
        tinEditor.AddGeometry(mapPoint, TinSurfaceType.MassPoint, 0, 10);

        // Add a z-aware multipoint as a nodes with tag value = 12 at height equal to the z-values of the points. Points and multipoints can only be added as mass points.
        tinEditor.AddGeometryZ(multipointZ, TinSurfaceType.MassPoint, 12);

        // Add a polyline as hard lines with tag value = 42 and height = 17.
        tinEditor.AddGeometry(polyline, TinSurfaceType.HardLine, 42, 17);

        // Add a z-aware polygon as an erase polygon with no tag value and height equal to the z-values of the vertices.
        Polygon polygonZ = PolygonBuilderEx.CreatePolygon(pointsForPolygon); //Define a z-aware polygon
        tinEditor.AddGeometryZ(polygonZ, TinSurfaceType.HardErase, 0);
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddMassPoints(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},System.Int32,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddMassPoints
      #region Add mass points
      {
        // Add points with no tag value and height = 17.
        // The points have the same spatial reference as the tin editor, so there is no need to provide it. 
        tinEditor.AddMassPoints(points, 0, 17);

        Coordinate3D[] coordinate3Ds = new Coordinate3D[] //Define some coordinates
        {
          new Coordinate3D(0,0,5),
          new Coordinate3D(1,1,6),
          new Coordinate3D(2,1,7)
        };
        // Add coordinates as nodes with tag value = 42. The height will come from the z-values of the coordinates.
        tinEditor.AddMassPointsZ(coordinate3Ds, 42);
        // Add z-aware points with tag value = 21. The height will come from the z-values of the points.
        // The points are in a different spatial reference than the tin editor, so we provide the spatial 
        // reference of the points. The points will be projected to the spatial reference of the tin editor.
        tinEditor.AddMassPointsZ(pointsZ, 21, SpatialReferenceBuilder.CreateSpatialReference(54004));
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddPointZ(ArcGIS.Core.Geometry.MapPoint,System.Int32)
      #region Add z-aware point
      {
        // Add a z-aware point with tag value = 56
        tinEditor.AddPointZ(pointZ, 56);
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddPolygons(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polygon},ArcGIS.Core.Data.Analyst3D.TinSurfaceType,System.Int32,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddPolygonsZ(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polygon},ArcGIS.Core.Data.Analyst3D.TinSurfaceType,System.Int32,ArcGIS.Core.Geometry.SpatialReference)
      #region Add polygons
      {
        // Add polygons with tagValue = 42 and height = 12. 
        // The polygons are in a different spatial reference than the tin editor, so we provide the spatial 
        // reference of the polygons. The polygons will be projected to the spatial reference of the tin editor.
        Polygon[] polygons = new Polygon[] //Define some polygons
        {
          PolygonBuilderEx.CreatePolygon(new List<Coordinate2D>
          {
            new Coordinate2D(0,0),
            //...
          }),
          PolygonBuilderEx.CreatePolygon(new List<Coordinate2D>
          {
            new Coordinate2D(20,20),
            //..
          })
        };
        tinEditor.AddPolygons(polygons, TinSurfaceType.ZLessSoftLine, 42, 12, SpatialReferenceBuilder.CreateSpatialReference(54004));

        // Add z-aware polygons with no tag value. The height comes from the z-values of the vertices. 
        // The polygons are in the same spatial reference as the tin editor, so there is no need to provide it.
        Polygon[] polygonsZ = new Polygon[] //Define some z-aware polygons
        {
          PolygonBuilderEx.CreatePolygon(new List<Coordinate3D>
          {
            new Coordinate3D(0,0,5),
            //...
          }),
          PolygonBuilderEx.CreatePolygon(new List<Coordinate3D>
          {
            new Coordinate3D(20,20,15),
            //..
          })
        };
        tinEditor.AddPolygonsZ(polygonsZ, TinSurfaceType.HardLine, 0);
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddPolylines(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Core.Data.Analyst3D.TinSurfaceType,System.Int32,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.AddPolylinesZ(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Core.Data.Analyst3D.TinSurfaceType,System.Int32,ArcGIS.Core.Geometry.SpatialReference)
      #region Add polylines
      { 
        // Add polylines with tagValue = 42 and height = 12. 
        // The polylines are in a different spatial reference than the tin editor, so we provide the spatial 
        // reference of the polylines. The polylines will be projected to the spatial reference of the tin editor.
        Polyline[] polylines = new Polyline[] //Define some polylines
        {
          PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>
          {
            new Coordinate2D(0,0),
            //...
          }),
          PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>
          {
            new Coordinate2D(20,20),
            //..
          })
        };
        tinEditor.AddPolylines(polylines, TinSurfaceType.ZLessSoftLine, 42, 12, SpatialReferenceBuilder.CreateSpatialReference(54004));

        // Add z-aware polylines with no tag value. The height comes from the z-values of the vertices. 
        // The polylines are in the same spatial reference as the tin editor, so there is no need to provide it.
        Polyline[] polylinesZ = new Polyline[] //Define some z-aware polylines
        {
          PolylineBuilderEx.CreatePolyline(new List<Coordinate3D>
          {
            new Coordinate3D(0,0,5),
            //...
          }),
          PolylineBuilderEx.CreatePolyline(new List<Coordinate3D>
          {
            new Coordinate3D(20,20,15),
            //..
          })
        };
        tinEditor.AddPolylinesZ(polylinesZ, TinSurfaceType.HardLine, 0);
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.DeleteEdgeTagValues()
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.DeleteNodeTagValues()
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.DeleteTriangleTagValues()
      #region Delete tag values
      {
        // Delete all edge tags
        tinEditor.DeleteEdgeTagValues();

        // Delete all node tags
        tinEditor.DeleteNodeTagValues();

        // Delete all triangle tags
        tinEditor.DeleteTriangleTagValues();
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetEdgeTagValue(System.Int32,System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetNodeTagValue(System.Int32,System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetTriangleTagValue(System.Int32,System.Int32)
      #region Set tag values
      {
        // Set the tag value for edge #6
        tinEditor.SetEdgeTagValue(6, 42);

        // Set the tag value for node #8
        tinEditor.SetNodeTagValue(8, 93);

        // Set the tag value for triangle #9
        tinEditor.SetTriangleTagValue(9, 17);
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.DeleteNode(System.Int32)
      #region Delete node
      {
        // Delete node by index 
        tinEditor.DeleteNode(7);

        // Node indices start at 1.
        try
        {
          tinEditor.DeleteNode(0);
        }
        catch (ArgumentException)
        {
          // Handle the exception
        }

        // Can't delete a super node (indices 1 - 4)
        try
        {
          tinEditor.DeleteNode(2);
        }
        catch (TinException)
        {
          // Handle the exception
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.DeleteNodesOutsideDataArea()
      #region Delete nodes outside of data area
      {
        // Delete all data nodes that are outside the data area. Does not delete super nodes.
        tinEditor.DeleteNodesOutsideDataArea();
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetEdgeType(System.Int32,ArcGIS.Core.Data.Analyst3D.TinEdgeType)
      #region Set edge type
      {
        // Set the type of edge #8
        tinEditor.SetEdgeType(8, TinEdgeType.SoftEdge);
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetNodeZ(System.Int32,System.Double)
      #region Set z-value of a node
      {
        // Set the z-value of node #10
        tinEditor.SetNodeZ(10, 12.5);
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetSpatialReference(ArcGIS.Core.Geometry.SpatialReference)
      #region Set the spatial reference
      {
        // Set the spatial reference
        tinEditor.SetSpatialReference(SpatialReferenceBuilder.CreateSpatialReference(54004));
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetToConstrainedDelaunay()
      #region Set to constrained Delaunay
      {
        // Set the triangulation method to constrained Delaunay from this point forward
        tinEditor.SetToConstrainedDelaunay();
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetTriangleInsideDataArea(System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SetTriangleOutsideDataArea(System.Int32)
      #region Set triangle in/out of data area
      {
        // Set triangle #7 to be inside the data area
        tinEditor.SetTriangleInsideDataArea(7);

        // Set triangle #9 to be outside the data area
        tinEditor.SetTriangleInsideDataArea(9);
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SaveEdits
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SaveAs(System.String,System.Boolean)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.StartEditing
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.StopEditing
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.#ctor(ArcGIS.Core.Geometry.Envelope)
      #region Create a new TIN and save edits
      {
        // Create a new TIN 
        tinEditor = new TinEditor(MapView.Active.Extent); // or use any envelope 
        tinEditor.AddMassPoints(points, 42, 13.7);

        // Since the TIN doesn't exist on disk, you can't call SaveEdits.
        // You must call SaveAs first.
        try
        {
          tinEditor.SaveEdits();
        }
        catch (TinException)
        {
          // Handle the exception
        }

        // Since the TIN doesn't exist on disk, you can't call StopEditing(true).
        // You must call SaveAs first.
        try
        {
          tinEditor.StopEditing(true);
        }
        catch (TinException)
        {
          // Handle the exception
        }

        // Now save the newly created TIN to disk
        tinEditor.SaveAs("C:\\Tin1", false);

        // Delete a node
        tinEditor.DeleteNode(7);

        // Since the TIN now exists on disk you can call SaveEdits
        tinEditor.SaveEdits();

        // Delete another node
        tinEditor.DeleteNode(11);

        // Since the TIN now exists on disk, you can call StopEditing(true).
        // The edits will be saved and the tin editor will be taken out of edit mode.
        tinEditor.StopEditing(true);
        bool isInEditMode = tinEditor.IsInEditMode; // isInEditMode = false

        // Now if you try to make an edit, an exception is thrown because the editor is not in edit mode.
        try
        {
          tinEditor.AddPointZ(pointZ, 0);
        }
        catch (TinException)
        {
          // Handle the exception
        }

        // Put the editor into edit mode.
        tinEditor.StartEditing();
        isInEditMode = tinEditor.IsInEditMode; // isInEditMode = true

        // Now you can add the point
        tinEditor.AddPointZ(pointZ, 0);

        // Oops, you didn't really want to add the point. You want to stop editing and discard the unsaved edits
        // since the last time the editor was put into edit mode. All previous saved edits remain.
        tinEditor.StopEditing(false);
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SaveEdits()
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.SaveAs(System.String, System.Boolean)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.StartEditing()
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.StopEditing()
      // cref: ArcGIS.Core.Data.Analyst3D.TinEditor.#ctor(ArcGIS.Core.Data.Analyst3D.TinDataset)
      #region Edit an existing TIN
      {
        string tinPath = "C:\\Tin"; //original TIN path
        // Create an instance of TinEditor from an existing TinDataset
        tinEditor = new TinEditor(tinDataset);
        int numNodes = tinDataset.GetNodeCount();  // numNodes = 10
        tinEditor.AddPointZ(pointZ, 7);

        // Calling SaveEdits modifies the existing TIN
        tinEditor.SaveEdits();
        numNodes = tinDataset.GetNodeCount();  // numNodes = 11

        // Adding twenty points
        tinEditor.AddMassPoints(points, 10, 112.5);

        // Calling SaveAs creates a new TIN on disk, and 
        // the tin editor points to the new TIN.
        string tinPath2 = "C:\\Tin2";
        tinEditor.SaveAs(tinPath2, true);

        tinEditor.StopEditing(true);
        var connection = new FileSystemConnectionPath(new Uri(Path.GetDirectoryName(tinPath2)), FileSystemDatastoreType.Tin);
        using (FileSystemDatastore dataStore = new FileSystemDatastore(connection))
        {
          // See https://github.com/esri/arcgis-pro-sdk/wiki/ProConcepts-3D-Analyst-Data#working-with-tin-data
          TinDataset tinDataset2 = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.TinDataset>(Path.GetFileName(tinPath2));
          numNodes = tinDataset2.GetNodeCount(); // numNodes = 31
        }

        // The edits still show up in the original TIN while it is in memory, but if you open it
        // again you will see that it only has the edits that were saved before SaveAs was called.
        numNodes = tinDataset.GetNodeCount(); // numNodes = 31
        var connection2 = new FileSystemConnectionPath(new Uri(Path.GetDirectoryName(tinPath)), FileSystemDatastoreType.Tin);
        using (FileSystemDatastore dataStore = new FileSystemDatastore(connection))
        {
          // See https://github.com/esri/arcgis-pro-sdk/wiki/ProConcepts-3D-Analyst-Data#working-with-tin-data
          tinDataset = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.TinDataset>(Path.GetFileName(tinPath));
          numNodes = tinDataset.GetNodeCount(); // numNodes = 11
        }
      }

      #endregion
      #region ProSnippet Group: Terrain
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      #region Open a Terrain
      {
        //Note: Use QueuedTask.Run to run on the MCT
        string path = @"d:\Data\Terrain\filegdb_Containing_A_Terrain.gdb";
        var fileConnection = new FileGeodatabaseConnectionPath(new Uri(path));

        using (Geodatabase dataStore = new Geodatabase(fileConnection))
        {
          string dsName = "nameOfTerrain";

          using (var dataset = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.Terrain>(dsName))
          {
          }
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetFeatureClassNames
      #region Get a Terrain Definition
      {
        string path = @"d:\Data\Terrain\filegdb_Containing_A_Terrain.gdb";
        var fileConnection = new FileGeodatabaseConnectionPath(new Uri(path));
        //Note: Use QueuedTask.Run to run on the MCT 
        using (Geodatabase dataStore = new Geodatabase(fileConnection))
        {
          string dsName = "nameOfTerrain";

          using (var terrainDefinition = dataStore.GetDefinition<ArcGIS.Core.Data.Analyst3D.TerrainDefinition>(dsName))
          {
            // get the feature class names that are used in the terrain
            var fcNames = terrainDefinition.GetFeatureClassNames();
          }
        }
      }


      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDataSourceCount
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDataSources
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.DataSourceName
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.SurfaceType
      // cref: ArcGIS.Core.Data.Analyst3D.TinSurfaceType
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.MinimumResolution
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.MaximumResolution
      #region Get datasources from a Terrain
      {
        var dsCount = terrainDataset.GetDataSourceCount();
        IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TerrainDataSource> dataSources = terrainDataset.GetDataSources();
        foreach (var ds in dataSources)
        {
          var dsName = ds.DataSourceName;
          var surfaceType = ds.SurfaceType;
          var maxResolution = ds.MaximumResolution;
          var minResolution = ds.MinimumResolution;
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetPyramidLevelCount
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetPyramidLevels
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel.Resolution
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel.MaximumScale
      #region Get Pyramid Level Information from a Terrain
      {
        var levelCount = terrainDataset.GetPyramidLevelCount();
        IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel> pyramidLevels = terrainDataset.GetPyramidLevels();
        foreach (var pyramidLevel in pyramidLevels)
        {
          var resolution = pyramidLevel.Resolution;
          var maxScale = pyramidLevel.MaximumScale;
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetTileProperties
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties 
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.ColumnCount
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.RowCount
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.TileSize
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.TileCount
      #region Get Tile Information from a Terrain
      {
        var tileInfo = terrainDataset.GetTileProperties();
        var colCount = tileInfo.ColumnCount;
        var rowCount = tileInfo.RowCount;
        var tileSize = tileInfo.TileSize;
        var tileCount = tileInfo.TileCount;
      }
      #endregion
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetPyramidType
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidType
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetPyramidWindowSizeProperties
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties.Method
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeMethod
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties.ZThreshold
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties.ZThresholdStrategy
      #region Get Pyramid Information from a TerrainDefinition
      {
        var pyramidType = terrainDef.GetPyramidType();
        var pyramidProps = terrainDef.GetPyramidWindowSizeProperties();

        var method = pyramidProps.Method;
        var threshold = pyramidProps.ZThreshold;
        var strategy = pyramidProps.ZThresholdStrategy;
      }
      #endregion
      #region ProSnippet Group: LAS Dataset
      #endregion
      // cref: ArcGIS.Core.Data.FileSystemDatastoreType.LasDataset
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
      // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
      // cref: ArcGIS.Core.Data.FileSystemDatastore.OpenDataset``1(System.String)
      #region Open a LAS Dataset
      {
        //Note: Use QueuedTask.Run to run on the MCT 

        string path = @"d:\Data\LASDataset";
        var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.LasDataset);

        using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
        {
          string name = "utrecht_tile.lasd";      // can specify with or without the .lasd extension

          using (var dataset = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.LasDataset>(name))
          {

          }
        }
      }

      #endregion
      // cref: ArcGIS.Core.Data.FileSystemDatastoreType.LasDataset
      // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition
      // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
      // cref: ArcGIS.Core.Data.FileSystemDatastore.GetDefinition``1(System.String)
      #region Get a LAS Dataset Defintion
      {
        //Note: Use QueuedTask.Run to run on the MCT
        string path = @"d:\Data\LASDataset";
        var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.LasDataset);

        using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
        {
          string name = "utrecht_tile.lasd";      // can specify with or without the .lasd extension

          using (var dataset = dataStore.GetDefinition<ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition>(name))
          {
            //Use the dataset
          }
        }
        #endregion
        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetFileCounts
        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetFiles
        // cref: ArcGIS.Core.Data.Analyst3D.LasFile
        // cref: ArcGIS.Core.Data.Analyst3D.LasFile.FilePath
        // cref: ArcGIS.Core.Data.Analyst3D.LasFile.FileName
        // cref: ArcGIS.Core.Data.Analyst3D.LasFile.PointCount
        // cref: ArcGIS.Core.Data.Analyst3D.LasFile.ZMin
        // cref: ArcGIS.Core.Data.Analyst3D.LasFile.ZMax
        #region Get Individual File Information from a LAS Dataset
        {
          var (lasFileCount, zLasFileCount) = lasDataset.GetFileCounts();
          IReadOnlyList<ArcGIS.Core.Data.Analyst3D.LasFile> fileInfo = lasDataset.GetFiles();
          foreach (var file in fileInfo)
          {
            var pathFile = file.FilePath;
            var name = file.FileName;
            var ptCount = file.PointCount;
            var zMin = file.ZMin;
            var zMax = file.ZMax;
          }
        }
        #endregion

        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetSurfaceConstraintCount
        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetSurfaceConstraints
        // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint
        // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.DataSourceName
        // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.WorkspacePath
        // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.HeightField
        // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.SurfaceType
        // cref: ArcGIS.Core.Data.Analyst3D.TinSurfaceType
        #region Get Surface Constraint information from a LAS Dataset
        {
          var constraintCount = lasDataset.GetSurfaceConstraintCount();
          IReadOnlyList<ArcGIS.Core.Data.Analyst3D.SurfaceConstraint> constraints = lasDataset.GetSurfaceConstraints();
          foreach (var constraint in constraints)
          {
            var dsName = constraint.DataSourceName;
            var wksPath = constraint.WorkspacePath;
            var heightField = constraint.HeightField;
            var surfaceType = constraint.SurfaceType;
          }
        }
        #endregion
        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetUniqueClassCodes
        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetUniqueReturns
        // cref: ArcGIS.Core.Data.Analyst3D.LasReturnType
        #region Get classification codes / Returns from a LAS Dataset
        {
          var classCodes = lasDataset.GetUniqueClassCodes();
          var returns = lasDataset.GetUniqueReturns();

        }
        #endregion
        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetPointByID(System.Double, ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Data.Analyst3D.LasPoint
        // cref: ArcGIS.Core.Data.Analyst3D.LasPoint.Coordinate3D
        // cref: ArcGIS.Core.Data.Analyst3D.LasPoint.ToMapPoint
        #region Access LAS Points by ID
        {
          // access by ID
          IReadOnlyList<ArcGIS.Core.Data.Analyst3D.LasPoint> pts = lasDataset.GetPointByID(123456);

          pts = lasDataset.GetPointByID(123456, envelope);
          ArcGIS.Core.Data.Analyst3D.LasPoint pt = pts.FirstOrDefault();

          var coordsLasPoint = pt.Coordinate3D;
          var mapPointLas = pt.ToMapPoint();
        }
        #endregion

        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.SearchPoints(ArcGIS.Core.Data.Analyst3D.LasPointFilter, System.Double, System.Double)
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNext
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.Current
        // cref: ArcGIS.Core.Data.Analyst3D.LasPoint
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.ClassCodes
        #region Search LAS Points
        {
          // search within an extent
          ArcGIS.Core.Data.Analyst3D.LasPointFilter pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
          pointFilter.FilterGeometry = envelope;
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(pointFilter))
          {
            while (ptCursor.MoveNext())
            {
              using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
              {

              }
            }
          }

          // search within an extent and limited to specific classification codes
          pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
          pointFilter.FilterGeometry = envelope;
          pointFilter.ClassCodes = new List<int> { 4, 5 };
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(pointFilter))
          {
            while (ptCursor.MoveNext())
            {
              using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
              {

              }
            }
          }
        }
        #endregion

        // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.SearchPoints(ArcGIS.Core.Data.Analyst3D.LasPointFilter, Systen.Double, System.Double)
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNextArray
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
        // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
        #region Search using pre initialized arrays
        {
          // search all points
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(null))
          {
            int count;
            Coordinate3D[] lasPointsRetrieved = new Coordinate3D[10000];
            while (ptCursor.MoveNextArray(lasPointsRetrieved, null, null, null, out count))
            {
              var pointsLas = lasPointsRetrieved.ToList();

              // ...
            }
          }

          // search within an extent
          // use MoveNextArray retrieving coordinates, fileIndex and pointIds
          ArcGIS.Core.Data.Analyst3D.LasPointFilter filter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
          filter.FilterGeometry = envelope;
          using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(filter))
          {
            int count;
            Coordinate3D[] lasPointsRetrieved = new Coordinate3D[50000];
            int[] fileIndexes = new int[50000];
            double[] pointIds = new double[50000];
            while (ptCursor.MoveNextArray(lasPointsRetrieved, null, fileIndexes, pointIds, out count))
            {
              var pointsLas = lasPointsRetrieved.ToList();

            }
          }
        }
        #endregion
      }
    }
  }
}


