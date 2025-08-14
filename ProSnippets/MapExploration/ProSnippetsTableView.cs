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
    /// Sets the viewing mode of the active table view to "selected records".
    /// </summary>
    /// <remarks>This method changes the viewing mode of the active table view to show only selected records.</remarks>
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
    /// Sets the zoom level of the active table view to the specified value.
    /// </summary>
    /// <remarks>If there is no active table view, this method performs no action. The actual zoom level set
    /// may differ from the specified value depending on the current zoom state of the table view.</remarks>
    /// <param name="zoomLevel">The desired zoom level to apply to the active table view. Must be a non-negative integer.</param>
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
    /// Toggles the visibility of field aliases in the active table view.
    /// </summary>
    /// <remarks>This method toggles the visibility of field aliases in the active table view.</remarks>
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
    /// Toggles the visibility of subtype domain descriptions in the active table view.
    /// </summary>
    /// <remarks>This method toggles the visibility of subtype domain descriptions in the active table view.</remarks>
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
    /// Demonstrates how to retrieve the index of the active row in the currently active table view.
    /// </summary>
    /// <remarks>This method shows how to access the <see cref="TableView.Active"/> instance and obtain its
    /// <see cref="TableView.ActiveRowIndex"/> property. Use this approach when you need to determine which row is
    /// currently selected or active in a table view within ArcGIS Pro.</remarks>
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
    /// Changes the active row in the currently active table view by moving the selection to a new row index.
    /// </summary>
    /// <remarks>This method retrieves the active <see cref="TableView"/>, obtains its current active row
    /// index, and moves the selection to a new row by adding 10 to the current index. If there is no active table view,
    /// the method does nothing.</remarks>
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
    /// Retrieves the active object ID from the currently active table view, if available.
    /// </summary>
    /// <remarks>This method attempts to access the <see cref="TableView.Active"/> instance and obtain its
    /// <see cref="TableView.ActiveObjectId"/> property. If there is no active table view, the method performs no
    /// action.</remarks>
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
    /// Demonstrates how to convert between row index and object ID in the currently active table view.
    /// </summary>
    /// <remarks>This method shows how to access the <see cref="TableView.Active"/> instance and obtain its
    /// <see cref="TableView.ActiveRowIndex"/> and <see cref="TableView.ActiveObjectId"/> properties. Use this approach
    /// when you need to translate between row indices and object IDs in a table view within ArcGIS Pro.</remarks>
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
    /// Demonstrates how to retrieve the selected rows or row indexes from the currently active table view.
    /// </summary>
    /// <remarks>This method shows how to access the <see cref="TableView.Active"/> instance and obtain its
    /// <see cref="TableView.GetSelectedObjectIds"/> and <see cref="TableView.GetSelectedRowIndexes"/> methods.
    /// Use this approach when you need to work with selected rows in a table view within ArcGIS Pro.</remarks>
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
    /// Changes the selection of rows in the active table view to include specific object IDs.
    /// </summary>
    /// <remarks>This method modifies the selection in the currently active <see
    /// cref="ArcGIS.Desktop.Mapping.TableView"/> by first selecting a set of object IDs and then adding additional rows
    /// to the selection. If there is no active table view, the method performs no action. This operation is performed
    /// asynchronously on the main QueuedTask thread.</remarks>
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
    /// Selects all rows in the currently active table view, if available.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if selecting all rows is not
    /// supported in the current context.</remarks>
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
    /// Demonstrates how to toggle, switch, and clear row selections in the currently active table view.
    /// </summary>
    /// <remarks>This method accesses the <see cref="TableView.Active"/> instance and performs selection
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
    /// Zooms to the currently selected rows in the active table view, if available.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if zooming to selected rows is not
    /// supported in the current context.</remarks>
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
    /// Deletes the currently selected rows in the active table view, if deletion is permitted.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if the selected rows cannot be
    /// deleted. Use this method to programmatically remove selected records from the active table view in the
    /// application.</remarks>
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
    /// Gets the currently highlighted row indexes in the active table view, if available.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if getting highlighted row indexes is not
    /// supported in the current context.</remarks>
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
    /// Highlights the first two selected rows in the active table view, if available.
    /// </summary>
    /// <remarks>This method checks for an active table view and, if present, highlights up to the first two
    /// currently selected rows. If fewer than two rows are selected, no rows are highlighted. The operation is
    /// performed asynchronously on the main QueuedTask thread. If no table view is active, the method does
    /// nothing.</remarks>
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
    /// Toggles the highlight state of the currently selected rows in the active table view, if available.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if toggling row highlights is not
    /// supported in the current context.</remarks>
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
    /// Zooms to the currently highlighted rows in the active table view, if available.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if zooming to highlighted rows is not
    /// supported in the current context.</remarks>
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
    /// Deletes the currently highlighted rows in the active table view, if any are selected and deletable.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if no highlighted rows can be
    /// deleted. It is typically used to remove selected records from the currently focused table in the
    /// application.</remarks>
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
    /// Demonstrates how to access fields in the active table view.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view.</remarks>
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
    /// <remarks>This method retrieves the active field index and description from the currently active table view.
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
    /// Demonstrates how to retrieve and set the selected fields in the active <see
    /// cref="ArcGIS.Desktop.Mapping.TableView"/>.
    /// </summary>
    /// <remarks>This method shows how to use the <see
    /// cref="ArcGIS.Desktop.Mapping.TableView.GetSelectedFields"/> and <see
    /// cref="ArcGIS.Desktop.Mapping.TableView.SetSelectedFields(System.Collections.Generic.List{string})"/> methods to
    /// manage field selection in the currently active table view. If there is no active table view, the method performs
    /// no action.</remarks>
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
    /// Sets the field order of the active table view to display the "STATE_NAME" and "STATE_FIPS" fields in order,
    /// after resetting any custom field order.
    /// </summary>
    /// <remarks>This method operates on the currently active <see cref="ArcGIS.Desktop.Mapping.TableView"/>.
    /// If no table view is active, the method performs no action. The field order is first reset to the default before
    /// applying the new order. Only the specified fields ("STATE_NAME" and "STATE_FIPS") will be visible and in the
    /// given order after this operation.</remarks>
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
    /// Demonstrates how to show or hide fields in the active <see cref="ArcGIS.Desktop.Mapping.TableView"/>.
    /// </summary>
    /// <remarks>This method shows how to use the <see cref="ArcGIS.Desktop.Mapping.TableView.GetHiddenFields"/> and <see
    /// cref="ArcGIS.Desktop.Mapping.TableView.SetHiddenFields(List<string>)"/> methods to manage field visibility in the
    /// currently active table view. If there is no active table view, the method performs no action.</remarks>
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
    /// Freezes a specified set of fields in the active table view, replacing any previously frozen fields.
    /// </summary>
    /// <remarks>This method first clears all currently frozen fields in the active <see
    /// cref="ArcGIS.Desktop.Mapping.TableView"/>, then freezes the fields "CITY_FIPS" and "STATE_FIPS". If there is no
    /// active table view, the method performs no action. <para> Use this method to programmatically control which
    /// fields remain visible when horizontally scrolling in the table view. Only the specified fields will be frozen
    /// after this operation. </para> <para> This method is asynchronous but returns <see langword="void"/>; exceptions
    /// may be unhandled if not observed by the synchronization context. </para></remarks>
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
    /// Demonstrates how to sort fields in the active <see cref="ArcGIS.Desktop.Mapping.TableView"/>.
    /// </summary>
    /// <remarks>This method shows how to use the <see cref="ArcGIS.Desktop.Mapping.TableView.SortDescending"/> and <see
    /// cref="ArcGIS.Desktop.Mapping.TableView.SortAscending"/> methods to sort fields in the currently active table view.
    /// If there is no active table view, the method performs no action.</remarks>
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
    /// Demonstrates how to find and replace text in the active <see cref="ArcGIS.Desktop.Mapping.TableView"/>.
    /// </summary>
    /// <remarks>This method shows how to use the <see cref="ArcGIS.Desktop.Mapping.TableView.Find"/> and <see
    /// cref="ArcGIS.Desktop.Mapping.TableView.FindAndReplace"/> methods to find and replace text in the currently active
    /// table view. If there is no active table view, the method performs no action.</remarks>
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
    /// Displays the Go To dialog for the active table view, allowing navigation to a specific row.
    /// </summary>
    /// <remarks>If there is no active table view, or if the Go To operation is not available, this method
    /// does nothing.</remarks>
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
    /// Refreshes the currently active table view, if one is available and can be refreshed.
    /// </summary>
    /// <remarks>This method has no effect if there is no active table view or if the active table view does
    /// not support refreshing.</remarks>
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
    /// Renames the caption of the active table view.
    /// </summary>
    /// <remarks>This method shows how to use the <see cref="ArcGIS.Desktop.Mapping.ITablePaneEx.Caption"/> property
    /// to rename the caption of the currently active table view. If there is no active table view, the method
    /// performs no action.</remarks>
    public static void RenameTableCaption()
    {
      // find all the table panes (table panes hosting map data)
      var tablePanes = FrameworkApplication.Panes.OfType<ITablePane>();
      var tablePane = tablePanes.FirstOrDefault(p => (p as ITablePaneEx)?.Caption == "oldCaption");
      var tablePaneEx = tablePane as ITablePaneEx;
      if (tablePaneEx != null)
        tablePaneEx.Caption = "newCaption";

      // find all the external table panes (table panes hosting external data)
      var externalPanes = FrameworkApplication.Panes.OfType<IExternalTablePane>();
      var externalTablePane = externalPanes.FirstOrDefault(p => p.Caption == "oldCaption");
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
    /// Gets the <see cref="ArcGIS.Desktop.Mapping.TableView"/> associated with a table pane by its caption.
    /// </summary>
    /// <remarks>This method demonstrates how to retrieve the <see cref="ArcGIS.Desktop.Mapping.TableView"/> associated
    /// with a table pane by searching for the pane using its caption. It first looks for standard table panes
    /// that host map data, and if not found, it searches for external table panes that host external data.
    /// If no matching pane is found, the method does nothing.</remarks>
    public static void GetTableViewFromTablePane()
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
