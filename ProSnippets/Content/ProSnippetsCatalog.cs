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
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.ProSnippets
{
  public static class ProSnippetsCatalog
  {
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Mapping.MapProjectItem
    #region Get MapProjectItems
    /// <summary>
    /// Retrieves all the map items in the current ArcGIS Pro project.
    /// </summary>
    public static void GetMapProjectItems()
    {
      // not to be included in sample regions
      var projectFolderConnection = (Project.Current.GetItems<FolderConnectionProjectItem>()).First();

      /// Get all the maps in a project
      IEnumerable<MapProjectItem> projectMaps = Project.Current.GetItems<MapProjectItem>();
    }
    #endregion //GetMapProjectItems

    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    #region Get FolderConnectionProjectItems
    /// <summary>
/// Retrieves all folder connections in the current ArcGIS Pro project.
/// </summary>
/// <returns>
/// A collection of <see cref="FolderConnectionProjectItem"/> objects representing 
/// the folder connections in the project.
/// </returns>
public static IEnumerable<FolderConnectionProjectItem> GetFolderConnectionProjectItems()
    {
      /// Get all the folder connections in a project
      IEnumerable<FolderConnectionProjectItem> projectFolders = Project.Current.GetItems<FolderConnectionProjectItem>();
      return projectFolders;
    }
    #endregion //GetFolderConnectiontProjectItems

    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
    #region Get ServerConnectionProjectItems
    /// <summary>
    /// Retrieves all server connections in the current ArcGIS Pro project.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="ServerConnectionProjectItem"/> objects representing 
    /// the server connections in the project.
    /// </returns>
    public static IEnumerable<ServerConnectionProjectItem> GetServerConnectionProjectItems()
    {
      /// Get all the server connections in a project
      IEnumerable<ServerConnectionProjectItem> projectServers = Project.Current.GetItems<ServerConnectionProjectItem>();
      return projectServers;
    }
    #endregion //Get ServerConnectionProjectItems

    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Catalog.LocatorsConnectionProjectItem
    #region Get LocatorConnectionProjectItems
    /// <summary>
    /// Retrieves all locator connections in the current ArcGIS Pro project.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="LocatorsConnectionProjectItem"/> objects representing 
    /// the locator connections in the project.
    /// </returns>
    public static IEnumerable<LocatorsConnectionProjectItem> GetLocatorConnectionProjectItems()
    {
      /// Get all the locator connections in a project
      IEnumerable<LocatorsConnectionProjectItem> projectLocators = Project.Current.GetItems<LocatorsConnectionProjectItem>();
      return projectLocators;
    }
    #endregion //Get LocatorConnectionProjectItems

    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Item.GetItems
    #region Get Project Items by ProjectItem type

    public static async Task GetProjectItems()
    {
      /// Get all the items that can be accessed from a folder connection. The items immediately 
      /// contained by a folder, that is, the folder's children, are returned including folders
      /// and individual items that can be used in ArcGIS Pro. This method does not return all 
      /// items contained by any sub-folder that can be accessed from the folder connection.
      FolderConnectionProjectItem folderConnection = Project.Current.GetItems<FolderConnectionProjectItem>()
                                                          .FirstOrDefault((folder => folder.Name.Equals("Data")));
      await QueuedTask.Run(() =>
      {
        IEnumerable<Item> folderContents = folderConnection.GetItems();
      });
    }
    #endregion //GetProjectItems

    // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
    #region Add Folder to Project as IProjectItem
    public static async Task<bool> AddFolderConnectionProjectItem()
    {
      /// Add a folder connection to a project
      Item folderToAdd = ItemFactory.Instance.Create(@"C:\Data\Oregon\Counties\Streets");
      bool wasAdded = await QueuedTask.Run(() => Project.Current.AddItem(folderToAdd as IProjectItem));
      return wasAdded;
    }
    #endregion //AddFolderConnectionProjectItem

    // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
    // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
    #region Add GDBProjectItem to Project as IProjectItem
    public static async Task<bool> AddGDBProjectItem(Item folderToAdd)
    {
      /// Add a file geodatabase or a SQLite or enterprise database connection to a project
      Item gdbToAdd = folderToAdd.GetItems().FirstOrDefault(folderItem => folderItem.Name.Equals("CountyData.gdb"));
      var addedGeodatabase = await QueuedTask.Run(() => Project.Current.AddItem(gdbToAdd as IProjectItem));
      return addedGeodatabase;
    }
    #endregion //AddGDBProjectItem

    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.RemoveItem
    #region Remove FolderConnection From Project
    public static async Task<bool> RemoveFolderConnectionFromProject()
    {
      /// Remove a folder connection from a project; the folder stored on the local disk 
      /// or the network is not deleted
      return await QueuedTask.Run(() =>
      {
        FolderConnectionProjectItem folderToRemove = Project.Current.GetItems<FolderConnectionProjectItem>()
          .FirstOrDefault(folder => folder.Name.Equals("Data"));
        if (folderToRemove != null)
          return Project.Current.RemoveItem(folderToRemove as IProjectItem);
        return false;
      });
    }
    #endregion //RemoveFolderConnectionFromProject

    // cref: ArcGIS.Desktop.Mapping.MapProjectItem
    // cref: ArcGIS.Desktop.Core.Project.RemoveItem
    #region Remove Map From Project
    public static async Task<bool> RemoveMapFromProject()
    {
      /// Remove a map from a project; the map is deleted
      IProjectItem mapToRemove = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(map => map.Name.Equals("OldStreetRoutes"));
      var removedMapProjectItem = await QueuedTask.Run(
               () => Project.Current.RemoveItem(mapToRemove));
      return removedMapProjectItem;
    }
    #endregion //RemoveMapFromProject

    // cref: ArcGIS.Desktop.Core.ItemFactory
    // cref: ArcGIS.Desktop.Core.ItemFactory.Create
    // cref: ArcGIS.Desktop.Core.ItemFactory.Create
    // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
    #region Importing Maps To Project
    public static async Task ImportMapsToProject()
    {
      /// Import a mxd
      Item mxdToImport = ItemFactory.Instance.Create(@"C:\Projects\RegionalSurvey\LatestResults.mxd");
      var addedMxd = await QueuedTask.Run(
                    () => Project.Current.AddItem(mxdToImport as IProjectItem));

      /// Add map package      
      Item mapPackageToAdd = ItemFactory.Instance.Create(@"c:\Data\Map.mpkx");
      var addedMapPackage = await QueuedTask.Run(
                    () => Project.Current.AddItem(mapPackageToAdd as IProjectItem));

      /// Add an exported Pro map
      Item proMapToAdd = ItemFactory.Instance.Create(@"C:\ExportedMaps\Election\Districts.mapx");
      var addedMapProjectItem = await QueuedTask.Run(
                    () => Project.Current.AddItem(proMapToAdd as IProjectItem));
    }
    #endregion //ImportToProject

    // not to be included in sample regions
    private static FolderConnectionProjectItem projectfolderConnection = (Project.Current.GetItems<FolderConnectionProjectItem>()).First();

    // cref: ArcGIS.Desktop.Core.ItemFactory.Create
    // cref: ArcGIS.Desktop.Core.Item
    #region Create An Item
    public static Item CreateMxdItem()
    {
      Item mxdItem = ItemFactory.Instance.Create(@"C:\Projects\RegionalSurvey\LatestResults.mxd");
      return mxdItem;
    }
    #endregion //CreateAnItem

    // cref: ArcGIS.Desktop.Core.ItemFactory.Create
    // cref: ArcGIS.Desktop.Core.ItemFactory.ItemType
    // cref: ArcGIS.Desktop.Core.Portal.PortalItem
    #region Create A PortalItem
    public static Item CreatePortalItem()
    {
      // Creates an Item from an existing portal item base on its ID
      string portalItemID = "9801f878ff4a22738dff3f039c43e395";
      Item portalItem = ItemFactory.Instance.Create(portalItemID, ItemFactory.ItemType.PortalItem);
      return portalItem;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ItemFactory.Create
    // cref: ArcGIS.Desktop.Core.ItemFactory.ItemType
    // cref: ArcGIS.Desktop.Core.Portal.PortalItem 
    #region Create A PortalFolder
    public static Item CreatePortalFolder()
    {
      // Creates an Item from an existing portal folder base on its ID
      string portalFolderID = "39c43e39f878f4a2279838dfff3f0015";
      Item portalFolder = ItemFactory.Instance.Create(portalFolderID, ItemFactory.ItemType.PortalFolderItem);
      return portalFolder;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Core.Portal.PortalItem 
    #region Get Folder Item Content from Project PortalItem
    public static IEnumerable<Item> GetFolderItemContentFromProjectPortalItem()
    {
      FolderConnectionProjectItem projectfolderConnection = (Project.Current.GetItems<FolderConnectionProjectItem>()).First();
      var folderConnectionContent = projectfolderConnection.GetItems();
      var folder = folderConnectionContent.FirstOrDefault(folderItem => folderItem.Name.Equals("Tourist Sites"));
      var folderContents = folder.GetItems();
      return folderContents;
    }
    #endregion //GetItemContent

  }
}
