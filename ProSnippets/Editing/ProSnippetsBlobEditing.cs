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
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public static class ProSnippetsBlobEditing
  {
    #region ProSnippet Group: Accessing Blob Fields
    #endregion
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector
    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.MODIFY(INSPECTOR)
    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.EXECUTE
    #region Read and Write blob fields with the attribute inspector
    /// <summary>
    /// Reads and writes binary large object (BLOB) data to and from a feature's attribute field using an <see
    /// cref="Inspector"/>.
    /// </summary>
    /// <remarks>This method demonstrates how to use the <see cref="Inspector"/> class to interact with BLOB
    /// fields in a feature's attributes. It reads a BLOB field from a selected feature, saves it to a file, reads a new
    /// file into memory, and writes the new data back to the BLOB field.</remarks>
    public static void ReadWriteBlobInspector()
    {
      QueuedTask.Run(() =>
      {
        //get selected feature into inspector
        var selectedFeatures = MapView.Active.Map.GetSelection();

        var insp = new Inspector();
        insp.Load(selectedFeatures.ToDictionary().Keys.First(), selectedFeatures.ToDictionary().Values.First());

        //read a blob field and save to a file
        var msw = new MemoryStream();
        msw = insp["Blobfield"] as MemoryStream;
        using (FileStream file = new FileStream(@"d:\temp\blob.jpg", FileMode.Create, FileAccess.Write))
        {
          msw.WriteTo(file);
        }

        //read file into memory stream
        var msr = new MemoryStream();
        using (FileStream file = new FileStream(@"d:\images\Hydrant.jpg", FileMode.Open, FileAccess.Read))
        {
          file.CopyTo(msr);
        }

        //put the memory stream in the blob field and save to feature
        var op = new EditOperation() { Name = "Blob Inspector" };
        insp["Blobfield"] = msr;
        op.Modify(insp);
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
    }
    #endregion

    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},DATASET)
    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},IENUMERABLE{DATASET})
    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},DATASET[])
    #region Read and Write blob fields with a row cursor in a callback
    /// <summary>
    /// Reads and writes blob data for each row in a feature layer using a row cursor within an edit operation.
    /// </summary>
    /// <remarks>This method demonstrates how to read a blob field from a feature layer, save it to a file,
    /// read a file into a memory stream, and update the blob field with the new data. The operation is performed within
    /// an edit operation callback to ensure transactional consistency. <para> The method assumes the existence of a
    /// feature layer named "Hydrant" in the active map and that the layer contains a blob field named "BlobField". The
    /// blob data is read from the field, written to a file, and then replaced with data read from another file. </para>
    /// <para> This method uses <see cref="QueuedTask.Run"/> to ensure that the operation is executed on the appropriate
    /// thread for ArcGIS Pro's geodatabase operations. </para></remarks>
    public static void ReadWriteBlobRow()
    {
      QueuedTask.Run(() =>
      {
        var editOp = new EditOperation() { Name = "Blob Cursor" };
        var featLayer = MapView.Active.Map.FindLayers("Hydrant").First() as FeatureLayer;

        editOp.Callback((context) =>
        {
          using (var rc = featLayer.GetTable().Search(null, false))
          {
            while (rc.MoveNext())
            {
              using (var record = rc.Current)
              {
                //read the blob field and save to a file
                var msw = new MemoryStream();
                msw = record["BlobField"] as MemoryStream;
                using (FileStream file = new FileStream(@"d:\temp\blob.jpg", FileMode.Create, FileAccess.Write))
                {
                  msw.WriteTo(file);
                }
                //read file into memory stream
                var msr = new MemoryStream();
                using (FileStream file = new FileStream(@"d:\images\Hydrant.jpg", FileMode.Open, FileAccess.Read))
                {
                  file.CopyTo(msr);
                }
                //put the memory stream in the blob field and save to feature
                record["BlobField"] = msr;
                record.Store();
              }
            }
          }
        }, featLayer.GetTable());
        if (!editOp.IsEmpty)
        {
          var result = editOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
    }
    #endregion

  }
}
