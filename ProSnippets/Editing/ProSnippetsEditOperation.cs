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

    // cref: ArcGIS.Desktop.Editing.EditOperation.IsEmpty
    // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
    // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
    #region Edit Operation - check for actions before Execute
    /// <summary>
    /// Checks for pending actions in an edit operation before attempting to execute it.
    /// </summary>
    /// <remarks>This method creates an edit operation and adds a modification action to it. Before executing
    /// the operation, it checks whether the operation contains any actions to perform. If the operation is empty (i.e.,
    /// no changes are required), the execution is skipped to avoid failure. This ensures that unnecessary execution
    /// attempts are avoided.</remarks>
    /// <param name="featureLayer">The feature layer containing the feature to be modified.</param>
    /// <param name="objectId">The object ID of the feature to be modified.</param>
    /// <param name="geometry">The new geometry to apply to the feature.</param>
    public static void CheckForActionsBeforeExecute(FeatureLayer featureLayer, long objectId, Geometry geometry)
    {
      _= QueuedTask.Run(() =>
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.#ctor()
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Core.Geometry.Geometry)
    // cref: ArcGIS.Desktop.Editing.EditOperation.IsSucceeded
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
    // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
    // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
    #region Edit Operation Create Features
    /// <summary>
    /// Creates new features in a feature layer using the specified geometry, attributes, or editing template.
    /// </summary>
    /// <remarks>This method demonstrates multiple ways to create features in a feature layer: <list
    /// type="bullet"> <item> <description>Creating a feature directly using a geometry.</description> </item> <item>
    /// <description>Creating a feature with specified attributes, including geometry and other field
    /// values.</description> </item> <item> <description>Creating a feature using an editing template, which must be
    /// executed within a <see cref="ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run"/> context.</description>
    /// </item> </list> After defining the features to create, the operation must be executed using <see
    /// cref="ArcGIS.Desktop.Editing.EditOperation.Execute"/> or <see
    /// cref="ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync"/>.  The operation will succeed only if it is executed
    /// within a valid editing context.</remarks>
    /// <param name="currentTemplate">The editing template to use for creating features. This is required when creating features based on a predefined
    /// template.</param>
    /// <param name="polygon">The geometry of the feature to create. Typically, this is a polygon representing the spatial shape of the
    /// feature.</param>
    /// <param name="featureLayer">The feature layer where the new features will be created.</param>
    public static void CreateFeatures(EditingTemplate currentTemplate, Geometry polygon, FeatureLayer featureLayer)
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
        createFeatures.Execute(); //Execute will return true if the operation was successful and false if not.
      }

      //or use async flavor
      //await createFeatures.ExecuteAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate, ArcGIS.Core.Geometry.Geometry)
    #region Create a feature using the current template
    /// <summary>
    /// Creates a new feature using the current editing template and the specified geometry.
    /// </summary>
    /// <remarks>This method uses the current editing template to create a feature. The operation is executed
    /// synchronously, and the success of the operation can be determined by the internal execution result.</remarks>
    /// <param name="editingTemplate">The editing template to use for creating the feature. This parameter is not directly used in the method but is
    /// included for consistency with the editing workflow.</param>
    /// <param name="geometry">The geometry of the feature to be created. This must be a valid geometry object and cannot be <see
    /// langword="null"/>.</param>
    public static void CreateFeatureUsingCurrentTemplate(EditingTemplate editingTemplate, Geometry geometry)
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
    public static void CreateFeatureFromModifiedInspector(FeatureLayer layer, long objectId)
    {
      //Create an inspector and load a feature
      //The inspector is used to modify the attributes of the feature
      //before creating it
      var insp = new Inspector();
      insp.Load(layer, objectId);

      QueuedTask.Run(() =>
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
    }


    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
    #region Create features from a CSV file
    public static void CreateFeaturesFromCSV(FeatureLayer featureLayer)
    {
      var csvData = new List<CSVData>();
      //Run on MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
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
    }
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate)
      #region Edit Operation Create row in a table using a table template
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

      #endregion
    }

    // cref: ArcGIS.Desktop.Editing.EditOperation.Clip(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Editing.ClipMode)
    #region Edit Operation Clip Features
    /// <summary>
    /// Clips a feature to the specified polygon.
    /// </summary>
    /// <param name="featureLayer">The feature layer containing the feature to clip.</param>
    /// <param name="objectId">The object ID of the feature to clip.</param>
    /// <param name="clipPoly">The polygon to use as the clipping boundary.</param>
    public static void ClipFeatures(FeatureLayer featureLayer, long objectId, Geometry clipPolygon)
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
    #region Edit Operation Cut Features
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = cutFeatures.Execute();

        //or use async flavor
        //await cutFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.SelectionSet)
    #region Edit Operation Delete Features
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
      { //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = deleteFeatures.Execute();
        //or use async flavor
        //await deleteFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATE(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
    // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATECHAINEDOPERATION
    #region Edit Operation Duplicate Features
    ///<summary>
    ///Duplicates a feature in a feature layer and modifies its geometry.
    ///</summary>
    /// <param name="featureLayer">The feature layer containing the feature to duplicate.</param>
    /// <param name="objectId">The ObjectID of the feature to duplicate.</param>
    /// <param name="polygon">The polygon defining the new geometry for the duplicated feature.</param>
    public static void DuplicateFeature(FeatureLayer featureLayer, long objectId, Geometry polygon)
    {
      var duplicateFeatures = new EditOperation() { Name = "Duplicate Features" };

        //Duplicate with an X and Y offset of 500 map units
        //At 2.x duplicateFeatures.Duplicate(featureLayer, objectId, 500.0, 500.0, 0.0);

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
    #region Edit Operation Explode Features
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
    #region Edit Operation Merge Features
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
      //At 2.x -
      //mergeFeatures.Merge(this.CurrentTemplate as EditingFeatureTemplate, featureLayer, new List<long>() { 10, 96, 12 });
      mergeFeatures.Merge(currentTemplate as EditingRowTemplate, featureLayer, new List<long>() { 10, 96, 12 });

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
    #region Edit Operation Modify single feature
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
    #region Edit Operation Modify multiple features
    /// <summary>
    /// Modifies multiple features in the specified feature layer by updating their attributes.
    /// </summary>
    /// <remarks>This method performs an attribute-based query to identify features with an OBJECTID less than
    /// 1,000,000. It then updates the "MOMC" attribute of the selected features to a value of 24.  The operation is
    /// executed as an edit operation, which supports undo and redo functionality.</remarks>
    /// <param name="featureLayer">The <see cref="FeatureLayer"/> containing the features to be modified.  This parameter cannot be null.</param>
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
    #region Edit Operation Planarize Features
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = planarizeFeatures.Execute();
        //or use async flavor
        //await planarizeFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.ParallelOffset.Builder.#ctor
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.ParallelOffset.Builder)
    #region Edit Operation ParallelOffset
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
    #region Edit Operation Reshape Features
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = reshapeFeatures.Execute(); 
        //or use async flavor
        //await reshapeFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Rotate(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double)
    #region Edit Operation Rotate Features
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = rotateFeatures.Execute();
        //or use async flavor
        //await rotateFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Scale(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double, System.Double, System.Double)
    #region Edit Operation Scale Features
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = scaleFeatures.Execute();
        //or use async flavor
        //await scaleFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.SplitByPercentage.#ctor
    // cref: ArcGIS.Desktop.Editing.SplitByEqualParts.#ctor
    // cref: ArcGIS.Desktop.Editing.SplitByDistance.#ctor
    // cref: ArcGIS.Desktop.Editing.SplitByVaryingDistance.#ctor
    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, System.Collections.Generic.IEnumerable<ArcGID.Core.Geometry.MapPoint>)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Desktop.Editing.SplitMethod)
    #region Edit Operation Split Features
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = splitFeatures.Execute();
        //or use async flavor
        //await splitAtPointsFeatures.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64)
    // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
    // cref: ArcGIS.Desktop.Editing.EditOperation.TransferAttributes(ArcGIS.Desktop.Mapping.MapMember,System.Int64,ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.Collections.Generic.Dictionary{System.String,System.String})
    #region Edit Operation: Transfer Attributes
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = transferAttributes.Execute();
        //or use async flavor
        //await transferAttributes.ExecuteAsync();
      }
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = transferAttributes.Execute();
        //or use async flavor
        //await transferAttributes.ExecuteAsync();
      }
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = transferAttributes.Execute();
        //or use async flavor
        //await transferAttributes.ExecuteAsync();
      }
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = transferAttributes.Execute();
        //or use async flavor
        //await transferAttributes.ExecuteAsync();
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.TransformByLinkLayer.#ctor
    // cref: ArcGIS.Desktop.Editing.TransformMethodType
    // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.Layer,ArcGIS.Desktop.Editing.TransformMethod)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.SelectionSet,ArcGIS.Desktop.Editing.TransformMethod)
    #region Edit Operation Transform Features
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
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = transformFeatures.Execute(); 
        //or use async flavor
        //await transformFeatures.ExecuteAsync();
      }
    }

    #endregion

    // cref: ArcGIS.Desktop.Editing.RubbersheetByGeometries.#ctor
    // cref: ArcGIS.Desktop.Editing.RubbersheetByLayers.#ctor
    // cref: ArcGIS.Desktop.Editing.EditOperation.Rubbersheet(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Editing.RubbersheetMethod)
    #region Edit Operation Rubbersheet Features
    /// <summary>
    /// Performs a rubber-sheet transformation on the features of a specified layer using the provided link lines,
    /// anchor points, and limited adjustment areas.
    /// </summary>
    /// <remarks>This method supports two types of rubber-sheet transformations:  <list type="bullet">
    /// <item><description>Linear transformation, which uses link lines and anchor points.</description></item>
    /// <item><description>Nearest neighbor transformation, which uses feature layers for link lines and anchor
    /// points.</description></item> </list> The transformation is applied only to the features in the specified
    /// <paramref name="layer"/> that fall within the limited adjustment areas.</remarks>
    /// <param name="layer">The feature layer containing the features to be transformed.</param>
    /// <param name="linkLayer">The feature layer containing link lines used for the transformation. This parameter is optional and can be null
    /// if <paramref name="linkLines"/> is provided.</param>
    /// <param name="linkLines">A collection of polylines representing the link lines used for the transformation. This parameter is optional
    /// and can be null if <paramref name="linkLayer"/> is provided.</param>
    /// <param name="anchorPointsLayer">The feature layer containing anchor points used for the transformation. This parameter is optional and can be
    /// null if <paramref name="anchorPoints"/> is provided.</param>
    /// <param name="anchorPoints">A collection of map points representing the anchor points used for the transformation. This parameter is
    /// optional and can be null if <paramref name="anchorPointsLayer"/> is provided.</param>
    /// <param name="limitedAdjustmentAreaLayer">The feature layer defining the limited adjustment areas where the transformation is applied. This parameter is
    /// optional and can be null if <paramref name="limitedAdjustmentAreas"/> is provided.</param>
    /// <param name="limitedAdjustmentAreas">A collection of polygons defining the limited adjustment areas where the transformation is applied. This
    /// parameter is optional and can be null if <paramref name="limitedAdjustmentAreaLayer"/> is provided.</param>
    public static void RubbersheetFeatures(FeatureLayer layer, FeatureLayer linkLayer,
      IEnumerable<Polyline> linkLines,
      FeatureLayer anchorPointsLayer,
      IEnumerable<MapPoint> anchorPoints,
      FeatureLayer limitedAdjustmentAreaLayer,
      IEnumerable<Polygon> limitedAdjustmentAreas)
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,Int64,Nullable{Double})
    #region Edit Operation Perform a Clip, Cut, and Planarize
    /// <summary>
    /// Performs a sequence of editing operations on a feature, including clipping, cutting, and planarizing.
    /// </summary>
    /// <remarks>This method combines multiple editing operations into a single transaction. The operations
    /// are executed in the following order: clip, cut, and planarize. If any operation fails, the entire transaction is
    /// rolled back. <para> The <see cref="EditOperation"/> ensures that all operations are performed atomically,
    /// meaning either all operations succeed or none are applied. Use this method to modify features in scenarios where
    /// multiple geometric transformations are required. </para> <para> Note: This method executes the edit operation
    /// synchronously. For asynchronous execution, use the <see cref="EditOperation.ExecuteAsync"/> method instead.
    /// </para></remarks>
    /// <param name="featureLayer">The feature layer containing the feature to be edited.</param>
    /// <param name="objectId">The object ID of the feature to be edited.</param>
    /// <param name="clipPoly">The polygon used to clip the feature. Must not be null.</param>
    /// <param name="cutLine">The polyline used to cut the feature. Must not be null.</param>
    public static void ClipCutAndPlanarizeFeatures(FeatureLayer featureLayer, long objectId, Polygon clipPoly, Polyline cutLine)
    {
      //Multiple operations can be performed by a single
      //edit operation.
      var clipCutPlanarizeFeatures = new EditOperation() { Name = "Clip, Cut, and Planarize Features" };
      clipCutPlanarizeFeatures.Clip(featureLayer, objectId, clipPoly);
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.CreateChainedOperation
    // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
    #region Edit Operation Chain Edit Operations
    /// <summary>
    /// Chains multiple edit operations into a single undoable transaction.
    /// </summary>
    /// <remarks>This method demonstrates how to chain edit operations, allowing multiple transactions to be
    /// grouped into a single undoable action. A common use case is creating a feature and then adding an attachment to
    /// it. Since adding an attachment requires the object ID of the newly created feature, the operations must be
    /// executed in sequence. By chaining the operations, they appear as a single undo action in the user
    /// interface.</remarks>
    /// <param name="featureLayer">The feature layer where the edit operations will be performed.</param>
    /// <param name="currentTemplate">The editing template used to create the new feature.</param>
    /// <param name="polygon">The geometry of the feature to be created.</param>
    /// <param name="objectId">The object ID of the feature to be edited or referenced.</param>
    public static void ChainEditOperations(FeatureLayer featureLayer,
      EditingTemplate currentTemplate, Polygon polygon, long objectId)
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.RowToken
    // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Editing.RowToken,System.String)
    #region Edit Operation add attachment via RowToken
    /// <summary>
    /// Creates a new feature using the specified editing template and polygon geometry, and adds an attachment to the
    /// feature in a single edit operation.
    /// </summary>
    /// <remarks>This method demonstrates the use of the <see
    /// cref="ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Editing.RowToken, string)"/> method, 
    /// which allows adding an attachment to a feature created within the same edit operation. The attachment is added
    /// using a file path. <para> The operation must be executed within a <see
    /// cref="ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask"/> to ensure thread safety. </para> <para> The method
    /// uses <see cref="ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.EditingTemplate,
    /// ArcGIS.Core.Geometry.Geometry)"/>  to create the feature and obtain a <see
    /// cref="ArcGIS.Desktop.Editing.RowToken"/> for the newly created feature. </para></remarks>
    /// <param name="currentTemplate">The editing template used to create the new feature. This must be associated with a valid layer.</param>
    /// <param name="polygon">The polygon geometry used to define the new feature.</param>
    public static void AddAttachmentViaRowToken(EditingTemplate currentTemplate, Polygon polygon)
    {
      //ArcGIS Pro 2.5 extends the EditOperation.AddAttachment method to take a RowToken as a parameter.
      //This allows you to create a feature, using EditOperation.CreateEx, and add an attachment in one transaction.

      var editOpAttach = new EditOperation();
      editOpAttach.Name = string.Format("Create new polygon with attachment in '{0}'", currentTemplate.Layer.Name);

      var attachRowToken = editOpAttach.Create(currentTemplate, polygon);
      editOpAttach.AddAttachment(attachRowToken, @"c:\temp\image.jpg");

      //Must be within a QueuedTask
      if (!editOpAttach.IsEmpty)
      {
        //Execute and ExecuteAsync will return true if the operation was successful and false if not
        var result = editOpAttach.Execute();
      }
    }
    #endregion


    // cref: ArcGIS.Desktop.Editing.EditOperation
    // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteMode
    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Core.Data.Row,System.String, System.object)
    // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
    #region Order edits sequentially
    /// <summary>
    /// Modifies the specified feature's attribute and then splits it using the provided geometry as part of a single
    /// edit operation.
    /// </summary>
    /// <remarks>This method performs the modification and split as a single sequential edit operation.  The
    /// operation is executed on the QueuedTask context to ensure thread safety in ArcGIS Pro.</remarks>
    /// <param name="layer">The feature layer containing the feature to be modified and split.</param>
    /// <param name="objectId">The object ID of the feature to be modified and split.</param>
    /// <param name="splitLine">The polyline geometry used to split the feature.</param>
    /// <param name="newName">The new value to assign to the feature's "NAME" attribute.</param>
    public static void ModifyAndSplitFeature(FeatureLayer layer, long objectId, Polyline splitLine, string newName)
    {
      // perform an edit and then a split as one operation.
      QueuedTask.Run(() =>
      {
        var queryFilter = new QueryFilter() { WhereClause = "OBJECTID = " + objectId.ToString() };

        // create an edit operation and name.
        var op = new EditOperation() { Name = "modify followed by split" };
        // set the ExecuteMode
        op.ExecuteMode = ExecuteModeType.Sequential;
        using (var rowCursor = layer.Search(queryFilter))
        {
          while (rowCursor.MoveNext())
          {
            using (var feature = rowCursor.Current as Feature)
            {
              op.Modify(feature, "NAME", newName);
            }
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
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnUndone(System.Action)
    // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnComitted(System.Action<System.Boolean>)
    // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnRedone(System.Action)
    #region SetOnUndone, SetOnRedone, SetOnComitted
    /// <summary>
    /// Demonstrates how to use the <see cref="ArcGIS.Desktop.Editing.EditOperation"/> methods  <see
    /// cref="ArcGIS.Desktop.Editing.EditOperation.SetOnUndone(System.Action)"/>,  <see
    /// cref="ArcGIS.Desktop.Editing.EditOperation.SetOnRedone(System.Action)"/>, and  <see
    /// cref="ArcGIS.Desktop.Editing.EditOperation.SetOnComitted(System.Action{System.Boolean})"/>  to manage external
    /// actions associated with an edit operation.
    /// </summary>
    /// <remarks>This method provides an example of how to set actions that are triggered when an edit
    /// operation  is undone, redone, or committed. These actions can be used to perform external tasks, such as 
    /// logging or updating related systems, in response to changes in the edit operation's state.</remarks>
    public static void SetOnUndoneRedoneComitted()
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

      updateTestField.SetOnComitted((bool b) => //called on edit session save(true)/discard(false).
      {
        // Sets an action that will be called when this edit operation is committed.
        Debug.WriteLine("Operation is committed");
      });

      if (!updateTestField.IsEmpty)
      {
        var result = updateTestField.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
    }
    #endregion


    //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.HasID
    //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.ID
    #region Convert vertices in a polyline to a Control Point

    public static void ConvertVerticesToControlPoints(FeatureLayer lineLayer)
    {
      //Control points are special vertices used to apply symbol effects to line or polygon features.
      //By default, they appear as diamonds when you edit them.
      //They can also be used to migrate representations from ArcMap to features in ArcGIS Pro.
      QueuedTask.Run(() =>
      {
        var changeVertexIDOperation = new EditOperation();
        //iterate through the points in the polyline.
        var lineLayerCursor = lineLayer.GetSelection().Search();
        var lineVertices = new List<MapPoint>();
        long objectId = -1;
        while (lineLayerCursor.MoveNext())
        {
          var lineFeature = lineLayerCursor.Current as Feature;
          var line = lineFeature.GetShape() as Polyline;
          int vertexIndex = 1;
          objectId = lineFeature.GetObjectID();
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
        changeVertexIDOperation.Modify(lineLayer, objectId, newLine);
        changeVertexIDOperation.Execute();
      });
    }
      #endregion


  }
}
