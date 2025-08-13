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
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.ProSnippets
{
  public static class ProSnippetsBrowseDialog
  {
    // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Filter
    // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.InitialLocation
    // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Title
    // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog
    #region OpenItemDialog
    /// <summary>
    /// Demonstrates how to configure and use the <see cref="OpenItemDialog"/> class 
    /// to select items in ArcGIS Pro.
    /// </summary>
    public static void OpenItemDialog()
    {
      // Variables not used in samples
      OpenItemDialog selectItemDialog = new OpenItemDialog(); // in #region BrowseDialogItems

      /// Adds a single item to a map
      OpenItemDialog addToMapDialog = new()
      {
        Title = "Add To Map",
        InitialLocation = @"C:\Data\NewYork\Counties\Erie\Streets",
        Filter = ItemFilters.Composite_AddToMap
      };
    }
    #endregion //OpenItemDialog

    // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
    // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.ShowDialog
    // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog.Items
    // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog.MultiSelect
    #region Show_OpenItemDialog
    /// <summary>
    /// Displays an <see cref="OpenItemDialog"/> to allow users to select one or more items 
    /// from a specified location in ArcGIS Pro and creates maps from the selected items.
    /// </summary>
    public static void ShowOpenItemDialog()
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
    #endregion //Show_OpenItemDialog

    // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Filter
    // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.InitialLocation
    // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Title
    // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog
    #region SaveItemDialog
    /// <summary>
    /// Opens an <see cref="OpenItemDialog"/> to allow users to select one or more items 
    /// from a specified directory and creates maps from the selected items in ArcGIS Pro.
    /// </summary>
    public static void SaveItemDialog()
    {
      SaveItemDialog saveLayerFileDialog = new SaveItemDialog()
      {
        Title = "Save Layer File",
        InitialLocation = @"C:\Data\ProLayers\Geographic\Streets",
        Filter = ItemFilters.Files_All
      };
    }
    #endregion //SaveItemDialog

    // cref: Show_SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.DefaultExt
    // cref: Show_SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.FilePath
    // cref: Show_SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.OverwritePrompt
    #region Show_SaveItemDialog
    /// <summary>
    /// Displays a <see cref="SaveItemDialog"/> to allow users to save a map file 
    /// to a specified location in ArcGIS Pro. Prompts the user for confirmation 
    /// if the file already exists.
    /// </summary>
    public static void ShowSaveItemDialog()
    {
      SaveItemDialog saveMapFileDialog = new SaveItemDialog()
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
    #endregion //Show_SaveItemDialog

    // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.Items
    // cref: ArcGIS.Desktop.Catalog.OpenItemDialog
    // cref: BrowseDialogItems;ArcGIS.Desktop.Mapping.MapFactory.CreateMapFromItem
    #region BrowseDialogItems
    /// <summary>
    /// Displays an <see cref="OpenItemDialog"/> to allow users to select one or more items 
    /// from a specified location in ArcGIS Pro and creates maps from the selected items.
    /// </summary>
    public static void BrowseSelectedDialogItems()
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
          ArcGIS.Desktop.Mapping.MapFactory.Instance.CreateMapFromItem(selectedDialogItem);
      }

    }
    #endregion //BrowseDialogItems

  }
}
