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

namespace ProSnippetsEditing
{
  public static class ProSnippetsMapTopology
  {
    #region ProSnippet Group: Topology Properties
    #endregion

    public static async void Topology()
    {
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetAvailableTopologiesAsync(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Editing.TopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      #region Get List of available topologies in the map

      _= QueuedTask.Run(async () =>
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
      var map = MapView.Active.Map;
      var activeTopologyProperties = await map.GetActiveTopologyAsync();
      var isMapTopology = activeTopologyProperties is MapTopologyProperties;
      var isGdbTopology = activeTopologyProperties is GeodatabaseTopologyProperties;
      var isNoTopology = activeTopologyProperties is NoTopologyProperties;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.Tolerance
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.DefaultTolerance
      #region Get map topology properties 
      var mapTopoProperties = await map.GetTopologyAsync("Map") as MapTopologyProperties;
      var tolerance_m = mapTopoProperties.Tolerance;
      var defaultTolerance_m = mapTopoProperties.DefaultTolerance;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.WorkspaceName
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.TopologyLayer
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.ClusterTolerance
      #region Get geodatabase topology properties by name
      var topoProperties = await map.GetTopologyAsync("TopologyName") as GeodatabaseTopologyProperties;

      var workspace = topoProperties.WorkspaceName;
      var topoLayer = topoProperties.TopologyLayer;
      var clusterTolerance = topoProperties.ClusterTolerance;

      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetMapTopology(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetMapTopologyAsync(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      #region Set Map Topology as the current topology
      if (map.CanSetMapTopology())
      {
        //Set the topology of the map as map topology
        mapTopoProperties = await map.SetMapTopologyAsync() as MapTopologyProperties;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanClearTopology(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.ClearTopologyAsync(ArcGIS.Desktop.Mapping.Map)
      #region Set 'No Topology' as the current topology

      if (map.CanClearTopology())
      {
        //Clears the topology of the map - no topology
        await map.ClearTopologyAsync();
      }

      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      #region Set the current topology by name

      if (map.CanSetActiveTopology("TopologyName"))
      {
        await map.SetActiveTopologyAsync("TopologyName");
      }

      #endregion

      GeodatabaseTopologyProperties gdbTopoProperties = null;

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
      #region Set the current topology by topologyProperties 

      if (map.CanSetActiveTopology(gdbTopoProperties))
      {
        await map.SetActiveTopologyAsync(gdbTopoProperties);
      }

      #endregion
    }

    #region ProSnippet Group: Map Topology
    #endregion

    // cref: ArcGIS.Core.Data.Topology.TopologyDefinition
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.BuildMapTopologyGraph<T>(ArcGIS.Desktop.Mapping.MapView, System.Action<ArcGIS.Core.Data.Topology.TopologyGraph>)
    // cref: ArcGIS.Core.Data.Topology.TopologyGraph.GetNodes()
    // cref: ArcGIS.Core.Data.Topology.TopologyGraph.GetEdges()
    // cref: ArcGIS.Core.Data.Topology.TopologyNode
    // cref: ArcGIS.Core.Data.Topology.TopologyEdge
    #region Build Map Topology
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
