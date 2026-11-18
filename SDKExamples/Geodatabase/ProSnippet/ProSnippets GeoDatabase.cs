using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.DDL;
using ArcGIS.Core.Data.Exceptions;
using ArcGIS.Core.Data.Mapping;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using FieldDescription = ArcGIS.Core.Data.DDL.FieldDescription;
using Index = ArcGIS.Core.Data.Index;
using Version = ArcGIS.Core.Data.Version;

namespace ProSnippets.GeoDatabaseSnippets
{
  partial class ProSnippetsGeoDatabase
  {
    /// <summary>
    /// This class provides code snippets for using the Geodatabase APIs in the ArcGIS Pro SDK for .NET.
    /// </summary>
    /// <remarks>
    /// These code snippets illustrate how to use Geodatabase APIs in the ArcGIS Pro SDK.
    /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
    /// Note: The Pro SDK Add-in may require certain code snippets to execute on the ArcGIS Pro Main CIM (MCT) thread or the UI thread.
    /// In such instances, the code should be enclosed within the QueuedTask.Run() block.
    /// <c>Cref</c>s are used for internal purposes only. Please ignore them in the context of this example.
    /// </remarks>
    public async Task ProSnippetsGeoDatabaseAsync()
    {
      #region ProSnippet Group: Geodatabases and Datastores
      #endregion

      #region Example of handling missing datasets in a geodatabase.
      {
        try
        {
          using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
          using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("NonExistentFeatureClass"))
          {
            // Use the feature class.
          }
        }
        catch (GeodatabaseTableException ex)
        {
          // Handle the case where the dataset does not exist.
          Console.WriteLine("Dataset not found: " + ex.Message);
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.FileGeodatabaseConnectionPath
      // cref: ArcGIS.Core.Data.FileGeodatabaseConnectionPath.#ctor(System.Uri)
      #region Opening a File Geodatabase given the path
      {
        await QueuedTask.Run(() =>
        {
          // Opens a file geodatabase. This will open the geodatabase if the folder exists and contains a valid geodatabase.
          using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
          {
            // Use the geodatabase.
          }
        });
      }
      #endregion Opening a File Geodatabase given the path


      // cref: ArcGIS.Core.Data.MobileGeodatabaseConnectionPath
      // cref: ArcGIS.Core.Data.Geodatabase.#ctor(ArcGIS.Core.Data.MobileGeodatabaseConnectionPath)
      // cref: ArcGIS.Core.Data.MobileGeodatabaseType
      #region Opening a Mobile Geodatabase given the path
      {
        await QueuedTask.Run(() =>
        {
          // Opens a mobile geodatabase. This will open the geodatabase if the folder exists and contains a valid mobile geodatabase.
          using (Geodatabase mobileGeodatabase = new Geodatabase(new MobileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.geodatabase"))))
          {
            Connector connector = mobileGeodatabase.GetConnector();
            if (connector is MobileGeodatabaseConnectionPath mobileConnPath)
            {
              // Get the type of the mobile geodatabase 
              MobileGeodatabaseType type = mobileConnPath.Type;
            }
          }
        });
      }
      #endregion Opening a Mobile Geodatabase given the path

      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.AuthenticationMode
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Instance
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Database
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.User
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Password
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Version
      // cref: ArcGIS.Core.Data.AuthenticationMode
      #region Opening an Enterprise Geodatabase using connection properties
      {
        await QueuedTask.Run(() =>
        {
          // Opening a Non-Versioned SQL Server instance.
          DatabaseConnectionProperties connectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
          {
            AuthenticationMode = AuthenticationMode.DBMS,

            // Where testMachine is the machine where the instance is running and testInstance is the name of the SqlServer instance.
            Instance = @"testMachine\testInstance",

            // Provided that a database called LocalGovernment has been created on the testInstance and geodatabase has been enabled on the database.
            Database = "LocalGovernment",

            // Provided that a login called gdb has been created and corresponding schema has been created with the required permissions.
            User = "gdb",
            Password = "password",
            Version = "dbo.DEFAULT"
          };

          using (Geodatabase geodatabase = new Geodatabase(connectionProperties))
          {
            // Use the geodatabase
          }
        });
      }

      #endregion Opening an Enterprise Geodatabase using connection properties

      // cref:ArcGIS.Core.Data.ArcGISServerConnectionFile
      // cref:ArcGIS.Core.Data.ArcGISServerConnectionFile.#ctor(System.Uri,System.String)
      #region Connect to the ArcGISServer using a connection file
      {
        await QueuedTask.Run(() =>
        {
          Uri uriPath = new Uri("path\\to\\the\\ArcGISServerConnectionFile.ags");
          ArcGISServerConnectionFile arcGISServerConnectionFile = new ArcGISServerConnectionFile(uriPath, "FeatureServiceName");

          using (Geodatabase geodatabase = new Geodatabase(arcGISServerConnectionFile))
          {
            // Use the geodatabase.
          }
        });
      }
      #endregion Connect to the ArcGISServer using a connection file

      // cref: ArcGIS.Core.Data.ServiceConnectionProperties
      #region Connecting to a Feature Service using a URL
      {
        //Url examples for (federated) feature services
        //(by "ref" online):
        var url =
            @"https://sampleserver6.arcgisonline.com/arcgis/rest/services/LocalGovernment/Recreation/FeatureServer";
        //(federated by ref on portal)
        //https://portal.example.com/server/rest/services/FeatureServiceName/FeatureServer
        //(federated by value - Hosted - on portal)
        //https://portal.example.com/server/rest/services/Hosted/FeatureServiceName/FeatureServer

        await QueuedTask.Run(() =>
        {
          var uri = new Uri(url, UriKind.Absolute);
          using (var fs_db =
              new ArcGIS.Core.Data.Geodatabase(new ServiceConnectionProperties(uri)))
          {
            //Use the geodatabase
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.ServiceConnectionProperties
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.Password
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.User
      #region Connecting to Different Types of Feature Services
      {
        await QueuedTask.Run(() =>
        {
          Uri nonFederatedServerURL = new Uri(
          "https://arcgis.server.example.com:6443/arcgis/rest/services/FeatureServiceName/FeatureServer");

          // Note that for non-federated ArcGIS Server hosted feature services,
          // the username and password have to be specified always.
          ServiceConnectionProperties nonFederatedArcGISServer =
                new ServiceConnectionProperties(nonFederatedServerURL)
                {
                  User = "serverUser",
                  Password = "serverPassword"
                };

          using (Geodatabase nonFederatedServerFeatureService =
            new Geodatabase(nonFederatedArcGISServer))
          {
            // Use the feature service geodatabase.
          }

          //Hosted (or published "by value")
          Uri federatedServerURL = new Uri(
            "http://federated.with.portal.example.com/server/rest/services/Hosted/FeatureServiceName/FeatureServer");
          //Published "by reference"
          //Uri federatedServerURL = new Uri(
          //"http://federated.with.portal.example.com/server/rest/services/FeatureServiceName/FeatureServer");

          // Note that for feature services hosted on ArcGIS Server federated with
          // ArcGIS Portal, the username and password cannot be specified through the API.
          // Even if the username and password were specified, they will be disregarded.
          // Instead the Portal authorization has to be configured by adding the Portal to
          // ArcGIS Pro with the user with which the connection should be established.
          // To connect to a Portal from a CoreHost application, use the
          // ArcGIS.Core.SystemCore.ArcGISSignOn class to authenticate with the Portal.

          ServiceConnectionProperties federatedArcGISServer =
            new ServiceConnectionProperties(federatedServerURL);

          using (Geodatabase federatedServerFeatureService =
            new Geodatabase(federatedArcGISServer))
          {
            // Use the feature service geodatabase.
          }

          Uri arcgisOnlineURL = new Uri(
            "http://services1.arcgis.com/47GG2ga246DGaLwa/arcgis/rest/services/FeatureServiceName/FeatureServer");
          //or
          //https://sampleserver6.arcgisonline.com/arcgis/rest/services/LocalGovernment/Recreation/FeatureServer
          //etc

          // Similar to Federated Feature Services, note that for feature services
          // hosted on ArcGIS Online, the username and password cannot be specified through
          // the API. Even if the username and password were specified, they will be
          // disregarded. Instead the connection will be established based on the ArcGIS
          // Online user credentials used to login to ArcGIS Pro at startup.

          ServiceConnectionProperties arcGISOnline =
            new ServiceConnectionProperties(arcgisOnlineURL);

          using (Geodatabase arcGISOnlineFeatureService =
            new Geodatabase(arcGISOnline))
          {
            // Use the feature service geodatabase.
          }
        });

      }
      #endregion

      // cref: ArcGIS.Core.Data.ServiceConnectionProperties
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.URL
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.User
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.Password
      // cref: ArcGIS.Core.Data.Datastore.GetConnector()
      // cref: ArcGIS.Core.Data.Dataset.GetDatastore()
      #region Get the Data Connection Properties from a Feature Service
      {
        var featureServiceMapView = MapView.Active;
        if (featureServiceMapView == null)
          return;

        await QueuedTask.Run(() =>
        {
          Layer selectedLayer = featureServiceMapView.GetSelectedLayers()[0];

          if (selectedLayer is FeatureLayer)
          {
            FeatureLayer featureLayer = selectedLayer as FeatureLayer;

            using (Table table = featureLayer.GetTable())
            using (Datastore datastore = table.GetDatastore())
            {
              ServiceConnectionProperties serviceConnectionProperties = datastore.GetConnector() as ServiceConnectionProperties;

              if (serviceConnectionProperties == null)
              {
                return;
              }

              Uri url = serviceConnectionProperties.URL;      // Will be the URL to the Feature Service.
              string user = serviceConnectionProperties.User;     // The username property will only be
                                                                  // populated for feature service hosted
                                                                  // on non-federated ArcGIS Server.
              string password = serviceConnectionProperties.Password; // Will always be empty.
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem.GetDatastore()
      // cref: ArcGIS.Core.Data.UnknownDatastore
      #region Obtaining Geodatabase from Project Item
      {
        var currentProject = Project.Current;
        if (currentProject == null)
          return;

        await QueuedTask.Run(() =>
        {
          IEnumerable<GDBProjectItem> gdbProjectItems = currentProject.GetItems<GDBProjectItem>();

          foreach (GDBProjectItem gdbProjectItem in gdbProjectItems)
          {
            using (Datastore datastore = gdbProjectItem.GetDatastore())
            {
              //Unsupported datastores (non File GDB and non Enterprise GDB) will be of type UnknownDatastore
              if (datastore is UnknownDatastore)
              {
                continue;
              }

              Geodatabase geodatabase = datastore as Geodatabase;
              // Use the geodatabase.
            }
          }
        });
      }
      #endregion Obtaining Geodatabase from Project Item

      // cref: ArcGIS.Core.Data.DatabaseConnectionFile
      // cref:ArcGIS.Core.Data.Geodatabase.#ctor(ArcGIS.Core.Data.DatabaseConnectionFile)
      // cref: ArcGIS.Core.Data.DatabaseClient.GetDatabaseConnectionProperties(ArcGIS.Core.Data.DatabaseConnectionFile)
      #region Getting Database Connection Properties from a Connection File
      {
        DatabaseConnectionFile connectionFile =
          new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"));
        DatabaseConnectionProperties connectionProperties =
          DatabaseClient.GetDatabaseConnectionProperties(connectionFile);

        // Now you could, for example, change the user name and password in the connection properties prior to use them to open a geodatabase
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.Layers
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetTable()
      // cref: ArcGIS.Core.Data.Dataset.GetDatastore()
      #region Obtaining Geodatabase from FeatureLayer
      {
        var featureLayerMapView = MapView.Active;
        if (featureLayerMapView == null)
          return;

        await QueuedTask.Run(() =>
        {
          IEnumerable<Layer> layers =
            featureLayerMapView.Map.Layers.Where(layer => layer is FeatureLayer);

          foreach (FeatureLayer featureLayer in layers)
          {
            using (Table table = featureLayer.GetTable())
            using (Datastore datastore = table.GetDatastore())
            {
              if (datastore is UnknownDatastore)
              {
                continue;
              }

              Geodatabase geodatabase = datastore as Geodatabase;
            }
          }
        });
      }
      #endregion Obtaining Geodatabase from FeatureLayer

      // cref: ArcGIS.Core.Data.DatabaseClient.ExecuteStatement(ArcGIS.Core.Data.Geodatabase,System.String)
      #region Executing SQL Statements
      {
        // Executes raw SQL on the underlying database management system.
        //  Any SQL is permitted (DDL or DML), but no results can be returned
        Task ExecuteSQLOnGeodatabaseAsync(Geodatabase geodatabase, string statement) => QueuedTask.Run(() =>
        {
          DatabaseClient.ExecuteStatement(geodatabase, statement);
        });
      }
      #endregion

      #region ProSnippet Group: Definitions
      #endregion

      // cref: ArcGIS.Core.Data.TableDefinition
      // cref: ArcGIS.Core.Data.FeatureClassDefinition
      // cref: ArcGIS.Core.Data.RelationshipClassDefinition
      // cref: ArcGIS.Core.Data.FeatureDatasetDefinition
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Obtaining Definition from Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          {
            // Remember that for Enterprise databases you have to qualify your dataset names with the DatabaseName and UserName.
            TableDefinition enterpriseTableDefinition = geodatabase.GetDefinition<TableDefinition>("LocalGovernment.GDB.CitizenContactInfo");

            // It does not matter if the dataset is within a FeatureDataset or not.
            FeatureClassDefinition featureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>("LocalGovernment.GDB.FireStation");

            // GetDefinition For a RelationshipClass.
            RelationshipClassDefinition relationshipClassDefinition = geodatabase.GetDefinition<RelationshipClassDefinition>(
                "LocalGovernment.GDB.AddressPointHasSiteAddresses");

            // GetDefinition For a FeatureDataset.
            FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("LocalGovernment.GDB.Address");
          }
        });
      }
      #endregion Obtaining Definition from Geodatabase

      // cref: ArcGIS.Core.Data.FeatureClassDefinition
      // cref: ArcGIS.Core.Data.RelationshipClassDefinition
      // cref: ArcGIS.Core.Data.FeatureDatasetDefinition
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinitions``1()
      // cref: ArcGIS.Core.Data.TableDefinition.HasGlobalID()
      // cref: ArcGIS.Core.Data.Definition.GetName()
      #region Obtaining List of Definitions from Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          {
            IReadOnlyList<FeatureClassDefinition> enterpriseDefinitions = geodatabase.GetDefinitions<FeatureClassDefinition>();
            IEnumerable<Definition> featureClassesHavingGlobalID = enterpriseDefinitions.Where(definition => definition.HasGlobalID());

            IReadOnlyList<FeatureDatasetDefinition> featureDatasetDefinitions = geodatabase.GetDefinitions<FeatureDatasetDefinition>();
            bool electionRelatedFeatureDatasets = featureDatasetDefinitions.Any(definition => definition.GetName().Contains("Election"));

            IReadOnlyList<AttributedRelationshipClassDefinition> attributedRelationshipClassDefinitions = geodatabase.GetDefinitions<AttributedRelationshipClassDefinition>();

            IReadOnlyList<RelationshipClassDefinition> relationshipClassDefinitions = geodatabase.GetDefinitions<RelationshipClassDefinition>();
          }
        });
      }
      #endregion Obtaining List of Defintions from Geodatabase

      // cref: ArcGIS.Core.Data.FeatureClassDefinition
      // cref: ArcGIS.Core.Data.TableDefinition
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      // cref: ArcGIS.Core.Data.TableDefinition.GetObjectIDField()
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      // cref: ArcGIS.Core.Data.FeatureClassDefinition.GetShapeField()
      #region Obtaining Definitions from a Feature Service by Layer ID
      {
        //Url examples for (federated) feature services
        //(by "ref" online):
        var url =
            @"https://sampleserver6.arcgisonline.com/arcgis/rest/services/SF311/FeatureServer";
        //(federated by ref on portal)
        //https://portal.example.com/server/rest/services/FeatureServiceName/FeatureServer");
        //(federated by value - Hosted - on portal)
        //https://portal.example.com/server/rest/services/Hosted/FeatureServiceName/FeatureServer");

        await QueuedTask.Run(() =>
        {
          var uri = new Uri(url, UriKind.Absolute);
          using (var fs_db = new ArcGIS.Core.Data.Geodatabase(new ServiceConnectionProperties(uri)))
          {
            //Assuming that id 0 is a FeatureClass for the given service
            FeatureClassDefinition featureClassDefinition =
                fs_db.GetDefinition<FeatureClassDefinition>("0");
            string shapeField = featureClassDefinition.GetShapeField();
            IReadOnlyList<Field> fields = featureClassDefinition.GetFields();

            //Assuming that id 1 is a Table for the given service
            TableDefinition tableDefinition =
                  fs_db.GetDefinition<TableDefinition>("1");
            string objectIDField = tableDefinition.GetObjectIDField();
          }
        });
      }

      #endregion

      // cref: ArcGIS.Core.Data.RelationshipClassDefinition
      // cref: ArcGIS.Core.Data.Geodatabase.GetRelatedDefinitions(ArcGIS.Core.Data.Definition,ArcGIS.Core.Data.DefinitionRelationshipType)
      // cref: ArcGIS.Core.Data.FeatureClassDefinition
      // cref: ArcGIS.Core.Data.DefinitionRelationshipType
      // cref: ArcGIS.Core.Data.Definition.GetName()
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Obtaining Related Definitions from Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase =
                 new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          {
            // Remember the qualification of DatabaseName. for the RelationshipClass.

            RelationshipClassDefinition enterpriseDefinition = geodatabase.GetDefinition<RelationshipClassDefinition>("LocalGovernment.GDB.AddressPointHasSiteAddresses");
            IReadOnlyList<Definition> enterpriseDefinitions = geodatabase.GetRelatedDefinitions(enterpriseDefinition,
                DefinitionRelationshipType.DatasetsRelatedThrough);
            FeatureClassDefinition enterpriseAddressPointDefinition = enterpriseDefinitions.First(defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPoint"))
                as FeatureClassDefinition;

            FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("LocalGovernment.GDB.Address");
            IReadOnlyList<Definition> datasetsInAddressDataset = geodatabase.GetRelatedDefinitions(featureDatasetDefinition,
                DefinitionRelationshipType.DatasetInFeatureDataset);
            FeatureClassDefinition addressPointInAddressDataset = datasetsInAddressDataset.First(defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPoint"))
                as FeatureClassDefinition;

            RelationshipClassDefinition addressPointHasSiteAddressInAddressDataset = datasetsInAddressDataset.First(defn =>
                  defn.GetName().Equals("LocalGovernment.GDB.AddressPointHasSiteAddresses"))
                as RelationshipClassDefinition;
          }
        });
      }
      #endregion Obtaining Related Definitions from Geodatabase

      // cref: ArcGIS.Core.Data.Table.IsJoinedTable()
      // cref: ArcGIS.Core.Data.Join.GetOriginTable()
      // cref: ArcGIS.Core.Data.Table.GetJoin()
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      #region Getting a Table Definition from a Layer
      {
        // GetDefinitionFromLayer - This code works even if the layer has a join to another table
        Task<TableDefinition> GetDefinitionFromLayerAsync(FeatureLayer featureLayer) => QueuedTask.Run(() =>
        {
          // Get feature class from the layer
          FeatureClass featureClass = featureLayer.GetFeatureClass();

          // Determine if feature class is a join
          if (featureClass.IsJoinedTable())
          {
            // Get join from feature class
            Join join = featureClass.GetJoin();

            // Get origin table from join
            Table originTable = join.GetOriginTable();

            // Return feature class definition from the join's origin table
            return originTable.GetDefinition();
          }
          return featureClass.GetDefinition();
        });
      }
      #endregion

      #region ProSnippet Group: Datasets
      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      #region Opening datasets from Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          {
            using (Table table = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.EmployeeInfo"))
            {
            }

            // Open a featureClass (within a feature dataset or outside a feature dataset).
            using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.AddressPoint"))
            {
            }

            // Open a FeatureClass as a Table which will return the Table reference.
            using (Table featureClassAsTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.AddressPoint"))
            {
              // But it is really a FeatureClass object.
              FeatureClass featureClassOpenedAsTable = featureClassAsTable as FeatureClass;
            }

            // Open a FeatureDataset.
            using (FeatureDataset featureDataset = geodatabase.OpenDataset<FeatureDataset>("LocalGovernment.GDB.Address"))
            {
            }

            // Open a RelationshipClass.  Just as you can open a FeatureClass as a Table, you can also open an AttributedRelationshipClass as a RelationshipClass.
            using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.AddressPointHasSiteAddresses"))
            {
            }
          }
        });
      }
      #endregion Opening datasets from Geodatabase

      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      #region Opening datasets from a Feature Service by Layer ID
      {
        //Url examples for (federated) feature services
        //(federated by ref on portal)
        var url =
            @"https://portal.example.com/server/rest/services/FeatureServiceName/FeatureServer";
        //(federated by value - Hosted - on portal)
        //https://portal.example.com/server/rest/services/Hosted/FeatureServiceName/FeatureServer

        await QueuedTask.Run(() =>
        {
          var uri = new Uri(url, UriKind.Absolute);
          using (var fs_db = new ArcGIS.Core.Data.Geodatabase(new ServiceConnectionProperties(uri)))
          {
            using (FeatureClass featureClass = fs_db.OpenDataset<FeatureClass>("0"))
            {
              // Use the feature class opened from layer ID 0.
            }

            using (Table table = fs_db.OpenDataset<Table>("4"))
            {
              // Use the table opened from layer ID 4.
            }

            using (AttributedRelationshipClass attributedRelationshipClass =
                fs_db.OpenDataset<AttributedRelationshipClass>("5"))
            {
              // Use the attributed relationship class opened from layer ID 5.
            }

            try
            {
              string idOfLayerWhichIsNotTable = "3";
              fs_db.OpenDataset<Table>(idOfLayerWhichIsNotTable);
            }
            catch (InvalidOperationException)
            {
              // Handle Exception.
            }
          }
        });
      }

      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Checking for the existence of a Table
      {
        Task<bool> TableExistsAsync(Geodatabase geodatabase, string tableName) => QueuedTask.Run(() =>
        {
          try
          {
            using TableDefinition tableDefinition =
              geodatabase.GetDefinition<TableDefinition>(tableName);
            return true;
          }
          catch
          {
            // GetDefinition throws an exception if the definition doesn't exist
            return false;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Checking for the existence of a Feature Class
      {
        Task<bool> FeatureClassExistsAsync(Geodatabase geodatabase, string featureClassName) => QueuedTask.Run(() =>
        {
          try
          {
            using FeatureClassDefinition featureClassDefinition =
              geodatabase.GetDefinition<FeatureClassDefinition>(featureClassName);
            return true;
          }
          catch
          {
            // GetDefinition throws an exception if the definition doesn't exist
            return false;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.OpenRelationshipClasses(System.String,System.String)
      #region Opening RelationshipClass between two Tables
      {
        // When used with file or enterprise geodatabases, this routine takes two table names.
        // When used with feature services, this routine takes layer IDs, or the names of the tables as they are exposed through the service (e.g., "L0States")
        Task<IReadOnlyList<RelationshipClass>> OpenRelationshipClassFeatureServicesAsync(
          Geodatabase geodatabase, string originClass, string destinationClass) => QueuedTask.Run(() =>
        {
          return geodatabase.OpenRelationshipClasses(originClass, destinationClass);
        });
      }
      #endregion Opening RelationshipClass between two Tables

      // cref: ArcGIS.Core.Data.Geodatabase.GetRelatedDefinitions(ArcGIS.Core.Data.Definition,ArcGIS.Core.Data.DefinitionRelationshipType)
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinitions``1()
      // cref: ArcGIS.Core.Data.RelationshipClassDefinition
      // cref: ArcGIS.Core.Data.DefinitionRelationshipType
      // cref: ArcGIS.Core.Data.Definition.GetName()
      #region Obtaining related Feature Classes from a Relationship Class
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
          {
            IReadOnlyList<RelationshipClassDefinition> relationshipClassDefinitions = geodatabase.GetDefinitions<RelationshipClassDefinition>();

            foreach (RelationshipClassDefinition relationshipClassDefinition in relationshipClassDefinitions)
            {
              IReadOnlyList<Definition> definitions = geodatabase.GetRelatedDefinitions(relationshipClassDefinition, DefinitionRelationshipType.DatasetsRelatedThrough);

              foreach (Definition definition in definitions)
              {
                Debug.WriteLine($"Feature class in the RelationshipClass is:{definition.GetName()}");
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.FileSystemDatastoreType
      // cref: ArcGIS.Core.Data.FileSystemDatastore.OpenDataset``1(string)
      #region Opening a FeatureClass from a ShapeFile Datastore
      {
        await QueuedTask.Run(() =>
        {
          FileSystemConnectionPath fileConnection = new FileSystemConnectionPath(new Uri("path\\to\\folder\\containing\\shapefiles"),
              FileSystemDatastoreType.Shapefile);
          using (FileSystemDatastore shapefile = new FileSystemDatastore(fileConnection))
          {
            FeatureClass taxLotsFeatureClass = shapefile.OpenDataset<FeatureClass>("TaxLots");

            // Can use the .shp extension, but its not needed.
            FeatureClass manHolesFeatureClass = shapefile.OpenDataset<FeatureClass>("ManHoles.shp");

            Table taxDetailsTableWithoutExtension = shapefile.OpenDataset<Table>("TaxDetails");
            Table taxDetailsTable = shapefile.OpenDataset<Table>("TaxDetails.dbf");
          }
        });
      }
      #endregion Opening a FeatureClass from a ShapeFile Datastore

      // cref: ArcGIS.Core.Data.FileSystemDatastoreType
      // cref: ArcGIS.Core.Data.FileSystemDatastore.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Opening a CAD Datastore
      {
        await QueuedTask.Run(() =>
        {
          FileSystemConnectionPath fileConnection = new FileSystemConnectionPath(new Uri("path\\to\\folder\\containing\\CAD"), FileSystemDatastoreType.Cad);
          using (FileSystemDatastore cadDatastore = new FileSystemDatastore(fileConnection))
          {
            //Note: Extension is required
            FeatureClass cadDataset = cadDatastore.OpenDataset<FeatureClass>("hatchplayboundaries.dwg");

            // Take note of the pattern for referencing a feature class.
            FeatureClass cadfeatureClass = cadDatastore.OpenDataset<FeatureClass>("hatchplayboundaries.dwg:Polyline");

            int numRows = 0;
            using (RowCursor cursor = cadfeatureClass.Search())
            {
              while (cursor.MoveNext())
              {
                numRows++;
              }
            }
          }
        });
      }
      #endregion Opening a CAD Datastore


      #region ProSnippet Group: Queries
      #endregion

      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.QueryFilter.SubFields
      // cref: ArcGIS.Core.Data.QueryFilter.PostfixClause
      // cref: ArcGIS.Core.Data.RowCursor
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      #region Searching a Table using QueryFilter
      {
        await QueuedTask.Run(() =>
        {
          try
          {
            using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
            using (Table table = geodatabase.OpenDataset<Table>("EmployeeInfo"))
            {
              QueryFilter queryFilter = new QueryFilter
              {
                WhereClause = "COSTCTRN = 'Information Technology'",
                SubFields = "KNOWNAS, OFFICE, LOCATION",
                PostfixClause = "ORDER BY OFFICE"
              };

              using (RowCursor rowCursor = table.Search(queryFilter, false))
              {
                while (rowCursor.MoveNext())
                {
                  using (Row row = rowCursor.Current)
                  {
                    string location = Convert.ToString(row["LOCATION"]);
                    string knownAs = Convert.ToString(row["KNOWNAS"]);
                  }
                }
              }
            }
          }
          catch (GeodatabaseFieldException fieldException)
          {
            // One of the fields in the where clause might not exist. There are multiple ways this can be handled:
            // Handle error appropriately
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.SQLSyntax.GetSupportedStrings(ArcGIS.Core.Data.SQLStringType)
      // cref: ArcGIS.Core.Data.Datastore.GetSQLSyntax()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      #region Searching a Table for non-Latin characters
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (Table table = geodatabase.OpenDataset<Table>("TableWithChineseCharacters"))
          {
            // This will fail with many database systems that expect Latin characters by default
            string incorrectWhereClause = "颜色 = '绿'";

            // Correct solution is to prepend the 'National String Prefix' to the attribute value
            // For example, with SQL Server this value is 'N'
            // This value is obtained using the SQLSyntax class
            string nationalStringPrefix = "";
            SQLSyntax sqlSyntax = geodatabase.GetSQLSyntax();
            nationalStringPrefix = sqlSyntax.GetSupportedStrings(SQLStringType.NationalStringPrefix).First();

            // This Where clause will work
            QueryFilter queryFilter = new QueryFilter()
            {
              WhereClause = "颜色 = " + nationalStringPrefix + "'绿'"
            };
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.QueryFilter.ObjectIDs
      #region Searching a Table using a set of ObjectIDs
      {
        Task<RowCursor> SearchingATableAsync(Table table, IReadOnlyList<long> objectIDs) => QueuedTask.Run(() =>
        {
          QueryFilter queryFilter = new QueryFilter()
          {
            ObjectIDs = objectIDs
          };

          return table.Search(queryFilter);
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.FilterGeometry
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.SpatialRelationship
      // cref: ArcGIS.Core.Data.SpatialRelationship
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.Row.GetObjectID()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      #region Searching a FeatureClass using SpatialQueryFilter
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (FeatureClass schoolBoundaryFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.SchoolBoundary"))
          {
            // Using a spatial query filter to find all features which have a certain district name and lying within a given Polygon.
            SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter
            {
              WhereClause = "DISTRCTNAME = 'Indian Prairie School District 204'",
              FilterGeometry = new PolygonBuilderEx(new List<Coordinate2D>
              {
                new Coordinate2D(1021880, 1867396),
                new Coordinate2D(1028223, 1870705),
                new Coordinate2D(1031165, 1866844),
                new Coordinate2D(1025373, 1860501),
                new Coordinate2D(1021788, 1863810)
              }).ToGeometry(),

              SpatialRelationship = SpatialRelationship.Within
            };

            using (RowCursor indianPrairieCursor = schoolBoundaryFeatureClass.Search(spatialQueryFilter, false))
            {
              while (indianPrairieCursor.MoveNext())
              {
                using (Feature feature = (Feature)indianPrairieCursor.Current)
                {
                  // Process the feature. For example...
                  Console.WriteLine(feature.GetObjectID());
                }
              }
            }
          }
        });
      }
      #endregion Searching a FeatureClass using SpatialQueryFilter

      // cref: ArcGIS.Core.Data.Selection
      // cref: ArcGIS.Core.Data.SelectionType
      // cref: ArcGIS.Core.Data.SelectionOption
      // cref: ArcGIS.Core.Data.Table.Select(ArcGIS.Core.Data.QueryFilter,ArcGIS.Core.Data.SelectionType,ArcGIS.Core.Data.SelectionOption)
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      #region Selecting Rows from a Table
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
          {
            QueryFilter anotherQueryFilter = new QueryFilter { WhereClause = "FLOOR = 1 AND WING = 'E'" };

            // For Selecting all matching entries.
            using (Selection anotherSelection = enterpriseTable.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
            {
            }

            // This can be used to get one record which matches the criteria. No assumptions can be made about which record satisfying the criteria is selected.
            using (Selection onlyOneSelection = enterpriseTable.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.OnlyOne))
            {
            }

            // This can be used to obtain a empty selection which can be used as a container to combine results from different selections.
            using (Selection emptySelection = enterpriseTable.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.Empty))
            {
            }

            // If you want to select all the records in a table.
            using (Selection allRecordSelection = enterpriseTable.Select(null, SelectionType.ObjectID, SelectionOption.Normal))
            {
            }
          }
        });
      }
      #endregion Selecting Rows from a Table

      // cref: ArcGIS.Core.Data.Table.Select(ArcGIS.Core.Data.QueryFilter,ArcGIS.Core.Data.SelectionType,ArcGIS.Core.Data.SelectionOption)
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.FilterGeometry
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.SpatialRelationship
      // cref: ArcGIS.Core.Data.SpatialRelationship
      // cref: ArcGIS.Core.Data.SelectionType
      // cref: ArcGIS.Core.Data.SelectionOption
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      #region Selecting Features from a FeatureClass
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
          {
            List<Coordinate2D> newCoordinates = new List<Coordinate2D>
            {
              new Coordinate2D(1021570, 1880583),
              new Coordinate2D(1028730, 1880994),
              new Coordinate2D(1029718, 1875644),
              new Coordinate2D(1021405, 1875397)
            };

            SpatialQueryFilter spatialFilter = new SpatialQueryFilter
            {
              WhereClause = "FCODE = 'Park'",
              FilterGeometry = new PolygonBuilderEx(newCoordinates).ToGeometry(),
              SpatialRelationship = SpatialRelationship.Crosses
            };

            // For Selecting all matching entries.
            using (Selection anotherSelection = enterpriseFeatureClass.Select(spatialFilter, SelectionType.ObjectID, SelectionOption.Normal))
            {
            }

            // This can be used to get one record which matches the criteria. No assumptions can be made about which record satisfying the
            // criteria is selected.
            using (Selection onlyOneSelection = enterpriseFeatureClass.Select(spatialFilter, SelectionType.ObjectID, SelectionOption.OnlyOne))
            {
            }

            // This can be used to obtain an empty selction which can be used as a container to combine results from different selections.
            using (Selection emptySelection = enterpriseFeatureClass.Select(spatialFilter, SelectionType.ObjectID, SelectionOption.Empty))
            {
            }

            // If you want to select all the records in a table.
            using (Selection allRecordSelection = enterpriseFeatureClass.Select(null, SelectionType.ObjectID, SelectionOption.Normal))
            {
            }
          }
        });
      }
      #endregion Selecting Features from a FeatureClass

      // cref: ArcGIS.Core.Data.Table.GetCount()
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetTable()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetFeatureClass()
      #region Gets the count of a Table or FeatureClass
      {
        Task GetCountAsync(FeatureLayer featureLayer)
        {
          var map = MapView.Active?.Map;
          if (map == null)
            return Task.CompletedTask;

          return QueuedTask.Run(() =>
          {
            // Row count in a Table
            Table table = featureLayer.GetTable();
            long count = table.GetCount();

            // Feature count in a FeatureClass
            FeatureLayer lyr = map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
            FeatureClass featureClass = lyr.GetFeatureClass();
            count = featureClass.GetCount();
          });
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.Table.Sort(ArcGIS.Core.Data.TableSortDescription)
      // cref: ArcGIS.Core.Data.FeatureClassDefinition
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      // cref: ArcGIS.Core.Data.SortDescription.CaseSensitivity
      // cref: ArcGIS.Core.Data.SortDescription.SortOrder
      // cref: ArcGIS.Core.Data.CaseSensitivity
      // cref: ArcGIS.Core.Data.SortOrder
      // cref: ArcGIS.Core.Data.Field.Name
      #region Sorting a Table
      {
        Task<RowCursor> SortWorldCitiesAsync(FeatureClass worldCitiesTable) => QueuedTask.Run(() =>
        {
          using (FeatureClassDefinition featureClassDefinition = worldCitiesTable.GetDefinition())
          {
            Field countryField = featureClassDefinition.GetFields()
              .First(x => x.Name.Equals("COUNTRY_NAME"));

            Field cityNameField = featureClassDefinition.GetFields()
              .First(x => x.Name.Equals("CITY_NAME"));

            // Create SortDescription for Country field
            SortDescription countrySortDescription = new SortDescription(countryField);
            countrySortDescription.CaseSensitivity = CaseSensitivity.Insensitive;
            countrySortDescription.SortOrder = SortOrder.Ascending;

            // Create SortDescription for City field
            SortDescription citySortDescription = new SortDescription(cityNameField);
            citySortDescription.CaseSensitivity = CaseSensitivity.Insensitive;
            citySortDescription.SortOrder = SortOrder.Ascending;

            // Create our TableSortDescription
            TableSortDescription tableSortDescription = new TableSortDescription(
              new List<SortDescription>() { countrySortDescription, citySortDescription });

            return worldCitiesTable.Sort(tableSortDescription);
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.TableStatisticsDescription.GroupBy
      // cref: ArcGIS.Core.Data.TableStatisticsDescription.OrderBy
      // cref: ArcGIS.Core.Data.Table.CalculateStatistics(ArcGIS.Core.Data.TableStatisticsDescription)
      // cref: ArcGIS.Core.Data.StatisticsFunction
      // cref: ArcGIS.Core.Data.TableStatisticsResult
      // cref: ArcGIS.Core.Data.TableStatisticsResult.StatisticsResults
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.StatisticsResult.Average
      // cref: ArcGIS.Core.Data.StatisticsResult.Sum
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      #region Calculating Statistics on a Table
      {
        // Calculate the Sum and Average of the Population_1990 and Population_2000 fields, grouped and ordered by Region
        Task CalculateStatisticsAsync(FeatureClass countryFeatureClass) => QueuedTask.Run(() =>
        {
          using (FeatureClassDefinition featureClassDefinition = countryFeatureClass.GetDefinition())
          {
            // Get fields
            Field regionField = featureClassDefinition.GetFields()
              .First(x => x.Name.Equals("Region"));

            Field pop1990Field = featureClassDefinition.GetFields()
              .First(x => x.Name.Equals("Population_1990"));

            Field pop2000Field = featureClassDefinition.GetFields()
              .First(x => x.Name.Equals("Population_2000"));

            // Create StatisticsDescriptions
            StatisticsDescription pop1990StatisticsDescription = new StatisticsDescription(pop1990Field,
              new List<StatisticsFunction>()
              {
                StatisticsFunction.Sum,
                StatisticsFunction.Average
              });

            StatisticsDescription pop2000StatisticsDescription = new StatisticsDescription(pop2000Field,
              new List<StatisticsFunction>()
              {
                StatisticsFunction.Sum,
                StatisticsFunction.Average
              });

            // Create TableStatisticsDescription
            TableStatisticsDescription tableStatisticsDescription = new TableStatisticsDescription(
              new List<StatisticsDescription>()
              {
                pop1990StatisticsDescription, pop2000StatisticsDescription
              });
            tableStatisticsDescription.GroupBy = new List<Field>() { regionField };
            tableStatisticsDescription.OrderBy = new List<SortDescription>() { new SortDescription(regionField) };

            // Calculate Statistics
            IReadOnlyList<TableStatisticsResult> tableStatisticsResults =
              countryFeatureClass.CalculateStatistics(tableStatisticsDescription);

            foreach (TableStatisticsResult tableStatisticsResult in tableStatisticsResults)
            {
              // Get the Region name
              // If multiple fields had been passed into TableStatisticsDescription.GroupBy, there would be multiple values in TableStatisticsResult.GroupBy
              string regionName = tableStatisticsResult.GroupBy.First().Value.ToString();

              // Get the statistics results for the Population_1990 field
              StatisticsResult pop1990Statistics = tableStatisticsResult.StatisticsResults[0];
              double population1990Sum = pop1990Statistics.Sum;
              double population1990Average = pop1990Statistics.Average;

              // Get the statistics results for the Population_2000 field
              StatisticsResult pop2000Statistics = tableStatisticsResult.StatisticsResults[1];
              double population2000Sum = pop2000Statistics.Sum;
              double population2000Average = pop2000Statistics.Average;

              // Do something with the results here...
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.Evaluate(ArcGIS.Core.Data.QueryDef,System.Boolean)
      // cref: ArcGIS.Core.Data.QueryDef
      // cref: ArcGIS.Core.Data.QueryDef.Tables
      // cref: ArcGIS.Core.Data.QueryDef.WhereClause
      // cref: ArcGIS.Core.Data.Feature.GetShape()
      // cref: ArcGIS.Core.Data.Row.GetTable()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      #region Evaluating a QueryDef on a single table
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          {
            QueryDef adaCompilantParksQueryDef = new QueryDef
            {
              Tables = "Park",
              WhereClause = "ADACOMPLY = 'Yes'",
            };

            using (RowCursor rowCursor = geodatabase.Evaluate(adaCompilantParksQueryDef, false))
            {
              while (rowCursor.MoveNext())
              {
                using (Row row = rowCursor.Current)
                {
                  Feature feature = row as Feature;
                  Geometry shape = feature.GetShape();

                  String type = Convert.ToString(row["ADACOMPLY"]); // will be "Yes" for each row.

                  try
                  {
                    Table table = row.GetTable(); // Will always throw exception.
                  }
                  catch (NotSupportedException exception)
                  {
                    // Handle not supported exception.
                  }
                }
              }
            }
          }
        });
      }
      #endregion Evaluating a QueryDef on a single table

      // cref: ArcGIS.Core.Data.Geodatabase.Evaluate(ArcGIS.Core.Data.QueryDef,System.Boolean)
      // cref: ArcGIS.Core.Data.QueryDef
      // cref: ArcGIS.Core.Data.QueryDef.Tables
      // cref: ArcGIS.Core.Data.QueryDef.WhereClause
      // cref: ArcGIS.Core.Data.QueryDef.SubFields
      // cref: ArcGIS.Core.Data.Row.GetFields()
      // cref: ArcGIS.Core.Data.Feature.GetShape()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      #region Evaluating a QueryDef on a Join using WHERE Clause
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          {
            QueryDef municipalEmergencyFacilitiesQueryDef = new QueryDef
            {
              SubFields = "EmergencyFacility.OBJECTID, EmergencyFacility.Shape, EmergencyFacility.FACILITYID, FacilitySite.FACILITYID, FacilitySite.FCODE",
              Tables = "EmergencyFacility, FacilitySite",
              WhereClause = "EmergencyFacility.FACNAME = FacilitySite.NAME AND EmergencyFacility.JURISDICT = 'Municipal'",
            };

            using (RowCursor rowCursor = geodatabase.Evaluate(municipalEmergencyFacilitiesQueryDef, false))
            {
              while (rowCursor.MoveNext())
              {
                using (Row row = rowCursor.Current)
                {
                  Feature feature = row as Feature;
                  Geometry shape = feature.GetShape();

                  long objectID = Convert.ToInt64(row["EmergencyFacility.OBJECTID"]);
                  String featureCode = Convert.ToString(row["FacilitySite.FCODE"]);

                  IReadOnlyList<Field>
                    fields = feature.GetFields(); //Contains one ArcGIS.Core.Data.Field objects for every subfield
                }
              }
            }
          }
        });
      }
      #endregion Evaluating a QueryDef on a Join using WHERE Clause

      // cref: ArcGIS.Core.Data.Geodatabase.Evaluate(ArcGIS.Core.Data.QueryDef,System.Boolean)
      // cref: ArcGIS.Core.Data.QueryDef.Tables
      // cref: ArcGIS.Core.Data.QueryDef.SubFields
      // cref: ArcGIS.Core.Data.Feature.GetShape()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      #region Evaluating a QueryDef on a OUTER JOIN
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          {
            QueryDef queryDefWithLeftOuterJoin = new QueryDef
            {
              Tables = "CommunityAddress LEFT OUTER JOIN MunicipalBoundary on CommunityAddress.Municipality = MunicipalBoundary.Name",
              SubFields = "CommunityAddress.OBJECTID, CommunityAddress.Shape, CommunityAddress.SITEADDID, CommunityAddress.ADDRNUM, CommunityAddress.FULLNAME, CommunityAddress.FULLADDR, CommunityAddress.MUNICIPALITY, MunicipalBoundary.Name, MunicipalBoundary.MUNITYP, MunicipalBoundary.LOCALFIPS",
            };

            using (RowCursor rowCursor = geodatabase.Evaluate(queryDefWithLeftOuterJoin, false))
            {
              while (rowCursor.MoveNext())
              {
                using (Row row = rowCursor.Current)
                {
                  Feature feature = row as Feature;
                  Geometry shape = feature.GetShape();

                  int siteAddressId = Convert.ToInt32(row["CommunityAddress.SITEADDID"]);
                  String stateName = Convert.ToString(row["MunicipalBoundary.name"]);
                }
              }
            }
          }
        });
      }
      #endregion Evaluating a QueryDef on a OUTER JOIN

      // cref: ArcGIS.Core.Data.Geodatabase.Evaluate(ArcGIS.Core.Data.QueryDef,System.Boolean)
      // cref: ArcGIS.Core.Data.QueryDef.Tables
      // cref: ArcGIS.Core.Data.QueryDef.SubFields
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      #region Evaluating a QueryDef on a INNER join
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          {
            QueryDef queryDef = new QueryDef()
            {
              Tables = "People INNER JOIN States ON People.FK_STATE_ID = States.OBJECTID",
              SubFields = "People.OBJECTID, People.First_Name, People.Last_Name, People.City, States.State_Name"
            };

            using (RowCursor cursor = geodatabase.Evaluate(queryDef))
            {
              while (cursor.MoveNext())
              {
                using (Row row = cursor.Current)
                {
                  // Handle row
                }
              }
            }
          }
        });
      }
      #endregion Evaluating a QueryDef on a INNER join

      // cref: ArcGIS.Core.Data.Geodatabase.Evaluate(ArcGIS.Core.Data.QueryDef,System.Boolean)
      // cref: ArcGIS.Core.Data.QueryDef.Tables
      // cref: ArcGIS.Core.Data.QueryDef.SubFields
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      #region Evaluating a QueryDef on a nested - INNER  and  OUTER join
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          {
            QueryDef queryDef = new QueryDef()
            {
              Tables = "((People INNER JOIN States ON People.FK_STATE_ID = States.OBJECTID) LEFT OUTER JOIN Homes ON People.OBJECTID = Homes.FK_People_ID)",
              SubFields = "People.OBJECTID, People.First_Name, People.Last_Name, States.State_Name, Homes.Address"
            };

            using (RowCursor cursor = geodatabase.Evaluate(queryDef, false))
            {
              while (cursor.MoveNext())
              {
                using (Row row = cursor.Current)
                {
                  // Handle row
                }
              }
            }
          }
        });
      }
      #endregion Evaluating a QueryDef on nested (INNER & OUTER) join

      // cref: ArcGIS.Core.Data.Database.GetQueryDescription(System.String)
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.AuthenticationMode
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Instance
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Database
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.User
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Password
      // cref: ArcGIS.Core.Data.AuthenticationMode
      // cref: ArcGIS.Core.Data.Database.OpenTable(ArcGIS.Core.Data.QueryDescription)
      // cref: ArcGIS.Core.Data.EnterpriseDatabaseType
      // cref: ArcGIS.Core.Data.QueryDescription
      #region Create Default QueryDescription for a Database table and obtain the ArcGIS.Core.Data.Table for the QueryDescription
      {
        await QueuedTask.Run(() =>
        {
          DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
          {
            AuthenticationMode = AuthenticationMode.DBMS,
            Instance = "instance",
            Database = "database",
            User = "user",
            Password = "password"
          };

          using (Database database = new Database(databaseConnectionProperties))
          {
            QueryDescription queryDescription = database.GetQueryDescription("CUSTOMERS");

            using (Table table = database.OpenTable(queryDescription))
            {
              //use table
            }
          }
        });
      }
      #endregion Create Default QueryDescription for a Database table and obtain the ArcGIS.Core.Data.Table for the QueryDescription

      // cref: ArcGIS.Core.Data.Database.GetQueryDescription(System.String,System.String)
      // cref: ArcGIS.Core.Data.EnterpriseDatabaseType
      // cref: ArcGIS.Core.Data.QueryDescription
      // cref: ArcGIS.Core.Data.Database.OpenTable(ArcGIS.Core.Data.QueryDescription)
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.AuthenticationMode
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Database
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Instance
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Password
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.User
      #region Create QueryDescription from a custom query for a Database table
      {
        await QueuedTask.Run(() =>
        {
          DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
          {
            AuthenticationMode = AuthenticationMode.DBMS,
            Instance = "instance",
            Database = "database",
            User = "user",
            Password = "password"
          };

          using (Database database = new Database(databaseConnectionProperties))
          {
            QueryDescription queryDescription = database.GetQueryDescription("SELECT OBJECTID, Shape, FACILITYID FROM EmergencyFacility WHERE JURISDICT = 'Municipal'",
              "MunicipalEmergencyFacilities");

            using (Table table = database.OpenTable(queryDescription))
            {
              // Use the table.
            }
          }
        });
      }
      #endregion Create QueryDescription from a custom query for a Database table

      // cref: ArcGIS.Core.Data.Database.GetQueryDescription(System.String,System.String)
      // cref: ArcGIS.Core.Data.QueryDescription.SetObjectIDFields(System.String)
      // cref: ArcGIS.Core.Data.Database.OpenTable(ArcGIS.Core.Data.QueryDescription)
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.AuthenticationMode
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Database
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Instance
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Password
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.User
      #region Create QueryDescription from a join query where there is no non-nullable unique id column
      {
        await QueuedTask.Run(() =>
        {
          DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
          {
            AuthenticationMode = AuthenticationMode.DBMS,
            Instance = "instance",
            Database = "database",
            User = "user",
            Password = "password"
          };

          using (Database database = new Database(databaseConnectionProperties))
          {
            QueryDescription queryDescription = database.GetQueryDescription("SELECT BUSLINES.ID as BUSLINESID, BUSSTOPS.ID as BUSSTOPSID, BUSLINES.RTE_DESC, BUSLINES.DIR, BUSSTOPS.JURISDIC, BUSSTOPS.LOCATION, BUSSTOPS.ROUTE,BUSSTOPS.SHAPE from demosql.dbo.BUSSTOPS JOIN demosql.dbo.BUSLINES ON BUSSTOPS.ROUTE = BUSLINES.ROUTE",
              "BusInfo");

            queryDescription.SetObjectIDFields("BUSLINESID,BUSSTOPSID");

            using (Table table = database.OpenTable(queryDescription))
            {
              // Use the table.
            }
          }
        });
      }
      #endregion Create QueryDescription from a join query where there is no non-nullable unique id column

      // cref: ArcGIS.Core.Data.Database.GetQueryDescription(System.String,System.String)
      // cref: ArcGIS.Core.Data.QueryDescription.SetShapeType(ArcGIS.Core.Geometry.GeometryType)
      // cref: ArcGIS.Core.Data.Database.OpenTable(ArcGIS.Core.Data.QueryDescription)
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.AuthenticationMode
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Database
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Instance
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Password
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.User
      #region Create QueryDescription from a query for a Database table which has more than one shape type
      {
        await QueuedTask.Run(() =>
        {
          DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
          {
            AuthenticationMode = AuthenticationMode.DBMS,
            Instance = "instance",
            Database = "database",
            User = "user",
            Password = "password"
          };

          using (Database database = new Database(databaseConnectionProperties))
          {
            QueryDescription pointQueryDescription = database.GetQueryDescription("select Description, SHAPE, UniqueID from MULTIGEOMETRYTEST",
                "MultiGeometryPoint");
            pointQueryDescription.SetShapeType(GeometryType.Point);
            using (Table pointTable = database.OpenTable(pointQueryDescription))
            {
              //use pointTable
            }

            QueryDescription polygonQueryDescription = database.GetQueryDescription("select Description, SHAPE, UniqueID from MULTIGEOMETRYTEST",
                "MultiGeometryPolygon");
            polygonQueryDescription.SetShapeType(GeometryType.Polygon);
            using (Table polygonTable = database.OpenTable(polygonQueryDescription))
            {
              //use polygonTable
            }
          }
        });
      }
      #endregion Create QueryDescription from a query for a Database table which has more than one shape type

      // cref: ArcGIS.Core.Data.Database.GetQueryDescription(System.String,System.String)
      // cref: ArcGIS.Core.Data.Database.OpenTable(ArcGIS.Core.Data.QueryDescription)
      #region Create QueryDescription from a query for an SQLite Database table
      {
        await QueuedTask.Run(() =>
        {
          using (Database database = new Database(new SQLiteConnectionPath(new Uri("Path\\To\\Sqlite\\Database\\USA.sqlite"))))
          {
            QueryDescription washingtonCitiesQueryDescription = database.GetQueryDescription("select OBJECTID, Shape, CITY_FIPS, CITY_NAME, STATE_FIPS, STATE_CITY, TYPE, CAPITAL from main.cities where STATE_NAME='Washington'",
              "WashingtonCities");

            using (Table washingtonTable = database.OpenTable(washingtonCitiesQueryDescription))
            {
              // Use washingtonTable.
            }
          }
        });
      }
      #endregion Create QueryDescription from a query for a SQLite Database table

      // cref: ArcGIS.Core.Data.SQLSyntax.GetFunctionName(ArcGIS.Core.Data.SQLFunction)
      // cref: ArcGIS.Core.Data.Datastore.GetSQLSyntax()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.Table.Select(ArcGIS.Core.Data.QueryFilter,ArcGIS.Core.Data.SelectionType,ArcGIS.Core.Data.SelectionOption)
      #region Using SQLSyntax to form platform agnostic queries
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Data\\LocalGovernment.gdb"))))
          using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("FacilitySite"))
          {
            SQLSyntax sqlSyntax = geodatabase.GetSQLSyntax();
            string substringFunctionName = sqlSyntax.GetFunctionName(SQLFunction.Substring);
            string upperFunctionName = sqlSyntax.GetFunctionName(SQLFunction.Upper);
            string substringfunction = string.Format("{0}({1}(FCODE, 1, 6)) = 'SCHOOL'", upperFunctionName,
              substringFunctionName);

            QueryFilter queryFilter = new QueryFilter
            {
              WhereClause = substringfunction
            };
            using (Selection selection =
                   featureClass.Select(queryFilter, SelectionType.ObjectID, SelectionOption.Normal))
            {
              // work with the selection.
            }
          }
        });
      }
      #endregion Using SQLSyntax to form platform agnostic queries

      // cref: ArcGIS.Core.Data.QueryFilter.FullTextExpression
      // cref: ArcGIS.Core.Data.QueryFilter
      // cref: ArcGIS.Core.Data.Datastore.GetDatastoreProperties()
      // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsFullTextIndex
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.FullTextSearchTermExpression.SearchFields
      // cref: ArcGIS.Core.Data.FullTextSearchTermExpression.SearchTerm
      // cref: ArcGIS.Core.Data.FullTextSearchTermExpression.SearchType
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.Index.GetFields()
      // cref: ArcGIS.Core.Data.QueryFilter.Offset
      // cref: ArcGIS.Core.Data.QueryFilter.PostfixClause
      // cref: ArcGIS.Core.Data.QueryFilter.RowCount
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.TableDefinition.GetIndexes()
      #region Fulltext search query
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (Table table = geodatabase.OpenDataset<Table>("WaterPipeInfo"))
          using (TableDefinition tableDefinition = table.GetDefinition())
          {
            bool supportsFullText = geodatabase.GetDatastoreProperties().SupportsFullTextIndex;
            if (!supportsFullText)
            {
              return;
            }

            IReadOnlyList<Index> indexes = tableDefinition.GetIndexes();
            IReadOnlyList<Field> fieldsToSearch = indexes.First().GetFields();

            FullTextSearchTermExpression searchRepairStatus = new FullTextSearchTermExpression()
            {
              SearchFields = fieldsToSearch[0].Name,
              SearchTerm = "Repaired",
              SearchType = FullTextSearchType.Simple
            };

            FullTextSearchTermExpression searchPipeMaterial = new FullTextSearchTermExpression()
            {
              SearchFields = fieldsToSearch[1].Name,
              SearchTerm = "Copper",
              SearchType = FullTextSearchType.Simple
            };

            FullTextOrExpression repairedOrCopperEx = new FullTextOrExpression(searchRepairStatus, searchPipeMaterial);

            QueryFilter queryfilter = new QueryFilter
            {
              FullTextExpression = repairedOrCopperEx,
              Offset = 1,
              RowCount = 20,
              PostfixClause = "Order By Size"
            };

            using (RowCursor rowCursor = table.Search(queryfilter, false))
            {
              // Use row cursor ...
            }
          }
        });
      }
      #endregion Fulltext search query

      // cref: ArcGIS.Core.Data.VirtualRelationshipClassDescription
      // cref: ArcGIS.Core.Data.Table.RelateTo(ArcGIS.Core.Data.Table,ArcGIS.Core.Data.VirtualRelationshipClassDescription)
      // cref: ArcGIS.Core.Data.Join.GetJoinedTable()
      // cref: ArcGIS.Core.Data.RelationshipCardinality
      // cref: ArcGIS.Core.Data.RelationshipClass
      // cref: ArcGIS.Core.Data.JoinDescription
      // cref: ArcGIS.Core.Data.JoinDescription.JoinDirection
      // cref: ArcGIS.Core.Data.JoinDescription.JoinType
      // cref: ArcGIS.Core.Data.JoinDirection
      // cref: ArcGIS.Core.Data.JoinType
      // cref: ArcGIS.Core.Data.Database.GetQueryDescription(System.String)
      // cref: ArcGIS.Core.Data.Database.OpenTable(ArcGIS.Core.Data.QueryDescription)
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.AuthenticationMode
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Database
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Instance
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Password
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.User
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      #region Joining a file geodatabase feature class to an Oracle database query layer feature class with a virtual relationship class
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Data\\LocalGovernment.gdb"))))
          using (Database database = new Database(new DatabaseConnectionProperties(EnterpriseDatabaseType.Oracle)
          {
            AuthenticationMode = AuthenticationMode.DBMS,
            Instance = "instance",
            User = "user",
            Password = "password",
            Database = "database"
          }))

          using (FeatureClass leftFeatureClass = geodatabase.OpenDataset<FeatureClass>("Hospital"))
          using (Table rightTable = database.OpenTable(database.GetQueryDescription("FacilitySite")))
          {
            Field originPrimaryKey = leftFeatureClass.GetDefinition().GetFields()
              .FirstOrDefault(field => field.Name.Equals("facilityId"));

            Field destinationForeignKey = rightTable.GetDefinition().GetFields()
              .FirstOrDefault(field => field.Name.Equals("hospitalID"));

            VirtualRelationshipClassDescription description = new VirtualRelationshipClassDescription(
              originPrimaryKey, destinationForeignKey, RelationshipCardinality.OneToOne);

            using (RelationshipClass relationshipClass = leftFeatureClass.RelateTo(rightTable, description))
            {
              JoinDescription joinDescription = new JoinDescription(relationshipClass)
              {
                JoinDirection = JoinDirection.Forward,
                JoinType = JoinType.LeftOuterJoin
              };

              Join join = new Join(joinDescription);

              using (Table joinedTable = join.GetJoinedTable())
              {
                // Perform operation on joined table.
              }
            }
          }
        });
      }
      #endregion Joining a file geodatabase feature class to an Oracle database query layer feature class with a virtual relationship class

      // cref: ArcGIS.Core.Data.VirtualRelationshipClassDescription
      // cref: ArcGIS.Core.Data.Join.GetJoinedTable()
      // cref: ArcGIS.Core.Data.JoinDescription
      // cref: ArcGIS.Core.Data.JoinDescription.JoinDirection
      // cref: ArcGIS.Core.Data.JoinDescription.JoinType
      // cref: ArcGIS.Core.Data.JoinDescription.TargetFields
      // cref: ArcGIS.Core.Data.JoinDirection
      // cref: ArcGIS.Core.Data.JoinType
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      // cref: ArcGIS.Core.Data.Table.RelateTo(ArcGIS.Core.Data.Table,ArcGIS.Core.Data.VirtualRelationshipClassDescription)
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      #region Joining two tables from different geodatabases
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase sourceGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("Path \\ to \\Geodatabase \\ one"))))
          using (Geodatabase destinationGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("Path \\ to \\Geodatabase \\ two"))))
          using (Table sourceTable = sourceGeodatabase.OpenDataset<Table>("State"))
          using (Table destinationTable = destinationGeodatabase.OpenDataset<Table>("Cities"))
          {
            Field primaryKeyField = sourceTable.GetDefinition().GetFields()
              .FirstOrDefault(field => field.Name.Equals("State.State_Abbreviation"));
            Field foreignKeyField = destinationTable.GetDefinition().GetFields()
              .FirstOrDefault(field => field.Name.Equals("Cities.State"));

            VirtualRelationshipClassDescription virtualRelationshipClassDescription = new VirtualRelationshipClassDescription(primaryKeyField, foreignKeyField,
                RelationshipCardinality.OneToMany);

            using (RelationshipClass relationshipClass = sourceTable.RelateTo(destinationTable, virtualRelationshipClassDescription))
            {
              JoinDescription joinDescription = new JoinDescription(relationshipClass)
              {
                JoinDirection = JoinDirection.Forward,
                JoinType = JoinType.InnerJoin,
                TargetFields = sourceTable.GetDefinition().GetFields()
              };

              using (Join join = new Join(joinDescription))
              {
                Table joinedTable = join.GetJoinedTable();

                //Process the joined table. For example ..
                using (RowCursor cursor = joinedTable.Search())
                {
                  while (cursor.MoveNext())
                  {
                    using (Row row = cursor.Current)
                    {
                      // Use Row
                    }
                  }
                }
              }
            }
          }
        });
      }
      #endregion Joining two tables from different geodatabases

      // cref: ArcGIS.Core.Data.QueryTableDescription
      // cref: ArcGIS.Core.Data.QueryTableDescription.Name
      // cref: ArcGIS.Core.Data.QueryTableDescription.PrimaryKeys
      // cref: ArcGIS.Core.Data.Geodatabase.OpenQueryTable(ArcGIS.Core.Data.QueryTableDescription)
      // cref: ArcGIS.Core.Data.QueryDef
      // cref: ArcGIS.Core.Data.QueryDef.Tables
      // cref: ArcGIS.Core.Data.QueryDef.SubFields
      // cref: ArcGIS.Core.Data.SQLSyntax.QualifyColumnName(System.String,System.String)
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.AuthenticationMode
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Database
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Instance
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Password
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.User
      // cref: ArcGIS.Core.Data.DatabaseConnectionProperties.Version
      // cref: ArcGIS.Core.Data.Datastore.GetSQLSyntax()
      #region Creating a QueryTable using a query which joins two versioned tables in a geodatabase
      {
        await QueuedTask.Run(() =>
        {
          QueryDef queryDef = new QueryDef
          {
            Tables = "CommunityAddress JOIN MunicipalBoundary on CommunityAddress.Municipality = MunicipalBoundary.Name",
            SubFields = "CommunityAddress.OBJECTID, CommunityAddress.Shape, CommunityAddress.SITEADDID, CommunityAddress.ADDRNUM, CommunityAddress.FULLNAME, CommunityAddress.FULLADDR, CommunityAddress.MUNICIPALITY, MunicipalBoundary.Name, MunicipalBoundary.MUNITYP, MunicipalBoundary.LOCALFIPS",
          };

          using (Geodatabase testVersion1Geodatabase = new Geodatabase(new DatabaseConnectionProperties(EnterpriseDatabaseType.Oracle)
          {
            AuthenticationMode = AuthenticationMode.DBMS,
            Instance = "instance",
            User = "user",
            Password = "password",
            Database = "database",
            Version = "user.testVersion1"
          }))
          {
            QueryTableDescription queryTableDescription = new QueryTableDescription(queryDef)
            {
              Name = "CommunityAddrJounMunicipalBoundr",
              PrimaryKeys = testVersion1Geodatabase.GetSQLSyntax().QualifyColumnName("CommunityAddress", "OBJECTID")
            };

            // Will be based on testVersion1.
            using (Table queryTable = testVersion1Geodatabase.OpenQueryTable(queryTableDescription))
            {
              // Use queryTable.
            }
          }

          using (Geodatabase testVersion2Geodatabase = new Geodatabase(new DatabaseConnectionProperties(EnterpriseDatabaseType.Oracle)
          {
            AuthenticationMode = AuthenticationMode.DBMS,
            Instance = "instance",
            User = "user",
            Password = "password",
            Database = "database",
            Version = "user.testVersion2"
          }))
          {
            QueryTableDescription queryTableDescription = new QueryTableDescription(queryDef)
            {
              Name = "CommunityAddrJounMunicipalBoundr",
              PrimaryKeys = testVersion2Geodatabase.GetSQLSyntax().QualifyColumnName("CommunityAddress", "OBJECTID")
            };

            // Will be based on testVersion2.
            using (Table queryTable = testVersion2Geodatabase.OpenQueryTable(queryTableDescription))
            {
              // Use queryTable.
            }
          }
        });
      }
      #endregion Creating a QueryTable using a query which joins two versioned tables in a geodatabase

      // cref: ArcGIS.Core.Data.Field.Name
      #region Checking a field value for null
      {
        Task<bool> CheckColumnForNullAsync(Row row, Field field) => QueuedTask.Run(() =>
        {
          object val = row[field.Name];
          if (val is DBNull || val == null)
          {
            return true;
          }
          return false;
        });
      }
      #endregion Checking a field value for null

      // cref: ArcGIS.Core.Data.Field.GetDomain(ArcGIS.Core.Data.Subtype)
      // cref: ArcGIS.Core.Data.TableDefinition.GetSubtypes()
      // cref: ArcGIS.Core.Data.Subtype
      // cref: ArcGIS.Core.Data.CodedValueDomain
      // cref: ArcGIS.Core.Data.CodedValueDomain.GetName(System.Object)
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.Row.GetTable()
      // cref: ArcGIS.Core.Data.Subtype.GetCode()
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      // cref: ArcGIS.Core.Data.TableDefinition.GetSubtypeField()
      #region Get domain string from a field
      {
        Task<string> GetDomainStringFromFieldAsync(Row row, Field field) => QueuedTask.Run(() =>
        {
          // Get the table and table definition from the Row
          using (Table table = row.GetTable())
          using (TableDefinition tableDefinition = table.GetDefinition())
          {
            // Get name of subtype field
            string subtypeFieldName = tableDefinition.GetSubtypeField();

            // Get subtype, if any
            Subtype subtype = null;

            if (subtypeFieldName.Length != 0)
            {
              // Get value of subtype field for this row
              object varSubtypeCode = row[subtypeFieldName];
              long subtypeCode = (long)varSubtypeCode;

              // Get subtype for this row
              subtype = tableDefinition.GetSubtypes().First(x => x.GetCode() == subtypeCode);
            }

            // Get the coded value domain for this field
            CodedValueDomain domain = field.GetDomain(subtype) as CodedValueDomain;

            // Return the text string for this field
            if (domain != null)
            {
              return domain.GetName(row[field.Name]);
            }

            return row[field.Name].ToString();
          }
        });
      }
      #endregion Get domain string from a field

      // cref: ArcGIS.Core.Data.DatastoreProperties
      // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsBigInteger
      // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsQueryPagination
      // cref: ArcGIS.Core.Data.DatastoreProperties.CanEdit
      // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsBigObjectID
      // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsDateOnly
      // cref: ArcGIS.Core.Data.Datastore.GetDatastoreProperties()
      // cref: ArcGIS.Core.Data.Datastore.AreDatastorePropertiesSupported()
      // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsTimeOnly
      // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsTimestampOffset
      #region Get datastore or workspace properties
      {
        Task GetDatastorePropertiesAsync(Datastore geodatabase) => QueuedTask.Run(() =>
        {
          // Check if a data store supports datastore properties
          bool areDatastorePropertiesSupported = geodatabase.AreDatastorePropertiesSupported();

          if (areDatastorePropertiesSupported)
          {
            DatastoreProperties datastoreProperties = geodatabase.GetDatastoreProperties();

            // Supports 64-bit integer field
            bool supportsBigInteger = datastoreProperties.SupportsBigInteger;

            // Supports pagination
            bool supportsQueryPagination = datastoreProperties.SupportsQueryPagination;

            // Supports datastore edit
            bool canEdit = datastoreProperties.CanEdit;

            // Supports 64-bit Object ID
            bool supportsBigObjectId = datastoreProperties.SupportsBigObjectID;

            // Supports DateOnly field
            bool supportsDateOnly = datastoreProperties.SupportsDateOnly;

            // Supports TimeOnly field
            bool supportsTimeOnly = datastoreProperties.SupportsTimeOnly;

            // Supports TimestampOffset field
            bool supportsTimestampOffset = datastoreProperties.SupportsTimestampOffset;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.QueryFilter.Offset
      // cref: ArcGIS.Core.Data.QueryFilter.RowCount
      // cref: ArcGIS.Core.Data.QueryFilter.ObjectIDs
      // cref: ArcGIS.Core.Data.QueryFilter.PostfixClause
      // cref: ArcGIS.Core.Data.Row.GetObjectID()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Pagination in QueryFilter
      {
        Task QueryFilterWithPaginationAsync(Table table, List<long> objectIDs) => QueuedTask.Run(() =>
        {
          int rowsPerBatch = 100;
          int offset = 0;

          // Query filter
          // Some datastores support pagination only through an SQL postfix clause
          QueryFilter queryFilter = new QueryFilter()
          {
            ObjectIDs = objectIDs,
            PostfixClause = "ORDER BY OBJECTID"
          };

          // Fetch rows in a batch from a table
          for (int index = offset; index <= objectIDs.Count; index += rowsPerBatch)
          {
            // Set number of rows to return from a table
            queryFilter.RowCount = rowsPerBatch;

            // Set positional offset to skip number of rows from a table
            queryFilter.Offset = index;

            using (RowCursor cursor = table.Search(queryFilter))
            {
              while (cursor.MoveNext())
              {
                using (Row row = cursor.Current)
                {
                  Console.WriteLine(row.GetObjectID());
                }
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Conflict
      // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions)
      // cref: ArcGIS.Core.Data.ConflictResolutionMethod
      // cref: ArcGIS.Core.Data.ConflictDetectionType
      // cref: ArcGIS.Core.Data.ConflictResolutionType
      // cref: ArcGIS.Core.Data.ReconcileResult
      // cref: ArcGIS.Core.Data.ReconcileResult.HasConflicts
      // cref: ArcGIS.Core.Data.Version.GetConflicts()
      // cref: ArcGIS.Core.Data.Version.HasConflicts()
      // cref: ArcGIS.Core.Data.Conflict.AncestorVersionValues
      // cref: ArcGIS.Core.Data.Conflict.ChildVersionValues
      // cref: ArcGIS.Core.Data.Conflict.ConflictType
      // cref: ArcGIS.Core.Data.Conflict.DatasetName
      // cref: ArcGIS.Core.Data.Conflict.ObjectID
      // cref: ArcGIS.Core.Data.Conflict.ParentVersionValues
      // cref: ArcGIS.Core.Data.FeatureClass.CreateRow(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.FeatureClassDefinition.GetShapeField()
      // cref: ArcGIS.Core.Data.FieldValue.FieldName
      // cref: ArcGIS.Core.Data.FieldValue.Value
      // cref: ArcGIS.Core.Data.Geodatabase.ApplyEdits(System.Action)
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.ObjectIDs
      // cref: ArcGIS.Core.Data.Row.GetObjectID()
      // cref: ArcGIS.Core.Data.Row.Store()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      // cref: ArcGIS.Core.Data.Table.DeleteRows(ArcGIS.Core.Data.QueryFilter)
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      // cref: ArcGIS.Core.Data.Version.Connect()
      // cref: ArcGIS.Core.Data.VersionManager.CreateVersion(ArcGIS.Core.Data.VersionDescription)
      // cref: ArcGIS.Core.Data.VersionManager.GetDefaultVersion()
      #region Illustrate version conflict information from a reconcile operation
      {
        Task GetVersionConflictsInfoInUpdateDeleteTypeAsync(
          ServiceConnectionProperties featureServiceConnectionProperties,
          string featureClassName) => QueuedTask.Run(() =>
        {
          // To illustrate the conflict between versions,
          // the feature is updated in the child version and deleted in the parent version.

          long featureObjectIDForEdit = Int64.MinValue;

          // Get branch versioned service
          using (Geodatabase fsGeodatabase = new Geodatabase(featureServiceConnectionProperties))
          using (VersionManager versionManager = fsGeodatabase.GetVersionManager())
          using (Version defaultVersion = versionManager.GetDefaultVersion())
          using (Geodatabase defaultGeodatabase = defaultVersion.Connect())
          using (FeatureClass defaultFeatureClass = defaultGeodatabase.OpenDataset<FeatureClass>(featureClassName))
          using (FeatureClassDefinition defaultFeatureClassDefinition = defaultFeatureClass.GetDefinition())
          {
            // Create a feature in the default version to edit in a branch
            defaultGeodatabase.ApplyEdits(() =>
            {
              using (RowBuffer rowBuffer = defaultFeatureClass.CreateRowBuffer())
              {
                rowBuffer["NAME"] = "Loblolly Pine";
                rowBuffer["TREEAGE"] = 1;
                rowBuffer[defaultFeatureClassDefinition.GetShapeField()] = new MapPointBuilderEx(new Coordinate2D(1, 1),
                  SpatialReferenceBuilder.CreateSpatialReference(4152, 0)).ToGeometry();

                using (Feature feature = defaultFeatureClass.CreateRow(rowBuffer))
                {
                  featureObjectIDForEdit = feature.GetObjectID();
                }
              }
            });
            // Add newly created feature in the filter
            QueryFilter queryFilter = new QueryFilter { ObjectIDs = new List<long> { featureObjectIDForEdit } };

            // Create a branch version
            VersionDescription versionDescription = new VersionDescription("UpdateDeleteConflictType",
              "Update-Delete version conflict type", VersionAccessType.Public);

            // Edit the feature in the branch
            using (Version editVersion = versionManager.CreateVersion(versionDescription))
            using (Geodatabase branchGeodatabase = editVersion.Connect())
            using (FeatureClass featureClass = branchGeodatabase.OpenDataset<FeatureClass>(featureClassName))
            using (RowCursor rowCursor = featureClass.Search(queryFilter, false))
            {
              branchGeodatabase.ApplyEdits(() =>
              {
                while (rowCursor.MoveNext())
                {
                  using (Row row = rowCursor.Current)
                  {
                    row["TREEAGE"] = 100;
                    row["NAME"] = $"{row["Name"]}_EditInBranch";
                    row.Store();
                  }
                }
              });

              // Delete the feature from the default version
              defaultGeodatabase.ApplyEdits(() =>
              {
                defaultFeatureClass.DeleteRows(queryFilter);
              });

              // Reconcile options
              ReconcileOptions reconcileOptions = new ReconcileOptions(defaultVersion)
              {
                ConflictResolutionType = ConflictResolutionType.FavorEditVersion,
                ConflictDetectionType = ConflictDetectionType.ByRow,
                ConflictResolutionMethod = ConflictResolutionMethod.Continue
              };

              // Reconcile with default
              ReconcileResult reconcileResult = editVersion.Reconcile(reconcileOptions);

              // Check for conflicts
              bool hasConflictsReconcileResults = reconcileResult.HasConflicts;
              bool hasConflictsAfterReconcile = editVersion.HasConflicts();

              // Fetch conflicts
              IReadOnlyList<Conflict> conflictsAfterReconcile = editVersion.GetConflicts();

              // Iterate conflicts
              foreach (Conflict conflict in conflictsAfterReconcile)
              {
                // Object ID of row where conflict occurs
                long objectId = conflict.ObjectID;

                ConflictType conflictType = conflict.ConflictType;

                IReadOnlyList<FieldValue> ancestorVersionValues = conflict.AncestorVersionValues;
                object nameAncestor = ancestorVersionValues.FirstOrDefault(f => f.FieldName.Contains("NAME")).Value;
                object treeAgeAncestor =
                  ancestorVersionValues.FirstOrDefault(f => f.FieldName.Contains("TREEAGE")).Value;

                IReadOnlyList<FieldValue> childVersionValues = conflict.ChildVersionValues;
                object nameChild = childVersionValues.FirstOrDefault(f => f.FieldName.Contains("NAME")).Value;
                object treeAgeChild = childVersionValues.FirstOrDefault(f => f.FieldName.Contains("TREEAGE")).Value;

                IReadOnlyList<FieldValue> parentVersionValues = conflict.ParentVersionValues;

                IReadOnlyList<Field> originalFields = defaultFeatureClassDefinition.GetFields();

                string datasetName = conflict.DatasetName;
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.ContingentCodedValue
      // cref: ArcGIS.Core.Data.ContingentAnyValue
      // cref: ArcGIS.Core.Data.ContingentNullValue
      // cref: ArcGIS.Core.Data.ContingentRangeValue
      // cref: ArcGIS.Core.Data.TableDefinition.GetContingencies()
      // cref: ArcGIS.Core.Data.Contingency.FieldGroup
      // cref: ArcGIS.Core.Data.Contingency.GetContingentValues()
      // cref: ArcGIS.Core.Data.Contingency.ID
      // cref: ArcGIS.Core.Data.Contingency.IsRetired
      // cref: ArcGIS.Core.Data.Contingency.Subtype
      // cref: ArcGIS.Core.Data.ContingentCodedValue.CodedValue
      // cref: ArcGIS.Core.Data.ContingentCodedValue.Name
      // cref: ArcGIS.Core.Data.ContingentRangeValue.Max
      // cref: ArcGIS.Core.Data.ContingentRangeValue.Min
      // cref: ArcGIS.Core.Data.FieldGroup.FieldNames
      // cref: ArcGIS.Core.Data.FieldGroup.IsRestrictive
      // cref: ArcGIS.Core.Data.FieldGroup.Name
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      #region Explore contingent attribute values
      {
        Task ExploreContingentValuesAsync(Table table) => QueuedTask.Run(() =>
        {
          using (TableDefinition tableDefinition = table.GetDefinition())
          {
            IReadOnlyList<Contingency> contingencies = tableDefinition.GetContingencies();
            foreach (Contingency contingency in contingencies)
            {
              // Field group
              FieldGroup filedGroup = contingency.FieldGroup;
              string fieldGroupName = filedGroup.Name;
              IReadOnlyList<string> fieldInFieldGroup = filedGroup.FieldNames;
              bool isEditRestriction = filedGroup.IsRestrictive;

              int contingencyId = contingency.ID;
              Subtype subtype = contingency.Subtype;
              bool isContingencyRetired = contingency.IsRetired;

              // Contingent values
              IReadOnlyDictionary<string, ContingentValue> contingentValuesByFieldName =
                contingency.GetContingentValues();
              foreach (KeyValuePair<string, ContingentValue> contingentValueKeyValuePair in contingentValuesByFieldName)
              {
                string attributeFieldName = contingentValueKeyValuePair.Key;

                // Contingent value type associated with the attribute field
                ContingentValue contingentValue = contingentValueKeyValuePair.Value;

                switch (contingentValue)
                {
                  case ContingentCodedValue contingentCodedValue:
                    string codedValueDomainName = contingentCodedValue.Name;
                    object codedValueDomainValue = contingentCodedValue.CodedValue;
                    break;
                  case ContingentRangeValue contingentRangeValue:
                    object rangeDomainMaxValue = contingentRangeValue.Max;
                    object rangeDomainMinValue = contingentRangeValue.Min;
                    break;
                  case ContingentAnyValue contingentAnyValue:
                    // Any value type
                    break;
                  case ContingentNullValue contingentNullValue:
                    // Null value
                    break;
                }
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.ContingencyValidationResult.Matches
      // cref: ArcGIS.Core.Data.ContingencyValidationResult.Violations
      // cref: ArcGIS.Core.Data.Table.ValidateContingencies(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.ContingencyViolation.Contingency
      // cref: ArcGIS.Core.Data.ContingencyViolation.Type
      // cref: ArcGIS.Core.Data.FeatureClass.CreateRow(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      #region Validate contingent attribute values
      {
        Task ValidateContingentValuesAsync(FeatureClass parcels, string zoningFieldName = "Zone", string taxCodeFieldName = "TaxCode") => QueuedTask.Run(() =>
        {
          using (RowBuffer rowBuffer = parcels.CreateRowBuffer())
          {
            // Insert values in a row buffer
            rowBuffer[zoningFieldName] = "Business";
            rowBuffer[taxCodeFieldName] = "TaxB";

            // Validate contingency values of the parcels' row
            ContingencyValidationResult contingencyValidationResult = parcels.ValidateContingencies(rowBuffer);

            // Valid contingencies
            IReadOnlyList<Contingency> matchedContingencies = contingencyValidationResult.Matches;
            if (matchedContingencies.Count > 0)
            {
              // Create a row with valid contingency values
              parcels.CreateRow(rowBuffer);
            }

            // Invalid contingencies
            IReadOnlyList<ContingencyViolation> violatedContingencies = contingencyValidationResult.Violations;
            foreach (ContingencyViolation contingencyViolation in violatedContingencies)
            {
              ContingencyViolationType violationType = contingencyViolation.Type;
              Contingency violatedContingency = contingencyViolation.Contingency;
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.ContingentCodedValue
      // cref: ArcGIS.Core.Data.ContingentAnyValue
      // cref: ArcGIS.Core.Data.ContingentNullValue
      // cref: ArcGIS.Core.Data.ContingentRangeValue
      // cref: ArcGIS.Core.Data.Table.GetContingentValues(ArcGIS.Core.Data.RowBuffer,System.String)
      // cref: ArcGIS.Core.Data.ContingentCodedValue.CodedValue
      // cref: ArcGIS.Core.Data.ContingentCodedValue.Name
      // cref: ArcGIS.Core.Data.ContingentRangeValue.Max
      // cref: ArcGIS.Core.Data.ContingentRangeValue.Min
      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      #region Get possible contingent values
      {
        Task GetPossibleContingentValuesAsync(FeatureClass parcels, string zoningFieldName = "Zone") => QueuedTask.Run(() =>
        {
          using (RowBuffer rowBuffer = parcels.CreateRowBuffer())
          {
            IReadOnlyDictionary<FieldGroup, IReadOnlyList<ContingentValue>> possibleZonings = parcels.GetContingentValues(rowBuffer, zoningFieldName);
            IEnumerable<FieldGroup> possibleFieldGroups = possibleZonings.Keys;

            foreach (FieldGroup possibleFieldGroup in possibleFieldGroups)
            {
              IReadOnlyList<ContingentValue> possibleZoningValues = possibleZonings[possibleFieldGroup];
              foreach (ContingentValue possibleZoningValue in possibleZoningValues)
              {
                switch (possibleZoningValue)
                {
                  case ContingentCodedValue codedValue:
                    string codedValueDomainName = codedValue.Name;
                    object codedValueDomainValue = codedValue.CodedValue;
                    break;
                  case ContingentRangeValue rangeValue:
                    object rangeDomainMaxValue = rangeValue.Max;
                    object rangeDomainMinValue = rangeValue.Min;
                    break;
                  case ContingentAnyValue contingentAnyValue:
                    // Any value type
                    break;
                  case ContingentNullValue contingentNullValue:
                    // Null value
                    break;
                }
              }
            }
          }
        });
      }
      #endregion

      #region ProSnippet Group: Editing
      #endregion

      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      // cref: ArcGIS.Core.Data.Table.CreateRow(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Callback(System.Action{ArcGIS.Desktop.Editing.EditOperation.IEditContext},ArcGIS.Core.Data.Dataset)
      // cref: ArcGIS.Desktop.Editing.EditOperation.IEditContext.Invalidate(ArcGIS.Core.Data.Row)
      // cref: ArcGIS.Core.Data.RowBuffer
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      // cref: ArcGIS.Core.Data.TableDefinition.FindField(System.String)
      // cref: ArcGIS.Core.Data.TableDefinition.GetSubtypeField()
      #region Creating a Row
      {
        await QueuedTask.Run(() =>
        {
          string message = String.Empty;
          bool creationResult = false;
          EditOperation editOperation = new EditOperation();

          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
          {
            // Declare the callback here. We are not executing it .yet.
            editOperation.Callback(context =>
            {
              TableDefinition tableDefinition = enterpriseTable.GetDefinition();
              int assetNameIndex = tableDefinition.FindField("ASSETNA");

              using (RowBuffer rowBuffer = enterpriseTable.CreateRowBuffer())
              {
                // Either the field index or the field name can be used in the indexer.
                rowBuffer[assetNameIndex] = "wMain";
                rowBuffer["COST"] = 700;
                rowBuffer["ACTION"] = "Open Cut";

                // subtype value for "Abandon".
                rowBuffer[tableDefinition.GetSubtypeField()] = 3;

                using (Row row = enterpriseTable.CreateRow(rowBuffer))
                {
                  // To Indicate that the attribute table has to be updated.
                  context.Invalidate(row);
                }
              }
            }, enterpriseTable);

            try
            {
              creationResult = editOperation.Execute();
              if (!creationResult)
              {
                message = editOperation.ErrorMessage;
              }
            }
            catch (GeodatabaseException exObj)
            {
              message = exObj.Message;
            }
          }

          if (!string.IsNullOrEmpty(message))
          {
            Debug.WriteLine(message);
          }
        });
      }
      #endregion Creating a Row

      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      // cref: ArcGIS.Core.Data.FeatureClass.CreateRow(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.RowBuffer
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.FeatureClassDefinition.GetShapeField()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.TableDefinition.FindField(System.String)
      // cref: ArcGIS.Core.Data.TableDefinition.GetSubtypeField()
      #region Creating a Feature
      {
        await QueuedTask.Run(() =>
        {
          string message = String.Empty;
          bool creationResult = false;
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
          {
            //Declare the callback here. We are not executing it yet
            EditOperation editOperation = new EditOperation();
            editOperation.Callback(context =>
            {
              FeatureClassDefinition facilitySiteDefinition = enterpriseFeatureClass.GetDefinition();
              int facilityIdIndex = facilitySiteDefinition.FindField("FACILITYID");

              using (RowBuffer rowBuffer = enterpriseFeatureClass.CreateRowBuffer())
              {
                // Either the field index or the field name can be used in the indexer.
                rowBuffer[facilityIdIndex] = "wMain";
                rowBuffer["NAME"] = "Griffith Park";
                rowBuffer["OWNTYPE"] = "Municipal";
                rowBuffer["FCODE"] = "Park";
                // Add it to   Attractions Subtype.
                rowBuffer[facilitySiteDefinition.GetSubtypeField()] = 820;

                List<Coordinate2D> newCoordinates = new List<Coordinate2D>
                {
                    new Coordinate2D(1021570, 1880583),
                    new Coordinate2D(1028730, 1880994),
                    new Coordinate2D(1029718, 1875644),
                    new Coordinate2D(1021405, 1875397)
                };

                rowBuffer[facilitySiteDefinition.GetShapeField()] = new PolygonBuilderEx(newCoordinates).ToGeometry();

                using (Feature feature = enterpriseFeatureClass.CreateRow(rowBuffer))
                {
                  //To Indicate that the attribute table has to be updated
                  context.Invalidate(feature);
                }
              }
            }, enterpriseFeatureClass);

            try
            {
              creationResult = editOperation.Execute();
              if (!creationResult)
              {
                message = editOperation.ErrorMessage;
              }
            }
            catch (GeodatabaseException exObj)
            {
              message = exObj.Message;
            }
          }

          if (!string.IsNullOrEmpty(message))
          {
            Debug.WriteLine(message);
          }
        });
      }
      #endregion Creating a Feature

      // cref: ArcGIS.Desktop.Editing.EditOperation.IEditContext.Invalidate(ArcGIS.Core.Data.Row)
      // cref: ArcGIS.Core.Data.Row.Store()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.GetDefinition()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.TableDefinition.FindField(System.String)
      // cref: ArcGIS.Core.Data.TableDefinition.GetSubtypeField()
      #region Modifying a Row
      {
        await QueuedTask.Run(() =>
        {
          string message = String.Empty;
          bool modificationResult = false;
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
          {
            EditOperation editOperation = new EditOperation();
            editOperation.Callback(context =>
            {
              QueryFilter openCutFilter = new QueryFilter { WhereClause = "ACTION = 'Open Cut'" };

              using (RowCursor rowCursor = enterpriseTable.Search(openCutFilter, false))
              {
                TableDefinition tableDefinition = enterpriseTable.GetDefinition();
                int subtypeFieldIndex = tableDefinition.FindField(tableDefinition.GetSubtypeField());

                while (rowCursor.MoveNext())
                {
                  using (Row row = rowCursor.Current)
                  {
                    // In order to update the Map and/or the attribute table.
                    // Has to be called before any changes are made to the row.
                    context.Invalidate(row);

                    row["ASSETNA"] = "wMainOpenCut";

                    if (Convert.ToDouble(row["COST"]) > 700)
                    {
                      // Abandon asset if cost is higher than 700 (if that is what you want to do).
                      row["ACTION"] = "Open Cut Abandon";
                      row[subtypeFieldIndex] = 3; //subtype value for "Abandon"
                    }

                    //After all the changes are done, persist it.
                    row.Store();

                    // Has to be called after the store too.
                    context.Invalidate(row);
                  }
                }
              }
            }, enterpriseTable);

            try
            {
              modificationResult = editOperation.Execute();
              if (!modificationResult)
              {
                message = editOperation.ErrorMessage;
              }
            }
            catch (GeodatabaseException exObj)
            {
              message = exObj.Message;
            }
          }

          if (!string.IsNullOrEmpty(message))
          {
            Debug.WriteLine(message);
          }
        });
      }
      #endregion Modifying a Row

      // cref: ArcGIS.Desktop.Editing.EditOperation.IEditContext.Invalidate(ArcGIS.Core.Data.Row)
      // cref: ArcGIS.Core.Data.Row.Store()
      // cref: ArcGIS.Core.Data.Feature.SetShape(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.FeatureClassDefinition.GetAreaField()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.TableDefinition.FindField(System.String)
      #region Modifying a Feature
      {
        await QueuedTask.Run(() =>
        {
          string message = String.Empty;
          bool modificationResult = false;

          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
          {
            FeatureClassDefinition facilitySiteDefinition = enterpriseFeatureClass.GetDefinition();

            int ownTypeIndex = facilitySiteDefinition.FindField("OWNTYPE");
            int areaIndex = facilitySiteDefinition.FindField(facilitySiteDefinition.GetAreaField());

            EditOperation editOperation = new EditOperation();
            editOperation.Callback(context =>
            {
              QueryFilter queryFilter = new QueryFilter
              {
                WhereClause = "FCODE = 'Hazardous Materials Facility' AND OWNTYPE = ' '"
              };

              using (RowCursor rowCursor = enterpriseFeatureClass.Search(queryFilter, false))
              {
                while (rowCursor.MoveNext())
                {
                  using (Feature feature = (Feature)rowCursor.Current)
                  {
                    // In order to update the Map and/or the attribute table.
                    // Has to be called before any changes are made to the row
                    context.Invalidate(feature);

                    // Transfer all Hazardous Material Facilities to the City.
                    feature[ownTypeIndex] = "Municipal";

                    if (Convert.ToDouble(feature[areaIndex]) > 50000)
                    {
                      // Set the Shape of the feature to whatever you need.
                      List<Coordinate2D> newCoordinates = new List<Coordinate2D>
                      {
                          new Coordinate2D(1021570, 1880583),
                          new Coordinate2D(1028730, 1880994),
                          new Coordinate2D(1029718, 1875644),
                          new Coordinate2D(1021405, 1875397)
                      };

                      feature.SetShape(new PolygonBuilderEx(newCoordinates).ToGeometry());
                    }

                    feature.Store();

                    // Has to be called after the store too
                    context.Invalidate(feature);
                  }
                }
              }
            }, enterpriseFeatureClass);

            try
            {
              modificationResult = editOperation.Execute();
              if (!modificationResult)
              {
                message = editOperation.ErrorMessage;
              }
            }
            catch (GeodatabaseException exObj)
            {
              message = exObj.Message;
            }
          }

          if (!string.IsNullOrEmpty(message))
          {
            Debug.WriteLine(message);
          }
        });
      }
      #endregion Modifying a Feature

      // cref: ArcGIS.Core.Data.Field.Name
      #region Writing a value into a Guid column
      {
        Task WritingIntoGuidColumnAsync(Row row, Field field, Guid guid) => QueuedTask.Run(() =>
        {
          row[field.Name] = "{" + guid.ToString() + "}";
        });
      }
      #endregion Writing a value into a Guid column

      // cref: ArcGIS.Desktop.Editing.EditOperation.IEditContext.Invalidate(ArcGIS.Core.Data.Row)
      // cref: ArcGIS.Core.Data.Row.Delete()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Deleting a Row/Feature
      {
        await QueuedTask.Run(() =>
        {
          string message = String.Empty;
          bool deletionResult = false;

          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
          {
            EditOperation editOperation = new EditOperation();
            editOperation.Callback(context =>
            {
              QueryFilter openCutFilter = new QueryFilter { WhereClause = "ACTION = 'Open Cut'" };

              using (RowCursor rowCursor = enterpriseTable.Search(openCutFilter, false))
              {
                while (rowCursor.MoveNext())
                {
                  using (Row row = rowCursor.Current)
                  {
                    // In order to update the Map and/or the attribute table. Has to be called before the delete.
                    context.Invalidate(row);

                    row.Delete();
                  }
                }
              }
            }, enterpriseTable);

            try
            {
              deletionResult = editOperation.Execute();
              if (!deletionResult)
              {
                message = editOperation.ErrorMessage;
              }
            }
            catch (GeodatabaseException exObj)
            {
              message = exObj.Message;
            }
          }

          if (!string.IsNullOrEmpty(message))
          {
            Debug.WriteLine(message);
          }
        });
      }
      #endregion Deleting a Row/Feature

      // cref: ArcGIS.Core.Data.Feature.Split(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Data.QueryFilter.ObjectIDs
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Split a feature by geometry
      {
        Task SplitALineByPointAsync(FeatureClass lineFeatureClass, MapPoint xPoint) => QueuedTask.Run(() =>
        {
          using (RowCursor rowCursor =
                 lineFeatureClass.Search(new QueryFilter() { ObjectIDs = new List<long>() { 1 } }))
          {
            if (rowCursor.MoveNext())
            {
              using (Feature feature = rowCursor.Current as Feature)
              {
                // ObjectIDs of newly created lines
                IReadOnlyList<long> splits = feature.Split(xPoint);
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Row.AddAttachment(ArcGIS.Core.Data.Attachment)
      // cref: ArcGIS.Core.Data.Attachment
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Adding Attachments
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (FeatureClass parkFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.Park"))
          {
            QueryFilter filter = new QueryFilter { WhereClause = "NUMPARKING > 0" };

            using (RowCursor parkingCursor = parkFeatureClass.Search(filter, false))
            {
              while (parkingCursor.MoveNext())
              {
                using (MemoryStream stream = CreateMemoryStreamFromContentsOf("Sample.xml"))
                {
                  Attachment attachment = new Attachment("Sample.xml", "text/xml", stream);

                  using (Row row = parkingCursor.Current)
                  {
                    long attachmentId = row.AddAttachment(attachment);
                  }
                }
              }
            }
          }
        });
        // Helper method to read a file into a memory stream
        MemoryStream CreateMemoryStreamFromContentsOf(String fileNameWithPath)
        {
          MemoryStream memoryStream = new MemoryStream();

          using (FileStream file = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read))
          {
            byte[] bytes = new byte[file.Length];
            file.Read(bytes, 0, (int)file.Length);
            memoryStream.Write(bytes, 0, (int)file.Length);
          }

          return memoryStream;
        }
      }
      #endregion Adding Attachments

      // cref: ArcGIS.Core.Data.Row.UpdateAttachment(ArcGIS.Core.Data.Attachment)
      // cref: ArcGIS.Core.Data.Row.GetAttachments(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
      // cref: ArcGIS.Core.Data.Attachment
      // cref: ArcGIS.Core.Data.Attachment.SetName(System.String)
      // cref: ArcGIS.Core.Data.Attachment.GetName()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Updating Attachments
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (FeatureClass landUseCaseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.LandUseCase"))
          {
            QueryFilter filter = new QueryFilter { WhereClause = "CASETYPE = 'Rezoning'" };

            using (RowCursor landUseCursor = landUseCaseFeatureClass.Search(filter, false))
            {
              while (landUseCursor.MoveNext())
              {
                using (Feature rezoningUseCase = (Feature)landUseCursor.Current)
                {
                  IReadOnlyList<Attachment> rezoningAttachments = rezoningUseCase.GetAttachments();
                  IEnumerable<Attachment> filteredAttachments = rezoningAttachments.Where(attachment => !attachment.GetName().Contains("rezoning"));

                  foreach (Attachment attachmentToUpdate in filteredAttachments)
                  {
                    attachmentToUpdate.SetName(attachmentToUpdate.GetName().Replace(".pdf", "Rezoning.pdf"));
                    rezoningUseCase.UpdateAttachment(attachmentToUpdate);
                  }
                }
              }
            }
          }
        });
      }
      #endregion Updating Attachments

      // cref: ArcGIS.Core.Data.Row.DeleteAttachments(System.Collections.Generic.IEnumerable{System.Int64})
      // cref: ArcGIS.Core.Data.Row.GetAttachments(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
      // cref: ArcGIS.Core.Data.Attachment.GetAttachmentID()
      // cref: ArcGIS.Core.Data.Attachment.GetContentType()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Deleting Attachments
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
          using (Table inspectionTable = geodatabase.OpenDataset<Table>("luCodeInspection"))
          {
            QueryFilter queryFilter = new QueryFilter { WhereClause = "ACTION = '1st Notice'" };

            using (RowCursor cursor = inspectionTable.Search(queryFilter, false))
            {
              while (cursor.MoveNext())
              {
                using (Row currentRow = cursor.Current)
                {
                  IReadOnlyList<Attachment> rowAttachments = currentRow.GetAttachments(null, true);
                  IEnumerable<Attachment> attachments = rowAttachments.Where(attachment =>
                    attachment.GetContentType().Equals("application/pdf"));

                  IReadOnlyList<long> attachmentIDs =
                    attachments.Select(attachment => attachment.GetAttachmentID()) as IReadOnlyList<long>;
                  IReadOnlyDictionary<long, Exception> failures = currentRow.DeleteAttachments(attachmentIDs);

                  if (failures.Count > 0)
                  {
                    //process errors
                  }
                }
              }
            }
          }
        });
      }
      #endregion Deleting Attachments

      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      // cref: ArcGIS.Core.Data.Table.CreateRow(ArcGIS.Core.Data.RowBuffer)
      #region Writing a Blob field
      {
        Task WriteBlobFieldAsync(Table table, string blobFieldName, string imageFileName) => QueuedTask.Run(() =>
        {
          // Read the image file into a MemoryStream
          MemoryStream memoryStream = new MemoryStream();

          using (FileStream imageFile = new FileStream(imageFileName, FileMode.Open, FileAccess.Read))
          {
            imageFile.CopyTo(memoryStream);
          }

          // Create a new row in the table, and write the Memory Stream into a blob fiele
          using (RowBuffer rowBuffer = table.CreateRowBuffer())
          {
            rowBuffer[blobFieldName] = memoryStream;
            table.CreateRow(rowBuffer).Dispose();
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Row
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Reading a Blob field
      {
        Task ReadBlobFieldAsync(Table table, QueryFilter queryFilter, string blobFieldName) => QueuedTask.Run(() =>
        {
          const string imageFileBaseName = "C:\\path\\to\\image\\directory\\Image";

          // For each row that satisfies the search criteria, write the blob field out to an image file
          using (RowCursor rowCursor = table.Search(queryFilter))
          {
            int fileCount = 0;
            while (rowCursor.MoveNext())
            {
              using (Row row = rowCursor.Current)
              {
                // Read the blob field into a MemoryStream
                MemoryStream memoryStream = row[blobFieldName] as MemoryStream;

                // Create a file
                using (FileStream outputFile = new FileStream(imageFileBaseName + fileCount.ToString(),
                         FileMode.Create, FileAccess.Write))
                {
                  // Write the MemoryStream into the file
                  memoryStream.WriteTo(outputFile);
                }
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.RelationshipClass.GetRowsRelatedToDestinationRows(System.Collections.Generic.IEnumerable{System.Int64})
      // cref: ArcGIS.Core.Data.RelationshipClass.GetRowsRelatedToOriginRows(System.Collections.Generic.IEnumerable{System.Int64})
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.Row.GetObjectID()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Selection.GetObjectIDs()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.Table.Select(ArcGIS.Core.Data.QueryFilter,ArcGIS.Core.Data.SelectionType,ArcGIS.Core.Data.SelectionOption)
      #region Getting Rows related by RelationshipClass
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.luCodeViolationHasInspections"))
          using (FeatureClass violationsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.luCodeViolation"))
          using (Table inspectionTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.luCodeInspection"))
          {
            List<Row> jeffersonAveViolations = new List<Row>();
            QueryFilter queryFilter = new QueryFilter { WhereClause = "LOCDESC LIKE '///%Jefferson///%'" };

            using (RowCursor rowCursor = violationsFeatureClass.Search(queryFilter, false))
            {
              while (rowCursor.MoveNext())
              {
                jeffersonAveViolations.Add(rowCursor.Current);
              }
            }

            IReadOnlyList<Row> relatedOriginRows = null;
            IReadOnlyList<Row> relatedDestinationRows = null;

            try
            {
              QueryFilter filter = new QueryFilter { WhereClause = "ACTION = '1st Notice'" };

              using (Selection selection = inspectionTable.Select(filter, SelectionType.ObjectID, SelectionOption.Normal))
              {
                relatedOriginRows = relationshipClass.GetRowsRelatedToDestinationRows(selection.GetObjectIDs());
              }

              bool containsJeffersonAve = relatedOriginRows.Any(row => Convert.ToString(row["LOCDESC"]).Contains("Jefferson"));

              List<long> jeffersonAveViolationObjectIds = jeffersonAveViolations.Select(row => row.GetObjectID()).ToList();

              relatedDestinationRows = relationshipClass.GetRowsRelatedToOriginRows(jeffersonAveViolationObjectIds);
              bool hasFirstNoticeInspections = relatedDestinationRows.Any(row => Convert.ToString(row["ACTION"]).Contains("1st Notice"));
            }
            finally
            {
              Dispose(jeffersonAveViolations);
              Dispose(relatedOriginRows);
              Dispose(relatedDestinationRows);
            }
          }
        });

        void Dispose(IEnumerable<Row> rows)
        {
          foreach (Row row in rows)
          {
            row.Dispose();
          }
        }
      }
      #endregion Getting Rows related by RelationshipClass

      // cref: ArcGIS.Core.Data.RelationshipClass.CreateRelationship(ArcGIS.Core.Data.Row,ArcGIS.Core.Data.Row)
      // cref: ArcGIS.Core.Data.RelationshipClassDefinition.GetOriginKeyField()
      // cref: ArcGIS.Core.Data.Relationship
      // cref: ArcGIS.Core.Data.FeatureClass.CreateRow(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.RelationshipClass.GetDefinition()
      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      #region Creating a Relationship
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.OverviewToProject"))
          using (FeatureClass projectsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjects"))
          using (FeatureClass overviewFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjectsOverview"))
          {
            // This will be PROJNAME. This can be used to get the field index or used directly as the field name.
            string originKeyField = relationshipClass.GetDefinition().GetOriginKeyField();

            EditOperation editOperation = new EditOperation();
            editOperation.Callback(context =>
            {
              // The rows are being added to illustrate adding relationships.
              // If one has existing rows, those can be used to add a relationship.
              using (RowBuffer projectsRowBuffer = projectsFeatureClass.CreateRowBuffer())
              using (RowBuffer overviewRowBuffer = overviewFeatureClass.CreateRowBuffer())
              {
                projectsRowBuffer["TOTCOST"] = 500000;

                overviewRowBuffer[originKeyField] = "LibraryConstruction";
                overviewRowBuffer["PROJECTMAN"] = "John Doe";
                overviewRowBuffer["FUNDSOUR"] = " ";

                using (Row projectsRow = projectsFeatureClass.CreateRow(projectsRowBuffer))
                using (Row overviewRow = overviewFeatureClass.CreateRow(overviewRowBuffer))
                {
                  Relationship relationship = relationshipClass.CreateRelationship(overviewRow, projectsRow);

                  //To Indicate that the Map has to draw this feature/row and/or the attribute table has to be updated
                  context.Invalidate(projectsRow);
                  context.Invalidate(overviewRow);
                  context.Invalidate(relationshipClass);
                }
              }
            }, projectsFeatureClass, overviewFeatureClass);

            bool editResult = editOperation.Execute();
          }
        });
      }
      #endregion Creating a Relationship

      // cref: ArcGIS.Core.Data.RelationshipClass.DeleteRelationship(ArcGIS.Core.Data.Row,ArcGIS.Core.Data.Row)
      // cref: ArcGIS.Core.Data.RelationshipClass.GetRowsRelatedToOriginRows(System.Collections.Generic.IEnumerable{System.Int64})
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.Row.GetObjectID()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Deleting a Relationship
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.luCodeViolationHasInspections"))
          using (FeatureClass violationsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.luCodeViolation"))
          {
            QueryFilter queryFilter = new QueryFilter { WhereClause = "LOCDESC LIKE '///%Jefferson///%'" };

            using (RowCursor rowCursor = violationsFeatureClass.Search(queryFilter, false))
            {
              if (!rowCursor.MoveNext())
              {
                return;
              }

              using (Row jeffersonAveViolation = rowCursor.Current)
              {
                IReadOnlyList<Row> relatedDestinationRows = relationshipClass.GetRowsRelatedToOriginRows(new List<long>
                    { jeffersonAveViolation.GetObjectID() });

                try
                {
                  EditOperation editOperation = new EditOperation();
                  editOperation.Callback(context =>
                  {
                    foreach (Row relatedDestinationRow in relatedDestinationRows)
                    {
                      try
                      {
                        relationshipClass.DeleteRelationship(jeffersonAveViolation, relatedDestinationRow);
                      }
                      catch (GeodatabaseRelationshipClassException exception)
                      {
                        Console.WriteLine(exception);
                      }
                    }
                  }, relationshipClass);

                  bool editResult = editOperation.Execute();
                }
                finally
                {
                  foreach (Row row in relatedDestinationRows)
                  {
                    row.Dispose();
                  }
                }
              }
            }
          }
        });
      }
      #endregion Deleting a Relationship

      // cref: ArcGIS.Core.Data.Table.CreateInsertCursor()
      // cref: ArcGIS.Core.Data.InsertCursor.Insert(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.InsertCursor.Flush()
      // cref: ArcGIS.Core.Data.Geodatabase.ApplyEdits(System.Action)
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      #region Using an Insert Cursor
      {
        // Insert Cursors are intended for use in CoreHost applications, not Pro Add-ins
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          using (Table citiesTable = geodatabase.OpenDataset<Table>("name\\of\\cities_table"))
          {
            geodatabase.ApplyEdits(() =>
            {
              using (InsertCursor insertCursor = citiesTable.CreateInsertCursor())
              using (RowBuffer rowBuffer = citiesTable.CreateRowBuffer())
              {
                rowBuffer["State"] = "Colorado";

                rowBuffer["Name"] = "Fort Collins";
                rowBuffer["Population"] = 167830;
                insertCursor.Insert(rowBuffer);

                rowBuffer["Name"] = "Denver";
                rowBuffer["Population"] = 727211;
                insertCursor.Insert(rowBuffer);

                // Insert more rows here
                // A more realistic example would be reading source data from a file

                insertCursor.Flush();
              }
            });
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClass.CreateRow(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer()
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.SetAnnotationClassID(System.Int64)
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.SetStatus(ArcGIS.Core.Data.AnnotationStatus)
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.SetGraphic(ArcGIS.Core.CIM.CIMGraphic)
      // cref: ArcGIS.Core.Data.AnnotationStatus
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition.GetLabelClassCollection()
      // cref: ArcGIS.Core.CIM.CIMLabelClass
      // cref: ArcGIS.Core.CIM.CIMLabelClass.TextSymbol
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.Row.Store()
      #region Creating a new Annotation Feature in an Annotation FeatureClass using a RowBuffer
      {
        Task CreatingAnAnnotationFeatureAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          using (AnnotationFeatureClass annotationFeatureClass = geodatabase.OpenDataset<AnnotationFeatureClass>("Annotation // feature // class // name"))
          using (AnnotationFeatureClassDefinition annotationFeatureClassDefinition = annotationFeatureClass.GetDefinition())
          using (RowBuffer rowBuffer = annotationFeatureClass.CreateRowBuffer())
          using (AnnotationFeature annotationFeature = annotationFeatureClass.CreateRow(rowBuffer))
          {
            annotationFeature.SetAnnotationClassID(0);
            annotationFeature.SetStatus(AnnotationStatus.Placed);

            // Get the annotation labels from the label collection
            IReadOnlyList<CIMLabelClass> labelClasses =
              annotationFeatureClassDefinition.GetLabelClassCollection();

            // Setup the symbol reference with the symbol id and the text symbol
            CIMSymbolReference cimSymbolReference = new CIMSymbolReference();
            cimSymbolReference.Symbol = labelClasses[0].TextSymbol.Symbol;
            cimSymbolReference.SymbolName = labelClasses[0].TextSymbol.SymbolName;

            // Setup the text graphic
            CIMTextGraphic cimTextGraphic = new CIMTextGraphic();
            cimTextGraphic.Text = "Charlotte, North Carolina";
            cimTextGraphic.Shape = new MapPointBuilderEx(new Coordinate2D(-80.843, 35.234), SpatialReferences.WGS84)
              .ToGeometry();
            cimTextGraphic.Symbol = cimSymbolReference;

            // Set the symbol reference on the graphic and store
            annotationFeature.SetGraphic(cimTextGraphic);
            annotationFeature.Store();
          }
        });
      }
      #endregion Creating a new Annotation Feature in an Annotation FeatureClass using a RowBuffer

      #region ProSnippet Group: Versioning
      #endregion

      // cref: ArcGIS.Core.Data.VersionManager.GetVersion(System.String)
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      // cref: ArcGIS.Core.Data.Geodatabase.IsVersioningSupported()
      // cref: ArcGIS.Core.Data.Version.Connect()
      #region Connecting to a Version
      {
        Task<Geodatabase> ConnectToVersionAsync(Geodatabase geodatabase, string versionName) => QueuedTask.Run(() =>
        {
          Geodatabase connectedVersion = null;

          if (geodatabase.IsVersioningSupported())
          {
            using (VersionManager versionManager = geodatabase.GetVersionManager())
            using (Version version = versionManager.GetVersion(versionName))
            {
              connectedVersion = version.Connect();
            }
          }

          return connectedVersion;
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.IsVersioningSupported()
      // cref: ArcGIS.Core.Data.VersionManager.GetCurrentVersion()
      // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions)
      // cref: ArcGIS.Core.Data.Version.GetParent()
      // cref: ArcGIS.Core.Data.ReconcileOptions
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionMethod
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictDetectionType
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionType
      // cref: ArcGIS.Core.Data.ConflictResolutionMethod
      // cref: ArcGIS.Core.Data.ConflictDetectionType
      // cref: ArcGIS.Core.Data.ConflictResolutionType
      // cref: ArcGIS.Core.Data.ReconcileResult
      // cref: ArcGIS.Core.Data.ReconcileResult.HasConflicts
      // cref: ArcGIS.Core.Data.PostOptions
      // cref: ArcGIS.Core.Data.PostOptions.ServiceSynchronizationType
      // cref: ArcGIS.Core.Data.ServiceSynchronizationType
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      #region Reconciling and Posting a Version with its Parent in separate edit sessions
      {
        Task ReconcileAndPostAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Get a reference to our version and our parent
          if (geodatabase.IsVersioningSupported())
          {
            using (VersionManager versionManager = geodatabase.GetVersionManager())
            using (Version currentVersion = versionManager.GetCurrentVersion())
            using (Version parentVersion = currentVersion.GetParent())
            {
              // Create a ReconcileOptions object
              ReconcileOptions reconcileOptions = new ReconcileOptions(parentVersion);
              reconcileOptions.ConflictResolutionMethod = ConflictResolutionMethod.Continue; // continue if conflicts are found
              reconcileOptions.ConflictDetectionType = ConflictDetectionType.ByRow; //Default
              reconcileOptions.ConflictResolutionType = ConflictResolutionType.FavorTargetVersion; //or FavorEditVersion

              // Reconcile
              ReconcileResult reconcileResult = currentVersion.Reconcile(reconcileOptions);
              if (!reconcileResult.HasConflicts)
              {
                //No conflicts, perform the post
                PostOptions postOptions = new PostOptions(parentVersion);
                //var postOptions = new PostOptions(); for default version
                postOptions.ServiceSynchronizationType = ServiceSynchronizationType.Synchronous; //Default

                // Post - Deprecated since Pro SDK 3.6 / Enterprise 12.0 . Use Reconcile(reconcileOptions, postOptions) instead.
                // currentVersion.Post(postOptions);

                currentVersion.Reconcile(reconcileOptions, postOptions);
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.IsVersioningSupported()
      // cref: ArcGIS.Core.Data.VersionManager.GetCurrentVersion()
      // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions,ArcGIS.Core.Data.PostOptions)
      // cref: ArcGIS.Core.Data.Version.GetParent()
      // cref: ArcGIS.Core.Data.ReconcileOptions
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionMethod
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictDetectionType
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionType
      // cref: ArcGIS.Core.Data.ConflictResolutionMethod
      // cref: ArcGIS.Core.Data.ConflictDetectionType
      // cref: ArcGIS.Core.Data.ConflictResolutionType
      // cref: ArcGIS.Core.Data.ReconcileResult
      // cref: ArcGIS.Core.Data.ReconcileResult.HasConflicts
      // cref: ArcGIS.Core.Data.PostOptions
      // cref: ArcGIS.Core.Data.PostOptions.ServiceSynchronizationType
      // cref: ArcGIS.Core.Data.ServiceSynchronizationType
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      #region Reconciling and Posting a Version with its Parent in the same edit session
      {
        Task ReconcileAndPost2Async(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Get a reference to our version and our parent
          if (geodatabase.IsVersioningSupported())
          {
            using (VersionManager versionManager = geodatabase.GetVersionManager())
            using (Version currentVersion = versionManager.GetCurrentVersion())
            using (Version parentVersion = currentVersion.GetParent())
            {
              // Create a ReconcileOptions object
              ReconcileOptions reconcileOptions = new ReconcileOptions(parentVersion);
              reconcileOptions.ConflictResolutionMethod = ConflictResolutionMethod.Continue; // continue if conflicts are found
              reconcileOptions.ConflictDetectionType = ConflictDetectionType.ByRow; //Default
              reconcileOptions.ConflictResolutionType = ConflictResolutionType.FavorTargetVersion; //or FavorEditVersion

              PostOptions postOptions = new PostOptions(parentVersion);
              //var postOptions = new PostOptions(); for default version
              postOptions.ServiceSynchronizationType = ServiceSynchronizationType.Synchronous; //Default

              // Reconcile
              ReconcileResult reconcileResult = currentVersion.Reconcile(reconcileOptions, postOptions);
              if (reconcileResult.HasConflicts)
              {
                //TODO resolve conflicts
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.VersionManager.GetVersionNames()
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      // cref: ArcGIS.Core.Data.Version.Connect()
      // cref: ArcGIS.Core.Data.Version.GetName()
      // cref: ArcGIS.Core.Data.Version.GetChildren()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.VersionManager.GetDefaultVersion()
      // cref: ArcGIS.Core.Data.VersionManager.GetVersion(System.String)
      #region Working with Versions
      {
        await QueuedTask.Run(() =>
        {
          using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
          using (VersionManager versionManager = geodatabase.GetVersionManager())
          {
            IReadOnlyList<string> versionNames = versionManager.GetVersionNames();

            Version defaultVersion = versionManager.GetDefaultVersion();

            string testVersionName = versionNames.First(v => v.Contains("Test"));
            Version testVersion = versionManager.GetVersion(testVersionName);

            Version qaVersion = defaultVersion.GetChildren().First(version => version.GetName().Contains("QA"));

            Geodatabase qaVersionGeodatabase = qaVersion.Connect();

            FeatureClass currentFeatureClass = geodatabase.OpenDataset<FeatureClass>("featureClassName");
            FeatureClass qaFeatureClass = qaVersionGeodatabase.OpenDataset<FeatureClass>("featureClassName");
          }
        });
      }
      #endregion Working with Versions

      // cref: ArcGIS.Core.Data.Version.GetParent()
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      // cref: ArcGIS.Core.Data.VersionManager.GetCurrentVersion()
      // cref: ArcGIS.Core.Data.Geodatabase.IsVersioningSupported()
      #region Working with the Default Version
      {
        Task<Version> FindDefaultVersionAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          if (!geodatabase.IsVersioningSupported())
          {
            return null;
          }

          using (VersionManager versionManager = geodatabase.GetVersionManager())
          {
            Version currentVersion = versionManager.GetCurrentVersion();
            Version defaultVersion = GetDefaultVersion(currentVersion);
            if (currentVersion != defaultVersion)
            {
              currentVersion.Dispose(); // If we are not pointing to default, we want to dispose this Version object
            }
            return defaultVersion;
          }

          // Check to see if the current version is default.
          // Works with both branch and traditional versioning.
          bool IsDefaultVersion(Version version)
          {
            Version parentVersion = version.GetParent();
            if (parentVersion == null)
            {
              return true;
            }

            parentVersion.Dispose();
            return false;
          }

          // Gets the default version.
          // Works with both branch and traditional versioning.
          // Note that this routine depends on IsDefaultVersion(), above.
          Version GetDefaultVersion(Version version)
          {
            if (IsDefaultVersion(version))
            {
              return version;
            }

            Version parent = version.GetParent();
            Version ancestor = GetDefaultVersion(parent);
            if (parent != ancestor)
            {
              parent.Dispose(); //If the versioning tree is more than 2 deep, we want to dispose any intermediary versions
            }

            return ancestor;
          }
        });
      }
      #endregion Working with the Default Version

      // cref: ArcGIS.Core.Data.VersionManager.CreateVersion(ArcGIS.Core.Data.VersionDescription)
      // cref: ArcGIS.Core.Data.VersionDescription
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      // cref: ArcGIS.Core.Data.Geodatabase.IsVersioningSupported()
      #region Creating a Version
      {
        Task<Version> CreateVersionAsync(Geodatabase geodatabase, string versionName, string description, VersionAccessType versionAccessType) => QueuedTask.Run(() =>
        {
          if (!geodatabase.IsVersioningSupported())
          {
            return null;
          }

          using (VersionManager versionManager = geodatabase.GetVersionManager())
          {
            VersionDescription versionDescription =
              new VersionDescription(versionName, description, versionAccessType);
            return versionManager.CreateVersion(versionDescription);
          }
        });
      }
      #endregion Creating a Version

      // cref: ArcGIS.Core.Data.VersionManager.CreateHistoricalVersion(ArcGIS.Core.Data.HistoricalVersionDescription)
      // cref: ArcGIS.Core.Data.HistoricalVersionDescription
      // cref: ArcGIS.Core.Data.HistoricalVersion
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      #region Creating a Historical version
      {
        Task<HistoricalVersion> CreateHistoricalVersionAsync(Geodatabase geodatabase, string versionName) => QueuedTask.Run(() =>
        {
          using (VersionManager versionManager = geodatabase.GetVersionManager())
          {
            HistoricalVersionDescription historicalVersionDescription =
              new HistoricalVersionDescription(versionName, DateTime.Now);
            HistoricalVersion historicalVersion =
              versionManager.CreateHistoricalVersion(historicalVersionDescription);

            return historicalVersion;
          }
        });
      }
      #endregion Creating a Historical version

      // cref: ArcGIS.Desktop.Mapping.Map.ChangeVersion(ArcGIS.Core.Data.VersionBase,ArcGIS.Core.Data.VersionBase)
      // cref: ArcGIS.Core.Data.VersionManager.GetCurrentVersionBaseType()
      // cref: ArcGIS.Core.Data.VersionManager.GetCurrentHistoricalVersion()
      // cref: ArcGIS.Core.Data.VersionManager.GetHistoricalVersion(System.String)
      // cref: ArcGIS.Core.Data.VersionBaseType
      // cref: ArcGIS.Core.Data.HistoricalVersion
      // cref: ArcGIS.Core.Data.Geodatabase.GetVersionManager()
      // cref: ArcGIS.Core.Data.VersionManager.GetCurrentVersion()
      // cref: ArcGIS.Core.Data.VersionManager.GetVersion(System.String)
      #region Switching between versions
      {
        Task ChangeVersionsAsync(Geodatabase geodatabase, string toVersionName)
        {
          var map = MapView.Active?.Map;
          if (map == null)
            return Task.CompletedTask;

          return QueuedTask.Run(() =>
          {
            using (VersionManager versionManager = geodatabase.GetVersionManager())
            {
              VersionBaseType versionBaseType = versionManager.GetCurrentVersionBaseType();

              if (versionBaseType == VersionBaseType.Version)
              {
                Version fromVersion = versionManager.GetCurrentVersion();
                Version toVersion = versionManager.GetVersion(toVersionName);

                // Switch between versions
                map.ChangeVersion(fromVersion, toVersion);
              }

              if (versionBaseType == VersionBaseType.HistoricalVersion)
              {
                HistoricalVersion fromHistoricalVersion = versionManager.GetCurrentHistoricalVersion();
                HistoricalVersion toHistoricalVersion = versionManager.GetHistoricalVersion(toVersionName);

                // Switch between historical versions
                map.ChangeVersion(fromHistoricalVersion, toHistoricalVersion);
              }

              // Switch from HistoricalVersion to Version and vice-versa
              // map.ChangeVersion(fromHistoricalVersion, toVersion);
              // map.ChangeVersion(fromVersion, toHistoricalVersion);
            }
          });
        }
      }
      #endregion Switching between versions

      // cref: ArcGIS.Core.Data.ReconcileOptions
      // cref: ArcGIS.Core.Data.PartialPostOptions
      // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions,ArcGIS.Core.Data.PartialPostOptions)
      // cref: ArcGIS.Core.Data.PartialPostOptions.Selections
      // cref: ArcGIS.Core.Data.PartialPostOptions.ServiceSynchronizationType
      // cref: ArcGIS.Core.Data.QueryFilter.WhereClause
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictDetectionType
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionMethod
      // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionType
      // cref: ArcGIS.Core.Data.Selection.Add(System.Collections.Generic.IEnumerable{System.Int64})
      // cref: ArcGIS.Core.Data.Table.Select(ArcGIS.Core.Data.QueryFilter,ArcGIS.Core.Data.SelectionType,ArcGIS.Core.Data.SelectionOption)
      #region Partial Posting
      {
        Task PartialPostingSnippetAsync(Version designVersion, FeatureClass supportStructureFeatureClass, List<long> deletedSupportStructureObjectIDs) => QueuedTask.Run(() =>
        {
          // Partial posting allows developers to post a subset of changes made in a version.
          // One sample use case is an electric utility that uses a version to design the facilities in
          // a new housing subdivision. At some point in the process, one block of new houses have been
          // completed, while the rest of the subdivision remains unbuilt.  Partial posting allows the user
          // to post the completed work, while leaving not yet constructed features in the version to be
          // posted later. Partial posting requires a branch-versioned feature service using ArcGIS
          // Enterprise 10.9 and higher

          // Specify a set of features that were constructed
          QueryFilter constructedFilter = new QueryFilter()
          {
            WhereClause = "ConstructedStatus = 'True'"
          };
          // This selection represents the inserts and updates to the support
          // structure feature class that we wish to post
          using (Selection constructedSupportStructures = supportStructureFeatureClass.Select(constructedFilter, SelectionType.ObjectID,
                   SelectionOption.Normal))
          {
            // Specifying which feature deletions you wish to post is slightly trickier, since you cannot issue
            // a query to fetch a set of deleted features Instead, a list of ObjectIDs must be used
            using (Selection deletedSupportStructures = supportStructureFeatureClass.Select(null, SelectionType.ObjectID, SelectionOption.Empty))
            {
              deletedSupportStructures.Add(deletedSupportStructureObjectIDs); //deletedSupportStructureObjectIDs is defined as List<long>

              // Perform the reconcile with partial post
              ReconcileOptions reconcileOptions = new ReconcileOptions(); // Reconcile against Default
              reconcileOptions.ConflictDetectionType = ConflictDetectionType.ByColumn;
              reconcileOptions.ConflictResolutionMethod = ConflictResolutionMethod.Continue;
              reconcileOptions.ConflictResolutionType = ConflictResolutionType.FavorEditVersion;

              /*
              PostOptions postOptions = new PostOptions();

              // PartialPostSelections - Deprecated since Pro SDK 3.7. Use PartialPostOptions instead.
              postOptions.PartialPostSelections = new List<Selection>()
                {
                  constructedSupportStructures, deletedSupportStructures
                };
              postOptions.ServiceSynchronizationType = ServiceSynchronizationType.Synchronous;
              */

              PartialPostOptions partialPostOptions = new PartialPostOptions(); // Partial post against the Default
              partialPostOptions.Selections = new List<Selection>()
              {
                constructedSupportStructures, deletedSupportStructures
              };
              partialPostOptions.ServiceSynchronizationType = ServiceSynchronizationType.Synchronous;

              ReconcileResult reconcileResult = designVersion.Reconcile(reconcileOptions, partialPostOptions);

              // Use the reconcileResult ...
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.FeatureDataset.GetDefinition``1(String)
      // cref: ArcGIS.Core.Data.FeatureDataset.GetDefinitions``1()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      #region Iterate datasets inside a feature dataset
      {
        Task IterateDatasetsFromAFeatureDatasetAsync(Geodatabase geodatabase, string featureDatasetName = "City", string featureClassInFeatureDataset = "Buildings") => QueuedTask.Run(() =>
        {
          // Open a feature dataset
          using (FeatureDataset cityFeatureDataset = geodatabase.OpenDataset<FeatureDataset>(featureDatasetName))
          {
            // Get a feature class definition from a feature dataset
            FeatureClassDefinition buildingsFeatureClassDefinition = cityFeatureDataset.GetDefinition<FeatureClassDefinition>(featureClassInFeatureDataset);

            // Iterate dataset definition
            IReadOnlyList<FeatureClassDefinition> cityFeatureClassDefinitions = cityFeatureDataset.GetDefinitions<FeatureClassDefinition>();
            foreach (FeatureClassDefinition cityFeatureClassDefinition in cityFeatureClassDefinitions)
            {
              // Use feature class definition ...
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetIsFieldEditable()
      // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetEvaluationOrder()
      // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetTriggeringEvents()
      // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetScriptExpression()
      // cref: ArcGIS.Core.Data.TableDefinition.GetAttributeRules(ArcGIS.Core.Data.AttributeRuleType)
      // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetAttributeRuleType()
      // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetDescription()
      // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetMinimumArcadeVersion()
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Get attribute rules of a dataset
      {
        Task GetAttributeRulesAsync(Geodatabase geodatabase, string tableName) => QueuedTask.Run(() =>
        {
          using (TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableName))
          {
            // Get all attribute rule types
            IReadOnlyList<AttributeRuleDefinition> ruleDefinitions = tableDefinition.GetAttributeRules();

            // Iterate rule definitions
            foreach (AttributeRuleDefinition ruleDefinition in ruleDefinitions)
            {
              AttributeRuleType ruleType = ruleDefinition.GetAttributeRuleType();
              string ruleDescription = ruleDefinition.GetDescription();
              bool isAttributeFieldEditable = ruleDefinition.GetIsFieldEditable();
              string arcadeVersionToSupportRule = ruleDefinition.GetMinimumArcadeVersion();
              int ruleEvaluationOrder = ruleDefinition.GetEvaluationOrder();
              AttributeRuleTriggers triggeringEvents = ruleDefinition.GetTriggeringEvents();
              string scriptExpression = ruleDefinition.GetScriptExpression();

              // More properties ...
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Table.CreateRowBuffer(ArcGIS.Core.Data.Row)
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Table.CreateRow(ArcGIS.Core.Data.RowBuffer)
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Creating  a row buffer from a template row
      {
        Task CreateRowBufferFromARowAsync(Table table) => QueuedTask.Run(() =>
        {
          using (RowCursor rowCursor = table.Search())
          {
            if (rowCursor.MoveNext())
            {
              using (Row templateRow = rowCursor.Current)
              {
                RowBuffer rowBuffer = table.CreateRowBuffer(templateRow);

                // Manipulate row buffer
                // Doesn't allow copying values of ObjectID and GlobalID
                //
                // rowBuffer["Field"] = "Update";
                //

                // Create a new row
                table.CreateRow(rowBuffer);
              }
            }
          }
        });
      }
      #endregion


      #region ProSnippet Group: Extension Ids
      #endregion

      // cref: ArcGIS.Core.Data.CoreDataExtensions.AddActivationExtension(System.Guid)
      // cref: ArcGIS.Core.Data.CoreDataExtensions.RemoveActivationExtension(System.Guid)
      // cref: ArcGIS.Core.Data.CoreDataExtensions.GetHasActivationExtension(System.Guid)
      // cref: ArcGIS.Core.Data.CoreDataExtensions.GetActivationExtensions()
      #region Table Extension Ids
      {
        Task ExtensionIds1Async(Table table) => QueuedTask.Run(() =>
        {
          // Add an extension id to a table
          // Check documentation for restrictions on backward compatibility - backward
          // compatibility is limited to ArcGIS Pro 3.1 if an extension id is added.
          // Note: This is an extension method. It is for use in addins only and not CoreHost.
          string extension_id_string = "52d8f3be-b73d-4140-beaf-23d4f9b697ea";
          Guid extension_id = Guid.Parse(extension_id_string);

          // Register the extension id with the relevant table
          table.AddActivationExtension(extension_id);

          // Remove an extension id from a table
          // Restores backward compatibility assuming no other compatibility limitations are in place.
          // Note: This is an extension method. It is for use in addins only and not CoreHost.
          table.RemoveActivationExtension(extension_id);

          // Check if a given extension id  is registered with a particular table.
          // Note: This is an extension method. It is for use in addins only and not CoreHost.
          if (table.GetHasActivationExtension(extension_id))
          {
            //Implement custom logic relevant to presence of extension_id ...
          }

          // Enumerate all extension ids on a given table.
          // Note: This is an extension method. It is for use in addins only and not CoreHost.
          foreach (Guid ext_id in table.GetActivationExtensions())
          {
            // Logic based on extension ids ...
          }
        });
      }
      #endregion

      #region ProSnippet Group: DDL
      #endregion


      // cref: ArcGIS.Core.Data.DDL.FieldDescription
      // cref: ArcGIS.Core.Data.FieldType
      #region 64-bit Integer field
      {
        FieldDescription bigIntegerFieldDescription =
          new FieldDescription("BigInteger_64", FieldType.BigInteger);
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.FieldDescription
      // cref: ArcGIS.Core.Data.FieldType
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.Length
      #region ObjectID field
      {
        // 64-bit
        FieldDescription oidFieldDescription_64 =
          new FieldDescription("ObjectID_64", FieldType.OID)
          {
            Length = 8
          };

        // 32-bit
        FieldDescription oidFieldDescription_32 =
          new FieldDescription("ObjectID_32", FieldType.OID)
          {
            Length = 4
          };
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.FieldDescription
      // cref: ArcGIS.Core.Data.FieldType
      #region DateOnly, TimeOnly, and TimestampOffset  field
      {
        // Earthquake occurrences date and time

        // 9/28/2014 (DateOnly)
        FieldDescription earthquakeDateOnlyFieldDescription =
          new FieldDescription("Earthquake_DateOnly", FieldType.DateOnly);

        // 1:16:42 AM (TimeOnly)
        FieldDescription earthquakeTimeOnlyFieldDescription =
          new FieldDescription("Earthquake_TimeOnly", FieldType.TimeOnly);

        // 9/28/2014 1:16:42.000 AM -09:00 (Timestamp with Offset)
        FieldDescription earthquakeTimestampOffsetFieldDescription =
          new FieldDescription("Earthquake_TimestampOffset_Local", FieldType.TimestampOffset);

        // 9/28/2014 1:16:42 AM (DateTime)
        FieldDescription earthquakeDateFieldDescription =
          new FieldDescription("Earthquake_Date", FieldType.Date);
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateObjectIDField()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateDomainField(System.String,ArcGIS.Core.Data.DDL.DomainDescription)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.AliasName
      #region Creating a Table
      {
        Task CreateTableSnippetAsync(Geodatabase geodatabase, CodedValueDomain inspectionResultsDomain) => QueuedTask.Run(() =>
        {
          // Create a PoleInspection table with the following fields
          // GlobalID
          // ObjectID
          // InspectionDate (date)
          // InspectionResults (pre-existing InspectionResults coded value domain)
          // InspectionNotes (string)

          // This helper routine creates a FieldDescription for a GlobalID field with default values
          FieldDescription globalIDFieldDescription = FieldDescription.CreateGlobalIDField();

          // This helper routine creates a FieldDescription for an ObjectID field with default values
          FieldDescription objectIDFieldDescription = FieldDescription.CreateObjectIDField();

          // Create a FieldDescription for the InspectionDate field
          FieldDescription inspectionDateFieldDescription = new FieldDescription("InspectionDate", FieldType.Date)
          {
            AliasName = "Inspection Date"
          };

          // This helper routine creates a FieldDescription for a Domain field (from a pre-existing domain)
          FieldDescription inspectionResultsFieldDescription = FieldDescription.CreateDomainField("InspectionResults",
            new CodedValueDomainDescription(inspectionResultsDomain));
          inspectionResultsFieldDescription.AliasName = "Inspection Results";

          // This helper routine creates a FieldDescription for a string field
          FieldDescription inspectionNotesFieldDescription =
            FieldDescription.CreateStringField("InspectionNotes", 512);
          inspectionNotesFieldDescription.AliasName = "Inspection Notes";

          // Assemble a list of all of our field descriptions
          List<FieldDescription> fieldDescriptions = new List<FieldDescription>()
            {
              globalIDFieldDescription, objectIDFieldDescription, inspectionDateFieldDescription,
              inspectionResultsFieldDescription, inspectionNotesFieldDescription
            };

          // Create a TableDescription object to describe the table to create
          TableDescription tableDescription = new TableDescription("PoleInspection", fieldDescriptions);

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Add the creation of PoleInspection to our list of DDL tasks
          schemaBuilder.Create(tableDescription);

          // Execute the DDL
          bool success = schemaBuilder.Build();

          // Inspect error messages
          if (!success)
          {
            IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
            //etc.
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateIntegerField(System.String)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateObjectIDField()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      #region Creating a feature class
      {
        Task CreateFeatureClassSnippetAsync(Geodatabase geodatabase, FeatureClass existingFeatureClass,
          SpatialReference spatialReference) => QueuedTask.Run(() =>
        {
          // Create a Cities feature class with the following fields
          // GlobalID
          // ObjectID
          // Name (string)
          // Population (integer)

          // This helper routine creates a FieldDescription for a GlobalID field with default values
          FieldDescription globalIDFieldDescription = FieldDescription.CreateGlobalIDField();

          // This helper routine creates a FieldDescription for an ObjectID field with default values
          FieldDescription objectIDFieldDescription = FieldDescription.CreateObjectIDField();

          // This helper routine creates a FieldDescription for a string field
          FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 255);

          // This helper routine creates a FieldDescription for an integer field
          FieldDescription populationFieldDescription = FieldDescription.CreateIntegerField("Population");

          // Assemble a list of all of our field descriptions
          List<FieldDescription> fieldDescriptions = new List<FieldDescription>()
              { globalIDFieldDescription, objectIDFieldDescription, nameFieldDescription, populationFieldDescription };

          // Create a ShapeDescription object
          ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Point, spatialReference);

          // Alternatively, ShapeDescriptions can be created from another feature class.  In this case, the new feature class will inherit the same shape properties of the existing class
          ShapeDescription alternativeShapeDescription = new ShapeDescription(existingFeatureClass.GetDefinition());

          // Enable support for measure (M) value, if needed
          // shapeDescription.HasM = true;

          // Enable support for the vertical coordinate system (Z), if needed
          // shapeDescription.HasZ = true;

          // Create a FeatureClassDescription object to describe the feature class to create
          FeatureClassDescription featureClassDescription =
            new FeatureClassDescription("Cities", fieldDescriptions, shapeDescription);

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Add the creation of the Cities feature class to our list of DDL tasks
          schemaBuilder.Create(featureClassDescription);

          // Execute the DDL
          bool success = schemaBuilder.Build();

          // Inspect error messages
          if (!success)
          {
            IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
            //etc.
          }
        });
      }
      #endregion

      // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
      // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref:ArcGIS.Core.Data.Table.GetDefinition()
      #region Deleting a Table
      {
        Task DeleteTableSnippetAsync(Geodatabase geodatabase, Table table) => QueuedTask.Run(() =>
        {
          // Create a TableDescription object
          TableDescription tableDescription = new TableDescription(table.GetDefinition());

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Add the deletion of the table to our list of DDL tasks
          schemaBuilder.Delete(tableDescription);

          // Execute the DDL
          bool success = schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition()
      #region Deleting a Feature Class
      {
        Task DeleteFeatureClassSnippetAsync(Geodatabase geodatabase, FeatureClass featureClass) => QueuedTask.Run(() =>
        {
          // Create a FeatureClassDescription object
          FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClass.GetDefinition());

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Add the deletion fo the feature class to our list of DDL tasks
          schemaBuilder.Delete(featureClassDescription);

          // Execute the DDL
          bool success = schemaBuilder.Build();
        });
      }
      #endregion


      // cref: ArcGIS.Core.Data.MemoryConnectionProperties
      #region Opens a memory geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Connects to the default memory geodatabase, if exists otherwise throws exception
          MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties();

          // Alternatively, connects to memory geodatabase named as 'InterimMemoryGeodatabase'
          // MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties("InterimMemoryGeodatabase");

          // Opens the memory geodatabase
          using (Geodatabase geodatabase = new Geodatabase(memoryConnectionProperties))
          {
            // Use memory geodatabase
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.MemoryConnectionProperties
      #region Creating a memory Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Create the memory connection properties to connect to  default memory geodatabase
          MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties();

          // Alternatively create the memory connection properties to connect to memory geodatabase named as 'InterimMemoryGeodatabase'
          // MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties("InterimMemoryGeodatabase");

          // Creates the new memory geodatabase if it does not exist or connects to an existing one if it already exists
          using (Geodatabase geodatabase = new Geodatabase(memoryConnectionProperties))
          {
            // Create additional schema here
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.DeleteGeodatabase(ArcGIS.Core.Data.MemoryConnectionProperties)
      // cref: ArcGIS.Core.Data.MemoryConnectionProperties
      #region Deleting a memory Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Create the memory connection properties to connect to default memory geodatabase
          MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties();

          // Delete the memory geodatabase
          SchemaBuilder.DeleteGeodatabase(memoryConnectionProperties);
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.FileGeodatabaseConnectionPath
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.CreateGeodatabase(ArcGIS.Core.Data.FileGeodatabaseConnectionPath)
      #region Creating a File Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Create a FileGeodatabaseConnectionPath with the name of the file geodatabase you wish to create
          FileGeodatabaseConnectionPath fileGeodatabaseConnectionPath =
            new FileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-File-Geodatabase\YourName.gdb"));

          // Create and use the file geodatabase
          using (Geodatabase geodatabase =
                 SchemaBuilder.CreateGeodatabase(fileGeodatabaseConnectionPath))
          {
            // Create additional schema here
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.FileGeodatabaseConnectionPath
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.DeleteGeodatabase(ArcGIS.Core.Data.FileGeodatabaseConnectionPath)
      #region Deleting a File Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Create a FileGeodatabaseConnectionPath with the name of the file geodatabase you wish to delete
          FileGeodatabaseConnectionPath fileGeodatabaseConnectionPath =
            new FileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-File-Geodatabase\YourName.gdb"));

          // Delete the file geodatabase
          SchemaBuilder.DeleteGeodatabase(fileGeodatabaseConnectionPath);
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.MobileGeodatabaseConnectionPath
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.CreateGeodatabase(ArcGIS.Core.Data.MobileGeodatabaseConnectionPath)
      #region Creating a Mobile Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Create a MobileGeodatabaseConnectionPath with the name of the mobile geodatabase you wish to create
          MobileGeodatabaseConnectionPath mobileGeodatabaseConnectionPath =
            new MobileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-Mobile-Geodatabase\YourName.geodatabase"));

          // Create and use the mobile geodatabase
          using (Geodatabase geodatabase = SchemaBuilder.CreateGeodatabase(mobileGeodatabaseConnectionPath))
          {
            // Create additional schema here
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.MobileGeodatabaseConnectionPath
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.DeleteGeodatabase(ArcGIS.Core.Data.MobileGeodatabaseConnectionPath)
      #region Deleting a Mobile Geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Create a MobileGeodatabaseConnectionPath with the name of the mobile geodatabase you wish to delete
          MobileGeodatabaseConnectionPath mobileGeodatabaseConnectionPath =
            new MobileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-Mobile-Geodatabase\YourName.geodatabase"));

          // Delete the mobile geodatabase
          SchemaBuilder.DeleteGeodatabase(mobileGeodatabaseConnectionPath);
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.RangeDomainDescription)
      // cref: ArcGIS.Core.Data.DDL.DomainDescription.Description
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Creating a Range domain
      {
        Task CreateRangeDomainSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Create a range description with minimum value = 0 and maximum value = 1000
          RangeDomainDescription rangeDomainDescriptionMinMax = new RangeDomainDescription("RangeDomain_0_1000",
              FieldType.Integer, 0, 1000)
          {
            Description = "Domain value ranges from 0 to 1000"
          };

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Create  a range domain
          schemaBuilder.Create(rangeDomainDescriptionMinMax);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.CodedValueDomainDescription)
      // cref: ArcGIS.Core.Data.DDL.DomainDescription.SplitPolicy
      // cref: ArcGIS.Core.Data.DDL.DomainDescription.MergePolicy
      // cref: ArcGIS.Core.Data.SplitPolicy
      // cref: ArcGIS.Core.Data.MergePolicy
      // cref: ArcGIS.Core.Data.DDL.CodedValueDomainToken
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Creating a CodedValue domain
      {
        Task CreateCodedDomainSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Create a CodedValueDomain description for water pipes
          CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription("WaterPipeTypes",
            FieldType.String, new SortedList<object, string> { { "C_1", "Copper" }, { "S_2", "Steel" } })
          {
            SplitPolicy = SplitPolicy.Duplicate,
            MergePolicy = MergePolicy.DefaultValue
          };

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Create a coded value domain
          CodedValueDomainToken codedValueDomainToken = schemaBuilder.Create(codedValueDomainDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      #region Creating a FeatureDataset
      {
        Task CreateFeatureDatasetSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Creating a FeatureDataset named as 'Parcel_Information'
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Create a FeatureDataset named as 'Parcel Information'
          FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription("Parcel_Information", SpatialReferences.WGS84);
          schemaBuilder.Create(featureDatasetDescription);

          // Build status
          bool buildStatus = schemaBuilder.Build();

          // Build errors
          if (!buildStatus)
          {
            IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
      // cref: ArcGIS.Core.Data.FeatureDatasetDefinition
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Deleting a FeatureDataset
      {
        Task DeleteFeatureDatasetSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Deleting a FeatureDataset named as 'Parcel_Information'
          FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("Parcel_Information");
          FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetDefinition);

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Delete an existing feature dataset named as 'Parcel_Information'
          schemaBuilder.Delete(featureDatasetDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Rename(ArcGIS.Core.Data.DDL.Description,System.String)
      // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref:ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Renaming a FeatureDataset
      {
        Task RenameFeatureDatasetSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Renaming a FeatureDataset from 'Parcel_Information' to 'Parcel_Information_With_Tax_Jurisdiction'
          string originalDatasetName = "Parcel_Information";
          string datasetRenameAs = "Parcel_Information_With_Tax_Jurisdiction";

          FeatureDatasetDefinition originalDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>(originalDatasetName);
          FeatureDatasetDescription originalFeatureDatasetDescription = new FeatureDatasetDescription(originalDatasetDefinition);

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Rename the existing FeatureDataset, 'Parcel_Information' to 'Parcel_Information_With_Tax_Jurisdiction'
          schemaBuilder.Rename(originalFeatureDatasetDescription, datasetRenameAs);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription,ArcGIS.Core.Data.DDL.FeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.FeatureDatasetToken
      // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
      // cref: ArcGIS.Core.Data.DDL.FieldDescription
      // cref: ArcGIS.Core.Data.DDL.ShapeDescription
      // cref: ArcGIS.Core.Data.DDL.FeatureClassToken
      // cref: ArcGIS.Core.Data.FieldType
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      #region Creating a FeatureDataset with a FeatureClass in one operation
      {
        Task CreateFeatureDatasetWithFeatureClassSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Creating a FeatureDataset named as 'Parcel_Information' and a FeatureClass with name 'Parcels' in one operation
          string featureDatasetName = "Parcel_Information";
          string featureClassName = "Parcels";

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Create a FeatureDataset token
          FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetName, SpatialReferences.WGS84);
          FeatureDatasetToken featureDatasetToken = schemaBuilder.Create(featureDatasetDescription);

          // Create a FeatureClass description
          FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassName,
            new List<FieldDescription>()
            {
                new FieldDescription("Id", FieldType.Integer),
                new FieldDescription("Address", FieldType.String)
            },
            new ShapeDescription(GeometryType.Point, SpatialReferences.WGS84));

          // Create a FeatureClass inside a FeatureDataset
          FeatureClassToken featureClassToken = schemaBuilder.Create(
            new FeatureDatasetDescription(featureDatasetToken),
            featureClassDescription);

          // Build status
          bool buildStatus = schemaBuilder.Build();

          // Build errors
          if (!buildStatus)
          {
            IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription,ArcGIS.Core.Data.DDL.FeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
      // cref: ArcGIS.Core.Data.DDL.FieldDescription
      // cref: ArcGIS.Core.Data.DDL.ShapeDescription
      // cref: ArcGIS.Core.Data.FieldType
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Creating a FeatureClass in existing FeatureDataset
      {
        Task CreateFeatureClassInsideFeatureDatasetSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Creating a FeatureClass named as 'Tax_Jurisdiction' in existing FeatureDataset with name 'Parcels_Information'
          string featureDatasetName = "Parcels_Information";
          string featureClassName = "Tax_Jurisdiction";

          // Create a FeatureClass description
          FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassName,
            new List<FieldDescription>()
            {
                new FieldDescription("Tax_Id", FieldType.Integer),
                new FieldDescription("Address", FieldType.String)
            },
            new ShapeDescription(GeometryType.Point, SpatialReferences.WGS84));

          FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>(featureDatasetName);

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Create a FeatureClass inside a FeatureDataset using a FeatureDatasetDefinition
          schemaBuilder.Create(new FeatureDatasetDescription(featureDatasetDefinition), featureClassDescription);

          // Build status
          bool buildStatus = schemaBuilder.Build();

          // Build errors
          if (!buildStatus)
          {
            IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.AddFeatureClass(ArcGIS.Core.Data.DDL.FeatureDatasetDescription,ArcGIS.Core.Data.DDL.FeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Adding a FeatureClass to a FeatureDataset
      {
        Task AddFeatureClassToFeatureDatasetSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Adding a FeatureClass with name 'Tax_Jurisdiction' into a FeatureDataset named as 'Parcels_Information'
          string featureDatasetName = "Parcels_Information";
          string featureClassNameToAdd = "Tax_Jurisdiction";

          FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>(featureDatasetName);
          FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetDefinition);

          FeatureClassDefinition featureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>(featureClassNameToAdd);
          FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassDefinition);

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Add the 'Tax_Jurisdiction' FeatureClass to the 'Parcels_Information' FeatureDataset
          schemaBuilder.AddFeatureClass(featureDatasetDescription, featureClassDescription);
          bool addStatus = schemaBuilder.Build();

          if (!addStatus)
          {
            IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Rename(ArcGIS.Core.Data.DDL.Description,System.String)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Renaming a Table
      {
        Task RenameTableSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Renaming a table from 'Original_Table' to 'Renamed_Table'
          string tableToBeRenamed = "Original_Table";
          string tableRenameAs = "Renamed_Table";

          TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableToBeRenamed);

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Table rename
          schemaBuilder.Rename(new TableDescription(tableDefinition), tableRenameAs);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.FeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateIntegerField(System.String)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.Description.Name
      // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription.ShapeDescription
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.DDL.TableDescription.FieldDescriptions
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Adding fields to a FeatureClass
      {
        Task AddFieldsInFeatureClassSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Adding following fields to the 'Parcels' FeatureClass
          // Global ID
          // Parcel_ID
          // Tax_Code
          // Parcel_Address

          // The FeatureClass to add fields
          string featureClassName = "Parcels";

          FeatureClassDefinition originalFeatureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>(featureClassName);
          FeatureClassDescription originalFeatureClassDescription = new FeatureClassDescription(originalFeatureClassDefinition);

          // The four new fields to add on the 'Parcels' FeatureClass
          FieldDescription globalIdField = FieldDescription.CreateGlobalIDField();
          FieldDescription parcelIdDescription = new FieldDescription("Parcel_ID", FieldType.GUID);
          FieldDescription taxCodeDescription = FieldDescription.CreateIntegerField("Tax_Code");
          FieldDescription addressDescription = FieldDescription.CreateStringField("Parcel_Address", 150);

          List<FieldDescription> fieldsToAdd = new List<FieldDescription>
            {
              globalIdField, parcelIdDescription,
              taxCodeDescription, addressDescription
            };

          // Add new fields on the new FieldDescription list
          List<FieldDescription> modifiedFieldDescriptions = new List<FieldDescription>(originalFeatureClassDescription.FieldDescriptions);
          modifiedFieldDescriptions.AddRange(fieldsToAdd);

          // The new FeatureClassDescription with additional fields
          FeatureClassDescription modifiedFeatureClassDescription = new FeatureClassDescription(originalFeatureClassDescription.Name,
            modifiedFieldDescriptions, originalFeatureClassDescription.ShapeDescription);

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Update the 'Parcels' FeatureClass with newly added fields
          schemaBuilder.Modify(modifiedFeatureClassDescription);
          bool modifyStatus = schemaBuilder.Build();

          if (!modifyStatus)
          {
            IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.CodedValueDomainToken
      // cref: ArcGIS.Core.Data.DDL.CodedValueDomainDescription
      // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
      // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription.ShapeDescription
      // cref: ArcGIS.Core.Data.DDL.Description.Name
      // cref: ArcGIS.Core.Data.DDL.DomainDescription.MergePolicy
      // cref: ArcGIS.Core.Data.DDL.DomainDescription.SplitPolicy
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.SetDomainDescription(ArcGIS.Core.Data.DDL.DomainDescription,System.Nullable{System.Int32})
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.CodedValueDomainDescription)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.FeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.TableDescription.FieldDescriptions
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Adding a Field that uses a domain
      {
        Task AddFieldWithDomainSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          //The FeatureClass to add field
          string featureClassName = "Pipes";

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Create a CodedValueDomain description for water pipes
          CodedValueDomainDescription pipeDomainDescription = new CodedValueDomainDescription("WaterPipeTypes",
            FieldType.String, new SortedList<object, string>
            {
                { "C_1", "Copper" },
                { "S_2", "Steel" }
            })
          {
            SplitPolicy = SplitPolicy.Duplicate,
            MergePolicy = MergePolicy.DefaultValue
          };

          // Create a coded value domain token
          CodedValueDomainToken codedValueDomainToken = schemaBuilder.Create(pipeDomainDescription);

          // Create a new description from domain token
          CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription(codedValueDomainToken);

          // Create a field named as 'PipeType' using a domain description
          FieldDescription domainFieldDescription = new FieldDescription("PipeType", FieldType.String);
          domainFieldDescription.SetDomainDescription(codedValueDomainDescription);

          //Retrieve existing information for 'Pipes' FeatureClass
          FeatureClassDefinition originalFeatureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>(featureClassName);
          FeatureClassDescription originalFeatureClassDescription =
            new FeatureClassDescription(originalFeatureClassDefinition);

          // Add domain field on existing fields
          // Adding a field,'PipeType', which uses the coded value domain to the 'Pipes' FeatureClass
          List<FieldDescription> modifiedFieldDescriptions = new List<FieldDescription>(originalFeatureClassDescription.FieldDescriptions)
            { domainFieldDescription };

          // Create a new description with updated fields for 'Pipes' FeatureClass
          FeatureClassDescription featureClassDescription = new FeatureClassDescription(originalFeatureClassDescription.Name, modifiedFieldDescriptions,
            originalFeatureClassDescription.ShapeDescription);

          // Update the 'Pipes' FeatureClass with domain field
          schemaBuilder.Modify(featureClassDescription);

          // Build status
          bool buildStatus = schemaBuilder.Build();

          // Build errors
          if (!buildStatus)
          {
            IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.Geodatabase.GetDefinition``1(System.String)
      #region Removing fields from a Table
      {
        Task RemoveFieldTableSnippetAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Removing all fields from 'Parcels' table except following
          // Tax_Code
          // Parcel_Address

          // The table to remove fields
          string tableName = "Parcels";

          TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableName);
          IReadOnlyList<Field> fields = tableDefinition.GetFields();

          // Existing fields from 'Parcels' table
          Field taxCodeField = fields.First(f => f.Name.Equals("Tax_Code"));
          Field parcelAddressField = fields.First(f => f.Name.Equals("Parcel_Address"));

          FieldDescription taxFieldDescription = new FieldDescription(taxCodeField);
          FieldDescription parcelAddressFieldDescription = new FieldDescription(parcelAddressField);

          // Fields to retain in modified table
          List<FieldDescription> fieldsToBeRetained = new List<FieldDescription>()
            {
              taxFieldDescription, parcelAddressFieldDescription
            };

          // New description of the 'Parcels' table with the 'Tax_Code' and 'Parcel_Address' fields
          TableDescription modifiedTableDescription = new TableDescription(tableName, fieldsToBeRetained);

          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Remove all fields except the 'Tax_Code' and 'Parcel_Address' fields
          schemaBuilder.Modify(modifiedTableDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.ShapeDescription
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.AllowBorderOverlap
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.PlacementQuality
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.DrawUnplacedLabels
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.InvertedLabelTolerance
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.RotateLabelWithDisplay
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.UnplacedLabelColor
      // cref: ArcGIS.Core.CIM.MaplexQualityType
      // cref: ArcGIS.Core.CIM.CIMLabelClass
      // cref: ArcGIS.Core.CIM.LabelExpressionEngine
      // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.AllowOverlappingLabels
      // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.LineOffset
      // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AlignLabelToLineDirection
      // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AvoidPolygonHoles
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsAutoCreate
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsSymbolIDRequired
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsUpdatedOnShapeChange
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      #region Creating an annotation feature class
      {
        Task CreateStandAloneAnnotationFeatureClassAsync(
          Geodatabase geodatabase, SpatialReference spatialReference) => QueuedTask.Run(() =>
        {
          // Creating a Cities annotation feature class
          // with following user defined fields
          // Name
          // GlobalID

          // Annotation feature class name
          string annotationFeatureClassName = "CitiesAnnotation";

          // Create user defined attribute fields for annotation feature class
          FieldDescription globalIDFieldDescription = FieldDescription.CreateGlobalIDField();
          FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 255);

          // Create a list of all field descriptions
          List<FieldDescription> fieldDescriptions = new List<FieldDescription>
              { globalIDFieldDescription, nameFieldDescription };

          // Create a ShapeDescription object
          ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Polygon, spatialReference);

          // Create general placement properties for Maplex engine
          CIMMaplexGeneralPlacementProperties generalPlacementProperties =
            new CIMMaplexGeneralPlacementProperties
            {
              AllowBorderOverlap = true,
              PlacementQuality = MaplexQualityType.High,
              DrawUnplacedLabels = true,
              InvertedLabelTolerance = 1.0,
              RotateLabelWithDisplay = true,
              UnplacedLabelColor = new CIMRGBColor
              {
                R = 0,
                G = 255,
                B = 0,
                Alpha = 0.5f // Green
              }
            };

          // Create general placement properties for Standard engine

          //CIMStandardGeneralPlacementProperties generalPlacementProperties =
          //  new CIMStandardGeneralPlacementProperties
          //  {
          //    DrawUnplacedLabels = true,
          //    InvertedLabelTolerance = 3.0,
          //    RotateLabelWithDisplay = true,
          //    UnplacedLabelColor = new CIMRGBColor
          //    {
          //      R = 255, G = 0, B = 0, Alpha = 0.5f // Red
          //    }
          //   };


          // Create annotation  label classes
          // Green label
          CIMLabelClass greenLabelClass = new CIMLabelClass
          {
            Name = "Green",
            ExpressionTitle = "Expression-Green",
            ExpressionEngine = LabelExpressionEngine.Arcade,
            Expression = "$feature.OBJECTID",
            ID = 1,
            Priority = 0,
            Visibility = true,
            TextSymbol = new CIMSymbolReference
            {
              Symbol = new CIMTextSymbol()
              {
                Angle = 45,
                FontType = FontType.Type1,
                FontFamilyName = "Tahoma",
                FontEffects = FontEffects.Normal,
                HaloSize = 2.0,

                Symbol = new CIMPolygonSymbol
                {
                  SymbolLayers = new CIMSymbolLayer[]
                  {
                      new CIMSolidFill
                      {
                        Color = CIMColor.CreateRGBColor(0, 255, 0)
                      }
                  },
                  UseRealWorldSymbolSizes = true
                }
              },
              MaxScale = 0,
              MinScale = 0,
              SymbolName = "TextSymbol-Green"
            },
            StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
            {
              AllowOverlappingLabels = true,
              LineOffset = 1.0
            },
            MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
            {
              AlignLabelToLineDirection = true,
              AvoidPolygonHoles = true
            }
          };

          // Blue label
          CIMLabelClass blueLabelClass = new CIMLabelClass
          {
            Name = "Blue",
            ExpressionTitle = "Expression-Blue",
            ExpressionEngine = LabelExpressionEngine.Arcade,
            Expression = "$feature.OBJECTID",
            ID = 2,
            Priority = 0,
            Visibility = true,
            TextSymbol = new CIMSymbolReference
            {
              Symbol = new CIMTextSymbol()
              {
                Angle = 45,
                FontType = FontType.Type1,
                FontFamilyName = "Consolas",
                FontEffects = FontEffects.Normal,
                HaloSize = 2.0,

                Symbol = new CIMPolygonSymbol
                {
                  SymbolLayers = new CIMSymbolLayer[]
                  {
                      new CIMSolidFill
                      {
                        Color = CIMColor.CreateRGBColor(0, 0, 255)
                      }
                  },
                  UseRealWorldSymbolSizes = true
                }
              },
              MaxScale = 0,
              MinScale = 0,
              SymbolName = "TextSymbol-Blue"
            },
            StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
            {
              AllowOverlappingLabels = true,
              LineOffset = 1.0
            },
            MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
            {
              AlignLabelToLineDirection = true,
              AvoidPolygonHoles = true
            }
          };

          // Create a list of labels
          List<CIMLabelClass> labelClasses = new List<CIMLabelClass> { greenLabelClass, blueLabelClass };

          // Create an annotation feature class description object to describe the feature class to create
          AnnotationFeatureClassDescription annotationFeatureClassDescription =
            new AnnotationFeatureClassDescription(annotationFeatureClassName, fieldDescriptions, shapeDescription,
              generalPlacementProperties, labelClasses)
            {
              IsAutoCreate = true,
              IsSymbolIDRequired = false,
              IsUpdatedOnShapeChange = true
            };

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Add the creation of the Cities annotation feature class to the list of DDL tasks
          schemaBuilder.Create(annotationFeatureClassDescription);

          // Execute the DDL
          bool success = schemaBuilder.Build();

          // Inspect error messages
          if (!success)
          {
            IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
            //etc.
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField()
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.ShapeDescription
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.AllowBorderOverlap
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.PlacementQuality
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.DrawUnplacedLabels
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.InvertedLabelTolerance
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.RotateLabelWithDisplay
      // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.UnplacedLabelColor
      // cref: ArcGIS.Core.CIM.MaplexQualityType
      // cref: ArcGIS.Core.CIM.CIMLabelClass
      // cref: ArcGIS.Core.CIM.LabelExpressionEngine
      // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.AllowOverlappingLabels
      // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.LineOffset
      // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AlignLabelToLineDirection
      // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AvoidPolygonHoles
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsAutoCreate
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsSymbolIDRequired
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsUpdatedOnShapeChange
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      #region Creating a feature-linked annotation feature class
      {
        Task CreateFeatureLinkedAnnotationFeatureClassAsync(Geodatabase geodatabase,
          SpatialReference spatialReference) => QueuedTask.Run(() =>
        {
          // Creating a feature-linked annotation feature class between water pipe and valve in water distribution network
          // with following user defined fields
          // PipeName
          // GlobalID

          // Annotation feature class name
          string annotationFeatureClassName = "WaterPipeAnnotation";

          // Create user defined attribute fields for annotation feature class
          FieldDescription pipeGlobalID = FieldDescription.CreateGlobalIDField();
          FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 255);

          // Create a list of all field descriptions
          List<FieldDescription> fieldDescriptions = new List<FieldDescription>
              { pipeGlobalID, nameFieldDescription };

          // Create a ShapeDescription object
          ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Polygon, spatialReference);

          // Create general placement properties for Maplex engine
          CIMMaplexGeneralPlacementProperties generalPlacementProperties = new CIMMaplexGeneralPlacementProperties
          {
            AllowBorderOverlap = true,
            PlacementQuality = MaplexQualityType.High,
            DrawUnplacedLabels = true,
            InvertedLabelTolerance = 1.0,
            RotateLabelWithDisplay = true,
            UnplacedLabelColor = new CIMRGBColor
            {
              R = 255,
              G = 0,
              B = 0,
              Alpha = 0.5f
            }
          };

          // Create annotation  label classes
          // Green label
          CIMLabelClass greenLabelClass = new CIMLabelClass
          {
            Name = "Green",
            ExpressionTitle = "Expression-Green",
            ExpressionEngine = LabelExpressionEngine.Arcade,
            Expression = "$feature.OBJECTID",
            ID = 1,
            Priority = 0,
            Visibility = true,
            TextSymbol = new CIMSymbolReference
            {
              Symbol = new CIMTextSymbol()
              {
                Angle = 45,
                FontType = FontType.Type1,
                FontFamilyName = "Tahoma",
                FontEffects = FontEffects.Normal,
                HaloSize = 2.0,

                Symbol = new CIMPolygonSymbol
                {
                  SymbolLayers = new CIMSymbolLayer[]
                  {
                      new CIMSolidFill
                      {
                        Color = CIMColor.CreateRGBColor(0, 255, 0)
                      }
                  },
                  UseRealWorldSymbolSizes = true
                }
              },
              MaxScale = 0,
              MinScale = 0,
              SymbolName = "TextSymbol-Green"
            },
            StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
            {
              AllowOverlappingLabels = true,
              LineOffset = 1.0
            },
            MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
            {
              AlignLabelToLineDirection = true,
              AvoidPolygonHoles = true
            }
          };

          // Blue label
          CIMLabelClass blueLabelClass = new CIMLabelClass
          {
            Name = "Blue",
            ExpressionTitle = "Expression-Blue",
            ExpressionEngine = LabelExpressionEngine.Arcade,
            Expression = "$feature.OBJECTID",
            ID = 2,
            Priority = 0,
            Visibility = true,
            TextSymbol = new CIMSymbolReference
            {
              Symbol = new CIMTextSymbol()
              {
                Angle = 45,
                FontType = FontType.Type1,
                FontFamilyName = "Consolas",
                FontEffects = FontEffects.Normal,
                HaloSize = 2.0,

                Symbol = new CIMPolygonSymbol
                {
                  SymbolLayers = new CIMSymbolLayer[]
                  {
                      new CIMSolidFill
                      {
                        Color = CIMColor.CreateRGBColor(0, 0, 255)
                      }
                  },
                  UseRealWorldSymbolSizes = true
                }
              },
              MaxScale = 0,
              MinScale = 0,
              SymbolName = "TextSymbol-Blue"
            },
            StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
            {
              AllowOverlappingLabels = true,
              LineOffset = 1.0
            },
            MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
            {
              AlignLabelToLineDirection = true,
              AvoidPolygonHoles = true
            }
          };

          // Create a list of labels
          List<CIMLabelClass> labelClasses = new List<CIMLabelClass> { greenLabelClass, blueLabelClass };


          // Create linked feature description
          // Linked feature class name
          string linkedFeatureClassName = "WaterPipe";

          // Create fields for water pipe
          FieldDescription waterPipeGlobalID = FieldDescription.CreateGlobalIDField();
          FieldDescription pipeName = FieldDescription.CreateStringField("PipeName", 255);

          // Create a list of water pipe field descriptions
          List<FieldDescription> pipeFieldDescriptions = new List<FieldDescription> { waterPipeGlobalID, pipeName };

          // Create a linked feature class description
          FeatureClassDescription linkedFeatureClassDescription = new FeatureClassDescription(linkedFeatureClassName,
            pipeFieldDescriptions,
            new ShapeDescription(GeometryType.Polyline, spatialReference));

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Add the creation of the linked feature class to the list of DDL tasks
          FeatureClassToken linkedFeatureClassToken = schemaBuilder.Create(linkedFeatureClassDescription);

          // Create an annotation feature class description object to describe the feature class to create
          AnnotationFeatureClassDescription annotationFeatureClassDescription =
            new AnnotationFeatureClassDescription(annotationFeatureClassName, fieldDescriptions, shapeDescription,
              generalPlacementProperties, labelClasses, new FeatureClassDescription(linkedFeatureClassToken))
            {
              IsAutoCreate = true,
              IsSymbolIDRequired = false,
              IsUpdatedOnShapeChange = true
            };

          // Add the creation of the annotation feature class to the list of DDL tasks
          schemaBuilder.Create(annotationFeatureClassDescription);

          // Execute the DDL
          bool success = schemaBuilder.Build();

          // Inspect error messages
          if (!success)
          {
            IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
            //etc.
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription, ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.FeatureDatasetDescription
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsAutoCreate
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsSymbolIDRequired
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsUpdatedOnShapeChange
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.FeatureClassDefinition.GetSpatialReference()
      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClass.GetDefinition()
      #region Creating an annotation feature class inside feature dataset
      {
        Task CreateAnnotationFeatureClassUsingExistingAnnotationFeatureClassInDatasetAsync(
          Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Create a Cities annotation feature class inside Places feature dataset using existing annotation feature class

          // Feature dataset name
          string featureDatasetName = "Places";

          // Annotation feature class name
          string annotationFeatureClassName = "CitiesAnnotation";

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Open existing annotation feature class name
          using (AnnotationFeatureClass existingAnnotationFeatureClass = geodatabase.OpenDataset<AnnotationFeatureClass>("ExistingAnnotationFeatureClass"))
          {
            // Create Feature dataset description
            FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetName, existingAnnotationFeatureClass.GetDefinition().GetSpatialReference());

            // Add the creation of the Places dataset to DDL task
            FeatureDatasetToken featureDatasetToken = schemaBuilder.Create(featureDatasetDescription);

            // Create an annotation feature class description using existing annotation feature class
            AnnotationFeatureClassDescription annotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassName,
                existingAnnotationFeatureClass.GetDefinition())
            {
              IsAutoCreate = true,
              IsSymbolIDRequired = false,
              IsUpdatedOnShapeChange = true
            };

            // Add the creation of the Cities annotation feature class inside Places feature dataset
            schemaBuilder.Create(new FeatureDatasetDescription(featureDatasetToken),
              annotationFeatureClassDescription);

            // Execute the DDL
            bool success = schemaBuilder.Build();

            // Inspect error messages
            if (!success)
            {
              IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
              //etc.
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.DimensionFeatureClassDescription )
      // cref: ArcGIS.Core.Data.DDL.DimensionFeatureClassDescription
      // cref: ArcGIS.Core.CIM.CIMDimensionStyle
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      #region Creating  a dimension feature class
      {
        Task CreateDimensionFeatureClassAsync(Geodatabase geodatabase) => QueuedTask.Run(() =>
        {
          // Dimension feature class name
          string dimensionFeatureClassName = "LineInfo";

          // Create user defined attribute fields for dimension feature class
          FieldDescription nameFieldDescription = new FieldDescription("Name", FieldType.String);
          FieldDescription distanceFieldDescription = new FieldDescription("DistanceMeasure", FieldType.Double);
          FieldDescription measurementDateFieldDescription = new FieldDescription("MeasurementDate", FieldType.Date);

          // Create a list of all field descriptions
          List<FieldDescription> fieldDescriptions = new List<FieldDescription>
              { nameFieldDescription, distanceFieldDescription, measurementDateFieldDescription };

          // Create a ShapeDescription object
          ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Polygon, SpatialReferences.WGS84);

          // Create a SchemaBuilder object
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

          // Create DimensionFeatureClassDescription object
          DimensionFeatureClassDescription dimensionFeatureClassDescription = new DimensionFeatureClassDescription(
            dimensionFeatureClassName, fieldDescriptions,
            shapeDescription, GetCIMDimensionStyles());

          // Token
          DimensionFeatureClassToken dimensionFeatureClassToken =
            schemaBuilder.Create(dimensionFeatureClassDescription);

          // Execute the DDL
          bool success = schemaBuilder.Build();

          // Inspect error messages
          if (!success)
          {
            IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
            //etc.
          }

          List<CIMDimensionStyle> GetCIMDimensionStyles()
          {
            CIMColor color = CIMColor.CreateRGBColor(10, 20, 30);
            CIMPointSymbol beginMarkerSymbol = GetMarkerSymbol(color);
            CIMPointSymbol endMarkerSymbol = GetMarkerSymbol(color);
            CIMLineSymbol dimensionLineSymbol = GetLineSymbol(color, 0.5);
            CIMLineSymbol extensionLineSymbol = GetLineSymbol(color, 0.5);
            CIMTextSymbol textSymbol = GetDefaultTextSymbol(10);
            textSymbol.Symbol = GetPolygonSymbol(color, color, 0);
            textSymbol.HorizontalAlignment = HorizontalAlignment.Center;

            CIMDimensionStyle cimDimensionStyle = new CIMDimensionStyle()
            {
              Name = "Style 1",
              ID = 0,
              Align = true,
              DisplayUnits = null,
              DisplayPrecision = 0,

              BeginMarkerSymbol = beginMarkerSymbol,
              EndMarkerSymbol = endMarkerSymbol,
              MarkerOption = DimensionPartOptions.Both,
              MarkerFit = DimensionMarkerFit.Text,

              DimensionLineSymbol = dimensionLineSymbol,
              DimensionLineOption = DimensionPartOptions.Both,
              DrawLineOnFit = false,
              ExtendLineOnFit = true,

              BaselineHeight = 0.0,
              ExtensionLineSymbol = extensionLineSymbol,
              ExtensionLineOption = DimensionPartOptions.Both,
              ExtensionLineOvershot = 0.0,
              ExtensionLineOffset = 0.0,

              Expression = "",
              ExpressionParserName = "Arcade",

              TextSymbol = textSymbol,
              TextOption = DimensionTextOption.Only,
              TextFit = DimensionTextFit.MoveBegin,
            };

            List<CIMDimensionStyle> cimDimensionStyles = [cimDimensionStyle];

            return cimDimensionStyles;
          }

          CIMFill DefaultFill(CIMColor color)
          {
            return new CIMSolidFill()
            {
              Enable = true,
              Name = "Fill_" + Guid.NewGuid().ToString(),
              ColorLocked = false,
              Color = color
            };
          }

          CIMStroke DefaultStroke(CIMColor color, double width = 1)
          {
            return new CIMSolidStroke()
            {
              Color = color,
              Name = "Stroke_" + Guid.NewGuid().ToString(),
              CapStyle = LineCapStyle.Round,
              JoinStyle = LineJoinStyle.Round,
              CloseCaps3D = false,
              LineStyle3D = Simple3DLineStyle.Strip,
              MiterLimit = 4,
              Width = width,

              ColorLocked = false,
              Enable = true,
            };
          }

          CIMPolygonSymbol GetPolygonSymbol(CIMColor fillColor, CIMColor outlineColor, double outlineWidth)
          {
            if ((outlineColor == null) || (outlineWidth <= 0))
            {
              return new CIMPolygonSymbol() { SymbolLayers = new CIMSymbolLayer[1] { DefaultFill(fillColor) } };
            }

            return new CIMPolygonSymbol()
            {
              SymbolLayers = new CIMSymbolLayer[2]
                { DefaultStroke(outlineColor, outlineWidth), DefaultFill(fillColor) }
            };
          }

          CIMLineSymbol GetLineSymbol(CIMColor color, double width)
          {
            CIMStroke stroke = DefaultStroke(color, width);

            return new CIMLineSymbol() { SymbolLayers = new CIMSymbolLayer[1] { stroke } };
          }

          Polygon CreateArrowGeometry()
          {
            double x = 2.39;
            double y = 1.20;
            double xMod = 0.3;

            Coordinate2D pt1 = new Coordinate2D() { X = -x + xMod, Y = -y };
            Coordinate2D pt2 = new Coordinate2D() { X = x + xMod, Y = 0 };
            Coordinate2D pt3 = new Coordinate2D() { X = -x + xMod, Y = y };
            List<Coordinate2D> coords = new List<Coordinate2D>() { pt1, pt2, pt3 };

            Polygon polygon = PolygonBuilderEx.CreatePolygon(coords);
            return polygon;
          }

          CIMPointSymbol GetMarkerSymbol(CIMColor color)
          {
            CIMMarkerGraphic graphic = new CIMMarkerGraphic()
            {
              Geometry = CreateArrowGeometry(),
              Symbol = new CIMPolygonSymbol()
              {
                SymbolLayers = new CIMSymbolLayer[1]
                {
                    new CIMSolidFill()
                    {
                      Enable = true,
                      Color = color,
                    }
                },
                UseRealWorldSymbolSizes = false
              }
            };

            CIMSymbolLayer symbolLayer = new CIMVectorMarker()
            {
              Enable = true,
              Size = 12,
              Frame = new EnvelopeBuilderEx() { XMin = -5, YMin = -3, XMax = 5, YMax = 3 }.ToGeometry() as Envelope,
              MarkerGraphics = new CIMMarkerGraphic[1] { graphic },
              ScaleSymbolsProportionally = true,
              RespectFrame = true
            };

            return new CIMPointSymbol()
            {
              SymbolLayers = new CIMSymbolLayer[1] { symbolLayer },
              HaloSize = 1,
              ScaleX = 1,
              AngleAlignment = AngleAlignment.Map,
            };
          }

          CIMTextSymbol GetDefaultTextSymbol(double height)
          {
            return new CIMTextSymbol()
            {
              DrawGlyphsAsGeometry = false,
              DrawSoftHyphen = false,
              ExtrapolateBaselines = true,
              FlipAngle = 0.0,
              IndentAfter = 0.0,
              IndentBefore = 0.0,
              IndentFirstLine = 0.0,
              Kerning = true,
              LetterSpacing = 0.0,
              LetterWidth = 100.0,
              Ligatures = true,
              LineGap = 0.0,
              LineGapType = LineGapType.ExtraLeading,
              Underline = false,
              Strikethrough = false,
              OffsetX = 0.0,
              OffsetY = 0.0,
              FontFamilyName = "Tahoma",
              FontStyleName = "Regular",
              WordSpacing = 100.0,
              Height = height
            };
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Creating the memory geodatabase
      {
        await QueuedTask.Run(() =>
        {
          // Creates the default memory geodatabase if not exist or connects to an existing one if already exists
          Geodatabase memoryGeodatabase = new Geodatabase(new MemoryConnectionProperties());

          // Creates schema
          SchemaBuilder schemaBuilder = new SchemaBuilder(memoryGeodatabase);
          schemaBuilder.Create(new TableDescription("MyTable", new List<FieldDescription>()));
          schemaBuilder.Build();

          return memoryGeodatabase;
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.SpatialQueryFilter.SpatialRelationship
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.SpatialRelationshipDescription
      // cref: ArcGIS.Core.Data.Feature.GetShape()
      // cref: ArcGIS.Core.Data.RowCursor.Current
      // cref: ArcGIS.Core.Data.RowCursor.MoveNext()
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.FilterGeometry
      // cref: ArcGIS.Core.Data.Table.Search(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Spatial query filter with DE9-IM spatial relationships
      {
        Task FindSpatiallyRelatedFeaturesUsingDE9IMPredicateAsync(Geodatabase geodatabase, FeatureClass polygonFeatureClass, FeatureClass polylineFeatureClass) => QueuedTask.Run(() =>
        {
          using (RowCursor polygonRowCursor = polygonFeatureClass.Search(new QueryFilter()))
          {
            if (polygonRowCursor.MoveNext())
            {
              using (Feature polygonFeature = polygonRowCursor.Current as Feature)
              {
                // DE9IM predicate string to find overlapping features
                string overlappingDE9IM = "1*T***T**";

                SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter()
                {
                  FilterGeometry = polygonFeature.GetShape(),
                  SpatialRelationship = SpatialRelationship.Relation,
                  SpatialRelationshipDescription = overlappingDE9IM
                };

                using (RowCursor overlappingPolyline = polylineFeatureClass.Search(spatialQueryFilter))
                {
                  while (overlappingPolyline.MoveNext())
                  {
                    // Overlapping polylines on the polygon
                  }
                }
              }
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
      // cref: ArcGIS.Core.Data.RegistrationType
      // cref: ArcGIS.Core.Data.Dataset.GetRegistrationType()
      #region Check if table is versioned
      {
        Task<bool> IsTableVersionedAsync(Geodatabase geodatabase, string tableName) => QueuedTask.Run(() =>
        {
          using (Table table = geodatabase.OpenDataset<Table>(tableName))
          {
            // Check table version type
            RegistrationType registrationType = table.GetRegistrationType();
            if (registrationType == RegistrationType.Versioned)
            {
              return true;
            }
          }
          return false;
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(IndexDescription)
      // cref: ArcGIS.Core.Data.DDL.Description.Name
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Creating a table with index from scratch
      {
        Task CreatingTableWithIndexAsync(SchemaBuilder schemaBuilder) => QueuedTask.Run(() =>
        {
          FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 50);
          FieldDescription addressFieldDescription = FieldDescription.CreateStringField("Address", 200);

          // Creating a feature class, 'Buildings' with two fields
          TableDescription tableDescription = new TableDescription("Buildings",
            new List<FieldDescription>() { nameFieldDescription, addressFieldDescription });

          // Enqueue DDL operation to create a table
          TableToken tableToken = schemaBuilder.Create(tableDescription);

          // Creating an attribute index named as 'Idx'
          AttributeIndexDescription attributeIndexDescription = new AttributeIndexDescription("Idx", new TableDescription(tableToken),
            new List<string> { nameFieldDescription.Name, addressFieldDescription.Name });

          // Enqueue DDL operation to create an attribute index
          schemaBuilder.Create(attributeIndexDescription);

          // Execute build indexes operation
          bool isBuildSuccess = schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(IndexDescription)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
      // cref: ArcGIS.Core.Data.Field.AliasName
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      #region Adding indexes in pre-existing dataset
      {
        Task AddingIndexesAsync(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition) => QueuedTask.Run(() =>
        {
          // Field names to add in the attribute index
          string fieldName = featureClassDefinition.GetFields().First(f => f.AliasName.Contains("Name")).Name;
          string fieldAddress = featureClassDefinition.GetFields().First(f => f.AliasName.Contains("Address")).Name;

          // Creating an attribute index with index name 'Idx' and two participating fields' name
          AttributeIndexDescription attributeIndexDescription = new AttributeIndexDescription("Idx",
            new TableDescription(featureClassDefinition), new List<string> { fieldName, fieldAddress });

          // Enqueue DDL operation for an attribute index creation
          schemaBuilder.Create(attributeIndexDescription);

          // Creating the spatial index
          SpatialIndexDescription spatialIndexDescription = new SpatialIndexDescription(new FeatureClassDescription(featureClassDefinition));

          // Enqueue DDL operation for the spatial index creation
          schemaBuilder.Create(spatialIndexDescription);

          // Execute build indexes operation
          bool isBuildSuccess = schemaBuilder.Build();

          if (!isBuildSuccess)
          {
            IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
            // Iterate and handle errors
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Index.GetName()
      // cref: ArcGIS.Core.Data.TableDefinition.GetIndexes()
      #region Removing attribute index
      {
        Task RemoveAttributeIndexAsync(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition, string attributeIndexName) => QueuedTask.Run(() =>
        {
          // Find a index to be removed
          ArcGIS.Core.Data.Index indexToRemove = featureClassDefinition.GetIndexes().First(f => f.GetName().Equals(attributeIndexName));

          // Index description of the index to be removed
          AttributeIndexDescription indexDescriptionToRemove = new AttributeIndexDescription(indexToRemove, new TableDescription(featureClassDefinition));

          // Enqueue the DDL operation to remove index
          schemaBuilder.Delete(indexDescriptionToRemove);

          // Execute the delete index operation
          bool isDeleteIndexSuccess = schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Removing spatial index
      {
        Task RemoveSpatialIndexAsync(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition) => QueuedTask.Run(() =>
        {
          // Create a spatial description
          SpatialIndexDescription spatialIndexDescription = new SpatialIndexDescription(new FeatureClassDescription(featureClassDefinition));

          // Enqueue the DDL operation to remove index
          schemaBuilder.Delete(spatialIndexDescription);

          // Execute the delete index operation
          bool isDeleteIndexSuccess = schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.DomainDescription)
      // cref: ArcGIS.Core.Data.DDL.CodedValueDomainDescription.CodedValuePairs
      // cref: ArcGIS.Core.Data.DDL.DomainDescription.Description
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Domain.GetName()
      // cref: ArcGIS.Core.Data.Geodatabase.GetDomains()
      #region Modifying domain
      {
        Task ModifyDomainAsync(Geodatabase geodatabase, string codedValueDomainName = "Pipe") => QueuedTask.Run(() =>
        {
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);
          CodedValueDomain codedValueDomain = geodatabase.GetDomains().First(f => f.GetName().Equals(codedValueDomainName)) as CodedValueDomain;
          CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription(codedValueDomain);

          // Update domain description
          codedValueDomainDescription.Description = "Water Pipe Domain";

          // Adding code/value pair
          codedValueDomainDescription.CodedValuePairs.Add("C", "Copper");

          schemaBuilder.Modify(codedValueDomainDescription);

          // To modify the orders of coded value domain
          // schemaBuilder.Modify(codedValueDomainDescription,SortBy.Name,SortOrder.Ascending);

          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Rename(ArcGIS.Core.Data.DDL.Description,System.String)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Domain.GetName()
      // cref: ArcGIS.Core.Data.Geodatabase.GetDomains()
      #region Rename domain
      {
        Task RenameDomainAsync(Geodatabase geodatabase, string rangeDomainOldName = "PipeDiameter", string rangeDomainNewName = "PipeDiam") => QueuedTask.Run(() =>
        {
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);
          RangeDomain rangeDomain = geodatabase.GetDomains().First(f => f.GetName().Equals(rangeDomainOldName)) as RangeDomain;

          // Renaming a domain
          schemaBuilder.Rename(new RangeDomainDescription(rangeDomain), rangeDomainNewName);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Domain.GetName()
      // cref: ArcGIS.Core.Data.Geodatabase.GetDomains()
      #region Delete domain
      {
        Task DeleteDomainAsync(Geodatabase geodatabase, string domainNameToBeDeleted = "PipeMaterial") => QueuedTask.Run(() =>
        {
          SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);
          CodedValueDomain codedValueDomain = geodatabase.GetDomains().First(f => f.GetName().Equals(domainNameToBeDeleted)) as CodedValueDomain;
          CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription(codedValueDomain);

          // Deleting a coded value domain
          schemaBuilder.Delete(codedValueDomainDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.Description.Name
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.SubtypeFieldDescription.DefaultSubtypeCode
      #region Creating table with subtypes
      {
        Task CreateTableWithSubtypesAsync(SchemaBuilder schemaBuilder) => QueuedTask.Run(() =>
        {
          // Creating a 'Building' table with the subtype field 'BuildingType'
          FieldDescription buildingType = new FieldDescription("BuildingType", FieldType.Integer);
          FieldDescription buildingName = new FieldDescription("Name", FieldType.String);

          TableDescription tableDescription = new TableDescription("Building", new List<FieldDescription> { buildingName, buildingType });

          // Add the building type subtype with three subtypes - Business, Marketing, Security
          tableDescription.SubtypeFieldDescription = new SubtypeFieldDescription(buildingType.Name,
            new Dictionary<int, string> { { 1, "Business" }, { 2, "Marketing" }, { 3, "Security" } })
          {
            DefaultSubtypeCode = 3 // Assigning 'Security' building type as the default subtype
          };

          schemaBuilder.Create(tableDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.TableDescription.SubtypeFieldDescription
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.FeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Removing subtype field designation
      {
        Task DeleteSubtypeFieldAsync(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition) => QueuedTask.Run(() =>
        {
          FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassDefinition);

          // Set subtype field to null to remove the subtype field designation
          featureClassDescription.SubtypeFieldDescription = null;

          schemaBuilder.Modify(featureClassDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Subtype.GetCode()
      // cref: ArcGIS.Core.Data.TableDefinition.GetSubtypes()
      #region Modifying subtypes
      {
        Task ModifySubtypesAsync(SchemaBuilder schemaBuilder, TableDefinition tableDefinition) => QueuedTask.Run(() =>
        {
          TableDescription tableDescription = new TableDescription(tableDefinition);

          // Remove the first subtype from the table
          IReadOnlyList<Subtype> subtypes = tableDefinition.GetSubtypes();
          tableDescription.SubtypeFieldDescription.Subtypes.Remove(subtypes.First().GetCode());

          // Adding a new subtype, 'Utility', in the existing table
          tableDescription.SubtypeFieldDescription.Subtypes.Add(4, "Utility");

          // Assigning 'Utility' subtype as the default subtype
          tableDescription.SubtypeFieldDescription.DefaultSubtypeCode = 4;

          schemaBuilder.Modify(tableDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(RelationshipClassDescription)
      // cref: ArcGIS.Core.Data.DDL.Description.Name
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateIntegerField(System.String)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.RelationshipClassDescription.RelationshipRuleDescriptions
      // cref: ArcGIS.Core.Data.DDL.RelationshipClassDescription.RelationshipType
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.TableDescription.SubtypeFieldDescription
      #region Creating relationship class
      {
        Task CreateRelationshipWithRelationshipRulesAsync(SchemaBuilder schemaBuilder) => QueuedTask.Run(() =>
        {
          // Creating a 'BuildingType' table with two fields - BuildingType and BuildingTypeDescription
          FieldDescription buildingType = FieldDescription.CreateIntegerField("BuildingType");
          FieldDescription buildingTypeeDescription = FieldDescription.CreateStringField("BuildingTypeDescription", 100);
          TableDescription buildingTypeDescription = new TableDescription("BuildingType",
            new List<FieldDescription>() { buildingType, buildingTypeeDescription });
          TableToken buildingtypeToken = schemaBuilder.Create(buildingTypeDescription);

          // Creating a 'Building' feature class with three fields - BuildingId, Address, and BuildingType
          FieldDescription buildingId = FieldDescription.CreateIntegerField("BuildingId");
          FieldDescription buildingAddress = FieldDescription.CreateStringField("Address", 100);
          FieldDescription usageSubType = FieldDescription.CreateIntegerField("UsageSubtype");
          FeatureClassDescription featureClassDescription = new FeatureClassDescription("Building",
            new List<FieldDescription> { buildingId, buildingAddress, buildingType, usageSubType },
            new ShapeDescription(GeometryType.Polygon, SpatialReferences.WGS84));

          // Set subtype details (optional)
          featureClassDescription.SubtypeFieldDescription = new SubtypeFieldDescription(usageSubType.Name,
            new Dictionary<int, string> { { 1, "Marketing" }, { 2, "Utility" } });

          FeatureClassToken buildingToken = schemaBuilder.Create(featureClassDescription);

          // Creating a 1:M relationship between the 'Building' feature class and 'BuildingType' table
          RelationshipClassDescription relationshipClassDescription = new RelationshipClassDescription(
            "BuildingToBuildingType", new FeatureClassDescription(buildingToken), new TableDescription(buildingtypeToken),
            RelationshipCardinality.OneToMany, buildingType.Name, buildingType.Name)
          {
            RelationshipType = RelationshipType.Composite
          };

          // Adding relationship rules for the 'Marketing' subtype
          relationshipClassDescription.RelationshipRuleDescriptions.Add(new RelationshipRuleDescription(1, null));

          schemaBuilder.Create(relationshipClassDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(AttributedRelationshipClassDescription)
      // cref: ArcGIS.Core.Data.DDL.AttributedRelationshipClassDescription.FieldDescriptions
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateIntegerField(System.String)
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Creating attributed relationship class
      {
        Task CreateAttributedRelationshipAsync(SchemaBuilder schemaBuilder) => QueuedTask.Run(() =>
        {
          // Creating a 'BuildingType' table with two fields - BuildingType and BuildingTypeDescription
          FieldDescription buildingType = FieldDescription.CreateIntegerField("BuildingType");
          FieldDescription buildingTypeeDescription = FieldDescription.CreateStringField("BuildingTypeDescription", 100);
          TableDescription buildingTypeDescription = new TableDescription("BuildingType",
            new List<FieldDescription>() { buildingType, buildingTypeeDescription });
          TableToken buildingtypeToken = schemaBuilder.Create(buildingTypeDescription);

          // Creating a 'Building' feature class with three fields - BuildingId, Address, and BuildingType
          FieldDescription buildingId = FieldDescription.CreateIntegerField("BuildingId");
          FieldDescription buildingAddress = FieldDescription.CreateStringField("Address", 100);
          FeatureClassDescription featureClassDescription = new FeatureClassDescription("Building",
            new List<FieldDescription> { buildingId, buildingAddress, buildingType },
            new ShapeDescription(GeometryType.Polygon, SpatialReferences.WGS84));
          FeatureClassToken buildingToken = schemaBuilder.Create(featureClassDescription);

          // Creating M:M relationship between the 'Building' feature class and 'BuildingType' table
          AttributedRelationshipClassDescription attributedRelationshipClassDescription =
            new AttributedRelationshipClassDescription("BuildingToBuildingType",
              new FeatureClassDescription(buildingToken),
              new TableDescription(buildingtypeToken), RelationshipCardinality.ManyToMany, "OBJECTID", "BuildingID",
              "OBJECTID", "BuildingTypeID");

          // Adding optional attribute field in the intermediate table - 'OwnershipPercentage' field
          attributedRelationshipClassDescription.FieldDescriptions.Add(FieldDescription.CreateIntegerField("OwnershipPercentage"));

          schemaBuilder.Create(attributedRelationshipClassDescription);
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.RelationshipClassDescription.RelationshipRuleDescriptions
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(AttributedRelationshipClassDescription)
      // cref: ArcGIS.Core.Data.DDL.AttributedRelationshipClassDescription.FieldDescriptions
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateIntegerField(System.String)
      // cref: ArcGIS.Core.Data.DDL.RelationshipClassDescription.RelationshipSplitPolicy
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Add relationship rules to a relationship class
      {
        Task ModifyRelationshipClassAsync(SchemaBuilder schemaBuilder, AttributedRelationshipClassDefinition attributedRelationshipClassDefinition) => QueuedTask.Run(() =>
        {
          AttributedRelationshipClassDescription attributedRelationshipClassDescription = new AttributedRelationshipClassDescription(attributedRelationshipClassDefinition);

          // Update the relationship split policy
          attributedRelationshipClassDescription.RelationshipSplitPolicy = RelationshipSplitPolicy.UseDefault;

          // Add field in the intermediate table
          attributedRelationshipClassDescription.FieldDescriptions.Add(FieldDescription.CreateIntegerField("RelationshipStatus"));

          // Add relationship rules based on subtypes,if available
          // Assuming origin class has subtype with code 1
          attributedRelationshipClassDescription.RelationshipRuleDescriptions.Add(
            new RelationshipRuleDescription(1, null));

          // Enqueue modify operation
          schemaBuilder.Modify(attributedRelationshipClassDescription);

          // Execute modify DDL operation
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
      // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Deleting a relationship class
      {
        Task DeleteRelationshipClassAsync(SchemaBuilder schemaBuilder, RelationshipClassDefinition relationshipClassDefinition) => QueuedTask.Run(() =>
        {
          schemaBuilder.Delete(new RelationshipClassDescription(relationshipClassDefinition));
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.RemoveRelationshipClass(FeatureDatasetDescription,RelationshipClassDescription)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Adding/Removing Relationship class in/out of a feature dataset
      {
        Task MoveRelationshipClassAsync(SchemaBuilder schemaBuilder, FeatureDatasetDefinition featureDatasetDefinition, RelationshipClassDefinition relationshipClassDefinition) => QueuedTask.Run(() =>
        {
          FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetDefinition);
          RelationshipClassDescription relationshipClassDescription = new RelationshipClassDescription(relationshipClassDefinition);

          // Remove relationship class from the feature dataset
          schemaBuilder.RemoveRelationshipClass(featureDatasetDescription, relationshipClassDescription);

          // Add relationship class inside the feature dataset
          // schemaBuilder.AddRelationshipClass(featureDatasetDescription, relationshipClassDescription);

          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription)
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.GeneralPlacementProperties
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.LabelClasses
      // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.Symbols
      // cref: ArcGIS.Core.Data.DDL.Description.Name
      // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription.ShapeDescription
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.DDL.TableDescription.FieldDescriptions
      #region Modifying annotation labels and symbols
      {
        Task ModifyAnnotationLabelAndSymbolsAsync(SchemaBuilder schemaBuilder, AnnotationFeatureClassDefinition annotationFeatureClassDefinition) => QueuedTask.Run(() =>
        {
          AnnotationFeatureClassDescription annotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassDefinition);
          IReadOnlyList<CIMLabelClass> labelClasses = annotationFeatureClassDescription.LabelClasses;

          // Adding a new annotation label class
          List<CIMLabelClass> modifiedLabelClasses = new List<CIMLabelClass>(labelClasses);
          modifiedLabelClasses.Add(new CIMLabelClass()
          {
            Name = "RedSymbol",
            TextSymbol = new CIMSymbolReference
            {
              Symbol = new CIMTextSymbol()
              {
                Angle = 45,
                FontType = FontType.Type1,
                FontFamilyName = "Arial",
                FontEffects = FontEffects.Normal,
                HaloSize = 2.0,

                Symbol = new CIMPolygonSymbol
                {
                  SymbolLayers = new CIMSymbolLayer[]
                    { new CIMSolidFill { Color = CIMColor.CreateRGBColor(255, 0, 0) } },
                  UseRealWorldSymbolSizes = true
                }
              },
              MaxScale = 0,
              MinScale = 0,
              SymbolName = "TextSymbol-RED"
            }
          });

          // Adding a  new symbol
          annotationFeatureClassDescription.Symbols.Add(new CIMSymbolIdentifier()
          {
            ID = 1001,
            Name = "ID_10001",
            Symbol = new CIMTextSymbol()
            {
              Angle = 43,
              FontEffects = FontEffects.Subscript,
              FontType = FontType.TTOpenType,
              FontStyleName = "Regular",
              FontFamilyName = "Tahoma",
              TextCase = TextCase.Allcaps
            }
          });

          // Modify annotation feature class
          AnnotationFeatureClassDescription modifiedAnnotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassDescription.Name,
              annotationFeatureClassDescription.FieldDescriptions, annotationFeatureClassDescription.ShapeDescription,
              annotationFeatureClassDescription.GeneralPlacementProperties, modifiedLabelClasses);

          // Enqueue modify
          schemaBuilder.Modify(modifiedAnnotationFeatureClassDescription);

          // DDL execute
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Rename(ArcGIS.Core.Data.DDL.Description,System.String)
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      #region Rename annotation feature class
      {
        Task RenameAsync(SchemaBuilder schemaBuilder, AnnotationFeatureClassDefinition annotationFeatureClassDefinition, string featureClassNewName) => QueuedTask.Run(() =>
        {
          AnnotationFeatureClassDescription annotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassDefinition);

          // Enqueue rename operation
          schemaBuilder.Rename(annotationFeatureClassDescription, featureClassNewName);

          // Execute DDL
          schemaBuilder.Build();
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.DDL.FieldDescription.SetDefaultValue(System.Object,System.Nullable{System.Int32})
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.AliasName
      // cref: ArcGIS.Core.Data.DDL.FieldDescription.Length
      // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build()
      // cref: ArcGIS.Core.Data.Field.Name
      // cref: ArcGIS.Core.Data.TableDefinition.GetFields()
      #region Modifying an existing field
      {
        Task ModifyExistingFieldAsync(SchemaBuilder schemaBuilder, TableDefinition tableDefinition, string fieldNameToBeModified = "PropertyAddress") => QueuedTask.Run(() =>
        {
          Field field = tableDefinition.GetFields().FirstOrDefault(f => f.Name.Contains(fieldNameToBeModified));

          // Update field's alias name and length
          FieldDescription fieldDescription = new FieldDescription(field)
          {
            AliasName = "Physical Property Address",
            Length = 50
          };

          // Update the default value
          fieldDescription.SetDefaultValue("123 Main St");

          // Enqueue modify operation
          schemaBuilder.Modify(new TableDescription(tableDefinition), field.Name, fieldDescription);

          // Execute DDL
          schemaBuilder.Build();
        });
      }
      #endregion

    }
  }
}
