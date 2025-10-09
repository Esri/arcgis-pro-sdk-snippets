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
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Editing.Templates;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProSnippets.EditingSnippets
{
  /// <summary>
  /// Provides a comprehensive set of static methods for performing feature and table editing operations in ArcGIS Pro.
  /// Includes functionality for creating, modifying, deleting, moving, splitting, merging, transforming, and managing attributes and attachments
  /// for features and rows, as well as advanced editing workflows such as planarizing, rubber-sheeting, and chaining edit operations.
  /// </summary>
  public static partial class ProSnippetsEditing
  {
		#region ProSnippet Group: Edit Operation Methods
		#endregion

		/// <summary>
		/// Demonstrates a wide range of feature and table editing operations in ArcGIS Pro,
		/// including creating, modifying, deleting, moving, splitting, merging, transforming,
		/// and managing attributes and attachments using the EditOperation API.
		/// </summary>
		public static async Task ProSnippetsEditOperations2Async()
    {
      #region Variable initialization

      var activeMap = MapView.Active.Map;
      var featureLayer = activeMap.Layers.OfType<FeatureLayer>().FirstOrDefault();
      var objectId = 1; // example ObjectID of the feature to load
      var selectedFeatures = activeMap.GetSelection();
      var geometry = activeMap.GetDefaultExtent().Center;
      var polygon = GeometryEngine.Instance.Buffer(geometry, 100) as Polygon;
      var currentTemplate = featureLayer.GetTemplate("North Precinct");
      var standaloneTable = activeMap.StandaloneTables.FirstOrDefault();
      var clipPolygon = GeometryEngine.Instance.Buffer(geometry, 200) as Polygon;
      var cutLine = GeometryEngine.Instance.Buffer(geometry, 50) as Polyline;
      var destinationLayer = activeMap.Layers.OfType<FeatureLayer>().FirstOrDefault(l => l.Name == "Parks");
      var splitLine = GeometryEngine.Instance.Buffer(geometry, 75) as Polyline;
      var modifyLine = GeometryEngine.Instance.Buffer(geometry, 150) as Polyline;
      var mp1 = GeometryEngine.Instance.Move(geometry, -50, -50) as MapPoint;
      var mp2 = GeometryEngine.Instance.Move(geometry, 50, -50) as MapPoint;
      var mp3 = GeometryEngine.Instance.Move(geometry, 0, 50) as MapPoint;
      var splitPoints = new List<MapPoint>() { mp1, mp2, mp3 };
      var linkLayer = activeMap.Layers.OfType<FeatureLayer>().FirstOrDefault(l => l.Name == "Roads");
      var layer = activeMap.Layers.OfType<FeatureLayer>().FirstOrDefault(l => l.Name == "Test");
      var lineLayer = activeMap.Layers.OfType<FeatureLayer>().FirstOrDefault(l => l.Name == "Lines");
      var linkLines = new List<Polyline>();
      var anchorPoints = new List<MapPoint>();
      var limitedAdjustmentAreas = new List<Polygon>();
      var anchorPointsLayer = featureLayer;
      var limitedAdjustmentAreaLayer = featureLayer;
      var origin = mp1;
      var angle = 90.0; // specify the angle in degrees to rotate the geometry

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.IsEmpty
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
      #region Edit Operation - check for actions before Execute
      // Checks for pending actions in an edit operation before attempting to execute it.
      await QueuedTask.Run(() =>
      {
        // Create an edit operation
        var op = new EditOperation() { Name = "My Edit Operation" };
        // Add some actions to the edit operation
        op.Modify(featureLayer, objectId, geometry);
        // EditOperation.Modify can unknowingly set an attribute to the already existing value
        // In this scenario the Modify action will detect that no changes are required and consequently the Execute operation will fail.
        // To avoid this, we can check if the edit operation is empty before executing it.
        if (!op.IsEmpty)
        {
          // Execute the edit operation
          var result = op.Execute();
          Debug.WriteLine($"Edit operation executed successfully: {result}");
        }
        else
        {
          Debug.WriteLine("No actions to execute in the edit operation.");
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.#ctor()
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Editing.EditOperation.IsSucceeded
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
      #region Edit Operation Create Features
      // Creates new features in a feature layer using the specified geometry, attributes, or editing template.
      await QueuedTask.Run(() =>
      {
        var createFeatures = new EditOperation() { Name = "Create Features" };
        //Create a feature with a polygon
        var token = createFeatures.Create(featureLayer, polygon);
        if (createFeatures.IsSucceeded)
        {
          // token.ObjectID will be populated with the objectID of the created feature after Execute has been successful
        }
        //Do a create features and set attributes
        var attributes = new Dictionary<string, object>
      {
        { "SHAPE", polygon },
        { "NAME", "Corner Market" },
        { "SIZE", 1200.5 },
        { "DESCRIPTION", "Corner Market" }
      };
        createFeatures.Create(featureLayer, attributes);

        //Create features using the current template
        //Must be within a MapTool
        createFeatures.Create(currentTemplate, polygon);

        //Execute to execute the operation
        //Must be called within QueuedTask.Run

        if (!createFeatures.IsEmpty)
        {
          //Execute will return true if the operation was successful and false if not.
          createFeatures.Execute();
          //or use async flavor
          //await createFeatures.ExecuteAsync();
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate, ArcGIS.Core.Geometry.Geometry)
      #region Create a feature using the current template
      // Creates a new feature using the current editing template and the specified geometry.
      await QueuedTask.Run(() =>
      {
        var myTemplate = currentTemplate ?? ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current;

        //Create edit operation and execute
        var op = new ArcGIS.Desktop.Editing.EditOperation() { Name = "Create my feature" };
        op.Create(myTemplate, geometry);
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
      #region Create feature from a modified inspector
      // Creates a new feature by copying and optionally modifying the attributes of an existing feature using an inspector.
      await QueuedTask.Run(() =>
          {
            // Create an inspector and load a feature
            // The inspector is used to modify the attributes of the feature before creating it
            var insp = new Inspector();
            insp.Load(featureLayer, objectId);
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

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
      #region Create features from a CSV file
      // Creates new point features in the specified feature layer using data from a CSV source.
      var csvData = new List<(double X, double Y, double StopOrder, double FacilityID)>();
      //Run on MCT
      await QueuedTask.Run(() =>
      {
        //Create the edit operation
        var createOperation = new ArcGIS.Desktop.Editing.EditOperation() { Name = "Generate points", SelectNewFeatures = false };

        // determine the shape field name - it may not be 'Shape' 
        string shapeField = featureLayer.GetFeatureClass().GetDefinition().GetShapeField();

        //Loop through csv data
        foreach (var item in csvData)
        {

          //Create the point geometry
          ArcGIS.Core.Geometry.MapPoint newMapPoint =
              ArcGIS.Core.Geometry.MapPointBuilderEx.CreateMapPoint(item.X, item.Y);

          // include the attributes via a dictionary
          var atts = new Dictionary<string, object>
            {
              { "StopOrder", item.StopOrder },
              { "FacilityID", item.FacilityID },
              { shapeField, newMapPoint }
            };

          // queue feature creation
          createOperation.Create(featureLayer, atts);
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
      // Creates a new row in a standalone table using the specified table template.
      await QueuedTask.Run(() =>
        {
          var tableTemplate = standaloneTable.GetTemplates().FirstOrDefault();
          var createRow = new EditOperation() { Name = "Create a row in a table" };
          //Creating a new row in a standalone table using the table template of your choice
          createRow.Create(tableTemplate);
          if (!createRow.IsEmpty)
          {
            var result = createRow.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Clip(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Editing.ClipMode)
      #region Edit Operation Clip Features
      // Clips a feature to the specified polygon.
      await QueuedTask.Run(() =>
      {
        var clipFeatures = new EditOperation() { Name = "Clip Features" };
        clipFeatures.Clip(featureLayer, objectId, clipPolygon, ClipMode.PreserveArea);
        //Execute to execute the operation
        //Must be called within QueuedTask.Run
        if (!clipFeatures.IsEmpty)
        {
          //Execute and ExecuteAsync will return true if the operation was successful and false if not
          var result = clipFeatures.Execute();
          //or use async flavor
          //await clipFeatures.ExecuteAsync();
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
      #region Edit Operation Cut Features
      // Cuts features in the specified feature layer using the provided cut line and clip polygon.
      await QueuedTask.Run(() =>
        {
          var select = MapView.Active.SelectFeatures(clipPolygon);

          var cutFeatures = new EditOperation() { Name = "Cut Features" };
          cutFeatures.Split(featureLayer, objectId, cutLine);

          //Cut all the selected features in the active view
          //Select using a polygon (for example)
          cutFeatures.Split(select, cutLine);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!cutFeatures.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = cutFeatures.Execute();

            //or use async flavor
            //await cutFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.SelectionSet)
      #region Edit Operation Delete Features
      // Deletes a single feature from the specified feature layer using its ObjectID.
      await QueuedTask.Run(() =>
            {
              var deleteFeatures = new EditOperation() { Name = "Delete single feature" };
              //Delete a row in a standalone table
              deleteFeatures.Delete(featureLayer, objectId);
              //Execute to execute the operation
              //Must be called within QueuedTask.Run
              if (!deleteFeatures.IsEmpty)
              { //Execute and ExecuteAsync will return true if the operation was successful and false if not
                var result = deleteFeatures.Execute();
                //or use async flavor
                //await deleteFeatures.ExecuteAsync();
              }
            });
      #endregion

      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATE(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATECHAINEDOPERATION
      #region Edit Operation Duplicate Features
      // Duplicates a feature in a feature layer and modifies its geometry.
      await QueuedTask.Run(() =>
        {
          var duplicateFeatures = new EditOperation() { Name = "Duplicate Features" };

          //Duplicate with an X and Y offset of 500 map units

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
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Explode(ARCGIS.DESKTOP.MAPPING.Layer,SYSTEM.COLLECTIONS.GENERIC.IEnumerable{Int64},Boolean)
      #region Edit Operation Explode Features
      // Explodes a multi-part feature into individual features.
      await QueuedTask.Run(() =>
        {
          var explodeFeatures = new EditOperation() { Name = "Explode Features" };

          //Take a multipart and convert it into one feature per part
          //Provide a list of ids to convert multiple
          explodeFeatures.Explode(featureLayer, [objectId], true);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!explodeFeatures.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = explodeFeatures.Execute();
            //or use async flavor
            //await explodeFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(EditingRowTemplate,ARCGIS.DESKTOP.MAPPING.Layer,IEnumerable{Int64})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
      #region Edit Operation Merge Features
      // Merges multiple features from a source feature layer into a new feature, optionally using a template or inspector, and supports merging into a destination layer.
      await QueuedTask.Run(() =>
      {
        var mergeFeatures = new EditOperation() { Name = "Merge Features" };

        //Merge three features into a new feature using defaults
        //defined in the current template
        mergeFeatures.Merge(currentTemplate as EditingRowTemplate, featureLayer, [10, 96, 12]);

        //Merge three features into a new feature in the destination layer
        mergeFeatures.Merge(destinationLayer, featureLayer, [10, 96, 12]);

        //Use an inspector to set the new attributes of the merged feature
        var inspector = new Inspector();
        inspector.Load(featureLayer, objectId);//base attributes on an existing feature
        inspector["NAME"] = "New name";
        inspector["DESCRIPTION"] = "New description";

        //Merge features into a new feature in the same layer using the
        //defaults set in the inspector
        mergeFeatures.Merge(featureLayer, [10, 96, 12], inspector);

        //Execute to execute the operation
        //Must be called within QueuedTask.Run
        if (!mergeFeatures.IsEmpty)
        {
          //Execute and ExecuteAsync will return true if the operation was successful and false if not
          var result = mergeFeatures.Execute();
          //or use async flavor
          //await mergeFeatures.ExecuteAsync();
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, Nullable<System.Collections.Generic.Dictionary<System.String, System.object>>)
      #region Edit Operation Modify single feature
      // Modifies a single feature in the specified feature layer.
      await QueuedTask.Run(() =>
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
          var featureAttributes = new Dictionary<string, object>
          {
            ["NAME"] = "Updated name"//Update attribute(s)
          };
          modifyFeature.Modify(featureLayer, objectId, polygon, featureAttributes);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!modifyFeature.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = modifyFeature.Execute();
            //or use async flavor
            //await modifyFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Edit Operation Modify multiple features
      // Modifies multiple features in the specified feature layer by updating their attributes.
      await QueuedTask.Run(() =>
        {
          //Search by attribute
          var queryFilter = new QueryFilter() { WhereClause = "OBJECTID < 1000000" };
          //Create list of oids to update
          var oidSet = new List<long>();
          using (var rc = featureLayer.Search(queryFilter))
          {
            while (rc.MoveNext())
            {
              using var record = rc.Current;
              oidSet.Add(record.GetObjectID());
            }
          }
          //create and execute the edit operation
          var modifyFeatures = new EditOperation
          {
            Name = "Modify features",
            ShowProgressor = true
          };

          var multipleFeaturesInsp = new Inspector();
          multipleFeaturesInsp.Load(featureLayer, oidSet);
          multipleFeaturesInsp["MOMC"] = 24;
          modifyFeatures.Modify(multipleFeaturesInsp);
          if (!modifyFeatures.IsEmpty)
          {
            var result = modifyFeatures.ExecuteAsync(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Move(ArcGIs.Desktop.Mapping.SelectionSet, System.Double, System.Double)
      #region Move features
      // Moves the shapes (geometries) of all selected features of a given feature layer of the active map view by a fixed offset.
      await QueuedTask.Run<bool>(() =>
        {
          double xOffset = 100; // specify your units along the x-axis to move the geometry
          double yOffset = 100; // specify your units along the y-axis to move the geometry
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
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(LAYER,INT64,GEOMETRY,DICTIONARY{STRING,OBJECT})
      #region Move feature to a specific coordinate
      // Moves the first selected feature in the specified feature layer to the given coordinates.
      await QueuedTask.Run<bool>(() =>
       {
         //Get all of the selected ObjectIDs from the layer.
         var mySelection = featureLayer.GetSelection();
         var selOid = mySelection.GetObjectIDs()?[0];
         var xCoordinate = 0.0;
         var yCoordinate = 0.0; // specify the target coordinates to move the geometry
         var moveToPoint = new MapPointBuilderEx(xCoordinate, yCoordinate, 0.0, 0.0, featureLayer.GetSpatialReference());

         var moveEditOperation = new EditOperation() { Name = "Move features" };
         moveEditOperation.Modify(featureLayer, selOid ?? -1, moveToPoint.ToGeometry());  //Modify the feature to the new geometry 
         if (!moveEditOperation.IsEmpty)
         {
           var result = moveEditOperation.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
           return result; // return the operation result: true if successful, false if not
         }
         return false; // return false to indicate that the operation was not empty
       });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,IEnumerable{Int64},Nullable{Double})
      #region Edit Operation Planarize Features
      // Planarizes the specified feature in the given feature layer.
      await QueuedTask.Run(() =>
        {
          // note - EditOperation.Planarize requires a standard license. 
          //  An exception will be thrown if Pro is running under a basic license. 

          var planarizeFeatures = new EditOperation() { Name = "Planarize Features" };

          // Planarize one or more features
          planarizeFeatures.Planarize(featureLayer, [objectId]);

          // Execute to execute the operation
          // Must be called within QueuedTask.Run
          if (!planarizeFeatures.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = planarizeFeatures.Execute();
            //or use async flavor
            //await planarizeFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.ParallelOffset.Builder.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.ParallelOffset.Builder)
      #region Edit Operation ParallelOffset
      // Creates parallel offset features from the selected features.
      await QueuedTask.Run(static () =>
        {
          //Create parallel features from the selected features

          //find the roads layer
          var roadsLayer = MapView.Active.Map.FindLayers("Roads")?[0];

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
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Reshape(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
      #region Edit Operation Reshape Features
      // Reshapes the specified feature in the given feature layer.
      await QueuedTask.Run(() =>
        {
          var reshapeFeatures = new EditOperation() { Name = "Reshape Features" };

          reshapeFeatures.Reshape(featureLayer, objectId, modifyLine);

          //Reshape a set of features that intersect some geometry....

          reshapeFeatures.Reshape(MapView.Active.GetFeatures(modifyLine), modifyLine);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!reshapeFeatures.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = reshapeFeatures.Execute();
            //or use async flavor
            //await reshapeFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Rotate(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double)
      #region Edit Operation Rotate Features
      // Rotates the selected features by a specified angle.
      await QueuedTask.Run(() =>
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
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = rotateFeatures.Execute();
            //or use async flavor
            //await rotateFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Scale(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double, System.Double, System.Double)
      #region Edit Operation Scale Features
      // Scales the selected features by a specified factor.
      await QueuedTask.Run(() =>
        {
          var scaleFeatures = new EditOperation() { Name = "Scale Features" };
          var scale = 2.0; // specify the scale factor for x and y

          //Rotate works on a selected set of features
          //Scale the selected features by scale in the X and Y direction
          scaleFeatures.Scale(MapView.Active.GetFeatures(polygon), origin, scale, scale, 0.0);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!scaleFeatures.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = scaleFeatures.Execute();
            //or use async flavor
            //await scaleFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.SplitByPercentage.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByEqualParts.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByDistance.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByVaryingDistance.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, System.Collections.Generic.IEnumerable<ArcGID.Core.Geometry.MapPoint>)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Desktop.Editing.SplitMethod)
      #region Edit Operation Split Features
      // Splits the specified feature at the given points.
      await QueuedTask.Run(() =>
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
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = splitFeatures.Execute();
            //or use async flavor
            //await splitAtPointsFeatures.ExecuteAsync();
          }
        });
      // Splits a feature in the specified feature layer based on a given percentage and object ID.
      await QueuedTask.Run(() =>
      {
        var percentage = 25.0;

        //Split features using EditOperation.Split overloads
        var splitFeatures = new EditOperation() { Name = "Split Features" };

        // split using percentage
        var splitByPercentage = new SplitByPercentage() { Percentage = percentage, SplitFromStartPoint = true };
        splitFeatures.Split(featureLayer, objectId, splitByPercentage);

        //Execute to execute the operation
        //Must be called within QueuedTask.Run
        if (!splitFeatures.IsEmpty)
        {
          //Execute and ExecuteAsync will return true if the operation was successful and false if not
          var result = splitFeatures.Execute();
          //or use async flavor
          //await splitAtPointsFeatures.ExecuteAsync();
        }
      });
      // Splits a feature in the specified feature layer into the specified number of parts.
      await QueuedTask.Run(() =>
        {
          var numParts = 3;
          // split using equal parts
          //Split features using EditOperation.Split overloads
          var splitFeatures = new EditOperation() { Name = "Split Features" };
          var splitByEqualParts = new SplitByEqualParts() { NumParts = numParts };
          splitFeatures.Split(featureLayer, objectId, splitByEqualParts);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!splitFeatures.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = splitFeatures.Execute();
            //or use async flavor
            //await splitAtPointsFeatures.ExecuteAsync();
          }
        });
      // Splits a feature in the specified feature layer at a given distance and object ID.
      await QueuedTask.Run(() =>
        {
          var distance = 150.0;

          //Split features using EditOperation.Split overloads
          var splitFeatures = new EditOperation() { Name = "Split Features" };

          // split using single distance
          var splitByDistance = new SplitByDistance() { Distance = distance, SplitFromStartPoint = false };
          splitFeatures.Split(featureLayer, objectId, splitByDistance);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!splitFeatures.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = splitFeatures.Execute();
            //or use async flavor
            //await splitAtPointsFeatures.ExecuteAsync();
          }

        });
      // Splits a feature in the specified feature layer at a given distance and object ID.
      await QueuedTask.Run(() =>
       {
         var distances = new List<double> { 100.0, 200.0, 50.0 };
         //Split features using EditOperation.Split overloads
         var splitFeatures = new EditOperation() { Name = "Split Features" };

         // split using varying distance
         var splitByVaryingDistance = new SplitByVaryingDistance() { Distances = distances, SplitFromStartPoint = true, ProportionRemainder = true };
         splitFeatures.Split(featureLayer, objectId, splitByVaryingDistance);

         //Execute to execute the operation
         //Must be called within QueuedTask.Run
         if (!splitFeatures.IsEmpty)
         {
           //Execute and ExecuteAsync will return true if the operation was successful and false if not
           var result = splitFeatures.Execute();
           //or use async flavor
           //await splitAtPointsFeatures.ExecuteAsync();
         }
       });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64)
      // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
      // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.Collections.Generic.Dictionary{System.String,System.String})
      #region Edit Operation: Transfer Attributes
      // Transfers attributes from a source feature to a target feature between specified layers.
      await QueuedTask.Run(() =>
        {
          var targetOID = 12345; // object ID of the target feature in the destination layer
          var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };

          // transfer attributes using the stored field mapping
          transferAttributes.TransferAttributes(featureLayer, objectId, destinationLayer, targetOID);

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!transferAttributes.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = transferAttributes.Execute();
            //or use async flavor
            //await transferAttributes.ExecuteAsync();
          }
        });
      // Transfers attributes from a source feature to a target feature between specified layers.
      await QueuedTask.Run(() =>
        {
          var targetOID = 12345; // object ID of the target feature in the destination layer
          var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };
          // transfer attributes using an auto-match on the attributes
          transferAttributes.TransferAttributes(featureLayer, objectId, destinationLayer, targetOID, "");

          //Execute to execute the operation
          //Must be called within QueuedTask.Run
          if (!transferAttributes.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = transferAttributes.Execute();
            //or use async flavor
            //await transferAttributes.ExecuteAsync();
          }
        });
      // Transfers attributes from a source feature to a target feature in a specified destination layer.
      await QueuedTask.Run(() =>
       {
         var targetOID = 12345; // object ID of the target feature in the destination layer
         var transferAttributes = new EditOperation() { Name = "Transfer Attributes" };
         // transfer attributes using a specified set of field mappings
         //  dictionary key is the field name in the destination layer, dictionary value is the field name in the source layer
         Dictionary<string, string> fldMapping = new()         {
          { "NAME", "SURNAME" },
          { "ADDRESS", "ADDRESS" }
         };
         transferAttributes.TransferAttributes(featureLayer, objectId, destinationLayer, targetOID, fldMapping);

         //Execute to execute the operation
         //Must be called within QueuedTask.Run
         if (!transferAttributes.IsEmpty)
         {
           //Execute and ExecuteAsync will return true if the operation was successful and false if not
           var result = transferAttributes.Execute();
           //or use async flavor
           //await transferAttributes.ExecuteAsync();
         }
       });
      // Transfers attributes from a source feature to a target feature in a specified destination layer.
      await QueuedTask.Run(() =>
        {
          var targetOID = 12345; // object ID of the target feature in the destination layer
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
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = transferAttributes.Execute();
            //or use async flavor
            //await transferAttributes.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.TransformByLinkLayer.#ctor
      // cref: ArcGIS.Desktop.Editing.TransformMethodType
      // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.Layer,ArcGIS.Desktop.Editing.TransformMethod)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.SelectionSet,ArcGIS.Desktop.Editing.TransformMethod)
      #region Edit Operation Transform Features
      // Transforms features from a source layer to a target layer using a specified transformation method.
      await QueuedTask.Run(() =>
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
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = transformFeatures.Execute();
            //or use async flavor
            //await transformFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.RubbersheetByGeometries.#ctor
      // cref: ArcGIS.Desktop.Editing.RubbersheetByLayers.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Rubbersheet(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Editing.RubbersheetMethod)
      #region Edit Operation Rubbersheet Features
      // Performs a rubber-sheet transformation on the features of a specified layer using the provided link lines, anchor points, and limited adjustment areas.
      await QueuedTask.Run(() =>
      {
        //Perform rubber-sheet by geometries
        var rubbersheetMethod = new RubbersheetByGeometries()
        {
          RubbersheetType = RubbersheetMethodType.Linear, //The RubbersheetType can be Linear of NearestNeighbor
          LinkLines = linkLines, //IEnumerable list of link lines (polylines)
          AnchorPoints = anchorPoints, //IEnumerable list of anchor points (map points)
          LimitedAdjustmentAreas = limitedAdjustmentAreas //IEnumerable list of limited adjustment areas (polygons)
        };

        var rubbersheetOp = new EditOperation();
        //Performs linear rubber-sheet transformation on the features belonging to "layer" that fall within the limited adjustment areas
        rubbersheetOp.Rubbersheet(layer, rubbersheetMethod);
        //Execute the operation
        if (!rubbersheetOp.IsEmpty)
        {
          var result = rubbersheetOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }

        //Alternatively, you can also perform rubber-sheet by layer
        var rubbersheetMethod2 = new RubbersheetByLayers()
        {
          RubbersheetType = RubbersheetMethodType.NearestNeighbor, //The RubbersheetType can be Linear of NearestNeighbor
          LinkLayer = linkLayer,
          AnchorPointLayer = anchorPointsLayer,
          LimitedAdjustmentAreaLayer = limitedAdjustmentAreaLayer
        };

        //Performs nearest neighbor rubber-sheet transformation on the features belonging to "layer" that fall within the limited adjustment areas
        rubbersheetOp.Rubbersheet(layer, rubbersheetMethod2);
        if (!rubbersheetOp.IsEmpty)
        {
          //Execute and ExecuteAsync will return true if the operation was successful and false if not
          var result = rubbersheetOp.Execute();
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,Int64,Nullable{Double})
      #region Edit Operation Perform a Clip, Cut, and Planarize
      // Performs a sequence of editing operations on a feature, including clipping, cutting, and planarizing.
      await QueuedTask.Run(() =>
        {
          //Multiple operations can be performed by a single
          //edit operation.
          var clipCutPlanarizeFeatures = new EditOperation() { Name = "Clip, Cut, and Planarize Features" };
          clipCutPlanarizeFeatures.Clip(featureLayer, objectId, clipPolygon);
          clipCutPlanarizeFeatures.Split(featureLayer, objectId, cutLine);
          clipCutPlanarizeFeatures.Planarize(featureLayer, objectId);

          if (!clipCutPlanarizeFeatures.IsEmpty)
          {
            //Note: An edit operation is a single transaction. 
            //Execute the operations (in the order they were declared)
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            clipCutPlanarizeFeatures.Execute();
            //or use async flavor
            //await clipCutPlanarizeFeatures.ExecuteAsync();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.CreateChainedOperation
      // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
      #region Edit Operation Chain Edit Operations
      // Chains multiple edit operations into a single undo-able transaction.
      await QueuedTask.Run(() =>
        {
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
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.RowToken
      // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Editing.RowToken,System.String)
      #region Edit Operation add attachment via RowToken
      // Creates a new feature using the specified editing template and polygon geometry, and adds an attachment to the feature in a single edit operation.
      await QueuedTask.Run(() =>
        {
          //The EditOperation.AddAttachment method to take a RowToken as a parameter 
          //allows you to create a feature, using EditOperation.CreateEx, and add an attachment in one transaction.

          EditOperation editOpAttach = new()
          {
            Name = string.Format($@"Create new polygon with attachment in '{currentTemplate.Layer.Name}'")
          };

          var attachRowToken = editOpAttach.Create(currentTemplate, polygon);
          editOpAttach.AddAttachment(attachRowToken, @"c:\temp\image.jpg");

          //Must be within a QueuedTask
          if (!editOpAttach.IsEmpty)
          {
            //Execute and ExecuteAsync will return true if the operation was successful and false if not
            var result = editOpAttach.Execute();
          }
        });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteMode
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Core.Data.Row,System.String, System.object)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
      #region Order edits sequentially
      // Modifies the "NAME" attribute of a specified feature and then splits the feature using the provided polyline, all within a single sequential edit operation.
      // perform an edit and then a split as one operation.
      await QueuedTask.Run(() =>
       {
         var newName = "Modified then Split";
         var queryFilter = new QueryFilter() { WhereClause = "OBJECTID = " + objectId.ToString() };

         // create an edit operation and name.
         var op = new EditOperation
         {
           Name = "modify followed by split",         // set the ExecuteMode
           ExecuteMode = ExecuteModeType.Sequential
         };
         using (var rowCursor = layer.Search(queryFilter))
         {
           while (rowCursor.MoveNext())
           {
             using var feature = rowCursor.Current as ArcGIS.Core.Data.Feature;
             op.Modify(feature, "NAME", newName);
           }
         }
         op.Split(layer, objectId, splitLine);
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
      // Demonstrates how to use the <see cref="ArcGIS.Desktop.Editing.EditOperation"/> methods  <see
      await QueuedTask.Run(() =>
        {
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
          updateTestField.Modify(testInspector);

          //actions for SetOn...
          updateTestField.SetOnUndone(() =>
          {
            //Sets an action that will be called when this operation is undone.
            Debug.WriteLine("Operation is undone");
          });

          updateTestField.SetOnRedone(() =>
          {
            //Sets an action that will be called when this edit operation is redone.
            Debug.WriteLine("Operation is redone");
          });

          updateTestField.SetOnComitted(b =>
          {
            // Sets an action that will be called when this edit operation is committed.
            Debug.WriteLine("Operation is committed");
          });

          if (!updateTestField.IsEmpty)
          {
            var result = updateTestField.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          }
        });
      #endregion


      //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.HasID
      //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.ID
      #region Convert vertices in a polyline to a Control Point
      //Control points are special vertices used to apply symbol effects to line or polygon features.
      //By default, they appear as diamonds when you edit them.
      //They can also be used to migrate representations from ArcMap to features in ArcGIS Pro.
      await QueuedTask.Run(() =>
      {
        var changeVertexIDOperation = new EditOperation();
        //iterate through the points in the polyline.
        var lineLayerCursor = lineLayer.GetSelection().Search();
        var lineVertices = new List<MapPoint>();
        long objectId = -1;
        while (lineLayerCursor.MoveNext())
        {
          var lineFeature = lineLayerCursor.Current as ArcGIS.Core.Data.Feature;
          var line = lineFeature.GetShape() as Polyline;
          int vertexIndex = 1;
          objectId = lineFeature.GetObjectID();
          //Each point is converted into a MapPoint and gets added to a list. 
          foreach (var point in line.Points)
          {
            MapPointBuilderEx mapPointBuilderEx = new(point);
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
        changeVertexIDOperation.Modify(lineLayer, objectId, newLine);
        changeVertexIDOperation.Execute();
      });
    }
    #endregion


  }
}
