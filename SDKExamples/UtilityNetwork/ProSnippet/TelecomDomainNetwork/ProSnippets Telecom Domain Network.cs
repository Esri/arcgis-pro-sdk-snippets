/*

   Copyright 2018 Esri

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

using ArcGIS.Core.Data;
using ArcGIS.Core.Data.UtilityNetwork;
using ArcGIS.Core.Data.UtilityNetwork.Telecom;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Field = ArcGIS.Core.Data.Field;
using Geometry = ArcGIS.Core.Geometry.Geometry;
using NetworkSource = ArcGIS.Core.Data.UtilityNetwork.NetworkSource;
using TraceConfiguration = ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration;

namespace ProSnippets.NetworkSnippets.TelecomDomainNetworkSnippets
{
  public static partial class ProSnippetsTelecomDomainNetwork
  {
    /// <summary>
    /// This class contains a collection of code snippets that demonstrate how to work with the telecom domain network in ArcGIS Pro.
    /// </summary>
    /// 
    /// <remarks>
    /// Each region in the class contains a code snippet that demonstrates a specific piece of functionality.
    /// </remarks>
    public static async void ProSnippetsTelecomDomainNetworkAsync()
    {
      #region ignore - Variable initialization

      using Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Path\To\Your\Geodatabase.gdb")));
      using UtilityNetwork utilityNetwork = geodatabase.OpenDataset<UtilityNetwork>("UtilityNetworkName");
      TelecomDomainNetwork telecomDomainNetwork = utilityNetwork.GetDefinition().GetDomainNetworks().OfType<TelecomDomainNetwork>().FirstOrDefault();
      NetworkSource networkSource = telecomDomainNetwork.NetworkSources.FirstOrDefault(ns => ns.Name.Contains("TelcoDevice"));
      string assetGroupName = "Fiber";
      string assetTypeName = "Fiber";
      string assetDescription = "Fiber";
      List<string> circuitNames = new List<string> { "LongIslandToLondonCircuit", "VirtualPhillyCircuit", "JacksonVilleToMaineCircuit" };
      Uri circuitExportPath = new Uri(@"C:\Path\To\Your\Location");
      Guid globalID = new Guid("5aee3391-bd00-4012-a9fd-d87792e715a4");
      TelecomElement startElement = utilityNetwork.CreateElement(networkSource.GetAssetGroup(assetTypeName).GetAssetType(assetTypeName), globalID) as TelecomElement;
      TelecomElement stopElement = utilityNetwork.CreateElement(networkSource.GetAssetGroup(assetTypeName).GetAssetType(assetTypeName), globalID) as TelecomElement;

      #endregion

      #region ProSnippet Group: Telecom Domain Network
      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.TelecomDomainNetwork
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.WavelengthScheme
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetCircuitManager(ArcGIS.Core.Data.UtilityNetwork.Telecom.TelecomDomainNetwork)
      #region Get the telecom domain network and network information from the utility network

      await QueuedTask.Run(() =>
      {
        if (!utilityNetwork.HasTelecomNetwork)
        {
          Console.WriteLine("The utility network does not have a telecom domain network.");
        }

        // Get the telecom domain network from the utility network definition.
        IReadOnlyList<DomainNetwork> domainNetworks = utilityNetwork.GetDefinition().GetDomainNetworks();
        foreach (DomainNetwork domainNetwork in domainNetworks)
        {
          if (domainNetwork is TelecomDomainNetwork tdn)
          {
            // The telecom domain network was found.
            telecomDomainNetwork = tdn;

            // Get information about circuit properties from the telecom domain network.
            CircuitProperties circuitProperties = telecomDomainNetwork.CircuitProperties;
            Console.WriteLine($"Maximum hops in a circuit: {circuitProperties.MaxHops}");
            Console.WriteLine($"Number of paths in a circuit: {circuitProperties.NumPaths}");

            // Get the circuit color scheme information
            IReadOnlyList<ColorScheme> colorSchemes = telecomDomainNetwork.ColorSchemes;
            foreach (ColorScheme colorScheme in colorSchemes)
            {
              string colorSchemeName = colorScheme.Name;
              int colorSchemeID = colorScheme.ID;

              IReadOnlyList<ColorSchemeGroup> colorSchemeGroups = colorScheme.Groups;
              foreach (ColorSchemeGroup group in colorSchemeGroups)
              {
                Console.WriteLine($"Color Scheme: {colorSchemeName}, Group: {group.Name}");

                // Color codes
                IReadOnlyList<ColorCode> colorCodes = group.ColorCodes;
                foreach (ColorCode colorCode in colorCodes)
                {
                  Console.WriteLine($"Color Code: {colorCode}");
                }

                // Capacities
                IReadOnlyList<int> capacities = group.Capacity;
                foreach (int capacity in capacities)
                {
                  Console.WriteLine($"Capacity: {capacity}");
                }
              }
            }
            
            // Use the telecom domain network to get additional information about the network.
            IReadOnlyList<WavelengthScheme> wavelengthSchemes = telecomDomainNetwork.WavelengthSchemes;
            IReadOnlyList<DividePolicy> dividePolicies = telecomDomainNetwork.DividePolicies;
            IReadOnlyList<CombinePolicy> combinePolicies = telecomDomainNetwork.CombinePolicies;
            IReadOnlyList<string> diagramTemplateNames = telecomDomainNetwork.DiagramTemplateNames;
          }
        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitLocation.#ctor(ArcGIS.Core.Data.UtilityNetwork.Element)
      #region Create a Telecom Element and Circuit Location in the utility network

      await QueuedTask.Run(() =>
      {
        using (NetworkSource networkSource = telecomDomainNetwork.NetworkSources.FirstOrDefault(ns => ns.Name.Contains(assetGroupName)))
        using (AssetGroup assetGroup = networkSource.GetAssetGroup(assetTypeName))
        using (AssetType assetType = assetGroup.GetAssetType(assetTypeName))
        {
          if (assetType == null)
          {
            throw new Exception($"Asset type '{assetTypeName}' not found in group '{assetGroupName}'.");
          }

          TelecomElement telecomElement = utilityNetwork.CreateElement(assetType, globalID) as TelecomElement;

          // Use the telecom element to create a circuit location.
          CircuitLocation circuitLocation = new CircuitLocation(telecomElement);
        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.Create(ArcGIS.Core.Data.UtilityNetwork.Telecom.Circuit)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.GetSubcircuitUserFields
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.GetCircuitUserFields
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.Circuit.SetSubcircuits(System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.UtilityNetwork.Telecom.Subcircuit})
      #region Create a physical circuit (non-sectioned) in the utility network with subcircuits
      await QueuedTask.Run(() =>
      {
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        {
          // Create a non-sectioned circuit object
          Circuit circuit = new Circuit(circuitManager);
          circuit.SetName(circuitNames.First());
          circuit.SetSectioned(false);
          circuit.SetCircuitType(CircuitType.Physical);

          // Set a circuit user attribute
          IReadOnlyList<Field> circuitFields = circuitManager.GetCircuitUserFields();
          circuit[circuitFields.First().Name] = "My circuit description";

          // Add start and stop locations
          circuit.SetStartLocation(new CircuitLocation(startElement));
          circuit.SetStopLocation(new CircuitLocation(stopElement));

          Subcircuit subcircuit1 = new Subcircuit(circuitManager);
          subcircuit1.SetName("Subcircuit1");

          // Set a subcircuit user attribute
          IReadOnlyList<Field> subcircuitFields = circuitManager.GetSubcircuitUserFields();
          subcircuit1[subcircuitFields[0].Name] = "My subcircuit description";

          Subcircuit subcircuit2 = new Subcircuit(circuitManager);
          subcircuit2.SetName("Subcircuit2");
          subcircuit2.SetProviderID(new Guid("9B587BB4-FA30-4AC8-ACAC-4CB1BB087111")); // Example GUID 

          // Add subcircuits to the circuit
          circuit.SetSubcircuits(new List<Subcircuit> {
                        subcircuit1,
                        subcircuit2
              });

          // Create the circuit.

          circuitManager.Create(circuit);

        }

      });
      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.Circuit.SetCircuitSections(System.Collections.Generic.IReadOnlyDictionary{ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitSection,System.Collections.Generic.List{ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitSection}})
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.Create(ArcGIS.Core.Data.UtilityNetwork.Telecom.Circuit)
      #region Create a virtual sectioned circuit in the utility network
      await QueuedTask.Run(() =>
      {
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        {
          // Create a sectioned virtual circuit object
          Circuit circuit = new Circuit(circuitManager);
          circuit.SetName(circuitNames.First());
          circuit.SetSectioned(true);
          circuit.SetCircuitType(CircuitType.Virtual);

          CircuitSection circuitSection1 = new CircuitSection(circuitManager);
          circuitSection1.SetSectionType(CircuitSectionType.Virtual);
          circuitSection1.SetStartLocation(new CircuitLocation(startElement));
          circuitSection1.SetStopLocation(new CircuitLocation(stopElement));

          CircuitSection circuitSection2 = new CircuitSection(circuitManager);
          circuitSection2.SetSectionType(CircuitSectionType.Physical);
          circuitSection2.SetStartLocation(new CircuitLocation(startElement));
          circuitSection2.SetStopLocation(new CircuitLocation(stopElement));

          // Add sections in parallel
          Dictionary<CircuitSection, List<CircuitSection>> circuitSections = new Dictionary<CircuitSection,
                    List<CircuitSection>> {
                        {
                            circuitSection1,
                            new List < CircuitSection > ()
                        },
                        {
                            circuitSection2,
                            new List < CircuitSection > ()
                        }
              };

          circuit.SetCircuitSections(circuitSections);

          // Create the circuit.

          circuitManager.Create(circuit);
        }
      });
      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.Alter(ArcGIS.Core.Data.UtilityNetwork.Telecom.Circuit)
      #region Alter an existing circuit in the utility network
      await QueuedTask.Run(() =>
      {
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        {
          // Get the existing circuit
          Circuit circuit = circuitManager.GetCircuits(new CircuitFilter(["circuitName"])).First();

          // Update circuit name
          circuit.SetName(circuitNames.First());

          // Update start and stop locations
          circuit.SetStartLocation(new CircuitLocation(startElement));
          circuit.SetStopLocation(new CircuitLocation(stopElement));

          // Clear existing subcircuits
          circuit.SetSubcircuits(new List<Subcircuit>());

          // Apply the changes
          circuitManager.Alter(circuit);
        }
      });
      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.Delete(System.Collections.Generic.IEnumerable{System.String})
      #region Delete circuits from the telecom domain network

      await QueuedTask.Run(() =>
      {
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        {
          // Delete circuits by name
          circuitManager.Delete(circuitNames);
        }

      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.GetCircuits(ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitFilter)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.GetCircuitNames(ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitFilter)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitFilter.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitLocation},ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitLocationTypeFilter)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitFilter.#ctor(System.Collections.Generic.IEnumerable{System.Guid})
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitFilter.#ctor(System.Collections.Generic.IEnumerable{System.String})
      #region Get all circuits at a specific location within the utility network

      await QueuedTask.Run(() =>
      {
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        {
          // Define a location in the utility network.
          CircuitLocation location = new CircuitLocation(startElement);

          // Get circuit names from the start location
          CircuitFilter circuitFilter = new CircuitFilter([location], CircuitLocationTypeFilter.All) { Hierarchy = CircuitHierarchy.ConsumersAndProviders };
          IReadOnlyList<string> circuitNames = circuitManager.GetCircuitNames(circuitFilter);

          // Get circuits from the start location
          IReadOnlyList<Circuit> circuits = circuitManager.GetCircuits(circuitFilter);

          // Get circuits by names
          circuitFilter = new CircuitFilter(circuitNames);
          circuits = circuitManager.GetCircuits(circuitFilter);

          // Iterate through the circuits.
          foreach (Circuit circuit in circuits)
          {
            Console.WriteLine($"Circuit name: {circuit.GetName()}");
          }
        }

      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitVerifyResult
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitVerifyOptions.#ctor
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitManager.Verify(System.Collections.Generic.IEnumerable{System.String},ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitVerifyOptions)
      #region Verify a circuit to check for inconsistencies in the utility network

      await QueuedTask.Run(() =>
      {
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        {
          IReadOnlyList<CircuitVerifyResult> circuitVerifyResults = circuitManager.Verify(circuitNames, new CircuitVerifyOptions() { SynthesizeGeometries = true });

          // Iterate through the circuit verification results
          foreach (CircuitVerifyResult circuitVerifyResult in circuitVerifyResults)
          {
            Console.WriteLine($"Circuit Name: {circuitVerifyResult.Name}, Geometry: {circuitVerifyResult.Geometry.ToJson()}, " +
                                    $"HasError: {circuitVerifyResult.HasError}, Message: {circuitVerifyResult.Message}");
          }
        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetUnitIdentifierManager
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifierManager.Query(System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifier})
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitRange
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifier
      #region Query the contents of equipment containers in the utility network

      await QueuedTask.Run(() =>
      {
        using (UnitIdentifierManager unitIdentifierManager = utilityNetwork.GetUnitIdentifierManager())
        {
          var networkSource = utilityNetwork.GetDefinition().GetNetworkSource("TelecomDevice");

          // Container to query.
          UnitIdentifier containerUnitId = new UnitIdentifier(networkSource, globalID);

          // Query the unit identifiers in the container
          IReadOnlyList<UnitQueryResult> unitQueryResults = unitIdentifierManager.Query(new List<UnitIdentifier> { containerUnitId });

          // Iterate through the query results
          foreach (UnitQueryResult unitQueryResult in unitQueryResults)
          {
            UnitIdentifier unitIdentifier = unitQueryResult.Container;
            IReadOnlyList<UnitRange> unitsInAContainer = unitQueryResult.UnitRanges;

            // Iterate through the unit ranges (contents) in the container
            foreach (UnitRange unitRange in unitsInAContainer)
            {
              UnitIdentifier contentUnitId = unitRange.Content;
              bool isContentGap = unitRange.IsGap; // True if the content is a gap (empty space reserved for installing a larger piece of equipment).
              short? firstUnit = unitRange.FirstUnit;
              short? lastUnit = unitRange.LastUnit;
            }
          }
        }

      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifier
      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifierManager.ReserveUnitIDs(ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifier,System.Int16,System.Int16)
      #region Reserve a gap in an equipment container in the utility network

      await QueuedTask.Run(() =>
      {
        using (UnitIdentifierManager unitIdentifierManager = utilityNetwork.GetUnitIdentifierManager())
        {
          var networkSource = utilityNetwork.GetDefinition().GetNetworkSource("TelecomDevice");
          UnitIdentifier containerUnitIdentifier = new UnitIdentifier(networkSource, globalID);

          // Reserve a gap in the container for units 2 through 5.
          unitIdentifierManager.ReserveUnitIDs(containerUnitIdentifier, 2, 5);
        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifierManager.Reset(System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifier})
      #region Reset the contents of a container in the utility network to remove gaps and make the unit space contiguous

      await QueuedTask.Run(() =>
      {
        using (UnitIdentifierManager unitIdentifierManager = utilityNetwork.GetUnitIdentifierManager())
        {
          var networkSource = utilityNetwork.GetDefinition().GetNetworkSource("TelecomDevice");
          UnitIdentifier containerUnitIdentifier = new UnitIdentifier(networkSource, globalID);

          // Reset the container to remove all gaps and make the unit space contiguous.
          unitIdentifierManager.Reset(new[] { containerUnitIdentifier });

        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifierManager.Resize(ArcGIS.Core.Data.UtilityNetwork.Telecom.UnitIdentifier,System.Int16)
      #region Resize the content of a container in the utility network

      await QueuedTask.Run(() =>
      {
        using (UnitIdentifierManager unitIdentifierManager = utilityNetwork.GetUnitIdentifierManager())
        {
          var networkSource = utilityNetwork.GetDefinition().GetNetworkSource("TelecomDevice");
          UnitIdentifier content = new UnitIdentifier(networkSource, globalID);

          // Update the number of units in an equipment container.
          unitIdentifierManager.Resize(content, 5);

        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.TelecomElement
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.PathTracer
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TracePath
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.PathConnectivity
      #region Run a path trace in the telecom domain network

      await QueuedTask.Run(() =>
      {
        using (TraceManager traceManager = utilityNetwork.GetTraceManager())
        using (NetworkSource deviceNetworkSource = telecomDomainNetwork.NetworkSources.First(f => f.Name.Contains("TelecomDevice")))
        using (AssetGroup assetGroup = deviceNetworkSource.GetAssetGroup("Port"))
        using (AssetType assetType = assetGroup.GetAssetType("Port"))
        {
          Guid startGuid = new Guid("4BB4560A-34F3-4BBA-93D5-C5D6B26F2709");
          Guid stopGuid = new Guid("32203C76-E484-473A-AEA5-B1AB92A0C198");

          TelecomElement startElement = utilityNetwork.CreateElement(assetType, startGuid) as TelecomElement;
          TelecomElement stopElement = utilityNetwork.CreateElement(assetType, stopGuid) as TelecomElement;

          List<TelecomElement> startingLocations = new List<TelecomElement>() { startElement };
          List<TelecomElement> stoppingLocations = new List<TelecomElement>() { stopElement };

          TraceConfiguration traceConfiguration = new TraceConfiguration()
          {
            MaxHops = 100,
            NumPaths = 2,
            DomainNetwork = telecomDomainNetwork,
            ValidateConsistency = true
          };

          TraceArgument traceArgument = new TraceArgument(startingLocations)
          {
            StoppingLocations = stoppingLocations,
            Configuration = traceConfiguration,
            ResultTypes = new List<ResultType>() { ResultType.Path },
            ResultOptions = new ResultOptions { IncludeGeometry = true }
          };

          PathTracer pathTracer = traceManager.GetTracer<PathTracer>();
          IReadOnlyList<Result> results = pathTracer.Trace(traceArgument, ServiceSynchronizationType.Asynchronous);
          // Iterate through the path trace results
          foreach (Result result in results)
          {
            if (result is PathResult pathResult)
            {
              IReadOnlyList<TracePath> paths = pathResult.Paths;
              foreach (TracePath path in paths)
              {
                Element startLocation = path.StartLocation;
                Element stopLocation = path.StopLocation;
                Geometry pathGeometry = path.Geometry;
                IReadOnlyList<PathConnectivity> pathConnectivities = path.PathConnectivities;

                foreach (PathConnectivity pathConnectivity in pathConnectivities)
                {
                  Console.WriteLine($"Edge: {pathConnectivity.Edge.GlobalID}, Junction: {pathConnectivity.Junction.GlobalID}");
                }
              }
            }
            else
            {
              Console.WriteLine("Unexpected result type.");
            }
          }
        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.CircuitResult
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.PathConnectivity
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.CircuitTracer
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.CircuitResult.CircuitPaths
      #region Run a circuit trace in the telecom domain network

      await QueuedTask.Run(() =>
      {
        using (TraceManager traceManager = utilityNetwork.GetTraceManager())
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        {
          Circuit circuitToTrace = circuitManager.GetCircuits(new CircuitFilter(circuitNames)).FirstOrDefault();

          TraceConfiguration traceConfiguration = new TraceConfiguration()
          {
            MaxHops = 10,
            NumPaths = 5,
            DomainNetwork = telecomDomainNetwork,
            InferConnectivity = true
          };

          TraceArgument traceArgument = new TraceArgument(circuitToTrace)
          {
            Configuration = traceConfiguration,
            ResultTypes = new List<ResultType> { ResultType.Circuit },
            ResultOptions = new ResultOptions() { IncludeGeometry = true }
          };

          CircuitTracer circuitTracer = traceManager.GetTracer<CircuitTracer>();
          IReadOnlyList<Result> results = circuitTracer.Trace(traceArgument);

          // Iterate through the circuit trace results
          foreach (Result result in results)
          {
            if (result is CircuitResult circuitResult)
            {
              IReadOnlyList<CircuitPath> circuitPaths = circuitResult.CircuitPaths;
              foreach (CircuitPath circuitPath in circuitPaths)
              {
                Circuit circuit = circuitPath.Circuit;
                TracePath path = circuitPath.Path;
                IReadOnlyList<PathConnectivity> pathConnectivities = path.PathConnectivities;

                foreach (PathConnectivity pathConnectivity in pathConnectivities)
                {
                  Console.WriteLine($"Edge: {pathConnectivity.Edge.GlobalID}, Junction: {pathConnectivity.Junction.GlobalID}");
                }
                Geometry geometry = circuitPath.Geometry;
                IReadOnlyDictionary<int, TracePath> sectionPaths = circuitPath.SectionPaths;

                foreach (KeyValuePair<int, TracePath> sectionPath in sectionPaths)
                {
                  int sectionID = sectionPath.Key;
                  TracePath sectionGeometry = sectionPath.Value;
                  Console.WriteLine($"Section ID: {sectionID}, Section Geometry: {sectionGeometry.Geometry.ToJson()}");
                }
              }
            }
            else
            {
              Console.WriteLine("Unexpected result type.");
            }
          }
        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Telecom.CircuitExportOptions.#ctor
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceExportOptions
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.CircuitTracer.Export(System.Uri,ArcGIS.Core.Data.UtilityNetwork.Trace.TraceArgument,ArcGIS.Core.Data.UtilityNetwork.Trace.TraceExportOptions)
      #region Export circuits

      await QueuedTask.Run(() =>
      {
        using (TraceManager traceManager = utilityNetwork.GetTraceManager())
        using (CircuitManager circuitManager = utilityNetwork.GetCircuitManager(telecomDomainNetwork))
        using (UtilityNetworkDefinition utilityNetworkDefinition = utilityNetwork.GetDefinition())
        {
          IReadOnlyList<NetworkSource> networkSources = utilityNetworkDefinition.GetNetworkSources();
          IReadOnlyList<NetworkAttribute> networkAttributes = utilityNetworkDefinition.GetNetworkAttributes();
          IEnumerable<string> networkAttributeNames = networkAttributes.Select(s => s.Name);

          NetworkSource telcoDeviceNWSource = networkSources.First(f => f.Name.Contains("TelcoDevice"));
          NetworkSource telcoCircuit = networkSources.First(f => f.Name.Contains("TelcoCircuit"));

          TraceConfiguration traceConfiguration = new TraceConfiguration()
          {
            InferConnectivity = false,
            DomainNetwork = telecomDomainNetwork,
            IncludeContainers = true,
            IncludeContent = true,
            SynthesizeGeometries = false
          };

          ResultOptions resultOptions = new ResultOptions()
          {
            IncludeGeometry = true,
            NetworkAttributes = networkAttributeNames.ToList(),
            ResultFields = new Dictionary<NetworkSource, List<string>>() { { telcoDeviceNWSource, ["NEXTUNITID", "PARTITIONID"] },
              { telcoCircuit, ["LongTestField", "TextTestField", "DateTestField"] }}
          };

          CircuitExportOptions circuitExportOptions = new CircuitExportOptions()
          {
            IncludeDomainDescriptions = true,
            ResultOptions = resultOptions,
            ResultTypes = new List<ResultType> { ResultType.Circuit },
            ServiceSynchronizationType = ServiceSynchronizationType.Asynchronous,
            SetAcknowledged = false,
            TraceConfiguration = traceConfiguration
          };

          circuitManager.Export(circuitNames, circuitExportOptions, circuitExportPath);
        }
      });

      #endregion

      // cref: ArcGIS.Core.Data.UtilityNetwork.Association.#ctor(ArcGIS.Core.Data.UtilityNetwork.AssociationType,ArcGIS.Core.Data.UtilityNetwork.Element,ArcGIS.Core.Data.UtilityNetwork.Element)
      #region Add or delete an association between Telecom elements in the utility network

      await QueuedTask.Run(() =>
      {
        using (NetworkSource junctionNetworkSource = telecomDomainNetwork.NetworkSources.FirstOrDefault(ns => ns.Name.Contains("TelcoDevice")))
        using (NetworkSource edgeNetworkSource = telecomDomainNetwork.NetworkSources.FirstOrDefault(ns => ns.Name.Contains("TelcoEdgeObject")))
        {
          TelecomElement junction = utilityNetwork.CreateElement(junctionNetworkSource.GetAssetGroup("Fiber").GetAssetType("Fiber"),
                    new Guid("CCFCF5DE-5B90-4FD9-A459-2F2E88107532")) as TelecomElement;

          TelecomElement edgeObject = utilityNetwork.CreateElement(edgeNetworkSource.GetAssetGroup("Fiber").GetAssetType("Fiber"),
                    new Guid("838B4A2B-8531-499D-8DCF-8567901D43D9")) as TelecomElement;

          Association association = new Association(AssociationType.JunctionEdgeObjectConnectivityMidspan, junction, edgeObject, 0.3);

          // Create the association from the port to the fiber.
          geodatabase.ApplyEdits(() =>
                {
                  utilityNetwork.AddAssociation(association);
                });
          // Delete the association
          geodatabase.ApplyEdits(() =>
                {
                  utilityNetwork.DeleteAssociation(association);
                });
        }

      });

      #endregion
    }
  }
}
