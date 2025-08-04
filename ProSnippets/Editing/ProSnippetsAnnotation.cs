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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProSnippetsEditing
{
  public class ProSnippetsAnnotation : MapTool
  {
    #region ProSnippet Group: Annotation
    #endregion

    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ONSKETCHCOMPLETEASYNC
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
    // cref: ArcGIS.Desktop.Editing.AnnotationProperties
    // cref: ArcGIS.Core.CIM.HorizontalAlignment
    #region Annotation Construction Tool

    //In your config.daml...set the categoryRefID
    //<tool id="..." categoryRefID="esri_editing_construction_annotation" caption="Create Anno" ...>

    //Sketch type Point or Line or BezierLine in the constructor...
    //internal class AnnoConstructionTool : MapTool  {
    //  public AnnoConstructionTool()  {
    //    IsSketchTool = true;
    //    UseSnapping = true;
    //    SketchType = SketchGeometryType.Point;
    //

    protected async override Task<bool> OnSketchCompleteAsync(Geometry geometry)
    {
      if (CurrentTemplate == null || geometry == null)
        return false;

      // Create an edit operation
      var createOperation = new EditOperation();
      createOperation.Name = string.Format("Create {0}", CurrentTemplate.Layer.Name);
      createOperation.SelectNewFeatures = true;

      var insp = CurrentTemplate.Inspector;
      var result = await QueuedTask.Run(() =>
      {
        // get the annotation properties class
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        // set custom annotation properties
        annoProperties.TextString = "my custom text";
        annoProperties.Color = ColorFactory.Instance.RedRGB;
        annoProperties.FontSize = 24;
        annoProperties.FontName = "Arial";
        annoProperties.HorizontalAlignment = ArcGIS.Core.CIM.HorizontalAlignment.Right;
        annoProperties.Shape = geometry;
        // assign annotation properties back to the inspector
        insp.SetAnnotationProperties(annoProperties);

        // Queue feature creation
        createOperation.Create(CurrentTemplate.Layer, insp);

        if (!createOperation.IsEmpty)
        {
          // Execute the operation
          return createOperation.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
        else
          return false;
      });
      return result;
    }

    #endregion

    public static void StartEditAnnotationTool()
    {
      // cref: ARCGIS.DESKTOP.FRAMEWORK.FRAMEWORKAPPLICATION.GETPLUGINWRAPPER
      // cref: ARCGIS.DESKTOP.FRAMEWORK.IPLUGINWRAPPER
      #region Programmatically start Edit Annotation

      var plugin = FrameworkApplication.GetPlugInWrapper("esri_editing_EditVerticesText");
      if (plugin.Enabled)
        ((ICommand)plugin).Execute(null);
      #endregion
    }

    //Using Inspector...
    internal async void UpdateAnnotation()
    {
      BasicFeatureLayer annoLayer = MapView.Active.Map.GetLayersAsFlattenedList().First() as BasicFeatureLayer;
      var oid = 1;
      MapPoint pnt = null;

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.TextString
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Programmatically Create an Annotation Feature

      await QueuedTask.Run(() =>
      {
        // annoLayer is ~your~ Annotation layer...
        // pnt is ~your~ Annotation geometry ...
        var op = new EditOperation();
        // Use the inspector
        var insp = new Inspector();
        insp.LoadSchema(annoLayer);
        // get the annotation properties from the inspector
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        // change the annotation text 
        annoProperties.TextString = DateTime.Now.ToLongTimeString();
        // change font color to green
        annoProperties.Color = ColorFactory.Instance.GreenRGB;
        // change the horizontal alignment
        annoProperties.HorizontalAlignment = HorizontalAlignment.Center;
        annoProperties.Shape = pnt;
        // set the annotation properties back on the inspector
        insp.SetAnnotationProperties(annoProperties);
        // create the annotation
        op.Create(annoLayer, insp);
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.TextString
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Update Annotation Text 

      await QueuedTask.Run(() =>
      {
        //annoLayer is ~your~ Annotation layer...

        // use the inspector methodology
        //at 2.x - var insp = new Inspector(true);
        var insp = new Inspector();
        insp.Load(annoLayer, oid);

        // get the annotation properties
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        // set the attribute
        annoProperties.TextString = "Hello World";
        // assign the annotation proeprties back to the inspector
        insp.SetAnnotationProperties(annoProperties);

        //create and execute the edit operation
        EditOperation op = new EditOperation();
        op.Name = "Update annotation";
        op.Modify(insp);
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.Shape
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Modify Annotation Shape

      await QueuedTask.Run(() =>
      {
        //Don't use 'Shape'....Shape is the bounding box of the annotation text. This is NOT what you want...
        //
        //var insp = new Inspector();
        //insp.Load(annoLayer, oid);
        //var shape = insp["SHAPE"] as Polygon;
        //...wrong shape...

        //Instead, we must use the AnnotationProperties

        //annoLayer is ~your~ Annotation layer
        //at 2.x - var insp = new Inspector(true);
        var insp = new Inspector();
        insp.Load(annoLayer, oid);

        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        var shape = annoProperties.Shape;
        if (shape.GeometryType != GeometryType.GeometryBag)
        {
          var newGeometry = GeometryEngine.Instance.Move(shape, 10, 10);
          annoProperties.Shape = newGeometry;
          insp.SetAnnotationProperties(annoProperties);

          EditOperation op = new EditOperation();
          op.Name = "Change annotation angle";
          op.Modify(insp);
          if (!op.IsEmpty)
          {
            var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          }
        }
      });

      #endregion

      // cref: ArcGIS.Core.CIM.HorizontalAlignment
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.LoadFromTextGraphic(ArcGIS.Core.CIM.CIMTextGraphic)
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.TextGraphic
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Modify Annotation Text Graphic

      await QueuedTask.Run(() =>
      {

        var selection = annoLayer.GetSelection();
        if (selection.GetCount() == 0)
          return;

        // use the first selelcted feature 
        //at 2.x - var insp = new Inspector(true);
        var insp = new Inspector();
        insp.Load(annoLayer, selection.GetObjectIDs().FirstOrDefault());

        // getAnnoProperties should return null if not an annotation feature
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        // get the textGraphic
        CIMTextGraphic textGraphic = annoProperties.TextGraphic;

        // change text
        textGraphic.Text = "Hello world";

        // set x,y offset via the symbol
        var symbol = textGraphic.Symbol.Symbol;
        var textSymbol = symbol as CIMTextSymbol;
        textSymbol.OffsetX = 2;
        textSymbol.OffsetY = 3;

        textSymbol.HorizontalAlignment = HorizontalAlignment.Center;

        // load the updated textGraphic
        annoProperties.LoadFromTextGraphic(textGraphic);
        // assign the annotation properties back 
        insp.SetAnnotationProperties(annoProperties);

        EditOperation op = new EditOperation();
        op.Name = "modify symbol";
        op.Modify(insp);
        if (!op.IsEmpty)
        {
          bool result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });

      #endregion
    }


  }
}
