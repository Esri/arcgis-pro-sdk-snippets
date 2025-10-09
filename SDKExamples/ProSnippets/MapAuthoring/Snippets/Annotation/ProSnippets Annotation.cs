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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Core.Data.Mapping;

namespace ProSnippets.MapAuthoringSnippets.Annotation
{
  /// <summary>
  /// Provides methods for authoring and manipulating annotation features within a map.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with Annotations in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsMapAuthoring
  {
    /// <summary>
    /// Demonstrates various operations on annotation features within a map, including updating text, rotating or moving
    /// annotations, and retrieving graphic and outline geometries.
    /// </summary>
    /// <remarks>This method provides examples of how to interact with annotation layers in ArcGIS Pro. It
    /// includes operations such as modifying annotation text attributes, rotating annotation graphics, and obtaining
    /// the graphic and outline geometries of annotation features. Each operation requires a <see cref="QueuedTask"/>
    /// context to ensure thread safety when accessing and modifying GIS data.</remarks>
    public static void ProSnippetsAnnotation()
    {
      #region Variable initialization
      AnnotationLayer annotationLayer = MapView.Active.Map.Layers.OfType<AnnotationLayer>().FirstOrDefault();
      #endregion Variable initialization

      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify
      #region Update Annotation Text via attribute. 
      {
        var oid = 1;

        // See "Change Annotation Text Graphic" for an alternative if TEXTSTRING is missing from the schema

        // Note: QueuedTask is required to access the Inspector
        {
          // use the inspector methodology
          var insp = new Inspector();
          insp.Load(annotationLayer, oid);

          // make sure TextString attribute exists.
          //It is not guaranteed to be in the schema
          ArcGIS.Desktop.Editing.Attributes.Attribute att = insp.FirstOrDefault(a => a.FieldName == "TEXTSTRING");
          if (att != null)
          {
            insp["TEXTSTRING"] = "Hello World";

            //create and execute the edit operation
            EditOperation op = new EditOperation();
            op.Name = "Update annotation";
            op.Modify(insp);

            //OR using a Dictionary - again TEXTSTRING has to exist in the schema
            //Dictionary<string, object> newAtts = new Dictionary<string, object>();
            //newAtts.Add("TEXTSTRING", "hello world");
            //op.Modify(annotationLayer, oid, newAtts);

            op.Execute();
          }
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.GetGraphic()
      // cref: ArcGIS.Core.CIM.CIMTextGraphic
      // cref: ArcGIS.Core.CIM.CIMTextGraphicBase.Shape
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Centroid
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Rotate
      #region Rotate or Move the Annotation
      {
        var oid = 1; //ObjectID of the annotation feature to modify
                     // Note: QueuedTask is required to access the AnnotationFeature
        {
          //Don't use 'Shape'....Shape is the bounding box of the annotation text. This is NOT what you want...
          //
          //var insp = new Inspector();
          //insp.Load(annotationLayer, oid);
          //var shape = insp["SHAPE"] as Polygon;
          //...wrong shape...

          //Instead, we must get the TextGraphic from the anno feature.
          //The TextGraphic shape will be the anno baseline...
          QueryFilter qf = new QueryFilter()
          {
            WhereClause = "OBJECTID = 1"
          };

          //annotationLayer is ~your~ Annotation layer

          using var rowCursor = annotationLayer.Search(qf);
          if (rowCursor.MoveNext())
          {
            using (var annoFeature = rowCursor.Current as
            ArcGIS.Core.Data.Mapping.AnnotationFeature)
            {
              var graphic = annoFeature.GetGraphic();
              var textGraphic = graphic as CIMTextGraphic;
              var textLine = textGraphic.Shape as Polyline;
              // rotate the shape 90 degrees
              var origin = GeometryEngine.Instance.Centroid(textLine);
              Geometry rotatedPolyline = GeometryEngine.Instance.Rotate(textLine, origin, System.Math.PI / 2);
              //Move the line 5 "units" in the x and y direction
              //GeometryEngine.Instance.Move(textLine, 5, 5);

              EditOperation op = new EditOperation();
              op.Name = "Change annotation angle";
              op.Modify(annotationLayer, oid, rotatedPolyline);
              op.Execute();
            }
          }
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.GetGraphic()
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetTable
      #region Get the Annotation Text Graphic
      {

        // Note: QueuedTask is required to access the AnnotationFeature
        {
          using var table = annotationLayer.GetTable();
          using var rc = table.Search();
          rc.MoveNext();
          using var af = rc.Current as AnnotationFeature;
          var graphic = af.GetGraphic();
          var textGraphic = graphic as CIMTextGraphic;

          //Note: 
          //var outline_geom = af.GetGraphicOutline(); 
          //gets the anno text outline geometry...
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.GetGraphicOutline()
      #region Get the Outline Geometry for an Annotation
      {

        // Note: QueuedTask is required to access the AnnotationFeature
        {
          //get the first annotation feature...
          //...assuming at least one feature gets selected
          using var fc = annotationLayer.GetFeatureClass();
          using var rc = fc.Search();
          rc.MoveNext();
          using var af = rc.Current as AnnotationFeature;
          var outline_geom = af.GetGraphicOutline();
          //TODO - use the outline...

          //Note: 
          //var graphic = annoFeature.GetGraphic(); 
          //gets the CIMTextGraphic...
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetDrawingOutline(System.Int64, ArcGIS.Desktop.Mapping.MapView, ArcGIS.Desktop.Mapping.DrawingOutlineType)
      // cref: ArcGIS.Desktop.Mapping.AnnotationLayer.GetFeatureClass
      #region Get the Mask Geometry for an Annotation
      {
        var mv = MapView.Active;

        // Note: QueuedTask is required to access the AnnotationFeature
        {
          //get the first annotation feature...
          //...assuming at least one feature gets selected
          using var fc = annotationLayer.GetFeatureClass();
          using var rc = fc.Search();
          rc.MoveNext();
          using var row = rc.Current;
          var oid = row.GetObjectID();

          //Use DrawingOutlineType.BoundingEnvelope to retrieve a generalized
          //mask geometry or "Box". The mask will be in the same SpatRef as the map.
          //The mask will be constructed using the anno class reference scale
          var mask_geom = annotationLayer.GetDrawingOutline(oid, mv, DrawingOutlineType.Exact);
        }
      }
      #endregion
    }
  }

  #region Create Annotation Construction Tool
  public class AnnoConstructionTool : MapTool
  {
    //In your config.daml...set the categoryRefID
    //<tool id="..." categoryRefID="esri_editing_construction_annotation" caption="Create Anno" ...>
    //Sketch type Point or Line or BezierLine in the constructor...
    public AnnoConstructionTool()
    {
      IsSketchTool = true;
      UseSnapping = true;
      SketchType = SketchGeometryType.Point;
    }
    /// <summary>
    /// Callback when the sketch operation is complete.
    /// </summary>
    /// <param name="geometry">The geometry resulting from the sketch operation. Must not be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the feature
    /// creation  operation succeeds; otherwise, <see langword="false"/>.</returns>
    protected async override Task<bool> OnSketchCompleteAsync(ArcGIS.Core.Geometry.Geometry geometry)
    {
      if (CurrentTemplate == null || geometry == null)
        return false;

      // Create an edit operation
      var createOperation = new EditOperation();
      createOperation.Name = string.Format("Create {0}", CurrentTemplate.Layer.Name);
      createOperation.SelectNewFeatures = true;

      // update the geometry point into a 2 point line
      //annotation needs at minimum a 2 point line for the text to be placed
      double tol = 0.01;
      var polyline = await CreatePolylineFromPointAsync((MapPoint)geometry, tol);

      // Queue feature creation
      createOperation.Create(CurrentTemplate, polyline);

      // Execute the operation
      return await createOperation.ExecuteAsync();
    }
    /// <summary>
    /// Creates a polyline starting from the specified map point, with an additional point offset by the given tolerance.
    /// </summary>
    /// <param name="pt">The starting <see cref="MapPoint"/> for the polyline.</param>
    /// <param name="tolerance">The distance, in the same spatial reference units as <paramref name="pt"/>, used to calculate the offset for the
    /// second point.</param>
    /// <returns>A <see cref="Task{Polyline}"/> representing the asynchronous operation. The result contains the constructed
    /// polyline.</returns>
    public Task<Polyline> CreatePolylineFromPointAsync(MapPoint pt, double tolerance)
    {
      return QueuedTask.Run(() =>
      {
        // create a polyline from a starting point
        //use a tolerance to construct the second point
        MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(pt.X + tolerance, pt.Y, pt.SpatialReference);
        return PolylineBuilderEx.CreatePolyline(new List<MapPoint>() { pt, pt2 });
      });
    }
  }
  #endregion
}
