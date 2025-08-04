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

namespace ProSnippetsEditing
{
  public static class ProSnippetsBlobEditing
  {
    #region ProSnippet Group: Accessing Blob Fields
    #endregion

    public static void ReadWriteBlobInspector()
    {
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.MODIFY(INSPECTOR)
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.EXECUTE
      #region Read and Write blob fields with the attribute inspector
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
      #endregion
    }

    public static void ReadWriteBlobRow()
    {
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},DATASET)
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},IENUMERABLE{DATASET})
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},DATASET[])
      #region Read and Write blob fields with a row cursor in a callback
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
      #endregion
    }

  }
}
