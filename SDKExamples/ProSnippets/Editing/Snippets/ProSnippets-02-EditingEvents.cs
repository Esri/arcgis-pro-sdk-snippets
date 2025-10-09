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
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProSnippets.EditingSnippets
{
  public static partial class ProSnippetsEditing
  {
    #region ProSnippet Group: Enable Editing
    #endregion

    /// <summary>
    /// Enables or disables editing for the current ArcGIS Pro project, handling unsaved edits as needed.
    /// Also demonstrates subscribing to row and edit events to monitor and respond to feature and table changes.
    /// </summary>
    public static async void ProSnippetsEnableDisableEditing()
    {
      #region Variable initialization

      Guid _currentRowDeletedGuid = Guid.Empty;

      #endregion

      // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
      // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
      #region Enable Editing
      // Enables editing for the current project asynchronously.
      if (!Project.Current.IsEditingEnabled)
      {
        var bEditingIsEnabled = await Project.Current.SetIsEditingEnabledAsync(true);
      }
      #endregion

      // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
      // cref: ArcGIs.Desktop.Core.Project.HasEdits
      // cref: ArcGIs.Desktop.Core.Project.DiscardEditsAsync
      // cref: ArcGIs.Desktop.Core.Project.SaveEditsAsync
      // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
      #region Disable Editing
      // Disables editing for the current project.
      // discard any edits if they exist
      var discardEdits = true;
      // only needed if editing is enabled
      if (Project.Current.IsEditingEnabled)
      {
        // check for edits
        if (Project.Current.HasEdits)
        {
          if (discardEdits)
            await Project.Current.DiscardEditsAsync();
          else
            await Project.Current.SaveEditsAsync();
        }
        var bEditingIsEnabled = !Project.Current.SetIsEditingEnabledAsync(false).Result;
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
      // Subscribes to row events for the currently selected feature layer in the active map view.
      await QueuedTask.Run(() =>
      {
        //Listen for row events on a layer
        var featLayer = MapView.Active.GetSelectedLayers()[0] as FeatureLayer;
        var layerTable = featLayer.GetTable();

        //subscribe to row events
        // row created event
        var rowCreateToken = RowCreatedEvent.Subscribe(rowChangedEventArgs => { }, layerTable);
        // row changed event
        var rowChangeToken = RowChangedEvent.Subscribe(rowChangedEventArgs => { }, layerTable);
        // row deleted event
        var rowDeleteToken = RowDeletedEvent.Subscribe(rowChangedEventArgs => { }, layerTable);
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
      #region Create a record in a separate table in the Map within Row Events
      // attach an event handler to receive notifications when a new row is created in the
      // associated table of the first feature layer found in the active map. Use this method to enable custom logic in
      // response to row creation events within your ArcGIS Pro add-in.
      // subscribe to the RowCreatedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(rowChangedEventArgs =>
        {
          // RowEvent callbacks are always called on the QueuedTask so there is no need 
          // to wrap your code within a QueuedTask.Run lambda.

          // get the edit operation
          var parentEditOp = rowChangedEventArgs.Operation;

          // set up some attributes
          var attribs = new Dictionary<string, object>
          {
            { "Layer", "Parcels" },
            { "Description", "objectId: " + rowChangedEventArgs.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString() }
          };

          //create a record in an audit table
          var sTable = MapView.Active.Map.FindStandaloneTables("EditHistory")[0];
          var table = sTable.GetTable();
          parentEditOp.Create(table, attribs);
        }, table);

      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
      #region Create a record in a separate table within Row Events
      // Attach an event handler to the <see cref="ArcGIS.Desktop.Editing.Events.RowCreatedEvent"/> for the table associated with the first <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> in the active map. Use this method to monitor when new rows are created
      // in that table, enabling custom logic to be executed in response to record creation events.
      Table firstTable = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(rowChangedEventArgs =>
      {
        // RowEvent callbacks are always called on the QueuedTask so there is no need 
        // to wrap your code within a QueuedTask.Run lambda.
        // update a separate table not in the map when a row is created
        // You MUST use the ArcGIS.Core.Data API to edit the table. Do NOT
        // use a new edit operation in the RowEvent callbacks
        try
        {
          // get the edit operation
          var parentEditOp = rowChangedEventArgs.Operation;
          // set up some attributes
          var attribs = new Dictionary<string, object>
            {
              { "Description", "objectId: " + rowChangedEventArgs.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString() }
            };
          // update Notes table with information about the new feature
          using var geoDatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(Project.Current.DefaultGeodatabasePath)));
          using var table = geoDatabase.OpenDataset<Table>("Notes");
          parentEditOp.Create(table, attribs);
        }
        catch (Exception e)
        {
          throw new Exception($@"Error in OnRowCreated for objectId: {rowChangedEventArgs.Row.GetObjectID()} : {e}");
        }
      }, firstTable);
      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Row
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Guid
      // cref: ArcGIS.Core.Data.Row.HasValueChanged(System.Int32)
      // cref: ArcGIS.Core.Data.Row.Store
      #region Modify a record within Row Events - using Row.Store
      // Attach an event handler to receive notifications when a row in the selected
      // table is changed. The subscription targets the first <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> found in
      // the active map. Use this method to monitor and respond to edits made to table records within the current map
      // context.
      Table thisTable = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(rowChangedEventArgs =>
        {
          // set the current row changed guid when you execute the Row.Store method
          // used for re-entry checking
          Guid _currentRowChangedGuid = new();
          // RowEvent callbacks are always called on the QueuedTask so there is no need 
          // to wrap your code within a QueuedTask.Run lambda.

          var row = rowChangedEventArgs.Row;

          // check for re-entry  (only if row.Store is called)
          if (_currentRowChangedGuid == rowChangedEventArgs.Guid)
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
                rowChangedEventArgs.CancelEdit($"Police district {row["POLICE_DISTRICT"]} is invalid");
              }
            }
            // update the description field
            row["Description"] = "Row Changed";
            //  this update with cause another OnRowChanged event to occur
            //  keep track of the row guid to avoid recursion
            _currentRowChangedGuid = rowChangedEventArgs.Guid;
            row.Store();
            _currentRowChangedGuid = Guid.Empty;
          }
        }, thisTable);

      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Core.Data.Row, System.String, System.Object)
      #region Modify a record within Row Events - using EditOperation.Modify
      // Subscribe to the <see cref="ArcGIS.Desktop.Editing.Events.RowChangedEvent"/> for
      // the first  <see cref="ArcGIS.Desktop.Mapping.FeatureLayer"/> in the active map. The event handler can be used to
      // respond to changes in rows within the table, such as modifications or updates.
      Table changeTable = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(rowChangedEventArgs =>
        {
          // set the last row changed guid when you execute the Row.Store method
          // used for re-entry checking
          Guid _lastEdit = new();

          // RowEvent callbacks are always called on the QueuedTask so there is no need 
          // to wrap your code within a QueuedTask.Run lambda.

          //example of modifying a field on a row that has been created
          var parentEditOp = rowChangedEventArgs.Operation;

          // avoid recursion
          if (_lastEdit != rowChangedEventArgs.Guid)
          {
            //update field on change
            parentEditOp.Modify(rowChangedEventArgs.Row, "ZONING", "New");

            _lastEdit = rowChangedEventArgs.Guid;
          }
        }, changeTable);

      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Row
      // cref: ArcGIS.Core.Data.Row.GetOriginalvalue
      #region Determine if Geometry Changed while editing
      // Set up a listener for row change events on the table associated with the
      // provided feature layer. The event subscription occurs within a queued task to ensure thread safety.
      await QueuedTask.Run(() =>
       {
         //Listen to the RowChangedEvent that occurs when a Row is changed.
         var modifiedFeatureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(); ;
         RowChangedEvent.Subscribe(rowChangedEventArgs =>
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
             var geomOrig = rowChangedEventArgs.Row.GetOriginalValue(shapeIndex) as Geometry;
             //New geometry of the modified row
             var geomNew = rowChangedEventArgs.Row[shapeIndex] as Geometry;
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
           }, modifiedFeatureLayer.GetTable());
       });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit(System.String, System.Boolean)
      // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit()
      #region Cancel a delete
      // Subscribe to the <see cref="ArcGIS.Desktop.Editing.Events.RowDeletedEvent"/> for the first feature layer's table in the active map.  The event handler can be used to intercept and handle row deletion events, such as canceling a delete operation if certain conditions are met.
      // subscribe to the RowDeletedEvent for the appropriate table
      // in preparation for the event: public static Guid _currentRowDeletedGuid = Guid.Empty;
      Table cancelDeleteTable = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowDeletedEvent.Subscribe(rowChangedEventArgs =>
        {
          // RowEvent callbacks are always called on the QueuedTask so there is no need 
          // to wrap your code within a QueuedTask.Run lambda.
          var row = rowChangedEventArgs.Row;
          // check for re-entry 
          if (_currentRowDeletedGuid == rowChangedEventArgs.Guid)
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
              rowChangedEventArgs.CancelEdit("Delete Event\nAre you sure", true);
              // or cancel without a dialog
              // args.CancelEdit();
            }
          }
          _currentRowDeletedGuid = rowChangedEventArgs.Guid;
        }, cancelDeleteTable);

      #endregion

      #region ProSnippet Group: EditCompletedEvent
      #endregion

      // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEvent
      // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs
      // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Creates
      // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Modifies
      // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Deletes
      #region Subscribe to EditCompletedEvent
      // Subscribe to the <see cref="ArcGIS.Desktop.Editing.Events.EditCompletedEvent"/>,
      // allowing the application to respond to edit operations such as feature creation, modification, or deletion.public static void SubEditEvents()
      var eceToken = EditCompletedEvent.Subscribe(editCompletedEventArgs =>
        {
          //show number of edits
          Console.WriteLine("Creates: " + editCompletedEventArgs.Creates.ToDictionary().Values.Sum(list => list.Count).ToString());
          Console.WriteLine("Modifies: " + editCompletedEventArgs.Modifies.ToDictionary().Values.Sum(list => list.Count).ToString());
          Console.WriteLine("Deletes: " + editCompletedEventArgs.Deletes.ToDictionary().Values.Sum(list => list.Count).ToString());
          return Task.FromResult(0);
        });

      #endregion

    }
  }
}
