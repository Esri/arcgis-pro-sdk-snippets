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
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Editing.Templates;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProSnippetsEditing
{
  /// <summary>
  /// Provides methods for creating features in an ArcGIS Pro environment using various input sources and templates.
  /// </summary>
  /// <remarks>This class includes static methods to create features from geometries, modified inspectors, or
  /// external data sources such as CSV files. These methods leverage the ArcGIS Pro SDK's editing framework to perform
  /// feature creation operations.</remarks>
  public static class ProSnippetsCreateFeatureOperations
  {
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate, ArcGIS.Core.Geometry.Geometry)
    #region Create a feature using the current template
    /// <summary>
    /// Creates a feature in ArcGIS Pro using the current template.
    /// </summary>
    /// <param name="geometry">The geometry of the feature.</param>
    public static void CreateFeatureFromTemplate(Geometry geometry)
    {
      var myTemplate = ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current;

      //Create edit operation and execute
      var op = new ArcGIS.Desktop.Editing.EditOperation() { Name = "Create my feature" };
      op.Create(myTemplate, geometry);
      if (!op.IsEmpty)
      {
        var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
    #region Create feature from a modified inspector
    /// <summary>
    /// Creates a feature in ArcGIS Pro from a modified inspector.
    /// </summary>
    /// <param name="layer">The feature layer to create the feature in.</param>
    /// <param name="geometry">The geometry of the feature.</param>
    /// <param name="templateName">The name of the template to use.</param>
    public static void CreateFeatureFromModifiedInspector(FeatureLayer layer, Geometry geometry, string templateName)
    {

      var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      insp.Load(layer, 86);

      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // modify attributes if necessary
        // insp["Field1"] = newValue;

        //Create new feature from an existing inspector (copying the feature)
        var createOp = new EditOperation() { Name = "Create from inspector" };
        createOp.Create(insp.MapMember, insp.ToDictionary(a => a.FieldName, a => a.CurrentValue));

        if (!createOp.IsEmpty)
        {
          var result = createOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
    #region Create features from a CSV file
    /// <summary>
    /// Creates features in ArcGIS Pro from a CSV file.
    /// </summary>
    /// <remarks>This method reads CSV data and creates features in the specified feature layer.</remarks>
    public static void CreateFeaturesFromCSV(FeatureLayer layer, List<CSVData> csvData)
    {
      //Run on MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        //Create the edit operation
        var createOperation = new ArcGIS.Desktop.Editing.EditOperation() { Name = "Generate points", SelectNewFeatures = false };

        // determine the shape field name - it may not be 'Shape' 
        string shapeField = layer.GetFeatureClass().GetDefinition().GetShapeField();

        //Loop through csv data
        foreach (var item in csvData)
        {

          //Create the point geometry
          ArcGIS.Core.Geometry.MapPoint newMapPoint =
              ArcGIS.Core.Geometry.MapPointBuilderEx.CreateMapPoint(item.X, item.Y);

          // include the attributes via a dictionary
          var atts = new Dictionary<string, object>();
          atts.Add("StopOrder", item.StopOrder);
          atts.Add("FacilityID", item.FacilityID);
          atts.Add(shapeField, newMapPoint);

          // queue feature creation
          createOperation.Create(layer, atts);
        }

        // execute the edit (feature creation) operation
        if (createOperation.IsEmpty)
        {
          return createOperation.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
        else
          return false;
      });
    }
    #endregion
  }

  /// <summary>
  /// Provides operations for moving features within a feature layer in an ArcGIS map view.
  /// </summary>
  /// <remarks>This class includes methods to move selected features by a specified offset or to a specific
  /// coordinate. It operates on the first feature layer in the active map view and requires that features be selected
  /// for the operations to take effect. If no features are selected, the operations will not be performed.</remarks>
  public static class ProSnippetsEditOperations
  {
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate)
    #region Create row in a table using a table template
    /// <summary>
    /// Creates a new row in a standalone table using the specified table template.
    /// </summary>
    /// <remarks>This method uses the first available template from the standalone table's templates to create
    /// the new row.  Ensure that the standalone table has at least one template defined before calling this
    /// method.</remarks>
    /// <param name="standaloneTable">The standalone table in which the new row will be created. Cannot be null.</param>
    public static void CreateRowInTableUsingTemplate(StandaloneTable standaloneTable)
    {
      var tableTemplate = standaloneTable.GetTemplates().FirstOrDefault();
      var createRow = new EditOperation() { Name = "Create a row in a table" };
      //Creating a new row in a standalone table using the table template of your choice
      createRow.Create(tableTemplate);

      if (!createRow.IsEmpty)
      {
        var result = createRow.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Clip(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Editing.ClipMode)
    #region Clip Features
    /// <summary>
    /// Clips a feature to the specified polygon.
    /// </summary>
    /// <param name="featureLayer">The feature layer containing the feature to clip.</param>
    /// <param name="oid">The object ID of the feature to clip.</param>
    /// <param name="clipPoly">The polygon to use as the clipping boundary.</param>
    public static void ClipFeatures(FeatureLayer featureLayer, long objectId, Geometry clipPolygon)
    {
      var clipFeatures = new EditOperation() { Name = "Clip Features" };
      clipFeatures.Clip(featureLayer, objectId, clipPolygon, ClipMode.PreserveArea);
      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!clipFeatures.IsEmpty)
      {
        var result = clipFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
                                             //or use async flavor
                                             //await clipFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
    #region Cut Features
    /// <summary>
    /// Cuts features in the specified feature layer using the provided cut line and clip polygon.
    /// </summary>
    /// <param name="featureLayer"></param>
    /// <param name="objectId"></param>
    /// <param name="cutLine"></param>
    /// <param name="clipPolygon"></param>
    public static void CutFeatures(SelectionSet polygon, FeatureLayer featureLayer, long objectId, Geometry cutLine, Polygon clipPolygon)
    {
      var select = MapView.Active.SelectFeatures(clipPolygon);

      var cutFeatures = new EditOperation() { Name = "Cut Features" };
      cutFeatures.Split(featureLayer, objectId, cutLine);

      //Cut all the selected features in the active view
      //Select using a polygon (for example)
      cutFeatures.Split(polygon, cutLine);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!cutFeatures.IsEmpty)
      {
        var result = cutFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await cutFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    #region Delete Features by ObjectID
    /// <summary>
    /// Deletes a single feature from the specified feature layer using its ObjectID.
    /// </summary>
    /// <remarks>This method performs a delete operation on a single feature identified by its ObjectID. Ensure that
    /// the provided <paramref name="featureLayer"/> is valid and that the ObjectID exists within the layer.</remarks>
    /// <param name="featureLayer">The feature layer containing the feature to be deleted. Must not be <see langword="null"/>.</param>
    /// <param name="objectId">The ObjectID of the feature to delete.</param>
    public static void DeleteFeatureByObjectID(FeatureLayer featureLayer, long objectId)
    {
      var deleteFeatures = new EditOperation() { Name = "Delete single feature" };
      var table = MapView.Active.Map.StandaloneTables[0];
      //Delete a row in a standalone table
      deleteFeatures.Delete(table, objectId);
      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!deleteFeatures.IsEmpty)
      {
        var result = deleteFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await deleteFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.SelectionSet)
    #region Delete Features by SelectionSet
    /// <summary>
    /// Deletes features from the specified feature layer that intersect with the given polygon.
    /// </summary>
    /// <remarks>This method performs a selection operation using the provided polygon and deletes the
    /// selected features. The operation must be executed within a <see
    /// cref="ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run"/> context.</remarks>
    /// <param name="featureLayer">The feature layer from which features will be deleted.</param>
    /// <param name="polygon">The polygon used to select features for deletion. Only features that intersect with this polygon will be
    /// deleted.</param>
    public static void DeleteFeaturesBySelection(FeatureLayer featureLayer, Polygon polygon)
    {
      var deleteFeatures = new EditOperation() { Name = "Delete selected features" };
      //Delete all the selected features in the active view
      //Select using a polygon (for example)
      //deleteFeatures.Delete(selection);
      var selection = MapView.Active.SelectFeatures(polygon);
      deleteFeatures.Delete(selection);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!deleteFeatures.IsEmpty)
      {
        var result = deleteFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await deleteFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATE(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATECHAINEDOPERATION
    #region Duplicate Features
    ///<summary>
    ///Duplicates a feature in a feature layer and modifies its geometry.
    ///</summary>
    /// <param name="featureLayer">The feature layer containing the feature to duplicate.</param>
    /// <param name="objectId">The ObjectID of the feature to duplicate.</param>
    /// <param name="polygon">The polygon defining the new geometry for the duplicated feature.</param>
    public static void DuplicateFeature(FeatureLayer featureLayer, long objectId, Geometry polygon)
    {
      var duplicateFeatures = new EditOperation() { Name = "Duplicate Features" };

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      var insp2 = new Inspector();
      insp2.Load(featureLayer, objectId);
      var geom = insp2["SHAPE"] as Geometry;

      var rtoken = duplicateFeatures.Create(insp2.MapMember, insp2.ToDictionary(a => a.FieldName, a => a.CurrentValue));
      if (!duplicateFeatures.IsEmpty)
      {
        if (duplicateFeatures.Execute())//Execute and ExecuteAsync will return true if the operation was successful and false if not
        {
          var modifyOp = duplicateFeatures.CreateChainedOperation();
          modifyOp.Modify(featureLayer, (long)rtoken.ObjectID, GeometryEngine.Instance.Move(geom, 500.0, 500.0));
          if (!modifyOp.IsEmpty)
          {
            var result = modifyOp.Execute();
          }
        }
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Explode(ARCGIS.DESKTOP.MAPPING.Layer,SYSTEM.COLLECTIONS.GENERIC.IEnumerable{Int64},Boolean)
    #region Explode Features
    /// <summary>
    /// Explodes a multi-part feature into individual features.
    /// </summary>
    /// <param name="featureLayer">The feature layer containing the feature to explode.</param>
    /// <param name="objectId">The ObjectID of the feature to explode.</param>
    public static void ExplodeFeatures(FeatureLayer featureLayer, long objectId)
    {
      var explodeFeatures = new EditOperation() { Name = "Explode Features" };

      //Take a multipart and convert it into one feature per part
      //Provide a list of ids to convert multiple
      explodeFeatures.Explode(featureLayer, new List<long>() { objectId }, true);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!explodeFeatures.IsEmpty)
      {
        var result = explodeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await explodeFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(EditingRowTemplate,ARCGIS.DESKTOP.MAPPING.Layer,IEnumerable{Int64})
    // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
    #region Merge Features
    /// <summary>
    /// Merges features from one layer into another.
    /// </summary>
    /// <param name="featureLayer"></param>
    /// <param name="destinationLayer"></param>
    /// <param name="objectId"></param>
    /// <param name="polygon"></param>
    /// <param name="currentTemplate"></param>
    public static void MergeFeatures(FeatureLayer featureLayer, FeatureLayer destinationLayer, long objectId, Polygon polygon, EditingRowTemplate currentTemplate)
    {
      var mergeFeatures = new EditOperation() { Name = "Merge Features" };

      //Merge three features into a new feature using defaults
      //defined in the current template
      mergeFeatures.Merge(currentTemplate, featureLayer, new List<long>() { 10, 96, 12 });

      //Merge three features into a new feature in the destination layer
      mergeFeatures.Merge(destinationLayer, featureLayer, new List<long>() { 10, 96, 12 });

      //Use an inspector to set the new attributes of the merged feature
      var inspector = new Inspector();
      inspector.Load(featureLayer, objectId);//base attributes on an existing feature
                                             //change attributes for the new feature
      inspector["NAME"] = "New name";
      inspector["DESCRIPTION"] = "New description";

      //Merge features into a new feature in the same layer using the
      //defaults set in the inspector
      mergeFeatures.Merge(featureLayer, new List<long>() { 10, 96, 12 }, inspector);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!mergeFeatures.IsEmpty)
      {
        var result = mergeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await mergeFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, Nullable<System.Collections.Generic.Dictionary<System.String, System.object>>)
    #region Modify single feature
    /// <summary>
    /// Modifies a single feature in the specified feature layer.
    /// </summary>
    /// <param name="featureLayer"></param>
    /// <param name="objectId"></param>
    /// <param name="polygon"></param>
    public static void ModifyFeature(FeatureLayer featureLayer, long objectId, Polygon polygon)
    {
      var modifyFeature = new EditOperation() { Name = "Modify a feature" };

      //use an inspector
      var modifyInspector = new Inspector();
      modifyInspector.Load(featureLayer, objectId);//base attributes on an existing feature

      //change attributes for the new feature
      modifyInspector["SHAPE"] = polygon;//Update the geometry
      modifyInspector["NAME"] = "Updated name";//Update attribute(s)

      modifyFeature.Modify(modifyInspector);

      //update geometry and attributes using overload
      var featureAttributes = new Dictionary<string, object>();
      featureAttributes["NAME"] = "Updated name";//Update attribute(s)
      modifyFeature.Modify(featureLayer, objectId, polygon, featureAttributes);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!modifyFeature.IsEmpty)
      {
        var result = modifyFeature.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await modifyFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
    #region Modify multiple features
    /// <summary>
    /// 
    /// </summary>
    /// <param name="featureLayer"></param>
    public static void ModifyMultipleFeatures(FeatureLayer featureLayer)
    {
      //Search by attribute
      var queryFilter = new QueryFilter() { WhereClause = "OBJECTID < 1000000" };
      //Create list of oids to update
      var oidSet = new List<long>();
      using (var rc = featureLayer.Search(queryFilter))
      {
        while (rc.MoveNext())
        {
          using (var record = rc.Current)
          {
            oidSet.Add(record.GetObjectID());
          }
        }
      }

      //create and execute the edit operation
      var modifyFeatures = new EditOperation() { Name = "Modify features" };
      modifyFeatures.ShowProgressor = true;

      var multipleFeaturesInsp = new Inspector();
      multipleFeaturesInsp.Load(featureLayer, oidSet);
      multipleFeaturesInsp["MOMC"] = 24;
      modifyFeatures.Modify(multipleFeaturesInsp);
      if (!modifyFeatures.IsEmpty)
      {
        var result = modifyFeatures.ExecuteAsync(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Move(ArcGIs.Desktop.Mapping.SelectionSet, System.Double, System.Double)
    #region Move features
    /// <summary>
    /// Moves the shapes (geometries) of all selected features of a given feature layer of the active map view by a fixed offset.
    /// </summary>
    /// <remarks>This method retrieves the selected features from the first feature layer in the
    /// active map view  and moves them by a fixed offset of 10 units along both the X and Y axes. If no features
    /// are  selected, the method performs no action.</remarks>
    /// <param name="featureLayer"> The feature layer containing the features to be moved.</param>
    /// <returns>true if geometries were moved, false otherwise</returns>
    public static async Task<bool> MoveFeaturesByOffsetAsync(FeatureLayer featureLayer, double xOffset, double yOffset)
    {
      return await QueuedTask.Run<bool>(() =>
      {
        // If there are no selected features, return
        if (featureLayer.GetSelection().GetObjectIDs().Count == 0)
          return false;
        // set up a dictionary to store the layer and the object IDs of the selected features
        var selectionDictionary = new Dictionary<MapMember, List<long>>
          {
                  { featureLayer, featureLayer.GetSelection().GetObjectIDs().ToList() }
          };
        var moveEditOperation = new EditOperation() { Name = "Move features" };
        moveEditOperation.Move(SelectionSet.FromDictionary(selectionDictionary), xOffset, yOffset);  //specify your units along axis to move the geometry
        if (!moveEditOperation.IsEmpty)
        {
          var result = moveEditOperation.Execute();
          return result; // return the operation result: true if successful, false if not
        }
        return false; // return false to indicate that the operation was not empty
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(LAYER,INT64,GEOMETRY,DICTIONARY{STRING,OBJECT})
    #region Move feature to a specific coordinate
    /// <summary>
    /// Moves the first selected feature in the specified feature layer to the given coordinates.
    /// </summary>
    /// <remarks>This method modifies the geometry of the first selected feature in the specified layer to
    /// match the provided coordinates. If no features are selected, the operation will not be performed, and the method
    /// will return <see langword="false"/>.</remarks>
    /// <param name="featureLayer">The feature layer containing the feature to be moved. Cannot be null.</param>
    /// <param name="xCoordinate">The x-coordinate to which the feature will be moved.</param>
    /// <param name="yCoordinate">The y-coordinate to which the feature will be moved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the feature was
    /// successfully moved; otherwise, <see langword="false"/>.</returns>
    public static async Task<bool> MoveFeatureToCoordinateAsync(FeatureLayer featureLayer, double xCoordinate, double yCoordinate)
    {
      return await QueuedTask.Run<bool>(() =>
      {
        //Get all of the selected ObjectIDs from the layer.
        var mySelection = featureLayer.GetSelection();
        var selOid = mySelection.GetObjectIDs().FirstOrDefault();

        var moveToPoint = new MapPointBuilderEx(xCoordinate, yCoordinate, 0.0, 0.0, featureLayer.GetSpatialReference());

        var moveEditOperation = new EditOperation() { Name = "Move features" };
        moveEditOperation.Modify(featureLayer, selOid, moveToPoint.ToGeometry());  //Modify the feature to the new geometry 
        if (!moveEditOperation.IsEmpty)
        {
          var result = moveEditOperation.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          return result; // return the operation result: true if successful, false if not
        }
        return false; // return false to indicate that the operation was not empty
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,IEnumerable{Int64},Nullable{Double})
    #region Planarize Features
    /// <summary>
    /// Planarizes the specified feature in the given feature layer.
    /// </summary>
    /// <param name="featureLayer"></param>
    /// <param name="objectId"></param>
    public static void PlanarizeFeatures(FeatureLayer featureLayer, long objectId)
    {
      // note - EditOperation.Planarize requires a standard license. 
      //  An exception will be thrown if Pro is running under a basic license. 

      var planarizeFeatures = new EditOperation() { Name = "Planarize Features" };

      //Planarize one or more features
      planarizeFeatures.Planarize(featureLayer, new List<long>() { objectId });

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!planarizeFeatures.IsEmpty)
      {
        var result = planarizeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await planarizeFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.ParallelOffset.Builder.#ctor
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.ParallelOffset.Builder)
    #region ParallelOffset
    /// <summary>
    /// Creates parallel offset features from the selected features.
    /// </summary>
    public static void CreateParallelOffsetFeatures()
    {
      //Create parallel features from the selected features

      //find the roads layer
      var roadsLayer = MapView.Active.Map.FindLayers("Roads").FirstOrDefault();

      //instantiate parallelOffset builder and set parameters
      var parOffsetBuilder = new ParallelOffset.Builder()
      {
        Selection = MapView.Active.Map.GetSelection(),
        Template = roadsLayer.GetTemplate("Freeway"),
        Distance = 200,
        Side = ParallelOffset.SideType.Both,
        Corner = ParallelOffset.CornerType.Mitered,
        Iterations = 1,
        AlignConnected = false,
        CopyToSeparateFeatures = false,
        RemoveSelfIntersectingLoops = true
      };

      //create EditOperation and execute
      var parallelOp = new EditOperation();
      parallelOp.Create(parOffsetBuilder);
      if (!parallelOp.IsEmpty)
      {
        var result = parallelOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Reshape(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
    #region Reshape Features
    /// <summary>
    /// Reshapes the specified feature in the given feature layer.
    /// </summary>
    /// <param name="featureLayer"></param>
    /// <param name="objectId"></param>
    /// <param name="modifyLine"></param>
    public static void ReshapeFeatures(FeatureLayer featureLayer, long objectId, Polyline modifyLine)
    {
      var reshapeFeatures = new EditOperation() { Name = "Reshape Features" };

      reshapeFeatures.Reshape(featureLayer, objectId, modifyLine);

      //Reshape a set of features that intersect some geometry....

      //at 2.x - var selFeatures = MapView.Active.GetFeatures(modifyLine).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //reshapeFeatures.Reshape(selFeatures, modifyLine);

      reshapeFeatures.Reshape(MapView.Active.GetFeatures(modifyLine), modifyLine);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!reshapeFeatures.IsEmpty)
      {
        var result = reshapeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await reshapeFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Rotate(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double)
    #region Rotate Features
    /// <summary>
    /// Rotates the selected features by a specified angle.
    /// </summary>
    /// <param name="polygon"></param>
    public static void RotateFeatures(Polygon polygon, MapPoint origin, double angle)
    {
      var rotateFeatures = new EditOperation() { Name = "Rotate Features" };

      //Rotate works on a selected set of features
      //Get all features that intersect a polygon
      //Rotate selected features 90 deg about "origin"
      rotateFeatures.Rotate(MapView.Active.GetFeatures(polygon), origin, angle);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!rotateFeatures.IsEmpty)
      {
        var result = rotateFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await rotateFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Scale(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double, System.Double, System.Double)
    #region Scale Features
    /// <summary>
    /// Scales the selected features by a specified factor.
    /// </summary>
    /// <param name="polygon"></param>
    /// <param name="origin"></param>
    /// <param name="scale"></param>
    public static void ScaleFeatures(Polygon polygon, MapPoint origin, double scale)
    {
      var scaleFeatures = new EditOperation() { Name = "Scale Features" };

      //Rotate works on a selected set of features
      //Scale the selected features by scale in the X and Y direction
      scaleFeatures.Scale(MapView.Active.GetFeatures(polygon), origin, scale, scale, 0.0);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!scaleFeatures.IsEmpty)
      {
        var result = scaleFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await scaleFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.SplitByPercentage.#ctor
    // cref: ArcGIS.Desktop.Editing.SplitByEqualParts.#ctor
    // cref: ArcGIS.Desktop.Editing.SplitByDistance.#ctor
    // cref: ArcGIS.Desktop.Editing.SplitByVaryingDistance.#ctor
    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, System.Collections.Generic.IEnumerable<ArcGID.Core.Geometry.MapPoint>)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Desktop.Editing.SplitMethod)
    #region Split Features
    /// <summary>
    /// Splits the specified feature at the given points.
    /// </summary>
    /// <param name="featureLayer"></param>
    /// <param name="splitPoints"></param>
    /// <param name="objectId"></param>
    public static void SplitFeatures(FeatureLayer featureLayer, List<MapPoint> splitPoints, long objectId)
    {
      //Split features at given points
      //Split features using EditOperation.Split overloads
      var splitFeatures = new EditOperation() { Name = "Split Features" };

      //Split the feature at given points
      splitFeatures.Split(featureLayer, objectId, splitPoints);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!splitFeatures.IsEmpty)
      {
        var result = splitFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await splitAtPointsFeatures.ExecuteAsync();
    }

    /// <summary>
    /// Splits a feature in the specified feature layer based on a given percentage and object ID.
    /// </summary>
    /// <remarks>This method uses the <see cref="EditOperation.Split"/> method to perform the split operation.
    /// The operation must be executed within a <see cref="ArcGIS.Core.Threading.QueuedTask.Run"/> context.</remarks>
    /// <param name="featureLayer">The feature layer containing the feature to be split.</param>
    /// <param name="percentage">The percentage at which the feature will be split. Must be a value between 0 and 100.</param>
    /// <param name="objectId">The object ID of the feature to be split.</param>
    public static void SplitFeatures(FeatureLayer featureLayer, double percentage, long objectId)
    {
      //Split features using EditOperation.Split overloads
      var splitFeatures = new EditOperation() { Name = "Split Features" };

      // split using percentage
      var splitByPercentage = new SplitByPercentage() { Percentage = 33, SplitFromStartPoint = true };
      splitFeatures.Split(featureLayer, objectId, splitByPercentage);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!splitFeatures.IsEmpty)
      {
        var result = splitFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await splitAtPointsFeatures.ExecuteAsync();
    }

    /// <summary>
    /// Splits a feature in the specified feature layer into the specified number of parts.
    /// </summary>
    /// <param name="featureLayer">The feature layer containing the feature to be split.</param>
    /// <param name="objectId">The object ID of the feature to be split.</param>
    /// <param name="numParts">The number of parts to split the feature into.</param>
    public static void SplitFeatures(FeatureLayer featureLayer, long objectId, int numParts)
    {
      // split using equal parts
      //Split features using EditOperation.Split overloads
      var splitFeatures = new EditOperation() { Name = "Split Features" };
      var splitByEqualParts = new SplitByEqualParts() { NumParts = numParts };
      splitFeatures.Split(featureLayer, objectId, splitByEqualParts);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!splitFeatures.IsEmpty)
      {
        var result = splitFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await splitAtPointsFeatures.ExecuteAsync();
    }

    /// <summary>
    /// Splits a feature in the specified feature layer at a given distance and object ID.
    /// </summary>
    /// <param name="featureLayer">The feature layer containing the feature to be split.</param>
    /// <param name="objectId">The object ID of the feature to be split.</param>
    /// <param name="distance">The distance at which to split the feature.</param>
    public static void SplitFeatures(FeatureLayer featureLayer, long objectId, double distance)
    {
      //Split features using EditOperation.Split overloads
      var splitFeatures = new EditOperation() { Name = "Split Features" };

      // split using single distance
      var splitByDistance = new SplitByDistance() { Distance = distance, SplitFromStartPoint = false };
      splitFeatures.Split(featureLayer, objectId, splitByDistance);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!splitFeatures.IsEmpty)
      {
        var result = splitFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await splitAtPointsFeatures.ExecuteAsync();
    }

    /// <summary>
    /// Splits a feature in the specified feature layer at a given distance and object ID.
    /// </summary>
    /// <param name="featureLayer">The feature layer containing the feature to be split.</param>
    /// <param name="objectId">The object ID of the feature to be split.</param>
    /// <param name="distances">A list of distances at which to split the feature.</param>
    public static void SplitFeatures(FeatureLayer featureLayer, long objectId, List<double> distances)
    {
      //Split features using EditOperation.Split overloads
      var splitFeatures = new EditOperation() { Name = "Split Features" };

      // split using varying distance
      var splitByVaryingDistance = new SplitByVaryingDistance() { Distances = distances, SplitFromStartPoint = true, ProportionRemainder = true };
      splitFeatures.Split(featureLayer, objectId, splitByVaryingDistance);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!splitFeatures.IsEmpty)
      {
        var result = splitFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await splitAtPointsFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64)
    // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
    // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.Collections.Generic.Dictionary{System.String,System.String})
    #region Transfer Attributes
    /// <summary>
    /// Transfers attributes from a source feature to a target feature between specified layers.
    /// </summary>
    /// <param name="featureLayer">The source <see cref="FeatureLayer"/> containing the feature with the attributes to transfer.</param>
    /// <param name="objectId">The object ID of the source feature whose attributes will be transferred.</param>
    /// <param name="targetOID">The object ID of the target feature to which the attributes will be transferred.</param>
    /// <param name="destinationLayer">The destination <see cref="FeatureLayer"/> containing the feature that will receive the transferred attributes.</param>
    public static void TransferAttributes(FeatureLayer featureLayer, long objectId, long targetOID, FeatureLayer destinationLayer)
    {
      var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };

      // transfer attributes using the stored field mapping
      transferAttributes.TransferAttributes(featureLayer, objectId, destinationLayer, targetOID);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!transferAttributes.IsEmpty)
      {
        var result = transferAttributes.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await transferAttributes.ExecuteAsync();
    }

    /// <summary>
    /// Transfers attributes from a source feature to a target feature between specified layers.
    /// </summary>
    /// <remarks>This method performs an attribute transfer operation using an auto-match mechanism for
    /// attributes.  It must be executed within the context of a <see cref="QueuedTask.Run"/> to ensure thread
    /// safety.</remarks>
    /// <param name="objectId">The object ID of the source feature whose attributes will be transferred.</param>
    /// <param name="targetOID">The object ID of the target feature to which the attributes will be transferred.</param>
    /// <param name="featureLayer">The source <see cref="FeatureLayer"/> containing the feature with the attributes to transfer.</param>
    /// <param name="destinationLayer">The destination <see cref="FeatureLayer"/> containing the feature that will receive the transferred attributes.</param>
    public static void TransferAttributes(FeatureLayer featureLayer, FeatureLayer destinationLayer, long objectId, long targetOID)
    {
      var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };
      // transfer attributes using an auto-match on the attributes
      transferAttributes.TransferAttributes(featureLayer, objectId, destinationLayer, targetOID, "");

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!transferAttributes.IsEmpty)
      {
        var result = transferAttributes.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await transferAttributes.ExecuteAsync();
    }

    /// <summary>
    /// Transfers attributes from a source feature to a target feature in a specified destination layer.
    /// </summary>
    /// <remarks>This method performs an attribute transfer operation using a predefined set of field
    /// mappings.  The operation must be executed within the context of <see cref="QueuedTask.Run"/> to ensure thread
    /// safety.</remarks>
    /// <param name="featureLayer">The source <see cref="FeatureLayer"/> containing the feature whose attributes will be transferred.</param>
    /// <param name="objectId">The object ID of the source feature in the <paramref name="featureLayer"/>.</param>
    /// <param name="targetOID">The object ID of the target feature in the <paramref name="destinationLayer"/>.</param>
    /// <param name="destinationLayer">The destination <see cref="FeatureLayer"/> where the attributes will be transferred to.</param>
    public static void TransferAttributesSourceToTargetFeatureDefaultMapping(FeatureLayer featureLayer, FeatureLayer destinationLayer, long objectId, long targetOID)
    {
      var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };
      // transfer attributes using a specified set of field mappings
      //  dictionary key is the field name in the destination layer, dictionary value is the field name in the source layer
      var fldMapping = new Dictionary<string, string>();
      fldMapping.Add("NAME", "SURNAME");
      fldMapping.Add("ADDRESS", "ADDRESS");
      transferAttributes.TransferAttributes(featureLayer, objectId, destinationLayer, targetOID, fldMapping);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!transferAttributes.IsEmpty)
      {
        var result = transferAttributes.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await transferAttributes.ExecuteAsync();
    }

    /// <summary>
    /// Transfers attributes from a source feature to a target feature in a specified destination layer.
    /// </summary>
    /// <param name="featureLayer">The source <see cref="FeatureLayer"/> containing the feature whose attributes will be transferred.</param>
    /// <param name="destinationLayer">The destination <see cref="FeatureLayer"/> where the attributes will be transferred to.</param>
    /// <param name="objectId">The object ID of the source feature in the <paramref name="featureLayer"/>.</param>
    /// <param name="targetOID">The object ID of the target feature in the <paramref name="destinationLayer"/>.</param>
    public static void TransferAttributesSourceToTargetFeatureCustomMapping(FeatureLayer featureLayer, FeatureLayer destinationLayer, long objectId, long targetOID)
    {
      var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };
      // transfer attributes using a custom field mapping expression
      string expression = "return {\r\n  " +
          "\"ADDRESS\" : $sourceFeature['ADDRESS'],\r\n  " +
          "\"IMAGE\" : $sourceFeature['IMAGE'],\r\n  + " +
          "\"PRECINCT\" : $sourceFeature['PRECINCT'],\r\n  " +
          "\"WEBSITE\" : $sourceFeature['WEBSITE'],\r\n  " +
          "\"ZIP\" : $sourceFeature['ZIP']\r\n " +
          "}";
      transferAttributes.TransferAttributes(featureLayer, objectId, destinationLayer, targetOID, expression);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!transferAttributes.IsEmpty)
      {
        var result = transferAttributes.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await transferAttributes.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.TransformByLinkLayer.#ctor
    // cref: ArcGIS.Desktop.Editing.TransformMethodType
    // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.Layer,ArcGIS.Desktop.Editing.TransformMethod)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.SelectionSet,ArcGIS.Desktop.Editing.TransformMethod)
    #region Transform Features
    /// <summary>
    /// Transforms features from a source layer to a target layer using a specified transformation method.
    /// </summary>
    /// <param name="featureLayer"></param>
    /// <param name="linklayer"></param>
    /// <param name="polygon"></param>
    public static void TransformFeatures(FeatureLayer featureLayer, FeatureLayer linkLayer, Polygon polygon)
    {
      //Transform features using EditOperation.Transform overloads
      var transformFeatures = new EditOperation() { Name = "Transform Features" };

      //Transform a selected set of features
      ////Perform an affine transformation
      //transformFeatures.TransformAffine(featureLayer, linkLayer);
      var affine_transform = new TransformByLinkLayer()
      {
        LinkLayer = linkLayer,
        TransformType = TransformMethodType.Affine //TransformMethodType.Similarity
      };
      //Transform a selected set of features
      transformFeatures.Transform(MapView.Active.GetFeatures(polygon), affine_transform);
      //Perform an affine transformation
      transformFeatures.Transform(featureLayer, affine_transform);
      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!transformFeatures.IsEmpty)
      {
        var result = transformFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      //or use async flavor
      //await transformFeatures.ExecuteAsync();
    }

    #endregion

  }
  public class CSVData
  {
    public Double X, Y, StopOrder, FacilityID;
  }
}