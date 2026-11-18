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
using System;
using System.Linq;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace ProSnippets.EditingSnippets
{
  public static partial class ProSnippetsEditing
  {
    #region ProSnippet Group: Accessing Raster Fields
    #endregion

    public static async void ReadwriteRasterFieldAsync()
    {
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.IEnumerable{System.Int64})
      #region Read from a raster field
      var readRasterMapView = MapView.Active;
      if (readRasterMapView == null)
        return;

      await QueuedTask.Run(() =>
      {
        var sel = readRasterMapView.Map.GetSelection();

        //Read a raster from a raster field as an InteropBitmap
        //the bitmap can then be used as an imagesource or written to disk
        var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
        insp.Load(sel.ToDictionary().Keys.First(), sel.ToDictionary().Values.First());
        var ibmp = insp["Photo"] as System.Windows.Interop.InteropBitmap;
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.IEnumerable{System.Int64})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute()
      // cref: ArcGIS.Desktop.Editing.EditOperation.IsEmpty
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Name
      #region Write an image to a raster field
      var writeRasterMapView = MapView.Active;
      if (writeRasterMapView == null)
        return;

      await QueuedTask.Run(() =>
      {
        var sel = writeRasterMapView.Map.GetSelection();

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

      // cref: ArcGIS.Core.Data.Raster.RasterStorageDef.SetCompressionType(ArcGIS.Core.Data.Raster.RasterCompressionType)
      // cref: ArcGIS.Core.Data.Raster.RasterStorageDef.SetCompressionQuality(System.Int32)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(Inspector)
      // cref: ArcGIS.Core.Data.Raster.RasterValue.SetRasterStorageDef(ArcGIS.Core.Data.Raster.RasterStorageDef)
      // cref: ArcGIS.Core.Data.Raster.RasterValue.SetRasterDataset(ArcGIS.Core.Data.Raster.RasterDataset)
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.IEnumerable{System.Int64})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute()
      // cref: ArcGIS.Desktop.Editing.EditOperation.IsEmpty
      // cref: ArcGIS.Desktop.Editing.EditOperation.Name
      #region Write a compressed image to a raster field
      var compressedRasterMapView = MapView.Active;
      if (compressedRasterMapView == null)
        return;

      await QueuedTask.Run(() =>
      {
        //Open the raster dataset on disk and create a compressed raster value dataset object
        var dataStore = new ArcGIS.Core.Data.FileSystemDatastore(new ArcGIS.Core.Data.FileSystemConnectionPath(new Uri(@"e:\temp"), ArcGIS.Core.Data.FileSystemDatastoreType.Raster));
        using (var fileRasterDataset = dataStore.OpenDataset<ArcGIS.Core.Data.Raster.RasterDataset>("Hydrant.jpg"))
        {
          var storageDef = new ArcGIS.Core.Data.Raster.RasterStorageDef();
          storageDef.SetCompressionType(ArcGIS.Core.Data.Raster.RasterCompressionType.JPEG);
          storageDef.SetCompressionQuality(90);

          var rv = new ArcGIS.Core.Data.Raster.RasterValue();
          rv.SetRasterDataset(fileRasterDataset);
          rv.SetRasterStorageDef(storageDef);

          var sel = compressedRasterMapView.Map.GetSelection();

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
