/*

   Copyright 2025 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Editing.Attributes;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippetsFeatureSceneLayer
  {
    #region ProSnippet Group: FeatureSceneLayer
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer
    #region Name of FeatureSceneLayer 
    /// <summary>
    /// Retrieves the name of the first <see cref="FeatureSceneLayer"/> in the active map, if one exists.
    /// </summary>
    public static void NameOfFeatureSceneLayer()
    {
      var featSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                       .OfType<FeatureSceneLayer>().FirstOrDefault();
      var scenelayerName = featSceneLayer?.Name;
    }

      #endregion


      // cref: ArcGIS.Desktop.Mapping.SceneLayerDataSourceType
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetDataSourceType
      #region Get the Data Source type
    /// <summary>
    /// Determines the data source type of the specified <see cref="FeatureSceneLayer"/>.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"/> whose data source type is to be determined.  This parameter cannot be null.</param>
    public static void GetDataSourceType(FeatureSceneLayer featSceneLayer)
      {
       QueuedTask.Run( () => {
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
       });
    }
    #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetFeatureClass
      #region Get the Associated Feature class
    /// <summary>
    /// Retrieves the associated feature class of the specified <see cref="FeatureSceneLayer"/> if it has an associated
    /// feature service.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"/> from which to retrieve the associated feature class.</param>
    public static void GetFeatureClassOfFeatureSceneLayer(FeatureSceneLayer featSceneLayer)
     {
      QueuedTask.Run(() => {
        if (featSceneLayer.HasAssociatedFeatureService)
        {
          using (var fc = featSceneLayer.GetFeatureClass())
          {
            //TODO query underlying feature class
          }
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetFieldDescriptions
    // cref: ARCGIS.DESKTOP.MAPPING.FIELDDESCRIPTION.ISREADONLY
    #region Get Field Definitions
    /// <summary>
    /// Retrieves the read-only field definitions from the specified <see cref="FeatureSceneLayer"/>.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"/> from which to retrieve the field definitions.</param>
      public async static void GetFieldDefinitionsAsync(FeatureSceneLayer featSceneLayer)
    {
      await QueuedTask.Run(() =>
      {
        //Get only the readonly fields
        var readOnlyFields = featSceneLayer.GetFieldDescriptions()
                                    .Where(fdesc => fdesc.IsReadOnly);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.SetDefinitionQuery
    #region Set a Definition Query
    /// <summary>
    /// Sets a definition query on the specified <see cref="FeatureSceneLayer"/> to filter its displayed features.
    /// </summary>
    /// /// <param name="featureSceneLayer">The <see cref="FeatureSceneLayer"/> on which to apply the definition query.  This parameter cannot be <see
    /// langword="null"/>.</param>
    public static void SetDefinitionQuery(FeatureSceneLayer featureSceneLayer)
    {
      featureSceneLayer.SetDefinitionQuery("Name = 'Ponderosa Pine'");
    }
    #endregion



    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
    // cref: ArcGIS.Desktop.Mapping.MapView.SelectFeaturesEx
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load
    #region Select features via the MapView
    /// <summary>
    /// Selects features in the active map view based on the specified geometry.
    /// </summary>
    /// <param name="geometry">The geometry used to define the selection area. This is typically provided by a map tool.</param>
    public async static void SelectFeaturesFromMapView(Geometry geometry)
    {
      //assume the geometry used in SelectFeaturesEx() is coming from a 
      //map tool...
      //SketchType = SketchGeometryType.Rectangle;
      //SketchOutputMode = SketchOutputMode.Screen;
      await QueuedTask.Run(() =>
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
            foreach (var oid in sel_oids)
            {
              insp.Load(scene_layer, oid);
              //TODO something with retrieved attributes
            }
          }
        }
      });
    }
    #endregion
    //var scenelayerName = featSceneLayer?.Name;
    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
    #region Has Associated FeatureService
    /// <summary>
    /// Checks whether the specified <see cref="FeatureSceneLayer"/> has an associated feature service.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"/> to check. Cannot be <c>null</c>.</param>
    public static void CheckFeatureSceneLayerService(FeatureSceneLayer featSceneLayer)
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
    /// <summary>
    /// Searches for rows in the specified <see cref="FeatureSceneLayer"/> based on a predefined query.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"/> to search. The layer must have an associated feature service; otherwise, the
    /// method will return without performing any operation.</param>
    public static async void SearchRowsOnFeatureSceneLayer(FeatureSceneLayer featSceneLayer)
    {
      if (!featSceneLayer.HasAssociatedFeatureService)
        return;//Search and Select not supported

      //Multipatch (Object3D) or point?
      //var is3dObject = ((ISceneLayerInfo)featSceneLayer).SceneServiceLayerType 
      //                                  == esriSceneServiceLayerType.Object3D;
      var is3dObject = featSceneLayer.FeatureSceneLayerType == FeatureSceneLayerType.Object3D;
      await QueuedTask.Run(() =>
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
              var oid = feature.GetObjectID();
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
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HideSelectedFeatures
    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.ShowHiddenFeatures
    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.SelectionCount
    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.Select
    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayerType
    #region Hide Selected features and Show Hidden features
    /// <summary>
    /// Hides the features currently selected in the specified <see cref="FeatureSceneLayer"/>  and then restores the
    /// visibility of any previously hidden features.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"/> on which the operation is performed.  This layer must not have an associated
    /// feature service, as search and selection are not supported in that case.</param>
    public static async void HideAndShowSelectedFeatures(FeatureSceneLayer featSceneLayer)
    {
      if (featSceneLayer.HasAssociatedFeatureService)
        return;//Search and Select not supported

      await QueuedTask.Run(() =>
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
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer
    // cref: ArcGIS.Desktop.Mapping.MapView.SelectFeaturesEx
    // cref: ArcGIS.Desktop.Mapping.MapView.GetViewSize
    // cref: ArcGIS.Desktop.Mapping.Map.ClearSelection
    #region Use MapView Selection SelectFeaturesEx or GetFeaturesEx
    /// <summary>
    /// Selects all features within the current map view for the specified <see cref="FeatureSceneLayer"/>.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"/> whose features will be selected within the current map view.</param>
    public async static void UseMapViewSelection(FeatureSceneLayer featSceneLayer)
    {
      var sname = featSceneLayer.Name;

      await QueuedTask.Run(() =>
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
      });
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
    /// <summary>
    /// Performs a spatial query on the specified <see cref="FeatureSceneLayer"> to select features within the current
    /// map view's extent.
    /// </summary>
    /// <param name="featSceneLayer">The <see cref="FeatureSceneLayer"> to query. The layer must have an associated feature service to support
    /// selection or search operations.</param>
      public static async void UseSelectOrSearchWithSpatialQuery(FeatureSceneLayer featSceneLayer)
      {
          await QueuedTask.Run(() =>
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
                    var oid = feature.GetObjectID();
                    //etc.
                  }
                }
              }
            }
            MapView.Active.Map.ClearSelection();
          });
        }
      #endregion
    }
  }
