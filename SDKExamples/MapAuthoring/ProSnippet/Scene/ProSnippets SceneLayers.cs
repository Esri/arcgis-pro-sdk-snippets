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
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippets.MapAuthoringSnippets.Scene
{
  /// <summary>
  /// This class demonstrates various operations on scene layers within an ArcGIS Pro map view.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with Scenes in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsMapAuthoring
  {
    /// <summary>
    /// Demonstrates various operations on scene layers within an ArcGIS Pro map view.
    /// </summary>
    /// <remarks>This method includes examples of creating, querying, and modifying different types of scene
    /// layers such as  <see cref="BuildingSceneLayer"/>, <see cref="FeatureSceneLayer"/>, and <see
    /// cref="PointCloudSceneLayer"/>. It covers operations like setting filters, editing attributes, and managing
    /// renderers.  The method is intended to be run within the context of a queued task in ArcGIS Pro.</remarks>
    public static void ProSnippetsScenes()
    {
      #region ignore - Variable initialization
      var sceneLayerUrl = @"https://myportal.com/server/rest/services/Hosted/SceneLayerServiceName/SceneServer";
      //portal items also ok as long as the portal is the current active portal...
      //var sceneLayerUrl = @"https://myportal.com/home/item.html?id=123456789abcdef1234567890abcdef0";
      BuildingSceneLayer bsl = MapView.Active.Map.GetLayersAsFlattenedList()
              .OfType<BuildingSceneLayer>().FirstOrDefault();
      FilterDefinition filter = new FilterDefinition();

      FeatureSceneLayer featSceneLayer;
      long oid = -1;
      MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint(0, 0, 0, SpatialReferences.WGS84);

      string filterID = ""; //populate with a valid filter ID
      Geometry geometry = null; //populate with a valid geometry

      PointCloudSceneLayer pointCloudSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
              .OfType<PointCloudSceneLayer>().FirstOrDefault();
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(LayerCreationParams,ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(Uri)
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create a Scene Layer
      {
        // Note: call within QueuedTask.Run()
        {
          //Create with initial visibility set to false. Add to current scene
          var createparams = new LayerCreationParams(new Uri(sceneLayerUrl, UriKind.Absolute))
          {
            IsVisible = false
          };

          //cast to specific type of scene layer being created - in this case FeatureSceneLayer
          var sceneLayer = LayerFactory.Instance.CreateLayer<Layer>(
                   createparams, MapView.Active.Map) as FeatureSceneLayer;
          //or...specify the cast directly
          var sceneLayer2 = LayerFactory.Instance.CreateLayer<FeatureSceneLayer>(
                   createparams, MapView.Active.Map);
          //ditto for BuildingSceneLayer, PointCloudSceneLayer, IntegratedMeshSceneLayer
          //...
        }
      }
      #endregion

      #region ProSnippet Group: Building Discipline Scene Layer
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingDisciplineSceneLayer
      // cref: ArcGIS.Desktop.Mapping.BuildingDisciplineSceneLayer.GetDiscipline
      #region Get BuildingDisciplineSceneLayer Discipline
      {
        var bsl_discipline = MapView.Active.Map.GetLayersAsFlattenedList().OfType<BuildingDisciplineSceneLayer>().FirstOrDefault(l => l.Name == "Architectural");
        var disciplineName = bsl_discipline.GetDiscipline();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer
      // cref: ArcGIS.Desktop.Mapping.BuildingDisciplineSceneLayer
      // cref: ArcGIS.Desktop.Mapping.BuildingDisciplineSceneLayer.GetDiscipline
      // cref: ARCGIS.DESKTOP.MAPPING.COMPOSITELAYER.LAYERS
      // cref: ARCGIS.DESKTOP.MAPPING.COMPOSITELAYER.FINDLAYERS
      #region Enumerate the Discipline Layers from a Building SceneLayer
      {
        var bldgLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<BuildingSceneLayer>().First();
        var disciplines = new Dictionary<string, BuildingDisciplineSceneLayer>();
        //A Building layer has two children - Overview and FullModel
        //Overview is a FeatureSceneLayer
        //Full Model is a BuildingDisciplineSceneLayer that contains the disciplines

        var fullModel = bldgLayer.FindLayers("Full Model").First()
                                       as BuildingDisciplineSceneLayer;

        //collect information on the disciplines
        var name = fullModel.Name;

        var discipline = fullModel.GetDiscipline();
        //etc
        //TODO - use collected information

        disciplines.Add(discipline, fullModel);

        //Discipline layers are composite layers too
        foreach (var childDiscipline in fullModel.Layers
                            .OfType<BuildingDisciplineSceneLayer>())
        {
          //Discipline layers can also contain FeatureSceneLayers that render the
          //individual full model contents
          var content_names = string.Join(", ", childDiscipline.Layers
               .OfType<FeatureSceneLayer>().Select(fl => fl.Name));
          // Recursively call this "function" to go deeper
        }
      }
      #endregion

      #region ProSnippet Group: Building Scene Layer
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer
      // cref: ArcGIS.Desktop.Mapping.MapMember.Name
      #region Name of BuildingSceneLayer 
      {
        bsl = MapView.Active.Map.GetLayersAsFlattenedList()
              .OfType<BuildingSceneLayer>().FirstOrDefault();
        var scenelayerName = bsl.Name;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetAvailableFieldsAndValues
      #region Query Building Scene Layer for available Types and Values
      {
        // Note: call within QueuedTask.Run()
        {
          var dict = bsl.GetAvailableFieldsAndValues();

          //get a list of existing disciplines
          var disciplines = dict.SingleOrDefault(
                   kvp => kvp.Key == "Discipline").Value ?? new List<string>();

          //get a list of existing categories
          var categories = dict.SingleOrDefault(
                   kvp => kvp.Key == "Category").Value ?? new List<string>();

          //get a list of existing floors or "levels"
          var floors = dict.SingleOrDefault(
                   kvp => kvp.Key == "BldgLevel").Value ?? new List<string>();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.CreateDefaultFilter
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilters
      // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
      #region Create a Default Filter and Get Filter Count
      {
        // Note: call within QueuedTask.Run()
        {
          var filter1 = bsl.CreateDefaultFilter();
          var values = filter1.FilterBlockDefinitions[0].SelectedValues;
          //values will be a single value for the type
          //"CreatedPhase", value "New Construction"

          //There will be at least one filter after "CreateDefaultFilter()" call
          var filtersCount = bsl.GetFilters().Count;
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilters
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
      // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
      // cref: ArcGIS.Core.CIM.Object3DRenderingMode
      #region Get all the Filters that Contain WireFrame Blocks
      {
        // Note: call within QueuedTask.Run()
        {
          //Note: wire_frame_filters can be null in this example
          var wire_frame_filters = bsl.GetFilters().Where(
            f => f.FilterBlockDefinitions.Any(
              fb => fb.FilterBlockMode == Object3DRenderingMode.Wireframe));
          //substitute Object3DRenderingMode.None to get blocks with a solid mode (default)
          //and...
          //fb.FilterBlockMode == Object3DRenderingMode.Wireframe &&
          //fb.FilterBlockMode == Object3DRenderingMode.None
          //for blocks with both
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.HasFilter
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.SetActiveFilter
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetActiveFilter
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.ClearActiveFilter
      #region Set and Clear Active Filter for BuildingSceneLayer
      {
        // Note: call within QueuedTask.Run()
        {
          //Note: Use HasFilter to check if a given filter id exists in the layer
          if (bsl.HasFilter(filter.ID))
            bsl.SetActiveFilter(filter.ID);
          var activeFilter = bsl.GetActiveFilter();

          //Clear the active filter
          bsl.ClearActiveFilter();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilter
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
      #region Get BuildingSceneLayer Filter ID and Filter    
      {
        var filterDefinition = bsl.GetFilter(filterID);
        //or via Linq
        //var filter = bsl.GetFilters().FirstOrDefault(f => f.ID == filterID1);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Name
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Description
      #region Modify BuildingSceneLayer Filter Name and Description
      {
        // Note: call within QueuedTask.Run()
        {

          filter.Name = "Updated Filter Name";
          filter.Description = "Updated Filter description";
          bsl.UpdateFilter(filter);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetAvailableFieldsAndValues
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.SetActiveFilter
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Name
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Description
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
      // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
      // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.Title
      // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
      #region Create a Filter using Building Level and Category
      {
        // Note: call within QueuedTask.Run()
        {
          //refer to "Query Building Scene Layer for available Types and Values
          var dict = bsl.GetAvailableFieldsAndValues();
          var categories = dict.SingleOrDefault(kvp => kvp.Key == "Category").Value;
          //get a list of existing floors or "levels"
          var floors = dict.SingleOrDefault(kvp => kvp.Key == "BldgLevel").Value;

          //Make a new filter definition
          var fd = new FilterDefinition()
          {
            Name = "Floor and Category Filter",
            Description = "Example filter",
          };
          //Set up the values for the filter
          var filtervals = new Dictionary<string, List<string>>();
          filtervals.Add("BldgLevel", new List<string>() { floors[0] });
          var category_vals = categories.Where(v => v == "Walls" || v == "Stairs").ToList() ?? new List<string>();
          if (category_vals.Count() > 0)
          {
            filtervals.Add("Category", category_vals);
          }
          //Create a solid block (other option is "Wireframe")
          var fdef = new FilterBlockDefinition()
          {
            FilterBlockMode = Object3DRenderingMode.None,
            Title = "Solid Filter",
            SelectedValues = filtervals//Floor and Category
          };
          //Apply the block
          fd.FilterBlockDefinitions = new List<FilterBlockDefinition>() { fdef };
          //Add the filter definition to the layer
          bsl.UpdateFilter(fd);
          //Set it active. The ID is auto-generated
          bsl.SetActiveFilter(fd.ID);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
      // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
      // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
      // cref: ArcGIS.Core.CIM.Object3DRenderingMode
      #region Modify BuildingSceneLayer Filter Block
      {
        // Note: call within QueuedTask.Run()
        {
          var filterBlock = new FilterBlockDefinition();
          filterBlock.FilterBlockMode = Object3DRenderingMode.Wireframe;

          var selectedValues = new Dictionary<string, List<string>>();
          //We assume QueryAvailableFieldsAndValues() contains "Walls" and "Doors"
          //For 'Category'
          selectedValues["Category"] = new List<string>() { "Walls", "Doors" };
          filterBlock.SelectedValues = selectedValues;

          //Overwrite
          filter.FilterBlockDefinitions = new List<FilterBlockDefinition>() { filterBlock };
          bsl.UpdateFilter(filter);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.HasFilter
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.RemoveFilter
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.RemoveAllFilters
      // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
      #region Remove BuildingSceneLayer Filter
      {
        // Note: call within QueuedTask.Run()
        {
          //Note: Use HasFilter to check if a given filter id exists in the layer
          if (bsl.HasFilter(filter.ID))
            bsl.RemoveFilter(filter.ID);
          //Or remove all filters
          bsl.RemoveAllFilters();
        }
      }
      #endregion

      #region ProSnippet Group: FeatureSceneLayer Editing
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      #region Determine if a FeatureSceneLayer supports editing
      {
        featSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                           .OfType<FeatureSceneLayer>().FirstOrDefault();
        if (!featSceneLayer.HasAssociatedFeatureService ||
            !featSceneLayer.IsEditable)
        {
          //not supported - exit the function
        }
        //TODO continue editing here...
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.ShapeType
      #region Create a new Point feature in FeatureSceneLayer
      {
        //must support editing!
        if (!featSceneLayer.HasAssociatedFeatureService ||
            !featSceneLayer.IsEditable)
        {
          //not supported - exit the function
        }

        //Check geometry type...must be point in this example
        var editOp = new EditOperation()
        {
          Name = "Create new 3d point feature",
          SelectNewFeatures = true
        };

        var attributes = new Dictionary<string, object>();
        //mapPoint contains the new 3d point location
        attributes.Add("SHAPE", mapPoint);
        attributes.Add("TreeID", "1");
        editOp.Create(featSceneLayer, attributes);
        editOp.ExecuteAsync();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetSelection
      // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.IEnumerable{System.Int64})
      #region Delete all the selected features in FeatureSceneLayer
      {
        if (!featSceneLayer.HasAssociatedFeatureService ||
          !featSceneLayer.IsEditable)
          return;
        // Note: call within QueuedTask.Run()
        {
          var delOp = new EditOperation()
          {
            Name = "Delete selected features"
          };
          //Assuming we have a selection on the layer...
          delOp.Delete(featSceneLayer, featSceneLayer.GetSelection().GetObjectIDs());
          delOp.ExecuteAsync();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify
      #region Edit the attributes of a FeatureSceneLayer
      {
        //must support editing!
        if (!featSceneLayer.HasAssociatedFeatureService ||
            !featSceneLayer.IsEditable)
          return;

        // Note: call within QueuedTask.Run()
        {
          var editOp = new EditOperation()
          {
            Name = "Edit FeatureSceneLayer Attributes",
            SelectModifiedFeatures = true
          };
          //make an inspector
          var inspector = new Inspector();
          //get the attributes for the specified oid
          inspector.Load(featSceneLayer, oid);
          inspector["PermitNotes"] = "test";//modify
          editOp.Modify(inspector);
          editOp.Execute();//synchronous flavor
        }
      }
      #endregion

      #region ProSnippet Group: FeatureSceneLayer
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer
      #region Name of FeatureSceneLayer 
      {
        featSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                         .OfType<FeatureSceneLayer>().FirstOrDefault();
        var scenelayerName = featSceneLayer?.Name;
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.SceneLayerDataSourceType
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetDataSourceType
      #region Get the Data Source type
      {
        // Note: call within QueuedTask.Run()
        {
          var dataSourceType = featSceneLayer?.GetDataSourceType() ??
                                   SceneLayerDataSourceType.Unknown;
          if (dataSourceType == SceneLayerDataSourceType.SLPK)
          {
            //Uses SLPK - only cached attributes
          }
          else if (dataSourceType == SceneLayerDataSourceType.Service)
          {
            //Hosted service - can have live attributes - check HasAssociatedFeatureService
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetFeatureClass
      #region Get the Associated Feature class
      {
        // Note: call within QueuedTask.Run()
        {
          if (featSceneLayer.HasAssociatedFeatureService)
          {
            using (var fc = featSceneLayer.GetFeatureClass())
            {
              //TODO query underlying feature class
            }
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetFieldDescriptions
      // cref: ARCGIS.DESKTOP.MAPPING.FIELDDESCRIPTION.ISREADONLY
      #region Get Field Definitions
      {
        // Note: call within QueuedTask.Run()
        {
          //Get only the readonly fields
          var readOnlyFields = featSceneLayer.GetFieldDescriptions()
                                    .Where(fdesc => fdesc.IsReadOnly);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.SetDefinitionQuery
      #region Set a Definition Query
      {
        featSceneLayer.SetDefinitionQuery("Name = 'Ponderosa Pine'");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.MapView.SelectFeaturesEx
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load
      #region Select features via the MapView
      {
        //assume the geometry used in SelectFeaturesEx() is coming from a 
        //map tool...
        //SketchType = SketchGeometryType.Rectangle;
        //SketchOutputMode = SketchOutputMode.Screen;

        // Note: call within QueuedTask.Run()
        {
          var result = MapView.Active.SelectFeaturesEx(geometry);
          //Get scene layers with selections
          var scene_layers = result.ToDictionary<FeatureSceneLayer>();
          foreach (var kvp in scene_layers)
          {
            var scene_layer = kvp.Key as FeatureSceneLayer;
            var sel_oids = kvp.Value;
            //If there are attributes then get them
            if (scene_layer.HasAssociatedFeatureService)
            {
              var insp = new Inspector();
              foreach (var objectid in sel_oids)
              {
                insp.Load(scene_layer, objectid);
                //TODO something with retrieved attributes
              }
            }
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      #region Has Associated FeatureService
      {
        if (featSceneLayer.HasAssociatedFeatureService)
        {
          //Can Select and Search...possibly edit
        }
      }

      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.FeatureSceneLayerType
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.Search
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayerType
      #region Search Rows on the FeatureSceneLayer
      {
        if (!featSceneLayer.HasAssociatedFeatureService)
          return;//Search and Select not supported

        //Multipatch (Object3D) or point?
        //var is3dObject = ((ISceneLayerInfo)featSceneLayer).SceneServiceLayerType 
        //                                  == esriSceneServiceLayerType.Object3D;
        var is3dObject = featSceneLayer.FeatureSceneLayerType == FeatureSceneLayerType.Object3D;
        // Note: call within QueuedTask.Run()
        {
          var queryFilter = new QueryFilter
          {
            WhereClause = "Name = 'Ponderosa Pine'",
            SubFields = "*"
          };

          int rowCount = 0;
          //or select... var select = featSceneLayer.Select(queryFilter)
          using (RowCursor rowCursor = featSceneLayer.Search(queryFilter))
          {
            while (rowCursor.MoveNext())
            {
              using (var feature = rowCursor.Current as Feature)
              {
                oid = feature.GetObjectID();
                var shape = feature.GetShape();
                var attrib = feature["Name"];
                if (is3dObject)
                {
                  //shape is a multipatch
                }
                else
                {
                  //shape is a point
                }
                rowCount += 1;
              }
            }
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HideSelectedFeatures
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.ShowHiddenFeatures
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.SelectionCount
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.Select
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayerType
      #region Hide Selected features and Show Hidden features
      {
        if (featSceneLayer.HasAssociatedFeatureService)
          return;//Search and Select not supported

        // Note: call within QueuedTask.Run()
        {
          QueryFilter qf = new QueryFilter()
          {
            ObjectIDs = new List<long>() { 6069, 6070, 6071 },
            SubFields = "*"
          };
          featSceneLayer.Select(qf, SelectionCombinationMethod.New);

          featSceneLayer.HideSelectedFeatures();
          var selectionCount = featSceneLayer.SelectionCount;

          featSceneLayer.ShowHiddenFeatures();
          selectionCount = featSceneLayer.SelectionCount;
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer
      // cref: ArcGIS.Desktop.Mapping.MapView.SelectFeaturesEx
      // cref: ArcGIS.Desktop.Mapping.MapView.GetViewSize
      // cref: ArcGIS.Desktop.Mapping.Map.ClearSelection
      #region Use MapView Selection SelectFeaturesEx or GetFeaturesEx
      {
        var sname = featSceneLayer.Name;

        // Note: call within QueuedTask.Run()
        {
          //Select all features within the current map view
          var sz = MapView.Active.GetViewSize();

          var c_ll = new Coordinate2D(0, 0);
          var c_ur = new Coordinate2D(sz.Width, sz.Height);
          //Use screen coordinates for 3D selection on MapView
          var env = EnvelopeBuilderEx.CreateEnvelope(c_ll, c_ur);

          //HasAssociatedFeatureService does not matter for SelectFeaturesEx
          //or GetFeaturesEx
          var result = MapView.Active.SelectFeaturesEx(env);
          //var result = MapView.Active.GetFeaturesEx(env);

          //The list of object ids from SelectFeaturesEx
          var oids1 = result.ToDictionary().Where(kvp => kvp.Key.Name == sname).First().Value;
          //TODO - use the object ids

          MapView.Active.Map.ClearSelection();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetFeatureClass
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.Search
      // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition
      // cref: ArcGIS.Core.Data.FeatureClassDefinition.GetSpatialReference
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Project
      // cref: ArcGIS.Core.Data.SpatialQueryFilter
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.FilterGeometry
      // cref: ArcGIS.Core.Data.SpatialQueryFilter.SpatialRelationship
      // cref: ArcGIS.Core.Data.QueryFilter.SubFields
      // cref: ArcGIS.Core.Data.SpatialRelationship
      // cref: ArcGIS.Core.Data.Selection.Search
      // cref: ArcGIS.Core.Data.Selection.GetCount
      // cref: ArcGIS.Desktop.Mapping.MapView.GetViewSize
      // cref: ArcGIS.Desktop.Mapping.MapView.ClientToMap
      // cref: ArcGIS.Desktop.Mapping.Map.ClearSelection
      #region Use Select or Search with a Spatial Query
      // Note: call within QueuedTask.Run()
      {
        if (!featSceneLayer.HasAssociatedFeatureService)
          return;//no search or select

        //Select all features within the current map view
        var sz = MapView.Active.GetViewSize();
        var map_pt1 = MapView.Active.ClientToMap(new System.Windows.Point(0, sz.Height));
        var map_pt2 = MapView.Active.ClientToMap(new System.Windows.Point(sz.Width, 0));

        //Convert to an envelope
        var temp_env = EnvelopeBuilderEx.CreateEnvelope(map_pt1, map_pt2, MapView.Active.Map.SpatialReference);

        //Project if needed to the layer spatial ref
        SpatialReference sr = null;
        using (var fc = featSceneLayer.GetFeatureClass())
        using (var fdef = fc.GetDefinition())
          sr = fdef.GetSpatialReference();

        var env = GeometryEngine.Instance.Project(temp_env, sr) as Envelope;

        //Set up a query filter
        var sf = new SpatialQueryFilter()
        {
          FilterGeometry = env,
          SpatialRelationship = SpatialRelationship.Intersects,
          SubFields = "*"
        };

        //Select against the feature service
        var select = featSceneLayer.Select(sf);
        if (select.GetCount() > 0)
        {
          //enumerate over the selected features
          using (var rc = select.Search())
          {
            while (rc.MoveNext())
            {
              using (var feature = rc.Current as Feature)
              {
                oid = feature.GetObjectID();
                //etc.
              }
            }
          }
        }
        MapView.Active.Map.ClearSelection();
      }
      #endregion

      #region ProSnippet Group: PointCloudSceneLayer
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
      #region Name of PointCloudSceneLayer 
      {
        var scenelayerName = pointCloudSceneLayer?.Name;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetDataSourceType
      // cref: ArcGIS.Desktop.Mapping.SceneLayerDataSourceType
      #region Get Data Source type for PointCloudSceneLayer
      {
        SceneLayerDataSourceType dataSourceType = pointCloudSceneLayer.GetDataSourceType();
        if (dataSourceType == SceneLayerDataSourceType.Service)
        {
          //TODO...
        }
        else if (dataSourceType == SceneLayerDataSourceType.SLPK)
        {

        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailableClassCodesAndLabels
      #region Query all class codes and labels in the PointCloudSceneLayer
      {
        // Note: call within QueuedTask.Run()
        {
          Dictionary<int, string> classCodesAndLabels =
                          pointCloudSceneLayer.GetAvailableClassCodesAndLabels();
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailableClassCodesAndLabels
      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition
      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ReturnValues
      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ToCIM
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetFilters
      // cref: ArcGIS.Core.CIM.PointCloudReturnType
      #region Set a Filter for PointCloudSceneLayer
      {
        // Note: call within QueuedTask.Run()
        {
          //Retrieve the available classification codes
          var dict = pointCloudSceneLayer.GetAvailableClassCodesAndLabels();

          //Filter out low noise and unclassified (7 and 1 respectively)
          //consult https://pro.arcgis.com/en/pro-app/help/data/las-dataset/storing-lidar-data.htm
          var filterDef = new PointCloudFilterDefinition()
          {
            ClassCodes = dict.Keys.Where(c => c != 7 && c != 1).ToList(),
            ReturnValues = new List<PointCloudReturnType> {
                              PointCloudReturnType.FirstOfMany }
          };
          //apply the filter
          pointCloudSceneLayer.SetFilters(filterDef.ToCIM());
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ClassFlags
      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.FromCIM
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetFilters
      // cref: ArcGIS.Desktop.Mapping.ClassFlag
      // cref: ArcGIS.Desktop.Mapping.ClassFlagOption
      #region Update the ClassFlags for PointCloudSceneLayer
      {
        // Note: call within QueuedTask.Run()
        {
          var filters = pointCloudSceneLayer.GetFilters();
          PointCloudFilterDefinition fdef = null;
          if (filters.Count() == 0)
          {
            fdef = new PointCloudFilterDefinition()
            {
              //7 is "edge of flight line" - exclude
              ClassFlags = new List<ClassFlag> {
               new ClassFlag(7, ClassFlagOption.Exclude) }
            };
          }
          else
          {
            fdef = PointCloudFilterDefinition.FromCIM(filters);
            //keep any include or ignore class flags
            var keep = fdef.ClassFlags.Where(
                   cf => cf.ClassFlagOption != ClassFlagOption.Exclude).ToList();
            //7 is "edge of flight line" - exclude
            keep.Add(new ClassFlag(7, ClassFlagOption.Exclude));
            fdef.ClassFlags = keep;
          }
          //apply
          pointCloudSceneLayer.SetFilters(fdef.ToCIM());
        }
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.FromCIM
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetFilters
      // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ReturnValues
      // cref: ArcGIS.Core.CIM.PointCloudReturnType
      // cref: ArcGIS.Core.CIM.CIMPointCloudFilter
      // cref: ArcGIS.Core.CIM.CIMPointCloudReturnFilter
      // cref: ArcGIS.Core.CIM.CIMPointCloudValueFilter
      // cref: ArcGIS.Core.CIM.CIMPointCloudBitFieldFilter
      #region Get the filters for PointCloudSceneLayer
      {
        // Note: call within QueuedTask.Run()
        {
          IReadOnlyList<CIMPointCloudFilter> updatedFilter = pointCloudSceneLayer.GetFilters();
          foreach (var f in updatedFilter)
          {
            //There is either 0 or 1 of each
            if (f is CIMPointCloudReturnFilter returnFilter)
            {
              PointCloudFilterDefinition pcfl = PointCloudFilterDefinition.FromCIM(updatedFilter);
              List<PointCloudReturnType> updatedReturnValues = pcfl.ReturnValues;

            }
            if (f is CIMPointCloudValueFilter classCodesFilter)
            {
              // do something
            }

            if (f is CIMPointCloudBitFieldFilter classFlagsFilter)
            {
              // do something
            }
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.ClearFilters
      #region Clear filters in PointCloudSceneLayer
      {
        // Note: call within QueuedTask.Run()
        pointCloudSceneLayer.ClearFilters();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetRenderer
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.RendererType
      // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
      // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer
      #region Get PointCloudSceneLayer Renderer and RendererType
      {
        // Note: call within QueuedTask.Run()
        {
          CIMPointCloudRenderer cimGetPCLRenderer = pointCloudSceneLayer.GetRenderer();
          //Can be one of Unknown, Stretch, ClassBreaks, UniqueValue, RGB
          PointCloudRendererType pclRendererType = pointCloudSceneLayer.RendererType;
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
      // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
      #region Query PointCloudSceneLayer Renderer fields
      {
        // Note: call within QueuedTask.Run()
        {
          IReadOnlyList<string> flds = pointCloudSceneLayer.GetAvailablePointCloudRendererFields(
                                PointCloudRendererType.UniqueValueRenderer);
          var fldCount = flds.Count;
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
      // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.CreateRenderer
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetRenderer
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      // cref: ArcGIS.Core.CIM.CIMPointCloudStretchRenderer
      // cref: ArcGIS.Core.CIM.CIMPointCloudStretchRenderer.ColorRamp
      // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.ColorModulation
      // cref: ArcGIS.Core.CIM.CIMColorRamp
      // cref: ArcGIS.Core.CIM.CIMColorModulationInfo
      // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MinValue
      // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MaxValue
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHCOLORRAMPS
      #region Create and Set a Stretch Renderer
      {
        // Note: call within QueuedTask.Run()
        {
          var fields = pointCloudSceneLayer.GetAvailablePointCloudRendererFields(
                                  PointCloudRendererType.StretchRenderer);
          var stretchDef = new PointCloudRendererDefinition(
                                    PointCloudRendererType.StretchRenderer)
          {
            //Will be either ELEVATION or INTENSITY
            Field = fields[0]
          };
          //Create the CIM Renderer
          var stretchRenderer = pointCloudSceneLayer.CreateRenderer(stretchDef)
                                              as CIMPointCloudStretchRenderer;
          //Apply a color ramp
          var style = Project.Current.GetItems<StyleProjectItem>()
                                          .First(s => s.Name == "ArcGIS Colors");
          var colorRamp = style.SearchColorRamps("").First();
          stretchRenderer.ColorRamp = colorRamp.ColorRamp;
          //Apply modulation
          stretchRenderer.ColorModulation = new CIMColorModulationInfo()
          {
            MinValue = 0,
            MaxValue = 100
          };
          //apply the renderer
          pointCloudSceneLayer.SetRenderer(stretchRenderer);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.CreateRenderer
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetRenderer
      // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
      // cref: ArcGIS.Desktop.Mapping.PointCloudRendererDefinition.Field
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.LOOKUPITEM
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.GenerateColorsFromColorRamp
      // cref: ArcGIS.Core.CIM.CIMPointCloudClassBreaksRenderer
      // cref: ArcGIS.Core.CIM.CIMPointCloudClassBreaksRenderer.Breaks
      // cref: ArcGIS.Core.CIM.CIMColorClassBreak
      // cref: ArcGIS.Core.CIM.CIMColorClassBreak.UpperBound
      // cref: ArcGIS.Core.CIM.CIMColorClassBreak.Label
      // cref: ArcGIS.Core.CIM.CIMColorClassBreak.Color
      #region Create and Set a ClassBreaks Renderer
      {
        // Note: call within QueuedTask.Run()
        {
          var fields = pointCloudSceneLayer.GetAvailablePointCloudRendererFields(
                             PointCloudRendererType.ClassBreaksRenderer);
          var classBreakDef = new PointCloudRendererDefinition(
                                    PointCloudRendererType.ClassBreaksRenderer)
          {
            //ELEVATION or INTENSITY
            Field = fields[0]
          };
          //create the renderer
          var cbr = pointCloudSceneLayer.CreateRenderer(classBreakDef)
                                    as CIMPointCloudClassBreaksRenderer;
          //Set up a color scheme to use
          var style = Project.Current.GetItems<StyleProjectItem>()
                                     .First(s => s.Name == "ArcGIS Colors");
          var rampStyle = style.LookupItem(
            StyleItemType.ColorRamp, "Spectrum By Wavelength-Full Bright_Multi-hue_2")
                                                                      as ColorRampStyleItem;
          var colorScheme = rampStyle.ColorRamp;
          //Set up 6 manual class breaks
          var breaks = 6;
          var colors = ColorFactory.Instance.GenerateColorsFromColorRamp(
                                                      colorScheme, breaks);
          var classBreaks = new List<CIMColorClassBreak>();
          var min = cbr.Breaks[0].UpperBound;
          var max = cbr.Breaks[cbr.Breaks.Count() - 1].UpperBound;
          var step = (max - min) / (double)breaks;

          //add in the class breaks
          double upper = min;
          for (int b = 1; b <= breaks; b++)
          {
            double lower = upper;
            upper = b == breaks ? max : min + (b * step);
            var cb = new CIMColorClassBreak()
            {
              UpperBound = upper,
              Label = string.Format("{0:#0.0#} - {1:#0.0#}", lower, upper),
              Color = colors[b - 1]
            };
            classBreaks.Add(cb);
          }
          cbr.Breaks = classBreaks.ToArray();
          pointCloudSceneLayer.SetRenderer(cbr);
        }
      }
      #endregion

      #region ProSnippet Group: PointCloudSceneLayer Extended Properties
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
      // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
      // cref: ArcGIS.Core.CIM.CIMPointCloudLayer
      // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
      // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.ColorModulation
      // cref: ArcGIS.Core.CIM.CIMColorModulationInfo
      // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MinValue
      // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MaxValue
      #region Edit Color Modulation
      {
        // Note: call within QueuedTask.Run()
        {
          var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;
          //Get the ColorModulation off the renderer
          var modulation = def.Renderer.ColorModulation;
          if (modulation == null)
            modulation = new CIMColorModulationInfo();
          //Set the minimum and maximum intensity as needed
          modulation.MinValue = 0;
          modulation.MaxValue = 100.0;
          //apply back
          def.Renderer.ColorModulation = modulation;
          //Commit changes back to the CIM
          pointCloudSceneLayer.SetDefinition(def);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
      // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
      // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
      // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointSizeAlgorithm
      // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointShape
      // cref: ArcGIS.Core.CIM.PointCloudShapeType
      // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm
      // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm.UseRealWorldSymbolSizes
      // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm.Size
      #region Edit The Renderer to use Fixed Size
      {
        // Note: call within QueuedTask.Run()
        {
          var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;

          //Set the point shape and sizing on the renderer
          def.Renderer.PointShape = PointCloudShapeType.DiskShaded;
          var pointSize = new CIMPointCloudFixedSizeAlgorithm()
          {
            UseRealWorldSymbolSizes = false,
            Size = 8
          };
          def.Renderer.PointSizeAlgorithm = pointSize;
          //Commit changes back to the CIM
          pointCloudSceneLayer.SetDefinition(def);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
      // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
      // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
      // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointSizeAlgorithm
      // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointShape
      // cref: ArcGIS.Core.CIM.PointCloudShapeType
      // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm
      // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm.MinSize
      // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm.ScaleFactor
      #region Edit the Renderer to Scale Size
      {
        // Note: call within QueuedTask.Run()
        {
          var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;

          //Set the point shape and sizing on the renderer
          def.Renderer.PointShape = PointCloudShapeType.DiskFlat;//default
          var scaleSize = new CIMPointCloudSplatAlgorithm()
          {
            MinSize = 8,
            ScaleFactor = 1.0 //100%
          };
          def.Renderer.PointSizeAlgorithm = scaleSize;
          //Commit changes back to the CIM
          pointCloudSceneLayer.SetDefinition(def);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
      // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
      // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.PointsBudget
      // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.PointsPerInch
      #region Edit Density settings
      {
        // Note: call within QueuedTask.Run()
        {
          var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;
          //PointsBudget - corresponds to Display Limit on the UI
          // - the absolute maximum # of points to display
          def.PointsBudget = 1000000;

          //PointsPerInch - corresponds to Density Min --- Max on the UI
          // - the max number of points per display inch to renderer
          def.PointsPerInch = 15;
          //Commit changes back to the CIM
          pointCloudSceneLayer.SetDefinition(def);
        }
      }
      #endregion

    }
  }
}
