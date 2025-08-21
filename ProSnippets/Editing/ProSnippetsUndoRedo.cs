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
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public static class ProSnippetsUndoRedo
  {
    #region ProSnippet Group: Undo / Redo
    #endregion

    // cref: ArcGIS.Desktop.Framework.OperationManager
    // cref: ArcGIS.Desktop.Framework.OperationManager.CanUndo
    // cref: ArcGIS.Desktop.Framework.OperationManager.UndoAsync()
    #region Undo/Redo Edit Operations
    /// <summary>
    /// Executes an edit operation and demonstrates how to undo the operation programmatically.
    /// </summary>
    public static void UndoRedoEditOperations()
    {
      _ = QueuedTask.Run(() =>
      {
        var editOp = new EditOperation
        {
          Name = "My Name"
        };
        if (!editOp.IsEmpty)
        {
          //Execute and ExecuteAsync will return true if the operation was successful and false if not
          var result = editOp.Execute();
          if (result == true)
          {
            // If the operation was successful, you can undo it
            editOp.UndoAsync();
          }
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.OperationManager
    // cref: ArcGIS.Desktop.Framework.OperationManager.CanUndo
    // cref: ArcGIS.Desktop.Framework.OperationManager.UndoAsync()
    // cref: ArcGIS.Desktop.Framework.OperationManager.CanRedo
    // cref: ArcGIS.Desktop.Framework.OperationManager.RedoAsync()
    // cref: ARCGIS.DESKTOP.MAPPING.MAP.OPERATIONMANAGER
    #region Undo/Redo the Most Recent Operation
    /// <summary>
    /// Demonstrates how to undo and redo the most recent operation using the map's operation manager.
    /// </summary>
    public static void UndoRedoMostRecentOperation()
    {
      //undo
      if (MapView.Active.Map.OperationManager.CanUndo)
        MapView.Active.Map.OperationManager.UndoAsync();//await as needed

      //redo
      if (MapView.Active.Map.OperationManager.CanRedo)
        MapView.Active.Map.OperationManager.RedoAsync();//await as needed
    }
    #endregion
  }

}
