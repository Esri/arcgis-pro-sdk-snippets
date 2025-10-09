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
using ArcGIS.Core.Data.Topology;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Linq;

namespace ProSnippets.EditingSnippets
{
  public static partial class ProSnippetsEditing
  {
    #region ProSnippet Group: Topology Properties
    #endregion

    public static async void ProSnippetsEditingMapTopology()
    {

      #region Variable initialization

      var activeMap = MapView.Active.Map;
      var topologyName = "YourTopologyName"; // replace with your topology name

      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetAvailableTopologiesAsync(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Editing.TopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      #region Get List of available topologies in the map
      // Retrieves and processes the list of available topologies in the active map.
      await QueuedTask.Run(async () =>
      {
        var map = MapView.Active.Map;
        //Get a list of all the available topologies for the map
        var availableTopologies = await map.GetAvailableTopologiesAsync();

        var gdbTopologies = availableTopologies.OfType<GeodatabaseTopologyProperties>();
        var mapTopologies = availableTopologies.OfType<MapTopologyProperties>();
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Editing.TopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      // cref: ArcGIS.Desktop.Editing.NoTopologyProperties
      #region Get the properties of the active topology in the map
      // Retrieves the properties of the active topology associated with the specified map asynchronously.
      var activeTopologyProperties = await activeMap.GetActiveTopologyAsync();
      var isMapTopology = activeTopologyProperties is MapTopologyProperties;
      var isGdbTopology = activeTopologyProperties is GeodatabaseTopologyProperties;
      var isNoTopology = activeTopologyProperties is NoTopologyProperties;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.Tolerance
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.DefaultTolerance
      #region Get map topology properties
      {
        // Retrieves the topology properties for the specified map asynchronously.
        var mapTopoProperties = await activeMap.GetTopologyAsync("Map") as MapTopologyProperties;
        var tolerance_m = mapTopoProperties.Tolerance;
        var defaultTolerance_m = mapTopoProperties.DefaultTolerance;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.WorkspaceName
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.TopologyLayer
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.ClusterTolerance
      #region Get geodatabase topology properties by name
      // Asynchronously retrieves the properties of a geodatabase topology with the specified name from the given map.
      var topoProperties = await activeMap.GetTopologyAsync(topologyName) as GeodatabaseTopologyProperties;

      var workspace = topoProperties.WorkspaceName;
      var topoLayer = topoProperties.TopologyLayer;
      var clusterTolerance = topoProperties.ClusterTolerance;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetMapTopology(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetMapTopologyAsync(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      #region Set Map Topology as the current topology
      // Sets the topology of the specified map as the current map topology asynchronously.
      if (activeMap.CanSetMapTopology())
      {
        //Set the topology of the map as map topology
        var mapTopoProperties = await activeMap.SetMapTopologyAsync() as MapTopologyProperties;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanClearTopology(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.ClearTopologyAsync(ArcGIS.Desktop.Mapping.Map)
      #region Set 'No Topology' as the current topology
      // Removes the current topology from the specified map asynchronously, setting it to have no topology.
      if (activeMap.CanClearTopology())
      {
        //Clears the topology of the map - no topology
        await activeMap.ClearTopologyAsync();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      #region Set the current topology by name
      // Sets the active topology for the specified map to the topology with the given name asynchronously.
      if (activeMap.CanSetActiveTopology(topologyName))
      {
        await activeMap.SetActiveTopologyAsync(topologyName);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
      #region Set the current topology by topologyProperties 
      // Sets the active topology for the specified map using the provided geodatabase topology properties.
      var gdbTopoProperties = await activeMap.GetTopologyAsync("TopologyName") as GeodatabaseTopologyProperties;
      if (activeMap.CanSetActiveTopology(gdbTopoProperties))
      {
        await activeMap.SetActiveTopologyAsync(gdbTopoProperties);
      }
      #endregion

      #region ProSnippet Group: Map Topology
      #endregion

      // cref: ArcGIS.Core.Data.Topology.TopologyDefinition
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.BuildMapTopologyGraph<T>(ArcGIS.Desktop.Mapping.MapView, System.Action<ArcGIS.Core.Data.Topology.TopologyGraph>)
      // cref: ArcGIS.Core.Data.Topology.TopologyGraph.GetNodes()
      // cref: ArcGIS.Core.Data.Topology.TopologyGraph.GetEdges()
      // cref: ArcGIS.Core.Data.Topology.TopologyNode
      // cref: ArcGIS.Core.Data.Topology.TopologyEdge
      #region Build Map Topology
      // Builds the map topology graph for the active map view and displays the number of topology nodes and edges.
      await QueuedTask.Run(() =>
      {
        //Build the map topology graph
        MapView.Active.BuildMapTopologyGraph<TopologyDefinition>(topologyGraph =>
        {
          //Getting the nodes and edges present in the graph
          var topologyGraphNodes = topologyGraph.GetNodes();
          var topologyGraphEdges = topologyGraph.GetEdges();

          foreach (var node in topologyGraphNodes)
          {
            // do something with the node
          }
          foreach (var edge in topologyGraphEdges)
          {
            // do something with the edge
          }
          MessageBox.Show($"Number of topo graph nodes are:  {topologyGraphNodes.Count}.\n Number of topo graph edges are {topologyGraphEdges.Count}.", "Map Topology Info");
        });
      });
      #endregion
    }
  }
}
