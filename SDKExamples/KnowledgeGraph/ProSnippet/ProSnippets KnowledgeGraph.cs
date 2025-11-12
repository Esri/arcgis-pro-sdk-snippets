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
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.DDL.Knowledge;
using ArcGIS.Core.Data.DDL;
using ArcGIS.Core.Data.Exceptions;
using ArcGIS.Core.Data.Knowledge;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Catalog;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.KnowledgeGraph;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Core.Data.Knowledge.Analytics;
using ArcGIS.Core.Data.Knowledge.Extensions;

namespace ProSnippets.KnowledgeGraphSnippets
{

	public static partial class ProSnippetsKnowledgeGraph
	{
		/// <summary>
		/// This methods has a collection of code snippets related to working with ArcGIS Pro knowledge graph.
		/// </summary>
		/// <remarks>
		/// The code snippets in this class are intended to be used as examples of how to work with knowledge graph in ArcGIS Pro.
		/// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
		/// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
		/// Crefs are used for internal purposes only. Please ignore them in the context of this example.
		/// </remarks>
		public static async void ProSnippetsKnowledgeGraphs()
		{
			#region ignore - Variable initialization

			//A kg service url
			//https://server_url.com/server/rest/services/Hosted/KG_Name/KnowledgeGraphServer";
			var url =
				@"https://acme.server.com/server/rest/services/Hosted/AcmeKnowledgeGraph/KnowledgeGraphServer";

			//Different ways to reference a knowledge graph
			var kg_props = new KnowledgeGraphConnectionProperties(new Uri(url));
			var kg = new KnowledgeGraph(kg_props);

			var kgLayer = MapView.Active.Map.GetLayersAsFlattenedList()
						.OfType<KnowledgeGraphLayer>().FirstOrDefault();
			var kgFromLayer = kgLayer.GetDatastore();

			//Reference a map
			Map map = MapView.Active.Map;
			//Or
			MapProjectItem mapProjectItem = Project.Current.GetItems<MapProjectItem>()
																			 .FirstOrDefault(); //Reference to a map project item
			var mapFromProjectItem = mapProjectItem.GetMap();

			//KG Row Cursor
			var kg_qry_filter = new KnowledgeGraphQueryFilter()
			{
				QueryText = "Open-cypher-query-string-here"
			};
			var kgRowCursor = kg.SubmitQuery(kg_qry_filter);

			MapMember mapMember = MapView.Active.Map.GetLayersAsFlattenedList()
												.OfType<MapMember>().First();
			var entityLayer = kgLayer.GetLayersAsFlattenedList()
												.OfType<FeatureLayer>().First() as MapMember;
			var entityLayer2 = kgLayer.GetLayersAsFlattenedList()
												.OfType<FeatureLayer>().Skip(1) as MapMember;
			List<long> oids = new List<long>();
			List<long> oids2 = new List<long>();
			var list_of_ids = new List<object>();
			var list_of_ids2 = new List<object>();

			var docLayer = kgLayer.GetLayersAsFlattenedList()
												.OfType<Layer>().First(l => l.Name == "Documents");
			Layer personLayer = kgLayer.GetLayersAsFlattenedList()
												.OfType<Layer>().First(l => l.Name == "Person");

			Dictionary<string, object> personAtts = new();

			var kgSubgraph = new CIMKnowledgeGraphSubGraph();
			var kgConfig = new CIMKnowledgeGraphCentralityConfiguration();
			var measures = new CentralityMeasure[0];
			var kgResults = kg.ComputeCentrality(kgConfig, kgSubgraph, measures);
			object uid = "";

			#endregion

			#region ProSnippet Group: KnowledgeGraph Datastore
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.#ctor(ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.#ctor(System.Uri)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph
			#region Opening a Connection to a KnowledgeGraph
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					try
					{
						//Open a connection
						using (var kg = new KnowledgeGraph(kg_props))
						{
							//TODO...use the KnowledgeGraph
						}
					}
					catch (GeodatabaseNotFoundOrOpenedException ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.ToString());
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetDatastore
			#region Getting a Connection from a KnowledgeGraphLayer 
			{
				//var kgLayer = MapView.Active.Map.GetLayersAsFlattenedList()
				//			.OfType<KnowledgeGraphLayer>().FirstOrDefault();

				await QueuedTask.Run(() =>
				{
					// use the layer directly
					KnowledgeGraph datastore = kgLayer.GetDatastore();

					// or you can use any of the sub items since
					//KnowledgeGraphLayer is a composite layer - get the first 
					// child feature layer or standalone table
					var featlayer = kgLayer?.GetLayersAsFlattenedList()?
													.OfType<FeatureLayer>()?.FirstOrDefault();
					KnowledgeGraph kg = null;
					if (featlayer != null)
					{
						using (var fc = featlayer.GetFeatureClass())
							kg = fc.GetDatastore() as KnowledgeGraph;
						//TODO use KnowledgeGraph
					}
					else
					{
						//try standalone table
						var stbl = kgLayer?.GetStandaloneTablesAsFlattenedList()?
														.FirstOrDefault();
						if (stbl != null)
						{
							using (var tbl = stbl.GetTable())
								kg = tbl.GetDatastore() as KnowledgeGraph;
							//TODO use KnowledgeGraph
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.#ctor(ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.#ctor(System.Uri)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDefinitions<T>
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.OpenDataset<T>(string)
			#region Retrieving GDB FeatureClasses and Definitions 
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					//Connect to the KnowledgeGraph datastore
					//KnowledgeGraph datastores contain tables and feature classes
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the featureclass definitions from the KG datastore
						var fc_defs = kg.GetDefinitions<FeatureClassDefinition>();
						//For each feature class definition, get the corresponding
						//feature class. Note: The name of the feature class will match 
						//the name of its corresponding KnowledgeGraph named object type
						//in the KnowledgeGraph graph data model
						foreach (var fc_def in fc_defs)
						{
							var fc_name = fc_def.GetName();
							using (var fc = kg.OpenDataset<FeatureClass>(fc_name))
							{
								//TODO - use the feature class
							}
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.#CTOR(ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.#CTOR(System.Uri)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDefinitions<T>
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.OpenDataset<T>(string)
			#region Retrieving GDB Tables and Definitions
			{
				QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					//Connect to the KnowledgeGraph datastore
					//KnowledgeGraph datastores contain tables and feature classes
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the table definitions from the KG datastore
						var tbl_defs = kg.GetDefinitions<TableDefinition>();
						//For each table definition, get the corresponding
						//table. Note: The name of the table will match the name
						//of its corresponding KnowledgeGraph named object type in
						//the KnowledgeGraph graph data model
						foreach (var tbl_def in tbl_defs)
						{
							var tbl_name = tbl_def.GetName();
							using (var fc = kg.OpenDataset<Table>(tbl_name))
							{
								//TODO - use the table
							}
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetConnector()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.URL
			#region Get service Uri from KG datastore
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
									new KnowledgeGraphConnectionProperties(new Uri(url));
					//Connect to the KnowledgeGraph datastore
					//KnowledgeGraph datastores contain tables and feature classes
					using (var kg = new KnowledgeGraph(kg_props))
					{
						var connector = kg.GetConnector() as KnowledgeGraphConnectionProperties;
						var uri = connector.URL;
						var serviceUri = uri.AbsoluteUri;
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.TransformToIDs(System.string, System. System.Collections.Generic.IEnumerable{System.Int64})
			#region Transform a set of objectIDs to IDs for an entity
			{
				await QueuedTask.Run(() =>
				{
					var kg_props =
									new KnowledgeGraphConnectionProperties(new Uri(url));
					//Connect to the KnowledgeGraph datastore
					//KnowledgeGraph datastores contain tables and feature classes
					using (var kg = new KnowledgeGraph(kg_props))
					{
						string entityName = "Name-of-the-entity";
						var oidList = new List<long>() { 260294, 678, 3523, 3, 669, 93754 };
						var idList = kg.TransformToIDs(entityName, oidList);
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.TransformToObjectIDs(System.string, System.Collections.Generic.IEnumerable{System.Object})
			#region Transform a set of IDs to objectIDs for an entity
			{
				await QueuedTask.Run(() =>
				{
					var idList = new List<object>() { "{CA2EF786-A10E-4B40-9737-9BDDDEA127B0}",
																			 "{14B5AD65-890D-4062-90A7-C42C23B0066E}",
																			 "{A428D1F6-CB00-4559-AAFD-40885A4F2340}"};
					var kg_props =
									new KnowledgeGraphConnectionProperties(new Uri(url));
					//Connect to the KnowledgeGraph datastore
					//KnowledgeGraph datastores contain tables and feature classes
					using (var kg = new KnowledgeGraph(kg_props))
					{
						string entityName = "Name-of-the-entity";
						var oidList = kg.TransformToObjectIDs(entityName, idList);
					}
				});
			}
			#endregion

			#region ProSnippet Group: KnowledgeGraph Graph Data Model
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDataModel()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel
			#region Retrieving the Data Model 
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the KnowledgeGraph Data Model
						using (var kg_dm = kg.GetDataModel())
						{
							//TODO use KG data model...
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDataModel()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetTimestamp
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetSpatialReference()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetIsArcGISManaged()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetOIDPropertyName()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetIsStrict()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel
			#region Get Data Model Properties
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the KnowledgeGraph Data Model
						using (var kg_dm = kg.GetDataModel())
						{
							var kg_name = System.IO.Path.GetFileName(
											 System.IO.Path.GetDirectoryName(kg_props.URL.ToString()));

							System.Diagnostics.Debug.WriteLine(
								$"\r\n'{kg_name}' Datamodel:\r\n-----------------");
							var time_stamp = kg_dm.GetTimestamp();
							var sr = kg_dm.GetSpatialReference();
							System.Diagnostics.Debug.WriteLine($"Timestamp: {time_stamp}");
							System.Diagnostics.Debug.WriteLine($"Sref: {sr.Wkid}");
							System.Diagnostics.Debug.WriteLine(
								$"IsStrict: {kg_dm.GetIsStrict()}");
							System.Diagnostics.Debug.WriteLine(
								$"OIDPropertyName: {kg_dm.GetOIDPropertyName()}");
							System.Diagnostics.Debug.WriteLine(
								$"IsArcGISManaged: {kg_dm.GetIsArcGISManaged()}");
						}
					}
				});
			}
			#endregion



			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetIdentifierInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierInfo.GetIdentifierGeneration
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierGeneration
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNativeIdentifier
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphUniformIdentifier
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphUniformIdentifier.GetIdentifierName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierGeneration.GetMethodHint
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphUUIDMethodHint
			#region Get Data Model Identifier Info
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the KnowledgeGraph Data Model
						using (var kg_dm = kg.GetDataModel())
						{
							var kg_id_info = kg_dm.GetIdentifierInfo();
							var kg_id_gen = kg_id_info.GetIdentifierGeneration();
							if (kg_id_info is KnowledgeGraphNativeIdentifier kg_ni)
							{
								System.Diagnostics.Debug.WriteLine(
									$"IdentifierInfo: KnowledgeGraphNativeIdentifier");
							}
							else if (kg_id_info is KnowledgeGraphUniformIdentifier kg_ui)
							{
								System.Diagnostics.Debug.WriteLine(
									$"IdentifierInfo: KnowledgeGraphUniformIdentifier");
								System.Diagnostics.Debug.WriteLine(
									$"IdentifierName: '{kg_ui.GetIdentifierName()}'");
							}
							System.Diagnostics.Debug.WriteLine(
								$"Identifier MethodHint: {kg_id_gen.GetMethodHint()}");
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
			#region Get Data Model MetaEntityTypes/Provenance
			{
				//Provenance entity type is stored as a MetaEntityType 
				QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the KnowledgeGraph Data Model
						using (var kg_dm = kg.GetDataModel())
						{
							var dict_types = kg_dm.GetMetaEntityTypes();
							//If there is no provenance then MetaEntityTypes will be
							//an empty collection
							foreach (var kvp in dict_types)
							{
								var meta_entity_type = kvp.Value;
								if (meta_entity_type.GetRole() ==
										KnowledgeGraphNamedObjectTypeRole.Provenance)
								{
									//TODO - use the provenance entity type
									var name = meta_entity_type.GetName();

								}
							}

						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetEntityTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetAliasName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetObjectIDPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
			#region Get KnowledgeGraph Entity Types
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the KnowledgeGraph Data Model
						using (var kg_dm = kg.GetDataModel())
						{
							var dict_types = kg_dm.GetEntityTypes();

							foreach (var kvp in dict_types)
							{
								var entity_type = kvp.Value;
								var role = entity_type.GetRole();
								//note "name" will be the same name as the corresponding
								//feature class or table in the KG's relational gdb model
								var name = entity_type.GetName();
								var alias = entity_type.GetAliasName();
								var objectIDPropertyName = entity_type.GetObjectIDPropertyName();
								//etc

							}

						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.SupportsProvenance
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.ProvenanceTypeName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.ProvenancePropertyInfo
			#region Get the KG Provenance using KnowledgeGraphPropertyInfo
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						// use the KnowledgeGraphPropertyInfo
						var propInfo = kg.GetPropertyNameInfo();
						var supportsProvenance = propInfo.SupportsProvenance;
						var provenanceType = propInfo.ProvenanceTypeName;
						var provenanceInfo = propInfo.ProvenancePropertyInfo;
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
			#region Get Whether KG Supports Provenance
			string GetProvenanceEntityTypeName(KnowledgeGraphDataModel kg_dm)
			{
				var entity_types = kg_dm.GetMetaEntityTypes();
				foreach (var entity_type in entity_types)
				{
					if (entity_type.Value.GetRole() == KnowledgeGraphNamedObjectTypeRole.Provenance)
						return entity_type.Value.GetName();
				}
				return "";
			}
			bool KnowledgeGraphSupportsProvenance(KnowledgeGraph kg)
			{
				//if there is a provenance entity type then the KnowledgeGraph
				//supports provenance
				return !string.IsNullOrEmpty(
					GetProvenanceEntityTypeName(kg.GetDataModel()));

				// OR use the KnowledgeGraphPropertyInfo
				var propInfo = kg.GetPropertyNameInfo();
				return propInfo.SupportsProvenance;
			}
      #endregion

      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.SupportsDocuments
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.DocumentTypeName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.DocumentPropertyInfo
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.HasDocumentTypeName
      //cref: ArcGIS.Core.Data.Knowledge.DocumentPropertyInfo
      #region Get the KG Document info using KnowledgeGraphPropertyInfo
      {
        await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						// use the KnowledgeGraphPropertyInfo
						var propInfo = kg.GetPropertyNameInfo();
						var supportsDocs = propInfo.SupportsDocuments;
						var documentType = propInfo.DocumentTypeName;
            var hasDocumentType = propInfo.HasDocumentTypeName; // available at Pro 3.6 only
            var documentInfo = propInfo.DocumentPropertyInfo;
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
			#region Get Whether KG Has a Document Type 
			string GetDocumentEntityTypeName(KnowledgeGraphDataModel kg_dm)
			{
				var entity_types = kg_dm.GetEntityTypes();
				foreach (var entity_type in entity_types)
				{
					var role = entity_type.Value.GetRole();
					if (role == KnowledgeGraphNamedObjectTypeRole.Document)
						return entity_type.Value.GetName();
				}
				return "";//Unusual - probably Neo4j user-managed
			}

			bool KnowledgeGraphHasDocumentType(KnowledgeGraph kg)
			{
				//uncommon for there not to be a document type
				return !string.IsNullOrEmpty(
					GetDocumentEntityTypeName(kg.GetDataModel()));

				// OR use the KnowledgeGraphPropertyInfo
				var propInfo = kg.GetPropertyNameInfo();
				return propInfo.SupportsDocuments;
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityValue
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetTypeName
			#region Check Whether A KG Entity Is a Document 

			//Use GetDocumentEntityTypeName(KnowledgeGraphDataModel kg_dm) from
			//the 'Get Whether KG Has a Document Type' snippet to
			//get the documentNameType parameter
			bool GetEntityIsDocument(KnowledgeGraphEntityValue entity,
				string documentNameType = "")
			{
				if (string.IsNullOrEmpty(documentNameType))
					return false;
				return entity.GetTypeName() == documentNameType;
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityValue
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetProperties
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType
			#region Check Whether A Graph Type has a Spatial Property 

			//Use GetDocumentEntityTypeName(KnowledgeGraphDataModel kg_dm) from
			//the 'Get Whether KG Has a Document Type' snippet to
			//get the documentNameType parameter
			bool HasGeometry(KnowledgeGraphNamedObjectType kg_named_obj)
			{
				var props = kg_named_obj.GetProperties();
				return props.Any(prop => prop.FieldType == FieldType.Geometry);
			}

			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetRelationshipTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType.GetEndPoints
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEndPoint
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEndPoint.GetOriginEntityTypeName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEndPoint.GetDestinationEntityTypeName
			#region Get KnowledgeGraph Relationship Types
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the KnowledgeGraph Data Model
						using (var kg_dm = kg.GetDataModel())
						{
							var dict_types = kg_dm.GetRelationshipTypes();

							foreach (var kvp in dict_types)
							{
								var rel_type = kvp.Value;
								var role = rel_type.GetRole();
								//note "name" will be the same name as the corresponding
								//feature class or table in the KG's relational gdb model
								var name = rel_type.GetName();
								//etc.
								//Get relationship end points
								var end_points = rel_type.GetEndPoints();
								foreach (var end_point in end_points)
								{
									System.Diagnostics.Debug.WriteLine(
										$"Origin: '{end_point.GetOriginEntityTypeName()}', " +
										$"Destination: '{end_point.GetDestinationEntityTypeName()}'");
								}
							}

						}
					}
				});
			}
			#endregion


			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetEntityTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetRelationshipTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetProperties
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
			#region Get All KnowledgeGraph Graph Types
			{
				await QueuedTask.Run(() =>
				{
					//Create a connection properties
					var kg_props =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					using (var kg = new KnowledgeGraph(kg_props))
					{
						//Get the KnowledgeGraph Data Model
						using (var kg_datamodel = kg.GetDataModel())
						{
							var entities = kg_datamodel.GetEntityTypes();
							var relationships = kg_datamodel.GetRelationshipTypes();
							var provenance = kg_datamodel.GetMetaEntityTypes();

							var all_graph_types = new List<KnowledgeGraphNamedObjectType>();
							all_graph_types.AddRange(entities.Values);
							all_graph_types.AddRange(relationships.Values);
							all_graph_types.AddRange(provenance.Values);

							System.Diagnostics.Debug.WriteLine("\r\nGraph Types");

							int c = 0;
							foreach (var graph_type in all_graph_types)
							{
								var type_name = graph_type.GetName();
								var role = graph_type.GetRole().ToString();

								//equivalent to:
								//var fields = featClassDef.GetFields().Select(f => f.Name).ToList();
								//var field_names = string.Join(",", fields);
								var props = graph_type.GetProperties().Select(p => p.Name).ToList();
								var prop_names = string.Join(",", props);

								System.Diagnostics.Debug.WriteLine($"[{c++}]: " +
										$"{type_name}, {role}, {prop_names}");
							}
						}
					}
				});
			}
			#endregion

			#region ProSnippet Group: KnowledgeGraph Investigations
			#endregion


			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationFactory.CreateInvestigation(System.String, System.String)
			//cref: ArcGIS.Desktop.KnowledgeGraph.IKnowledgeGraphInvestigationFactory.CreateInvestigation(System.String, System.String)
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigation
			#region Create an Investigation
			{
				await QueuedTask.Run(() =>
				{
					var investigation = KnowledgeGraphInvestigationFactory.Instance.CreateInvestigation(url, "myInvestigation");
					//TODO - use the investigation

				});
			}
			#endregion



			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationFactory.CreateInvestigation(System.String, System.String)
			//cref: ArcGIS.Desktop.KnowledgeGraph.IKnowledgeGraphInvestigationFactory.CreateInvestigation(System.String, System.String)
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigation
			//cref: ArcGIS.Desktop.Core.KnowledgeGraphFrameworkExtender.CreateInvestigationPaneAsync
			#region Create and open an investigation pane
			{
				await QueuedTask.Run(() =>
				{
					var investigation = KnowledgeGraphInvestigationFactory.Instance.CreateInvestigation(url, "myInvestigation");

					ProApp.Panes.CreateInvestigationPaneAsync(investigation);
				});
			}
			#endregion


			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationProjectItem
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationProjectItem.GetInvestigation
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigation
			//cref: ArcGIS.Desktop.Core.KnowledgeGraphFrameworkExtender.CreateInvestigationPaneAsync
			#region Open an existing investigation
			{
				// open an existing investigation
				var investigationProjectItems = Project.Current.GetItems<KnowledgeGraphInvestigationProjectItem>();
				var investigationProjectItem = investigationProjectItems.FirstOrDefault(ipi => ipi.Name.Equals("myInvestigation"));
				await QueuedTask.Run(() =>
				{
					KnowledgeGraphInvestigation investigation = investigationProjectItem.GetInvestigation();
					ProApp.Panes.CreateInvestigationPaneAsync(investigation);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationProjectItem
			#region Get the Knowlege Graph Investigation container
			{
				var containerPath = "KnowledgeGraph";

				// find the first container with the correct path (key)
				var invContainer =
					 Project.Current.ProjectItemContainers.FirstOrDefault(c => c.Path == containerPath);

			}
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationProjectItem
			#region Select an investigation in the catalog pane
			{
				var investigationContainer = Project.Current.GetProjectItemContainer("KnowledgeGraph");
				var item = Project.Current.GetItems<KnowledgeGraphInvestigationProjectItem>().FirstOrDefault();

				//Select the fc
				if (item != null)
				{
					var projectWindow = Project.GetCatalogPane();
					await projectWindow.SelectItemAsync(item, true, true, investigationContainer);
				}
			}
			#endregion

			#region ProSnippet Group: KnowledgeGraphInvestigationView
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.Active
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.Investigation
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigation
			#region Get the active KnowledgeGraphInvestigationView, KnowledgeGraphInvestigation
			{
				// access the currently active knowledge graph investigation view
				KnowledgeGraphInvestigationView activeView = KnowledgeGraphInvestigationView.Active;
				KnowledgeGraphInvestigation investigation = activeView?.Investigation;
				if (investigation != null)
				{
					// perform some action
				}
			}
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationProjectItem
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationProjectItem.GetInvestigation
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigation
			//cref: ArcGIS.Desktop.KnowledgeGraph.IKnowledgeGraphInvestigationPane
			//cref: ArcGIS.Desktop.KnowledgeGraph.IKnowledgeGraphInvestigationPane.InvestigationView
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView
			#region Activate an existing investigation view
			{
				//Confirm if investigation exists as a project item
				KnowledgeGraphInvestigationProjectItem investigationItem =
					 Project.Current.GetItems<KnowledgeGraphInvestigationProjectItem>().FirstOrDefault(
																															item => item.Name.Equals("myInvestigation"));
				if (investigationItem != null)
				{
					KnowledgeGraphInvestigation investigation =
						 await QueuedTask.Run(() => investigationItem.GetInvestigation());

					// see if a view is already open that references the same investigation
					foreach (var investigationPane in ProApp.Panes.OfType<IKnowledgeGraphInvestigationPane>())
					{
						//if there is a match, activate the view
						if (investigationPane.InvestigationView.Investigation == investigation)
						{
							(investigationPane as Pane).Activate();
							return;
						}
					}
				}
			}
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.Active
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.SelectEntities
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.SelectRelationships
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.SelectNamedObjectTypes
			#region Select entity, relationship types in an investigation view
			{
				// get the active investigation view
				var iv = KnowledgeGraphInvestigationView.Active;

				// clear any TOC selection
				iv.ClearTOCSelection();

				// select entities
				List<string> entities = new List<string>();
				entities.Add("Person");
				entities.Add("Org");
				iv.SelectEntities(entities);

				// or select relationships
				List<string> relationships = new List<string>();
				relationships.Add("HasEmployee");
				iv.SelectRelationships(relationships);

				// or select a combination
				List<string> namedObjectTypes = new List<string>();
				namedObjectTypes.Add("Person");
				namedObjectTypes.Add("Org");
				namedObjectTypes.Add("HasEmployee");
				iv.SelectNamedObjectTypes(namedObjectTypes);
			}
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigation.URI
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.Active
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.SetSelectedRecords
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet
			#region Select records in an investigation view
			{
				// get the active investigation view 
				var iv = KnowledgeGraphInvestigationView.Active;
				var serviceUri = iv.Investigation.ServiceUri;

				// build a dictionary of records
				string some_entity = "Entity_Type_Name";
				var dict = new Dictionary<string, List<long>>();
				//Each entry consists of the type name and corresponding lists of ids
				dict.Add(some_entity, new List<long>() { 1, 5, 18, 36, 78 });

				//Create the id set...
				var idSet = KnowledgeGraphLayerIDSet.FromDictionary(new Uri(serviceUri), dict);

				// select the records on the investigation view
				iv.SetSelectedRecords(idSet, SelectionCombinationMethod.New);
			}
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.Active
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.Investigation
			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphInvestigationView.GetSelectedRecords
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			#region Get Selected records and open in a new link chart
			{
				// get the active investigation view
				var iv = KnowledgeGraphInvestigationView.Active;

				await QueuedTask.Run(() =>
				{
					// get the investigation
					var inv = iv.Investigation;

					// get the set of selected records
					var idSet = iv.GetSelectedRecords();

					// view these records in a link chart
					var map = MapFactory.Instance.CreateLinkChart("myLinkChart", new Uri(inv.ServiceUri), idSet);
					ProApp.Panes.CreateMapPaneAsync(map);
				});
			}
			#endregion

			#region ProSnippet Group: KnowledgeGraphLayer Creation with Maps
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams.#ctor(ArcGIS.Core.Data.Knowledge.KnowledgeGraph)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams.#ctor(System.Uri)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer
			#region Create a KG Layer containing all Entity and Relate types
			{
				await QueuedTask.Run(() =>
				{
					//With a connection to a KG established or source uri available...
					//Create a KnowledgeGraphLayerCreationParams
					var kg_params = new KnowledgeGraphLayerCreationParams(kg)
					{
						Name = "KG_With_All_Types",
						IsVisible = false
					};
					//Or
					var kg_params2 = new KnowledgeGraphLayerCreationParams(new Uri(url))
					{
						Name = "KG_With_All_Types",
						IsVisible = false
					};
					//Call layer factory with map or group layer container. 
					//A KG layer containing a feature layer and/or standalone table per
					//entity and relate type (except provenance) is created
					var kg_layer = LayerFactory.Instance.CreateLayer<KnowledgeGraphLayer>(
							kg_params, map);

				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams.IDSet
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromDictionary
			//cref: ArcGIS.Desktop.Mapping.ILayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
			#region Create a KG Layer containing a subset of Entity and Relate types
			{
				await QueuedTask.Run(() =>
				{
					//To create a KG layer (on a map or link chart) with a subset of
					//entities and relates, you must create an "id set". The workflow
					//is very similar to how you would create a SelectionSet.

					//First, create a dictionary containing the names of the types to be
					//added plus a corresponding list of ids (just like with a selection set).
					//Note: null or an empty list means add all records for "that" type..
					var kg_datamodel = kg.GetDataModel();
					//Arbitrarily get the name of the first entity and relate type
					var first_entity = kg_datamodel.GetEntityTypes().Keys.First();
					var first_relate = kg_datamodel.GetRelationshipTypes().Keys.First();

					//Make entries in the dictionary for each
					var dict = new Dictionary<string, List<long>>();
					dict.Add(first_entity, new List<long>());//Empty list means all records
					dict.Add(first_relate, null);//null list means all records
																			 //or specific records - however the ids are obtained
																			 //dict.Add(entity_or_relate_name, new List<long>() { 1, 5, 18, 36, 78});

					//Create the id set...
					var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict);

					//Create the layer params and assign the id set
					var kg_params = new KnowledgeGraphLayerCreationParams(kg)
					{
						Name = "KG_With_ID_Set",
						IsVisible = false,
						IDSet = idSet
					};

					//Call layer factory with map or group layer container
					//A KG layer containing just the feature layer(s) and/or standalone table(s)
					//for the entity and/or relate types + associated records will be created
					var kg_layer = LayerFactory.Instance.CreateLayer<KnowledgeGraphLayer>(
							kg_params, map);

				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams
			//cref: ArcGIS.Desktop.Mapping.LayerFactory.CanCreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
			#region Using LayerFactory.Instance.CanCreateLayer with KG Create Layer Params
			{
				await QueuedTask.Run(() =>
				{
					//Feature class and/or standalone tables representing KG entity and
					//relate types can only be added to a map (or link chart) as a child
					//of a KnowledgeGraph layer....

					//For example:
					var fc = kg.OpenDataset<FeatureClass>("Some_Entity_Or_Relate_Name");
					try
					{
						//Attempting to create a feature layer containing the returned fc
						//is NOT ALLOWED - can only be a child of a KG layer
						var fl_params_w_kg = new FeatureLayerCreationParams(fc);
						//CanCreateLayer will return false
						if (!(LayerFactory.Instance.CanCreateLayer<FeatureLayer>(
							fl_params_w_kg, map)))
						{
							System.Diagnostics.Debug.WriteLine(
								$"Cannot create a feature layer for {fc.GetName()}");
							return;
						}
						//This will throw an exception
						LayerFactory.Instance.CreateLayer<FeatureLayer>(fl_params_w_kg, map);
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.ToString());
					}

					//Can only be added as a child of a parent KG
					var dict = new Dictionary<string, List<long>>();
					dict.Add(fc.GetName(), new List<long>());
					var kg_params = new KnowledgeGraphLayerCreationParams(kg)
					{
						Name = $"KG_With_Just_{fc.GetName()}",
						IsVisible = false,
						IDSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict)
					};
					var kg_layer = LayerFactory.Instance.CreateLayer<KnowledgeGraphLayer>(
						kg_params, map);

				});
			}
			#endregion

			#region  ProSnippet Group: KnowledgeGraph Layer
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.ToOIDDictionary
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromDictionary(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,System.Collections.Generic.Dictionary{System.String,System.Collections.Generic.List{System.Int64}})
			#region Get and Set a KG Layer IDSet
			{
				var kg_layer = MapView.Active.Map.GetLayersAsFlattenedList()
												.OfType<KnowledgeGraphLayer>().FirstOrDefault();
				if (kg_layer == null)
					return;

				await QueuedTask.Run(() =>
				{
					//Get the existing kg layer id set and convert to a dictionary
					var layer_id_set = kg_layer.GetIDSet();
					var dict = layer_id_set.ToOIDDictionary();//Empty list means all records

					//Create an id set from a dictionary
					var dict2 = new Dictionary<string, List<long>>();
					dict2.Add("Enity_Or_Relate_Type_Name1", null);//Null means all records
					dict2.Add("Enity_Or_Relate_Type_Name2", new List<long>());//Empty list means all records
					dict2.Add("Enity_Or_Relate_Type_Name3",
						new List<long>() { 3, 5, 9, 101, 34 });//Explicit list of ids
																									 //dict2.Add("Enity_Or_Relate_Type_Name4",
																									 // new List<long>() { etc, etc });

					//Create the id set
					var idset = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict2);

					//Can be used to create a layer, link chart, append to link chart, etc...
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphExtensions.GetIsKnowledgeGraphDataset
			#region Is a dataset part of a Knowledge Graph
			{
				var featureLyer = MapView.Active.Map.GetLayersAsFlattenedList()
												.OfType<FeatureLayer>().FirstOrDefault();
				if (featureLyer == null)
					return;

				await QueuedTask.Run(() =>
				{
					//Get the feature class
					var fc = featureLyer.GetFeatureClass();

					// is it part of a KnowledgeGraph?
					var isPartOfKG = fc.GetIsKnowledgeGraphDataset();

				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetDatastore
			#region Get KG Datastore
			{
				var kg_layer = MapView.Active.Map.GetLayersAsFlattenedList()
												.OfType<KnowledgeGraphLayer>().FirstOrDefault();
				if (kg_layer == null)
					return;

				await QueuedTask.Run(() =>
				{
					// get the datastore
					var kg = kg_layer.GetDatastore();

					// now submit a search or a query
					// kg.SubmitSearch
					// kg.SubmitQuery
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetServiceUri
			#region Get KG Service uri
			{
				kgLayer.GetServiceUri();

			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer
			//cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer
			//cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer.IsEntity
			//cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer.IsRelationship
			#region SubLayers of a KnowledgeGraph Layer
			{
				var kg_layer = map.GetLayersAsFlattenedList().OfType<KnowledgeGraphLayer>().FirstOrDefault();
				if (kg_layer == null)
					return;

				if (map.MapType == MapType.LinkChart)
				{
					// if map is of MapType.LinkChart then the first level
					// children of the kg_layer are of type LinkChartFeatureLayer
					var childLayers = kg_layer.Layers;
					foreach (var childLayer in childLayers)
					{
						if (childLayer is LinkChartFeatureLayer lcFeatureLayer)
						{
							var isEntity = lcFeatureLayer.IsEntity;
							var isRel = lcFeatureLayer.IsRelationship;

							// TODO - continue processing
						}
					}
				}
				else if (map.MapType == MapType.Map)
				{
					// if map is of MapType.Map then the children of the
					// kg_layer are the standard Featurelayer and StandAloneTable
					var chidlren = kg_layer.GetMapMembersAsFlattenedList();
					foreach (var child in chidlren)
					{
						if (child is FeatureLayer fl)
						{
							// TODO - process the feature layer
						}
						else if (child is StandaloneTable st)
						{
							// TODO - process the standalone table
						}
					}
				}
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetIDSet
			//cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer.GetTypeName
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart
			#region Create a LinkChart from a subset of an existing LinkChart IDSet
			{
				//var map = ...
				if (map.MapType != MapType.LinkChart)
					return;

				// find the KG layer
				//var kgLayer = map.GetLayersAsFlattenedList()
				//        .OfType<KnowledgeGraphLayer>().FirstOrDefault();

				// find the first LinkChartFeatureLayer in the KG layer
				var lcFeatureLayer = kgLayer.GetLayersAsFlattenedList().OfType<LinkChartFeatureLayer>().FirstOrDefault();
				if (lcFeatureLayer != null)
					return;

				await QueuedTask.Run(() =>
				{
					// get the ID set of the KG layer
					var idSet = kgLayer.GetIDSet();

					// get the named object type in the KG that the LinkChartFeatureLayer represents
					var typeName = lcFeatureLayer.GetTypeName();
					// if there are items in the ID Set for this type
					if (idSet.Contains(typeName))
					{
						// build a new ID set containing only these records
						var dict = idSet.ToOIDDictionary();
						var oids = dict[typeName];

						var newDict = new Dictionary<string, List<long>>();
						newDict.Add(typeName, oids);

						var newIDSet = KnowledgeGraphLayerIDSet.FromDictionary(kgLayer.GetDatastore(), newDict);

						// now create a new link chart using just this subset of records
						MapFactory.Instance.CreateLinkChart("subset LinkChart", kgLayer.GetDatastore(), newIDSet);
					}
				});
			}
			#endregion

			#region ProSnippet Group: Graph Query and Text Search
			#endregion


			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitQuery
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.#Ctor
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.QueryText
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.ProvenanceBehavior
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphProvenanceBehavior
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.MoveNext
			//cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync()
			//cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.Current
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.GetCount
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.Item(System.Int32)
			#region Submit a Graph Query
			{
				await QueuedTask.Run(async () =>
				{
					//and assuming you have established a connection to a knowledge graph
					//...
					//Construct an openCypher query - return the first 10 entities (whatever
					//they are...)
					var query = "MATCH (n) RETURN n LIMIT 10";//default limit is 100 if not specified
																										//other examples...
																										//query = "MATCH (a:Person) RETURN [a.name, a.age] ORDER BY a.age DESC LIMIT 50";
																										//query = "MATCH (b:Person) RETURN { Xperson: { Xname: b.name, Xage: b.age } } ORDER BY b.name DESC";
																										//query = "MATCH p = (c:Person)-[:HasCar]-() RETURN p ORDER BY c.name DESC";

					//Create a query filter
					//Note: OutputSpatialReference is currently ignored
					var kg_qf = new KnowledgeGraphQueryFilter()
					{
						QueryText = query
					};
					//Optionally - u can choose to include provenance in the results
					//(_if_ the KG has provenance - otherwise the query will fail)
					bool includeProvenanceIfPresent = KnowledgeGraphSupportsProvenance(kg);

					if (includeProvenanceIfPresent)
					{
						//Only include if the KG has provenance
						kg_qf.ProvenanceBehavior =
								KnowledgeGraphProvenanceBehavior.Include;//default is exclude
					}
					//submit the query - returns a KnowledgeGraphCursor
					using (var kg_rc = kg.SubmitQuery(kg_qf))
					{
						//wait for rows to be returned from the server
						//note the "await"...
						while (await kg_rc.WaitForRowsAsync())
						{
							//Rows have been retrieved - process this "batch"...
							while (kg_rc.MoveNext())
							{
								//Get the current KnowledgeGraphRow
								using (var graph_row = kg_rc.Current)
								{
									//Graph row is an array, process all returned values...
									var val_count = (int)graph_row.GetCount();
									for (int i = 0; i < val_count; i++)
									{
										var retval = graph_row[i];
										//Process row value (note: recursive)
										//See "Process a KnowledgeGraphRow Value" snippet
										ProcessKnowledgeGraphRowValue(retval);
									}
								}
							}
						}//WaitForRowsAsync
					}//SubmitQuery
				});
			}
			#endregion


			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitSearch
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.GetCount
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.Item(System.Int32)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.#ctor
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.SearchTarget
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.SearchText
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.ReturnSearchContext
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.MaxRowCount
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedTypeCategory
			#region Submit a Text Search
			{
				await QueuedTask.Run(async () =>
				{
					//and assuming you have established a connection to a knowledge graph
					//...
					//Construct a KG search filter. Search text uses Apache Lucene Query Parser
					//syntax - https://lucene.apache.org/core/2_9_4/queryparsersyntax.html
					var kg_sf = new KnowledgeGraphSearchFilter()
					{
						SearchTarget = KnowledgeGraphNamedTypeCategory.Entity,
						SearchText = "Acme Electric Co.",
						ReturnSearchContext = true,
						MaxRowCount = 10 //Default is 100 if not specified
					};

					//submit the search - returns a KnowledgeGraphCursor
					var e = 0;
					using (var kg_rc = kg.SubmitSearch(kg_sf))
					{
						//wait for rows to be returned from the server
						//note the "await"...
						while (await kg_rc.WaitForRowsAsync())
						{
							//Rows have been retrieved - process this "batch"...
							while (kg_rc.MoveNext())
							{
								//Get the current KnowledgeGraphRow
								using (var graph_row = kg_rc.Current)
								{
									//We are returning entities from this search
									var entity = graph_row[0] as KnowledgeGraphEntityValue;
									var entity_type = entity.GetTypeName();
									var record = new List<string>();
									//discover keys(aka "fields") dynamically via GetKeys
									foreach (var prop_name in entity.GetKeys())
									{
										var obj_val = entity[prop_name] ?? "null";
										record.Add(obj_val.ToString());
									}
									System.Diagnostics.Debug.WriteLine(
										$"{entity_type}[{e++}] " + string.Join(", ", record));
									//or use "Process a KnowledgeGraphRow Value" snippet
									//ProcessKnowledgeGraphRowValue(entity);
								}
							}
						}//WaitForRowsAsync
					}//SubmitSearch
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitQuery(ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter)
			#region Convert an Open Cypher Query Result to a Selection
			{
				await QueuedTask.Run(async () =>
				{
					//Given an open-cypher qry against an entity or relationship type
					var qry = @"MATCH (p:PhoneNumber) RETURN p LIMIT 10";

					//create a KG query filter
					var kg_qry_filter = new KnowledgeGraphQueryFilter()
					{
						QueryText = qry
					};

					//save a list of the ids
					var oids = new List<long>();
					using (var kgRowCursor = kg.SubmitQuery(kg_qry_filter))
					{
						//wait for rows to be returned asynchronously from the server
						while (await kgRowCursor.WaitForRowsAsync())
						{
							//get the rows using "standard" move next
							while (kgRowCursor.MoveNext())
							{
								//current row is accessible via ".Current" prop of the cursor
								using (var graphRow = kgRowCursor.Current)
								{
									var cell_phone = graphRow[0] as KnowledgeGraphEntityValue;
									//note: some user-managed graphs do not have objectids
									oids.Add(cell_phone.GetObjectID());
								}
							}
						}
					}
					//create a query filter using the oids
					if (oids.Count > 0)
					{
						//select them on the layer
						var qf = new QueryFilter()
						{
							ObjectIDs = oids //apply the oids to the ObjectIds property
						};
						//select the child feature layer or standalone table representing
						//the given entity or relate type whose records are to be selected
						var phone_number_fl = kgLayer.GetLayersAsFlattenedList()
								.OfType<FeatureLayer>().First(l => l.Name == "PhoneNumber");

						//perform the selection
						phone_number_fl.Select(qf);
					}
				});
			}
			#endregion


			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.BindParameters
			#region Use Bind Parameters with an Open Cypher Query
			{
				await QueuedTask.Run(async () =>
				{
					//assume we have, in this case, a list of ids (perhaps retrieved
					//via a selection, hardcoded (like here), etc.
					var oids = new List<long>() { 3,4,7,8,9,11,12,14,15,19,21,25,29,
				 31,32,36,37,51,53,54,55,56,59,63,75,78,80,84,86,88,96,97,98,101,
				 103,106};
					//In the query, we refer to the "bind parameter" with the
					//"$" and a variable name - '$object_ids' in this example
					var qry = @"MATCH (p:PhoneNumber) " +
										@" WHERE p.objectid IN $object_ids " +
										@"RETURN p";

					//we provide the values to be substituted for the variable via the
					//KnowledgeGraphQueryFilter BindParameter property...
					var kg_qry_filter = new KnowledgeGraphQueryFilter()
					{
						QueryText = qry
					};
					//the bind parameter added to the query filter must refer to
					//the variable name used in the query string (without the "$")
					//Note:
					//Collections must be converted to a KnowledgeGraphArrayValue before
					//being assigned to a BindParameter
					var kg_oid_array = new KnowledgeGraphArrayValue();
					kg_oid_array.AddRange(oids);
					oids.Clear();

					kg_qry_filter.BindParameters["object_ids"] = kg_oid_array;

					//submit the query
					using (var kgRowCursor = kg.SubmitQuery(kg_qry_filter))
					{
						//wait for rows to be returned asynchronously from the server
						while (await kgRowCursor.WaitForRowsAsync())
						{
							//get the rows using "standard" move next
							while (kgRowCursor.MoveNext())
							{
								//current row is accessible via ".Current" prop of the cursor
								using (var graphRow = kgRowCursor.Current)
								{
									var cell_phone = graphRow[0] as KnowledgeGraphEntityValue;
									var oid = cell_phone.GetObjectID();

									var name = (string)cell_phone["FULL_NAME"];
									var ph_number = (string)cell_phone["PHONE_NUMBER"];
									System.Diagnostics.Debug.WriteLine(
										$"[{oid}] {name}, {ph_number}");
								}
							}
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.BindParameters
			#region Use Bind Parameters with an Open Cypher Query2
			{
				await QueuedTask.Run(async () =>
				{
					//assume we have, in this case, a list of ids (perhaps retrieved
					//via a selection, hardcoded (like here), etc.
					var oids = new List<long>() { 3,4,7,8,9,11,12,14,15,19,21,25,29,
				 31,32,36,37,51,53,54,55,56,59,63,75,78,80,84,86,88,96,97,98,101,
				 103,106};
					//In the query, we refer to the "bind parameter" with the
					//"$" and a variable name - '$object_ids' and '$sel_geom'
					//in this example
					var qry = @"MATCH (p:PhoneNumber) " +
											@"WHERE p.objectid IN $object_ids AND " +
											@"esri.graph.ST_Intersects($sel_geom, p.shape) " +
											@"RETURN p";
					//create a KG query filter
					var kg_qry_filter = new KnowledgeGraphQueryFilter()
					{
						QueryText = qry
					};

					//the bind parameter added to the query filter must refer to
					//the variable name used in the query string (without the "$")
					//Note:
					//Collections must be converted to a KnowledgeGraphArrayValue before
					//being assigned to a BindParameter
					var kg_oid_array = new KnowledgeGraphArrayValue();
					kg_oid_array.AddRange(oids);
					kg_qry_filter.BindParameters["object_ids"] = kg_oid_array;

					//Create a polygon based on current map extent
					var extent = MapView.Active.Extent;
					var poly = PolygonBuilder.CreatePolygon(extent.Expand(0.25, 0.25, true));
					kg_qry_filter.BindParameters["sel_geom"] = poly;
					oids.Clear();

					//submit the query
					using (var kgRowCursor = kg.SubmitQuery(kg_qry_filter))
					{
						//wait for rows to be returned asynchronously from the server
						while (await kgRowCursor.WaitForRowsAsync())
						{
							//get the rows using "standard" move next
							while (kgRowCursor.MoveNext())
							{
								//current row is accessible via ".Current" prop of the cursor
								using (var graphRow = kgRowCursor.Current)
								{
									#region Process Row

									var cell_phone = graphRow[0] as KnowledgeGraphEntityValue;
									var oid = cell_phone.GetObjectID();

									var name = (string)cell_phone["FULL_NAME"];
									var ph_number = (string)cell_phone["PHONE_NUMBER"];
									System.Diagnostics.Debug.WriteLine(
										$"[{oid}] {name}, {ph_number}");

									#endregion
								}
							}
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitQuery
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitSearch
			//cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
			#region Call WaitForRowsAsync With Cancellation
			{
				//On the QueuedTask...
				//and assuming you have established a connection to a knowledge graph
				//...
				//submit query or search to return a KnowledgeGraphCursor
				//using (var kgRowCursor = kg.SubmitQuery(kg_qf)) {
				//using (var kgRowCursor = kg.SubmitSearch(kg_sf)) {
				//...
				//wait for rows to be returned from the server
				//"auto-cancel" after 20 seconds
				var cancel = new CancellationTokenSource(new TimeSpan(0, 0, 20));
				//catch TaskCanceledException
				try
				{
					while (await kgRowCursor.WaitForRowsAsync(cancel.Token))
					{
						//check for row events
						while (kgRowCursor.MoveNext())
						{
							using (var graph_row = kgRowCursor.Current)
							{
								//Graph row is an array, process all returned values...
								var val_count = (int)graph_row.GetCount();
								for (int i = 0; i < val_count; i++)
								{
									var retval = graph_row[i];
									//Process row value (note: recursive)
									//See "Process a KnowledgeGraphRow Value" snippet
									ProcessKnowledgeGraphRowValue(retval);
								}
							}
						}
					}
				}
				//Timeout expired
				catch (TaskCanceledException tce)
				{
					//Handle cancellation as needed
				}
				cancel.Dispose();
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphValue
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphValueType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPrimitiveValue.GetValue
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphArrayValue.GetSize
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphArrayValue.Item(System.UInt64)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetEntityCount
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetEntity
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetRelationshipCount
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetRelationship
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphObjectValue.GetKeys
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphObjectValue.Item(System.String)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetID
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetObjectID
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetTypeName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityValue.GetLabel
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetHasRelatedEntityIDs
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetOriginID
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetDestinationID
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.GetCount
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.Item(System.Int32)
			#region Process a KnowledgeGraphRow Value

			void ProcessGraphNamedObjectValue(
				KnowledgeGraphNamedObjectValue kg_named_obj_val)
			{
				if (kg_named_obj_val is KnowledgeGraphEntityValue kg_entity)
				{
					var label = kg_entity.GetLabel();
					//TODO - use label
				}
				else if (kg_named_obj_val is KnowledgeGraphRelationshipValue kg_rel)
				{
					var has_entity_ids = kg_rel.GetHasRelatedEntityIDs();
					if (kg_rel.GetHasRelatedEntityIDs())
					{
						var origin_id = kg_rel.GetOriginID();
						var dest_id = kg_rel.GetDestinationID();
						//TODO - use ids
					}
				}
				var id = kg_named_obj_val.GetID();
				var oid = kg_named_obj_val.GetObjectID();
				//Note: Typename corresponds to the name of the feature class or table
				//in the relational gdb model -and- to the name of the KnowledgeGraphNamedObjectType
				//in the knowledge graph data model
				var type_name = kg_named_obj_val.GetTypeName();
				//TODO use id, object id, etc.
			}

			//Object values include entities, relationships, and anonymous objects
			void ProcessGraphObjectValue(KnowledgeGraphObjectValue kg_obj_val)
			{
				switch (kg_obj_val)
				{
					case KnowledgeGraphEntityValue kg_entity:
						ProcessGraphNamedObjectValue(kg_entity);
						break;
					case KnowledgeGraphRelationshipValue kg_rel:
						ProcessGraphNamedObjectValue(kg_rel);
						break;
					default:
						//Anonymous objects
						break;
				}
				//graph object values have a set of properties (equivalent
				//to a collection of key/value pairs)
				var keys = kg_obj_val.GetKeys();
				foreach (var key in keys)
					ProcessKnowledgeGraphRowValue(kg_obj_val[key]);//Recurse
			}

			//Process a KnowledgeGraphValue from a query or search
			void ProcessGraphValue(KnowledgeGraphValue kg_val)
			{
				switch (kg_val)
				{
					case KnowledgeGraphPrimitiveValue kg_prim:
						//KnowledgeGraphPrimitiveValue not currently used in
						//query and search 
						ProcessKnowledgeGraphRowValue(kg_prim.GetValue());//Recurse
						return;
					case KnowledgeGraphArrayValue kg_array:
						var count = kg_array.GetSize();
						//Recursively process each value in the array
						for (ulong i = 0; i < count; i++)
							ProcessKnowledgeGraphRowValue(kg_array[i]);//Recurse
						return;
					case KnowledgeGraphPathValue kg_path:
						//Entities
						var entity_count = kg_path.GetEntityCount();
						//Recursively process each entity value in the path
						for (ulong i = 0; i < entity_count; i++)
							ProcessGraphObjectValue(kg_path.GetEntity(i));//Recurse

						//Recursively process each relationship value in the path
						var relate_count = kg_path.GetRelationshipCount();
						for (ulong i = 0; i < relate_count; i++)
							ProcessGraphObjectValue(kg_path.GetRelationship(i));//Recurse
						return;
					case KnowledgeGraphObjectValue kg_object:
						ProcessGraphObjectValue(kg_object);//Recurse
						return;
					default:
						var type_string = kg_val.GetType().ToString();
						System.Diagnostics.Debug.WriteLine(
							$"Unknown: '{type_string}'");
						return;
				}
			}

			//Process each value from the KnowledgeGraphRow array
			void ProcessKnowledgeGraphRowValue(object value)
			{
				switch (value)
				{
					//Graph value?
					case KnowledgeGraphValue kg_val:
						var kg_type = kg_val.KnowledgeGraphValueType.ToString();
						System.Diagnostics.Debug.WriteLine(
							$"KnowledgeGraphValue: '{kg_type}'");
						ProcessGraphValue(kg_val);//Recurse
						return;
					//Primitive types...add additional logic as needed
					case System.DBNull dbn:
						System.Diagnostics.Debug.WriteLine("DBNull.Value");
						return;
					case string str:
						System.Diagnostics.Debug.WriteLine($"'{str}' (string)");
						return;
					case long l_val:
						System.Diagnostics.Debug.WriteLine($"{l_val} (long)");
						return;
					case int i_val:
						System.Diagnostics.Debug.WriteLine($"{i_val} (int)");
						return;
					case short s_val:
						System.Diagnostics.Debug.WriteLine($"{s_val} (short)");
						return;
					case double d_val:
						System.Diagnostics.Debug.WriteLine($"{d_val} (double)");
						return;
					case float f_val:
						System.Diagnostics.Debug.WriteLine($"{f_val} (float)");
						return;
					case DateTime dt_val:
						System.Diagnostics.Debug.WriteLine($"{dt_val} (DateTime)");
						return;
					case DateOnly dt_only_val:
						System.Diagnostics.Debug.WriteLine($"{dt_only_val} (DateOnly)");
						return;
					case TimeOnly tm_only_val:
						System.Diagnostics.Debug.WriteLine($"{tm_only_val} (TimeOnly)");
						return;
					case DateTimeOffset dt_tm_offset_val:
						System.Diagnostics.Debug.WriteLine(
							$"{dt_tm_offset_val} (DateTimeOffset)");
						return;
					case System.Guid guid_val:
						var guid_string = guid_val.ToString("B");
						System.Diagnostics.Debug.WriteLine($"'{guid_string}' (Guid)");
						return;
					case Geometry geom_val:
						var geom_type = geom_val.GeometryType.ToString();
						var is_empty = geom_val.IsEmpty;
						var wkid = geom_val.SpatialReference?.Wkid ?? 0;
						System.Diagnostics.Debug.WriteLine(
							$"geometry: {geom_type}, empty: {is_empty}, sr_wkid {wkid} (shape)");
						return;
					default:
						//Blob? Others?
						var type_str = value.GetType().ToString();
						System.Diagnostics.Debug.WriteLine($"Primitive: {type_str}");
						return;
				}
			}

			// ...submit query or search
			//using (var kgRowCursor = kg.SubmitQuery(kg_qf)) {
			//using (var kgRowCursor = kg.SubmitSearch(kg_sf)) {
			//  ...wait for rows ...
			//  while (await kgRowCursor.WaitForRowsAsync()) {
			//   ...rows have been retrieved
			//   while (kgRowCursor.MoveNext()) {
			//     ...get the current KnowledgeGraphRow
			//     using (var graph_row = kgRowCursor.Current) {
			//        var val_count = (int)graph_row.GetCount();
			//        for (int i = 0; i<val_count; i++)
			//           ProcessKnowledgeGraphRowValue(graph_row[i]);

			#endregion

			#region ProSnippet Group: Link Charts
			#endregion

			#region Find link chart project items
			{
				// find all the link chart project items
				var linkChartItems = Project.Current.GetItems<MapProjectItem>()
																		.Where(pi => pi.MapType == MapType.LinkChart);

				// find a link chart project item by name
				var linkChartItem =
					 Project.Current.GetItems<MapProjectItem>()
							.FirstOrDefault(pi => pi.Name == "Acme Link Chart");
			}
			#endregion

			#region Find link chart map by name
			{
				var projectItem = Project.Current.GetItems<MapProjectItem>()
										 .FirstOrDefault(pi => pi.Name == "Acme Link Chart");
				var linkChartMap = projectItem?.GetMap();
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.MapView.IsLinkChartView
			//cref: ArcGIS.Desktop.Mapping.Map.IsLinkChart
			#region Does Active MapView contain a link chart map
			{
				var mv = MapView.Active;
				// check the view
				var isLinkChartView = mv.IsLinkChartView;

				// or alternatively get the map and check that
				//var map = MapView.Active.Map;
				// check the MapType to determine if it's a link chart map
				var isLinkChart = map.MapType == MapType.LinkChart;
				// or you could use the following
				// var isLinkChart = map.IsLinkChart;
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.MapView.IsLinkChartView
			#region Find Link Chart from Map panes
			{
				var mapPanes = FrameworkApplication.Panes.OfType<IMapPane>().ToList();
				var mapPane = mapPanes.FirstOrDefault(
						mp => mp.MapView.IsLinkChartView && mp.MapView.Map.Name == "Acme Link Chart");
				var lcMap = mapPane.MapView.Map;
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.GetLinkChartLayout 
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SetLinkChartLayoutAsync 
			//cref: ArcGIS.Core.CIM.KnowledgeLinkChartLayoutAlgorithm
			//cref: ArcGIS.Core.CIM.MapType
			//cref: ArcGIS.Desktop.Mapping.Map.IsLinkChart
			#region Get and set the link chart layout
			{
				//var map = MapView.Active.Map;
				// a MapView can encapsulate a link chart IF it's map
				// is of type MapType.LinkChart
				var isLinkChart = map.MapType == MapType.LinkChart;
				// or use the following
				// var isLinkChart = map.IsLinkChart;

				var mv = MapView.Active;

				await QueuedTask.Run(() =>
				{
					if (isLinkChart)
					{
						// get the layout algorithm
						var layoutAlgorithm = mv.GetLinkChartLayout();

						// toggle the value
						if (layoutAlgorithm == KnowledgeLinkChartLayoutAlgorithm.Geographic_Organic_Standard)
							layoutAlgorithm = KnowledgeLinkChartLayoutAlgorithm.Organic_Standard;
						else
							layoutAlgorithm = KnowledgeLinkChartLayoutAlgorithm.Geographic_Organic_Standard;

						// set it
						mv.SetLinkChartLayoutAsync(layoutAlgorithm);

						// OR set it and force a redraw / update
						// await mv.SetLinkChartLayoutAsync(layoutAlgorithm, true);
					}
				});
			}
			#endregion

			#region ProSnippet Group: Create and Append to Link Charts
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromKnowledgeGraph
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphFilterType.AllNamedObjects
			#region Create a Link Chart Containing All Records for a KG
			{
				await QueuedTask.Run(() =>
				{
					//Create the link chart and show it
					//build the IDSet using KnowledgeGraphFilterType.AllNamedObjects
					var idSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraph(
						kg, KnowledgeGraphFilterType.AllNamedObjects);
					var linkChart = MapFactory.Instance.CreateLinkChart(
														"KG Link Chart", kg, idSet);
					FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			#region Create a Link Chart With an Empty KG Layer
			{
				await QueuedTask.Run(() =>
				{
					//Create the link chart with a -null- id set
					//This will create a KG Layer with empty sub-layers
					//(Note: you cannot create an empty KG layer on a map - only on a link chart)
					var linkChart = MapFactory.Instance.CreateLinkChart(
														"KG Link Chart", kg, null);
					FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromKnowledgeGraph
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphFilterType.AllEntities
			#region Create a link chart with all the entities of the Knowledge Graph
			{
				await QueuedTask.Run(() =>
				{
					using (var kg_for_lc = new KnowledgeGraph(
						new KnowledgeGraphConnectionProperties(new Uri(url))))
					{
						var idSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraph(
							kg_for_lc, KnowledgeGraphFilterType.AllEntities);
						var newLinkChart = MapFactory.Instance.CreateLinkChart(
							"All_Entities link chart", kg_for_lc, idSet);
					};
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromDictionary
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			#region Create a Link Chart from a query
			{
				//use the results of a query to create an idset. Create the link chart
				//containing just records corresponding to the query results
				var qry = @"MATCH (p1:PhoneNumber)-[r1:MADE_CALL|RECEIVED_CALL]->(c1:PhoneCall)<-" +
									@"[r2:MADE_CALL|RECEIVED_CALL]-(p2:PhoneNumber)-[r3:MADE_CALL|RECEIVED_CALL]" +
									@"->(c2:PhoneCall)<-[r4:MADE_CALL|RECEIVED_CALL]-(p3:PhoneNumber) " +
									@"WHERE p1.FULL_NAME = ""Robert Johnson"" AND " +
									@"p3.FULL_NAME= ""Dan Brown"" AND " +
									@"p1.globalid <> p2.globalid AND " +
									@"p2.globalid <> p3.globalid " +
									@"RETURN p1, r1, c1, r2, p2, r3, c2, r4, p3";

				var dict = new Dictionary<string, List<long>>();

				await QueuedTask.Run(async () =>
				{
					using (var kg_for_lc = kgLayer.GetDatastore())
					{
						var graphQuery = new KnowledgeGraphQueryFilter()
						{
							QueryText = qry
						};

						using (var kg_rc = kg_for_lc.SubmitQuery(graphQuery))
						{
							while (await kg_rc.WaitForRowsAsync())
							{
								while (kg_rc.MoveNext())
								{
									using (var graphRow = kg_rc.Current)
									{
										// process the row
										var cnt_val = (int)graphRow.GetCount();
										for (int v = 0; v < cnt_val; v++)
										{
											var obj_val = graphRow[v] as KnowledgeGraphNamedObjectValue;
											var type_name = obj_val.GetTypeName();
											var oid = (long)obj_val.GetObjectID();
											if (!dict.ContainsKey(type_name))
											{
												dict[type_name] = new List<long>();
											}
											if (!dict[type_name].Contains(oid))
												dict[type_name].Add(oid);
										}
									}
								}
							}
						}
						//make an ID Set to create the LinkChart
						var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg_for_lc, dict);

						//Create the link chart and show it
						var linkChart = MapFactory.Instance.CreateLinkChart(
															"KG With ID Set", kg_for_lc, idSet);
						FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
					}
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromKnowledgeGraph
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphFilterType.AllEntities
			#region Create a link chart based on a template link chart
			{
				// note that the template link chart MUST use the same KG server

				await QueuedTask.Run(() =>
				{
					// find the existing link chart by name
					var projectItem = Project.Current.GetItems<MapProjectItem>()
					.FirstOrDefault(pi => pi.Name == "Acme Link Chart");
					var linkChartMap = projectItem?.GetMap();
					if (linkChartMap == null)
						return;

					//Create a connection properties
					var kg_props2 =
							new KnowledgeGraphConnectionProperties(new Uri(url));
					try
					{
						//Open a connection
						using (var kg_for_lc = new KnowledgeGraph(kg_props2))
						{
							//Add all entities to the link chart
							var idSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraph(
										kg_for_lc, KnowledgeGraphFilterType.AllEntities);
							//Create the new link chart and show it
							var newLinkChart = MapFactory.Instance.CreateLinkChart(
																"KG from Template", kg_for_lc, idSet, linkChartMap.URI);
							FrameworkApplication.Panes.CreateMapPaneAsync(newLinkChart);
						}
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.ToString());
					}
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphLayerException
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
			#region Checking KnowledgeGraphLayerException
			{
				// running on QueuedTask

				var dict = new Dictionary<string, List<long>>();
				dict.Add("person", new List<long>());  //Empty list means all records
				dict.Add("made_call", null);  //null list means all records

				// or specific records - however the ids are obtained
				dict.Add("phone_call", new List<long>() { 1, 5, 18, 36, 78 });

				// make the id set
				var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict);

				try
				{
					//Create the link chart and show it
					var linkChart = MapFactory.Instance.CreateLinkChart(
														"KG With ID Set", kg, idSet);
					FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
				}
				catch (KnowledgeGraphLayerException e)
				{
					// get the invalid named types
					//   remember that the named types are case-sensitive
					var invalidNamedTypes = e.InvalidNamedTypes;

					// do something with the invalid named types 
					// for example - log or return to caller to show message to user
				}
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.CanAppendToLinkChart
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.AppendToLinkChart
			#region Append to Link Chart
			{
				//We create an id set to contain the records to be appended
				var dict = new Dictionary<string, List<long>>();
				dict["Suspects"] = new List<long>();

				//In this case, via results from a query...
				var qry2 = "MATCH (s:Suspects) RETURN s";

				await QueuedTask.Run(async () =>
				{
					using (var kg_for_lc = kgLayer.GetDatastore())
					{
						var graphQuery = new KnowledgeGraphQueryFilter()
						{
							QueryText = qry2
						};

						using (var kg_rc = kg_for_lc.SubmitQuery(graphQuery))
						{
							while (await kg_rc.WaitForRowsAsync())
							{
								while (kg_rc.MoveNext())
								{
									using (var graphRow = kg_rc.Current)
									{
										var obj_val = graphRow[0] as KnowledgeGraphNamedObjectValue;
										var oid = (long)obj_val.GetObjectID();
										dict["Suspects"].Add(oid);
									}
								}
							}
						}

						//make an ID Set to append to the LinkChart
						var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg_for_lc, dict);
						//Get the relevant link chart to which records will be
						//appended...in this case, from an open map pane in the
						//Pro application...
						var mapPanes = FrameworkApplication.Panes.OfType<IMapPane>().ToList();
						var mapPane = mapPanes.First(
							mp => mp.MapView.IsLinkChartView &&
							mp.MapView.Map.Name == "Acme Link Chart");
						var linkChartMap = mapPane.MapView.Map;

						//or get the link chart from an item in the catalog...etc.,etc.
						//var projectItem = Project.Current.GetItems<MapProjectItem>()
						//      .FirstOrDefault(pi => pi.Name == "Acme Link Chart");
						//var linkChartMap = projectItem?.GetMap();

						//Call AppendToLinkChart with the id set
						if (linkChartMap.CanAppendToLinkChart(idSet))
							linkChartMap.AppendToLinkChart(idSet);
					}
				});
			}
			#endregion

			#region ProSnippet Group: ID Sets
			#endregion


			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetIDSet
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.IsEmpty
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.NamedObjectTypeCount
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.Contains
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.ToOIDDictionary
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.ToUIDDictionary
			#region Get the ID Set of a KG layer
			{
				await QueuedTask.Run(() =>
				{
					var idSet = kgLayer.GetIDSet();

					// is the set empty?
					var isEmpty = idSet.IsEmpty;
					// get the count of named object types
					var countNamedObjects = idSet.NamedObjectTypeCount;
					// does it contain the entity "Species";
					var contains = idSet.Contains("Species");

					// get the idSet as a dictionary of namedObjectType and oids
					var oidDict = idSet.ToOIDDictionary();
					var speciesOIDs = oidDict["Species"];

					// alternatively get the idSet as a dictionary of 
					// namedObjectTypes and uuids
					var uuidDict = idSet.ToUIDDictionary();
					var speciesUuids = uuidDict["Species"];

				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromSelectionSet
			#region Create an ID set from a SelectionSet
			{
				await QueuedTask.Run(() =>
				{
					// get the selection set
					var sSet = map.GetSelection();

					// translate to an KnowledgeGraphLayerIDSet
					//  if the selectionset does not contain any KG entity or relationship records
					//    then idSet will be null  
					var idSet = KnowledgeGraphLayerIDSet.FromSelectionSet(sSet);
					if (idSet == null)
						return;

					// you can use the idSet to create a new linkChart
					//   (using MapFactory.Instance.CreateLinkChart)
				});
			}
			#endregion

			#region ProSnippet Group: Root Nodes
			#endregion


			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.GetShowRootNodes(ArcGIS.Desktop.Mapping.MapView)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SetShowRootNodes(ArcGIS.Desktop.Mapping.MapView, System.Boolean)
			#region Toggle Root Node Display
			{
				var val = MapView.Active.GetShowRootNodes();

				await QueuedTask.Run(() =>
				{
					MapView.Active.SetShowRootNodes(!val);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.GetRootNodes(ArcGIS.Desktop.Mapping.MapView)
			//cref: ArcGIS.Desktop.Mapping.MapMemberIDSet
			//cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary
			#region Get records that are set as Root Nodes
			{
				await QueuedTask.Run(() =>
				{
					MapMemberIDSet rootNodes = MapView.Active.GetRootNodes();
					var rootNodeDict = rootNodes.ToDictionary();

					// rootNodeDict is a Dictionary<MapMember, List<long>>

					// access a particular mapMember in the Dictionary
					if (rootNodeDict.ContainsKey(mapMember))
					{
						var oids = rootNodeDict[mapMember];
					}

					// OR iterate through the dictionary
					foreach (var (mm, oids) in rootNodeDict)
					{
						// do something
					}
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SetRootNodes(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.MapMemberIDSet)
			//cref: ArcGIS.Desktop.Mapping.MapMemberIDSet
			//cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.FromDictionary``1
			#region Assign a set of records as Root Nodes
			{
				await QueuedTask.Run(() =>
				{
					var dict = new Dictionary<MapMember, List<long>>();
					dict.Add(entityLayer, oids);
					MapMemberIDSet mmIDSet = MapMemberIDSet.FromDictionary(dict);

					MapView.Active.SetRootNodes(mmIDSet);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SetRootNodes(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.MapMemberIDSet)
			//cref: ArcGIS.Desktop.Mapping.MapMemberIDSet
			#region Assign a selection as Root Nodes 
			{
				await QueuedTask.Run(() =>
				{
					var mapSel = MapView.Active.Map.GetSelection();

					MapView.Active.SetRootNodes(mapSel);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SelectAllRootNodes(ArcGIS.Desktop.Mapping.MapView)
			#region Select the records that are Root Node 
			{
				await QueuedTask.Run(() =>
				{
					var mapSel = MapView.Active.SelectAllRootNodes();

					// this is the same as 
					MapMemberIDSet rootNodes = MapView.Active.GetRootNodes();
					SelectionSet selSet = SelectionSet.FromMapMemberIDSet(rootNodes);
					MapView.Active.Map.SetSelection(selSet);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SelectRootNodes(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.MapMemberIDSet)
			//cref: ArcGIS.Desktop.Mapping.MapMemberIDSet
			//cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.FromDictionary``1
			#region Define and select a set of records as Root Nodes
			{
				await QueuedTask.Run(() =>
				{
					var dict = new Dictionary<MapMember, List<long>>();
					dict.Add(entityLayer, oids);
					dict.Add(entityLayer2, oids2);
					MapMemberIDSet mmIDSet = MapMemberIDSet.FromDictionary(dict);

					MapView.Active.SelectRootNodes(mmIDSet);
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.ClearRootNodes(ArcGIS.Desktop.Mapping.MapView)
			#region Clear Root Nodes
			{
				await QueuedTask.Run(() =>
				{
					//Assuming mapview is a link chart view
					MapView.Active.ClearRootNodes();
				});
			}
			#endregion

			#region PrSnippet Group: Non Spatial Data
			#endregion

			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.GetShowNonSpatialData(ArcGIS.Desktop.Mapping.MapView)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SetShowNonSpatialData(ArcGIS.Desktop.Mapping.MapView, System.Boolean)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SelectNonSpatialData(ArcGIS.Desktop.Mapping.MapView)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SelectSpatialData(ArcGIS.Desktop.Mapping.MapView)
			#region Non spatial data
			{
				await QueuedTask.Run(() =>
				{
					// display non spatial data
					MapView.Active.SetShowNonSpatialData(true);

					// select the current set of non spatial data
					var selNonSpatial = MapView.Active.SelectNonSpatialData();

					// perform some action

					// select the current set of spatial data
					var selSpatial = MapView.Active.SelectSpatialData();

					// perform some other action
				});
			}
			#endregion

			#region ProSnippet Group: Editing
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table,System.Collections.Generic.Dictionary{System.String,System.Object})
			#region Create a new Entity
			{
				var mv = MapView.Active;
				await QueuedTask.Run(() =>
				{
					//Instantiate an operation for the Create
					var edit_op = new EditOperation()
					{
						Name = "Create a new organization",
						SelectNewFeatures = true
					};

					//Use datasets or feature layer(s) or standalone table(s)
					//Get a reference to the KnowledgeGraph
					//var kg = ... ; 

					//Open the feature class or Table to be edited
					var org_fc = kg.OpenDataset<FeatureClass>("Organization");

					//Alternatively, use the feature layer for 'Organization' if your context is a map
					//Get the parent KnowledgeGraphLayer
					var kg_layer = mv.Map.GetLayersAsFlattenedList()?
												.OfType<ArcGIS.Desktop.Mapping.KnowledgeGraphLayer>().First();
					//From the KG Layer get the relevant child feature layer
					var org_fl = kg_layer.GetLayersAsFlattenedList().OfType<FeatureLayer>()
													.First(child_layer => child_layer.Name == "Organization");

					//Define attributes
					var attribs = new Dictionary<string, object>();
					attribs["Name"] = "Acme Ltd.";
					attribs["Description"] = "Specializes in household items";
					attribs["SHAPE"] = mv.Extent.Center;//whatever is its location

					//Add it to the operation via the dataset...
					edit_op.Create(org_fc, attribs);
					//or use the feature layer/stand alone table if preferred and available
					//edit_op.Create(org_fl, attribs);

					if (edit_op.Execute())
					{
						//TODO: Operation succeeded
					}

				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo()
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.OriginIDPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.DestinationIDPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table,System.Collections.Generic.Dictionary{System.String,System.Object})
			#region Create a new Relationship from Existing Entities 1
			{
				var create_rel = await QueuedTask.Run(() =>
				{
					//Instantiate an operation for the Create
					var edit_op = new EditOperation()
					{
						Name = "Create a new relationship record",
						SelectNewFeatures = true
					};

					//Use datasets or feature layer(s) or standalone table(s)
					//Get a reference to the KnowledgeGraph
					//var kg = ... ; 

					//We will use a relate called 'HasEmployee' to relate an Organization w/ a Person
					//Use either tables or map members to get the rows to be related...
					var org_fc = kg.OpenDataset<FeatureClass>("Organization");
					var person_tbl = kg.OpenDataset<Table>("Person");

					//Get the relationship dataset
					//We can use either a table or standalone table
					var emp_tbl = kg.OpenDataset<Table>("HasEmployee");

					//we need the names of the origin and destination relationship properties
					var kg_prop_info = kg.GetPropertyNameInfo();

					//Arbitrarily use the first record from the two entity datasets "to be" related
					//Entities are always related by Global ID. Origin to Destination specifies the
					//direction (of the relate).
					//
					//Populate the attributes for the relationship
					var attribs = new Dictionary<string, object>();

					using (var rc = org_fc.Search())
					{
						if (rc.MoveNext())
							//Use the KnowledgeGraphPropertyInfo to avoid hardcoding...
							attribs[kg_prop_info.OriginIDPropertyName] = rc.Current.GetGlobalID();
					}
					using (var rc = person_tbl.Search())
					{
						if (rc.MoveNext())
							//Use the KnowledgeGraphPropertyInfo to avoid hardcoding...
							attribs[kg_prop_info.DestinationIDPropertyName] = rc.Current.GetGlobalID();
					}

					//Add any extra attribute information for the relation as needed
					attribs["StartDate"] = new DateTimeOffset(DateTime.Now);

					//Add a create for the relationship to the operation
					edit_op.Create(emp_tbl, attribs);

					//Do the create
					return edit_op.Execute();
				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription.#ctor(System.Guid,System.Guid,System.Collections.Generic.IReadOnlyDictionary{System.String,System.Object})
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			#region Create a new Relationship from Existing Entities 2
			{
				var create_rel2 = await QueuedTask.Run(() =>
				{
					//Instantiate an operation for the Create
					var edit_op = new EditOperation()
					{
						Name = "Create a new relationship record",
						SelectNewFeatures = true
					};

					//Use datasets or feature layer(s) or standalone table(s)
					//Get a reference to the KnowledgeGraph
					//var kg = ... ; 

					//We will use a relate called 'HasEmployee' to relate an Organization w/ a Person
					//Use either tables or map members to get the rows to be related...
					var org_fc = kg.OpenDataset<FeatureClass>("Organization");
					var person_tbl = kg.OpenDataset<Table>("Person");

					//Get the relationship dataset
					//We can use either a table or standalone table
					var emp_tbl = kg.OpenDataset<Table>("HasEmployee");

					// get the origin, destination records
					Guid guidOrigin = Guid.Empty;
					Guid guidDestination = Guid.Empty;
					using (var rc = org_fc.Search())
					{
						if (rc.MoveNext())
							//Use the KnowledgeGraphPropertyInfo to avoid hardcoding...
							guidOrigin = rc.Current.GetGlobalID();
					}
					using (var rc = person_tbl.Search())
					{
						if (rc.MoveNext())
							//Use the KnowledgeGraphPropertyInfo to avoid hardcoding...
							guidDestination = rc.Current.GetGlobalID();
					}

					//Add any extra attribute information for the relation as needed
					var attribs = new Dictionary<string, object>();
					attribs["StartDate"] = new DateTimeOffset(DateTime.Now);

					var rd = new KnowledgeGraphRelationshipDescription(guidOrigin, guidDestination, attribs);
					//Add a create for the relationship to the operation
					edit_op.Create(emp_tbl, rd);

					//Do the create
					return edit_op.Execute();
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.OriginIDPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.DestinationIDPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Desktop.Editing.EditOperation.CreateChainedOperation
			//cref: ArcGIS.Desktop.Editing.RowToken
			//cref: ArcGIS.Desktop.Editing.RowToken.GlobalID
			#region Create a new Relationship and New Entities 1
			{
				var mv = MapView.Active;
				var create_rel1 = await QueuedTask.Run(() =>
				{
					//This example uses a chained edit operation
					var edit_op = new EditOperation()
					{
						Name = "Create entities and a relationship",
						SelectNewFeatures = true
					};

					//We are just going to use the GDB objects in this one but
					//we could equally use feature layers/standalone tables

					//using Feature Class/Tables (works within Investigation or map)
					var org_fc = kg.OpenDataset<FeatureClass>("Organization");
					var person_tbl = kg.OpenDataset<Table>("Person");
					//Relationship table
					var emp_tbl = kg.OpenDataset<Table>("HasEmployee");

					var attribs = new Dictionary<string, object>();

					//New Organization
					attribs["Name"] = "Acme Ltd.";
					attribs["Description"] = "Specializes in household items";
					attribs["SHAPE"] = mv.Extent.Center;//whatever is its location

					//Add it to the operation - we need the rowtoken
					var rowtoken = edit_op.Create(org_fc, attribs);

					attribs.Clear();//we are going to re-use the dictionary

					//New Person
					attribs["Name"] = "Bob";
					attribs["Age"] = "41";
					attribs["Skills"] = "Plumbing, Framing, Flooring";

					//Add it to the operation
					var rowtoken2 = edit_op.Create(person_tbl, attribs);

					attribs.Clear();

					//At this point we must execute the create of the entities
					if (edit_op.Execute())
					{
						//if we are here, the create of the entities was successful

						//Next, "chain" a second create for the relationship - this ensures that
						//Both creates (entities _and_ relation) will be -undone- together if needed
						//....in other words they will behave as if they are a -single- transaction
						var edit_op_rel = edit_op.CreateChainedOperation();

						//we need the names of the origin and destination relation properties
						var kg_prop_info = kg.GetPropertyNameInfo();
						//use the row tokens we held on to from the entity creates
						attribs[kg_prop_info.OriginIDPropertyName] = rowtoken.GlobalID;
						attribs[kg_prop_info.DestinationIDPropertyName] = rowtoken2.GlobalID;

						//Add any extra attribute information for the relation as needed
						attribs["StartDate"] = new DateTimeOffset(DateTime.Now);

						//Do the create of the relate
						edit_op_rel.Create(emp_tbl, attribs);
						return edit_op_rel.Execute();
					}
					return false;//Create of entities failed
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription.#ctor(ArcGIS.Desktop.Editing.RowHandle,ArcGIS.Desktop.Editing.RowHandle,System.Collections.Generic.IReadOnlyDictionary{System.String,System.Object})
			//cref: ArcGIS.Desktop.Editing.RowToken
			//cref: ArcGIS.Desktop.Editing.RowToken.GlobalID
			//cref: ArcGIS.Desktop.Editing.RowHandle.#ctor(ArcGIS.Desktop.Editing.RowToken)
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			#region Create a new Relationship and New Entities 2
			{
				var mv = MapView.Active;
				var createRel = await QueuedTask.Run(() =>
				{
					//This example uses a KnowledgeGraphRelationshipDescription
					var edit_op = new EditOperation()
					{
						Name = "Create entities and a relationship using a KG relate desc",
						SelectNewFeatures = true
					};

					//We are just going to use mapmembers in this example
					//we could equally use feature classes/tables
					var kg_layer = mv.Map.GetLayersAsFlattenedList()?
												.OfType<ArcGIS.Desktop.Mapping.KnowledgeGraphLayer>().First();
					//From the KG Layer get the relevant child feature layer(s) and/or standalone
					//table(s)
					var org_fl = kg_layer.GetLayersAsFlattenedList().OfType<FeatureLayer>()
													.First(child_layer => child_layer.Name == "Organization");

					var person_stbl = kg_layer.GetStandaloneTablesAsFlattenedList()
													.First(child_layer => child_layer.Name == "Person");

					var rel_stbl = kg_layer.GetStandaloneTablesAsFlattenedList()
													.First(child_layer => child_layer.Name == "HasEmployee");

					var attribs = new Dictionary<string, object>();

					//New Organization
					attribs["Name"] = "Acme Ltd.";
					attribs["Description"] = "Specializes in household items";
					attribs["SHAPE"] = mv.Extent.Center;//whatever is its location

					//Add it to the operation - we need the rowtoken
					var rowtoken_org = edit_op.Create(org_fl, attribs);

					attribs.Clear();//we are going to re-use the dictionary

					//New Person
					attribs["Name"] = "Bob";
					attribs["Age"] = "41";
					attribs["Skills"] = "Plumbing, Framing, Flooring";

					//Add it to the operation
					var rowtoken_person = edit_op.Create(person_stbl, attribs);

					attribs.Clear();

					//Create the new relationship using a KnowledgeGraphRelationshipDescription
					//Row handles act as the placeholders for the TO BE created new entities that will
					//be related
					var src_row_handle = new RowHandle(rowtoken_org);
					var dest_row_handle = new RowHandle(rowtoken_person);

					//Add any extra attribute information for the relation as needed
					attribs["StartDate"] = new DateTimeOffset(DateTime.Now);

					var rel_desc = new KnowledgeGraphRelationshipDescription(
																			src_row_handle, dest_row_handle, attribs);

					//Add the relate description to the edit operation
					edit_op.Create(rel_stbl, rel_desc);

					//Execute the create of the entities and relationship
					return edit_op.Execute();
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.ProvenancePropertyInfo      
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo.ProvenanceTypeNamePropertyName
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo.ProvenanceFieldNamePropertyName
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo.ProvenanceSourceNamePropertyName
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo.ProvenanceSourceTypePropertyName
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo.ProvenanceSourcePropertyName
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo.ProvenanceCommentPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.ProvenancePropertyInfo.ProvenanceInstanceIDPropertyName
			#region Create a Provenance Record
			{
				await QueuedTask.Run(() =>
				{
					//Instantiate an operation for the Create
					var edit_op = new EditOperation()
					{
						Name = "Create a new provenance record",
						SelectNewFeatures = true
					};

					//lets get the provenance table (provenance is not added to the
					//map TOC)
					var provenance_tbl = kg.OpenDataset<Table>("Provenance");
					if (provenance_tbl == null)
						return;
					//we will add a row to the provenance for person entity
					var person_tbl = kg.OpenDataset<Table>("Person");

					//Arbitrarily retrieve the first "person" row
					var instance_id = Guid.Empty;
					using (var rc = person_tbl.Search())
					{
						if (!rc.MoveNext())
							return;
						instance_id = rc.Current.GetGlobalID();//Get the global id
					}

					//Define the provenance attributes - we need the names
					//of the provenance properties from the KG ProvenancePropertyInfo
					var kg_prop_info = kg.GetPropertyNameInfo();
					var attribs = new Dictionary<string, object>();
					var ppi = kg_prop_info.ProvenancePropertyInfo;

					attribs[ppi.ProvenanceTypeNamePropertyName] =
							person_tbl.GetDefinition().GetName();//entity type name
					attribs[ppi.ProvenanceFieldNamePropertyName] = "name";//Must be a property/field on the entity
					attribs[ppi.ProvenanceSourceNamePropertyName] = "Annual Review 2024";//can be anything - can be null
																																							 //note: Source type is controlled by the CodedValueDomain "esri__provenanceSourceType"
					attribs[ppi.ProvenanceSourceTypePropertyName] = "Document";//one of ["Document", "String", "URL"].
					attribs[ppi.ProvenanceSourcePropertyName] = "HR records";//can be anything, not null
					attribs[ppi.ProvenanceCommentPropertyName] = "Rock star";//can be anything - can be null

					//Add in the id of the provenance owner - our "person" in this case
					attribs[ppi.ProvenanceInstanceIDPropertyName] = instance_id;

					//Specify any additional custom attributes added to the provenance
					//schema by the user as needed....
					//attribs["custom_attrib"] = "Foo";
					//attribs["custom_attrib2"] = "Bar";

					//Create the provenance row
					edit_op.Create(provenance_tbl, attribs);
					if (edit_op.Execute())
					{
						//TODO: Operation succeeded
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.SupportsProvenance
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.ProvenanceTypeName
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphProvenanceDescription
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphSourceType
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.KnowledgeGraphProvenanceDescription)
			#region Create a Provenance Record 2
			{
				await QueuedTask.Run(() =>
				{
					// check if provenance supported
					var propInfo = kg.GetPropertyNameInfo();
					if (!propInfo.SupportsProvenance)
						return;

					//Instantiate an operation for the Create
					var edit_op = new EditOperation()
					{
						Name = "Create a new provenance record",
						SelectNewFeatures = true
					};

					var provName = propInfo.ProvenanceTypeName;

					//we will add a row to the provenance for person entity
					var person_tbl = kg.OpenDataset<Table>("Person");

					//Arbitrarily retrieve the first "person" row
					var instance_id = Guid.Empty;
					using (var rc = person_tbl.Search())
					{
						if (!rc.MoveNext())
							return;
						instance_id = rc.Current.GetGlobalID();//Get the global id
					}

					var originHandle = new RowHandle(person_tbl, instance_id);
					var pd = new KnowledgeGraphProvenanceDescription(
						originHandle, "name", KnowledgeGraphSourceType.Document,
						"Annual Review 2024", "HR records", "Rock star");

					//Create the provenance row
					edit_op.Create(pd);
					if (edit_op.Execute())
					{
						//TODO: Operation succeeded
					}

				});
			}
			#endregion

			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphDocumentDescription
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Desktop.Editing.KnowledgeGraphDocumentDescription)
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphProvenanceDescription
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.KnowledgeGraphProvenanceDescription)
			#region Create an Entity, a Document, a HasDocument and a Provenance record
			{
				await QueuedTask.Run(() =>
				{
					//Instantiate an operation for the Create
					var edit_op = new EditOperation()
					{
						Name = "Create a records",
						SelectNewFeatures = true
					};

					// create the entity
					var personToken = edit_op.Create(personLayer, personAtts);

					// create the document
					var kgDocDesc = new KnowledgeGraphDocumentDescription(
						@"D:\Data\BirthCertificate.jpg");
					var docToken = edit_op.Create(docLayer, kgDocDesc);

					// create RowHandles from the returned RowTokens
					var personHandle = new RowHandle(personToken);
					var docHandle = new RowHandle(docToken);

					// create the "hasDocument" relationship
					var rd = new KnowledgeGraphRelationshipDescription(personHandle, docHandle);
					edit_op.Create(docLayer, rd);

					// create the provenance record for the person entity using the document entity
					// provenance record is on the "name" field 
					var pd = new KnowledgeGraphProvenanceDescription(
						personHandle, "name", docHandle, "", "comments");
					edit_op.Create(pd);

					// execute - create all the entities and relationship rows _together_
					edit_op.Execute();
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.OriginIDPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.DestinationIDPropertyName
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole.Document
			//cref: ArcGIS.Desktop.Editing.EditOperation.CreateChainedOperation
			//cref: ArcGIS.Desktop.Editing.RowToken
			//cref: ArcGIS.Desktop.Editing.RowToken.GlobalID
			#region Create a Document Record 1
			{
				var ok = await QueuedTask.Run(() =>
				{
					using (var kg_for_doc = kgLayer.GetDatastore())
					{
						var edit_op = new EditOperation()
						{
							Name = "Create Document Example",
							SelectNewFeatures = true
						};

						var doc_entity_name = GetDocumentTypeName(kg_for_doc.GetDataModel());
						if (string.IsNullOrEmpty(doc_entity_name))
							return false;
						var hasdoc_rel_name = GetHasDocumentTypeName(kg_for_doc.GetDataModel());
						if (string.IsNullOrEmpty(hasdoc_rel_name))
							return false;

						//Document can also be FeatureClass
						var doc_tbl = kg_for_doc.OpenDataset<Table>(doc_entity_name);
						var doc_rel_tbl = kg_for_doc.OpenDataset<Table>(hasdoc_rel_name);

						//This is the document to be added...file, image, resource, etc.
						var doc_url = @"E:\Data\Temp\HelloWorld.txt";
						var text = System.IO.File.ReadAllText(url);

						//Set document properties
						var attribs = new Dictionary<string, object>();
						attribs["contentType"] = @"text/plain";
						attribs["name"] = System.IO.Path.GetFileName(url);
						attribs["url"] = doc_url;
						//Add geometry if relevant
						//attribs["Shape"] = doc_location;

						//optional
						attribs["fileExtension"] = System.IO.Path.GetExtension(doc_url);
						attribs["text"] = System.IO.File.ReadAllText(doc_url);

						//optional and arbitrary - your choice
						attribs["title"] = System.IO.Path.GetFileNameWithoutExtension(doc_url);
						attribs["keywords"] = @"text,file,example";
						attribs["metadata"] = "";

						//Specify any additional custom attributes added to the document
						//schema by the user as needed....
						//attribs["custom_attrib"] = "Foo";
						//attribs["custom_attrib2"] = "Bar";

						//Get the entity whose document this is...
						var org_fc = kg_for_doc.OpenDataset<FeatureClass>("Organization");
						var qf = new QueryFilter()
						{
							WhereClause = "name = 'Acme'",
							SubFields = "*"
						};
						var origin_org_id = Guid.Empty;
						using (var rc = org_fc.Search(qf))
						{
							if (!rc.MoveNext())
								return false;
							origin_org_id = rc.Current.GetGlobalID();//For the relate
						}

						//Create the document row/feature
						var rowtoken = edit_op.Create(doc_tbl, attribs);
						if (edit_op.Execute())
						{
							//Create the relationship row
							attribs.Clear();
							//we need the names of the origin and destination relation properties
							var kg_prop_info = kg_for_doc.GetPropertyNameInfo();
							//Specify the origin entity (i.e. the document 'owner') and
							//the document being related to (i.e. the document 'itself')
							attribs[kg_prop_info.OriginIDPropertyName] = origin_org_id;//entity
							attribs[kg_prop_info.DestinationIDPropertyName] = rowtoken.GlobalID;//document

							//Specify any custom attributes added to the has document
							//schema by the user as needed....
							//attribs["custom_attrib"] = "Foo";
							//attribs["custom_attrib2"] = "Bar";

							//"Chain" a second create for the relationship - this ensures that
							//Both creates (doc _and_ "has doc" relation) will be -undone- together if needed
							//....in other words they will behave as if they are a -single- transaction
							var edit_op_rel = edit_op.CreateChainedOperation();
							edit_op_rel.Create(doc_rel_tbl, attribs);
							return edit_op_rel.Execute();
						}
						return false;
					}
				});
			}

			string GetDocumentTypeName(KnowledgeGraphDataModel kg_dm)
			{
				var entity_types = kg_dm.GetEntityTypes();
				foreach (var entity_type in entity_types)
				{
					var role = entity_type.Value.GetRole();
					if (role == KnowledgeGraphNamedObjectTypeRole.Document)
						return entity_type.Value.GetName();
				}
				return "";
			}

			string GetHasDocumentTypeName(KnowledgeGraphDataModel kg_dm)
			{
				var rel_types = kg_dm.GetRelationshipTypes();
				foreach (var rel_type in rel_types)
				{
					var role = rel_type.Value.GetRole();
					if (role == KnowledgeGraphNamedObjectTypeRole.Document)
						return rel_type.Value.GetName();
				}
				return "";
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.SupportsDocuments
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.DocumentTypeName
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphDocumentDescription
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, ArcGIS.Core.Data.Knowledge.KnowledgeGraphDocumentDescription)
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, ArcGIS.Core.Data.Knowledge.KnowledgeGraphDocumentDescription)
			//cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table,ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			#region Create a Document Record 2
			{
				await QueuedTask.Run(() =>
				{
					using (var kg_for_doc = kgLayer.GetDatastore())
					{
						var propInfo = kg_for_doc.GetPropertyNameInfo();
						if (!propInfo.SupportsDocuments)
							return false;

						var edit_op = new EditOperation()
						{
							Name = "Create Document Example",
							SelectNewFeatures = true
						};

						var doc_entity_name = propInfo.DocumentTypeName;
						var hasdoc_rel_name = GetHasDocumentTypeName(kg.GetDataModel());

						//Document can also be FeatureClass
						var doc_tbl = kg_for_doc.OpenDataset<Table>(doc_entity_name);
						var doc_rel_tbl = kg_for_doc.OpenDataset<Table>(hasdoc_rel_name);

						//This is the document to be added...file, image, resource, etc.
						var doc_url = @"E:\Data\Temp\HelloWorld.txt";

						// create the KnowledgeGraphDocumentDescription
						var kgDocDesc = new KnowledgeGraphDocumentDescription(doc_url);

						// if there is a geometry use the following ctor
						// var kgDocDesc = new KnowledgeGraphDocumentDescription(doc_url, doc_location);

						// if you have additional custom attributes 
						//var customDocAtts = new Dictionary<string, object>();
						//customDocAtts.Add("custom_attrib", "Foo");
						//customDocAtts.Add("custom_attrib2", "Bar");
						//var kgDocDesc = new KnowledgeGraphDocumentDescription(url, null, customDocAtts);

						// add additional properties if required
						kgDocDesc.Keywords = @"text,file,example";

						// The Create method will auto-populate the Url, Name, FileExtension and contentType fields of the document row
						// from the path supplied.  
						var rowToken = edit_op.Create(doc_tbl, kgDocDesc);

						//Get the entity whose document this is...
						var org_fc = kg_for_doc.OpenDataset<FeatureClass>("Organization");
						var qf = new QueryFilter()
						{
							WhereClause = "name = 'Acme'",
							SubFields = "*"
						};
						var origin_org_id = Guid.Empty;
						using (var rc = org_fc.Search(qf))
						{
							if (!rc.MoveNext())
								return false;
							origin_org_id = rc.Current.GetGlobalID();//For the relate
						}

						// set up the row handles
						var originHandle = new RowHandle(org_fc, origin_org_id);    // entity
						var destinationHandle = new RowHandle(rowToken);            // document

						// create the KnowledgeGraphRelationshipDescription
						var rd = new KnowledgeGraphRelationshipDescription(originHandle, destinationHandle);

						// if you have additional custom attributes for the "HasDocument" relationship
						//var customHasDocAtts = new Dictionary<string, object>();
						//customHasDocAtts.Add("custom_attrib", "Foo");
						//customHasDocAtts.Add("custom_attrib2", "Bar");
						//var rd = new KnowledgeGraphRelationshipDescription(originHandle, destinationHandle, customHasDocAtts);

						// create the relate record using the same edit operation
						edit_op.Create(doc_rel_tbl, rd);

						//Call execute to create all the entities and relationship rows _together_
						return edit_op.Execute();
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetPropertyNameInfo
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPropertyInfo.OriginIDPropertyName
			#region Modify an Entity and Relationship record
			{
				var mv = MapView.Active;
				await QueuedTask.Run(() =>
				{
					var edit_op = new EditOperation()
					{
						Name = "Modify an Entity and Relationship record",
						SelectModifiedFeatures = true
					};

					//We are  going to use mapmembers in this example
					//we could equally use feature classes/tables
					var kg_layer = mv.Map.GetLayersAsFlattenedList()?
												.OfType<ArcGIS.Desktop.Mapping.KnowledgeGraphLayer>().First();
					//Entity
					var org_fl = kg_layer.GetLayersAsFlattenedList().OfType<FeatureLayer>()
													.First(child_layer => child_layer.Name == "Organization");
					//and/or Relationship
					var rel_stbl = kg_layer.GetStandaloneTablesAsFlattenedList()
													.First(child_layer => child_layer.Name == "HasEmployee");

					//Get the entity feature to modify
					long org_oid = -1;
					var org_gid = Guid.Empty;
					var qf = new QueryFilter()
					{
						WhereClause = "name = 'Acme'",
						SubFields = "*"
					};
					using (var rc = org_fl.Search(qf))
					{
						if (!rc.MoveNext())
							return;
						org_oid = rc.Current.GetObjectID();
						org_gid = rc.Current.GetGlobalID();
					}
					if (org_oid == -1)
						return; //nothing to modify

					var attribs = new Dictionary<string, object>();

					//Specify attributes to be updated
					attribs["Name"] = "Acme Ltd.";
					attribs["Description"] = "Specializes in household items";
					attribs["SHAPE"] = mv.Extent.Center;

					//Add to the edit operation
					edit_op.Modify(org_fl, org_oid, attribs);

					//Get the relationship record (if a relate is being updated)
					//we need the name of the origin id property
					var kg_prop_info = kg.GetPropertyNameInfo();
					var sql = $"{kg_prop_info.OriginIDPropertyName} = ";
					sql += "'" + org_gid.ToString("B").ToUpper() + "'";

					qf = new QueryFilter()
					{
						WhereClause = sql,
						SubFields = "*"
					};
					long rel_oid = -1;
					using (var rc = rel_stbl.Search(qf))
					{
						if (!rc.MoveNext())
							return;
						rel_oid = rc.Current.GetObjectID();
					}
					if (rel_oid > -1)
					{
						//add the relate row updates to the edit operation
						attribs.Clear();//we are going to re-use the dictionary
						attribs["StartDate"] = new DateTimeOffset(DateTime.Now);
						attribs["custom_attrib"] = "Foo";
						attribs["custom_attrib2"] = "Bar";
						//Add to the edit operation
						edit_op.Modify(rel_stbl, rel_oid, attribs);
					}
					//do the update(s)
					if (edit_op.Execute())
					{
						//TODO: Operation succeeded
					}

				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
			#region Delete an Entity record
			{
				var mv = MapView.Active;
				await QueuedTask.Run(() =>
				{
					var edit_op = new EditOperation()
					{
						Name = "Delete an Entity record"
					};

					//We are  going to use mapmembers in this example
					//we could equally use feature classes/tables
					var kg_layer = mv.Map.GetLayersAsFlattenedList()?
												.OfType<ArcGIS.Desktop.Mapping.KnowledgeGraphLayer>().First();
					//Entity
					var org_fl = kg_layer.GetLayersAsFlattenedList().OfType<FeatureLayer>()
													.First(child_layer => child_layer.Name == "Organization");

					//Get the entity feature(s) to delete
					long org_oid = -1;
					var qf = new QueryFilter()
					{
						WhereClause = "name = 'Acme'",
						SubFields = "*"
					};
					using (var rc = org_fl.Search(qf))
					{
						if (!rc.MoveNext())
							return;//nothing to delete
						org_oid = rc.Current.GetObjectID();
					}

					edit_op.Delete(org_fl, org_oid);
					edit_op.Execute();//Do the delete
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			#region Delete a Relationship record 1
			{
				var mv = MapView.Active;
				await QueuedTask.Run(() =>
				{
					var edit_op = new EditOperation()
					{
						Name = "Delete a Relationship record"
					};

					//We are  going to use mapmembers in this example
					//we could equally use feature classes/tables
					var kg_layer = mv.Map.GetLayersAsFlattenedList()?
												.OfType<ArcGIS.Desktop.Mapping.KnowledgeGraphLayer>().First();
					//Relationship
					var rel_stbl = kg_layer.GetStandaloneTablesAsFlattenedList()
													.First(child_layer => child_layer.Name == "HasEmployee");

					//Get the relation row to delete
					long rel_oid = -1;
					using (var rc = rel_stbl.Search())
					{
						if (!rc.MoveNext())
							return;
						//arbitrarily, in this example, get the first row
						rel_oid = rc.Current.GetObjectID();
					}

					edit_op.Delete(rel_stbl, rel_oid);
					edit_op.Execute();//Do the delete
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription
			//cref: ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription.#ctor(System.Guid,System.Guid,System.Collections.Generic.IReadOnlyDictionary{System.String,System.Object})
			//cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Core.Data.Table, ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			//cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember, ArcGIS.Desktop.Editing.KnowledgeGraphRelationshipDescription)
			#region Delete a Relationship record 2
			{
				var mv = MapView.Active;
				await QueuedTask.Run(() =>
				{

					var edit_op = new EditOperation()
					{
						Name = "Delete a Relationship record"
					};

					//We are  going to use mapmembers in this example
					//we could equally use feature classes/tables
					var kg_layer = mv.Map.GetLayersAsFlattenedList()?
												.OfType<ArcGIS.Desktop.Mapping.KnowledgeGraphLayer>().First();

					//entities
					var entityOrg = kg_layer.GetStandaloneTablesAsFlattenedList()
													.First(child_layer => child_layer.Name == "Organization");
					var entityPerson = kg_layer.GetStandaloneTablesAsFlattenedList()
													.First(child_layer => child_layer.Name == "Person");

					//Relationship
					var rel_stbl = kg_layer.GetStandaloneTablesAsFlattenedList()
													.First(child_layer => child_layer.Name == "HasEmployee");

					// get the origin, destination records
					Guid guidOrigin = Guid.Empty;
					Guid guidDestination = Guid.Empty;
					using (var rc = entityOrg.Search())
					{
						if (rc.MoveNext())
							//Use the KnowledgeGraphPropertyInfo to avoid hardcoding...
							guidOrigin = rc.Current.GetGlobalID();
					}
					using (var rc = entityPerson.Search())
					{
						if (rc.MoveNext())
							//Use the KnowledgeGraphPropertyInfo to avoid hardcoding...
							guidDestination = rc.Current.GetGlobalID();
					}

					var rd = new KnowledgeGraphRelationshipDescription(guidOrigin, guidDestination);
					edit_op.Delete(rel_stbl, rd);
					edit_op.Execute();//Do the delete
				});
			}
			#endregion

			#region ProSnippet Group: Schema Edits
			#endregion

			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.#ctor(ArcGIS.Core.Data.Knowledge.KnowledgeGraph)
			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphTypeDescription)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphExtensions.ApplySchemaEdits(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.Data.DDL.SchemaBuilder)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphEntityTypeDescription
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphEntityTypeDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription})
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphEntityTypeDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription},ArcGIS.Core.Data.DDL.ShapeDescription)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphRelationshipTypeDescription
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphRelationshipTypeDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription})
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphRelationshipTypeDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription},ArcGIS.Core.Data.DDL.ShapeDescription)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription.#ctor(System.String,ArcGIS.Core.Data.FieldType)
			#region Create Entity and Relationship Types with SchemaBuilder
			{
				await QueuedTask.Run(() =>
				{
					using (var kg = GetKnowledgeGraph(url))
					{
						var entity_name = "PhoneCall";
						var relate_name = "WhoCalledWho";

						//Entity Fields
						var descs1 =
								new List<KnowledgeGraphPropertyDescription>();
						descs1.Add(
							new KnowledgeGraphPropertyDescription("PhoneOwner", FieldType.String));
						descs1.Add(
							new KnowledgeGraphPropertyDescription("PhoneNumber", FieldType.String));
						descs1.Add(
							new KnowledgeGraphPropertyDescription("LocationID", FieldType.BigInteger));
						descs1.Add(
							new KnowledgeGraphPropertyDescription("DateAndTime", FieldType.Date));

						//Relate Fields
						var descs2 =
								new List<KnowledgeGraphPropertyDescription>();
						descs2.Add(
							new KnowledgeGraphPropertyDescription("Foo", FieldType.String));
						descs2.Add(
							new KnowledgeGraphPropertyDescription("Bar", FieldType.String));


						var includeShape = true;//change to false to omit the shape column
						var hasZ = false;
						var hasM = false;

						KnowledgeGraphEntityTypeDescription entityDesc = null;
						KnowledgeGraphRelationshipTypeDescription relateDesc = null;
						if (includeShape)
						{
							var sr = kg.GetSpatialReference();
							var shp_desc = new ShapeDescription(GeometryType.Point, sr)
							{
								HasM = hasM,
								HasZ = hasZ
							};
							entityDesc = new KnowledgeGraphEntityTypeDescription(
								entity_name, descs1, shp_desc);
							relateDesc = new KnowledgeGraphRelationshipTypeDescription(
								relate_name, descs2, shp_desc);
						}
						else
						{
							entityDesc = new KnowledgeGraphEntityTypeDescription(
								entity_name, descs1);
							relateDesc = new KnowledgeGraphRelationshipTypeDescription(
								relate_name, descs2);
						}
						//Run the schema builder
						try
						{
							SchemaBuilder sb = new(kg);
							sb.Create(entityDesc);
							sb.Create(relateDesc);
							//Use the KnowledgeGraph extension method 'ApplySchemaEdits(...)'
							//to refresh the Pro UI
							if (!kg.ApplySchemaEdits(sb))
							{
								var err_msg = string.Join(",", sb.ErrorMessages.ToArray());
								System.Diagnostics.Debug.WriteLine($"Entity/Relate Create error: {err_msg}");
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.#ctor(ArcGIS.Core.Data.Knowledge.KnowledgeGraph)
			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphExtensions.ApplySchemaEdits(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.Data.DDL.SchemaBuilder)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphEntityTypeDescription.#ctor(System.String)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphRelationshipTypeDescription.#ctor(System.String)
			#region Delete Entity and Relationship Types with SchemaBuilder
			{
				await QueuedTask.Run(() =>
				{
					using (var kg = GetKnowledgeGraph(url))
					{
						var entity_name = "PhoneCall";
						var relate_name = "WhoCalledWho";

						var entityDesc = new KnowledgeGraphEntityTypeDescription(entity_name);
						var relateDesc = new KnowledgeGraphRelationshipTypeDescription(relate_name);

						//Run the schema builder
						try
						{
							SchemaBuilder sb = new(kg);
							sb.Delete(entityDesc);
							sb.Delete(relateDesc);
							//Use the KnowledgeGraph extension method 'ApplySchemaEdits(...)'
							//to refresh the Pro UI
							if (!kg.ApplySchemaEdits(sb))
							{
								var err_msg = string.Join(",", sb.ErrorMessages.ToArray());
								System.Diagnostics.Debug.WriteLine($"Entity/Relate Delete error: {err_msg}");
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphTypeDescription)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphExtensions.ApplySchemaEdits(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.Data.DDL.SchemaBuilder)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphEntityTypeDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription},ArcGIS.Core.Data.DDL.ShapeDescription)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphRelationshipTypeDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription},ArcGIS.Core.Data.DDL.ShapeDescription)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription.#ctor(ArcGIS.Core.Data.Knowledge.KnowledgeGraphProperty)
			//cref: ArcGIS.Core.Data.DDL.Knowledge.KnowledgeGraphPropertyDescription.CreateStringProperty(System.String,System.Int32)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetIsSpatial
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetShapeDefinition
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetShapeField
			#region Modify Entity and Relationship Type Schemas with SchemaBuilder
			{
				await QueuedTask.Run(() =>
				{
					using (var kg = GetKnowledgeGraph(url))
					{
						var entity_name = "PhoneCall";
						var relate_name = "WhoCalledWho";

						var kvp_entity = kg.GetDataModel().GetEntityTypes()
								 .First(r => r.Key == entity_name);
						var kvp_relate = kg.GetDataModel().GetRelationshipTypes()
													 .First(r => r.Key == relate_name);

						//Let's delete one field and add a new one from each
						//A field gets deleted implicitly if it is not included in the list of
						//fields - or "properties" in this case....so we will remove the last
						//property from the list
						var entity_props = kvp_entity.Value.GetProperties().Reverse().Skip(1).Reverse();
						var prop_descs = new List<KnowledgeGraphPropertyDescription>();

						foreach (var prop in entity_props)
						{
							if (prop.FieldType == FieldType.Geometry)
							{
								continue;//skip shape
							}
							var prop_desc = new KnowledgeGraphPropertyDescription(prop);
							prop_descs.Add(prop_desc);
						}
						//deal with shape - we need to keep it
						//SchemaBuilder deletes any field not included in the "modify" list
						ShapeDescription shape_desc = null;
						if (kvp_entity.Value.GetIsSpatial())
						{
							var geom_def = kvp_entity.Value.GetShapeDefinition();
							var shape_name = kvp_entity.Value.GetShapeField();
							shape_desc = new ShapeDescription(
								shape_name, geom_def.geometryType, geom_def.sr);
						}
						//add the new entity property
						prop_descs.Add(
							KnowledgeGraphPropertyDescription.CreateStringProperty("foo", 10));
						//make a description for the entity type - ok if shape_desc is null
						var entityDesc = new KnowledgeGraphEntityTypeDescription(
							entity_name, prop_descs, shape_desc);

						//Add the entity type description to the schema builder using 'Modify'
						SchemaBuilder sb = new(kg);
						sb.Modify(entityDesc);

						//Repeat for the relationship - assuming we have at least one custom attribute field
						//that can be deleted on our relationship schema...
						var rel_props = kvp_relate.Value.GetProperties().Reverse().Skip(1).Reverse();
						var rel_prop_descs = new List<KnowledgeGraphPropertyDescription>();

						foreach (var prop in rel_props)
						{
							if (prop.FieldType == FieldType.Geometry)
							{
								continue;//skip shape
							}
							var prop_desc = new KnowledgeGraphPropertyDescription(prop);
							rel_prop_descs.Add(prop_desc);
						}
						//deal with shape - we need to keep it
						//SchemaBuilder deletes any field not included in the "modify" list
						ShapeDescription shape_desc_rel = null;
						if (kvp_relate.Value.GetIsSpatial())
						{
							var geom_def = kvp_relate.Value.GetShapeDefinition();
							var shape_name = kvp_relate.Value.GetShapeField();
							shape_desc_rel = new ShapeDescription(
								shape_name, geom_def.geometryType, geom_def.sr);
						}
						//add a new relationship property
						rel_prop_descs.Add(
							KnowledgeGraphPropertyDescription.CreateStringProperty("bar", 10));
						//make a description for the relationship type - ok if shape_desc is null
						var relDesc = new KnowledgeGraphRelationshipTypeDescription(
							relate_name, rel_prop_descs, shape_desc_rel);

						//Add the relationship type description to the schema builder using 'Modify'
						sb.Modify(relDesc);

						//Run the schema builder
						try
						{
							//Use the KnowledgeGraph extension method 'ApplySchemaEdits(...)'
							//to refresh the Pro UI
							if (!kg.ApplySchemaEdits(sb))
							{
								var err_msg = string.Join(",", sb.ErrorMessages.ToArray());
								System.Diagnostics.Debug.WriteLine($"Entity/Relate Modify error: {err_msg}");
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.IndexDescription)
			//cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription
			//cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription.#ctor(System.String,ArcGIS.Core.Data.DDL.TableDescription,System.Collections.Generic.IEnumerable{System.String})
			#region Create Attribute Indexes on KG Schemas with SchemaBuilder
			{
				await QueuedTask.Run(() =>
				{
					using (var kg = GetKnowledgeGraph(url))
					{
						var entity_name = "PhoneCall";

						//indexes are managed on the GDB objects...
						var entity_table_def = kg.GetDefinition<TableDefinition>(entity_name);
						var entity_table_desc = new TableDescription(entity_table_def);

						var entity_table_flds = entity_table_def.GetFields();
						AttributeIndexDescription attr_index1 = null;
						AttributeIndexDescription attr_index2 = null;
						foreach (var fld in entity_table_flds)
						{
							//index the first string field
							if (fld.FieldType == FieldType.String && attr_index1 == null)
							{
								if (fld.Name == "ESRI__ID")//special case
									continue;
								//Index _must_ be ascending for KG
								attr_index1 = new AttributeIndexDescription(
									"Index1", entity_table_desc, new List<string> { fld.Name })
								{
									IsAscending = true
								};
							}
							//index the first numeric field (if there is one)
							if ((fld.FieldType == FieldType.BigInteger ||
									 fld.FieldType == FieldType.Integer ||
									 fld.FieldType == FieldType.Single ||
									 fld.FieldType == FieldType.SmallInteger ||
									 fld.FieldType == FieldType.Double) && attr_index2 == null)
							{
								attr_index2 = new AttributeIndexDescription(
									"Index2", entity_table_desc, new List<string> { fld.Name })
								{
									IsAscending = true,
									IsUnique = true //optional - unique if all values are to be unique in the index
								};
							}
							if (attr_index1 != null && attr_index2 != null) break;
						}

						if (attr_index1 == null && attr_index2 == null)
							return; //nothing to index

						//Run the schema builder
						try
						{
							SchemaBuilder sb = new(kg);
							if (attr_index1 != null)
								sb.Create(attr_index1);
							if (attr_index2 != null)
								sb.Create(attr_index2);
							if (!kg.ApplySchemaEdits(sb))
							{
								var err_msg = string.Join(",", sb.ErrorMessages.ToArray());
								System.Diagnostics.Debug.WriteLine($"Create index error: {err_msg}");
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
			//cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription
			//cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription.#ctor(System.String,ArcGIS.Core.Data.DDL.TableDescription,System.Collections.Generic.IEnumerable{System.String})
			#region Delete Attribute Indexes on KG Schemas with SchemaBuilder
			{
				await QueuedTask.Run(() =>
				{
					using (var kg = GetKnowledgeGraph(url))
					{
						var entity_name = "PhoneCall";

						//indexes are managed on the GDB objects...
						var entity_table_def = kg.GetDefinition<TableDefinition>(entity_name);
						var entity_table_desc = new TableDescription(entity_table_def);

						var indexes = entity_table_def.GetIndexes();
						foreach (var idx in indexes)
						{
							System.Diagnostics.Debug.WriteLine($"Index {idx.GetName()}");
						}
						var idx1 = indexes.FirstOrDefault(
							idx => idx.GetName().ToLower() == "Index1".ToLower());
						var idx2 = indexes.FirstOrDefault(
							idx => idx.GetName().ToLower() == "Index2".ToLower());

						if (idx1 == null && idx2 == null)
							return;

						//Run the schema builder
						try
						{
							SchemaBuilder sb = new(kg);

							if (idx1 != null)
							{
								var idx_attr = new AttributeIndexDescription(idx1, entity_table_desc);
								sb.Delete(idx_attr);
							}
							if (idx2 != null)
							{
								var idx_attr = new AttributeIndexDescription(idx2, entity_table_desc);
								sb.Delete(idx_attr);
							}

							if (!kg.ApplySchemaEdits(sb))
							{
								var err_msg = string.Join(",", sb.ErrorMessages.ToArray());
								System.Diagnostics.Debug.WriteLine($"Delete index error: {err_msg}");
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.CodedValueDomainDescription)
			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.RangeDomainDescription)
			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.FeatureClassDescription)
			//cref: ArcGIS.Core.Data.DDL.RangeDomainDescription.#ctor(System.String,ArcGIS.Core.Data.FieldType,System.Object,System.Object)
			//cref: ArcGIS.Core.Data.DDL.CodedValueDomainDescription.#ctor(System.String,ArcGIS.Core.Data.FieldType,System.Collections.Generic.SortedList{System.Object,System.String})
			//cref: ArcGIS.Core.Data.DDL.FieldDescription.#ctor(System.String,ArcGIS.Core.Data.FieldType)
			//cref: ArcGIS.Core.Data.DDL.FieldDescription.SetDomainDescription(ArcGIS.Core.Data.DDL.DomainDescription,System.Nullable{System.Int32})
			//cref: ArcGIS.Core.Data.DDL.FeatureClassDescription.#ctor(ArcGIS.Core.Data.FeatureClassDefinition)
			//cref: ArcGIS.Core.Data.DDL.FeatureClassDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.FieldDescription},ArcGIS.Core.Data.DDL.ShapeDescription)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphExtensions.ApplySchemaEdits(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.Data.DDL.SchemaBuilder)
			#region Create Domain and Field Definition on KG Schemas with SchemaBuilder
			{
				await QueuedTask.Run(() =>
				{
					using (var kg = GetKnowledgeGraph(url))
					{
						var entity_name = "Fruit";

						//Domains are managed on the GDB objects...
						var fruit_fc = kg.OpenDataset<FeatureClass>(entity_name);
						var fruit_fc_def = fruit_fc.GetDefinition();

						var fieldFruitTypes = fruit_fc_def.GetFields()
									.FirstOrDefault(f => f.Name == "FruitTypes");
						var fieldShelfLife = fruit_fc_def.GetFields()
								.FirstOrDefault(f => f.Name == "ShelfLife");

						//Create a coded value domain and add it to a new field
						var fruit_cvd_desc = new CodedValueDomainDescription(
							"FruitTypes", FieldType.String,
							new SortedList<object, string> {
													{ "A", "Apple" },
													{ "B", "Banana" },
													{ "C", "Coconut" }
							})
						{
							SplitPolicy = SplitPolicy.Duplicate,
							MergePolicy = MergePolicy.DefaultValue
						};

						//Create a Range Domain and add the domain to a new field description also
						var shelf_life_rd_desc = new RangeDomainDescription(
																					"ShelfLife", FieldType.Integer, 0, 14);

						var sb = new SchemaBuilder(kg);
						sb.Create(fruit_cvd_desc);
						sb.Create(shelf_life_rd_desc);

						//Create the new field descriptions that will be associated with the
						//"new" FruitTypes coded value domain and the ShelfLife range domain
						var fruit_types_fld = new ArcGIS.Core.Data.DDL.FieldDescription(
																					"FruitTypes", FieldType.String);
						fruit_types_fld.SetDomainDescription(fruit_cvd_desc);

						//ShelfLife will use the range domain
						var shelf_life_fld = new ArcGIS.Core.Data.DDL.FieldDescription(
		"ShelfLife", FieldType.Integer);
						shelf_life_fld.SetDomainDescription(shelf_life_rd_desc);

						//Add the descriptions to the list of field descriptions for the
						//fruit feature class - Modify schema needs _all_ fields to be included
						//in the schema, not just the new ones to be added.
						var fruit_fc_desc = new FeatureClassDescription(fruit_fc_def);

						var modified_fld_descs = new List<ArcGIS.Core.Data.DDL.FieldDescription>(
							fruit_fc_desc.FieldDescriptions);

						modified_fld_descs.Add(fruit_types_fld);
						modified_fld_descs.Add(shelf_life_fld);

						//Create a feature class description to modify the fruit entity
						//with the new fields and their associated domains
						var updated_fruit_fc =
							new FeatureClassDescription(entity_name, modified_fld_descs,
																					fruit_fc_desc.ShapeDescription);

						//Add the modified fruit fc desc to the schema builder
						sb.Modify(updated_fruit_fc);

						//Run the schema builder
						try
						{
							if (!kg.ApplySchemaEdits(sb))
							{
								var err_msg = string.Join(",", sb.ErrorMessages.ToArray());
								System.Diagnostics.Debug.WriteLine($"Create domains error: {err_msg}");
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.DDL.RangeDomainDescription.#ctor(ArcGIS.Core.Data.RangeDomain)
			//cref: ArcGIS.Core.Data.DDL.CodedValueDomainDescription.#ctor(ArcGIS.Core.Data.CodedValueDomain)
			//cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDomains
			//cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
			#region Delete Domain on KG Schemas with SchemaBuilder
			{
				await QueuedTask.Run(() =>
				{
					using (var kg = GetKnowledgeGraph(url))
					{
						//Get all the domains in the KG
						var domains = kg.GetDomains();
						var sb = new SchemaBuilder(kg);

						foreach (var domain in domains)
						{
							//skip the special provenance domain
							var name = domain.GetName();
							if (string.Compare(name, "esri__provenanceSourceType", true) == 0)
								continue;//skip this one

							//Delete all other domains
							if (domain is RangeDomain rd)
								sb.Delete(new RangeDomainDescription(rd));
							else if (domain is CodedValueDomain cvd)
								sb.Delete(new CodedValueDomainDescription(cvd));
						}

						try
						{
							//note: will throw an InvalidOperationException if there are no operations
							//to run. Will also delete associated fields dependent on deleted domain(s)
							if (!kg.ApplySchemaEdits(sb))
							{
								var err_msg = string.Join(",", sb.ErrorMessages.ToArray());
								System.Diagnostics.Debug.WriteLine($"Delete domains error: {err_msg}");
							}
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				});
			}

			KnowledgeGraph GetKnowledgeGraph(string url)
			{
				var kg_props =
					new KnowledgeGraphConnectionProperties(new Uri(url));
				return new KnowledgeGraph(kg_props);
			}
			#endregion

			#region ProSnippet Group: KnowledgeGraph Centrality
			#endregion


			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration
			//cref: ArcGIS.Core.CIM.CentralityMeasure
			//cref: ArcGIS.Core.Data.Knowledge.Extensions.KnowledgeGraphExtensions.ComputeCentrality(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration,ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph,System.Collections.Generic.IEnumerable{ArcGIS.Core.CIM.CentralityMeasure})
			#region Compute Centrality Using Defaults
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ....) {
					//take default settings...
					//undirected relationship interpretation
					//use default relationship importance = 0
					//use default relationship cost = 0
					//use default Multiedge factor = 0
					//no normalization
					var kg_config = new CIMKnowledgeGraphCentralityConfiguration();

					//include all entities from the kg in the subgraph
					//(no filters)
					var kg_subgraph = new CIMKnowledgeGraphSubGraph();

					//include all centrality measures
					CentralityMeasure[] measures = [
						CentralityMeasure.Degree,
			CentralityMeasure.InDegree,
			CentralityMeasure.OutDegree,
			CentralityMeasure.Coreness,//Coreness only wks w/ undirected relates
      CentralityMeasure.Betweenness,
			CentralityMeasure.Closeness,
			CentralityMeasure.Harmonic,
			CentralityMeasure.Eigenvector,
			CentralityMeasure.PageRank
					];

					//compute centrality
					var kg_centrality_results = kg.ComputeCentrality(
																						kg_config, kg_subgraph, measures);
					//TODO - process results
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.Scores
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.Measures
			//cref: ArcGIS.Core.Data.Knowledge.Extensions.KnowledgeGraphExtensions.ComputeCentrality(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration,ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph,System.Collections.Generic.IEnumerable{ArcGIS.Core.CIM.CentralityMeasure})
			#region Process Centrality Results
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ....) {
					//use defaults...
					var kg_config = new CIMKnowledgeGraphCentralityConfiguration();

					//include all entities from the kg in the subgraph
					//(no filters)
					var kg_subgraph = new CIMKnowledgeGraphSubGraph();

					//include all centrality measures
					CentralityMeasure[] measures = [
						CentralityMeasure.Degree,
			CentralityMeasure.InDegree,
			CentralityMeasure.OutDegree,
			CentralityMeasure.Coreness,
			CentralityMeasure.Betweenness,
			CentralityMeasure.Closeness,
			CentralityMeasure.Harmonic,
			CentralityMeasure.Eigenvector,
			CentralityMeasure.PageRank
					];

					//compute centrality
					var kg_centrality_results = kg.ComputeCentrality(
																						kg_config, kg_subgraph, measures);
					//output results - results include measure scores for all entities
					//in all types in the subgraph
					System.Diagnostics.Debug.WriteLine("Centrality Results:");
					foreach (var named_type in kg_centrality_results.NamedTypes)
					{
						System.Diagnostics.Debug.WriteLine($"Named type: {named_type}");
						foreach (var uid in kg_centrality_results.GetUidsForNamedType(named_type))
						{
							//measure scores include one score per measure in the input measures array
							var scores = kg_centrality_results.Scores[uid];
							StringBuilder sb = new StringBuilder();
							var sep = "";
							//or use kg_centrality_results.Scores.Measures.Length
							//kg_centrality_results.Scores.Measures is there for convenience
							for (int m = 0; m < measures.Length; m++)
							{
								sb.Append($"{sep}{measures[m].ToString()}: {scores[m]}");
								sep = ", ";
							}
							System.Diagnostics.Debug.WriteLine($"  '{uid}' {sb.ToString()}");
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration.RelationshipsInterpretation
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration.MultiedgeFactor
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration.Normalization
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration.DefaultRelationshipCost
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration.DefaultRelationshipImportance
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration.RelationshipCostProperty
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration.RelationshipImportanceProperty
			//cref: ArcGIS.Core.CIM.CentralityRelationshipInterpretation
			//cref: ArcGIS.Core.CIM.CentralityScoresNormalization
			#region Configure Centrality
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ....) {
					//configure centrality with custom settings
					var kg_config = new CIMKnowledgeGraphCentralityConfiguration()
					{
						RelationshipsInterpretation = CentralityRelationshipInterpretation.Directed,
						MultiedgeFactor = 1.0,//cumulative importance
						Normalization = CentralityScoresNormalization.None,
						DefaultRelationshipCost = 1.0,
						DefaultRelationshipImportance = 1.0,
						RelationshipCostProperty = string.Empty,
						RelationshipImportanceProperty = string.Empty
					};

					//include all entities from the kg in the subgraph
					//(no filters)
					var kg_subgraph = new CIMKnowledgeGraphSubGraph();

					//include the relevant centrality measures
					CentralityMeasure[] measures = [
						CentralityMeasure.Degree,
			CentralityMeasure.InDegree,
			CentralityMeasure.OutDegree,
			CentralityMeasure.Betweenness,
			CentralityMeasure.Closeness,
			CentralityMeasure.Harmonic,
			CentralityMeasure.Eigenvector,
			CentralityMeasure.PageRank
					];
					//Note: CentralityMeasure.Coreness cannot be calculated
					//with directed relationships, so it is not included here.
					//Specfying Coreness (with directed or reveresed relationships) will
					//throw an exception

					//compute centrality
					var kg_centrality_results = kg.ComputeCentrality(
																						kg_config, kg_subgraph, measures);
					//TODO process results
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph.EntityFilters
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilter
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByInstances
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByType
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilter.FilterType
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilter.NamedType
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByInstances.InstancesIDs
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByType.PropertyFilterPredicate
			//cref: ArcGIS.Core.CIM.KGFilterType.Include
			//cref: ArcGIS.Core.CIM.KGFilterType.Exclude
			#region Configure Centrality Entity SubGraph Filters
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ....) {
					//Assume KG contains Entity NamedTypes "A","B","C", and "D"
					//
					//We can use either of a CIMKnowledgeGraphNamedTypeFilterByInstances
					//or CIMKnowledgeGraphNamedTypeFilterByType to filter instances for
					//_including_ in or _excluding_ from entities and relates wrt the subgraph.
					//
					//Use CIMKnowledgeGraphNamedTypeFilterByInstances to filter by instance id
					//Use CIMKnowledgeGraphNamedTypeFilterByType to filter by type name and with
					//an optional property filter predicate to filter by property value(s)

					//Example 1.
					//Include all entities (and relates) from the kg in the subgraph
					//Leave kg_subgraph.EntityFilters null.
					//(kg_subgraph.RelationshipFilters is also null)

					//The collection of Entity and Relate filters are both empty
					//Everything is included
					var kg_subgraph = new CIMKnowledgeGraphSubGraph();

					//Example 2
					//Include a set of Entities from A (all of B, C, D will be implicitly excluded)
					var kg_filter2 = new CIMKnowledgeGraphNamedTypeFilterByInstances()
					{
						FilterType = KGFilterType.Include,
						NamedType = "A",
						InstancesIDs = list_of_ids.ToArray() //list_of_ids contains desired uids for "A"
																								 //note: -if list_of_ids is empty then -no-
																								 //instances of A will be included.
					};

					var entity_filters2 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters2.Add(kg_filter2);

					//note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters2.ToArray();

					//Example 3
					//Exclude a set of Entities from A (all of B, C, D will be implicitly included)
					var kg_filter3 = new CIMKnowledgeGraphNamedTypeFilterByInstances()
					{
						FilterType = KGFilterType.Exclude,
						NamedType = "A",
						InstancesIDs = list_of_ids.ToArray() //list_of_ids contains desired uids for "A"
																								 //note: -if list_of_ids is empty then -no-
																								 //instances of A will be excluded.
					};

					var entity_filters3 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters3.Add(kg_filter3);

					//note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters3.ToArray();

					//Example 4
					//Include all Entities from A via named type (all of B, C, D
					//will be implicitly excluded)

					var kg_filter4 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Include,
						NamedType = "A" //all of A will be included
														//predicate is empty
					};

					var entity_filters4 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters4.Add(kg_filter4);

					//note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters4.ToArray();

					//Example 5
					//Exclude all Entities from A via named type (all of B, C, D
					//will be implicitly included)

					var kg_filter5 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Exclude,
						NamedType = "A" //all of A will be excluded
														//predicate is empty
					};

					var entity_filters5 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters5.Add(kg_filter5);

					//note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters5.ToArray();

					//Example 6
					//Include all entities from B and C and
					//a subset from D via a set of ids (A is implicitly excluded)
					var kg_filter6_b = new CIMKnowledgeGraphNamedTypeFilterByType();
					var kg_filter6_c = new CIMKnowledgeGraphNamedTypeFilterByType();

					kg_filter6_b.NamedType = "B";//Include is default, predicate is empty
					kg_filter6_c.NamedType = "C";//Include is default, predicate is empty

					//TODO... populate list with uids from "D"...
					var kg_filter6_d = new CIMKnowledgeGraphNamedTypeFilterByInstances();
					kg_filter6_d.NamedType = "D";//Include is default
					kg_filter6_d.InstancesIDs = list_of_ids.ToArray();

					var entity_filters6 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters6.Add(kg_filter6_b);//order doesn't matter
					entity_filters6.Add(kg_filter6_c);
					entity_filters6.Add(kg_filter6_d);

					//note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters6.ToArray();

					//Example 7
					//Exclude all entities from B and C and
					//a subset from D (A is implicitly included)
					var kg_filter7_b = new CIMKnowledgeGraphNamedTypeFilterByType();
					var kg_filter7_c = new CIMKnowledgeGraphNamedTypeFilterByType();

					kg_filter7_b.NamedType = "B";//predicate is empty
					kg_filter7_c.NamedType = "C";//predicate is empty
					kg_filter7_b.FilterType = KGFilterType.Exclude;
					kg_filter7_c.FilterType = KGFilterType.Exclude;

					var kg_filter7_d = new CIMKnowledgeGraphNamedTypeFilterByInstances();
					kg_filter7_d.NamedType = "D";
					kg_filter7_d.FilterType = KGFilterType.Exclude;
					kg_filter7_d.InstancesIDs = list_of_ids.ToArray();//ids to be excluded

					var entity_filters7 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters7.Add(kg_filter7_b);//order doesn't matter
					entity_filters7.Add(kg_filter7_c);
					entity_filters7.Add(kg_filter7_d);

					//note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters7.ToArray();

					//Example 8
					//Include a subset of Entities from A using a property
					//filter predicate (all of B, C, D will be implicitly excluded)

					//we -must- use a predicate prefix in our expression
					//select instances of A w/ Material = 'Steel'
					var expr8 = $"a.Material = 'Steel'";//prefix can be anything - foo, bar, fred, etc.

					var kg_filter8 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "A",//Include is default
						PropertyFilterPredicate = expr8 //include ids w/Material='Steel'
					};

					var entity_filters8 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters8.Add(kg_filter8);

					//note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters8.ToArray();

					//Example 9
					//Exclude a subset of Entities from A using a property
					//filter predicate (all of B, C, D will be implicitly included)

					//we -must- use a predicate prefix in our expression
					//select instances of A with Material <> 'Steel'
					var expr9 = $"a.Material <> 'Steel'";//prefix can be anything - foo, bar, fred, etc.

					var kg_filter9 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Exclude,
						NamedType = "A",
						PropertyFilterPredicate = expr9 //exclude ids w/Material <> 'Steel'
					};

					var entity_filters9 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters9.Add(kg_filter9);

					//Note: All relates are included as no RelationshipFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.EntityFilters = entity_filters9.ToArray();
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph.RelationshipFilters
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilter
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByInstances
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByType
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilter.FilterType
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilter.NamedType
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByInstances.InstancesIDs
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphNamedTypeFilterByType.PropertyFilterPredicate
			//cref: ArcGIS.Core.CIM.KGFilterType.Include
			//cref: ArcGIS.Core.CIM.KGFilterType.Exclude
			#region Configure Centrality Relationship SubGraph Filters
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ...) {
					//Assume Relationship NamedTypes "R1","R2",and "R3"
					//
					//We can use either of a CIMKnowledgeGraphNamedTypeFilterByInstances
					//or CIMKnowledgeGraphNamedTypeFilterByType to filter instances for
					//_including_ in or _excluding_ from entities and relates wrt the subgraph.
					//
					//Use CIMKnowledgeGraphNamedTypeFilterByInstances to filter by instance id
					//Use CIMKnowledgeGraphNamedTypeFilterByType to filter by type name and with
					//an optional property filter predicate to filter by property value(s)

					//Example 1.
					//Include all relates (and entities) from the kg in the subgraph
					//Leave kg_subgraph.RelationshipFilters null
					//(kg_subgraph.EntityFilters is also null)

					//The collection of Entity and Relate filters are both empty
					//Everything is included
					var kg_subgraph = new CIMKnowledgeGraphSubGraph();
					//note - relates can only be included if their corresponding entity end points
					//are also included in the subgraph.

					//Example 2
					//Include a set of Relates from R1 (all of R2, R3 will be implicitly excluded)
					var kg_filter_r2 = new CIMKnowledgeGraphNamedTypeFilterByInstances()
					{
						FilterType = KGFilterType.Include,
						NamedType = "R1",
						InstancesIDs = list_of_ids.ToArray() //list_of_ids contains desired uids for "R1"
																								 //note: -if list_of_ids is empty then -no-
																								 //instances of R1 will be included.
					};

					//var kg_subgraph = ...
					var relate_filters2 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters2.Add(kg_filter_r2);

					//note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters2.ToArray();

					//Example 3
					//Exclude a set of Relates from R1 (all of R2, R3 will be implicitly included)
					var kg_filter_r3 = new CIMKnowledgeGraphNamedTypeFilterByInstances()
					{
						FilterType = KGFilterType.Exclude,
						NamedType = "R1",
						InstancesIDs = list_of_ids.ToArray() //list_of_ids contains desired uids for "R1"
																								 //note: -if list_of_ids is empty then -no-
																								 //instances of R1 will be excluded.
					};

					var relate_filters3 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters3.Add(kg_filter_r3);
					//note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters3.ToArray();

					//Example 4
					//Include all Relates from R1 via named type (all of R2, R3
					//will be implicitly excluded)
					var kg_filter_r4 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Include,
						NamedType = "R1",//all of R1 will be included
														 //predicate is empty
					};

					var relate_filters4 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters4.Add(kg_filter_r4);

					//note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters4.ToArray();

					//Example 5
					//Exclude all Relates from R1 via named type (all of R2, R3
					//will be implicitly included)

					var kg_filter_r5 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Exclude,
						NamedType = "R1" //all of R1 will be excluded
					};

					var relate_filters5 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters5.Add(kg_filter_r5);

					//note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters5.ToArray();

					//Example 6
					//Include all relates from R2 and
					//a subset from R3 (R1 is implicitly excluded)
					var kg_filter_r6_r2 = new CIMKnowledgeGraphNamedTypeFilterByType();
					kg_filter_r6_r2.NamedType = "R2"; //Include is default, predicate is empty

					//TODO... populate list with uids from "R3"...
					var kg_filter_r6_r3 = new CIMKnowledgeGraphNamedTypeFilterByInstances();
					kg_filter_r6_r3.NamedType = "R3"; //Include is default
					kg_filter_r6_r3.InstancesIDs = list_of_ids.ToArray();

					var relate_filters6 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters6.Add(kg_filter_r6_r2);//order doesn't matter
					relate_filters6.Add(kg_filter_r6_r3);

					//note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters6.ToArray();

					//Example 7
					//Exclude all relates from R2 and
					//a subset from R3 (R1 is implicitly included)
					var kg_filter_r7_r2 = new CIMKnowledgeGraphNamedTypeFilterByType();
					kg_filter_r7_r2.NamedType = "R2";
					kg_filter_r7_r2.FilterType = KGFilterType.Exclude;

					//TODO... populate list with uids from "R3"...
					var kg_filter_r7_r3 = new CIMKnowledgeGraphNamedTypeFilterByInstances();
					kg_filter_r7_r3.NamedType = "R3";
					kg_filter_r7_r3.FilterType = KGFilterType.Exclude;
					kg_filter_r7_r3.InstancesIDs = list_of_ids.ToArray();

					var relate_filters7 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters7.Add(kg_filter_r7_r2);//order doesn't matter
					relate_filters7.Add(kg_filter_r7_r3);

					//note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters6.ToArray();

					//Example 8
					//Include a subset of Relates from R1 using a property
					//filter predicate (all of R2, R3 will be implicitly excluded)

					//we -must- use a predicate prefix in our expression
					//select instances of R1 w/Distance = 2000
					var expr8 = $"r1.Distance = 2000";//prefix can be anything - foo, bar, fred, etc.

					var kg_filter_r8 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "R1", //Include is default
						PropertyFilterPredicate = expr8
					};

					var relate_filters8 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters8.Add(kg_filter_r8);

					//note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters8.ToArray();

					//Example 9
					//Exclude a subset of Relates from R1 using a property
					//filter predicate (all of R2, R3 will be implicitly included)

					//we -must- use a predicate prefix in our expression
					//select instances of R1 w/ Distance <> 2000
					var expr9 = $"r1.Distance <> 2000";//prefix can be anything - foo, bar, fred, etc.

					var kg_filter_r9 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Exclude,
						NamedType = "R1",
						PropertyFilterPredicate = expr9 //exclude ids w/ Distance <> 2000
					};

					var relate_filters9 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters9.Add(kg_filter_r9);

					//Note: All entities are included as no EntityFilters are specified
					//var kg_subgraph = ...
					kg_subgraph.RelationshipFilters = relate_filters9.ToArray();
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph.EntityFilters
			//cref: ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph.RelationshipFilters
			#region Combine Centrality Entity and Relationship SubGraph Filters
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ...) {
					//Assume Entity NamedTypes "A","B","C", and "D"
					//Assume Relationship NamedTypes "R1","R2",and "R3"

					//Example 1
					//Include all Entities from A and B
					//Include all Relates from R1 and R2
					//(all of C, D, and R3 will be implicitly excluded)
					var kg_filter1_a = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Include,//default
						NamedType = "A"
					};
					var kg_filter1_b = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Include,//default
						NamedType = "B"
					};
					var kg_filter1_r1 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Include,//default
						NamedType = "R1"
					};
					var kg_filter1_r2 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						FilterType = KGFilterType.Include,//default
						NamedType = "R2"
					};

					var entity_filters1 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters1.Add(kg_filter1_a);
					entity_filters1.Add(kg_filter1_b);

					var relate_filters1 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters1.Add(kg_filter1_r1);
					relate_filters1.Add(kg_filter1_r2);

					//var kgSubgraph = ...
					kgSubgraph.EntityFilters = entity_filters1.ToArray();
					kgSubgraph.RelationshipFilters = relate_filters1.ToArray();

					//Example 2 - 
					//Include a subset of Entities from A using a predicate,
					//and all of B, C, D and all of R1 and R3 (but not R2)

					var prefix2 = "a";//prefix can be anything - foo, bar, fred, etc.
					var expr2 = $"{prefix2}.Material = 'Steel' AND " +
													$"{prefix2}.Role = 'Tier1'";

					var kg_filter2_a = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "A", //Include is default
						PropertyFilterPredicate = expr2
					};
					var kg_filter2_b = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "B" //Include is default
					};
					var kg_filter2_c = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "C" //Include is default
					};
					var kg_filter2_d = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "D" //Include is default
					};

					var kg_filter2_r1 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "R1" //Include is default
					};
					var kg_filter2_r3 = new CIMKnowledgeGraphNamedTypeFilterByType()
					{
						NamedType = "R3" //Include is default
					};

					var entity_filters2 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters2.Add(kg_filter2_a);
					entity_filters2.Add(kg_filter2_b);
					entity_filters2.Add(kg_filter2_c);
					entity_filters2.Add(kg_filter2_d);

					var relate_filters2 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters2.Add(kg_filter2_r1);
					relate_filters2.Add(kg_filter2_r3);

					//var kgSubgraph = ...
					kgSubgraph.EntityFilters = entity_filters2.ToArray();
					kgSubgraph.RelationshipFilters = relate_filters2.ToArray();

					//Example 3
					//Exclude all entities from B and C and
					//a subset from D (A is implicitly included)
					//
					//Include all relates from R2 and
					//a subset from R3 (R1 is implicitly excluded)

					var kg_filter3_b = new CIMKnowledgeGraphNamedTypeFilterByType();
					var kg_filter3_c = new CIMKnowledgeGraphNamedTypeFilterByType();

					kg_filter3_b.NamedType = "B";
					kg_filter3_c.NamedType = "C";
					kg_filter3_b.FilterType = KGFilterType.Exclude;
					kg_filter3_c.FilterType = KGFilterType.Exclude;

					//TODO... populate list with uids from "D"...
					var kg_filter3_d = new CIMKnowledgeGraphNamedTypeFilterByInstances();
					kg_filter3_d.NamedType = "D";
					kg_filter3_d.FilterType = KGFilterType.Exclude;
					kg_filter3_d.InstancesIDs = list_of_ids.ToArray();

					var kg_filter3_r2 = new CIMKnowledgeGraphNamedTypeFilterByType();
					kg_filter3_r2.NamedType = "R2";//Include is default

					//TODO... populate list with uids from "R3"...
					var kg_filter3_r3 = new CIMKnowledgeGraphNamedTypeFilterByInstances();
					kg_filter3_r3.NamedType = "R3";//Include is default
					kg_filter3_r3.InstancesIDs = list_of_ids2.ToArray();

					var entity_filters3 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					entity_filters3.Add(kg_filter3_b);//order doesn't matter
					entity_filters3.Add(kg_filter3_c);
					entity_filters3.Add(kg_filter3_d);

					var relate_filters3 = new List<CIMKnowledgeGraphNamedTypeFilter>();
					relate_filters3.Add(kg_filter3_r2);//order doesn't matter
					relate_filters3.Add(kg_filter3_r3);

					//var kgSubgraph = ...
					kgSubgraph.EntityFilters = entity_filters3.ToArray();
					kgSubgraph.RelationshipFilters = relate_filters3.ToArray();

				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Extensions.KnowledgeGraphExtensions.ComputeCentrality(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.CIM.CIMKnowledgeGraphCentralityConfiguration,ArcGIS.Core.CIM.CIMKnowledgeGraphSubGraph,System.Collections.Generic.IEnumerable{ArcGIS.Core.CIM.CentralityMeasure})
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.NamedTypes
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.Scores
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.GetUidsForNamedType(System.String)
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.Measures
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.Item(System.Object)
			#region Output Centrality Results
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					///var kgConfig = ...;
					//var kgSubgraph = ...;
					//var measures = ...;
					//using(var kg = ...) {
					var results = kg.ComputeCentrality(kgConfig, kgSubgraph, measures);
					//loop through each (entity) named type (relates are never included in results)
					foreach (var named_type in results.NamedTypes)
					{
						//Get the entity uids for each named type
						foreach (var uid in results.GetUidsForNamedType(named_type))
						{
							//Get the scores for each uid via the [] indexer on "Scores"
							var scores = results.Scores[uid];
							//There is one score per measure in the input measures array
							//Note: results.Scores.Measures is also provided for convenience...
							//for (int m = 0; m < results.Scores.Measures.Length; m++)
							for (int m = 0; m < measures.Length; m++)
							{
								var score = scores[m];//score for the given measure
																				//TODO - use measure score

							}
						}
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.RawUids
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.Measures
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.RawScores
			#region Get Max and Min Centrality Result Scores No Sort or LINQ
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ...) {
					//var kgResults = kg.ComputeCentrality(...);
					var count_entities = kgResults.RawUids.Count();
					var score_len = kgResults.Scores.RawScores.Length;
					//Find the max and min score for each measure w/out
					//using LINQ or sorting
					for (int m = 0; m < kgResults.Scores.Measures.Length; m++)
					{
						//index into the scores array for the current measure
						var start_idx = count_entities * m;
						var end_idx = start_idx + count_entities;

						double max_score = double.MinValue;
						double min_score = double.MaxValue;
						List<object> max_uids = new List<object>();
						List<object> min_uids = new List<object>();

						for (int i = 0, s = start_idx; s < end_idx; i++, s++)
						{
							var score = kgResults.Scores.RawScores[s];
							//max
							if (score > max_score)
							{
								max_score = score;
								max_uids.Clear();
								max_uids.Add(kgResults.RawUids[i]);
							}
							else if (score == max_score)
							{
								//Collect all uids with the max score
								max_uids.Add(kgResults.RawUids[i]);
							}
							//min
							if (score < min_score)
							{
								min_score = score;
								min_uids.Clear();
								min_uids.Add(kgResults.RawUids[i]);
							}
							else if (score == min_score)
							{
								//Collect all uids with the min score
								min_uids.Add(kgResults.RawUids[i]);
							}
						}
						//TODO - use the max and min scores and uids for
						//the current measure "measures[m]"
						//max_score, max_uids
						//min_score, min_uids

					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.RawUids
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.Measures
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.RawScores
			#region Get Max and Min Centrality Result Scores With Sort and Linq
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					//using(var kg = ...) {
					//var kgResults = kg.ComputeCentrality(...);

					var count_entities = kgResults.RawUids.Count();
					var score_len = kgResults.Scores.RawScores.Length;
					//Find the max and min score for each measure using LINQ
					for (int m = 0; m < kgResults.Scores.Measures.Length; m++)
					{
						//index into the scores array for the current measure
						var start_idx = count_entities * m;
						var end_idx = start_idx + count_entities;

						//Get the scores for the specified measure
						var scores = Enumerable.Range(start_idx, count_entities)
							.Select(i => kgResults.Scores.RawScores[i]).ToArray();

						//max + min
						var max_score = scores.Max();
						var min_score = scores.Min();

						//uids with the max score
						var max_uids = Enumerable.Range(0, count_entities)
						.Where(i => scores[i] == max_score)
														 .Select(i => kgResults.RawUids[i])
														 .ToList();

						//uids with the min score
						var min_uids = Enumerable.Range(0, count_entities)
														 .Where(i => scores[i] == min_score)
														 .Select(i => kgResults.RawUids[i])
														 .ToList();
						//TODO - use the max and min scores and uids for
						//the current measure "measures[m]"
						//max_score, max_uids
						//min_score, min_uids
					}
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.RawUids
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.Measures
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityScores.RawScores
			#region Get Top or Bottom N Centrality Result Scores
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				//Note: logic uses the following custom class:
				//
				//public class KG_Score_Value {
				//   public KG_Score_Value(double score, int uid_idx) {
				//      Score = score;
				//      Uid_idx = uid_idx;
				//   }
				//
				//   public double Score { get; set; }
				//   public int Uid_idx { get; set; }
				//}

				//var kgResults = kg.ComputeCentrality(...);

				var count_entities = kgResults.RawUids.Count();
				var score_len = kgResults.Scores.RawScores.Length;
				int n = 100;

				//Find the top and bottom "n" scores for each measure
				for (int m = 0; m < kgResults.Scores.Measures.Length; m++)
				{
					//index into the scores array for the current measure
					var start_idx = count_entities * m;
					var end_idx = start_idx + count_entities;

					//Get the scores for the specified measure
					var scores = Enumerable.Range(start_idx, count_entities)
						.Select(i => kgResults.Scores.RawScores[i]).ToArray();

					//use "KG_Score_Value" to hold the score and uid index
					var uid_idx = 0;
					var kg_score_vals = Enumerable.Range(start_idx, count_entities)
								 .Select(i => new KG_Score_Value(kgResults.Scores.RawScores[i], uid_idx++))
								 .OrderByDescending(k => k.Score)
								 .ToArray();

					//get up to the top or bottom N scores
					if (n > kg_score_vals.Length)
						n = kg_score_vals.Length;

					//get the top N scores and uids - largest first
					var top_n = kg_score_vals.Take(n).ToList();

					//get the bottom N scores and uids - smallest first
					var bottom_n = kg_score_vals.TakeLast(n)
														.Reverse().ToList();

					foreach (var kg_score in top_n)
					{
						//TODO - use top "n" scores
						var score = kg_score.Score;
						var uid1 = kgResults.RawUids[kg_score.Uid_idx];
						//...use it
					}

					foreach (var kg_score in bottom_n)
					{
						//TODO - use bottom "n" scores
						var score = kg_score.Score;
						var uid1 = kgResults.RawUids[kg_score.Uid_idx];
						//...use it
					}
				}
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphCentralityResults.GetUidsForNamedType(System.String)
			#region Get The NamedType for a Given UID in Result Scores
			{
				//object uid = "{.... uid value ....}";
				//or
				//object uid = kgResults.RawUids[i]; e.g. in/from a loop
				var named_type = "";
				foreach (var nt in kgResults.NamedTypes)
				{
					var uids = kgResults.GetUidsForNamedType(nt);
					if (uids.Contains(uid))
					{
						named_type = nt;//found it
						break;
					}
				}
				if (string.IsNullOrEmpty(named_type))
				{
					//uid not found in any named type
					//TODO - handle this case
				}
				//TODO - use the named type for the uid
			}
			#endregion

			#region ProSnippet Group: KnowledgeGraph FFP
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Extensions.KnowledgeGraphExtensions.RunFilteredFindPaths(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration)
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.OriginEntities
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.DestinationEntities
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsEntity
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.DefaultTraversalDirectionType
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.EntityUsage
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.PathMode
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.ClosedPathPolicy
			#region Run FFP Using Defaults
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Run FFP Using Defaults";

					//Origin Entities
					var originEntities = new List<CIMFilteredFindPathsEntity>();

					var poi_entity = new CIMFilteredFindPathsEntity();
					poi_entity.EntityTypeName = "POI";//All entities of entity type "POI"
					poi_entity.PropertyFilterPredicate = "";
					originEntities.Add(poi_entity);
					//Add the CIMFilteredFindPathsEntity to the OriginEntities collection
					ffp_config.OriginEntities = originEntities.ToArray();

					//Destination Entities
					var destEntities = new List<CIMFilteredFindPathsEntity>();

					var supp_entity = new CIMFilteredFindPathsEntity();
					supp_entity.EntityTypeName = "Supplier";//All entities of entity type "Supplier"
					supp_entity.PropertyFilterPredicate = "";
					destEntities.Add(supp_entity);

					//Add the CIMFilteredFindPathsEntity to the DestinationEntities collection
					ffp_config.DestinationEntities = destEntities.ToArray();

					//Path Filters
					ffp_config.PathFilters = [];//Empty
																			//Traversal
					ffp_config.TraversalDirections = [];//Empty

					//Other
					ffp_config.RelationshipCostProperty = "";
					ffp_config.DefaultRelationshipCost = 1.0;
					ffp_config.DefaultTraversalDirectionType = KGTraversalDirectionType.Any;
					ffp_config.EntityUsage = FilteredFindPathsEntityUsage.AnyOriginAnyDestination;
					ffp_config.PathMode = KGPathMode.Shortest;
					ffp_config.MinPathLength = (int)1;//Min number of relationships in path
					ffp_config.MaxPathLength = (int)8;//Max number of relationships in path
					ffp_config.MaxCountPaths = (int)100000;//Total number of paths to return
					ffp_config.ClosedPathPolicy = KGClosedPathPolicy.Forbid;
					ffp_config.TimeFilter = null;

					var results = kg.RunFilteredFindPaths(ffp_config);
					//TODO process/analyze results
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsEntity
			#region Run FFP Using Multiple Entities and Destinations
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Run FFP w Multiple Entities and Destinations";

					//Origin Entities
					var originEntities = new List<CIMFilteredFindPathsEntity>();

					foreach (var entity_name in new List<string> { "Person", "POI", "Supplier", "Plant" })
					{
						var origin_entity = new CIMFilteredFindPathsEntity();
						origin_entity.EntityTypeName = entity_name;
						origin_entity.PropertyFilterPredicate = "";
						originEntities.Add(origin_entity);
					}

					//Add the CIMFilteredFindPathsEntity to the OriginEntities collection
					ffp_config.OriginEntities = originEntities.ToArray();

					//Destination Entities
					var destEntities = new List<CIMFilteredFindPathsEntity>();

					foreach (var entity_name in new List<string> {
					"Supplier", "Plant", "Part", "Customer" })
					{
						var dest_entity = new CIMFilteredFindPathsEntity();
						dest_entity.EntityTypeName = entity_name;
						dest_entity.PropertyFilterPredicate = "";
						destEntities.Add(dest_entity);
					}

					//Add the CIMFilteredFindPathsEntity to the DestinationEntities collection
					ffp_config.DestinationEntities = destEntities.ToArray();

					//Path Filters
					ffp_config.PathFilters = [];//Empty
																			//Traversal
					ffp_config.TraversalDirections = [];//Empty

					//Other
					ffp_config.RelationshipCostProperty = "";
					ffp_config.DefaultRelationshipCost = 1.0;
					ffp_config.DefaultTraversalDirectionType = KGTraversalDirectionType.Any;
					ffp_config.EntityUsage = FilteredFindPathsEntityUsage.AnyOriginAnyDestination;
					ffp_config.PathMode = KGPathMode.Shortest;
					ffp_config.MinPathLength = (int)1;//Min number of relationships in path
					ffp_config.MaxPathLength = (int)8;//Max number of relationships in path
					ffp_config.MaxCountPaths = (int)100000;//Total number of paths to return
					ffp_config.ClosedPathPolicy = KGClosedPathPolicy.Forbid;
					ffp_config.TimeFilter = null;

					var results = kg.RunFilteredFindPaths(ffp_config);
					//TODO process/analyze results
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsEntity.ID
			#region Run FFP Using Specific Entities and Destinations by ID
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Run FFP w Specific Entities and Destinations";

					//Origin Entities
					var originEntities = new List<CIMFilteredFindPathsEntity>();

					var origin_entity = new CIMFilteredFindPathsEntity();
					origin_entity.EntityTypeName = "POI";
					origin_entity.ID = "{EC2A2D91-B09C-4CF6-93A3-51D6527CF51E}";//upper case guid
					origin_entity.PropertyFilterPredicate = "";//Ignored
					originEntities.Add(origin_entity);

					var origin_entity2 = new CIMFilteredFindPathsEntity();
					origin_entity2.EntityTypeName = "POI";
					origin_entity2.ID = "{5008792F-3C67-4FCA-B1E9-756D6E389FDD}";//upper case guid
					origin_entity2.PropertyFilterPredicate = "";//Ignored
					originEntities.Add(origin_entity2);

					//etc.

					//Add the CIMFilteredFindPathsEntity to the OriginEntities collection
					ffp_config.OriginEntities = originEntities.ToArray();

					//Destination Entities
					//Same thing, add specific entities using their Uids
					var destEntities = new List<CIMFilteredFindPathsEntity>();

					var dest_entity = new CIMFilteredFindPathsEntity();
					dest_entity.EntityTypeName = "Supplier";
					dest_entity.ID = "{A3F5C2E1-8D3B-4E2A-9F4B-1C2D3E4F5A6B}";//upper case guid
					dest_entity.PropertyFilterPredicate = "";
					destEntities.Add(dest_entity);

					var dest_entity2 = new CIMFilteredFindPathsEntity();
					dest_entity2.EntityTypeName = "Supplier";
					dest_entity2.ID = "{B1C2D3E4-F5A6-7B8C-9D0E-1F2A3B4C5D6E}";//upper case guid
					dest_entity2.PropertyFilterPredicate = "";
					destEntities.Add(dest_entity2);

					//etc.
					//Add the CIMFilteredFindPathsEntity to the OriginEntities collection
					ffp_config.DestinationEntities = destEntities.ToArray();
					//TODO - use the config
					//var results = kg.RunFilteredFindPaths(ffp_config);
					// ...
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsEntity.PropertyFilterPredicate
			#region Run FFP Using PropertyFilterPredicates
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Run FFP w PropertyFilterPredicates";

					//Origin Entities
					var originEntities = new List<CIMFilteredFindPathsEntity>();

					var origin_entity = new CIMFilteredFindPathsEntity();
					origin_entity.EntityTypeName = "POI";
					//prefix can be anything - foo, bar, fred, n, x, etc.
					origin_entity.PropertyFilterPredicate = "n.name = 'Robert Johnston'";
					originEntities.Add(origin_entity);

					var origin_entity2 = new CIMFilteredFindPathsEntity();
					origin_entity2.EntityTypeName = "EnergySource";
					//prefix can be anything - foo, bar, fred, n, x, s, etc.
					origin_entity2.PropertyFilterPredicate = "s.Source_Name = 'natural gas'";
					originEntities.Add(origin_entity2);

					//etc.

					//Add the CIMFilteredFindPathsEntity to the OriginEntities collection
					ffp_config.OriginEntities = originEntities.ToArray();

					//Destination Entities
					//Same thing, add specific entities using a PropertyFilterPredicate as needed
					var destEntities = new List<CIMFilteredFindPathsEntity>();

					var dest_entity = new CIMFilteredFindPathsEntity();
					dest_entity.EntityTypeName = "Supplier";
					//prefix can be anything - foo, bar, fred, n, x, s, etc.
					origin_entity.PropertyFilterPredicate = "x.Supplier_Name = 'Supplier 84'";
					destEntities.Add(dest_entity);

					//etc.
					//Add the CIMFilteredFindPathsEntity to the OriginEntities collection
					ffp_config.DestinationEntities = destEntities.ToArray();
					//TODO - use the config
					//var results = kg.RunFilteredFindPaths(ffp_config);
					// ...
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.PathFilters
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsPathFilter
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsPathFilter.ItemTypeName
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsPathFilter.ItemType
			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsPathFilter.FilterType
			//cref: ArcGIS.Core.CIM.KGPathFilterItemType.Entity
			//cref: ArcGIS.Core.CIM.KGPathFilterType
			#region Run FFP Using Path Filters
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Run FFP w Path Filters";

					//set origin entities
					//set destination entities

					//set path filters
					var pathFilters = new List<CIMFilteredFindPathsPathFilter>();

					var path_filter = new CIMFilteredFindPathsPathFilter();
					path_filter.ItemTypeName = "POI";
					path_filter.ItemType = KGPathFilterItemType.Entity;
					//To select specific entities, set the ID property or provide a filter predicate
					//path_filter.ID = "{B1C2D3E4-F5A6-7B8C-9D0E-1F2A3B4C5D6E}";//set a specific id
					//...or...use a predicate
					//path_filter.PropertyFilterPredicate = "n.name = 'Robert Johnston'";

					//Use IncludeOnly to only consider entities of this type in the path
					path_filter.FilterType = KGPathFilterType.IncludeOnly;
					//Use Exclude to ignore paths that contain excluded types
					path_filter.FilterType = KGPathFilterType.Exclude;
					//Use MandatoryWaypoint if you have specific entities/entity types through
					//which all paths must pass through at least one entity of this type
					path_filter.FilterType = KGPathFilterType.MandatoryWaypoint;
					//Use OptionalWaypoint if you have specific entities/entity types through
					//which all paths must pass through at least one entity from any of the
					//optional waypoints
					path_filter.FilterType = KGPathFilterType.OptionalWaypoint;
					//Add the path filter(s) to the collection
					pathFilters.Add(path_filter);

					//Add more filters as needed
					//"Person", "Supplier", "Plant", etc.

					//Path Filters
					ffp_config.PathFilters = pathFilters.ToArray();
					//TODO - use the config
					//var results = kg.RunFilteredFindPaths(ffp_config);
					// ...
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration.TraversalDirections
			//cref: ArcGIS.Core.CIM.CIMKGTraversalDirection
			//cref: ArcGIS.Core.CIM.CIMKGTraversalDirection.RelationshipTypeName
			//cref: ArcGIS.Core.CIM.CIMKGTraversalDirection.TraversalDirectionType
			//cref: ArcGIS.Core.CIM.KGTraversalDirectionType
			#region Run FFP Using Traversal Filters
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(() =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Run FFP w Traversal Filters";

					//set origin entities
					//set destination entities

					//set any path filters

					//Set traversal filters
					//A traversal filter is used to specify the traversal direction of a relationship (to be)
					//included in the result paths. By default, paths are evaluated without considering the
					//direction of relationships in the graph - i.e. KGTraversalDirectionType.Any.
					//Relationships can be evaluated in specific directions by changing the KGTraversalDirectionType
					//enum value of the traversal_filter.TraversalDirectionType.

					//For example, the traversal direction is being set for the following relationship types...
					var traversalFilters = new List<CIMKGTraversalDirection>();
					foreach (var relation in new List<string> { "near_poi", "near_facility", "near_facility2" })
					{
						var traversal_filter = new CIMKGTraversalDirection();
						traversal_filter.RelationshipTypeName = relation;

						//All relationships of the specified type are considered (default)
						traversal_filter.TraversalDirectionType = KGTraversalDirectionType.Any;
						//Relationship can only be traversed from origin to destination
						traversal_filter.TraversalDirectionType = KGTraversalDirectionType.Forward;
						//Relationship can only be traversed from destination to origin
						traversal_filter.TraversalDirectionType = KGTraversalDirectionType.Backward;

						traversalFilters.Add(traversal_filter);
					}

					ffp_config.TraversalDirections = traversalFilters.ToArray();

					//etc.
					//TODO - use the config
					//var results = kg.RunFilteredFindPaths(ffp_config);
					// ...
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.ExtractPathsEntitiesAndRelationships(System.Collections.Generic.SortedSet{System.UInt64})
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.ToKnowledgeGraphIDSet(ArcGIS.Core.Data.Knowledge.Analytics.KGResultContentFromFFP)
			//cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SetRootNodes(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.MapMemberIDSet)
			#region Create Link Chart from FFP Results
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(async () =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Create Link Chart from FFP Results";
					//set up config
					//...

					var results = kg.RunFilteredFindPaths(ffp_config);

					var pathsEntitiesAndRelationships = results.ExtractPathsEntitiesAndRelationships(null);

					//Create a KG layer id set
					var kgLayerIdSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraphIDSet(
						pathsEntitiesAndRelationships.ToKnowledgeGraphIDSet(
							KGResultContentFromFFP.EntitiesAndRelationships));

					//Create a brand new link chart with the results and show it
					var linkChart = MapFactory.Instance.CreateLinkChart("KG Intro", kg, kgLayerIdSet);

					var mapPane = await FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
					var linkChartView = mapPane.MapView;

					//Change layout algo to match the default used by the UI after FFP
					await linkChartView.SetLinkChartLayoutAsync(
						KnowledgeLinkChartLayoutAlgorithm.Hierarchical_TopToBottom);

					//Set root nodes - they correspond to the origin nodes of the result paths
					var kgLayerIdSetForRootNodes = KnowledgeGraphLayerIDSet.FromKnowledgeGraphIDSet(
						pathsEntitiesAndRelationships.ToKnowledgeGraphIDSet(
							KGResultContentFromFFP.OnlyPathsOriginEntities));

					//To correctly identify the ids in the link chart we must change the ids
					//from Geodatabase oids returned in the KnowledgeGraphLayerIDSet to the
					//temporary/synthetic oids used by layers in the link chart...
					var kg_layer = linkChart.GetLayersAsFlattenedList().OfType<KnowledgeGraphLayer>().First();
					var mapMembers = kg_layer.GetMapMembersAsFlattenedList();
					var oidDict = kgLayerIdSetForRootNodes.ToOIDDictionary();
					var mmDict = new Dictionary<MapMember, List<long>>();
					foreach (var kvp in oidDict)
					{
						var named_type = kvp.Key;
						foreach (var mm in mapMembers)
						{
							if (mm is LinkChartFeatureLayer fl_lc && fl_lc.IsEntity)
							{
								if (fl_lc.GetTypeName().ToLower() == named_type.ToLower())
								{
									var lc_oids = new List<long>();
									//these oids are from the geodatabase
									var oid_field = $"{fl_lc.GetTypeName()}.objectid";
									var id_list = string.Join(',', kvp.Value.ToArray());
									var where = $"{fl_lc.GetTypeName()}.objectid IN ({id_list})";

									var qf = new ArcGIS.Core.Data.QueryFilter()
									{
										WhereClause = where,
										SubFields = $"LC.OID,{oid_field}"//the 'LC.OID' oids are the ones
																										 //we need for the mapmember id set
																										 //in the link chart
									};
									var rc = fl_lc.Search(qf);
									var oid_idx = rc.FindField(oid_field);
									while (rc.MoveNext())
									{
										var oid = (long)rc.Current[oid_idx];
										var lc_oid = rc.Current.GetObjectID();
										lc_oids.Add(lc_oid);
									}
									rc.Dispose();
									mmDict[fl_lc] = lc_oids;
									break;
								}
							}
						}
					}

					var mmIdSet = MapMemberIDSet.FromDictionary(mmDict);
					linkChartView.SetRootNodes(mmIdSet);
				});
			}
			#endregion

			//cref: ArcGIS.Core.CIM.CIMFilteredFindPathsConfiguration
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.ExtractPathsEntitiesAndRelationships(System.Collections.Generic.SortedSet{System.UInt64})
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.ToKnowledgeGraphIDSet(ArcGIS.Core.Data.Knowledge.Analytics.KGResultContentFromFFP)
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.CanAppendToLinkChart
			//cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.AppendToLinkChart
			#region Append to Link Chart from FFP Results
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				var linkChartView = MapView.Active;

				await QueuedTask.Run(async () =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "Append to Link Chart from FFP Results";
					//set up config
					//...

					var results = kg.RunFilteredFindPaths(ffp_config);

					var pathsEntitiesAndRelationships = results.ExtractPathsEntitiesAndRelationships(null);

					//Create a KG layer id set
					var kgLayerIdSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraphIDSet(
						pathsEntitiesAndRelationships.ToKnowledgeGraphIDSet(
							KGResultContentFromFFP.EntitiesAndRelationships));

					var map = linkChartView.Map;

					if (!map.CanAppendToLinkChart(kgLayerIdSet))
						return;//not compatible

					map.AppendToLinkChart(kgLayerIdSet);
					//switch layout algo
					var algo = linkChartView.GetLinkChartLayout();
					if (algo != KnowledgeLinkChartLayoutAlgorithm.Hierarchical_TopToBottom)
					{
						//Change layout algo to match the default used by the UI after FFP
						await linkChartView.SetLinkChartLayoutAsync(
							KnowledgeLinkChartLayoutAlgorithm.Hierarchical_TopToBottom);
					}

					//To set link chart root nodes see 'Create Link Chart from FFP Results'
				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.CountPaths
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.PathIndicesOrderedByIncreasingPathLength
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.PathIndicesOrderedByIncreasingMinPathCost
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.PathIndicesOrderedByIncreasingMaxPathCost
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.MaterializePath(System.Int64)
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathRelationshipGroup
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathRelationshipGroup.Relationships
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathRelationship
			#region List out FFP Results by Path Length, Min Cost, Max Cost
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(async () =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "List out FFP Results by Path Length, Min Cost, Max Cost";
					//set up config
					//...

					var results = kg.RunFilteredFindPaths(ffp_config);

					if (results.CountPaths == 0)
					{
						System.Diagnostics.Debug.WriteLine("FFP returned no paths");
						return;
					}

					//print out paths by increasing length, min cost, max cost
					var path_by_len_indices = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingPathLength
																			.Select(idx => idx.index);
					var path_by_min_cost = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingMinPathCost
																			.Select(idx => idx.index);
					var path_by_max_cost = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingMaxPathCost
																			.Select(idx => idx.index);

					var x = 0;
					StringBuilder sb = new StringBuilder();

					foreach (var path_indeces in new List<IEnumerable<long>> {
					path_by_len_indices,
					path_by_min_cost,
					path_by_max_cost})
					{
						if (x == 0)
							sb.AppendLine($"Paths by length: {path_by_len_indices.Count()}");
						else if (x == 1)
							sb.AppendLine($"Paths by min cost: {path_by_min_cost.Count()}");
						else if (x == 2)
							sb.AppendLine($"Paths by max cost: {path_by_max_cost.Count()}");
						x++;
						foreach (var path_idx in path_indeces)
						{
							var path = (ResultPath)results.MaterializePath(path_idx);
							sb.AppendLine(
								$"Path[{path_idx}] length: {path.Length}, min: {path.MinCost} max: {path.MaxCost}");
							var g = 0;
							foreach (var rel_group in path.RelationshipGroups)
							{
								var first_entity = $"({rel_group.FirstEntity.TypeName}:{rel_group.FirstEntity.Uid})";
								var second_entity = $"({rel_group.SecondEntity.TypeName}:{rel_group.SecondEntity.Uid})";

								foreach (var relation in rel_group.Relationships)
								{
									sb.Append($"  [{g++}] ");
									var arrow = relation.SameDirectionAsPath ? "->" : "<-";
									var rel_uid = FormatUID(relation.Relationship.Uid.ToString());
									sb.Append($"{first_entity} '\r\n\t{relation.Relationship.TypeName}:{rel_uid}' {arrow}\r\n" +
										$"\t\t{second_entity}");
									sb.Append($" cost: {relation.Relationship.Cost}\r\n");
								}
							}
						}
					}

				});
			}

			string FormatUID(string id)
			{
				id = id.ToUpperInvariant();
				if (!id.StartsWith('{'))
					id = '{' + id;
				if (!id.EndsWith('}'))
					id += '}';
				return id;
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.MaterializePath(System.Int64)
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.ExtractPathsEntitiesAndRelationships(System.Collections.Generic.SortedSet{System.UInt64})
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.PathsOriginEntitiesUIDsIndexes
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.PathsDestinationEntitiesUIDsIndexes
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.EntitiesUIDs
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.EntityTypes
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.EntityTypeNames
			#region List out FFP Results Origin, Destination, Other Entities
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(async () =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "List out FFP Results Origin, Destination, Other Entities";
					//set up config
					//...

					var results = kg.RunFilteredFindPaths(ffp_config);

					if (results.CountPaths == 0)
					{
						System.Diagnostics.Debug.WriteLine("FFP returned no paths");
						return;
					}

					//print out paths by increasing length, min cost, max cost
					var path_by_len_indices = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingPathLength
																			.Select(idx => idx.index);
					var path_by_min_cost = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingMinPathCost
																			.Select(idx => idx.index);
					var path_by_max_cost = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingMaxPathCost
																			.Select(idx => idx.index);

					var x = 0;
					StringBuilder sb = new StringBuilder();

					foreach (var path_indeces in new List<IEnumerable<long>> {
					path_by_len_indices,
					path_by_min_cost,
					path_by_max_cost})
					{
						if (x == 0)
							sb.AppendLine($"Entities by length: {path_by_len_indices.Count()}");
						else if (x == 1)
							sb.AppendLine($"Entities by min cost: {path_by_min_cost.Count()}");
						else if (x == 2)
							sb.AppendLine($"Entities by max cost: {path_by_max_cost.Count()}");
						x++;
						foreach (var path_idx in path_indeces)
						{
							var path = (ResultPath)results.MaterializePath(path_idx);
							sb.AppendLine(
								$"Path[{path_idx}] length: {path.Length}, min: {path.MinCost} max: {path.MaxCost}");

							var sorted_set = new SortedSet<ulong>();
							sorted_set.Add((ulong)path_idx);
							var per = results.ExtractPathsEntitiesAndRelationships(sorted_set);

							var origin_dest_uids = new List<string>();
							var sep = "";

							sb.Append(" Origin EntitiesUIDs: ");
							foreach (var idx in per.PathsOriginEntitiesUIDsIndexes)
							{
								//See 'List out FFP Results by Path Length, Min Cost, Max Cost' for
								//FormatID method above
								var uid = FormatID(per.EntitiesUIDs[idx].ToString());
								origin_dest_uids.Add(uid);

								var origin =
									$"{sep}{per.EntityTypeNames[per.EntityTypes[idx]]}:{uid}";
								sep = ", ";
								sb.Append($"{origin}");
							}
							sb.AppendLine("");

							sep = "";
							sb.Append(" Destination EntitiesUIDs: ");
							foreach (var idx in per.PathsDestinationEntitiesUIDsIndexes)
							{
								var uid = FormatID(per.EntitiesUIDs[idx].ToString());
								origin_dest_uids.Add(uid);

								var dest =
									$"{sep}{per.EntityTypeNames[per.EntityTypes[idx]]}:{uid}";
								sep = ", ";
								sb.Append($"{dest}");
							}
							sb.AppendLine("");

							sep = "";
							var idx2 = 0;
							sb.Append(" Other EntitiesUIDs: ");
							bool wereAnyOthers = false;
							foreach (var raw_uid in per.EntitiesUIDs)
							{
								var uid = FormatID(raw_uid.ToString());
								if (!origin_dest_uids.Contains(uid))
								{
									var other =
									$"{sep}{per.EntityTypeNames[per.EntityTypes[idx2]]}:{uid}";
									sep = ", ";
									sb.Append($"{other}");
									wereAnyOthers = true;
								}
								idx2++;
							}
							if (!wereAnyOthers)
								sb.Append(" <<none>>");

							//sb.AppendLine("");

							var entity_str = sb.ToString();
							System.Diagnostics.Debug.WriteLine(entity_str);

							sb.Clear();
							sep = "";
						}
					}

				});
			}
			#endregion

			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.KnowledgeGraphFilteredFindPathsResults.ExtractPathsEntitiesAndRelationships(System.Collections.Generic.SortedSet{System.UInt64})
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.RelationshipsUIDs
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.RelationshipTypes
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.RelationshipTypeNames
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.RelationshipsFrom
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.RelationshipsTo
			//cref: ArcGIS.Core.Data.Knowledge.Analytics.PathsEntitiesAndRelationships.EntitiesUIDs
			#region List out FFP Results Relationships
			{
				//using ArcGIS.Core.Data.Knowledge.Extensions;

				await QueuedTask.Run(async () =>
				{
					var ffp_config = new CIMFilteredFindPathsConfiguration();
					ffp_config.Name = "List out FFP Results Relationships";
					//set up config
					//...

					var results = kg.RunFilteredFindPaths(ffp_config);

					if (results.CountPaths == 0)
					{
						System.Diagnostics.Debug.WriteLine("FFP returned no paths");
						return;
					}

					//print out paths by increasing length, min cost, max cost
					var path_by_len_indices = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingPathLength
																			.Select(idx => idx.index);
					var path_by_min_cost = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingMinPathCost
																			.Select(idx => idx.index);
					var path_by_max_cost = (IEnumerable<long>)results.PathIndicesOrderedByIncreasingMaxPathCost
																			.Select(idx => idx.index);

					var x = 0;
					StringBuilder sb = new StringBuilder();

					foreach (var path_indeces in new List<IEnumerable<long>> {
					path_by_len_indices,
					path_by_min_cost,
					path_by_max_cost})
					{
						if (x == 0)
							sb.AppendLine($"Relationships by length: {path_by_len_indices.Count()}");
						else if (x == 1)
							sb.AppendLine($"Relationships by min cost: {path_by_min_cost.Count()}");
						else if (x == 2)
							sb.AppendLine($"Relationships by max cost: {path_by_max_cost.Count()}");
						x++;
						foreach (var path_idx in path_indeces)
						{
							var path = (ResultPath)results.MaterializePath(path_idx);
							sb.AppendLine(
								$"Path[{path_idx}] length: {path.Length}, min: {path.MinCost} max: {path.MaxCost}");

							var sorted_set = new SortedSet<ulong>();
							sorted_set.Add((ulong)path_idx);
							var per = results.ExtractPathsEntitiesAndRelationships(sorted_set);

							var idx = 0;
							foreach (var rel_uid in per.RelationshipsUIDs)
							{
								sb.Append($" RelationshipsUIDs[{idx}]: ");

								var uid = FormatID(rel_uid.ToString());
								var rel_info =
									$"{per.RelationshipTypeNames[per.RelationshipTypes[idx]]}:{uid}";
								sb.Append($"{rel_info}\r\n");
								//From entity:
								var entity_idx = per.RelationshipsFrom[idx];
								var origin_uid = FormatID(per.EntitiesUIDs[entity_idx].ToString());
								var origin = $"{per.EntityTypeNames[per.EntityTypes[entity_idx]]}:{origin_uid}";
								sb.Append($"   RelationshipsFrom: {origin}\r\n");
								//To entity
								entity_idx = per.RelationshipsTo[idx];
								var dest_uid = FormatID(per.EntitiesUIDs[entity_idx].ToString());
								var dest = $"{per.EntityTypeNames[per.EntityTypes[entity_idx]]}:{dest_uid}";
								sb.Append($"   RelationshipsTo: {dest}\r\n");
								idx++;
							}

							var rel_str = sb.ToString();
							System.Diagnostics.Debug.WriteLine(rel_str);
							sb.Clear();
						}
					}

				});
			}

			string FormatID(string id)
			{
				id = id.ToUpperInvariant();
				if (!id.StartsWith('{'))
					id = '{' + id;
				if (!id.EndsWith('}'))
					id += '}';
				return id;
			}
			#endregion
		}
	}

	//dont delete - used in KG Centrality Results snippets
	public class KG_Score_Value
	{
		public KG_Score_Value(double score, int uid_idx)
		{
			Score = score;
			Uid_idx = uid_idx;
		}

		public double Score { get; set; }
		public int Uid_idx { get; set; }

		public (double score, object uid) ToTuple(object[] uids)
		{
			return (Score, uids[Uid_idx]);
		}
	}
}
