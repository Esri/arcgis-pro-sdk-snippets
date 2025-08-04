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

namespace ProSnippetsEditing
{
  public static class ProSnippetsRasterEditing
  {
    #region ProSnippet Group: Accessing Raster Fields
    #endregion

    public static void ReadFromRasterField()
    {
      // cref: ARCGIS.DESKTOP.EDITING.ATTRIBUTES.INSPECTOR.LOAD(MAPMEMBER,INT64)
      #region Read from a raster field
      QueuedTask.Run(() =>
      {
        var sel = MapView.Active.Map.GetSelection();

        //Read a raster from a raster field as an InteropBitmap
        //the bitmap can then be used as an imagesource or written to disk
        var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
        insp.Load(sel.ToDictionary().Keys.First(), sel.ToDictionary().Values.First());
        var ibmp = insp["Photo"] as System.Windows.Interop.InteropBitmap;
      });
      #endregion
    }

    public static void WriteImageToRasterField()
    {
      // cref: ARCGIS.DESKTOP.EDITING.ATTRIBUTES.INSPECTOR.LOAD(MAPMEMBER,INT64)
      #region Write an image to a raster field
      QueuedTask.Run(() =>
      {
        var sel = MapView.Active.Map.GetSelection();

        //Insert an image into a raster field
        //Image will be written with no compression
        var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
        insp.Load(sel.ToDictionary().Keys.First(), sel.ToDictionary().Values.First());
        insp["Photo"] = @"e:\temp\Hydrant.jpg";

        var op = new EditOperation() { Name = "Raster Inspector" };
        op.Modify(insp);
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion
    }

    public static void WriteCompImageToRasterField()
    {
      // cref: ArcGIS.Core.Data.Raster.RasterStorageDef.#ctor
      // cref: ArcGIS.Core.Data.Raster.RasterStorageDef.#ctor()
      // cref: ARCGIS.CORE.DATA.RASTER.RASTERSTORAGEDEF.SETCOMPRESSIONTYPE
      // cref: ARCGIS.CORE.DATA.RASTER.RASTERSTORAGEDEF.SETCOMPRESSIONQUALITY
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.MODIFY(INSPECTOR)
      // cref: ArcGIS.Core.Data.Raster.RasterValue.SetRasterStorageDef
      // cref: ArcGIS.Core.Data.Raster.RasterValue.SetRasterDataset
      #region Write a compressed image to a raster field
      QueuedTask.Run(() =>
      {
        //Open the raster dataset on disk and create a compressed raster value dataset object
        var dataStore = new ArcGIS.Core.Data.FileSystemDatastore(new ArcGIS.Core.Data.FileSystemConnectionPath(new System.Uri(@"e:\temp"), ArcGIS.Core.Data.FileSystemDatastoreType.Raster));
        using (var fileRasterDataset = dataStore.OpenDataset<ArcGIS.Core.Data.Raster.RasterDataset>("Hydrant.jpg"))
        {
          var storageDef = new ArcGIS.Core.Data.Raster.RasterStorageDef();
          storageDef.SetCompressionType(ArcGIS.Core.Data.Raster.RasterCompressionType.JPEG);
          storageDef.SetCompressionQuality(90);

          var rv = new ArcGIS.Core.Data.Raster.RasterValue();
          rv.SetRasterDataset(fileRasterDataset);
          rv.SetRasterStorageDef(storageDef);

          var sel = MapView.Active.Map.GetSelection();

          //insert a raster value object into the raster field
          var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
          insp.Load(sel.ToDictionary().Keys.First(), sel.ToDictionary().Values.First());
          insp["Photo"] = rv;

          var op = new EditOperation() { Name = "Raster Inspector" };
          op.Modify(insp);
          if (!op.IsEmpty)
          {
            var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          }
        }
      });
      #endregion
    }
  }
}
