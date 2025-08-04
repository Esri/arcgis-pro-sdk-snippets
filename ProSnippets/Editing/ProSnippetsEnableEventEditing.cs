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

namespace ProSnippetsEditing
{
  public static class ProSnippetsEnableEventEditing
  {

    #region ProSnippet Group: Enable Editing
    #endregion

    private static void CanWeEdit()
    {
      // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
      // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
      #region Enable Editing

      // if not editing
      if (!Project.Current.IsEditingEnabled)
      {
        var res = MessageBox.Show("You must enable editing to use editing tools. Would you like to enable editing?",
                                                              "Enable Editing?", System.Windows.MessageBoxButton.YesNoCancel);
        if (res == System.Windows.MessageBoxResult.No ||
                      res == System.Windows.MessageBoxResult.Cancel)
        {
          return;
        }
        Project.Current.SetIsEditingEnabledAsync(true);
      }

      #endregion

      // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
      // cref: ArcGIs.Desktop.Core.Project.HasEdits
      // cref: ArcGIs.Desktop.Core.Project.DiscardEditsAsync
      // cref: ArcGIs.Desktop.Core.Project.SaveEditsAsync
      // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
      #region Disable Editing

      // if editing
      if (Project.Current.IsEditingEnabled)
      {
        var res = MessageBox.Show("Do you want to disable editing? Editing tools will be disabled",
                                                               "Disable Editing?", System.Windows.MessageBoxButton.YesNoCancel);
        if (res == System.Windows.MessageBoxResult.No ||
                      res == System.Windows.MessageBoxResult.Cancel)
        {
          return;
        }

        //we must check for edits
        if (Project.Current.HasEdits)
        {
          res = MessageBox.Show("Save edits?", "Save Edits?", System.Windows.MessageBoxButton.YesNoCancel);
          if (res == System.Windows.MessageBoxResult.Cancel)
            return;
          else if (res == System.Windows.MessageBoxResult.No)
            Project.Current.DiscardEditsAsync();
          else
          {
            Project.Current.SaveEditsAsync();
          }
        }
        Project.Current.SetIsEditingEnabledAsync(false);
      }

      #endregion

    }


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
    public static void SubscribeRowEvent()
    {
      QueuedTask.Run(() =>
      {
        //Listen for row events on a layer
        var featLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
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

    private static Guid _lastEdit = Guid.Empty;

    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
    #region Create a record in a separate table in the Map within Row Events

    // Use the EditOperation in the RowChangedEventArgs to append actions to be executed. 
    //  Your actions will become part of the operation and combined into one item on the undo stack

    public static void HookRowCreatedEvent()
    {
      // subscribe to the RowCreatedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(MyRowCreatedEvent, table);
    }

    public static void MyRowCreatedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      // get the edit operation
      var parentEditOp = args.Operation;

      // set up some attributes
      var attribs = new Dictionary<string, object> { };
      attribs.Add("Layer", "Parcels");
      attribs.Add("Description", "OID: " + args.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString());

      //create a record in an audit table
      var sTable = MapView.Active.Map.FindStandaloneTables("EditHistory").First();
      var table = sTable.GetTable();
      parentEditOp.Create(table, attribs);
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
    #region Create a record in a separate table within Row Events

    // Use the EditOperation in the RowChangedEventArgs to append actions to be executed. 
    //  Your actions will become part of the operation and combined into one item on the undo stack

    public static void HookCreatedEvent()
    {
      // subscribe to the RowCreatedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(OnRowCreatedEvent, table);
    }

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
        var attribs = new Dictionary<string, object> { };
        attribs.Add("Description", "OID: " + args.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString());

        // update Notes table with information about the new feature
        using (var geoDatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(Project.Current.DefaultGeodatabasePath))))
        {
          using (var table = geoDatabase.OpenDataset<Table>("Notes"))
          {
            parentEditOp.Create(table, attribs);
          }
        }
      }
      catch (Exception e)
      {
        MessageBox.Show($@"Error in OnRowCreated for OID: {args.Row.GetObjectID()} : {e.ToString()}");
      }
    }
    #endregion


    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Row
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Guid
    // cref: ArcGIS.Core.Data.Row.HasValueChanged(System.Int32)
    // cref: ArcGIS.Core.Data.Row.Store
    #region Modify a record within Row Events - using Row.Store

    public static void HookRowChangedEvent()
    {
      // subscribe to the RowChangedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(OnRowChangedEvent, table);
    }

    public static Guid _currentRowChangedGuid = Guid.Empty;
    public static void OnRowChangedEvent(RowChangedEventArgs args)
    {
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
    public static void HookChangedEvent()
    {
      // subscribe to the RowChangedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(MyRowChangedEvent, table);
    }

    public static void MyRowChangedEvent(RowChangedEventArgs args)
    {
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
    public static FeatureLayer featureLayer;
    public static void DetermineGeometryChange()
    {
      featureLayer = MapView.Active?.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      if (featureLayer == null)
        return;

      QueuedTask.Run(() =>
      {
        //Listen to the RowChangedEvent that occurs when a Row is changed.
        ArcGIS.Desktop.Editing.Events.RowChangedEvent.Subscribe(OnRowChangedEvent2, featureLayer.GetTable());
      });
    }
    public static void OnRowChangedEvent2(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      //Get the layer's definition
      var lyrDefn = featureLayer.GetFeatureClass().GetDefinition();
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit(System.String, System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit()
    #region Cancel a delete
    public static void StopADelete()
    {
      // subscribe to the RowDeletedEvent for the appropriate table
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowDeletedEvent.Subscribe(OnRowDeletedEvent, table);
    }

    public static Guid _currentRowDeletedGuid = Guid.Empty;
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

    public static void SubEditEvents()
    {
      //subscribe to editcompleted
      var eceToken = EditCompletedEvent.Subscribe(OnEce);
    }

    public static Task OnEce(EditCompletedEventArgs args)
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
