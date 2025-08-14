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
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippetsContent
{
  #region ProSnippet Group: Geodatabase Content 
  #endregion

  public static class ProSnippetsGeodatabase
  {
    // cref: ArcGIS.Desktop.Catalog.OpenItemDialog
    // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
    // cref: ArcGIS.Desktop.Core.BrowseProjectFilter
    #region Geodatabase Content from Browse Dialog
    public static async Task BrowseGeodatabaseContent()
    {
      var openDlg = new OpenItemDialog
      {
        Title = "Select a Feature Class",
        InitialLocation = @"C:\Data",
        MultiSelect = false,
        BrowseFilter = BrowseProjectFilter.GetFilter(ArcGIS.Desktop.Catalog.ItemFilters.GeodatabaseItems_All)
      };
      //show the browse dialog
      bool? ok = openDlg.ShowDialog();
      if (!ok.HasValue || openDlg.Items.Count() == 0)
        return;   //nothing selected

      await QueuedTask.Run(() =>
      {
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
          if (ds is ArcGIS.Core.Data.FeatureClass fc)
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
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEventArgs
    // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEvent
    // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEvent.Subscribe
    // cref: ArcGIS.Desktop.Core.IItemFactory.CanGetDataset
    // cref: ArcGIS.Desktop.Core.ItemFactory.CanGetDataset
    #region Geodatabase Content from Catalog selection
    public static void CatalogWindow()
    {
      // subscribe to event
      ProjectWindowSelectedItemsChangedEvent.Subscribe(async (ProjectWindowSelectedItemsChangedEventArgs args) =>
      {
        if (args.IProjectWindow.SelectionCount > 0)
        {
          // get the first selected item
          var selectedItem = args.IProjectWindow.SelectedItems.First();
          await QueuedTask.Run(() =>
          {
            // datasetType
            var dataType = ItemFactory.Instance.GetDatasetType(selectedItem);
            // get the dataset Definition
            if (ItemFactory.Instance.CanGetDefinition(selectedItem))
            {
              using var def = ItemFactory.Instance.GetDefinition(selectedItem);
              if (def is ArcGIS.Core.Data.FeatureClassDefinition fcDef)
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
              if (ds is ArcGIS.Core.Data.FeatureDataset fds)
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
          });
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
    public static void AddFavorite()
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
    public static void InsertFavorite()
    {
      string gdbPath = "@C:\\myDataFolder\\myData.gdb";
      var itemGDB = ItemFactory.Instance.Create(gdbPath);

      // is the item already a favorite?
      var fav = FavoritesManager.Current.GetFavorite(itemGDB);
      // no; add it with IsAddedToAllNewProjects set to true
      if (fav != null)
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
    public static void AddFavoriteStyle()
    {
      StyleProjectItem styleItem = Project.Current.GetItems<StyleProjectItem>().
                              FirstOrDefault(style => (style.Name == "ArcGIS 3D"));
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
    public static void ToggleFavoriteFlag()
    {
      var itemFolder = ItemFactory.Instance.Create(@"d:\data");

      // is the folder item already a favorite?
      var fav = FavoritesManager.Current.GetFavorite(itemFolder);
      if (fav != null)
      {
        if (fav.IsAddedToAllNewProjects)
          FavoritesManager.Current.ClearIsAddedToAllNewProjects(fav.Item);
        else
          FavoritesManager.Current.SetIsAddedToAllNewProjects(fav.Item);
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorites
    #region Get the set of favorites and iterate
    public static void GetFavorites()
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
        // if it's a goedatabase item
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
    public static void RemoveAllFavorites()
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
    public static void FavoriteEvent()
    {
      ArcGIS.Desktop.Core.Events.FavoritesChangedEvent.Subscribe((args) =>
      {
        // favorites have changed
        int count = FavoritesManager.Current.GetFavorites().Count;
      });
    }
    #endregion

  }
}
