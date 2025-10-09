/*

   Copyright 2022 Esri

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
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Parcels;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProSnippets.ParcelFabricSnippets
{
	/// <summary>
	/// Provides code snippets demonstrating common parcel fabric operations in ArcGIS Pro,
	/// including adding parcel layers, managing records, copying and assigning features,
	/// creating and building parcels, changing parcel types, and accessing parcel fabric datasets and topology.
	/// </summary>
	public static partial class ProSnippetsParcelFabric
  {
		#region ProSnippet Group: Parcel Fabric
		#endregion

		/// <summary>
		/// Demonstrates key parcel fabric operations in ArcGIS Pro, including adding parcel layers,
		/// managing and setting active records, copying and assigning features, creating and building parcels,
		/// changing parcel types, retrieving parcel features and topology, and working with parcel fabric datasets.
		/// </summary>
		public static async Task ProSnippetsParcelFabricAsync()
    {

      #region Variable initialization

      var activeMap = MapView.Active.Map;
      ParcelLayer myParcelFabricLayer = activeMap.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault(); ;
      FeatureLayer featLayer = activeMap.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      GeometryType geomType = GeometryType.Polygon;
      Table table = activeMap.GetStandaloneTablesAsFlattenedList().OfType<StandaloneTable>().FirstOrDefault().GetTable();

      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelLayer
      // cref: ArcGIS.Desktop.Mapping.ParcelLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.ParcelLayerCreationParams.#ctor(System.Uri)
      #region Add a Parcel Layer to the map
      string path = @"C:\MyTestData\MyFileGeodatabase.gdb\MyFeatureDS\MyFabric";
      await QueuedTask.Run(() =>
      {
        var lyrCreateParams = new ParcelLayerCreationParams(new Uri(path));
        try
        {
          var parcelLayer = LayerFactory.Instance.CreateLayer<ParcelLayer>(
            lyrCreateParams, MapView.Active.Map);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Add Parcel Fabric Layer");
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelLayer
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetActiveRecord(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Editing.ParcelRecord
      #region Get the active record
      {
        string errorMessage = await QueuedTask.Run(() =>
        {
          try
          {
            var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
            //if there is no fabric in the map then bail
            if (myParcelFabricLayer == null)
              return "There is no fabric in the map.";
            var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
            if (theActiveRecord == null)
              return "There is no Active Record. Please set the active record and try again.";
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          return "";
        });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Get Active Record.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelLayer
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.SetActiveRecordAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      #region Set the active record
      {
        string errorMessage = await QueuedTask.Run(async () =>
        {
          try
          {
            string sExistingRecord = "MyRecordName";
            var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
            //if there is no fabric in the map then bail
            if (myParcelFabricLayer == null)
              return "There is no fabric in the map.";

            bool bSuccess = await myParcelFabricLayer.SetActiveRecordAsync(sExistingRecord);
            if (!bSuccess)
              return "No record called " + sExistingRecord + " was found.";
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          return "";
        });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Set Active Record.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetRecordsLayerAsync(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.SetActiveRecordAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.Int64)
      #region Create a new record
      {
        string errorMessage = await QueuedTask.Run(async () =>
      {
        Dictionary<string, object> RecordAttributes = [];
        string sNewRecord = "MyRecordName";
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric in the map.";
          var recordsLayer = await myParcelFabricLayer.GetRecordsLayerAsync();
          var editOper = new EditOperation()
          {
            Name = "Create Parcel Fabric Record",
            ProgressMessage = "Create Parcel Fabric Record...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = false,
            SelectModifiedFeatures = false
          };
          RecordAttributes.Add("Name", sNewRecord);
          var editRowToken = editOper.Create(recordsLayer.FirstOrDefault(), RecordAttributes);
          if (!editOper.Execute())
            return editOper.ErrorMessage;

          var defOID = -1;
          var lOid = editRowToken.ObjectID ?? defOID;
          await myParcelFabricLayer.SetActiveRecordAsync(lOid);
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Create New Record.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelTypeNamesAsync(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelLineLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Editing.EditOperation.CopyLineFeaturesToParcelType(ArcGIS.Desktop.Mapping.Layer, System.Collections.Generic.IEnumerable<System.Int64>, ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Mapping.Layer)
      #region Copy standard line features into a parcel type
      {
        string errorMessage = await QueuedTask.Run(async () =>
     {
       // check for selected layer
       if (MapView.Active.GetSelectedLayers().Count == 0)
         return "Please select a target parcel polygon layer in the table of contents.";
       //get the feature layer that's selected in the table of contents
       var destPolygonL = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
       try
       {
         var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
         //if there is no fabric in the map then bail
         if (myParcelFabricLayer == null)
           return "There is no fabric in the map.";
         var pRec = myParcelFabricLayer.GetActiveRecord();
         if (pRec == null)
           return "There is no Active Record. Please set the active record and try again.";
         string ParcelTypeName = "";
         IEnumerable<string> parcelTypeNames = await myParcelFabricLayer.GetParcelTypeNamesAsync();
         foreach (string parcelTypeNm in parcelTypeNames)
         {
           var polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync(parcelTypeNm);
           foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
             if (lyr == destPolygonL)
             {
               ParcelTypeName = parcelTypeNm;
               break;
             }
         }
         if (String.IsNullOrEmpty(ParcelTypeName))
           return "Please select a target parcel polygon layer in the table of contents.";
         var srcFeatLyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(l => l.Name.Contains("MySourceLines") && l.IsVisible);
         if (srcFeatLyr == null)
           return "Looking for a layer named 'MySourceLines' in the table of contents.";
         //now get the line layer for this parcel type
         var destLineLyrEnum = await myParcelFabricLayer.GetParcelLineLayerByTypeNameAsync(ParcelTypeName);
         if (destLineLyrEnum.Count == 0) //make sure there is one in the map
           return ParcelTypeName + " not found.";
         var destLineL = destLineLyrEnum.FirstOrDefault();
         if (destLineL == null || destPolygonL == null)
           return "";
         var editOper = new EditOperation()
         {
           Name = "Copy Line Features To Parcel Type",
           ProgressMessage = "Copy Line Features To Parcel Type...",
           ShowModalMessageAfterFailure = true,
           SelectNewFeatures = true,
           SelectModifiedFeatures = false
         };
         var ids = new List<long>((srcFeatLyr as FeatureLayer).GetSelection().GetObjectIDs());
         if (ids.Count == 0)
           return "No selected lines were found. Please select line features and try again.";
         editOper.CopyLineFeaturesToParcelType(srcFeatLyr, ids, destLineL, destPolygonL);
         if (!editOper.Execute())
           return editOper.ErrorMessage;
       }
       catch (Exception ex)
       {
         return ex.Message;
       }
       return "";
     });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Copy Line Features To Parcel Type.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelLineLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Editing.EditOperation.CopyParcelLinesToParcelType(ArcGIS.Desktop.Mapping.ParcelLayer, ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Mapping.Layer, System.Boolean, System.Boolean, System.Boolean)
      #region Copy parcel lines to a parcel type
      {
        string errorMessage = await QueuedTask.Run(async () =>
       {
         // check for selected layer
         if (MapView.Active.GetSelectedLayers().Count == 0)
           return "Please select a source parcel polygon feature layer in the table of contents.";
         try
         {
           var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
           if (myParcelFabricLayer == null)
             return "No parcel layer found in the map.";

           //get the feature layer that's selected in the table of contents
           var srcParcelFeatLyr = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
           string sTargetParcelType = "Tax";
           var destLineLEnum = await myParcelFabricLayer.GetParcelLineLayerByTypeNameAsync(sTargetParcelType);
           if (destLineLEnum.Count == 0)
             return "";
           var destLineL = destLineLEnum.FirstOrDefault();
           var destPolygonLEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync(sTargetParcelType);
           if (destPolygonLEnum.Count == 0)
             return "";
           var destPolygonL = destPolygonLEnum.FirstOrDefault();
           if (destLineL == null || destPolygonL == null)
             return "";
           var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
           if (theActiveRecord == null)
             return "There is no Active Record. Please set the active record and try again.";
           var editOper = new EditOperation()
           {
             Name = "Copy Lines To Parcel Type",
             ProgressMessage = "Copy Lines To Parcel Type ...",
             ShowModalMessageAfterFailure = true,
             SelectNewFeatures = true,
             SelectModifiedFeatures = false
           };
           var ids = new List<long>(srcParcelFeatLyr.GetSelection().GetObjectIDs());
           if (ids.Count == 0)
             return "No selected parcels found. Please select parcels and try again.";
           //add the standard feature line layers source, and their feature ids to a new Dictionary
           var sourceParcelFeatures = new Dictionary<MapMember, List<long>>
           {
             { srcParcelFeatLyr, ids }
           };
           editOper.CopyParcelLinesToParcelType(myParcelFabricLayer, SelectionSet.FromDictionary(sourceParcelFeatures), destLineL, destPolygonL, true, false, true);
           if (!editOper.Execute())
             return editOper.ErrorMessage;
         }
         catch (Exception ex)
         {
           return ex.Message;
         }
         return "";
       });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Copy Parcel Lines To Parcel Type.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetActiveRecord(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Editing.EditOperation.AssignFeaturesToRecord(ArcGIS.Desktop.Mapping.ParcelLayer, ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Editing.ParcelRecord)
      #region Assign features to active record
      {
        string errorMessage = await QueuedTask.Run(() =>
       {
         //check for selected layer
         if (MapView.Active.GetSelectedLayers().Count == 0)
           return "Please select a source feature layer in the table of contents.";
         //first get the feature layer that's selected in the table of contents
         var srcFeatLyr = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
         var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
         if (myParcelFabricLayer == null)
           return "No parcel layer found in the map.";
         try
         {
           var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
           if (theActiveRecord == null)
             return "There is no Active Record. Please set the active record and try again.";
           var editOper = new EditOperation()
           {
             Name = "Assign Features to Record",
             ProgressMessage = "Assign Features to Record...",
             ShowModalMessageAfterFailure = true,
             SelectNewFeatures = true,
             SelectModifiedFeatures = false
           };
           //add parcel type layers and their feature ids to a new Dictionary
           var ids = new List<long>(srcFeatLyr.GetSelection().GetObjectIDs());
           var sourceFeatures = new Dictionary<MapMember, List<long>>
           {
             { srcFeatLyr, ids }
           };
           editOper.AssignFeaturesToRecord(myParcelFabricLayer,
             SelectionSet.FromDictionary(sourceFeatures), theActiveRecord);
           if (!editOper.Execute())
             return editOper.ErrorMessage;
         }
         catch (Exception ex)
         {
           return ex.Message;
         }
         return "";
       });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Assign Features To Record.");
      }
      #endregion

      {
        string errorMessage = await QueuedTask.Run(async () =>
       {
         // check for selected layer
         if (MapView.Active.GetSelectedLayers().Count == 0)
           return "Please select a target parcel polygon layer in the table of contents";
         //get the feature layer that's selected in the table of contents
         var destPolygonL = MapView.Active.GetSelectedLayers().FirstOrDefault() as FeatureLayer;
         var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
         if (myParcelFabricLayer == null)
           return "Parcel layer not found in map.";

         //is it a fabric parcel type layer?
         string ParcelTypeName = "";
         IEnumerable<string> parcelTypeNames = await myParcelFabricLayer.GetParcelTypeNamesAsync();
         foreach (string parcelTypeNm in parcelTypeNames)
         {
           var polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync(parcelTypeNm);
           foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
             if (lyr == destPolygonL)
             {
               ParcelTypeName = parcelTypeNm;
               break;
             }
         }
         if (String.IsNullOrEmpty(ParcelTypeName))
           return "Please select a target parcel polygon layer in the table of contents.";
         var destLineL = await myParcelFabricLayer.GetParcelLineLayerByTypeNameAsync(ParcelTypeName);
         if (destLineL == null)
           return "";

         // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetActiveRecord(ArcGIS.Desktop.Mapping.ParcelLayer)
         // cref: ArcGIS.Desktop.Editing.EditOperation.CreateParcelSeedsByRecord(ArcGIS.Desktop.Mapping.ParcelLayer, System.Guid, ArcGIS.Core.Geometry.Envelope,  System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.MapMember>)
         #region Create parcel seeds
         try
         {
           var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
           if (theActiveRecord == null)
             return "There is no Active Record. Please set the active record and try again.";

           var guid = theActiveRecord.Guid;
           var editOper = new EditOperation()
           {
             Name = "Create Parcel Seeds",
             ProgressMessage = "Create Parcel Seeds...",
             ShowModalMessageAfterFailure = true,
             SelectNewFeatures = true,
             SelectModifiedFeatures = false
           };
           editOper.CreateParcelSeedsByRecord(myParcelFabricLayer, guid, MapView.Active.Extent);
           if (!editOper.Execute())
             return editOper.ErrorMessage;
         }
         catch (Exception ex)
         {
           return ex.Message;
         }
         return "";
         #endregion
       });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Create Parcel Seeds.");
      }

      {
        string errorMessage = await QueuedTask.Run(() =>
       {
         // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetActiveRecord(ArcGIS.Desktop.Mapping.ParcelLayer)
         // cref: ArcGIS.Desktop.Editing.EditOperation.BuildParcelsByRecord(ArcGIS.Desktop.Mapping.ParcelLayer, System.Guid, System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.MapMember>)
         #region Build parcels
         try
         {
           var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
           if (myParcelFabricLayer == null)
             return "Parcel layer not found in map.";

           var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
           var guid = theActiveRecord.Guid;
           var editOper = new EditOperation()
           {
             Name = "Build Parcels",
             ProgressMessage = "Build Parcels...",
             ShowModalMessageAfterFailure = true,
             SelectNewFeatures = true,
             SelectModifiedFeatures = true
           };
           editOper.BuildParcelsByRecord(myParcelFabricLayer, guid);
           if (!editOper.Execute())
             return editOper.ErrorMessage;
         }
         catch (Exception ex)
         {
           return ex.Message;
         }
         return "";
         #endregion
       });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Build Parcels.");
      }

      {
        string errorMessage = await QueuedTask.Run(async () =>
       {
         // check for selected layer
         if (MapView.Active.GetSelectedLayers().Count == 0)
           return "Please select the source layer in the table of contents.";

         // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
         // cref: ArcGIS.Desktop.Editing.EditOperation.DuplicateParcels(ArcGIS.Desktop.Mapping.ParcelLayer, ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Editing.ParcelRecord, ArcGIS.Desktop.Mapping.Layer)
         #region Duplicate parcels
         var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
         if (myParcelFabricLayer == null)
           return "Parecl layer not found in the map.";
         //get the source polygon layer from the parcel fabric layer type, in this case a layer called Lot
         var srcFeatLyrEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync("Lot");
         if (srcFeatLyrEnum.Count == 0)
           return "";
         var sourcePolygonL = srcFeatLyrEnum.FirstOrDefault();
         //get the target polygon layer from the parcel fabric layer type, in this case a layer called Tax
         var targetFeatLyrEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync("Tax");
         if (targetFeatLyrEnum.Count == 0)
           return "";
         var targetFeatLyr = targetFeatLyrEnum.FirstOrDefault();
         var ids = new List<long>(sourcePolygonL.GetSelection().GetObjectIDs());
         if (ids.Count == 0)
           return "No selected parcels found. Please select parcels and try again.";
         //add polygon layers and the feature ids to be duplicated to a new Dictionary
         var sourceFeatures = new Dictionary<MapMember, List<long>>
         {
           { sourcePolygonL, ids }
         };
         try
         {
           var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
           if (theActiveRecord == null)
             return "There is no Active Record. Please set the active record and try again.";
           var editOper = new EditOperation()
           {
             Name = "Duplicate Parcels",
             ProgressMessage = "Duplicate Parcels...",
             ShowModalMessageAfterFailure = true,
             SelectNewFeatures = true,
             SelectModifiedFeatures = false
           };
           editOper.DuplicateParcels(myParcelFabricLayer,
             SelectionSet.FromDictionary(sourceFeatures), theActiveRecord, targetFeatLyr);
           if (!editOper.Execute())
             return editOper.ErrorMessage;
         }
         catch (Exception ex)
         {
           return ex.Message;
         }
         #endregion
         return "";
       });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Duplicate Parcels.");
      }


      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelTypeNamesAsync(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Editing.EditOperation.SetParcelHistoryRetired(ArcGIS.Desktop.Mapping.ParcelLayer, ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Editing.ParcelRecord)
      #region Set parcels historic
      {
        string errorMessage = await QueuedTask.Run(async () =>
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          if (myParcelFabricLayer == null)
            return "Please add a parcel fabric to the map";
          try
          {
            FeatureLayer destPolygonL = null;
            //find the first layer that is a parcel type, is non-historic, and has a selection
            bool bFound = false;
            var ParcelTypesEnum = await myParcelFabricLayer.GetParcelTypeNamesAsync();
            foreach (FeatureLayer mapFeatLyr in MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>())
            {
              foreach (string ParcelType in ParcelTypesEnum)
              {
                var layerEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync(ParcelType);
                foreach (FeatureLayer flyr in layerEnum)
                {
                  if (flyr == mapFeatLyr)
                  {
                    bFound = mapFeatLyr.SelectionCount > 0;
                    destPolygonL = mapFeatLyr;
                    break;
                  }
                }
                if (bFound) break;
              }
              if (bFound) break;
            }
            if (!bFound)
              return "Please select parcels to set as historic.";

            var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
            if (theActiveRecord == null)
              return "There is no Active Record. Please set the active record and try again.";

            var ids = new List<long>(destPolygonL.GetSelection().GetObjectIDs());
            //can do multi layer selection but using single per code above
            var sourceFeatures = new Dictionary<MapMember, List<long>>
            {
              { destPolygonL, ids }
            };
            var editOper = new EditOperation()
            {
              Name = "Set Parcels Historic",
              ProgressMessage = "Set Parcels Historic...",
              ShowModalMessageAfterFailure = true,
              SelectNewFeatures = true,
              SelectModifiedFeatures = false
            };
            editOper.SetParcelHistoryRetired(myParcelFabricLayer,
              SelectionSet.FromDictionary(sourceFeatures), theActiveRecord);
            if (!editOper.Execute())
              return editOper.ErrorMessage;
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          return "";
        });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Set Parcels Historic.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelTypeNamesAsync(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Editing.EditOperation.ShrinkParcelsToSeeds(ArcGIS.Desktop.Mapping.ParcelLayer, ArcGIS.Desktop.Mapping.SelectionSet)
      #region Shrink parcels to seeds
      {
        string errorMessage = await QueuedTask.Run(async () =>
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          if (myParcelFabricLayer == null)
            return "Please add a parcel fabric to the map";
          try
          {
            FeatureLayer parcelPolygonLyr = null;
            //find the first layer that is a polygon parcel type, is non-historic, and has a selection
            bool bFound = false;
            var ParcelTypesEnum = await myParcelFabricLayer.GetParcelTypeNamesAsync();
            foreach (FeatureLayer mapFeatLyr in MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>())
            {
              foreach (string ParcelType in ParcelTypesEnum)
              {
                var layerEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync(ParcelType);
                foreach (FeatureLayer flyr in layerEnum)
                {
                  if (flyr == mapFeatLyr)
                  {
                    bFound = mapFeatLyr.SelectionCount > 0;
                    parcelPolygonLyr = mapFeatLyr;
                    break;
                  }
                }
                if (bFound) break;
              }
              if (bFound) break;
            }
            if (!bFound)
              return "Please select parcels to shrink to seeds.";
            var editOper = new EditOperation()
            {
              Name = "Shrink Parcels To Seeds",
              ProgressMessage = "Shrink Parcels To Seeds...",
              ShowModalMessageAfterFailure = true,
              SelectNewFeatures = true,
              SelectModifiedFeatures = false
            };
            var ids = new List<long>(parcelPolygonLyr.GetSelection().GetObjectIDs());
            var sourceParcelFeatures = new Dictionary<MapMember, List<long>>
            {
              { parcelPolygonLyr, ids }
            };
            editOper.ShrinkParcelsToSeeds(myParcelFabricLayer, SelectionSet.FromDictionary(sourceParcelFeatures));
            if (!editOper.Execute())
              return editOper.ErrorMessage;
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          return "";
        });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Shrink Parcels To Seeds.");
      }
      #endregion

      {
        string errorMessage = await QueuedTask.Run(async () =>
       {
         //check for selected layer
         if (MapView.Active.GetSelectedLayers().Count == 0)
           return "Please select a source layer in the table of contents";
         //get the feature layer that's selected in the table of contents
         var sourcePolygonL = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
         if (sourcePolygonL == null)
           return "";
         var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
         if (myParcelFabricLayer == null)
           return "";
         var targetFeatLyrEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync("Tax");
         if (targetFeatLyrEnum.Count == 0)
           return "";
         var targetFeatLyr = targetFeatLyrEnum.FirstOrDefault(); //the target parcel polygon feature layer

         // cref: ArcGIS.Desktop.Editing.EditOperation.ChangeParcelType(ArcGIS.Desktop.Mapping.ParcelLayer, ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Mapping.Layer)
         #region Change parcel type
         //add polygon layers and the feature ids to change the type on to a new Dictionary
         var ids = new List<long>(sourcePolygonL.GetSelection().GetObjectIDs());
         var sourceFeatures = new Dictionary<MapMember, List<long>>
         {
           { sourcePolygonL, ids }
         };
         try
         {
           var editOper = new EditOperation()
           {
             Name = "Change Parcel Type",
             ProgressMessage = "Change Parcel Type...",
             ShowModalMessageAfterFailure = true,
             SelectNewFeatures = true,
             SelectModifiedFeatures = false
           };
           editOper.ChangeParcelType(myParcelFabricLayer, SelectionSet.FromDictionary(sourceFeatures), targetFeatLyr);
           if (!editOper.Execute())
             return editOper.ErrorMessage;
         }
         catch (Exception ex)
         {
           return ex.Message;
         }
         return "";
         #endregion
       });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Change Parcel Type.");
      }

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelFeaturesAsync(ArcGIS.Desktop.Mapping.ParcelLayer, ArcGIS.Desktop.Mapping.SelectionSet)
      // cref: ArcGIS.Desktop.Editing.ParcelFeatures
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelLineLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetPointsLayerAsync(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Editing.ParcelFeatures.Lines
      // cref: ArcGIS.Desktop.Editing.ParcelFeatures.Points
      #region Get parcel features
      {
        string sReportResult = "Polygon Information --" + Environment.NewLine;
        string sParcelTypeName = "tax";
        string errorMessage = await QueuedTask.Run(async () =>
        {
          var myParcelFabricLayer =
            MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric layer in the map.";

          //first get the parcel type feature layer
          var featSrcLyr = myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync(sParcelTypeName).Result.FirstOrDefault();

          if (featSrcLyr.SelectionCount == 0)
            return "There is no selection on the " + sParcelTypeName + " layer.";

          sReportResult += " Parcel Type: " + sParcelTypeName + Environment.NewLine;
          sReportResult += " Poygons: " + featSrcLyr.SelectionCount + Environment.NewLine + Environment.NewLine;

          try
          {
            // ------- get the selected parcels ---------
            var ids = new List<long>((featSrcLyr as FeatureLayer).GetSelection().GetObjectIDs());
            var sourceParcels = new Dictionary<MapMember, List<long>>
            {
              { featSrcLyr, ids }
            };
            //---------------------------------------------
            ParcelFeatures parcFeatures =
                            await myParcelFabricLayer.GetParcelFeaturesAsync(SelectionSet.FromDictionary(sourceParcels));
            //since we know that we want to report on Tax lines only, and for this functionality 
            // we can use any of the Tax line layer instances (if there happens to be more than one)
            // we can get the first instance as follows
            FeatureLayer myLineFeatureLyr =
                myParcelFabricLayer.GetParcelLineLayerByTypeNameAsync(sParcelTypeName).Result.FirstOrDefault();
            if (myLineFeatureLyr == null)
              return sParcelTypeName + " line layer not found";

            FeatureLayer myPointFeatureLyr =
                myParcelFabricLayer.GetPointsLayerAsync().Result.FirstOrDefault();
            if (myPointFeatureLyr == null)
              return "fabric point layer not found";

            var LineInfo = parcFeatures.Lines; //then get the line information from the parcel features object
                                               //... and then do some work for each of the lines
            int iRadiusAttributeCnt = 0;
            int iDistanceAttributeCnt = 0;
            sReportResult += "Line Information --";
            foreach (KeyValuePair<string, List<long>> kvp in LineInfo)
            {
              if (!kvp.Key.Equals(sParcelTypeName, StringComparison.CurrentCultureIgnoreCase))
                continue; // ignore any other lines from different parcel types

              foreach (long oid in kvp.Value)
              {
                var insp = myLineFeatureLyr.Inspect(oid);
                var dRadius = insp["RADIUS"];
                var dDistance = insp["DISTANCE"];

                if (dRadius != DBNull.Value)
                  iRadiusAttributeCnt++;
                if (dDistance != DBNull.Value)
                  iDistanceAttributeCnt++;
                //Polyline poly = (Polyline)insp["SHAPE"];
              }
              sReportResult += Environment.NewLine + " Distance attributes: " + iDistanceAttributeCnt.ToString();
              sReportResult += Environment.NewLine + " Radius attributes: " + iRadiusAttributeCnt.ToString();
            }

            var PointInfo = parcFeatures.Points; //get the point information from the parcel features object
                                                 //... and then do some work for each of the points
            sReportResult += Environment.NewLine + Environment.NewLine + "Point Information --";
            int iFixedPointCnt = 0;
            int iNonFixedPointCnt = 0;
            foreach (long oid in PointInfo)
            {
              var insp = myPointFeatureLyr.Inspect(oid);
              var isFixed = insp["ISFIXED"];
              if (isFixed == DBNull.Value || (int)isFixed == 0)
                iNonFixedPointCnt++;
              else
                iFixedPointCnt++;
              // var pt = insp["SHAPE"];

            }
            sReportResult += Environment.NewLine + " Fixed Points: " + iFixedPointCnt.ToString();
            sReportResult += Environment.NewLine + " Non-Fixed Points: " + iNonFixedPointCnt.ToString();
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          return "";
        });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Get Parcel Features");
        else
          MessageBox.Show(sReportResult, "Get Parcel Features");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelFabric(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Core.Data.Parcels.ParcelFabric
      #region Get parcel fabric dataset controller from parcel layer
      {
        string errorMessage = await QueuedTask.Run(() =>
        {
          try
          {
            var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
            //if there is no fabric in the map then bail
            if (myParcelFabricLayer == null)
              return "There is no fabric in the map.";
            var myParcelFabricDataset = myParcelFabricLayer.GetParcelFabric();
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          return "";
        });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Get Parcel Fabric Dataset from layer.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelFabric(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Core.Data.Parcels.ParcelFabric
      // cref: ArcGIS.Core.Data.Parcels.ParcelFabric.GetParcelTopology()
      #region Get parcel topology of parcel fabric dataset
      {
        string errorMessage = await QueuedTask.Run(() =>
        {
          try
          {
            var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
            //if there is no fabric in the map then bail
            if (myParcelFabricLayer == null)
              return "There is no fabric in the map.";
            var myParcelFabricDataset = myParcelFabricLayer.GetParcelFabric();
            var myTopology = myParcelFabricDataset.GetParcelTopology();
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          return "";
        });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Get Parcel Fabric Topology.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelFabric(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Core.Data.Parcels.ParcelFabric.GetSystemTable(ArcGIS.Core.Data.Parcels.SystemTableType)
      // cref: ArcGIS.Core.Data.Parcels.SystemTableType
      #region Get point, connection, and record feature classes from the parcel fabric dataset
      {
        string errorMessage = await QueuedTask.Run(() =>
      {
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric in the map.";
          var myParcelFabricDataset = myParcelFabricLayer.GetParcelFabric();
          FeatureClass myPointsFC = myParcelFabricDataset.GetSystemTable(SystemTableType.Points) as FeatureClass;
          FeatureClass myCoonectionsFC = myParcelFabricDataset.GetSystemTable(SystemTableType.Connections) as FeatureClass;
          FeatureClass myRecordsFC = myParcelFabricDataset.GetSystemTable(SystemTableType.Records) as FeatureClass;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Get point, connection, and record feature classes.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelFabric(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Core.Data.Parcels.ParcelFabric.GetParcelTypeInfo()
      // cref: ArcGIS.Core.Data.Parcels.ParcelTypeInfo
      // cref: ArcGIS.Core.Data.Parcels.ParcelTypeInfo.LineFeatureTable
      // cref: ArcGIS.Core.Data.Parcels.ParcelTypeInfo.PolygonFeatureTable
      #region Get parcel type feature classes from the parcel fabric dataset
      {
        string errorMessage = await QueuedTask.Run(() =>
      {
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric in the map.";
          string myParcelTypeName = "Tax";
          var myParcelFabricDataset = myParcelFabricLayer.GetParcelFabric();
          var typeInfo = myParcelFabricDataset.GetParcelTypeInfo();
          FeatureClass lineFCType = null;
          FeatureClass polyFCType = null;
          foreach (var info in typeInfo)
          {
            if (info.Name.Equals(myParcelTypeName, StringComparison.CurrentCultureIgnoreCase))
            {
              lineFCType = info.LineFeatureTable as FeatureClass;
              polyFCType = info.PolygonFeatureTable as FeatureClass;
              break;
            }
          }
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
        if (!string.IsNullOrEmpty(errorMessage))
          MessageBox.Show(errorMessage, "Get Parcel Type feature classes.");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelTypeNamesAsync(ArcGIS.Desktop.Mapping.ParcelLayer)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, String.String)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetHistoricParcelPolygonLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetParcelLineLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricExtensions.GetHistoricParcelLineLayerByTypeNameAsync(ArcGIS.Desktop.Mapping.ParcelLayer, System.String)
      #region Get parcel type name from feature layer
      {
        var theparcelTypeName = await QueuedTask.Run(async () =>
        {
          IEnumerable<string> parcelTypeNames = await myParcelFabricLayer.GetParcelTypeNamesAsync();
          foreach (string parcelTypeName in parcelTypeNames)
          {
            if (geomType == GeometryType.Polygon)
            {
              var polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeNameAsync(parcelTypeName);
              foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
                if (lyr == featLayer)
                  return parcelTypeName;

              polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetHistoricParcelPolygonLayerByTypeNameAsync(parcelTypeName);
              foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
                if (lyr == featLayer)
                  return parcelTypeName;
            }
            if (geomType == GeometryType.Polyline)
            {
              var lineLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelLineLayerByTypeNameAsync(parcelTypeName);
              foreach (FeatureLayer lyr in lineLyrParcelTypeEnum)
                if (lyr == featLayer)
                  return parcelTypeName;

              lineLyrParcelTypeEnum = await myParcelFabricLayer.GetHistoricParcelLineLayerByTypeNameAsync(parcelTypeName);
              foreach (FeatureLayer lyr in lineLyrParcelTypeEnum)
                if (lyr == featLayer)
                  return parcelTypeName;
            }
          }
          return String.Empty;
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Table.IsControllerDatasetSupported()
      // cref: ArcGIS.Core.Data.Table.GetControllerDatasets()
      // cref: ArcGIS.Core.Data.Parcels.ParcelFabric
      #region Get parcel fabric from table
      {
        var parcelFabricDataset = await QueuedTask.Run(() =>
        {
          ParcelFabric myParcelFabricDataset = null;
          if (table.IsControllerDatasetSupported())
          {
            // Tables can belong to multiple controller datasets, but at most one of them will be a parcel fabric

            IReadOnlyList<Dataset> controllerDatasets = table.GetControllerDatasets();
            foreach (Dataset controllerDataset in controllerDatasets)
            {
              if (controllerDataset is ParcelFabric)
              {
                myParcelFabricDataset = controllerDataset as ParcelFabric;
              }
              else
              {
                controllerDataset.Dispose();
              }
            }
          }
          return myParcelFabricDataset;
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.IsControlledByParcelFabricAsync(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Mapping.ParcelFabricType)
      // cref: ArcGIS.Desktop.Mapping.ParcelFabricType
      #region Check if layer is controlled by parcel fabric
      {
        var layer = activeMap.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(l => l.Name == "Records");
        bool isProFabric = await layer.IsControlledByParcelFabricAsync(ParcelFabricType.ParcelFabric);
        bool isArcMapFabric = await layer.IsControlledByParcelFabricAsync(ParcelFabricType.ParcelFabricForArcMap);
      }
      #endregion
    }
  }
}
