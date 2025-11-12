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
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Desktop.Core.Portal;
using ArcGIS.Desktop.Core.UnitFormats;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.GeoProcessing;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippets.ContentSnippets
{
  /// <summary>
  /// Provides a collection of code snippets and examples demonstrating various functionalities  in ArcGIS Pro, such as
  /// working with projects, maps, layouts, geodatabases, and metadata.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with creating text symbols in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsContent
  {
    /// <summary>
    /// Demonstrates various content management functionalities in ArcGIS Pro, including working with project items,
    /// metadata, and geodatabases.
    /// </summary>
    /// <remarks>This method contains a series of code snippets that illustrate how to perform common tasks
    /// related to content management in ArcGIS Pro. These snippets cover a range of topics, including working with
    /// project items, managing metadata, and interacting with geodatabases.
    /// </remarks>
    private static FolderConnectionProjectItem projectfolderConnection;
    private static readonly GDBProjectItem folderToAdd = Project.Current.GetItems<GDBProjectItem>().First();

    public static void ContentProSnippets()
    {
      #region ignore - Variable initialization
      projectfolderConnection = Project.Current.GetItems<FolderConnectionProjectItem>().First();

      IMetadata metadataItemToSaveAsHTML = null;
      IMetadata metadataItemToSaveAsXML = null;
      IMetadata metadataItemToSaveAsUsingCustomXSLT = null;
      IMetadata metadataItemExport = null;
      IMetadata metadataItemImport = null;
      IMetadata gdbMetadataItem = null;
      IMetadata metadataItemToSync = null;
      IMetadata metadataItemToCheck = null;
      IMetadata metadataCopyFrom = null;
      IMetadata featureClassMetadataItem = null;

      string projectName = @"C:\Data\MyProject1\MyProject1.aprx";
      string projectPath = @"C:\Data\MyProject1";
      Item gdb = ItemFactory.Instance.Create(@"E:\CurrentProject\RegionalPolling\polldata.gdb");
      string oldGDBItemPath = @"C:\Path\Project\OldLocation.gdb";
      string newGDDItemPath = @"C:\path\ArcGIS\Project\NewLocation.gdb";
      string newProjectPath = @"C:\Data\MyProject1\MyProject1_New.aprx";
      string oldProjectPath = @"C:\Data\MyProject1\MyProject1.aprx";
      string templatePath = @"C:\Data\MyProject1\CustomTemplate.aptx";
      string newTemplatePath = @"C:\Data\MyProject1\CustomTemplate2.aptx";
      string folderPath = "@C:\\myDataFolder";
      string gdbPath = "@C:\\myDataFolder\\myData.gdb";
      string projectStyleName = "ArcGIS 3D";
      string symbolName = "Cone_Volume_3";
      #endregion

      #region ProSnippet Group: Dialogs
      #endregion

      // cref: ArcGIS.Desktop.Catalog.ItemDialog.Filter
      // cref: ArcGIS.Desktop.Catalog.ItemDialog.InitialLocation
      // cref: ArcGIS.Desktop.Catalog.ItemDialog.Title
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog
      #region OpenItemDialog
      {
        // Variables not used in samples
        OpenItemDialog selectItemDialog = new(); // in #region BrowseDialogItems

        // Adds a single item to a map
        OpenItemDialog addToMapDialog = new()
        {
          Title = "Add To Map",
          InitialLocation = @"C:\Data\NewYork\Counties\Erie\Streets",
          Filter = ItemFilters.Composite_AddToMap
        };
      }
      #endregion //OpenItemDialog

      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
      // cref: ArcGIS.Desktop.Catalog.SaveItemDialog.ShowDialog
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.Items
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.MultiSelect
      #region Show OpenItemDialog
      {
        OpenItemDialog addToProjectDialog = new()
        {
          Title = "Add To Project",
          InitialLocation = @"C:\Data\NewYork\Counties\Maps",
          MultiSelect = true,
          Filter = ItemFilters.Composite_Maps_Import
        };

        bool? ok = addToProjectDialog.ShowDialog();

        if (ok == true)
        {
          IEnumerable<Item> selectedItems = addToProjectDialog.Items;
          foreach (Item selectedItem in selectedItems)
            MapFactory.Instance.CreateMapFromItem(selectedItem);
        }
      }
      #endregion //Show OpenItemDialog

      // cref: ArcGIS.Desktop.Catalog.ItemDialog.Filter
      // cref: ArcGIS.Desktop.Catalog.ItemDialog.InitialLocation
      // cref: ArcGIS.Desktop.Catalog.ItemDialog.Title
      // cref: ArcGIS.Desktop.Catalog.SaveItemDialog
      #region SaveItemDialog
      {
        SaveItemDialog saveLayerFileDialog = new()
        {
          Title = "Save Layer File",
          InitialLocation = @"C:\Data\ProLayers\Geographic\Streets",
          Filter = ItemFilters.Files_All
        };
      }
      #endregion //SaveItemDialog

      // cref: ArcGIS.Desktop.Catalog.SaveItemDialog.DefaultExt
      // cref: ArcGIS.Desktop.Catalog.SaveItemDialog.FilePath
      // cref: ArcGIS.Desktop.Catalog.SaveItemDialog.OverwritePrompt
      #region Show SaveItemDialog
      {
        SaveItemDialog saveMapFileDialog = new()
        {
          Title = "Save Map File",
          InitialLocation = @"C:\Data\NewYork\Counties\Maps",
          DefaultExt = @"mapx",
          Filter = ItemFilters.Maps_All,
          OverwritePrompt = true
        };
        bool? result = saveMapFileDialog.ShowDialog();

        if (result == true)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Returned file name: " + saveMapFileDialog.FilePath);
        }
      }
      #endregion //Show SaveItemDialog

      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.Items
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog
      // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMapFromItem
      #region BrowseDialogItems
      {
        OpenItemDialog addToProjectDialog = new()
        {
          Title = "Add To Project",
          MultiSelect = true,
          Filter = ItemFilters.Composite_Maps_Import
        };
        bool? ok = addToProjectDialog.ShowDialog();
        if (ok == true)
        {
          IEnumerable<Item> selectedDialogItems = addToProjectDialog.Items;
          foreach (Item selectedDialogItem in selectedDialogItems)
            MapFactory.Instance.CreateMapFromItem(selectedDialogItem);
        }
      }
      #endregion //BrowseDialogItems

      #region ProSnippet Group: Retrieving Project Items
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      #region Get MapProjectItems
      {
        // not to be included in sample regions
        var projectFolderConnection = Project.Current.GetItems<FolderConnectionProjectItem>().First();

        // Get all the maps in a project
        IEnumerable<MapProjectItem> projectMaps = Project.Current.GetItems<MapProjectItem>();
      }
      #endregion //GetMapProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      #region Get FolderConnectionProjectItems
      {
        // Get all the folder connections in a project
        IEnumerable<FolderConnectionProjectItem> projectFolders = Project.Current.GetItems<FolderConnectionProjectItem>();
        // use projectFolders;
      }
      #endregion //GetFolderConnectiontProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
      #region Get ServerConnectionProjectItems
      {
        // Get all the server connections in a project
        IEnumerable<ServerConnectionProjectItem> projectServers = Project.Current.GetItems<ServerConnectionProjectItem>();
        // use projectServers;
      }
      #endregion //Get ServerConnectionProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.LocatorsConnectionProjectItem
      #region Get LocatorConnectionProjectItems
      {
        // Get all the locator connections in a project
        IEnumerable<LocatorsConnectionProjectItem> projectLocators = Project.Current.GetItems<LocatorsConnectionProjectItem>();
        // use projectLocators;
      }
      #endregion //Get LocatorConnectionProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Item.GetItems
      #region Get Project Items by ProjectItem type
      {
        // Get all the items that can be accessed from a folder connection. The items immediately 
        // contained by a folder, that is, the folder's children, are returned including folders
        // and individual items that can be used in ArcGIS Pro. This method does not return all 
        // items contained by any sub-folder that can be accessed from the folder connection.
        FolderConnectionProjectItem folderConnection = Project.Current.GetItems<FolderConnectionProjectItem>()
                                                            .FirstOrDefault(folder => folder.Name.Equals("Data"));
        //Note: Needs QueuedTask to run
        IEnumerable<Item> folderContents = folderConnection.GetItems();
      }
      #endregion //GetProjectItems

      #region ProSnippet Group: Working with Project Items
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Add Folder to Project as IProjectItem
      {
        // Add a folder connection to a project

        Item folderToAdd = ItemFactory.Instance.Create(@"C:\Data\Oregon\Counties\Streets");
        // Note: Needs QueuedTask to run
        bool wasAdded = Project.Current.AddItem(folderToAdd as IProjectItem);
        // use wasAdded;
      }
      #endregion //AddFolderConnectionProjectItem

      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Add GDBProjectItem to Project as IProjectItem
      {
        // Add a file geodatabase or a SQLite or enterprise database connection to a project
        Item gdbToAdd = folderToAdd.GetItems().FirstOrDefault(folderItem => folderItem.Name.Equals("CountyData.gdb"));
        // Note: Needs QueuedTask to run
        var addedGeodatabase = Project.Current.AddItem(gdbToAdd as IProjectItem);
        // use addedGeodatabase;
      }
      #endregion //AddGDBProjectItem

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Remove FolderConnection From Project
      {
        // Remove a folder connection from a project; the folder stored on the local disk 
        // or the network is not deleted

        //Note: Needs QueuedTask to run
        {
          bool result = false;
          FolderConnectionProjectItem folderToRemove = Project.Current.GetItems<FolderConnectionProjectItem>()
            .FirstOrDefault(folder => folder.Name.Equals("Data"));
          if (folderToRemove != null)
            result = Project.Current.RemoveItem(folderToRemove as IProjectItem);
          // use result
        }
      }
      #endregion //RemoveFolderConnectionFromProject

      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Remove Map From Project
      {
        // Remove a map from a project; the map is deleted
        IProjectItem mapToRemove = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(map => map.Name.Equals("OldStreetRoutes"));
        // Note: Needs QueuedTask to run
        var removedMapProjectItemResult = Project.Current.RemoveItem(mapToRemove);
        // Use removedMapProjectItemResult
      }
      #endregion //RemoveMapFromProject

      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Importing Maps To Project
      {
        // Import a mxd
        Item mxdToImport = ItemFactory.Instance.Create(@"C:\Projects\RegionalSurvey\LatestResults.mxd");
        // Note: Needs QueuedTask to run
        var addedMxd = Project.Current.AddItem(mxdToImport as IProjectItem);

        // Add map package
        Item mapPackageToAdd = ItemFactory.Instance.Create(@"c:\Data\Map.mpkx");
        // Note: Needs QueuedTask to run
        var addedMapPackage = Project.Current.AddItem(mapPackageToAdd as IProjectItem);

        // Add an exported Pro map
        Item proMapToAdd = ItemFactory.Instance.Create(@"C:\ExportedMaps\Election\Districts.mapx");
        // Note: Needs QueuedTask to run
        var addedMapProjectItem = Project.Current.AddItem(proMapToAdd as IProjectItem);
      }
      #endregion //ImportToProject

      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Item
      #region Create An Item
      {
        Item mxdItem = ItemFactory.Instance.Create(@"C:\Projects\RegionalSurvey\LatestResults.mxd");
        // use mxdItem;
      }
      #endregion //CreateAnItem

      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.ItemFactory.ItemType
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem
      #region Create A PortalItem
      {
        // Creates an Item from an existing portal item base on its ID
        string portalItemID = "9801f878ff4a22738dff3f039c43e395";
        Item portalItem = ItemFactory.Instance.Create(portalItemID, ItemFactory.ItemType.PortalItem);
        // use portalItem;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.ItemFactory.ItemType
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem 
      #region Create A PortalFolder
      {
        // Creates an Item from an existing portal folder base on its ID
        string portalFolderID = "39c43e39f878f4a2279838dfff3f0015";
        Item portalFolder = ItemFactory.Instance.Create(portalFolderID, ItemFactory.ItemType.PortalFolderItem);
        // use portalFolder;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem 
      #region Get Folder Item Content from Project PortalItem
      {
        FolderConnectionProjectItem projectfolderConnection = Project.Current.GetItems<FolderConnectionProjectItem>().First();
        var folderConnectionContent = projectfolderConnection.GetItems();
        var folder = folderConnectionContent.FirstOrDefault(folderItem => folderItem.Name.Equals("Tourist Sites"));
        var folderContents = folder.GetItems();
        //use folderContents;
      }
      #endregion //GetItemContent

      #region ProSnippet Group: Catalog Window
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.GetCatalogPane(System.Boolean)
      //cref: ArcGIS.Desktop.Framework.Contracts.DockPane.Activate
      #region Set the catalog dockpane as the active window
      {
        //cast ICatalogWindow to ArcGIS.Desktop.Framework.Contracts.DockPane
        var catalogWindow = Project.GetCatalogPane() as DockPane;
        //Activate it
        catalogWindow.Activate();
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.ICatalogWindow.IsActiveWindow
      #region Check if the Catalog Window is the active window
      {
        var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
        if (catalogWindow.IsActiveWindow)
        {
          //TODO - query/change catalog window content
        }
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.GetCatalogPane(System.Boolean)
      //cref: ArcGIS.Desktop.Core.ICatalogWindow
      //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentContentType
      //cref: ArcGIS.Desktop.Core.CatalogContentType
      #region Get the catalog content type currently being shown
      {
        //Gets the Catalog pane
        var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
        var catContentType = catalogWindow.GetCurrentContentType();
        // use catContentType;
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentContentType
      //cref: ArcGIS.Desktop.Core.CatalogContentType
      //cref: ArcGIS.Desktop.Core.ICatalogWindow.SetContentTypeAsync(ArcGIS.Desktop.Core.CatalogContentType)
      #region Set the catalog content type
      {
        //Gets the Catalog pane
        var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
        if (!catalogWindow.IsActiveWindow)
        {
          //catalog dockpane must be the active window
        }

        //Change the content to whatever is the next tab
        var catContentType = (int)catalogWindow.GetCurrentContentType();
        catContentType++;

        if (catContentType > (int)CatalogContentType.Favorites)
          catContentType = (int)CatalogContentType.Project;

        //Must be on the UI - no QueuedTask!
        catalogWindow.SetContentTypeAsync((CatalogContentType)catContentType);
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentSecondaryPortalContentType
      //cref: ArcGIS.Desktop.Core.CatalogContentType
      //cref: ArcGIS.Desktop.Core.CatalogSecondaryPortalContentType
      #region Get the secondary portal catalog content
      {
        //Gets the Catalog pane
        var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
        var catContentType = catalogWindow.GetCurrentContentType();
        //Is Portal the content type?
        if (catContentType == CatalogContentType.Portal)
        {
          //check what is the portal content type being shown...
          var secondaryContentType =
            catalogWindow.GetCurrentSecondaryPortalContentType();
          //TODO use secondary portal content type...
        }
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentContentType
      //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentSecondaryPortalContentType
      //cref: ArcGIS.Desktop.Core.CatalogContentType
      //cref: ArcGIS.Desktop.Core.CatalogSecondaryPortalContentType
      //cref: ArcGIS.Desktop.Core.ICatalogWindow.SetSecondaryPortalContentTypeAsync(ArcGIS.Desktop.Core.CatalogSecondaryPortalContentType)
      #region Set the secondary portal catalog content
      {
        //Gets the Catalog pane
        var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
        if (!catalogWindow.IsActiveWindow)
        {
          //catalog dockpane must be the active window
        }

        var catContentType = catalogWindow.GetCurrentContentType();
        //Is portal content being shown?
        if (catContentType == CatalogContentType.Portal)
        {
          //check what is the portal content type being shown...
          var portalContentType = (int)catalogWindow.GetCurrentSecondaryPortalContentType();
          //advance to the next tab
          portalContentType++;
          if (portalContentType > (int)CatalogSecondaryPortalContentType.LivingAtlas)
            portalContentType = (int)CatalogSecondaryPortalContentType.UserContent;

          //set the secondary portal content type
          //Must be on the UI - no QueuedTask!
          catalogWindow.SetSecondaryPortalContentTypeAsync(
            (CatalogSecondaryPortalContentType)portalContentType);
        }
      }
      #endregion

      #region ProSnippet Group: Geodatabase Content 
      #endregion

      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
      // cref: ArcGIS.Desktop.Core.BrowseProjectFilter
      #region Geodatabase Content from Browse Dialog
      {
        var openDlg = new OpenItemDialog
        {
          Title = "Select a Feature Class",
          InitialLocation = @"C:\Data",
          MultiSelect = false,
          BrowseFilter = BrowseProjectFilter.GetFilter(ItemFilters.GeodatabaseItems_All)
        };
        //show the browse dialog
        bool? ok = openDlg.ShowDialog();
        if (!ok.HasValue || openDlg.Items.Count == 0)
        {
          // nothing selected, leave
        }

        // Note: Needs QueuedTask to run
        // get the item
        var item = openDlg.Items.First();
        // see if the item has a dataset
        if (ItemFactory.Instance.CanGetDataset(item))
        {
          // get it
          using var ds = ItemFactory.Instance.GetDataset(item);
          // access some properties
          var name = ds.GetName();
          var path = ds.GetPath();
          // if it's a featureclass
          if (ds is FeatureClass fc)
          {
            // create a layer 
            var featureLayerParams = new FeatureLayerCreationParams(fc)
            {
              MapMemberIndex = 0
            };
            var layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(featureLayerParams, MapView.Active.Map);

            // continue
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.IItemFactory.CanGetDataset
      // cref: ArcGIS.Desktop.Core.ItemFactory.CanGetDataset
      #region Geodatabase Content from Catalog selection
      {
        // subscribe to event
        ProjectWindowSelectedItemsChangedEvent.Subscribe(args =>
        {
          if (args.IProjectWindow.SelectionCount > 0)
          {
            // get the first selected item
            var selectedItem = args.IProjectWindow.SelectedItems.First();

            // Note: Needs QueuedTask to run
            // datasetType
            var dataType = ItemFactory.Instance.GetDatasetType(selectedItem);
            // get the dataset Definition
            if (ItemFactory.Instance.CanGetDefinition(selectedItem))
            {
              using var def = ItemFactory.Instance.GetDefinition(selectedItem);
              if (def is FeatureClassDefinition fcDef)
              {
                var oidField = fcDef.GetObjectIDField();
                var shapeField = fcDef.GetShapeField();
                var shapeType = fcDef.GetShapeType();
              }
              else if (def is ArcGIS.Core.Data.Parcels.ParcelFabricDefinition pfDef)
              {
                string ver = pfDef.GetSchemaVersion();
                bool enabled = pfDef.GetTopologyEnabled();
              }
            }
            // get the dataset
            if (ItemFactory.Instance.CanGetDataset(selectedItem))
            {
              using var ds = ItemFactory.Instance.GetDataset(selectedItem);
              if (ds is FeatureDataset fds)
              {
                // open featureclasses within the feature dataset
                // var fcPoint = fds.OpenDataset<FeatureClass>("Point");
                // var fcPolyline = fds.OpenDataset<FeatureClass>("Polyline");
              }
              else if (ds is FeatureClass fc)
              {
                var name = fc.GetName() + "_copy";
                // create
                var featureLayerParams = new FeatureLayerCreationParams(fc)
                {
                  Name = name,
                  MapMemberIndex = 0
                };
                LayerFactory.Instance.CreateLayer<FeatureLayer>(featureLayerParams, MapView.Active.Map);
              }
              else if (ds is Table table)
              {
                var name = table.GetName() + "_copy";
                var tableParams = new StandaloneTableCreationParams(table)
                {
                  Name = name
                };
                // create
                StandaloneTableFactory.Instance.CreateStandaloneTable(tableParams, MapView.Active.Map);
              }
            }
          }
        });
      }
      #endregion

      #region ProSnippet Group: Favorites
      #endregion

      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.CanAddAsFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.AddFavorite(ArcGIS.Desktop.Core.Item)
      #region Add a Favorite - Folder
      {
        var itemFolder = ItemFactory.Instance.Create(@"d:\data");

        // is the folder item already a favorite?
        var fav = FavoritesManager.Current.GetFavorite(itemFolder);
        if (fav == null)
        {
          if (FavoritesManager.Current.CanAddAsFavorite(itemFolder))
          {
            fav = FavoritesManager.Current.AddFavorite(itemFolder);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.CanAddAsFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.InsertFavorite(ArcGIS.Desktop.Core.Item,Int32)
      // cref: ArcGIS.Desktop.Core.FavoritesManager.InsertFavorite(ArcGIS.Desktop.Core.Item,Int32,Boolean)
      #region Insert a Favorite - Geodatabase path
      {
        gdbPath = "@C:\\myDataFolder\\myData.gdb";
        var itemGDB = ItemFactory.Instance.Create(gdbPath);

        // is the item already a favorite?
        var favorite = FavoritesManager.Current.GetFavorite(itemGDB);
        // no; add it with IsAddedToAllNewProjects set to true
        if (favorite != null)
        {
          if (FavoritesManager.Current.CanAddAsFavorite(itemGDB))
            FavoritesManager.Current.InsertFavorite(itemGDB, 1, true);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.CanAddAsFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.AddFavorite(ArcGIS.Desktop.Core.Item)
      #region Add a Favorite - Style project item
      {
        StyleProjectItem styleItem = Project.Current.GetItems<StyleProjectItem>().
                                FirstOrDefault(style => style.Name == "ArcGIS 3D");
        if (FavoritesManager.Current.CanAddAsFavorite(styleItem))
        {
          // add to favorites with IsAddedToAllNewProjects set to false
          FavoritesManager.Current.AddFavorite(styleItem);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.ClearIsAddedToAllNewProjects
      // cref: ArcGIS.Desktop.Core.FavoritesManager.SetIsAddedToAllNewProjects
      #region Toggle the flag IsAddedToAllNewProjects for a favorite
      {
        var newItemFolder = ItemFactory.Instance.Create(@"d:\data");

        // is the folder item already a favorite?
        var favorite = FavoritesManager.Current.GetFavorite(newItemFolder);
        if (favorite != null)
        {
          if (favorite.IsAddedToAllNewProjects)
            FavoritesManager.Current.ClearIsAddedToAllNewProjects(favorite.Item);
          else
            FavoritesManager.Current.SetIsAddedToAllNewProjects(favorite.Item);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorites
      #region Get the set of favorites and iterate
      {
        var favorites = FavoritesManager.Current.GetFavorites();
        foreach (var favorite in favorites)
        {
          bool isAddedToAllProjects = favorite.IsAddedToAllNewProjects;
          // retrieve the underlying item of the favorite
          Item item = favorite.Item;

          // Item properties
          var itemType = item.TypeID;
          var path = item.Path;

          // if it's a folder item
          if (item is FolderConnectionProjectItem)
          {
          }
          // if it's a geodatabase item
          else if (item is GDBProjectItem)
          {
          }
          // else 
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorites
      // cref: ArcGIS.Desktop.Core.FavoritesManager.RemoveFavorite(ArcGIS.Desktop.Core.Item)
      #region Remove All Favorites
      {
        var favorites = FavoritesManager.Current.GetFavorites();
        foreach (var favorite in favorites)
          FavoritesManager.Current.RemoveFavorite(favorite.Item);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Events.FavoritesChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.FavoritesChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorites
      #region FavoritesChangedEvent
      {
        FavoritesChangedEvent.Subscribe((args) =>
        {
          // favorites have changed
          int count = FavoritesManager.Current.GetFavorites().Count;
        });
      }
      #endregion

      #region ProSnippet Group: Metadata
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata
      #region Item: Get its IMetadata interface
      {
        Item gdbItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb");
        gdbMetadataItem = gdbItem as IMetadata;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.GetXml
      // cref: ArcGIS.Desktop.Core.Item.GetXml
      #region Item: Get an item's metadata: GetXML
      {
        string gdbXMLMetadataXmlAsString = string.Empty;
        // Note: Needs QueuedTask to run
        gdbXMLMetadataXmlAsString = gdbMetadataItem.GetXml();
        //check metadata was returned
        if (!string.IsNullOrEmpty(gdbXMLMetadataXmlAsString))
        {
          //use the metadata
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.SetXml(System.String)
      // cref: ArcGIS.Desktop.Core.Item.SetXml(System.String)
      // cref: ArcGIS.Desktop.Core.Project.SetXml(System.String)
      // cref: ARCGIS.DESKTOP.CORE.ITEM.CANEDIT
      #region Item: Set the metadata of an item: SetXML
      {
        // Note: Needs QueuedTask to run
        var xmlMetadata = File.ReadAllText(@"E:\Data\Metadata\MetadataForFeatClass.xml");
        //Will throw InvalidOperationException if the metadata cannot be changed
        //so check "CanEdit" first
        if (featureClassMetadataItem.CanEdit())
          featureClassMetadataItem.SetXml(xmlMetadata);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.CanEdit
      // cref: ArcGIS.Desktop.Core.Item.CanEdit
      // cref: ArcGIS.Desktop.Core.Project.CanEdit
      #region Item: Check the metadata can be edited: CanEdit
      {
        bool canEdit;
        //Call CanEdit before calling SetXml
        // Note: Needs QueuedTask to run
        canEdit = metadataItemToCheck.CanEdit();
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.Synchronize
      // cref: ArcGIS.Desktop.Core.Item.Synchronize
      // cref: ArcGIS.Desktop.Core.Project.Synchronize
      #region Item: Updates metadata with the current properties of the item: Synchronize
      {
        string syncedMetadataXml = string.Empty;
        // Note: Needs QueuedTask to run
        syncedMetadataXml = metadataItemToSync.Synchronize();
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Core.Item.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Core.Project.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      #region Item: Copy metadata from the source item's metadata: CopyMetadataFromItem
      {
        Item featureClassItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb\SourceFeatureClass");
        // Note: Needs QueuedTask to run
        metadataCopyFrom.CopyMetadataFromItem(featureClassItem);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.MDDeleteContentOption
      // cref: ArcGIS.Desktop.Core.IMetadata.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
      // cref: ArcGIS.Desktop.Core.Item.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
      // cref: ArcGIS.Desktop.Core.Project.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      #region Item: Delete certain content from the metadata of the current item: DeleteMetadataContent
      {
        Item featureClassWithMetadataItem = ItemFactory.Instance.Create(@"C:\projectBeta\GDBs\regionFive.gdb\SourceFeatureClass");
        //Delete thumbnail content from item's metadata
        featureClassWithMetadataItem.DeleteMetadataContent(MDDeleteContentOption.esriMDDeleteThumbnail);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.MDImportExportOption
      // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
      // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      // cref: ArcGIS.Desktop.Core.Item.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
      // cref: ArcGIS.Desktop.Core.Item.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      // cref: ArcGIS.Desktop.Core.Project.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
      // cref: ArcGIS.Desktop.Core.Project.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      #region Item: Updates metadata with the imported metadata - the input path can be the path to an item with metadata, or a URI to a XML file: ImportMetadata
      {
        // the input path can be the path to an item with metadata, or a URI to an XML file
        // Note: Needs QueuedTask to run
        metadataItemImport.ImportMetadata(@"E:\YellowStone.gdb\MyDataset\MyFeatureClass", MDImportExportOption.esriCurrentMetadataStyle);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.MDImportExportOption
      // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      #region Item: Updates metadata with the imported metadata: ImportMetadata
      {
        // the input path can be the path to an item with metadata, or a URI to an XML file
        // Note: Needs QueuedTask to run
        metadataItemImport.ImportMetadata(@"E:\YellowStone.gdb\MyDataset\MyFeatureClass", MDImportExportOption.esriCustomizedStyleSheet, @"E:\StyleSheets\Import\MyImportStyleSheet.xslt");
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.MDImportExportOption
      // cref: ArcGIS.Desktop.Core.MDExportRemovalOption
      // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      #region Item: export the metadata of the currently selected item: ExportMetadata
      {
        // Note: Needs QueuedTask to run
        metadataItemExport.ExportMetadata(@"E:\Temp\OutputXML.xml", MDImportExportOption.esriCustomizedStyleSheet, MDExportRemovalOption.esriExportExactCopy, @"E:\StyleSheets\Export\MyExportStyleSheet.xslt");
        // Or export using the current metadata style
        metadataItemExport.ExportMetadata(@"E:\Temp\OutputXML.xml", MDImportExportOption.esriCurrentMetadataStyle, MDExportRemovalOption.esriExportExactCopy);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.MDSaveAsXMLOption
      // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
      // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
      // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
      #region Item: Save the metadata of the current item as XML: SaveMetadataAsXML
      {
        metadataItemToSaveAsXML.SaveMetadataAsXML(@"E:\Temp\OutputXML.xml", MDSaveAsXMLOption.esriExactCopy);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.MDSaveAsHTMLOption
      // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
      // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
      // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
      #region Item: Save the metadata of the current item as HTML: SaveMetadataAsHTML
      {
        // Note: Needs QueuedTask to run
        metadataItemToSaveAsHTML.SaveMetadataAsHTML(@"E:\Temp\OutputHTML.htm", MDSaveAsHTMLOption.esriCurrentMetadataStyle);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
      // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
      // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
      #region Item: Save the metadata of the current item using customized XSLT: SaveMetadataAsUsingCustomXSLT
      {
        // Note: Needs QueuedTask to run
        metadataItemToSaveAsUsingCustomXSLT.SaveMetadataAsUsingCustomXSLT(@"E:\Data\Metadata\CustomXSLT.xsl", @"E:\Temp\OutputXMLCustom.xml");
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.MDUpgradeOption
      // cref: ArcGIS.Desktop.Core.IMetadata.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
      // cref: ArcGIS.Desktop.Core.Item.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
      // cref: ArcGIS.Desktop.Core.Project.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
      #region Item: Upgrade the metadata of the current item: UpgradeMetadata
      {
        var fgdcItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\testData.gdb");
        // Note: Needs QueuedTask to run
        fgdcItem.UpgradeMetadata(MDUpgradeOption.esriUpgradeFgdcCsdgm);
      }
      #endregion

      #region ProSnippet Group: Portal Projects
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.CanOpen(System.String,System.String@)
      //cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
      //cref: ArcGIS.Desktop.Core.Project
      //cref: ArcGIS.Desktop.Core.ArcGISPortalManager
      //cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetPortal(System.Uri)
      //cref: ArcGIS.Desktop.Core.ArcGISPortal.GetSignOnUsername
      //cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetUserContentAsync
      #region Workflow to open an ArcGIS Pro project
      {
        projectPath = @"https://<userName>.<domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
        string docVer = string.Empty;

        // A portal project path looks like this:
        //@"https://<ServerName>.<Domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
        //A local project path looks like this:
        //@"C:\Users\<UserName>\Documents\ArcGIS\Projects\MyProject\MyProject.aprx";

        //Check if the project can be opened
        if (Project.CanOpen(projectPath, out docVer))
        {
          //Open the project
          Project.OpenAsync(projectPath);
        }
        else //The project cannot be opened
        {
          //One possible reason: If the project is a portal project, the active portal must match the portal of the project
          //Check if this is a portal project
          bool isPortalProject = Uri.TryCreate(projectPath, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

          if (isPortalProject)
          {
            //Parse the project path to get the portal
            var uri = new Uri(projectPath);
            var portalUrlOfProjectToOpen = $"{uri.Scheme}://{uri.Host}/portal/";

            //Get the current active portal
            var activePortal = ArcGISPortalManager.Current.GetActivePortal();
            //Compare to see if the active Portal is the same as the portal of the project
            bool isSamePortal = activePortal != null && activePortal.PortalUri.ToString() == portalUrlOfProjectToOpen;
            if (!isSamePortal) //not the same. 
            {
              //Set new active portal to be the portal of the project
              //Find the portal to sign in with using its Uri...
              var projectPortal = ArcGISPortalManager.Current.GetPortal(new Uri(portalUrlOfProjectToOpen, UriKind.Absolute));
              // Note: Needs QueuedTask to run
              {
                if (!projectPortal.IsSignedOn())
                {
                  //Calling "SignIn" will trigger the OAuth popup if your credentials are
                  //not cached (eg from a previous sign in in the session)
                  if (projectPortal.SignIn().success)
                  {
                    //Set this portal as my active portal
                    ArcGISPortalManager.Current.SetActivePortal(projectPortal);
                    return;
                  }
                }
                //Set this portal as my active portal
                ArcGISPortalManager.Current.SetActivePortal(projectPortal);
              }
              //Now try opening the project again
              if (Project.CanOpen(projectPath, out docVer))
              {
                Project.OpenAsync(projectPath);
              }
              else
              {
                System.Diagnostics.Debug.WriteLine("The project cannot be opened.");
              }
            }
            else //The portals are the same. So the problem could be something else - permissions, portal is down?
            {
              System.Diagnostics.Debug.WriteLine("The project cannot be opened.");
            }
          }
          else //Project is on disk and cannot be opened. 
          {
            System.Diagnostics.Debug.WriteLine("The project cannot be opened.");
          }
        }
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.IsPortalProject
      //cref: ArcGIS.Desktop.Core.Project.Current
      //cref: ArcGIS.Desktop.Core.Project
      #region Determine if the project is a portal project from a project's path
      {
        projectPath = Project.Current.Url;
        // A portal project path looks like this:
        //@"https://<ServerName>.<Domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
        //A local project path looks like this:
        //@"C:\Users\<UserName>\Documents\ArcGIS\Projects\MyProject\MyProject.aprx";
        bool isPortalProject = Uri.TryCreate(projectPath, UriKind.Absolute, out Uri uriResult)
                               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        if (isPortalProject)
        {
          System.Diagnostics.Debug.WriteLine("This is a portal project");
        }
        else
        {
          System.Diagnostics.Debug.WriteLine("This is not a portal project");
        }
        // Use isPortalProject;
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.IsPortalProject
      //cref: ArcGIS.Desktop.Core.Project.Current
      //cref: ArcGIS.Desktop.Core.Project
      #region Determine if the project is a portal project from a project object
      {
        var isPortalProject = Project.Current.IsPortalProject;
        // Use isPortalProject; 
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.CanOpen(System.String,System.String@)
      //cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
      //cref: ArcGIS.Desktop.Core.Project
      #region Get the portal from a portal project's path
      {
        projectPath = Project.Current.Url;
        // A portal project path looks like this:
        //@"https://<ServerName>.<Domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
        //A local project path looks like this:
        //@"C:\Users\<UserName>\Documents\ArcGIS\Projects\MyProject\MyProject.aprx";

        //Check if the project is a portal project
        bool isPortalProject = Uri.TryCreate(projectPath, UriKind.Absolute, out Uri uriResult)
                               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        if (isPortalProject)
        {
          //Parse the project path to get the portal
          var uri = new Uri(projectPath);
          var fullUri = $"{uri.Scheme}://{uri.Host}/portal/";
          System.Diagnostics.Debug.WriteLine($"The Url of the project is: {fullUri}");
          //Now get the ArcGISPortal object from the portal Uri
          var arcgisPortal = ArcGISPortalManager.Current.GetPortal(new Uri(fullUri, UriKind.Absolute));
          System.Diagnostics.Debug.WriteLine($"The portal of the project is: {arcgisPortal.PortalUri}");
          //Note: You can set the active portal to be the portal of the project. Refer to this snippet: [ArcGISPortalManager: Get a portal and Sign In, Set it Active](ProSnippets-sharing#arcgisportalmanager-get-a-portal-and-sign-in-set-it-active)
        }
      }
      #endregion

      //cref: ArcGIS.Desktop.Catalog.OpenItemDialog
      //cref: ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
      //cref: ArcGIS.Desktop.Core.BrowseProjectFilter
      //cref: ArcGIS.Desktop.Core.BrowseProjectFilter.AddFilter(ArcGIS.Desktop.Core.BrowseProjectFilter)
      #region Workflow to open an ArcGIS Pro project using the OpenItemDialog
      {
        BrowseProjectFilter portalAndLocalProjectsFilter = new();
        //A filter to pick projects from the portal
        //This filter will allow selection of ppkx and portal project items on the portal
        portalAndLocalProjectsFilter.AddFilter(BrowseProjectFilter.GetFilter("esri_browseDialogFilters_projects_online_proprojects"));
        //A filter to pick projects from the local machine
        portalAndLocalProjectsFilter.AddFilter(BrowseProjectFilter.GetFilter("esri_browseDialogFilters_projects"));
        //Create the OpenItemDialog and set the filter to the one we just created
        var openDlg = new OpenItemDialog()
        {
          Title = "Select a Project",
          MultiSelect = false,
          BrowseFilter = portalAndLocalProjectsFilter
        };
        //Show the dialog
        var result = openDlg.ShowDialog();
        //Check if the user clicked OK and selected an item
        bool? ok = openDlg.ShowDialog();
        if (!ok.HasValue || openDlg.Items.Count == 0)
          return; //nothing selected
        var selectedItem = openDlg.Items.FirstOrDefault();
        //Open the project use the OpenAsync method.
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.CanOpen(System.String,System.String@)
      //cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
      //cref: ArcGIS.Desktop.Core.Project
      //cref: ArcGIS.Desktop.Core.ArcGISPortalManager
      //cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetPortal(System.Uri)
      //cref: ArcGIS.Desktop.Core.ArcGISPortal.GetSignOnUsername
      //cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetUserContentAsync
      #region Retrieve a project item from a portal and open it
      {
        var projectPortal = ArcGISPortalManager.Current.GetPortal(new Uri(@"https://<serverName>.<domain>.com/portal/", UriKind.Absolute));
        string owner = string.Empty;
        // Note: Needs QueuedTask to run
        {
          //Get the signed on user name
          owner = projectPortal.GetSignOnUsername();
        }
        //Get the user content from the portal
        PortalUserContent userContent = projectPortal.GetUserContentAsync(owner).Result;
        //Get the first portal project item
        var firstPortalProject = userContent.PortalItems.FirstOrDefault(pi => pi.PortalItemType == PortalItemType.ProProject);
        var portalProjectUri = firstPortalProject.ItemUri.ToString();
        //Check if project can be opened
        string docVer = string.Empty;
        if (Project.CanOpen(portalProjectUri, out docVer))
        {
          Project.OpenAsync(portalProjectUri);
        }
        //Note: If Project.CanOpen returns false, the project cannot be opened. One reason could be 
        // the active portal is not the same as the portal of the project. Refer to the snippet: [Workflow to open an ArcGIS Pro project](ProSnippets-sharing#workflow-to-open-an-arcgis-pro-project)
      }
      #endregion

      //cref: ArcGIS.Desktop.Core.Project.GetRecentProjectsEx
      #region Retrieve the list of recently opened projects
      {
        IReadOnlyList<Tuple<string, string>> result = [];
        //A list of Tuple instances containing two strings.
        //The first string: full path to the .aprx. In case of Portal projects, 
        //this is the cached location of the project on the local machine.
        //Second string:  url for portal projects
        result = Project.GetRecentProjectsEx();
        foreach (var project in result)
        {
          string projectUrl;
          if (!string.IsNullOrEmpty(project.Item2))
          {
            //this is a portal project
            //Url
            projectUrl = project.Item2;
            //local cached location of the portal project
            projectPath = project.Item1;
          }
          else
          {
            //this is a local project
            //path to local project
            projectPath = project.Item1;
          }
          projectName = new FileInfo(project.Item1).Name;
        }
      }
      #endregion

      #region ProSnippet Group: Project
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync()
      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
      #region Create a new project
      {
        //Create a new project using Pro's default settings
        var defaultProjectSettings = Project.GetDefaultProjectSettings();
        var project = Project.CreateAsync(defaultProjectSettings).Result;

        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync
      #region Create an empty project
      {
        //Create an empty project. The project will be created in the default folder
        //It will be named MyProject1, MyProject2, or similar...
        var project = Project.CreateAsync().Result;

        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      #region Create a new project with specified name
      {
        //Settings used to create a new project
        CreateProjectSettings projectSettings = new()
        {
          //Sets the name of the project that will be created
          // example: projectName =  @"C:\Data\MyProject1\MyProject1.aprx"
          Name = projectName
        };
        //Create the new project
        var project = Project.CreateAsync(projectSettings).Result;

        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
      #region Create new project using Pro's default settings
      {
        //Get Pro's default project settings.
        var defaultProjectSettings = Project.GetDefaultProjectSettings();
        //Create a new project using the default project settings
        var project = Project.CreateAsync(defaultProjectSettings).Result;

        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      #region New project using a custom template file
      {
        //Settings used to create a new project
        CreateProjectSettings projectSettings = new()
        {
          //Sets the project's name
          Name = projectName,
          //Path where new project will be stored in
          LocationPath = @"C:\Data\NewProject",
          //Sets the project template that will be used to create the new project
          TemplatePath = @"C:\Data\MyProject1\CustomTemplate.aptx"
        };
        //Create the new project
        var project = Project.CreateAsync(projectSettings).Result;

        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      // cref: ArcGIS.Desktop.Core.TemplateType
      #region Create a project using template available with ArcGIS Pro
      {
        //Settings used to create a new project
        CreateProjectSettings proTemplateSettings = new()
        {
          //Sets the project's name
          Name = projectName,
          //Path where new project will be stored in
          LocationPath = @"C:\Data\NewProject",
          //Select which Pro template you like to use
          TemplateType = TemplateType.Catalog
          //TemplateType = TemplateType.LocalScene
          //TemplateType = TemplateType.GlobalScene
          //TemplateType = TemplateType.Map
        };
        //Create the new project
        var project = Project.CreateAsync(proTemplateSettings).Result;

        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
      #region Open an existing project
      {
        //Opens an existing project or project package
        // example: @"C:\Data\MyProject1\MyProject1.aprx"
        var project = Project.OpenAsync(projectPath).Result;
        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current 
      #region Get the Current project
      {
        //Gets the current project
        var project = Project.Current;
        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current
      // cref: ArcGIS.Desktop.Core.Project.URI
      #region Get location of current project
      {
        //Gets the location of the current project; that is, the path to the current project file (*.aprx)  
        projectPath = Project.Current.URI;
        // Use projectPath
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current
      // cref: ArcGIS.Desktop.Core.Project.DefaultGeodatabasePath
      #region Get the project's default gdb path
      {
        var projGDBPath = Project.Current.DefaultGeodatabasePath;
        // Use projGDBPath;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.SetDefaultGeoDatabasePath(System.String)
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem(ArcGIS.Desktop.Core.IProjectItem)
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Change the Project's default gdb path
      {
        //Create a new GDB item and add it to the project
        if (ItemFactory.Instance.Create(newGDDItemPath) is not IProjectItem newGDBItem)
        {
          // could not create the item
          return;
        }
        var success = Project.Current.AddItem(newGDBItem);
        //make the newly added GDB item the default
        if (success)
          Project.Current.SetDefaultGeoDatabasePath(newGDDItemPath);
        //Now remove the old item
        if (Project.Current.GetItems<Item>().FirstOrDefault(i => i.Path == oldGDBItemPath) is not IProjectItem oldGDBItem)
        {
          // could not find the item
          return;
        }
        var removeSuccess = Project.Current.RemoveItem(oldGDBItem);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.SaveAsync
      #region Save project
      {
        //Saves the project
        var project = Project.Current.SaveAsync().Result;
        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current
      // cref: ArcGIS.Desktop.Core.Project.IsDirty
      #region Check if project needs to be saved
      {
        //The project's dirty state indicates changes made to the project have not yet been saved. 
        bool isProjectDirty = Project.Current.IsDirty;
        // Use isProjectDirty;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.SaveAsAsync
      #region SaveAs project
      {
        //Saves a copy of the current project file (*.aprx) to the specified location with the specified file name, 
        //then opens the new project file
        var project = Project.Current.SaveAsAsync(newProjectPath).Result;

        // Use project
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
      #region Close a project
      //A project cannot be closed using the ArcGIS Pro API. 
      //A project is only closed when another project is opened, a new one is created, or the application is shutdown.
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMap(System.String,ArcGIS.Core.CIM.MapType,ArcGIS.Core.CIM.MapViewingMode,ArcGIS.Desktop.Mapping.Basemap)
      #region How to add a new map to a project
      {
        // Note: Needs QueuedTask to run
        //Note: see also MapFactory in ArcGIS.Desktop.Mapping
        var map = MapFactory.Instance.CreateMap("New Map", MapType.Map, MapViewingMode.Map, Basemap.Oceans);
        ArcGIS.Desktop.Framework.FrameworkApplication.Panes.CreateMapPaneAsync(map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetRecentProjects()
      #region Get Recent Projects
      {
        var recentProjects = Project.GetRecentProjects();
        // Use recentProjects;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearRecentProjects()
      #region Clear Recent Projects
      {
        Project.ClearRecentProjects();
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.RemoveRecentProject(string)
      #region Remove a Recent Project
      {
        Project.RemoveRecentProject(projectPath);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetPinnedProjects()
      #region Get Pinned Projects
      {
        var pinnedProjects = Project.GetPinnedProjects();
        // Use pinnedProjects;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearPinnedProjects()
      #region Clear Pinned Projects
      {
        Project.ClearPinnedProjects();
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.PinProject(string)
      // cref: ArcGIS.Desktop.Core.Project.UnpinProject(string)
      #region Pin / UnPin Projects
      {
        Project.PinProject(newProjectPath);
        Project.UnpinProject(oldProjectPath);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetRecentProjectTemplates()
      #region Get Recent Project Templates
      {
        var recentTemplates = Project.GetRecentProjectTemplates();
        // Use recentTemplates;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearRecentProjectTemplates()
      #region Clear Recent Project Templates
      {
        Project.ClearRecentProjectTemplates();
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.RemoveRecentProjectTemplate(string)
      #region Remove a Recent Project Template
      {
        Project.RemoveRecentProjectTemplate(templatePath);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetPinnedProjectTemplates()
      #region Get Pinned Project Templates
      {
        var pinnedTemplates = Project.GetPinnedProjectTemplates();
        // Use pinnedTemplates;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearPinnedProjectTemplates()
      #region Clear Pinned Project Templates
      {
        Project.ClearPinnedProjectTemplates();
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.PinProjectTemplate(string)
      // cref: ArcGIS.Desktop.Core.Project.UnpinTemplateProject(string)
      #region Pin / UnPin Project Templates
      {
        Project.PinProjectTemplate(templatePath);
        Project.UnpinTemplateProject(newTemplatePath);
      }
      #endregion

      #region ProSnippet Group: Project Items
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.IItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Add a folder connection item to the current project
      {
        //Adding a folder connection

        //Create the folder connection project item
        var item = ItemFactory.Instance.Create(folderPath) as IProjectItem;

        // Note: Needs QueuedTask to run
        var folder = Project.Current.AddItem(item) ? item as FolderConnectionProjectItem : null;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.IItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Add a geodatabase item to the current project
      {
        //Adding a Geodatabase:
        //Create the File GDB project item
        // Note: Needs QueuedTask to run
        var item = ItemFactory.Instance.Create(gdbPath) as IProjectItem;
        var newlyAddedGDB = Project.Current.AddItem(item) ? item as GDBProjectItem : null;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all project items
      {
        IEnumerable<Item> allProjectItems = Project.Current.GetItems<Item>();
        foreach (var pi in allProjectItems)
        {
          //Do Something 
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all MapProjectItems for a project
      {
        var project = Project.Current;
        IEnumerable<MapProjectItem> newMapItemsContainer = project.GetItems<MapProjectItem>();
        // Note: Needs QueuedTask to run
        foreach (var mp in newMapItemsContainer)
        {
          //Do Something with the map. For Example:
          Map myMap = mp.GetMap();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific MapProjectItem
      {
        MapProjectItem mapProjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("EuropeMap"));
        // Use mapProjItem;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all StyleProjectItems
      {
        IEnumerable<StyleProjectItem> newStyleItemsContainer = null;
        newStyleItemsContainer = Project.Current.GetItems<StyleProjectItem>();
        foreach (var styleItem in newStyleItemsContainer)
        {
          //Do Something with the style.
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific StyleProjectItem
      {
        var container = Project.Current.GetItems<StyleProjectItem>();
        // example: projectStyleName = "ArcGIS 3D";
        StyleProjectItem testStyle = container.FirstOrDefault(style => style.Name == projectStyleName);
        StyleItem cone = null;
        if (testStyle != null)
        {
          // example: symbolName = "Cone_Volume_3";
          cone = testStyle.LookupItem(StyleItemType.PointSymbol, symbolName);
        }
        // Use (testStyle, cone);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get the Favorite StyleProjectItem
      {
        // Note: Needs QueuedTask to run
        var containerStyle = Project.Current.GetProjectItemContainer("Style");
        var fav_style_item = containerStyle.GetItems().OfType<StyleProjectItem>().First(item => item.TypeID == "personal_style");
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all GDBProjectItems
      {
        IEnumerable<GDBProjectItem> newGDBItemsContainer = null;
        newGDBItemsContainer = Project.Current.GetItems<GDBProjectItem>();
        foreach (var GDBItem in newGDBItemsContainer)
        {
          //Do Something with the GDB.
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific GDBProjectItem
      {
        GDBProjectItem gdbProjItem = Project.Current.GetItems<GDBProjectItem>().FirstOrDefault(item => item.Name.Equals("myGDB"));
        // Use gdbProjItem;
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all ServerConnectionProjectItems
      {
        IEnumerable<ServerConnectionProjectItem> newServerConnections = null;
        var project = Project.Current;
        newServerConnections = project.GetItems<ServerConnectionProjectItem>();
        foreach (var serverItem in newServerConnections)
        {
          //Do Something with the server connection.
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific ServerConnectionProjectItem
      {
        ServerConnectionProjectItem serverProjItem = Project.Current.GetItems<ServerConnectionProjectItem>().FirstOrDefault(item => item.Name.Equals("myServer"));
        // Use serverProjItem;
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all folder connections in a project
      {
        //Gets all the folder connections in the current project
        var projectFolders = Project.Current.GetItems<FolderConnectionProjectItem>();
        foreach (var FolderItem in projectFolders)
        {
          //Do Something with the Folder connection.
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific folder connection
      {
        FolderConnectionProjectItem folderProjItem = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(item => item.Name.Equals("myDataFolder"));
        // Use folderProjItem;
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Remove a specific folder connection
      {
        // Remove a folder connection from a project; the folder stored on the local disk or the network is not deleted
        FolderConnectionProjectItem folderToRemove = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(myfolder => myfolder.Name.Equals("PlantSpecies"));
        if (folderToRemove != null)
          Project.Current.RemoveItem(folderToRemove as IProjectItem);
      }
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Gets a specific LayoutProjectItem
      {
        LayoutProjectItem layoutProjItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("myLayout"));
        // Use layoutProjItem;
      }
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all layouts in a project
      {
        //Gets all the layouts in the current project
        IEnumerable<LayoutProjectItem> projectLayouts = Project.Current.GetItems<LayoutProjectItem>();
        foreach (var layoutItem in projectLayouts)
        {
          //Do Something with the layout
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.GeoProcessing.GeoprocessingProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific GeoprocessingProjectItem
      {
        //Gets a specific GeoprocessingProjectItem in the current project
        // example: "myToolbox" is the name of the GeoprocessingProjectItem
        GeoprocessingProjectItem gpProjItem = Project.Current.GetItems<GeoprocessingProjectItem>().FirstOrDefault(item => item.Name.Equals("myToolbox"));
        // Use gpProjItem;
      }
      #endregion

      // cref: ArcGIS.Desktop.GeoProcessing.GeoprocessingProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all GeoprocessingProjectItems in a project
      {
        //Gets all the GeoprocessingProjectItem in the current project
        var GPItems = Project.Current.GetItems<GeoprocessingProjectItem>();
        foreach (var tbx in GPItems)
        {
          //Do Something with the toolbox
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Search project for a specific item
      {
        List<Item> _mxd = [];
        //Gets all the folder connections in the current project
        var allFoldersItem = Project.Current.GetItems<FolderConnectionProjectItem>();
        if (allFoldersItem != null)
        {
          //iterate through all the FolderConnectionProjectItems found
          foreach (var folderItem in allFoldersItem)
          {
            //Search for mxd files in that folder connection and add it to the List<T>
            //Note:ArcGIS Pro automatically creates and dynamically updates a searchable index as you build and work with projects. 
            //Items are indexed when they are added to a project.
            //The first time a folder or database is indexed, indexing may take a while if it contains a large number of items. 
            //While the index is being created, searches will not return any results.
            _mxd.AddRange(folderItem.GetItems());
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.CreateProjectSettings.LocationPath
      // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
      #region Get the Default Project Folder
      {
        //Get Pro's default project settings.
        var defaultSettings = Project.GetDefaultProjectSettings();
        var defaultProjectPath = defaultSettings.LocationPath;
        // If not set, projects are saved in the user's My Documents\ArcGIS\Projects folder;
        // this folder is created if it doesn't already exist.
        defaultProjectPath ??= Path.Combine(
                    Environment.GetFolderPath(
                         Environment.SpecialFolder.MyDocuments),
                    @"ArcGIS\Projects");
        // Use defaultProjectPath;
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Core.Item.Refresh
      #region Refresh the child item for a folder connection Item
      {
        var contentItem = Project.Current.GetItems<FolderConnectionProjectItem>().First();
        //Check if the MCT is required for Refresh()
        if (contentItem.IsMainThreadRequired)
        {
          // Note: Needs QueuedTask to run
          contentItem.Refresh();
        }
        else
        {
          //if item.IsMainThreadRequired returns false, any
          //thread can be used to invoke Refresh(), though
          //BackgroundTask is preferred.
          contentItem.Refresh();

          //Or, via BackgroundTask
          ArcGIS.Core.Threading.Tasks.BackgroundTask.Run(() =>
            contentItem.Refresh(), ArcGIS.Core.Threading.Tasks.BackgroundProgressor.None);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Item.ItemCategories
      #region Get Item Categories
      {
        // Get the ItemCategories with which an item is associated
        gdb = ItemFactory.Instance.Create(@"E:\CurrentProject\RegionalPolling\polldata.gdb");
        List<ItemCategory> gdbItemCategories = gdb.ItemCategories;
        // Use gdbItemCategories;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemCategory.Items(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Core.Item})
      #region Using Item Categories
      {
        // Browse items using an ItemCategory as a filter
        IEnumerable<Item> gdbContents = gdb.GetItems();
        IEnumerable<Item> filteredGDBContents1 = gdbContents.Where(item => item.ItemCategories.OfType<ItemCategoryDataSet>().Any());
        IEnumerable<Item> filteredGDBContents2 = new ItemCategoryDataSet().Items(gdbContents);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      #region Create Project with Template
      {
        var projectFolder = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments),
            @"ArcGIS\Projects");
        CreateProjectSettings ps = new()
        {
          Name = projectName,
          LocationPath = projectFolder,
          TemplatePath = templatePath
        };
        var project = Project.CreateAsync(ps).Result;
        //Use project;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ProjectItemContainers
      // cref: ArcGIS.Desktop.Core.Project.GetProjectItemContainer(System.String)
      #region Select project containers - for use with SelectItemAsync
      {
        //Use Project.Current.ProjectItemContainers
        var folderContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "FolderConnection");
        var gdbContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "GDB");
        var mapContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "Map");
        var layoutContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "Layout");
        var toolboxContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "GP");
        //etc.

        //or...use Project.Current.GetProjectItemContainer

        folderContainer = Project.Current.GetProjectItemContainer("FolderConnection");
        gdbContainer = Project.Current.GetProjectItemContainer("GDB");
        mapContainer = Project.Current.GetProjectItemContainer("Map");
        layoutContainer = Project.Current.GetProjectItemContainer("Layout");
        toolboxContainer = Project.Current.GetProjectItemContainer("GP");
        //etc.
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.FindItem(System.String)
      #region ProjectItem: Get an Item or Find an Item
      {
        //GetItems searches project content
        var map = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(m => m.Name == "Map1");
        var layout = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(m => m.Name == "Layout1");
        var folders = Project.Current.GetItems<FolderConnectionProjectItem>();
        var style = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 3D");

        //Find item uses a catalog path. The path can be to a file or dataset
        var fcPath = @"C:\Pro\CommunitySampleData\Interacting with Maps\Interacting with Maps.gdb\Crimes";
        var pdfPath = @"C:\Temp\Layout1.pdf";
        var imgPath = @"C:\Temp\AddinDesktop16.png";

        var fc = Project.Current.FindItem(fcPath);
        var pdf = Project.Current.FindItem(pdfPath);
        var img = Project.Current.FindItem(imgPath);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Core.Project.GetCatalogPane
      // cref: ArcGIS.Desktop.Core.IProjectWindow
      // cref: ArcGIS.Desktop.Core.Project.ProjectItemContainers
      // cref: ArcGIS.Desktop.Core.IProjectWindow.SelectItemAsync
      #region Select an item in the Catalog pane
      {
        //Get the catalog pane
        IProjectWindow projectWindow = Project.GetCatalogPane();
        //or get the active catalog view...
        //ArcGIS.Desktop.Core.IProjectWindow projectWindow = Project.GetActiveCatalogWindow();

        //eg Find a toolbox in the project
        string gpName = "Interacting with Maps.tbx";
        var toolbox = Project.Current.GetItems<GeoprocessingProjectItem>().FirstOrDefault(tbx => tbx.Name == gpName);
        //Select it under Toolboxes
        projectWindow.SelectItemAsync(toolbox, true, true, null);//null selects it in the first container - optionally await
                                                                 //Note: Project.Current.GetProjectItemContainer("GP") would get toolbox container...

        //assume toolbox is also under Folders container. Select it under Folders instead of Toolboxes
        var foldersContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "FolderConnection");
        //We must specify the container because Folders comes second (after Toolboxes)
        projectWindow.SelectItemAsync(toolbox, true, true, foldersContainer);//optionally await

        //Find a map and select it
        var mapItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(m => m.Name == "Map");
        //Map only occurs under "Maps" so the container need not be specified
        projectWindow.SelectItemAsync(mapItem, true, false, null);
      }
      #endregion

      #region ProSnippet Group: Project Units
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetPredefinedProjectUnitFormats
      #region Get The Full List of All Available Unit Formats
      {
        //Note: Must be on the QueuedTask.Run()
        var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                      .OfType<UnitFormatType>().ToList();
        System.Diagnostics.Debug.WriteLine("All available units\r\n");

        foreach (var unit_format in unit_formats)
        {
          var units = DisplayUnitFormats.Instance.GetPredefinedProjectUnitFormats(unit_format);
          System.Diagnostics.Debug.WriteLine(unit_format.ToString());

          foreach (var display_unit_format in units)
          {
            var line = $"{display_unit_format.DisplayName}, {display_unit_format.UnitCode}";
            System.Diagnostics.Debug.WriteLine(line);
          }
          System.Diagnostics.Debug.WriteLine("");
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
      #region Get The List of Unit Formats for the Current Project
      {
        // Note: Must be on the QueuedTask.Run()
        var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                        .OfType<UnitFormatType>().ToList();
        System.Diagnostics.Debug.WriteLine("Project units\r\n");
        foreach (var unit_format in unit_formats)
        {
          var units = DisplayUnitFormats.Instance.GetProjectUnitFormats(unit_format);
          System.Diagnostics.Debug.WriteLine(unit_format.ToString());

          foreach (var display_unit_format in units)
          {
            var line = $"{display_unit_format.DisplayName}, {display_unit_format.UnitCode}";
            System.Diagnostics.Debug.WriteLine(line);
          }
          System.Diagnostics.Debug.WriteLine("");
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
      #region Get A Specific List of Unit Formats for the Current Project
      {
        //Must be on the QueuedTask.Run()

        //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
        //UnitFormatType.Direction, UnitFormatType.Location, UnitFormatType.Page
        //UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
        var units = DisplayUnitFormats.Instance.GetProjectUnitFormats(UnitFormatType.Distance);
        // Use units;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
      #region Get The List of Default Formats for the Current Project
      {
        // Note: Must be on the QueuedTask.Run()
        var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                            .OfType<UnitFormatType>().ToList();
        System.Diagnostics.Debug.WriteLine("Default project units\r\n");

        foreach (var unit_format in unit_formats)
        {
          var default_unit = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(unit_format);
          var line = $"{unit_format}: {default_unit.DisplayName}, {default_unit.UnitCode}";
          System.Diagnostics.Debug.WriteLine(line);
        }
        System.Diagnostics.Debug.WriteLine("");
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
      #region Get A Specific Default Unit Format for the Current Project
      {
        // Note: Must be on the QueuedTask.Run()
        //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
        //UnitFormatType.Direction, UnitFormatType.Location, UnitFormatType.Page
        //UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
        var default_unit = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(
                                                         UnitFormatType.Distance);
        // Use default_unit;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetPredefinedProjectUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetProjectUnitFormats
      #region Set a Specific List of Unit Formats for the Current Project
      {
        // Note: Must be on the QueuedTask.Run()
        //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
        //UnitFormatType.Direction, UnitFormatType.Location

        //Get the full list of all available location units
        var all_units = DisplayUnitFormats.Instance.GetPredefinedProjectUnitFormats(
                                                            UnitFormatType.Location);
        //keep units with an even factory code
        var list_units = all_units.Where(du => du.UnitCode % 2 == 0).ToList();

        //set them as the new location unit collection. A new default is not being specified...
        DisplayUnitFormats.Instance.SetProjectUnitFormats(list_units);

        //set them as the new location unit collection along with a new default
        DisplayUnitFormats.Instance.SetProjectUnitFormats(
                                                list_units, list_units.First());

        //Note: UnitFormatType.Page, UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
        //cannot be set.
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetDefaultProjectUnitFormat
      #region Set the Defaults for the Project Unit Formats
      {
        // Note: Must be on the QueuedTask.Run()
        var unit_formats = Enum.GetValues(typeof(UnitFormatType)).OfType<UnitFormatType>().ToList();
        foreach (var unit_type in unit_formats)
        {
          var current_default = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(unit_type);
          //Arbitrarily pick the last unit in each unit format list
          var replacement = DisplayUnitFormats.Instance.GetProjectUnitFormats(unit_type).Last();
          DisplayUnitFormats.Instance.SetDefaultProjectUnitFormat(replacement);

          var line = $"{current_default.DisplayName}, {current_default.UnitName}, {current_default.UnitCode}";
          var line2 = $"{replacement.DisplayName}, {replacement.UnitName}, {replacement.UnitCode}";

          System.Diagnostics.Debug.WriteLine($"Format: {unit_type}");
          System.Diagnostics.Debug.WriteLine($" Current default: {line}");
          System.Diagnostics.Debug.WriteLine($" Replacement default: {line2}");
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetProjectUnitFormats
      #region Update Unit Formats for the Project
      {
        //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
        //UnitFormatType.Direction, UnitFormatType.Location
        var angle_units = DisplayUnitFormats.Instance.GetProjectUnitFormats(UnitFormatType.Angular);

        //Edit the display name of each unit - append the abbreviation
        foreach (var unit in angle_units)
        {
          unit.DisplayName = $"{unit.DisplayName} ({unit.Abbreviation})";
        }
        //apply the changes to the units and set the default to be the first entry
        DisplayUnitFormats.Instance.SetProjectUnitFormats(angle_units, angle_units.First());
        //The project must be saved to persist the changes...
      }
      #endregion

      #region ProSnippet Group: Application Options 
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
      // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
      #region Get GeneralOptions
      {
        var startMode = ApplicationOptions.GeneralOptions.StartupOption;
        var aprx_path = ApplicationOptions.GeneralOptions.StartupProjectPath;

        var hf_option = ApplicationOptions.GeneralOptions.HomeFolderOption;
        var folder = ApplicationOptions.GeneralOptions.CustomHomeFolder;

        var gdb_option = ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption;
        var def_gdb = ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase;

        var tbx_option = ApplicationOptions.GeneralOptions.DefaultToolboxOption;
        var def_tbx = ApplicationOptions.GeneralOptions.CustomDefaultToolbox;

        var create_in_folder = ApplicationOptions.GeneralOptions.ProjectCreateInFolder;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
      // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
      #region Set GeneralOptions to Use Custom Settings
      {
        //Set the application to use a custom project, home folder, gdb, and toolbox
        //In each case, the custom _path_ must be set _first_ before 
        //setting the "option". This ensures the application remains 
        //in a consistent state. This is the same behavior as on the Pro UI.
        if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.StartupProjectPath))
          ApplicationOptions.GeneralOptions.StartupProjectPath = @"D:\data\usa.aprx";//custom project path first
        ApplicationOptions.GeneralOptions.StartupOption = StartProjectMode.WithDefaultProject;//option to use it second

        if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomHomeFolder))
          ApplicationOptions.GeneralOptions.CustomHomeFolder = @"D:\home_folder";//custom home folder first
        ApplicationOptions.GeneralOptions.HomeFolderOption = OptionSetting.UseCustom;//option to use it second

        if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase))
          ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase = @"D:\data\usa.gdb";//custom gdb path first
        ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption = OptionSetting.UseCustom;//option to use it second

        if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomDefaultToolbox))
          ApplicationOptions.GeneralOptions.CustomDefaultToolbox = @"D:\data\usa.tbx";//custom toolbox path first
        ApplicationOptions.GeneralOptions.DefaultToolboxOption = OptionSetting.UseCustom;//option to use it second
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
      // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
      // cref: ArcGIS.Desktop.Core.StartProjectMode
      // cref: ArcGIS.Desktop.Core.OptionSetting
      #region Set GeneralOptions to Use Defaults
      {
        //Default options can be set regardless of the value of the "companion"
        //path (to a project, folder, gdb, toolbox, etc.). The path value is ignored if
        //the option setting does not use it. This is the same behavior as on the Pro UI.
        ApplicationOptions.GeneralOptions.StartupOption = StartProjectMode.ShowStartPage;
        ApplicationOptions.GeneralOptions.HomeFolderOption = OptionSetting.UseDefault;
        ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption = OptionSetting.UseDefault;
        ApplicationOptions.GeneralOptions.DefaultToolboxOption = OptionSetting.UseDefault;//set default option first

        //path values can (optionally) be set (back) to null if their 
        //"companion" option setting is the default option.
        if (ApplicationOptions.GeneralOptions.StartupOption != StartProjectMode.WithDefaultProject)
          ApplicationOptions.GeneralOptions.StartupProjectPath = null;
        if (ApplicationOptions.GeneralOptions.HomeFolderOption == OptionSetting.UseDefault)
          ApplicationOptions.GeneralOptions.CustomHomeFolder = null;
        if (ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption == OptionSetting.UseDefault)
          ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase = null;
        if (ApplicationOptions.GeneralOptions.DefaultToolboxOption == OptionSetting.UseDefault)
          ApplicationOptions.GeneralOptions.CustomDefaultToolbox = null;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions
      #region Get DownloadOptions
      {
        var staging = ApplicationOptions.DownloadOptions.StagingLocation;

        var ppkx_loc = ApplicationOptions.DownloadOptions.UnpackPPKXLocation;
        var ask_ppkx_loc = ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation;

        var other_loc = ApplicationOptions.DownloadOptions.UnpackOtherLocation;
        var ask_other_loc = ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation;
        var use_proj_folder = ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation;

        var offline_loc = ApplicationOptions.DownloadOptions.OfflineMapsLocation;
        var ask_offline_loc = ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation;
        var use_proj_folder_offline = ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.StagingLocation
      #region Set Staging Location for Sharing and Publishing
      {
        ApplicationOptions.DownloadOptions.StagingLocation = @"D:\data\staging";
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.AskForUnpackPPKXLocation
      #region Set DownloadOptions for PPKX
      {
        //Options are mutually exclusive.

        //Setting ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = true
        //supersedes any value in ApplicationOptions.DownloadOptions.UnpackPPKXLocation
        //and will prompt the user on an unpack. The value of 
        //ApplicationOptions.DownloadOptions.UnpackPPKXLocation will be unaffected
        //and is ignored. This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = true;//override location

        //The default location is typically <My Documents>\ArcGIS\Packages
        //Setting ApplicationOptions.DownloadOptions.UnpackPPKXLocation to any
        //location overrides ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation
        //and sets it to false. This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.UnpackPPKXLocation = @"D:\data\for_ppkx";

        //Or, if ApplicationOptions.DownloadOptions.UnpackPPKXLocation already
        //contains a valid path, set ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation
        //explicitly to false to use the UnpackPPKXLocation
        if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.UnpackPPKXLocation))
          ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = false;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.UnpackOtherLocation
      #region Set DownloadOptions for UnpackOther
      {
        //UnpackOther settings control unpacking of anything _other than_
        //a ppkx or aptx. Options are mutually exclusive.

        //Set ApplicationOptions.DownloadOptions.UnpackOtherLocation explicitly to
        //toggle ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation and
        //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false
        //Note: default is typically <My Documents>\ArcGIS\Packages, _not_ null.
        //This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.UnpackOtherLocation = @"D:\data\for_other";

        //or...to use a location already stored in UnpackOtherLocation as the
        //default without changing it, 
        //set ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation and
        //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false
        //explicitly. This is the same behavior as on the Pro UI.
        if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.UnpackOtherLocation))
        {
          ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation = false;
          ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation = false;
        }

        //Setting ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation to
        //true overrides any UnpackOtherLocation value and sets 
        //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false.
        //This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation = true;

        //Setting ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to
        //true overrides any UnpackOtherLocation value and sets 
        //ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation to false.
        //This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation = false;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.OfflineMapsLocation
      #region Set DownloadOptions for OfflineMaps
      {
        //OfflineMaps settings control where map content that is taken
        //offline is copied to on the local machine. Options are mutually exclusive.

        //Set ApplicationOptions.DownloadOptions.OfflineMapsLocation explicitly to
        //toggle ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation and
        //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false
        //Note: default is typically <My Documents>\ArcGIS\OfflineMaps, _not_ null.
        //This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.OfflineMapsLocation = @"D:\data\for_offline";

        //or...to use a location already stored in OfflineMapsLocation as the
        //default without changing it, 
        //set ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation and
        //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false
        //explicitly.
        if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.OfflineMapsLocation))
        {
          ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation = false;
          ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation = false;
        }

        //Setting ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation to
        //true overrides any OfflineMapsLocation value and sets 
        //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false.
        //This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation = true;

        //Setting ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to
        //true overrides any OfflineMapsLocation value and sets 
        //ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation to false.
        //This is the same behavior as on the Pro UI.
        ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation = true;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectDeleteLocalCopyOnClose
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectDownloadLocation
      #region Get/Set Portal Project Options
      {
        // access the current options
        var def_home = ApplicationOptions.GeneralOptions.PortalProjectCustomHomeFolder;
        var def_gdb = ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultGeodatabase;
        var def_tbx = ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultToolbox;
        var deleteOnClose = ApplicationOptions.GeneralOptions.PortalProjectDeleteLocalCopyOnClose;
        var def_location = ApplicationOptions.GeneralOptions.PortalProjectDownloadLocation;

        // set the options
        ApplicationOptions.GeneralOptions.PortalProjectCustomHomeFolder = @"E:\data";
        ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultGeodatabase = @"E:\data\usa.gdb";
        ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultToolbox = @"E:\data\usa.tbx";
        ApplicationOptions.GeneralOptions.PortalProjectDeleteLocalCopyOnClose = false;
        ApplicationOptions.GeneralOptions.PortalProjectDownloadLocation = @"E:\data";
      }
      #endregion
    }
  }
}