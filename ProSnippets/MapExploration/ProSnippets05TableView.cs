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
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
  /// <summary>
  /// Provides a collection of utility methods for interacting with and manipulating table views in ArcGIS Pro.
  /// </summary>
  /// <remarks>This static class contains methods to perform various operations on table views, such as setting
  /// view modes,  managing selections, zooming or panning to rows, sorting, and accessing or modifying fields. These
  /// methods  primarily operate on the active table view and leverage the ArcGIS Pro SDK's <see
  /// cref="ArcGIS.Desktop.Mapping.TableView"/> API.</remarks>
  public static class ProSnippetsTableView
  {
    #region ProSnippet Group: TableView
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.SetViewMode(TableViewMode)
    // cref: ArcGIS.Desktop.Mapping.TableViewMode
    // cref: ArcGIS.Desktop.Mapping.TableViewMode.eSelectedRecords
    // cref: ArcGIS.Desktop.Mapping.TableViewMode.eAllRecords
    #region Set Table ViewingMode
    /// <summary>
    /// Sets the table view to display only selected records.
    /// </summary>
    public static void SetViewingModelTableView()
    {
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // change to "selected record" mode
      tableView.SetViewMode(TableViewMode.eSelectedRecords);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.SetZoomLevel(System.Int32)
    // cref: ArcGIS.Desktop.Mapping.TableView.ZoomLevel
    #region Set ZoomLevel
    /// <summary>
    /// Sets the zoom level of the active table view.
    /// </summary>
    /// <param name="zoomLevel">The desired zoom level.</param>
    public static void SetZoomLevelTableView(int zoomLevel)
    {
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // change zoom level
      if (tableView.ZoomLevel > zoomLevel)
        tableView.SetZoomLevel(zoomLevel);
      else
        tableView.SetZoomLevel(zoomLevel * 2);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.ShowFieldAlias
    // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleFieldAlias
    // cref: ArcGIS.Desktop.Mapping.TableView.ToggleFieldAlias()
    #region Toggle Field Alias
    /// <summary>
    /// Shows or toggles the field alias in the active table view.
    /// </summary>
    public static void AliasTableView()
    {
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // set the value
      tableView.ShowFieldAlias = true;

      // OR toggle it
      if (tableView.CanToggleFieldAlias)
        tableView.ToggleFieldAlias();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.ShowSubtypeDomainDescriptions
    // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleSubtypeDomainDescriptions
    // cref: ArcGIS.Desktop.Mapping.TableView.ToggleSubtypeDomainDescriptionsAsync()
    #region Toggle Subtype Descriptions
    /// <summary>
    /// Shows or toggles subtype domain descriptions in the active table view.
    /// </summary>
    public static void DomainDescTableView()
    {
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // set the value
      tableView.ShowSubtypeDomainDescriptions = true;

      // OR toggle it
      if (tableView.CanToggleSubtypeDomainDescriptions)
        tableView.ToggleSubtypeDomainDescriptionsAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRowIndex
    #region Get the active row 
    /// <summary>
    /// Gets the active row index from the active table view.
    /// </summary>
    public static void GetActiveRowIndexTableView()
    {
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active rowindex
      int rowIndex = tableView.ActiveRowIndex;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRowIndex
    // cref: ArcGIS.Desktop.Mapping.TableView.BringIntoView(System.Int32,System.Int32)
    #region Change the active row 
    /// <summary>
    /// Changes the active row index in the active table view.
    /// </summary>
    public static async void ChangeActiveRowIndexTableView()
    {
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active rowindex
      int rowIndex = tableView.ActiveRowIndex;

      // move to a different row
      var newIndex = 10 + rowIndex;
      await tableView.BringIntoView(newIndex);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.ActiveObjectId
    #region Get the active object ID
    /// <summary>
    /// Gets the active object ID from the active table view.
    /// </summary>
    public static void GetActiveOIDTableView()
    {
      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active objectID
      long? OID = tableView.ActiveObjectId;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRoWindex
    // cref: ArcGIS.Desktop.Mapping.TableView.GetObjectIdAsync(System.Int32)
    // cref: ArcGIS.Desktop.Mapping.TableView.GetRowIndexAsync(System.Int64, System.Boolean)
    #region Translate between rowIndex and objectID
    /// <summary>
    /// Converts between row index and object ID in the active table view.
    /// </summary>
    public static async void ConvertBetweenRowAndObjectIDTableView()
    {

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active rowindex
      int rowIndex = tableView.ActiveRowIndex;
      // increase
      int newIndex = rowIndex + 10;
      // get the objectID
      long newOID = await tableView.GetObjectIdAsync(newIndex);

      // get the rowIndex for a specific objectID
      //   2nd parameter indicates if the search only occurs for the pages loaded
      //   if pass false, then in the worst case, a full table scan will occur to 
      //    find the objectID.
      long OID = 100;
      var idx = await tableView.GetRowIndexAsync(OID, true);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedObjectIds
    // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedRowIndexes
    #region Get selected rows  or row indexes
    /// <summary>
    /// Gets the selected object IDs and row indexes from the active table view.
    /// </summary>
    public static void GetSelectionTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(() =>
      {
        // get the set of selected objectIDs 
        var selOids = tv.GetSelectedObjectIds();
        // get the set of selected row indexes
        var selRows = tv.GetSelectedRowIndexes();

      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.Select(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedRowIndexes
    #region Change selected rows 
    /// <summary>
    /// Sets or adds to the selection of rows in the active table view.
    /// </summary>
    public static void SetSelectionTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(() =>
      {
        // set of selected OIDS
        var newoids = new List<long>();
        newoids.AddRange(new List<long>() { 10, 15, 17 });
        tv.Select(newoids, true);


        // add to set of selected row indexes
        var selRows = tv.GetSelectedRowIndexes();
        var newRows = new List<long>(selRows);
        newRows.AddRange(new List<long>() { 21, 35 });
        tv.Select(newRows, false);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanSelectAll
    // cref: ArcGIS.Desktop.Mapping.TableView.SelectAll()
    #region Select all rows
    /// <summary>
    /// Selects all rows in the active table view.
    /// </summary>
    public static void SelectAllTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanSelectAll)
        tv.SelectAll();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleRowSelection
    // cref: ArcGIS.Desktop.Mapping.TableView.ToggleRowSelection()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanSwitchSelection
    // cref: ArcGIS.Desktop.Mapping.TableView.SwitchSelection()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanClearSelection
    // cref: ArcGIS.Desktop.Mapping.TableView.ClearSelection()
    #region Toggle, Switch, Clear Selection
    /// <summary>
    /// Toggles, switches, or clears the selection in the active table view.
    /// </summary>
    public static void OtherSelectionTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // toggle the active rows selection
      if (tv.CanToggleRowSelection)
        tv.ToggleRowSelection();

      // switch the selection
      if (tv.CanSwitchSelection)
        tv.SwitchSelection();

      // clear the selection
      if (tv.CanClearSelection)
        tv.ClearSelection();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanZoomToSelected
    // cref: ArcGIS.Desktop.Mapping.TableView.ZoomToSelected()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanPanToSelected
    // cref: ArcGIS.Desktop.Mapping.TableView.PanToSelected()
    #region Zoom or Pan To Selected Rows
    /// <summary>
    /// Zooms or pans to the selected rows in the active table view.
    /// </summary>
    public static void ZoomSelectedTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanZoomToSelected)
        tv.ZoomToSelected();

      if (tv.CanPanToSelected)
        tv.PanToSelected();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanDeleteSelected
    // cref: ArcGIS.Desktop.Mapping.TableView.DeleteSelected()
    #region Delete Selected Rows
    /// <summary>
    /// Deletes the selected rows in the active table view.
    /// </summary>
    public static void DeleteSelectedTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanDeleteSelected)
        tv.DeleteSelected();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanGetHighlightedObjectIds
    // cref: ArcGIS.Desktop.Mapping.TableView.GetHighlightedObjectIds
    #region Get highlighted row indexes
    /// <summary>
    /// Gets the highlighted object IDs from the active table view.
    /// </summary>
    public static void GetHighlightTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(() =>
      {
        IReadOnlyList<long> highlightedOIDs = null;
        if (tv.CanGetHighlightedObjectIds)
          // get the set of selected objectIDs 
          highlightedOIDs = tv.GetHighlightedObjectIds();
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanHighlight
    // cref: ArcGIS.Desktop.Mapping.TableView.Highlight(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
    #region Change highlighted rows 
    /// <summary>
    /// Highlights specific rows in the active table view.
    /// </summary>
    public static void SetHighlightedTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(() =>
      {
        // get list of current selected objectIDs
        var selectedObjectIds = tv.GetSelectedObjectIds();
        var idsToHighlight = new List<long>();

        // add the first two selected objectIds to highlight
        if (selectedObjectIds.Count >= 2)
        {
          idsToHighlight.Add(selectedObjectIds[0]);
          idsToHighlight.Add(selectedObjectIds[1]);
        }

        // highlight
        if (tv.CanHighlight)
          tv.Highlight(idsToHighlight, true);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleRowHighlight
    // cref: ArcGIS.Desktop.Mapping.TableView.ToggleRowHighlight()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanSwitchHighlight
    // cref: ArcGIS.Desktop.Mapping.TableView.SwitchHighlight()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanClearHighlighted
    // cref: ArcGIS.Desktop.Mapping.TableView.ClearHighlighted()
    #region Toggle, Switch, Clear Highlights
    /// <summary>
    /// Toggles, switches, or clears highlights in the active table view.
    /// </summary>
    public static void OtherHighlightedTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // toggle the active rows selection
      if (tv.CanToggleRowHighlight)
        tv.ToggleRowHighlight();

      // switch highlighted rows
      if (tv.CanSwitchHighlight)
        tv.SwitchHighlight();

      // clear the highlights
      if (tv.CanClearHighlighted)
        tv.ClearHighlighted();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanZoomToHighlighted
    // cref: ArcGIS.Desktop.Mapping.TableView.ZoomToHighlighted()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanPanToHighlighted
    // cref: ArcGIS.Desktop.Mapping.TableView.PanToHighlighted()
    #region Zoom or Pan To Highlighted Rows
    /// <summary>
    /// Zooms or pans to the highlighted rows in the active table view.
    /// </summary>
    public static void ZoomHighlightedTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanZoomToHighlighted)
        tv.ZoomToHighlighted();

      if (tv.CanPanToHighlighted)
        tv.PanToHighlighted();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanDeleteHighlighted
    // cref: ArcGIS.Desktop.Mapping.TableView.DeleteHighlighted()
    #region Delete Highlighted Rows
    /// <summary>
    /// Deletes the highlighted rows in the active table view.
    /// </summary>
    public static void DeleteHighlightedTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanDeleteHighlighted)
        tv.DeleteHighlighted();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.GetFields()
    // cref: ArcGIS.Desktop.Mapping.TableView.GetFieldIndex(System.String)
    // cref: ArcGIS.Desktop.Mapping.TableView.GetField(System.Int32)
    // cref: ArcGIS.Desktop.Mapping.FieldDescription
    #region Field Access
    /// <summary>
    /// Accesses fields and field descriptions in the active table view.
    /// </summary>
    public static void FieldsTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // field access
      var flds = tv.GetFields();
      var fldIdx = tv.GetFieldIndex("STATE_NAME");
      var fldDesc = tv.GetField(fldIdx);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.ActiveFieldIndex
    // cref: ArcGIS.Desktop.Mapping.TableView.GetField(System.Int32)
    // cref: ArcGIS.Desktop.Mapping.TableView.SetActiveField(System.String)
    // cref: ArcGIS.Desktop.Mapping.TableView.SetActiveField(System.Int32)
    // cref: ArcGIS.Desktop.Mapping.FieldDescription
    #region Get or set the Active Field
    /// <summary>
    /// Gets or sets the active field in the active table view.
    /// </summary>
    public static void ActiveFieldTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get active field, active field name
      var activeFieldIdx = tv.ActiveFieldIndex;
      var fldDesc = tv.GetField(activeFieldIdx);
      var fldName = fldDesc.Name;

      // set active field by name
      tv.SetActiveField("STATE_NAME");

      // or set active field by index
      tv.SetActiveField(3);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedFields
    // cref: ArcGIS.Desktop.Mapping.TableView.SetSelectedFields(List<string>)
    #region Select Fields
    /// <summary>
    /// Gets or sets the selected fields in the active table view.
    /// </summary>
    public static void SelectFieldTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get selected fields
      var selectedfields = tv.GetSelectedFields();

      // set selected fields
      tv.SetSelectedFields(new List<string> { "CITY_FIPS", "STATE_FIPS" });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanResetFieldOrder()
    // cref: ArcGIS.Desktop.Mapping.TableView.ResetFieldOrder()
    // cref: ArcGIS.Desktop.Mapping.TableView.SetFieldOrderAsync(List<string>)
    #region Set Field Order 
    /// <summary>
    /// Sets the field order in the active table view.
    /// </summary>
    public static async void SetFieldOrderTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanResetFieldOrder)
      {
        tv.ResetFieldOrder();

        var fldOrder = new List<string>();
        fldOrder.Add("STATE_NAME");
        fldOrder.Add("STATE_FIPS");
        await tv.SetFieldOrderAsync(fldOrder);
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.GetHiddenFields()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanShowAllFields
    // cref: ArcGIS.Desktop.Mapping.TableView.ShowAllFields()
    // cref: ArcGIS.Desktop.Mapping.TableView.SetHiddenFields(List<string>)
    // cref: ArcGIS.Desktop.Mapping.TableView.CanHideSelectedFields
    // cref: ArcGIS.Desktop.Mapping.TableView.HideSelectedFields()
    #region Show or Hide Fields
    /// <summary>
    /// Shows or hides fields in the active table view.
    /// </summary>
    public static void HideFieldsTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get list of hidden fields
      var hiddenFields = tv.GetHiddenFields();

      // show all fields
      if (tv.CanShowAllFields)
        tv.ShowAllFields();

      // hide only "CITY_FIPS", "STATE_FIPS"
      if (tv.CanShowAllFields)
      {
        // show all fields
        tv.ShowAllFields();
        tv.SetHiddenFields(new List<string> { "CITY_FIPS", "STATE_FIPS" });
      }

      // add "STATE_NAME to set of hidden fields
      tv.SetHiddenFields(new List<string> { "STATE_NAME" });

      // hide selected fields
      if (tv.CanHideSelectedFields)
        tv.HideSelectedFields();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.GetFrozenFields()
    // cref: ArcGIS.Desktop.Mapping.TableView.ClearAllFrozenFieldsAsync()
    // cref: ArcGIS.Desktop.Mapping.TableView.SetFrozenFieldsAsync(List<string>)
    #region Freeze Fields
    /// <summary>
    /// Freezes or unfreezes fields in the active table view.
    /// </summary>
    public static async void FreezeFieldsTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get list of frozen fields
      var frozenfields = tv.GetFrozenFields();

      // unfreeze all fields
      await tv.ClearAllFrozenFieldsAsync();

      // freeze a set of fields
      await tv.SetFrozenFieldsAsync(new List<string> { "CITY_FIPS", "STATE_FIPS" });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanSortDescending
    // cref: ArcGIS.Desktop.Mapping.TableView.SortDescending()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanSortAscending
    // cref: ArcGIS.Desktop.Mapping.TableView.SortAscending()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanCustomSort
    // cref: ArcGIS.Desktop.Mapping.TableView.CustomSort()
    // cref: ArcGIS.Desktop.Mapping.TableView.SortAsync
    #region Sort 
    /// <summary>
    /// Sorts the table view by the active field or custom sort order.
    /// </summary>
    public static async void SortTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // sort the active field descending
      if (tv.CanSortDescending)
        tv.SortDescending();


      // sort the active field ascending
      if (tv.CanSortAscending)
        tv.SortAscending();


      // perform a custom sort programmatically
      if (tv.CanCustomSort)
      {
        // sort fields
        var dict = new Dictionary<string, FieldSortInfo>();
        dict.Add("STATE_NAME", FieldSortInfo.Asc);
        dict.Add("CITY_NAME", FieldSortInfo.Desc);
        await tv.SortAsync(dict);
      }

      // perform a custom sort via the UI
      if (tv.CanCustomSort)
        tv.CustomSort();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanFind
    // cref: ArcGIS.Desktop.Mapping.TableView.Find()
    // cref: ArcGIS.Desktop.Mapping.TableView.CanFindAndReplace
    // cref: ArcGIS.Desktop.Mapping.TableView.FindAndReplace()
    #region Find and Replace
    /// <summary>
    /// Launches the find or find and replace UI in the active table view.
    /// </summary>
    public static void FindReplaceTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // launch the find UI
      if (tv.CanFind)
        tv.Find();


      // or launch the find and replace UI
      if (tv.CanFindAndReplace)
        tv.FindAndReplace();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanGoTo
    // cref: ArcGIS.Desktop.Mapping.TableView.GoTo()
    #region GoTo TableView
    /// <summary>
    /// Launches the GoTo UI in the active table view.
    /// </summary>
    public static void GoToTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // launch the GoTo UI
      if (tv.CanGoTo)
        tv.GoTo();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TableView.CanRefresh
    // cref: ArcGIS.Desktop.Mapping.TableView.Refresh()
    #region Refresh
    /// <summary>
    /// Refreshes the active table view.
    /// </summary>
    public static void RefreshTableView()
    {
      var tv = TableView.Active;
      if (tv == null)
        return;

      // refresh
      if (tv.CanRefresh)
        tv.Refresh();
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
    // cref: ArcGIS.Desktop.Mapping.ITablePane
    // cref: ArcGIS.Desktop.Mapping.ITablePaneEx
    // cref: ArcGIS.Desktop.Mapping.ITablePaneEx.Caption
    // cref: ArcGIS.Desktop.Mapping.IExternalTablePane
    // cref: ArcGIS.Desktop.Mapping.IExternalTablePane.Caption
    #region Change table View caption
    /// <summary>
    /// Changes the caption of a table view pane.
    /// </summary>
    public static void TableCaption()
    {
      // find all the table panes (table panes hosting map data)
      var tablePanes = FrameworkApplication.Panes.OfType<ITablePane>();
      var tablePane = tablePanes.FirstOrDefault(p => (p as ITablePaneEx)?.Caption == "oldcCaption");
      var tablePaneEx = tablePane as ITablePaneEx;
      if (tablePaneEx != null)
        tablePaneEx.Caption = "newCaption";

      // find all the external table panes (table panes hosting external data)
      var externalPanes = FrameworkApplication.Panes.OfType<IExternalTablePane>();
      var externalTablePane = externalPanes.FirstOrDefault(p => p.Caption == "oldcCaption");
      if (externalTablePane != null)
        externalTablePane.Caption = "newCaption";
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
    // cref: ArcGIS.Desktop.Mapping.ITablePane
    // cref: ArcGIS.Desktop.Mapping.ITablePaneEx
    // cref: ArcGIS.Desktop.Mapping.ITablePaneEx.TableView
    // cref: ArcGIS.Desktop.Mapping.IExternalTablePane
    // cref: ArcGIS.Desktop.Mapping.IExternalTablePane.TableView
    #region Get TableView from table pane
    /// <summary>
    /// Gets the TableView from a table pane or external table pane.
    /// </summary>
    public static void TableViewFromPane()
    {
      TableView tv = null;

      // find all the table panes (table panes hosting map data)
      var tablePanes = FrameworkApplication.Panes.OfType<ITablePane>();
      var tablePane = tablePanes.FirstOrDefault(p => (p as ITablePaneEx)?.Caption == "caption");
      var tablePaneEx = tablePane as ITablePaneEx;
      if (tablePaneEx != null)
        tv = tablePaneEx.TableView;

      // if it's not found, maybe it's an external table pane
      if (tv == null)
      {
        // find all the external table panes (table panes hosting external data)
        var externalPanes = FrameworkApplication.Panes.OfType<IExternalTablePane>();
        var externalTablePane = externalPanes.FirstOrDefault(p => p.Caption == "caption");
        if (externalTablePane != null)
          tv = externalTablePane.TableView;
      }
    }
    #endregion
  }
}
