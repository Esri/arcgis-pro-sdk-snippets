using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Editing.Templates;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProSnippetsEditing.ProSnippetsSnapping;

namespace ProSnippetsEditing
{
  public static class ProSnippetsEditOperation
  {

    #region ProSnippet Group: Edit Operation Methods
    #endregion

    public static void EditOperations(EditingTemplate currentTemplate, Geometry geometry)
    {

      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList()[0] as FeatureLayer;
      var polygon = new PolygonBuilderEx().ToGeometry();
      var clipPoly = new PolygonBuilderEx().ToGeometry();
      var cutLine = new PolylineBuilderEx().ToGeometry();
      var modifyLine = cutLine;
      var oid = 1;
      var layer = featureLayer;
      var standaloneTable = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().FirstOrDefault();

      var opEdit = new EditOperation();
      // cref: ArcGIS.Desktop.Editing.EditOperation.IsEmpty
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
      #region Edit Operation - check for actions before Execute

      // Some times when using EditOperation.Modify you can unknowingly be attempting to set
      //  an attribute to value 
      //  setting 
      // In this scenario the Modify action will detect that nothing is required
      // and do nothing. Because no actions have occurred, the
      // Consequently the Execute operation will fail. 
      if (!opEdit.IsEmpty)
        opEdit.Execute();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.#ctor()
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Editing.EditOperation.IsSucceeded
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
      #region Edit Operation Create Features

      var createFeatures = new EditOperation() { Name = "Create Features" };
      //Create a feature with a polygon
      var token = createFeatures.Create(featureLayer, polygon);
      if (createFeatures.IsSucceeded)
      {
        // token.ObjectID wll be populated with the objectID of the created feature after Execute has been successful
      }
      //Do a create features and set attributes
      var attributes = new Dictionary<string, object>();
      attributes.Add("SHAPE", polygon);
      attributes.Add("NAME", "Corner Market");
      attributes.Add("SIZE", 1200.5);
      attributes.Add("DESCRIPTION", "Corner Market");

      createFeatures.Create(featureLayer, attributes);

      //Create features using the current template
      //Must be within a MapTool
      createFeatures.Create(currentTemplate, polygon);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run

      if (!createFeatures.IsEmpty)
      {
        createFeatures.Execute(); //Execute will return true if the operation was successful and false if not.
      }

      //or use async flavor
      //await createFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate, ArcGIS.Core.Geometry.Geometry)
      #region Create a feature using the current template
      var myTemplate = ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current;

      //Create edit operation and execute
      var op = new ArcGIS.Desktop.Editing.EditOperation() { Name = "Create my feature" };
      op.Create(myTemplate, geometry);
      if (!op.IsEmpty)
      {
        var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
      #region Create feature from a modified inspector

      var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      insp.Load(layer, 86);

      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // modify attributes if necessary
        // insp["Field1"] = newValue;

        //Create new feature from an existing inspector (copying the feature)
        var createOp = new EditOperation() { Name = "Create from insp" };
        createOp.Create(insp.MapMember, insp.ToDictionary(a => a.FieldName, a => a.CurrentValue));

        if (!createOp.IsEmpty)
        {
          var result = createOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion

      var csvData = new List<CSVData>();

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
      #region Create features from a CSV file
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
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate)
      #region Edit Operation Create row in a table using a table template
      var tableTemplate = standaloneTable.GetTemplates().FirstOrDefault();
      var createRow = new EditOperation() { Name = "Create a row in a table" };
      //Creating a new row in a standalone table using the table template of your choice
      createRow.Create(tableTemplate);

      if (!createRow.IsEmpty)
      {
        var result = createRow.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Clip(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Editing.ClipMode)
      #region Edit Operation Clip Features

      var clipFeatures = new EditOperation() { Name = "Clip Features" };
      clipFeatures.Clip(featureLayer, oid, clipPoly, ClipMode.PreserveArea);
      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!clipFeatures.IsEmpty)
      {
        var result = clipFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await clipFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
      #region Edit Operation Cut Features

      var select = MapView.Active.SelectFeatures(clipPoly);

      var cutFeatures = new EditOperation() { Name = "Cut Features" };
      cutFeatures.Split(featureLayer, oid, cutLine);

      //Cut all the selected features in the active view
      //Select using a polygon (for example)
      //at 2.x - var kvps = MapView.Active.SelectFeatures(polygon).Select(
      //      k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //cutFeatures.Split(kvps, cutLine);
      var sset = MapView.Active.SelectFeatures(polygon);
      cutFeatures.Split(sset, cutLine);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!cutFeatures.IsEmpty)
      {
        var result = cutFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await cutFeatures.ExecuteAsync();

      #endregion
      {
        // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
        // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.SelectionSet)
        #region Edit Operation Delete Features

        var deleteFeatures = new EditOperation() { Name = "Delete Features" };
        var table = MapView.Active.Map.StandaloneTables[0];
        //Delete a row in a standalone table
        deleteFeatures.Delete(table, oid);

        //Delete all the selected features in the active view
        //Select using a polygon (for example)
        //at 2.x - var selection = MapView.Active.SelectFeatures(polygon).Select(
        //      k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
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

        #endregion
      }

      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATE(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATECHAINEDOPERATION
      #region Edit Operation Duplicate Features
      {
        var duplicateFeatures = new EditOperation() { Name = "Duplicate Features" };

        //Duplicate with an X and Y offset of 500 map units
        //At 2.x duplicateFeatures.Duplicate(featureLayer, oid, 500.0, 500.0, 0.0);

        //Execute to execute the operation
        //Must be called within QueuedTask.Run
        var insp2 = new Inspector();
        insp2.Load(featureLayer, oid);
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
      #region Edit Operation Explode Features

      var explodeFeatures = new EditOperation() { Name = "Explode Features" };

      //Take a multipart and convert it into one feature per part
      //Provide a list of ids to convert multiple
      explodeFeatures.Explode(featureLayer, new List<long>() { oid }, true);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!explodeFeatures.IsEmpty)
      {
        var result = explodeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await explodeFeatures.ExecuteAsync();

      #endregion

      var destinationLayer = featureLayer;

      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(EditingRowTemplate,ARCGIS.DESKTOP.MAPPING.Layer,IEnumerable{Int64})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
      #region Edit Operation Merge Features

      var mergeFeatures = new EditOperation() { Name = "Merge Features" };

      //Merge three features into a new feature using defaults
      //defined in the current template
      //At 2.x -
      //mergeFeatures.Merge(this.CurrentTemplate as EditingFeatureTemplate, featureLayer, new List<long>() { 10, 96, 12 });
      mergeFeatures.Merge(currentTemplate as EditingRowTemplate, featureLayer, new List<long>() { 10, 96, 12 });

      //Merge three features into a new feature in the destination layer
      mergeFeatures.Merge(destinationLayer, featureLayer, new List<long>() { 10, 96, 12 });

      //Use an inspector to set the new attributes of the merged feature
      var inspector = new Inspector();
      inspector.Load(featureLayer, oid);//base attributes on an existing feature
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

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, Nullable<System.Collections.Generic.Dictionary<System.String, System.object>>)
      #region Edit Operation Modify single feature

      var modifyFeature = new EditOperation() { Name = "Modify a feature" };

      //use an inspector
      var modifyInspector = new Inspector();
      modifyInspector.Load(featureLayer, oid);//base attributes on an existing feature

      //change attributes for the new feature
      modifyInspector["SHAPE"] = polygon;//Update the geometry
      modifyInspector["NAME"] = "Updated name";//Update attribute(s)

      modifyFeature.Modify(modifyInspector);

      //update geometry and attributes using overload
      var featureAttributes = new Dictionary<string, object>();
      featureAttributes["NAME"] = "Updated name";//Update attribute(s)
      modifyFeature.Modify(featureLayer, oid, polygon, featureAttributes);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!modifyFeature.IsEmpty)
      {
        var result = modifyFeature.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await modifyFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Edit Operation Modify multiple features

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
      #endregion
      // cref: ArcGIS.Desktop.Editing.EditOperation.Move(ArcGIs.Desktop.Mapping.SelectionSet, System.Double, System.Double)
      #region Move features

      //Get all of the selected ObjectIDs from the layer.
      var firstLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      var selectionfromMap = firstLayer.GetSelection();

      // set up a dictionary to store the layer and the object IDs of the selected features
      var selectionDictionary = new Dictionary<MapMember, List<long>>();
      selectionDictionary.Add(firstLayer as MapMember, selectionfromMap.GetObjectIDs().ToList());

      var moveFeature = new EditOperation() { Name = "Move features" };
      //at 2.x - moveFeature.Move(selectionDictionary, 10, 10);  //specify your units along axis to move the geometry
      moveFeature.Move(SelectionSet.FromDictionary(selectionDictionary), 10, 10);  //specify your units along axis to move the geometry
      if (!moveFeature.IsEmpty)
      {
        var result = moveFeature.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(LAYER,INT64,GEOMETRY,DICTIONARY{STRING,OBJECT})
      #region Move feature to a specific coordinate

      //Get all of the selected ObjectIDs from the layer.
      var abLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      var mySelection = abLayer.GetSelection();
      var selOid = mySelection.GetObjectIDs().FirstOrDefault();

      var moveToPoint = new MapPointBuilderEx(1.0, 2.0, 3.0, 4.0, MapView.Active.Map.SpatialReference); //can pass in coordinates.

      var modifyFeatureCoord = new EditOperation() { Name = "Move features" };
      modifyFeatureCoord.Modify(abLayer, selOid, moveToPoint.ToGeometry());  //Modify the feature to the new geometry 
      if (!modifyFeatureCoord.IsEmpty)
      {
        var result = modifyFeatureCoord.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,IEnumerable{Int64},Nullable{Double})
      #region Edit Operation Planarize Features

      // note - EditOperation.Planarize requires a standard license. 
      //  An exception will be thrown if Pro is running under a basic license. 

      var planarizeFeatures = new EditOperation() { Name = "Planarize Features" };

      //Planarize one or more features
      planarizeFeatures.Planarize(featureLayer, new List<long>() { oid });

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!planarizeFeatures.IsEmpty)
      {
        var result = planarizeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await planarizeFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.ParallelOffset.Builder.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.ParallelOffset.Builder)
      #region Edit Operation ParallelOffset
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

      //create editoperation and execute
      var parallelOp = new EditOperation();
      parallelOp.Create(parOffsetBuilder);
      if (!parallelOp.IsEmpty)
      {
        var result = parallelOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Reshape(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
      #region Edit Operation Reshape Features

      var reshapeFeatures = new EditOperation() { Name = "Reshape Features" };

      reshapeFeatures.Reshape(featureLayer, oid, modifyLine);

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

      #endregion

      var origin = MapPointBuilderEx.CreateMapPoint(0, 0, null);

      // cref: ArcGIS.Desktop.Editing.EditOperation.Rotate(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double)
      #region Edit Operation Rotate Features

      var rotateFeatures = new EditOperation() { Name = "Rotate Features" };

      //Rotate works on a selected set of features
      //Get all features that intersect a polygon

      //at 2.x - var rotateSelection = MapView.Active.GetFeatures(polygon).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //rotateFeatures.Rotate(rotateSelection, origin, Math.PI / 2);

      //Rotate selected features 90 deg about "origin"
      rotateFeatures.Rotate(MapView.Active.GetFeatures(polygon), origin, Math.PI / 2);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!rotateFeatures.IsEmpty)
      {
        var result = rotateFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await rotateFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Scale(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double, System.Double, System.Double)
      #region Edit Operation Scale Features

      var scaleFeatures = new EditOperation() { Name = "Scale Features" };

      //Rotate works on a selected set of features

      //var scaleSelection = MapView.Active.GetFeatures(polygon).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //scaleFeatures.Scale(scaleSelection, origin, 2.0, 2.0, 0.0);

      //Scale the selected features by 2.0 in the X and Y direction
      scaleFeatures.Scale(MapView.Active.GetFeatures(polygon), origin, 2.0, 2.0, 0.0);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!scaleFeatures.IsEmpty)
      {
        var result = scaleFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await scaleFeatures.ExecuteAsync();

      #endregion

      var mp1 = MapPointBuilderEx.CreateMapPoint(0, 0, null);
      var mp2 = mp1;
      var mp3 = mp1;

      // cref: ArcGIS.Desktop.Editing.SplitByPercentage.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByEqualParts.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByDistance.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByVaryingDistance.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, System.Collections.Generic.IEnumerable<ArcGID.Core.Geometry.MapPoint>)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Desktop.Editing.SplitMethod)
      #region Edit Operation Split Features

      var splitFeatures = new EditOperation() { Name = "Split Features" };

      var splitPoints = new List<MapPoint>() { mp1, mp2, mp3 };

      //Split the feature at 3 points
      splitFeatures.Split(featureLayer, oid, splitPoints);

      // split using percentage
      var splitByPercentage = new SplitByPercentage() { Percentage = 33, SplitFromStartPoint = true };
      splitFeatures.Split(featureLayer, oid, splitByPercentage);

      // split using equal parts
      var splitByEqualParts = new SplitByEqualParts() { NumParts = 3 };
      splitFeatures.Split(featureLayer, oid, splitByEqualParts);

      // split using single distance
      var splitByDistance = new SplitByDistance() { Distance = 27.3, SplitFromStartPoint = false };
      splitFeatures.Split(featureLayer, oid, splitByDistance);

      // split using varying distance
      var distances = new List<double>() { 12.5, 38.2, 89.99 };
      var splitByVaryingDistance = new SplitByVaryingDistance() { Distances = distances, SplitFromStartPoint = true, ProportionRemainder = true };
      splitFeatures.Split(featureLayer, oid, splitByVaryingDistance);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!splitFeatures.IsEmpty)
      {
        var result = splitFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await splitAtPointsFeatures.ExecuteAsync();

      #endregion

      var targetOID = oid;

      // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64)
      // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
      // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.Collections.Generic.Dictionary{System.String,System.String})
      #region Edit Operation: Transfer Attributes
      var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };

      // transfer attributes using the stored field mapping
      transferAttributes.TransferAttributes(featureLayer, oid, destinationLayer, targetOID);

      // OR transfer attributes using an auto-match on the attributes
      transferAttributes.TransferAttributes(featureLayer, oid, destinationLayer, targetOID, "");

      // OR transfer attributes using a specified set of field mappings
      //  dictionary key is the field name in the destination layer, dictionary value is the field name in the source layer
      var fldMapping = new Dictionary<string, string>();
      fldMapping.Add("NAME", "SURNAME");
      fldMapping.Add("ADDRESS", "ADDRESS");
      transferAttributes.TransferAttributes(featureLayer, oid, destinationLayer, targetOID, fldMapping);

      // OR transfer attributes using a custom field mapping expression
      string expression = "return {\r\n  " +
          "\"ADDRESS\" : $sourceFeature['ADDRESS'],\r\n  " +
          "\"IMAGE\" : $sourceFeature['IMAGE'],\r\n  + " +
          "\"PRECINCT\" : $sourceFeature['PRECINCT'],\r\n  " +
          "\"WEBSITE\" : $sourceFeature['WEBSITE'],\r\n  " +
          "\"ZIP\" : $sourceFeature['ZIP']\r\n " +
          "}";
      transferAttributes.TransferAttributes(featureLayer, oid, destinationLayer, targetOID, expression);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!transferAttributes.IsEmpty)
      {
        var result = transferAttributes.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await transferAttributes.ExecuteAsync();
      #endregion

      var linkLayer = featureLayer;


      // cref: ArcGIS.Desktop.Editing.TransformByLinkLayer.#ctor
      // cref: ArcGIS.Desktop.Editing.TransformMethodType
      // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.Layer,ArcGIS.Desktop.Editing.TransformMethod)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.SelectionSet,ArcGIS.Desktop.Editing.TransformMethod)
      #region Edit Operation Transform Features

      var transformFeatures = new EditOperation() { Name = "Transform Features" };

      //Transform a selected set of features
      //At 2.x - var transformSelection = MapView.Active.GetFeatures(polygon).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //transformFeatures.Transform(transformSelection, linkLayer);
      ////Transform just a layer
      //transformFeatures.Transform(featureLayer, linkLayer);
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

      #endregion

      IEnumerable<Polyline> linkLines = new List<Polyline>();
      IEnumerable<MapPoint> anchorPoints = new List<MapPoint>();
      IEnumerable<Polygon> limitedAdjustmentAreas = new List<Polygon>();
      var anchorPointsLayer = featureLayer;
      var limitedAdjustmentAreaLayer = featureLayer;

      // cref: ArcGIS.Desktop.Editing.RubbersheetByGeometries.#ctor
      // cref: ArcGIS.Desktop.Editing.RubbersheetByLayers.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Rubbersheet(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Editing.RubbersheetMethod)
      #region Edit Operation Rubbersheet Features

      //Perform rubbersheet by geometries
      var rubbersheetMethod = new RubbersheetByGeometries()
      {
        RubbersheetType = RubbersheetMethodType.Linear, //The RubbersheetType can be Linear of NearestNeighbor
        LinkLines = linkLines, //IEnumerable list of link lines (polylines)
        AnchorPoints = anchorPoints, //IEnumerable list of anchor points (map points)
        LimitedAdjustmentAreas = limitedAdjustmentAreas //IEnumerable list of limited adjustment areas (polygons)
      };

      var rubbersheetOp = new EditOperation();
      //Performs linear rubbersheet transformation on the features belonging to "layer" that fall within the limited adjustment areas
      rubbersheetOp.Rubbersheet(layer, rubbersheetMethod);
      //Execute the operation
      if (!rubbersheetOp.IsEmpty)
      {
        var result = rubbersheetOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //Alternatively, you can also perform rubbersheet by layer
      var rubbersheetMethod2 = new RubbersheetByLayers()
      {
        RubbersheetType = RubbersheetMethodType.NearestNeighbor, //The RubbersheetType can be Linear of NearestNeighbor
        LinkLayer = linkLayer,
        AnchorPointLayer = anchorPointsLayer,
        LimitedAdjustmentAreaLayer = limitedAdjustmentAreaLayer
      };

      //Performs nearest neighbor rubbersheet transformation on the features belonging to "layer" that fall within the limited adjustment areas
      rubbersheetOp.Rubbersheet(layer, rubbersheetMethod2);
      if (!rubbersheetOp.IsEmpty)
      {
        //Execute the operation
        var result = rubbersheetOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,Int64,Nullable{Double})
      #region Edit Operation Perform a Clip, Cut, and Planarize

      //Multiple operations can be performed by a single
      //edit operation.
      var clipCutPlanarizeFeatures = new EditOperation() { Name = "Clip, Cut, and Planarize Features" };
      clipCutPlanarizeFeatures.Clip(featureLayer, oid, clipPoly);
      clipCutPlanarizeFeatures.Split(featureLayer, oid, cutLine);
      clipCutPlanarizeFeatures.Planarize(featureLayer, oid);

      if (!clipCutPlanarizeFeatures.IsEmpty)
      {
        //Note: An edit operation is a single transaction. 
        //Execute the operations (in the order they were declared)
        clipCutPlanarizeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await clipCutPlanarizeFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.CreateChainedOperation
      // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
      #region Edit Operation Chain Edit Operations

      //Chaining operations is a special case. Use "Chained Operations" when you require multiple transactions 
      //to be undo-able with a single "Undo".

      //The most common use case for operation chaining is creating a feature with an attachment. 
      //Adding an attachment requires the object id (of a new feature) has already been created. 
      var editOperation1 = new EditOperation() { Name = string.Format("Create point in '{0}'", currentTemplate.Layer.Name) };

      long newFeatureID = -1;
      //The Create operation has to execute so we can get an object_id
      var token2 = editOperation1.Create(currentTemplate, polygon);

      //Must be within a QueuedTask
      editOperation1.Execute(); //Note: Execute and ExecuteAsync will return true if the operation was successful and false if not
      if (editOperation1.IsSucceeded)
      {
        newFeatureID = (long)token2.ObjectID;
        //Now, because we have the object id, we can add the attachment.  As we are chaining it, adding the attachment 
        //can be undone as part of the "Undo Create" operation. In other words, only one undo operation will show on the 
        //Pro UI and not two.
        var editOperation2 = editOperation1.CreateChainedOperation();
        //Add the attachment using the new feature id
        editOperation2.AddAttachment(currentTemplate.Layer, newFeatureID, @"C:\data\images\Hydrant.jpg");
        //Execute the chained edit operation. editOperation1 and editOperation2 show up as a single Undo operation
        //on the UI even though we had two transactions
        editOperation2.Execute();
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.RowToken
      // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Editing.RowToken,System.String)
      #region Edit Operation add attachment via RowToken

      //ArcGIS Pro 2.5 extends the EditOperation.AddAttachment method to take a RowToken as a parameter.
      //This allows you to create a feature, using EditOperation.CreateEx, and add an attachment in one transaction.

      var editOpAttach = new EditOperation();
      editOperation1.Name = string.Format("Create point in '{0}'", currentTemplate.Layer.Name);

      var attachRowToken = editOpAttach.Create(currentTemplate, polygon);
      editOpAttach.AddAttachment(attachRowToken, @"c:\temp\image.jpg");

      //Must be within a QueuedTask
      if (!editOpAttach.IsEmpty)
      {
        var result = editOpAttach.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      string newName = "";
      FeatureClass fc = null;
      Geometry splitLine = null;

      // cref: ArcGIS.Desktop.Editing.EditOperation
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteMode
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Core.Data.Row,System.String, System.object)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
      #region Order edits sequentially

      // perform an edit and then a split as one operation.
      QueuedTask.Run(() =>
      {
        var queryFilter = new QueryFilter() { WhereClause = "OBJECTID = " + oid.ToString() };

        // create an edit operation and name.
        var op = new EditOperation() { Name = "modify followed by split" };
        // set the ExecuteMode
        op.ExecuteMode = ExecuteModeType.Sequential;

        using (var rowCursor = fc.Search(queryFilter, false))
        {
          while (rowCursor.MoveNext())
          {
            using (var feature = rowCursor.Current as Feature)
            {
              op.Modify(feature, "NAME", newName);
            }
          }
        }

        op.Split(layer, oid, splitLine);
        if (!op.IsEmpty)
        {
          bool result = op.Execute();
        }
        // else
        //  The operation doesn't make any changes to the database so if executed it will fail
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnUndone(System.Action)
      // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnComitted(System.Action<System.Boolean>)
      // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnRedone(System.Action)
      #region SetOnUndone, SetOnRedone, SetOnComitted

      // SetOnUndone, SetOnRedone and SetOnComitted can be used to manage 
      // external actions(such as writing to a log table) that are associated with 
      // each edit operation.

      //get selected feature and update attribute
      var selectedFeatures = MapView.Active.Map.GetSelection();
      var testInspector = new Inspector();
      testInspector.Load(selectedFeatures.ToDictionary().Keys.First(), selectedFeatures.ToDictionary().Values.First());
      testInspector["Name"] = "test";

      //create and execute the edit operation
      var updateTestField = new EditOperation() { Name = "Update test field" };
      updateTestField.Modify(insp);

      //actions for SetOn...
      updateTestField.SetOnUndone(() =>
      {
        //Sets an action that will be called when this operation is undone.
        Debug.WriteLine("Operation is undone");
      });

      updateTestField.SetOnRedone(() =>
      {
        //Sets an action that will be called when this editoperation is redone.
        Debug.WriteLine("Operation is redone");
      });

      updateTestField.SetOnComitted((bool b) => //called on edit session save(true)/discard(false).
      {
        // Sets an action that will be called when this editoperation is committed.
        Debug.WriteLine("Operation is committed");
      });

      if (!updateTestField.IsEmpty)
      {
        var result = updateTestField.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      var lineLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();

      var changeVertexIDOperation = new EditOperation();
      //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.HasID
      //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.ID
      #region Convert vertices in a polyline to a Control Point
      //Control points are special vertices used to apply symbol effects to line or polygon features.
      //By default, they appear as diamonds when you edit them.
      //They can also be used to migrate representations from ArcMap to features in ArcGIS Pro.
      QueuedTask.Run(() =>
      {
        //iterate through the points in the polyline.
        var lineLayerCursor = lineLayer.GetSelection().Search();
        var lineVertices = new List<MapPoint>();
        long oid = -1;
        while (lineLayerCursor.MoveNext())
        {
          var lineFeature = lineLayerCursor.Current as Feature;
          var line = lineFeature.GetShape() as Polyline;
          int vertexIndex = 1;
          oid = lineFeature.GetObjectID();
          //Each point is converted into a MapPoint and gets added to a list. 
          foreach (var point in line.Points)
          {
            MapPointBuilderEx mapPointBuilderEx = new MapPointBuilderEx(point);
            //Changing the vertex 6 and 7 to control points
            if (vertexIndex == 6 || vertexIndex == 7)
            {
              //These points are made "ID Aware" and the IDs are set to be 1.
              mapPointBuilderEx.HasID = true;
              mapPointBuilderEx.ID = 1;
            }

            lineVertices.Add(mapPointBuilderEx.ToGeometry() as MapPoint);
            vertexIndex++;
          }
        }
        //create a new polyline using the point collection.
        var newLine = PolylineBuilderEx.CreatePolyline(lineVertices);
        //edit operation to modify the original line with the new line that contains control points.
        changeVertexIDOperation.Modify(lineLayer, oid, newLine);
        changeVertexIDOperation.Execute();
      });
      #endregion

    }


  }
}
