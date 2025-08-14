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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing.Events;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public static class ProSnippetsEnableEventEditing
  {

    #region ProSnippet Group: Enable Editing
    #endregion

    // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
    // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
    #region Enable Editing
    /// <summary>
    /// Enables editing for the current project asynchronously.
    /// </summary>
    /// <remarks>This method checks if editing is already enabled for the current project.  If editing is not
    /// enabled, it enables editing and returns the result.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result is  <see langword="true"/> if editing is
    /// successfully enabled or already enabled;  otherwise, <see langword="false"/>.</returns>
    public static Task<bool> EnableEditingAsync()
    {
      // if not editing
      if (Project.Current.IsEditingEnabled) return Task.FromResult(true);
      return Project.Current.SetIsEditingEnabledAsync(true);
    }
    #endregion

    // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
    // cref: ArcGIs.Desktop.Core.Project.HasEdits
    // cref: ArcGIs.Desktop.Core.Project.DiscardEditsAsync
    // cref: ArcGIs.Desktop.Core.Project.SaveEditsAsync
    // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
    #region Disable Editing
    /// <summary>
    /// Disables editing for the current project.
    /// </summary>
    /// <remarks>If the project is not currently in editing mode, the method returns <see langword="true"/>
    /// immediately. If there are unsaved edits, the method will either save or discard them based on the value of
    /// <paramref name="discardEdits"/>.</remarks>
    /// <param name="discardEdits">A value indicating whether to discard unsaved edits.  If <see langword="true"/>, unsaved edits will be
    /// discarded;  otherwise, they will be saved before disabling editing.</param>
    /// <returns><see langword="true"/> if editing was successfully disabled;  otherwise, <see langword="false"/>.</returns>
    public static bool DisableEditing(bool discardEdits)
    {
      // if editing
      if (!Project.Current.IsEditingEnabled) return true;
      // check for edits
      if (Project.Current.HasEdits)
      {
        if (discardEdits)
          Project.Current.DiscardEditsAsync();
        else
          Project.Current.SaveEditsAsync();
      }
      return !Project.Current.SetIsEditingEnabledAsync(false).Result;
    }
    #endregion

    #region ProSnippet Group: Row Events
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.RowChangedEventArgs},ArcGIS.Core.Data.Table,System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.RowChangedEventArgs},ArcGIS.Core.Data.Table,System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.RowChangedEventArgs},ArcGIS.Core.Data.Table,System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs
    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent
    #region Subscribe to Row Events
    /// <summary>
    /// Subscribes to row events for the currently selected feature layer in the active map view.
    /// </summary>
    /// <remarks>This method subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowCreatedEvent"/>, 
    /// <see cref="ArcGIS.Desktop.Editing.Events.RowChangedEvent"/>, and  <see
    /// cref="ArcGIS.Desktop.Editing.Events.RowDeletedEvent"/> for the table associated with the  selected feature
    /// layer. The event handlers are invoked when rows are created, modified, or deleted.  The method runs
    /// asynchronously on a queued task to ensure thread safety when accessing the map view  and layer data.</remarks>
    public static void SubscribeRowEvent()
    {
      QueuedTask.Run(() =>
      {
        //Listen for row events on a layer
        var featLayer = MapView.Active.GetSelectedLayers()[0] as FeatureLayer;
        var layerTable = featLayer.GetTable();

        //subscribe to row events
        var rowCreateToken = RowCreatedEvent.Subscribe(OnRowCreated, layerTable);
        var rowChangeToken = RowChangedEvent.Subscribe(OnRowChanged, layerTable);
        var rowDeleteToken = RowDeletedEvent.Subscribe(OnRowDeleted, layerTable);
      });
    }

    public static void OnRowCreated(RowChangedEventArgs args)
    {
    }

    public static void OnRowChanged(RowChangedEventArgs args)
    {
    }

    public static void OnRowDeleted(RowChangedEventArgs args)
    {
    }
    #endregion


    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
    #region Create a record in a separate table in the Map within Row Events
/// <summary>
/// Subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowCreatedEvent"/> for the first <see
/// cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> in the active map.
/// </summary>
/// <remarks>This method attaches an event handler to receive notifications when a new row is created in the
/// associated table of the first feature layer found in the active map. Use this method to enable custom logic in
/// response to row creation events within your ArcGIS Pro add-in.</remarks>
    public static void HookRowCreatedEvent()
    {
      // subscribe to the RowCreatedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(MyRowCreatedEvent, table);
    }

    /// <summary>
    /// Handles the event triggered when a new row is created, allowing custom logic to be executed in response to the
    /// creation.
    /// </summary>
    /// <remarks>This method is typically used as a callback for row creation events within an edit operation.
    /// It records audit information for the newly created row in a designated audit table. The method is always invoked
    /// on the QueuedTask, so additional task scheduling is not required.</remarks>
    /// <param name="args">The event arguments containing information about the created row and the associated edit operation. Must not be
    /// <c>null</c>.</param>
public static void MyRowCreatedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      // get the edit operation
      var parentEditOp = args.Operation;

      // set up some attributes
      var attribs = new Dictionary<string, object>
      {
        { "Layer", "Parcels" },
        { "Description", "objectId: " + args.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString() }
      };

      //create a record in an audit table
      var sTable = MapView.Active.Map.FindStandaloneTables("EditHistory")[0];
      var table = sTable.GetTable();
      parentEditOp.Create(table, attribs);
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
    #region Create a record in a separate table within Row Events

/// <summary>
/// Subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowCreatedEvent"/> for the first feature layer in the
/// active map.
/// </summary>
/// <remarks>This method attaches an event handler to the <see
/// cref="ArcGIS.Desktop.Editing.Events.RowCreatedEvent"/> for the table associated with the first <see
/// cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> in the active map. Use this method to monitor when new rows are created
/// in that table, enabling custom logic to be executed in response to record creation events. <para> If no feature
/// layers are present in the active map, the subscription will not be established. </para></remarks>
    public static void HookCreatedEvent()
    {
      // subscribe to the RowCreatedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(OnRowCreatedEvent, table);
    }

    /// <summary>
    /// Handles the row creation event by updating a separate table with information about the newly created row.
    /// </summary>
    /// <remarks>This method is intended to be called within a row event callback and updates a table that is
    /// not part of the map. The ArcGIS.Core.Data API must be used for editing the table; do not initiate a new edit
    /// operation within row event callbacks. Row event callbacks are always executed on the QueuedTask, so additional
    /// task wrapping is unnecessary.</remarks>
    /// <param name="args">The event arguments containing details about the created row and the associated edit operation.</param>
    public static void OnRowCreatedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      // update a separate table not in the map when a row is created
      // You MUST use the ArcGIS.Core.Data API to edit the table. Do NOT
      // use a new edit operation in the RowEvent callbacks
      try
      {
        // get the edit operation
        var parentEditOp = args.Operation;

        // set up some attributes
        var attribs = new Dictionary<string, object>
        {
          { "Description", "objectId: " + args.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString() }
        };

        // update Notes table with information about the new feature
        using var geoDatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(Project.Current.DefaultGeodatabasePath)));
        using var table = geoDatabase.OpenDataset<Table>("Notes");
        parentEditOp.Create(table, attribs);
      }
      catch (Exception e)
      {
        MessageBox.Show($@"Error in OnRowCreated for objectId: {args.Row.GetObjectID()} : {e}");
      }
    }
    #endregion


    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Row
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Guid
    // cref: ArcGIS.Core.Data.Row.HasValueChanged(System.Int32)
    // cref: ArcGIS.Core.Data.Row.Store
    #region Modify a record within Row Events - using Row.Store
    /// <summary>
    /// Subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowChangedEvent"/> for the first feature layer in the
    /// active map.
    /// </summary>
    /// <remarks>This method attaches an event handler to receive notifications when a row in the selected
    /// table is changed. The subscription targets the first <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> found in
    /// the active map. Use this method to monitor and respond to edits made to table records within the current map
    /// context.</remarks>
    public static void HookRowChangedEvent()
    {
      // subscribe to the RowChangedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(OnRowChangedEvent, table);
    }

    /// <summary>
    /// Handles the row changed event by validating and updating the affected row.
    /// </summary>
    /// <remarks>This method validates changes to the "POLICE_DISTRICT" field and cancels the edit if the new
    /// value is invalid. If the change is valid, it updates the "Description" field and stores the row. The method
    /// prevents recursive event handling by tracking the row's unique identifier.</remarks>
    /// <param name="args">The event arguments containing information about the changed row and the event context.</param>
    public static void OnRowChangedEvent(RowChangedEventArgs args)
    {
      // set the current row changed guid when you execute the Row.Store method
      // used for re-entry checking
      Guid _currentRowChangedGuid = new();
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      var row = args.Row;

      // check for re-entry  (only if row.Store is called)
      if (_currentRowChangedGuid == args.Guid)
        return;

      var fldIdx = row.FindField("POLICE_DISTRICT");
      if (fldIdx != -1)
      {
        //Validate any change to �police district�
        //   cancel the edit if validation on the field fails
        if (row.HasValueChanged(fldIdx))
        {
          // cancel edit with invalid district (5)
          var value = row["POLICE_DISTRICT"].ToString();
          if (value == "5")
          {
            //Cancel edits with invalid �police district� values
            args.CancelEdit($"Police district {row["POLICE_DISTRICT"]} is invalid");
          }
        }

        // update the description field
        row["Description"] = "Row Changed";

        //  this update with cause another OnRowChanged event to occur
        //  keep track of the row guid to avoid recursion
        _currentRowChangedGuid = args.Guid;
        row.Store();
        _currentRowChangedGuid = Guid.Empty;
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Core.Data.Row, System.String, System.Object)
    #region Modify a record within Row Events - using EditOperation.Modify
    /// <summary>
    /// Subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowChangedEvent"/> for a specified table.
    /// </summary>
    /// <remarks>This method subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowChangedEvent"/> for
    /// the first  <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> in the active map. The event handler can be used to
    /// respond to changes in rows within the table, such as modifications or updates.</remarks>
    public static void HookChangedEvent()
    {
      // subscribe to the RowChangedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(MyRowChangedEvent, table);
    }

    

    /// <summary>
    /// Handles the event triggered when a row is changed, allowing for custom modifications to the row.
    /// </summary>
    /// <remarks>This method is always executed on the QueuedTask, so there is no need to wrap the code within
    /// a <see cref="ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run"/> lambda. It ensures that modifications to
    /// the row are performed without causing recursion by tracking the unique identifier of the last edit.</remarks>
    /// <param name="args">The event arguments containing details about the row change, including the row, operation, and unique identifier
    /// for the change.</param>
    public static void MyRowChangedEvent(RowChangedEventArgs args)
    { 
      // set the last row changed guid when you execute the Row.Store method
      // used for re-entry checking
      Guid _lastEdit = new();

      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      //example of modifying a field on a row that has been created
      var parentEditOp = args.Operation;

      // avoid recursion
      if (_lastEdit != args.Guid)
      {
        //update field on change
        parentEditOp.Modify(args.Row, "ZONING", "New");

        _lastEdit = args.Guid;
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Row
    // cref: ArcGIS.Core.Data.Row.GetOriginalvalue
    #region Determine if Geometry Changed while editing
    private static FeatureLayer modifiedFeatureLayer;
    /// <summary>
    /// Subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowChangedEvent"/> for the specified feature layer.
    /// </summary>
    /// <remarks>This method sets up a listener for row change events on the table associated with the
    /// provided feature layer. The event subscription occurs within a queued task to ensure thread safety.</remarks>
    /// <param name="featureLayer">The feature layer whose table will be monitored for row changes.</param>
    public static void DetermineGeometryChange(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
      {
        //Listen to the RowChangedEvent that occurs when a Row is changed.
        modifiedFeatureLayer = featureLayer;
        ArcGIS.Desktop.Editing.Events.RowChangedEvent.Subscribe(OnRowChangedEvent2, featureLayer.GetTable());
      });
    }
    /// <summary>
    /// Handles the event triggered when a row in a feature layer is changed.
    /// </summary>
    /// <remarks>This method is invoked on the QueuedTask, so there is no need to wrap the code within a  <see
    /// cref="ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run"/> lambda. It provides access  to the original and
    /// new geometry of the modified row, allowing for comparison or further processing.</remarks>
    /// <param name="args">The event arguments containing information about the changed row.</param>
    public static void OnRowChangedEvent2(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      //Get the layer's definition
      var lyrDefn = modifiedFeatureLayer.GetFeatureClass().GetDefinition();
      //Get the shape field of the feature class
      string shapeField = lyrDefn.GetShapeField();
      //Index of the shape field
      var shapeIndex = lyrDefn.FindField(shapeField);
      //Original geometry of the modified row
      var geomOrig = args.Row.GetOriginalValue(shapeIndex) as Geometry;
      //New geometry of the modified row
      var geomNew = args.Row[shapeIndex] as Geometry;
      //Compare the two
      bool shapeChanged = geomOrig.IsEqual(geomNew);
      if (shapeChanged)
      {
        // The geometry has not changed
        Console.WriteLine("Geometry has not changed");
      }
      else
      {
        // The geometry has changed
        Console.WriteLine("Geometry has changed");
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit(System.String, System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit()
    #region Cancel a delete
    /// <summary>
    /// Subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowDeletedEvent"/> to monitor and potentially cancel
    /// delete operations on a table.
    /// </summary>
    /// <remarks>This method subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.RowDeletedEvent"/> for
    /// the first feature layer's table in the active map.  The event handler can be used to intercept and handle row
    /// deletion events, such as canceling a delete operation if certain conditions are met.</remarks>
    public static void StopADelete()
    {
      // subscribe to the RowDeletedEvent for the appropriate table
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowDeletedEvent.Subscribe(OnRowDeletedEvent, table);
    }

    public static Guid _currentRowDeletedGuid = Guid.Empty;
    /// <summary>
    /// Handles the event triggered when a row is deleted, allowing for custom logic to be executed and the deletion to
    /// be conditionally canceled.
    /// </summary>
    /// <remarks>This method is invoked on the QueuedTask, so there is no need to wrap the logic within a
    /// QueuedTask.Run lambda. Use the <see cref="RowChangedEventArgs.CancelEdit(string, bool)"/> method to cancel the
    /// deletion, optionally displaying a dialog to the user. Note that feature edits on Hosted and Standard Feature
    /// Services cannot be canceled.</remarks>
    /// <param name="args">The event arguments containing information about the deleted row, including its data and a unique identifier for
    /// the event.</param>
    public static void OnRowDeletedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      var row = args.Row;

      // check for re-entry 
      if (_currentRowDeletedGuid == args.Guid)
        return;

      // cancel the delete if the feature is in Police District 5
      var fldIdx = row.FindField("POLICE_DISTRICT");
      if (fldIdx != -1)
      {
        var value = row[fldIdx].ToString();
        if (value == "5")
        {
          //cancel with dialog
          // Note - feature edits on Hosted and Standard Feature Services cannot be cancelled.
          args.CancelEdit("Delete Event\nAre you sure", true);

          // or cancel without a dialog
          // args.CancelEdit();
        }
      }
      _currentRowDeletedGuid = args.Guid;
    }
    #endregion

    #region ProSnippet Group: EditCompletedEvent
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEvent
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Creates
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Modifies
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Deletes
    #region Subscribe to EditCompletedEvent
    /// <summary>
    /// Subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.EditCompletedEvent"/> to handle edit completion
    /// events.
    /// </summary>
    /// <remarks>This method subscribes to the <see cref="ArcGIS.Desktop.Editing.Events.EditCompletedEvent"/>,
    /// allowing the application  to respond to edit operations such as feature creation, modification, or deletion. The
    /// subscription is tied to the  <see cref="OnEditCompleted"/> handler, which processes the event when it
    /// occurs.</remarks>
    public static void SubEditEvents()
    {
      //subscribe to edit completed
      var eceToken = EditCompletedEvent.Subscribe(OnEditCompleted);
    }
    /// <summary>
    /// Handles the completion of an edit operation by processing and displaying the number of created, modified, and
    /// deleted items.
    /// </summary>
    /// <param name="args">The event arguments containing details about the edit operation, including created, modified, and deleted items.</param>
    /// <returns>A completed <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task OnEditCompleted(EditCompletedEventArgs args)
    {
      //show number of edits
      Console.WriteLine("Creates: " + args.Creates.ToDictionary().Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Modifies: " + args.Modifies.ToDictionary().Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Deletes: " + args.Deletes.ToDictionary().Values.Sum(list => list.Count).ToString());
      return Task.FromResult(0);
    }
    #endregion


  }
}
