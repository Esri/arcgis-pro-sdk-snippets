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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public static class ProSnippetsMapTopology
  {
    #region ProSnippet Group: Topology Properties
    #endregion


    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetAvailableTopologiesAsync(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Editing.TopologyProperties
    // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
    // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
    #region Get List of available topologies in the map
    /// <summary>
    /// Retrieves and processes the list of available topologies in the active map.
    /// </summary>
    /// <remarks>This method runs asynchronously on a background thread to fetch the available topologies for
    /// the currently active map. The topologies can include geodatabase topologies and map topologies, which can be
    /// further processed as needed.</remarks>
    public static void GetAvailableTopologies()
    {
      _ = QueuedTask.Run(async () =>
      {
        var map = MapView.Active.Map;
        //Get a list of all the available topologies for the map
        var availableTopologies = await map.GetAvailableTopologiesAsync();

        var gdbTopologies = availableTopologies.OfType<GeodatabaseTopologyProperties>();
        var mapTopologies = availableTopologies.OfType<MapTopologyProperties>();
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Editing.TopologyProperties
    // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
    // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
    // cref: ArcGIS.Desktop.Editing.NoTopologyProperties
    #region Get the properties of the active topology in the map
    /// <summary>
    /// Retrieves the properties of the active topology associated with the specified map asynchronously.
    /// </summary>
    /// <remarks>The method determines the type of topology currently active in the map, which may be map topology,
    /// geodatabase topology, or no topology. The retrieved properties can be used to inspect or interact with the topology
    /// configuration of the map.</remarks>
    /// <param name="map">The <see cref="ArcGIS.Desktop.Mapping.Map"/> instance for which to obtain active topology properties. Must not be
    /// <c>null</c>.</param>
    public static async void GetActiveTopologyPropertiesAsync(Map map)
    {
      var activeTopologyProperties = await map.GetActiveTopologyAsync();
      var isMapTopology = activeTopologyProperties is MapTopologyProperties;
      var isGdbTopology = activeTopologyProperties is GeodatabaseTopologyProperties;
      var isNoTopology = activeTopologyProperties is NoTopologyProperties;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
    // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
    // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.Tolerance
    // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.DefaultTolerance
    #region Get map topology properties
    /// <summary>
    /// Retrieves the topology properties for the specified map asynchronously.
    /// </summary>
    /// <remarks>This method retrieves the topology properties, including the current tolerance and default
    /// tolerance, for the specified map. The retrieved properties can be used to understand or configure the map's
    /// topology behavior.</remarks>
    /// <param name="map">The <see cref="Map"/> instance for which to retrieve topology properties. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the topology properties have been
    /// retrieved.</returns>
    public static async Task GetMapTopologyPropertiesAsync(Map map)
    {
      var mapTopoProperties = await map.GetTopologyAsync("Map") as MapTopologyProperties;
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
    /// <summary>
    /// Asynchronously retrieves the properties of a geodatabase topology with the specified name from the given map.
    /// </summary>
    /// <remarks>This method obtains topology properties such as workspace name, topology layer, and cluster
    /// tolerance for the specified topology within the provided map. The operation is performed asynchronously and may
    /// require that the map contains a topology with the given name.</remarks>
    /// <param name="map">The <see cref="Map"/> instance from which to retrieve the topology properties. Must not be <c>null</c>.</param>
    /// <param name="topologyName">The name of the geodatabase topology to retrieve. Must not be <c>null</c> or empty.</param>
    /// <returns></returns>
    public static async Task GetGeodatabaseTopologyPropertiesAsync(Map map, string topologyName)
    {
      var topoProperties = await map.GetTopologyAsync(topologyName) as GeodatabaseTopologyProperties;

      var workspace = topoProperties.WorkspaceName;
      var topoLayer = topoProperties.TopologyLayer;
      var clusterTolerance = topoProperties.ClusterTolerance;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetMapTopology(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetMapTopologyAsync(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
    #region Set Map Topology as the current topology
    /// <summary>
    /// Sets the topology of the specified map as the current map topology asynchronously.
    /// </summary>
    /// <remarks>This method checks whether the specified map supports setting map topology before performing
    /// the operation.  If the map does not support map topology, no changes are made and the operation completes
    /// without effect.</remarks>
    /// <param name="map">The <see cref="Map"/> for which to set the map topology. Must support map topology operations.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the map topology has been set.</returns>
    public static async Task SetMapTopologyAsync(Map map)
    {
      if (map.CanSetMapTopology())
      {
        //Set the topology of the map as map topology
        var mapTopoProperties = await map.SetMapTopologyAsync() as MapTopologyProperties;
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanClearTopology(ArcGIS.Desktop.Mapping.Map)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.ClearTopologyAsync(ArcGIS.Desktop.Mapping.Map)
    #region Set 'No Topology' as the current topology
    /// <summary>
    /// Removes the current topology from the specified map asynchronously, setting it to have no topology.
    /// </summary>
    /// <remarks>This method has no effect if the map does not support clearing topology. To determine if
    /// topology can be cleared, use <see cref="ArcGIS.Desktop.Mapping.MappingExtensions.CanClearTopology(Map)"/> before
    /// calling this method.</remarks>
    /// <param name="map">The <see cref="Map"/> instance whose topology will be cleared. Must support clearing topology.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task ClearTopologyAsync(Map map)
    {
      if (map.CanClearTopology())
      {
        //Clears the topology of the map - no topology
        await map.ClearTopologyAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, System.String)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
    #region Set the current topology by name
    /// <summary>
    /// Sets the active topology for the specified map to the topology with the given name asynchronously.
    /// </summary>
    /// <remarks>This method checks whether the specified topology can be set as active before attempting to
    /// set it. If the topology cannot be set as active, no changes are made and no exception is thrown.</remarks>
    /// <param name="map">The <see cref="Map"/> instance for which to set the active topology. Cannot be <c>null</c>.</param>
    /// <param name="topologyName">The name of the topology to activate. Must correspond to an existing topology in the map.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task SetActiveTopologyAsync(Map map, string topologyName)
    {
      if (map.CanSetActiveTopology(topologyName))
      {
        await map.SetActiveTopologyAsync(topologyName);
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
    #region Set the current topology by topologyProperties 
/// <summary>
/// Sets the active topology for the specified map using the provided geodatabase topology properties.
/// </summary>
/// <remarks>This method sets the active topology only if the map supports the specified topology properties. If
/// the topology cannot be set, no changes are made and no exception is thrown.</remarks>
/// <param name="map">The map for which to set the active topology. Must not be <c>null</c>.</param>
/// <param name="gdbTopoProperties">The geodatabase topology properties to apply as the active topology. Must not be <c>null</c>.</param>
/// <returns>A task that represents the asynchronous operation of setting the active topology.</returns>
    public static async Task SetActiveTopologyAsync(Map map, GeodatabaseTopologyProperties gdbTopoProperties)
    {
      if (map.CanSetActiveTopology(gdbTopoProperties))
      {
        await map.SetActiveTopologyAsync(gdbTopoProperties);
      }
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
    /// <summary>
    /// Builds the map topology graph for the active map view and displays the number of topology nodes and edges.
    /// </summary>
    /// <remarks>This method executes asynchronously on the ArcGIS QueuedTask and uses the active <see
    /// cref="ArcGIS.Desktop.Mapping.MapView"/> to construct a topology graph based on the current map's topology
    /// definition. After building the graph, it retrieves all nodes and edges and displays their counts in a message
    /// box. The method is intended for use within the ArcGIS Pro add-in environment and requires an active map view
    /// with a valid topology.</remarks>
    /// <returns></returns>
    public static async Task BuildGraphWithActiveView()
    {
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
    }
    #endregion

  }
}
